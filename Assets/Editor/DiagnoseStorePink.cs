using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public static class DiagnoseStorePink
{
    [MenuItem("Obsidian Protocol/Store/Diagnose Pink Materials")]
    private static void Run()
    {
        Debug.Log("[STORE DIAG] Active pipeline: " +
            (GraphicsSettings.currentRenderPipeline
                ? GraphicsSettings.currentRenderPipeline.name + " (" +
                  GraphicsSettings.currentRenderPipeline.GetType().FullName + ")"
                : "Built-in"));

        const string path = "Assets/Prefabs/Buildings/Store/STORE_FACILITY.prefab";
        GameObject root = AssetDatabase.LoadAssetAtPath<GameObject>(path);

        if (!root)
        {
            Debug.LogError("[STORE DIAG] Production prefab not found: " + path);
            return;
        }

        int renderers = 0;
        int missing = 0;
        int unsupported = 0;

        foreach (Renderer r in root.GetComponentsInChildren<Renderer>(true))
        {
            renderers++;
            foreach (Material m in r.sharedMaterials)
            {
                if (!m)
                {
                    missing++;
                    Debug.LogError("[STORE DIAG] Missing material: " + r.name);
                    continue;
                }

                if (!m.shader)
                {
                    missing++;
                    Debug.LogError("[STORE DIAG] Missing shader: " + m.name +
                        " on " + r.name);
                    continue;
                }

                if (!m.shader.isSupported)
                {
                    unsupported++;
                    Debug.LogError("[STORE DIAG] Unsupported shader: " +
                        m.shader.name + " | " + m.name + " | " + r.name);
                }
            }
        }

        Debug.Log($"[STORE DIAG] Prefab={path}; Renderers={renderers}; MissingMaterialsOrShaders={missing}; UnsupportedShaders={unsupported}");
    }
}
