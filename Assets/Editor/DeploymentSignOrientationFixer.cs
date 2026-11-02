using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class DeploymentSignOrientationFixer
{
    private const string ScenePath =
        "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

    [MenuItem("Obsidian Protocol/Deployment/Fix Existing Sign Text Orientation")]
    private static void FixSignTextOrientation()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogError("Exit Play Mode before fixing signage.");
            return;
        }

        Scene scene = SceneManager.GetActiveScene();

        if (scene.path != ScenePath)
        {
            Debug.LogError(
                "Open SCN12_DEPLOYMENT.unity before running this fix. No changes made.");
            return;
        }

        GameObject root = GameObject.Find("[FACILITY] GENERATED SIGNAGE");

        if (root == null || !root.scene.IsValid() || root.scene != scene)
        {
            Debug.LogError("Generated signage root not found in the active scene.");
            return;
        }

        Transform[] transforms = root.GetComponentsInChildren<Transform>(true);
        int fixedCount = 0;

        foreach (Transform item in transforms)
        {
            if (item.name != "SIGN TEXT")
                continue;

            if (item.GetComponent<TextMesh>() == null)
                continue;

            Undo.RecordObject(item, "Fix Deployment Sign Text Orientation");
            item.localRotation = Quaternion.Euler(0f, 180f, 0f);
            EditorUtility.SetDirty(item);
            fixedCount++;
        }

        if (fixedCount != 6)
        {
            Debug.LogWarning(
                $"Expected 6 sign text objects but found {fixedCount}. Review the scene before saving.");
        }

        EditorSceneManager.MarkSceneDirty(scene);
        Debug.Log($"Updated {fixedCount} existing deployment sign text objects. Inspect before saving.");
    }
}
