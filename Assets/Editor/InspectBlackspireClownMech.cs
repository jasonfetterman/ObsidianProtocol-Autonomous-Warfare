using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class InspectBlackspireClownMech
{
    [MenuItem("Obsidian Protocol/Diagnostics/Inspect Blackspire Clown Mech")]
    public static void Inspect()
    {
        const string path = "Assets/Scenes/SCN30_ACHERON/Blackspire_Continent.unity";
        Scene scene = SceneManager.GetSceneByPath(path);
        bool openedHere = !scene.IsValid() || !scene.isLoaded;

        if (openedHere)
            scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);

        int matches = 0;

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            foreach (Transform item in root.GetComponentsInChildren<Transform>(true))
            {
                string n = item.name.ToLowerInvariant();
                if (!n.Contains("clown") && !n.Contains("harlequin") &&
                    !n.Contains("mech") && !n.Contains("war_machine"))
                    continue;

                matches++;
                Debug.Log("[BLACKSPIRE INSPECT] Object: " + GetPath(item) +
                    " | Position: " + item.position, item.gameObject);

                foreach (MeshFilter mf in item.GetComponents<MeshFilter>())
                    if (mf.sharedMesh != null)
                        Debug.Log("[BLACKSPIRE INSPECT] Mesh: " +
                            mf.sharedMesh.name + " | Asset: " +
                            AssetDatabase.GetAssetPath(mf.sharedMesh), item.gameObject);

                foreach (SkinnedMeshRenderer smr in item.GetComponents<SkinnedMeshRenderer>())
                    if (smr.sharedMesh != null)
                        Debug.Log("[BLACKSPIRE INSPECT] Skinned mesh: " +
                            smr.sharedMesh.name + " | Asset: " +
                            AssetDatabase.GetAssetPath(smr.sharedMesh), item.gameObject);

                Debug.Log("[BLACKSPIRE INSPECT] Components: " +
                    string.Join(", ", System.Array.ConvertAll(
                        item.GetComponents<Component>(),
                        c => c == null ? "Missing Script" : c.GetType().Name)),
                    item.gameObject);
            }
        }

        Debug.Log("[BLACKSPIRE INSPECT] Total matches: " + matches);

        if (openedHere && scene.IsValid() && scene.isLoaded)
            EditorSceneManager.CloseScene(scene, true);
    }

    private static string GetPath(Transform t)
    {
        string path = t.name;
        while (t.parent != null)
        {
            t = t.parent;
            path = t.name + "/" + path;
        }
        return path;
    }
}
