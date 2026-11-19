using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public static class OPAWSettingsVisualFix
{
    static readonly Color Background = new Color(.007f,.018f,.028f,1);
    static readonly Color Surface = new Color(.018f,.040f,.057f,1);
    static readonly Color Card = new Color(.030f,.060f,.080f,1);
    static readonly Color Cyan = new Color(.25f,.80f,.93f,1);
    static readonly Color Text = new Color(.91f,.96f,.99f,1);
    static readonly Color Secondary = new Color(.53f,.67f,.74f,1);

    [MenuItem("Obsidian Protocol/Settings/APPLY VISIBLE AAA REDESIGN")]
    static void Build()
    {
        const string path = "Assets/Scenes/SCN14_SETTINGS/[HUD] SETTINGS HUD/Settings.unity";

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

        Scene scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);

        // Remove the previous ineffective decorative overlay.
        foreach (Canvas c in Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None))
        {
            if (c.gameObject.scene != scene) continue;
            Transform old = c.transform.Find("AAA_SETTINGS_INTERFACE");
            if (old != null) Object.DestroyImmediate(old.gameObject);
        }

        int textCount = 0;
        int imageCount = 0;
        int buttonCount = 0;

        foreach (TMP_Text t in Object.FindObjectsByType<TMP_Text>(FindObjectsSortMode.None))
        {
            if (t.gameObject.scene != scene) continue;

            string n = t.gameObject.name.ToUpperInvariant();
            string p = t.transform.parent ? t.transform.parent.name.ToUpperInvariant() : "";

            t.enableAutoSizing = false;
            t.fontSize = 18;
            t.color = Text;
            t.textWrappingMode = TextWrappingModes.Normal;
            t.overflowMode = TextOverflowModes.Ellipsis;
            t.raycastTarget = false;

            if (n.Contains("TITLE") || n.Contains("PAGE_TITLE"))
            {
                t.fontSize = 30;
                t.fontStyle = FontStyles.Bold;
                t.color = Text;
            }
            else if (n.Contains("SUBTITLE") || n.Contains("DESCRIPTION") ||
                     n.Contains("HINT"))
            {
                t.fontSize = 13;
                t.color = Secondary;
            }
            else if (n.Contains("VALUE") || n.Contains("STATUS"))
            {
                t.fontSize = 17;
                t.fontStyle = FontStyles.Bold;
                t.color = Cyan;
            }
            else if (n.Contains("LABEL") || n.Contains("NAV"))
            {
                t.fontSize = 17;
                t.fontStyle = FontStyles.Bold;
                t.color = Text;
            }
            else if (p.Contains("NAVIGATION"))
            {
                t.fontSize = 17;
                t.fontStyle = FontStyles.Bold;
            }

            textCount++;
        }

        foreach (Text t in Object.FindObjectsByType<Text>(FindObjectsSortMode.None))
        {
            if (t.gameObject.scene != scene) continue;
            t.fontSize = Mathf.Max(t.fontSize, 17);
            t.color = Text;
            textCount++;
        }

        foreach (Image img in Object.FindObjectsByType<Image>(FindObjectsSortMode.None))
        {
            if (img.gameObject.scene != scene) continue;

            string n = img.gameObject.name.ToUpperInvariant();

            if (n.Contains("HEADER_SURFACE"))
                img.color = new Color(.012f,.029f,.043f,1);
            else if (n.Contains("CONTENT_SURFACE"))
                img.color = Background;
            else if (n.Contains("SETTINGS_NAVIGATION"))
                img.color = Surface;
            else if (n.Contains("SETTINGS_CARD") || n.Contains("GENERAL_SETTINGS") ||
                     n.Contains("DISPLAY_SETTINGS") || n.Contains("AUDIO_SETTINGS") ||
                     n.Contains("CONTROL_SETTINGS") || n.Contains("INTERFACE_SETTINGS") ||
                     n.Contains("ACCESSIBILITY_SETTINGS"))
                img.color = Card;
            else if (n.Contains("TITLE_RULE") || n.Contains("ACTIVE_INDICATOR") ||
                     n.Contains("CARD_ACCENT"))
                img.color = Cyan;
            else if (n.Contains("BACKGROUND"))
                img.color = Background;

            imageCount++;
        }

        foreach (Button b in Object.FindObjectsByType<Button>(FindObjectsSortMode.None))
        {
            if (b.gameObject.scene != scene) continue;

            ColorBlock cb = b.colors;
            cb.normalColor = new Color(.025f,.060f,.080f,1);
            cb.highlightedColor = new Color(.045f,.16f,.20f,1);
            cb.pressedColor = new Color(.10f,.35f,.42f,1);
            cb.selectedColor = new Color(.035f,.13f,.17f,1);
            cb.disabledColor = new Color(.10f,.13f,.15f,.55f);
            cb.colorMultiplier = 1;
            b.colors = cb;

            buttonCount++;
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log($"VISIBLE SETTINGS REDESIGN APPLIED: {textCount} text elements, {imageCount} images, {buttonCount} buttons modified.");
    }
}
