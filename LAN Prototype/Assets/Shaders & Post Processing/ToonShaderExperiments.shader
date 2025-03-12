Shader"Tuna/Toon"
{
    Properties
    {
        _Color("Color", Color) = (0.5, 0.65, 1, 1)
        _MainTex("Main Texture", 2D) = "white" {}

        [HDR]
        _AmbientColor("Ambient Color", Color) = (0.2,0.2,0.2,1)

        [HDR]
        _SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
        _Glossiness("Glossiness", Float) = 32

        [HDR]
        _RimColor("Rim Color", Color) = (1,1,1,1)
        _RimAmount("Rim Amount", Range(0, 1)) = 0.716
        _RimThreshold("Rim Threshold", Range(0,1)) = 0.1

        _LightRadius("Light Radius", Float) = 5.0
    }
    SubShader
    {
        Pass
        {
            Tags { "LightMode" = "ForwardBase" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #pragma multi_compile_shadowcaster
            
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormal : NORMAL;
                float3 viewDir : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
                            SHADOW_COORDS(3)
            };

            sampler2D _MainTex;
            float4 _Color, _AmbientColor, _SpecularColor, _RimColor;
            float _Glossiness, _RimAmount, _RimThreshold;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = WorldSpaceViewDir(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                TRANSFER_SHADOW(o)

                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float3 normal = normalize(i.worldNormal);
                float3 viewDir = normalize(i.viewDir);

                            // Fix Light Direction
                float3 lightDir;
                if (_WorldSpaceLightPos0.w == 0) // Directional Light
                    lightDir = normalize(_WorldSpaceLightPos0.xyz);
                else // Point or Spot Light
                    lightDir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos);

                            // Fix Light Intensity Calculation
                float NdotL = max(dot(lightDir, normal), 0);
                float attenuation = SHADOW_ATTENUATION(i);
                if (_WorldSpaceLightPos0.w != 0)
                {
                    float distance = length(_WorldSpaceLightPos0.xyz - i.worldPos);
                    attenuation *= 1.0 / (1.0 + 0.1 * distance + 0.01 * (distance * distance));
                }
                float lightIntensity = NdotL * attenuation;
                float4 light = lightIntensity * _LightColor0;

                            // Fix Specular Highlights
                float3 halfVector = normalize(lightDir + viewDir);
                float NdotH = max(dot(normal, halfVector), 0);
                float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
                float specular = smoothstep(0.005, 0.01, specularIntensity) * _SpecularColor;

                            // Rim Lighting
                float rimFactor = 1 - dot(viewDir, normal);
                float rim = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimFactor * pow(NdotL, _RimThreshold));

                            // Texture Sample
                float4 sample = tex2D(_MainTex, i.uv);
                return _Color * sample * (_AmbientColor + light + specular + rim);
            }
            ENDCG
        }

        // ForwardAdd Pass for Multiple Lights
        //Pass
        //{
        //    Tags { "LightMode" = "ForwardAdd" }
        //    Blend One One// Additive blending for multiple lights

        //    CGPROGRAM
            
        //    #pragma vertex vert
        //    #pragma fragment frag
        //    #pragma multi_compile_fwdadd
        //    #pragma multi_compile_shadowcaster

        //    #include "UnityCG.cginc"
        //    #include "Lighting.cginc"
        //    #include "AutoLight.cginc"

        //    struct appdata
        //    {
        //        float4 vertex : POSITION;
        //        float4 uv : TEXCOORD0;
        //        float3 normal : NORMAL;
        //    };

        //    struct v2f
        //    {
        //        float4 pos : SV_POSITION;
        //        float2 uv : TEXCOORD0;
        //        float3 worldNormal : NORMAL;
        //        float3 viewDir : TEXCOORD1;
        //        float3 worldPos : TEXCOORD2;
        //        LIGHTING_COORDS(3, 4)
        //        SHADOW_COORDS(3)
        //    };

        //    v2f vert(appdata v)
        //    {
        //        v2f o;
        //        o.pos = UnityObjectToClipPos(v.vertex);
        //        o.uv = v.uv;
        //        o.worldNormal = UnityObjectToWorldNormal(v.normal);
        //        o.viewDir = WorldSpaceViewDir(v.vertex);
        //        o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
    
        //        TRANSFER_VERTEX_TO_FRAGMENT(o); // <-- Ensures light attenuation is set
        //        return o;
        //    }

        //    float4 _SpecularColor;
        //    float _Glossiness;
        //    float _LightRadius;

        //    float4 frag(v2f i) : SV_Target
        //    {
        //        float3 normal = normalize(i.worldNormal);
        //        float3 viewDir = normalize(i.viewDir);

        //        // Calculate light direction and distance
        //        float3 lightDir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos);
        //        float distance = length(_WorldSpaceLightPos0.xyz - i.worldPos);

        //        // Custom falloff for smoother light fading
        //        float falloff = saturate(1.0 - (distance / _LightRadius));

        //        // Use smoothstep for a natural light fade
        //        float attenuation = smoothstep(0.0, 0.1, LIGHT_ATTENUATION(i)) * falloff;
    
        //        float NdotL = max(dot(normal, lightDir), 0);
        //        float lightIntensity = NdotL * attenuation;
        //        float4 light = lightIntensity * _LightColor0;

        //        // Specular Lighting
        //        float3 halfVector = normalize(lightDir + viewDir);
        //        float NdotH = max(dot(normal, halfVector), 0);
        //        float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
        //        float specular = smoothstep(0.005, 0.01, specularIntensity) * _SpecularColor;

<<<<<<< Updated upstream
				float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
				float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
				float4 specular = specularIntensitySmooth * _SpecularColor;

				float4 rimDot = 1 - dot(viewDir, normal);
				float rimIntensity = rimDot * pow(NdotL, _RimThreshold);
				float outlineIntensity = rimDot * NdotL;
				rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
				outlineIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimDot);
				float4 rim = rimIntensity * _RimColor;
				float4 outline = outlineIntensity * _OutlineColor;

				float4 sample = tex2D(_MainTex, i.uv);

				return _Color * sample * (_AmbientColor + light + specular + rim + outline);
			}
			ENDCG
		}
		Pass
		{
			Tags
			{
				"LightMode" = "ForwardAdd"
				//"PassFlags" = "OnlyDirectional"
			}
			Blend One One
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			struct appdata
			{
				float4 vertex : POSITION;				
				float4 uv : TEXCOORD0;
				float3 normal : NORMAL;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 worldNormal : NORMAL;
				float3 viewDir : TEXCOORD1;

				SHADOW_COORDS(2)

				UNITY_VERTEX_OUTPUT_STEREO
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;

				UNITY_SETUP_INSTANCE_ID(v); //Insert
				UNITY_INITIALIZE_OUTPUT(v2f, o); //Insert
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //Insert

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.viewDir = WorldSpaceViewDir(v.vertex);

				TRANSFER_SHADOW(o)

				return o;
			}
			
			float4 _Color;
			float4 _AmbientColor;
			float _Glossiness;
			float4 _SpecularColor;
			float4 _RimColor;
			float _RimAmount;
			float _RimThreshold;
			float4 _OutlineColor;
			float _OutlineSize;

			//UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex); //Insert

			float4 frag (v2f i) : SV_Target
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i); //Insert
    
				//float4 col = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.uv); //Insert


				float3 normal = normalize(i.worldNormal);
				float NdotL = dot(_WorldSpaceLightPos0, normal);

				float shadow = SHADOW_ATTENUATION(i);

				float lightIntensity = smoothstep(0, 0.01, NdotL * shadow);
				float4 light = lightIntensity * _LightColor0;

				float3 viewDir = normalize(i.viewDir);

				float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
				float NdotH = dot(normal, halfVector);

				float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
				float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
				float4 specular = specularIntensitySmooth * _SpecularColor;

				float4 rimDot = 1 - dot(viewDir, normal);
				float rimIntensity = rimDot * pow(NdotL, _RimThreshold);
				float outlineIntensity = rimDot * NdotL;
				rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
				outlineIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimDot);
				float4 rim = rimIntensity * _RimColor;
				float4 outline = outlineIntensity * _OutlineColor;

				float4 sample = tex2D(_MainTex, i.uv);

				return _Color * sample * (_AmbientColor + light + specular + rim + outline);
			}
			ENDCG
		}
		UsePass "Standard/SHADOWCASTER"
	}
=======
        //        return light + specular;
        //    }
        //    ENDCG
        //}
    }
>>>>>>> Stashed changes
}