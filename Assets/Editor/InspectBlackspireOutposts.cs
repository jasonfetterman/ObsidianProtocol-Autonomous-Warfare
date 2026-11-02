using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class InspectBlackspireOutposts
{
    [MenuItem("Obsidian Protocol/Diagnostics/Inspect Ironclad Outposts")]
    public static void Run()
    {
        string[] paths =
        {
            "Assets/Art/Models/others/Meshy_AI_Ironclad_Outpost_1015181452_texture_fbx/Meshy_AI_Ironclad_Outpost_1015181452_texture_fbx/Meshy_AI_Ironclad_Outpost_1015181452_texture.fbx",
            "Assets/Art/Models/others/Meshy_AI_Ironclad_Outpost_1015183324_texture_fbx/Meshy_AI_Ironclad_Outpost_1015183324_texture_fbx/Meshy_AI_Ironclad_Outpost_1015183324_texture.fbx",
            "Assets/Art/Models/others/Meshy_AI_Ironclad_Outpost_1015183731_texture_fbx/Meshy_AI_Ironclad_Outpost_1015183731_texture_fbx/Meshy_AI_Ironclad_Outpost_1015183731_texture.fbx"
        };

        var preview = EditorSceneManager.NewPreviewScene();

        try
        {
            foreach (string path in paths)
            {
                GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                if (!asset)
                {
                    Debug.LogWarning("[OUTPOST INSPECT] Missing: " + path);
                    continue;
                }

                GameObject instance =
                    (GameObject)PrefabUtility.InstantiatePrefab(asset, preview);

                if (!instance)
                {
                    Debug.LogWarning("[OUTPOST INSPECT] Cannot instantiate: " + path);
                    continue;
                }

                var renderers = instance.GetComponentsInChildren<Renderer>(true);
                var filters = instance.GetComponentsInChildren<MeshFilter>(true);
                bool hasBounds = false;
                Bounds combined = new Bounds();

                foreach (Renderer r in renderers)
                {
                    if (!hasBounds) { combined = r.bounds; hasBounds = true; }
                    else combined.Encapsulate(r.bounds);
                }

                Debug.Log("[OUTPOST INSPECT] " + path +
                    " | Root scale=" + instance.transform.localScale.ToString("F3") +
                    " | Renderers=" + renderers.Length +
                    " | MeshFilters=" + filters.Length +
                    " | Size=" + (hasBounds ? combined.size.ToString("F2") : "NO RENDERERS") +
                    " | Materials=" + instance.GetComponentsInChildren<Renderer>(true).Length);

                Object.DestroyImmediate(instance);
            }
        }
        finally
        {
            EditorSceneManager.ClosePreviewScene(preview);
        }
    }
}
