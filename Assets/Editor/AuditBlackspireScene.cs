using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class AuditBlackspireScene
{
    [MenuItem("Obsidian Protocol/Diagnostics/Audit Blackspire Scene")]
    public static void Run()
    {
        const string path = "Assets/Scenes/SCN30_ACHERON/Blackspire_Continent.unity";
        var scene = SceneManager.GetSceneByPath(path);
        bool openedHere = !scene.IsValid() || !scene.isLoaded;

        if (openedHere)
            scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);

        try
        {
            foreach (var root in scene.GetRootGameObjects())
            {
                if (root.name != "BLACKSPIRE MESHY TERRAIN" &&
                    root.name != "CRIMSON_CLOWN_CITADEL_HEADQUARTERS" &&
                    root.name != "Crimson_Clowns") continue;

                Debug.Log("[BLACKSPIRE AUDIT] " + root.name +
                    " | Position=" + root.transform.position.ToString("F2") +
                    " | Scale=" + root.transform.lossyScale.ToString("F3"));

                foreach (var r in root.GetComponentsInChildren<Renderer>(true))
                    Debug.Log("[BLACKSPIRE AUDIT] Renderer=" + r.name +
                        " | Center=" + r.bounds.center.ToString("F2") +
                        " | Size=" + r.bounds.size.ToString("F2"));

                foreach (var c in root.GetComponentsInChildren<Collider>(true))
                    Debug.Log("[BLACKSPIRE AUDIT] Collider=" + c.GetType().Name +
                        " | Object=" + c.name +
                        " | Enabled=" + c.enabled +
                        " | Center=" + c.bounds.center.ToString("F2") +
                        " | Size=" + c.bounds.size.ToString("F2"));
            }

            string folder = "Assets/Rocks and Boulders 2/Rocks/Prefabs";
            foreach (string guid in AssetDatabase.FindAssets("t:Prefab", new[] { folder }))
            {
                string prefabPath = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if (prefab == null) continue;

                var renderers = prefab.GetComponentsInChildren<Renderer>(true);
                if (renderers.Length == 0)
                {
                    Debug.Log("[BLACKSPIRE ROCK] " + prefab.name + " | No renderers");
                    continue;
                }

                Bounds bounds = renderers[0].bounds;
                for (int i = 1; i < renderers.Length; i++)
                    bounds.Encapsulate(renderers[i].bounds);

                Debug.Log("[BLACKSPIRE ROCK] " + prefab.name +
                    " | Path=" + prefabPath +
                    " | LocalBoundsSize=" +
                    prefab.GetComponentInChildren<MeshFilter>()?.sharedMesh?.bounds.size.ToString("F2"));
            }
        }
        finally
        {
            if (openedHere)
                EditorSceneManager.CloseScene(scene, true);
        }
    }
}
