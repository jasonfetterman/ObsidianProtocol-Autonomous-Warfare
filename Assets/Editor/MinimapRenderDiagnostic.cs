using UnityEditor;
using UnityEngine;
using System.IO;

public static class MinimapRenderDiagnostic
{
    [MenuItem("Obsidian Protocol/Battlefield/Test Minimap Camera Render")]
    public static void Test()
    {
        var camObject = GameObject.Find("Minimap_Camera");
        var terrainObject = GameObject.Find("MAP001_BLACKSPIRE_EXPANSE_TERRAIN");

        if (!camObject || !terrainObject)
        {
            Debug.LogError("[MINIMAP] Camera or terrain not found.");
            return;
        }

        var cam = camObject.GetComponent<Camera>();
        var renderer = terrainObject.GetComponent<Renderer>();

        if (!cam || !renderer)
        {
            Debug.LogError("[MINIMAP] Camera or terrain Renderer missing.");
            return;
        }

        Bounds bounds = renderer.bounds;
        Vector3 viewport = cam.WorldToViewportPoint(bounds.center);

        Debug.Log("[MINIMAP] Camera position: " + cam.transform.position);
        Debug.Log("[MINIMAP] Camera rotation: " + cam.transform.eulerAngles);
        Debug.Log("[MINIMAP] Orthographic size: " + cam.orthographicSize);
        Debug.Log("[MINIMAP] Terrain bounds center: " + bounds.center);
        Debug.Log("[MINIMAP] Terrain bounds size: " + bounds.size);
        Debug.Log("[MINIMAP] Terrain center viewport: " + viewport);
        Debug.Log("[MINIMAP] Terrain center visible: " +
            (viewport.z > 0 && viewport.x >= 0 && viewport.x <= 1 &&
             viewport.y >= 0 && viewport.y <= 1));
        Debug.Log("[MINIMAP] Renderer enabled: " + renderer.enabled);
        Debug.Log("[MINIMAP] Terrain active: " + terrainObject.activeInHierarchy);

        foreach (var material in renderer.sharedMaterials)
        {
            if (material && material.shader)
                Debug.Log("[MINIMAP] Material: " + material.name +
                    " | Shader: " + material.shader.name +
                    " | Supported: " + material.shader.isSupported);
        }

        var oldActive = RenderTexture.active;
        var oldTarget = cam.targetTexture;
        var test = new RenderTexture(512, 512, 24, RenderTextureFormat.ARGB32);

        try
        {
            cam.targetTexture = test;
            cam.Render();
            RenderTexture.active = test;

            var image = new Texture2D(512, 512, TextureFormat.RGB24, false);
            image.ReadPixels(new Rect(0, 0, 512, 512), 0, 0);
            image.Apply();

            var path = Path.GetFullPath("MinimapCameraTest.png");
            File.WriteAllBytes(path, image.EncodeToPNG());
            Object.DestroyImmediate(image);

            Debug.Log("[MINIMAP] Capture saved: " + path);
        }
        finally
        {
            cam.targetTexture = oldTarget;
            RenderTexture.active = oldActive;
            test.Release();
            Object.DestroyImmediate(test);
        }
    }
}
