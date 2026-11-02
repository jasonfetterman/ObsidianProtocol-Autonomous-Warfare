using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class RemoveHarlequinCollider
{
    [MenuItem("Obsidian Protocol/Unit Takeover/Remove Harlequin BoxCollider")]
    private static void Run()
    {
        const string path = "Assets/Scenes/SCN30_ACHERON/Blackspire_Continent.unity";
        var scene = SceneManager.GetSceneByPath(path);
        bool openedHere = !scene.IsValid() || !scene.isLoaded;

        if (openedHere)
            scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);

        try
        {
            foreach (var root in scene.GetRootGameObjects())
            foreach (var smr in root.GetComponentsInChildren<SkinnedMeshRenderer>(true))
            {
                if (smr.sharedMesh == null ||
                    !AssetDatabase.GetAssetPath(smr.sharedMesh)
                        .Contains("autorig-7e7ecd87-v4-glb"))
                    continue;

                Transform mech = smr.transform;
                while (mech.parent != null && mech.parent != root.transform)
                    mech = mech.parent;

                var box = mech.GetComponent<BoxCollider>();
                if (box)
                {
                    Undo.DestroyObjectImmediate(box);
                    EditorSceneManager.MarkSceneDirty(scene);
                    EditorSceneManager.SaveScene(scene);
                    Debug.Log("[HARLEQUIN COLLIDER] Removed BoxCollider from " + mech.name);
                }
                else
                    Debug.Log("[HARLEQUIN COLLIDER] No BoxCollider found on " + mech.name);

                return;
            }

            Debug.LogError("[HARLEQUIN COLLIDER] Harlequin instance not found.");
        }
        finally
        {
            if (openedHere)
                EditorSceneManager.CloseScene(scene, true);
        }
    }
}
