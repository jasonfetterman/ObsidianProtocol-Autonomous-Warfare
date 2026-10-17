using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class MultiplayerMatchSettingsFixBuilder
{
    [MenuItem("Obsidian Protocol/Multiplayer/REBUILD MATCH SETTINGS PANEL")]
    public static void Build()
    {
        Canvas canvas =
            Object.FindFirstObjectByType<Canvas>();

        if (canvas == null)
        {
            Debug.LogError(
                "[MULTIPLAYER] No Canvas found.");

            return;
        }

        Transform existing =
            canvas.transform.Find(
                "MATCH_SETTINGS_PANEL");

        if (existing == null)
        {
            Debug.LogError(
                "[MULTIPLAYER] MATCH_SETTINGS_PANEL not found.");

            return;
        }

        GameObject panel =
            existing.gameObject;

        ClearChildren(panel.transform);

        RectTransform rect =
            panel.GetComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0.5f, 0.5f);

        rect.anchorMax =
            new Vector2(0.5f, 0.5f);

        rect.pivot =
            new Vector2(0.5f, 0.5f);

        rect.sizeDelta =
            new Vector2(900, 620);

        rect.anchoredPosition =
            Vector2.zero;

        Image background =
            panel.GetComponent<Image>();

        if (background == null)
            background =
                panel.AddComponent<Image>();

        background.color =
            new Color(
                0.025f,
                0.035f,
                0.05f,
                0.97f);

        AddText(
            panel.transform,
            "TITLE",
            "MATCH SETTINGS",
            40,
            new Vector2(0, 240),
            760);

        AddText(
            panel.transform,
            "SUBTITLE",
            "OPERATION CONFIGURATION",
            23,
            new Vector2(0, 195),
            760);

        CreateButton(
            panel.transform,
            "RANKED_BUTTON",
            "RANKED OPERATIONS // RANKED",
            new Vector2(0, 115));

        CreateButton(
            panel.transform,
            "RULESET_BUTTON",
            "RULESET // STANDARD",
            new Vector2(0, 35));

        CreateButton(
            panel.transform,
            "COMMANDER_COUNT_BUTTON",
            "COMMANDERS // 4",
            new Vector2(0, -45));

        AddText(
            panel.transform,
            "DEPLOYMENT_BUDGET",
            "DEPLOYMENT BUDGET // 10,000 DP",
            27,
            new Vector2(0, -125),
            760);

        CreateButton(
            panel.transform,
            "CLOSE_BUTTON",
            "CLOSE",
            new Vector2(0, -245));

        MultiplayerMatchSettingsController controller =
            panel.GetComponent<
                MultiplayerMatchSettingsController>();

        if (controller == null)
        {
            panel.AddComponent<
                MultiplayerMatchSettingsController>();
        }

        panel.SetActive(false);

        EditorUtility.SetDirty(panel);

        Debug.Log(
            "[MULTIPLAYER] MATCH SETTINGS PANEL FIXED.");
    }

    private static void CreateButton(
        Transform parent,
        string name,
        string label,
        Vector2 position)
    {
        GameObject button =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image),
                typeof(Button));

        button.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            button.GetComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0.5f, 0.5f);

        rect.anchorMax =
            new Vector2(0.5f, 0.5f);

        rect.pivot =
            new Vector2(0.5f, 0.5f);

        rect.sizeDelta =
            new Vector2(520, 62);

        rect.anchoredPosition =
            position;

        Image image =
            button.GetComponent<Image>();

        image.color =
            new Color(
                0.08f,
                0.12f,
                0.18f,
                1f);

        Text text =
            AddText(
                button.transform,
                "LABEL",
                label,
                22,
                Vector2.zero,
                500);

        text.raycastTarget = false;
    }

    private static Text AddText(
        Transform parent,
        string name,
        string value,
        int size,
        Vector2 position,
        float width)
    {
        GameObject obj =
            new GameObject(
                name,
                typeof(RectTransform));

        obj.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            obj.GetComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0.5f, 0.5f);

        rect.anchorMax =
            new Vector2(0.5f, 0.5f);

        rect.pivot =
            new Vector2(0.5f, 0.5f);

        rect.sizeDelta =
            new Vector2(width, 48);

        rect.anchoredPosition =
            position;

        Text text =
            obj.AddComponent<Text>();

        text.text = value;
        text.fontSize = size;
        text.alignment =
            TextAnchor.MiddleCenter;

        text.color = Color.white;

        text.horizontalOverflow =
            HorizontalWrapMode.Wrap;

        text.verticalOverflow =
            VerticalWrapMode.Truncate;

        Font font =
            Resources.GetBuiltinResource<Font>(
                "LegacyRuntime.ttf");

        if (font != null)
            text.font = font;

        return text;
    }

    private static void ClearChildren(
        Transform parent)
    {
        for (int i = parent.childCount - 1;
             i >= 0;
             i--)
        {
            Object.DestroyImmediate(
                parent.GetChild(i).gameObject);
        }
    }
}
