﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialBlurImageEffect : MonoBehaviour
{
    public float blurSize = 0.1f;
    public Vector2 blurCenterPos = new Vector2(0.5f, 0.5f);
    [Range(1, 48)]
    public int samples;
    public Material radialBlurMaterial = null;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (blurSize > 0.0f)
        {
            radialBlurMaterial.SetInt("_Samples", samples);
            radialBlurMaterial.SetFloat("_BlurSize", blurSize);
            radialBlurMaterial.SetVector("_BlurCenterPos", blurCenterPos);
            Graphics.Blit(source, destination, radialBlurMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
