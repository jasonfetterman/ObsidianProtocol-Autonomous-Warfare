using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public static class OPAWSettingsReadabilityFix
{
    [MenuItem("Obsidian Protocol/Settings/FIX TEXT READABILITY")]
    public static void Fix()
    {
        const string scenePath =
            "Assets/Scenes/SCN14_SETTINGS/[HUD] SETTINGS HUD/Settings.unity";

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return;

        var scene = EditorSceneManager.OpenScene(
            scenePath, UnityEditor.SceneManagement.OpenSceneMode.Single);

        int fixedTMP = 0;
        int fixedLegacy = 0;

        foreach (var t in Object.FindObjectsByType<TMP_Text>(
                     FindObjectsSortMode.None))
        {
            if (t.gameObject.scene != scene) continue;

            string n = t.gameObject.name.ToUpperInvariant();

            float minimum = 16f;
            if (n.Contains("TITLE") || n.Contains("HEADER"))
                minimum = 24f;
            else if (n.Contains("SUBTITLE") || n.Contains("DESCRIPTION"))
                minimum = 15f;
            else if (n.Contains("NAV_") || n.Contains("STATUS"))
                minimum = 16f;

            t.fontSize = Mathf.Max(t.fontSize, minimum);
            t.color = n.Contains("STATUS") || n.Contains("VALUE") ||
                      n.Contains("INDICATOR")
                ? new Color(.25f, .80f, .93f, 1f)
                : new Color(.90f, .95f, .98f, 1f);

            t.enableAutoSizing = false;
            t.textWrappingMode = TextWrappingModes.Normal;
            t.raycastTarget = false;
            fixedTMP++;
        }

        foreach (var t in Object.FindObjectsByType<Text>(
                     FindObjectsSortMode.None))
        {
            if (t.gameObject.scene != scene) continue;

            t.fontSize = Mathf.Max(t.fontSize, 16);
            t.color = new Color(.90f, .95f, .98f, 1f);
            fixedLegacy++;
        }

        foreach (var image in Object.FindObjectsByType<Image>(
                     FindObjectsSortMode.None))
        {
            if (image.gameObject.scene != scene) continue;

            string n = image.gameObject.name.ToUpperInvariant();
            if (n.Contains("HEADER_SURFACE") ||
                n.Contains("CONTENT_SURFACE") ||
                n.Contains("SETTINGS_NAVIGATION"))
                image.color = new Color(.012f, .029f, .043f, 1f);
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("SETTINGS READABILITY REPAIR COMPLETE. TMP text fixed: " +
                  fixedTMP + " | Legacy text fixed: " + fixedLegacy);
    }
}
