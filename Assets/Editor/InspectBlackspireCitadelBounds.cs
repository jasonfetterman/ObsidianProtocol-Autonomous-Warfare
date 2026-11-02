using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class InspectBlackspireCitadelBounds
{
    [MenuItem("Obsidian Protocol/Diagnostics/Inspect Blackspire Citadel Bounds")]
    public static void Inspect()
    {
        const string path =
            "Assets/Art/Models/ClownSyndicate/Crimson_Clown_Citadel/Meshy_AI_Crimson_Clown_Citadel_20261018_texture.fbx";

        GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if (model == null)
        {
            Debug.LogError("[CITADEL BOUNDS] FBX could not be loaded: " + path);
            return;
        }

        Scene preview = EditorSceneManager.NewPreviewScene();
        GameObject instance = null;

        try
        {
            instance = (GameObject)PrefabUtility.InstantiatePrefab(model, preview);
            if (instance == null)
            {
                Debug.LogError("[CITADEL BOUNDS] Preview instantiation failed.");
                return;
            }

            var renderers = instance.GetComponentsInChildren<Renderer>(true);
            bool hasBounds = false;
            Bounds combined = new Bounds();

            foreach (Renderer r in renderers)
            {
                if (!hasBounds)
                {
                    combined = r.bounds;
                    hasBounds = true;
                }
                else combined.Encapsulate(r.bounds);
            }

            Debug.Log("[CITADEL BOUNDS] Asset: " + path);
            Debug.Log("[CITADEL BOUNDS] Root scale: " + instance.transform.localScale);
            Debug.Log("[CITADEL BOUNDS] Renderer count: " + renderers.Length);

            if (hasBounds)
                Debug.Log("[CITADEL BOUNDS] Size XYZ: " + combined.size.ToString("F2") +
                          " | Center: " + combined.center.ToString("F2"));
            else
                Debug.LogWarning("[CITADEL BOUNDS] No renderers found.");
        }
        finally
        {
            if (instance != null) Object.DestroyImmediate(instance);
            EditorSceneManager.ClosePreviewScene(preview);
        }
    }
}
