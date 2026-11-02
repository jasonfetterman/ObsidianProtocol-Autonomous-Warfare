using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class RepairHarlequinCollider
{
    [MenuItem("Obsidian Protocol/Unit Takeover/Restore Harlequin Collider")]
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

                Undo.RegisterFullObjectHierarchyUndo(mech.gameObject,
                    "Restore Harlequin Collider");

                var box = mech.GetComponent<BoxCollider>();
                if (!box)
                    box = Undo.AddComponent<BoxCollider>(mech.gameObject);

                Bounds world = smr.bounds;
                Vector3 scale = mech.lossyScale;
                box.center = mech.InverseTransformPoint(world.center);
                box.size = new Vector3(
                    world.size.x / Mathf.Max(Mathf.Abs(scale.x), 0.001f),
                    world.size.y / Mathf.Max(Mathf.Abs(scale.y), 0.001f),
                    world.size.z / Mathf.Max(Mathf.Abs(scale.z), 0.001f));

                box.isTrigger = false;
                EditorSceneManager.MarkSceneDirty(scene);
                EditorSceneManager.SaveScene(scene);

                Debug.Log("[HARLEQUIN COLLIDER] Restored on " + mech.name +
                    "; center=" + box.center + "; size=" + box.size, mech.gameObject);
                return;
            }

            Debug.LogError("[HARLEQUIN COLLIDER] Existing mech not found.");
        }
        finally
        {
            if (openedHere)
                EditorSceneManager.CloseScene(scene, true);
        }
    }
}
