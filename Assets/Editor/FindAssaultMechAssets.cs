using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class FindAssaultMechAssets
{
    [MenuItem("Obsidian Protocol/Diagnostics/Find Assault Mech Assets")]
    public static void Find()
    {
        const string path = "Assets/Scenes/SCN30_ACHERON/Blackspire_Continent.unity";
        Scene scene = SceneManager.GetSceneByPath(path);
        bool openedHere = !scene.IsValid() || !scene.isLoaded;

        if (openedHere)
            scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);

        int count = 0;
        foreach (GameObject root in scene.GetRootGameObjects())
        foreach (Transform t in root.GetComponentsInChildren<Transform>(true))
        {
            foreach (MeshFilter mf in t.GetComponents<MeshFilter>())
            {
                if (mf.sharedMesh == null) continue;
                string asset = AssetDatabase.GetAssetPath(mf.sharedMesh);
                if (asset.IndexOf("Harlequin", System.StringComparison.OrdinalIgnoreCase) < 0 &&
                    asset.IndexOf("Assault", System.StringComparison.OrdinalIgnoreCase) < 0)
                    continue;

                count++;
                Debug.Log("[ASSAULT SEARCH] " + t.name + " | " + asset, t.gameObject);
            }
        }

        Debug.Log("[ASSAULT SEARCH] Matches: " + count);
        if (openedHere && scene.IsValid() && scene.isLoaded)
            EditorSceneManager.CloseScene(scene, true);
    }
}
