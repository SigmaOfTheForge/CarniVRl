Shader"Tuna/Toon2"
{
    Properties
    {
        _Color("Color", Color) = (0.5, 0.65, 1, 1)
        _MainTex("Main Texture", 2D) = "white" {}

        [HDR]
        _AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)

        [HDR]
        _SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
        _Glossiness("Glossiness", Float) = 32

        [HDR]
        _RimColor("Rim Color", Color) = (1,1,1,1)
        _RimAmount("Rim Amount", Range(0, 1)) = 0.716
        _RimThreshold("Rim Threshold", Range(0,1)) = 0.1

        [HDR]
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineSize("Outline Size", Range(0, 1)) = 0.7
    }
    SubShader
    {
        Pass
        {
            Tags
            {
                "RenderType" = "Opaque"
                "LightMode" = "ForwardBase"
            }

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
                float3 worldPos : TEXCOORD2;

                            SHADOW_COORDS(3)

                            UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            v2f vert(appdata v)
            {
                v2f o;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = WorldSpaceViewDir(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

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

            float4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

                float3 normal = normalize(i.worldNormal);
                float3 viewDir = normalize(i.viewDir);

                            // Correct Light Direction Calculation
                float3 lightDir;
                if (_WorldSpaceLightPos0.w == 0) // Directional Light
                {
                    lightDir = normalize(_WorldSpaceLightPos0.xyz);
                }
                else // Point or Spot Light
                {
                    lightDir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos);
                }

                float NdotL = dot(lightDir, normal);
                
                            // Correct Attenuation
                float attenuation = LIGHT_ATTENUATION(i);
                float lightIntensity = smoothstep(0, 0.01, NdotL * attenuation);
                float4 light = lightIntensity * _LightColor0;

                            // Correct Specular Calculation
                float3 halfVector = normalize(lightDir + viewDir);
                float NdotH = dot(normal, halfVector);
                float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
                float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
                float4 specular = specularIntensitySmooth * _SpecularColor;

                            // Rim Lighting
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
        UsePass"Standard/SHADOWCASTER"
    }
}