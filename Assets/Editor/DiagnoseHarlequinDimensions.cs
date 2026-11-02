using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class DiagnoseHarlequinDimensions
{
    [MenuItem("Obsidian Protocol/Unit Takeover/Diagnose Harlequin Dimensions")]
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

                Debug.Log(
                    "[HARLEQUIN DIMENSIONS] root=" + mech.name +
                    "; rootPosition=" + mech.position +
                    "; rootLossyScale=" + mech.lossyScale +
                    "; renderer=" + smr.name +
                    "; rendererLocalBounds=" + smr.localBounds +
                    "; rendererWorldBounds=" + smr.bounds +
                    "; rendererLossyScale=" + smr.transform.lossyScale +
                    "; collider=" + (box ? box.bounds.ToString() : "MISSING"),
                    mech.gameObject);

                return;
            }

            Debug.LogError("[HARLEQUIN DIMENSIONS] Existing mech renderer not found.");
        }
        finally
        {
            if (openedHere)
                EditorSceneManager.CloseScene(scene, true);
        }
    }
}
