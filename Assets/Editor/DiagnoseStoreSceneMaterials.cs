using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class DiagnoseStoreSceneMaterials
{
    [MenuItem("Obsidian Protocol/Store/Inspect Scene Store Materials")]
    private static void Run()
    {
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log("[STORE SCENE DIAG] Active scene: " + scene.path);

        int found = 0;
        foreach (GameObject go in Object.FindObjectsByType<GameObject>(
            FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (go.scene != scene) continue;
            if (go.name != "STORE_AAA_REBUILD" &&
                go.name != "STORE_FACILITY") continue;

            found++;
            int renderers = 0;
            Debug.Log("[STORE SCENE DIAG] Object: " + go.name +
                " | Position: " + go.transform.position);

            foreach (Renderer r in go.GetComponentsInChildren<Renderer>(true))
            {
                renderers++;
                foreach (Material m in r.sharedMaterials)
                {
                    if (!m)
                    {
                        Debug.LogError("[STORE SCENE DIAG] NULL material on " + r.name);
                        continue;
                    }

                    Debug.Log("[STORE SCENE DIAG] " + r.name +
                        " | Material=" + m.name +
                        " | Shader=" + (m.shader ? m.shader.name : "NULL") +
                        " | MaterialPath=" + AssetDatabase.GetAssetPath(m));
                }
            }

            Debug.Log("[STORE SCENE DIAG] Object renderer count: " + renderers);
        }

        Debug.Log("[STORE SCENE DIAG] Matching Store objects found: " + found);
    }
}
