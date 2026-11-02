using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class FixDeploymentBuildingTextMaterials
{
    private const string ScenePath =
        "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

    [MenuItem("Obsidian Protocol/Deployment/FIX BUILDING TEXT MATERIALS")]
    public static void Fix()
    {
        EditorSceneManager.OpenScene(
            ScenePath,
            OpenSceneMode.Single);

        TextMesh[] texts =
            Object.FindObjectsByType<TextMesh>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);

        int changed = 0;

        foreach (TextMesh text in texts)
        {
            Renderer renderer =
                text.GetComponent<Renderer>();

            if (renderer == null)
                continue;

            if (renderer.sharedMaterial != null)
            {
                renderer.sharedMaterial = null;
                EditorUtility.SetDirty(renderer);
                changed++;
            }
        }

        EditorSceneManager.MarkSceneDirty(
            EditorSceneManager.GetActiveScene());

        EditorSceneManager.SaveScene(
            EditorSceneManager.GetActiveScene());

        Debug.Log(
            "[BUILDING TEXT MATERIAL] TEXT MESH OBJECTS FOUND = " +
            texts.Length);

        Debug.Log(
            "[BUILDING TEXT MATERIAL] MATERIALS CLEARED = " +
            changed);
    }
}
