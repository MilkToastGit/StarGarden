using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateAndSetRenderTexture : MonoBehaviour
{
    public RawImage rawImage;
    private RenderTexture texture;

    private void Awake()
    {
        texture = new RenderTexture(Screen.width, Screen.height, 0);
        if (TryGetComponent(out Camera cam))
        {
            if (cam.targetTexture != null)
                cam.targetTexture.Release();
            cam.targetTexture = texture;
        }

        if (rawImage)
            rawImage.texture = texture;
    }
}
