using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using TMPro;

public static class FixDeploymentHUDFonts
{
    private const string ScenePath =
        "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

    [MenuItem("Obsidian Protocol/Deployment/FIX HUD FONTS")]
    public static void Fix()
    {
        EditorSceneManager.OpenScene(
            ScenePath,
            OpenSceneMode.Single);

        TMP_FontAsset font = null;

        string[] guids =
            AssetDatabase.FindAssets(
                "EurostileExtendedBlack t:TMP_FontAsset");

        if (guids.Length > 0)
        {
            font =
                AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(
                    AssetDatabase.GUIDToAssetPath(guids[0]));
        }

        if (font == null)
        {
            Debug.LogError(
                "[DEPLOYMENT FONT] EurostileExtendedBlack TMP font not found.");
            return;
        }

        GameObject hud = null;

        GameObject[] objects =
            Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in objects)
        {
            if (obj.name == "[HUD] DEPLOYMENT HUD" &&
                obj.scene == EditorSceneManager.GetActiveScene())
            {
                hud = obj;
                break;
            }
        }

        if (hud == null)
        {
            Debug.LogError(
                "[DEPLOYMENT FONT] [HUD] DEPLOYMENT HUD not found.");
            return;
        }

        TMP_Text[] texts =
            hud.GetComponentsInChildren<TMP_Text>(true);

        int changed = 0;

        foreach (TMP_Text text in texts)
        {
            if (text.font != font)
            {
                text.font = font;
                EditorUtility.SetDirty(text);
                changed++;
            }
        }

        EditorSceneManager.MarkSceneDirty(
            EditorSceneManager.GetActiveScene());

        EditorSceneManager.SaveScene(
            EditorSceneManager.GetActiveScene());

        Debug.Log(
            "[DEPLOYMENT FONT] FONT ASSIGNED = " +
            font.name);

        Debug.Log(
            "[DEPLOYMENT FONT] TEXT OBJECTS FOUND = " +
            texts.Length);

        Debug.Log(
            "[DEPLOYMENT FONT] TEXT OBJECTS UPDATED = " +
            changed);
    }
}
