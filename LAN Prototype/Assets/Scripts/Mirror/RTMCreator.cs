using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTMCreator : MonoBehaviour
{
    public RenderTexture rt;
    public Camera cam;

    void Start()
    {
        Renderer rend = GetComponent<Renderer>();

        rt = new RenderTexture(1080, 1920, 16, RenderTextureFormat.Default);
        rt.Create();
        cam.targetTexture = rt;

        //rend.material.mainTexture = rt;

        //rend.material.color = new Vector4(0, 46, 96, 55);

        rend.material.EnableKeyword("_EMISSION");
        rend.material.SetTexture("_EmissionMap", rt);
        rend.material.SetColor("_EmissionColor", new Color(1, 1, 1, 1));
    }
}
