using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public static class StorePinkMaterialRepair
{
    private static readonly string[] Folders =
    {
        "Assets/Materials/StoreExtracted",
        "Assets/Materials/StoreNeonFrames",
        "Assets/Materials/StoreVideoScreens",
        "Assets/Scenes/SCN16_STORE/Materials",
        "Assets/Video"
    };

    [MenuItem("Obsidian Protocol/Store/Repair Broken Pink Materials")]
    public static void Repair()
    {
        Shader lit = Shader.Find("Universal Render Pipeline/Lit");
        Shader unlit = Shader.Find("Universal Render Pipeline/Unlit");

        if (lit == null || unlit == null)
        {
            Debug.LogError("[STORE REPAIR] URP fallback shaders were not found. No materials changed.");
            return;
        }

        int repaired = 0;
        var seen = new HashSet<string>();

        foreach (string folder in Folders)
        {
            if (!AssetDatabase.IsValidFolder(folder))
                continue;

            foreach (string guid in AssetDatabase.FindAssets("t:Material", new[] { folder }))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (!seen.Add(path))
                    continue;

                Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
                if (mat == null || mat.shader == null)
                    continue;

                string shaderName = mat.shader.name;
                if (shaderName != "Hidden/InternalErrorShader" &&
                    shaderName != "InternalErrorShader")
                    continue;

                bool visualEffect = path.ToLowerInvariant().Contains("neon") ||
                                    path.ToLowerInvariant().Contains("glow") ||
                                    path.ToLowerInvariant().Contains("video") ||
                                    path.ToLowerInvariant().Contains("screen");

                mat.shader = visualEffect ? unlit : lit;
                EditorUtility.SetDirty(mat);
                repaired++;

                Debug.Log(
                    $"[STORE REPAIR] Replaced broken shader on '{mat.name}' " +
                    $"with '{mat.shader.name}' at {path}");
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"[STORE REPAIR] Complete. Broken shader materials repaired: {repaired}");
    }
}
