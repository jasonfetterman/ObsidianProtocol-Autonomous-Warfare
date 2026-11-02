using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class FixDeploymentBuildingFonts
{
    private const string ScenePath =
        "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

    [MenuItem("Obsidian Protocol/Deployment/FIX BUILDING TEXT FONTS")]
    public static void Fix()
    {
        EditorSceneManager.OpenScene(
            ScenePath,
            OpenSceneMode.Single);

        Font font = null;

        string[] guids =
            AssetDatabase.FindAssets(
                "LiberationSans t:Font");

        if (guids.Length > 0)
        {
            font =
                AssetDatabase.LoadAssetAtPath<Font>(
                    AssetDatabase.GUIDToAssetPath(guids[0]));
        }

        if (font == null)
        {
            Debug.LogError(
                "[BUILDING FONT] Liberation Sans font not found.");
            return;
        }

        TextMesh[] texts =
            Object.FindObjectsByType<TextMesh>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);

        int updated = 0;

        foreach (TextMesh text in texts)
        {
            if (text.font != font)
            {
                text.font = font;
                EditorUtility.SetDirty(text);
                updated++;
            }
        }

        EditorSceneManager.MarkSceneDirty(
            EditorSceneManager.GetActiveScene());

        EditorSceneManager.SaveScene(
            EditorSceneManager.GetActiveScene());

        Debug.Log(
            "[BUILDING FONT] FONT ASSIGNED = " +
            font.name);

        Debug.Log(
            "[BUILDING FONT] TEXT MESH OBJECTS FOUND = " +
            texts.Length);

        Debug.Log(
            "[BUILDING FONT] TEXT MESH OBJECTS UPDATED = " +
            updated);
    }
}
