using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class FindClownGlbInstance
{
    [MenuItem("Obsidian Protocol/Diagnostics/Find Clown GLB Instance")]
    public static void Find()
    {
        const string path = "Assets/Scenes/SCN30_ACHERON/Blackspire_Continent.unity";
        Scene scene = SceneManager.GetSceneByPath(path);
        bool openedHere = !scene.IsValid() || !scene.isLoaded;
        if (openedHere) scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);

        int count = 0;
        foreach (GameObject root in scene.GetRootGameObjects())
        foreach (Transform t in root.GetComponentsInChildren<Transform>(true))
        {
            foreach (SkinnedMeshRenderer smr in t.GetComponents<SkinnedMeshRenderer>())
            {
                if (smr.sharedMesh == null) continue;
                string asset = AssetDatabase.GetAssetPath(smr.sharedMesh);
                if (asset.Contains("autorig-7e7ecd87-v4-glb"))
                {
                    count++;
                    Debug.Log("[GLB INSTANCE] " + t.name + " | " + asset +
                              " | Position: " + t.position, t.gameObject);
                }
            }
        }

        Debug.Log("[GLB INSTANCE] Matches: " + count);
        if (openedHere && scene.IsValid() && scene.isLoaded)
            EditorSceneManager.CloseScene(scene, true);
    }
}
