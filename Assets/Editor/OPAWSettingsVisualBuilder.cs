using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public static class OPAWSettingsVisualBuilder
{
    private static readonly Color Background = new Color(0.008f, 0.022f, 0.034f, 1f);
    private static readonly Color Surface = new Color(0.016f, 0.039f, 0.056f, 0.99f);
    private static readonly Color Card = new Color(0.025f, 0.055f, 0.075f, 1f);
    private static readonly Color Nav = new Color(0.026f, 0.052f, 0.071f, 1f);
    private static readonly Color Cyan = new Color(0.251f, 0.796f, 0.922f, 1f);
    private static readonly Color White = new Color(0.886f, 0.929f, 0.961f, 1f);
    private static readonly Color Muted = new Color(0.43f, 0.57f, 0.65f, 1f);

    [MenuItem("Obsidian Protocol/Settings/BUILD COMMAND CENTER VISUAL")]
    public static void Build()
    {
        const string scenePath =
            "Assets/Scenes/SCN14_SETTINGS/[HUD] SETTINGS HUD/Settings.unity";

        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            var scene = EditorSceneManager.OpenScene(
                scenePath, OpenSceneMode.Single);

            if (!scene.IsValid())
            {
                Debug.LogError("Settings scene could not be opened.");
                return;
            }

            int changed = 0;

            foreach (var image in Object.FindObjectsByType<Image>(
                         FindObjectsSortMode.None))
            {
                string n = image.gameObject.name.ToUpperInvariant();

                if (n.Contains("HEADER_SURFACE"))
                    image.color = Surface;
                else if (n.Contains("CONTENT_SURFACE"))
                    image.color = Background;
                else if (n.Contains("SETTINGS_NAVIGATION"))
                    image.color = Surface;
                else if (n.Contains("SETTINGS_CARD") ||
                         n.Contains("GENERAL_SETTINGS") ||
                         n.Contains("DISPLAY_SETTINGS") ||
                         n.Contains("AUDIO_SETTINGS") ||
                         n.Contains("CONTROL_SETTINGS") ||
                         n.Contains("INTERFACE_SETTINGS") ||
                         n.Contains("ACCESSIBILITY_SETTINGS"))
                    image.color = Card;
                else if (n.Contains("ACTIVE_INDICATOR") ||
                         n.Contains("CARD_ACCENT") ||
                         n.Contains("TITLE_RULE"))
                    image.color = Cyan;
                else if (n.StartsWith("NAV_") || n.Contains("OPTION_"))
                    image.color = Nav;
                else if (n.Contains("BACKGROUND"))
                    image.color = Background;

                changed++;
            }

            foreach (var text in Object.FindObjectsByType<TMP_Text>(
                         FindObjectsSortMode.None))
            {
                string n = text.gameObject.name.ToUpperInvariant();

                if (n.Contains("SUBTITLE") || n.Contains("DESCRIPTION") ||
                    n.Contains("HINT") || n.Contains("SECONDARY"))
                    text.color = Muted;
                else if (n.Contains("VALUE") || n.Contains("STATUS"))
                    text.color = Cyan;
                else
                    text.color = White;
            }

            foreach (var text in Object.FindObjectsByType<Text>(
                         FindObjectsSortMode.None))
            {
                string n = text.gameObject.name.ToUpperInvariant();
                text.color = (n.Contains("VALUE") || n.Contains("STATUS"))
                    ? Cyan : White;
            }

            AddAccent(scene, "HEADER_SURFACE", "OPAW_HEADER_CYAN_RULE",
                new Vector2(0f, 0f), new Vector2(1f, 0f),
                new Vector2(0f, 0f), new Vector2(0f, 3f));

            AddAccent(scene, "SETTINGS_CARD", "OPAW_CARD_TOP_RULE",
                new Vector2(0f, 1f), new Vector2(1f, 1f),
                new Vector2(0f, -3f), new Vector2(0f, 3f));

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);

            Debug.Log(
                $"OPAW SETTINGS VISUAL BUILD COMPLETE. Styled {changed} UI images. Scene saved.");
        }
    }

    private static void AddAccent(Scene scene, string parentName,
        string childName, Vector2 anchorMin, Vector2 anchorMax,
        Vector2 offsetMin, Vector2 offsetMax)
    {
        GameObject parent = null;

        foreach (var root in scene.GetRootGameObjects())
        {
            var transforms = root.GetComponentsInChildren<Transform>(true);
            foreach (var t in transforms)
            {
                if (t.name == parentName)
                {
                    parent = t.gameObject;
                    break;
                }
            }
            if (parent != null) break;
        }

        if (parent == null) return;

        Transform existing = parent.transform.Find(childName);
        if (existing != null) return;

        var go = new GameObject(childName,
            typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));

        go.transform.SetParent(parent.transform, false);

        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = offsetMin;
        rect.offsetMax = offsetMax;

        var image = go.GetComponent<Image>();
        image.color = Cyan;
        image.raycastTarget = false;
    }
}
