using UnityEditor;
using UnityEngine;
using System.IO;

public static class LogoRenderCapture
{
    [MenuItem("Obsidian Protocol/Logo/Capture 1920x1080")]
    public static void Capture()
    {
        Camera cam = Camera.main;

        if (cam == null)
        {
            Debug.LogError("LOGO CAPTURE: Main Camera not found.");
            return;
        }

        string folder = "Assets/Art/logo/Renders";
        Directory.CreateDirectory(folder);

        RenderTexture rt = new RenderTexture(1920, 1080, 24);
        Texture2D image = new Texture2D(1920, 1080, TextureFormat.RGB24, false);

        RenderTexture previous = cam.targetTexture;
        cam.targetTexture = rt;
        cam.Render();

        RenderTexture.active = rt;
        image.ReadPixels(new Rect(0, 0, 1920, 1080), 0, 0);
        image.Apply();

        cam.targetTexture = previous;
        RenderTexture.active = null;

        File.WriteAllBytes(
            folder + "/ObsidianProtocol_Logo_1920x1080.png",
            image.EncodeToPNG()
        );

        Object.DestroyImmediate(image);
        rt.Release();
        Object.DestroyImmediate(rt);

        AssetDatabase.Refresh();
        Debug.Log("LOGO CAPTURE COMPLETE: " + folder);
    }
}
