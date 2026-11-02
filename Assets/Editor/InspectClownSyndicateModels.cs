using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class InspectClownSyndicateModels
{
    [MenuItem("Obsidian Protocol/Diagnostics/Inspect Clown Syndicate Models")]
    public static void Run()
    {
        string[] guids = AssetDatabase.FindAssets(
            "t:Model", new[] { "Assets/Art/Models/ClownSyndicate" });

        var preview = EditorSceneManager.NewPreviewScene();

        try
        {
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (!asset) continue;

                GameObject instance =
                    (GameObject)PrefabUtility.InstantiatePrefab(asset, preview);
                if (!instance) continue;

                var renderers = instance.GetComponentsInChildren<Renderer>(true);
                bool hasBounds = false;
                Bounds bounds = new Bounds();

                foreach (Renderer r in renderers)
                {
                    if (!hasBounds) { bounds = r.bounds; hasBounds = true; }
                    else bounds.Encapsulate(r.bounds);
                }

                Debug.Log("[CLOWN MODEL] " + asset.name +
                    " | Size=" + (hasBounds ? bounds.size.ToString("F2") : "NO RENDERERS") +
                    " | DefaultScale=" + asset.transform.localScale.ToString("F2") +
                    " | Path=" + path);

                Object.DestroyImmediate(instance);
            }
        }
        finally
        {
            EditorSceneManager.ClosePreviewScene(preview);
        }
    }
}
