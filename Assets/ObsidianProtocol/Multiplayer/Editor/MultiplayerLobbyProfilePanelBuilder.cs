using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class MultiplayerLobbyProfilePanelBuilder
{
    [MenuItem("Obsidian Protocol/Multiplayer/BUILD PROFILE PANEL")]
    public static void Build()
    {
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("[MULTIPLAYER] No Canvas found.");
            return;
        }

        Transform existing = canvas.transform.Find("PROFILE_PANEL");

        if (existing != null)
            Object.DestroyImmediate(existing.gameObject);

        GameObject panel = new GameObject(
            "PROFILE_PANEL",
            typeof(RectTransform),
            typeof(Image));

        panel.transform.SetParent(canvas.transform, false);

        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(900f, 620f);
        rect.anchoredPosition = Vector2.zero;

        Image background = panel.GetComponent<Image>();
        background.color = new Color(0.025f, 0.035f, 0.05f, 0.97f);

        AddText(panel.transform, "TITLE",
            "COMMANDER PROFILE", 40, new Vector2(0, 235));

        AddText(panel.transform, "SUBTITLE",
            "COMMAND RECORD", 23, new Vector2(0, 190));

        AddText(panel.transform, "COMMANDER",
            "COMMANDER-01", 30, new Vector2(0, 115));

        AddText(panel.transform, "RATING",
            "COMMAND RATING // 2418", 27, new Vector2(0, 55));

        AddText(panel.transform, "OPERATIONS",
            "OPERATIONS COMPLETED // 184", 27, new Vector2(0, -5));

        AddText(panel.transform, "STATUS",
            "VETERAN STATUS // ACTIVE", 27, new Vector2(0, -65));

        GameObject close = new GameObject(
            "CLOSE_BUTTON",
            typeof(RectTransform),
            typeof(Image),
            typeof(Button));

        close.transform.SetParent(panel.transform, false);

        RectTransform closeRect =
            close.GetComponent<RectTransform>();

        closeRect.sizeDelta = new Vector2(260f, 60f);
        closeRect.anchoredPosition = new Vector2(0, -235);

        Image closeImage = close.GetComponent<Image>();
        closeImage.color = new Color(0.08f, 0.12f, 0.18f, 1f);

        AddText(
            close.transform,
            "LABEL",
            "CLOSE",
            25,
            Vector2.zero);

        panel.SetActive(false);

        Debug.Log("[MULTIPLAYER] PROFILE_PANEL BUILT.");
    }

    private static void AddText(
        Transform parent,
        string name,
        string value,
        int size,
        Vector2 position)
    {
        GameObject obj =
            new GameObject(name, typeof(RectTransform));

        obj.transform.SetParent(parent, false);

        RectTransform rect =
            obj.GetComponent<RectTransform>();

        rect.sizeDelta = new Vector2(760f, 48f);
        rect.anchoredPosition = position;

        Text text = obj.AddComponent<Text>();

        text.text = value;
        text.fontSize = size;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Truncate;

        Font font =
            Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        if (font != null)
            text.font = font;
    }
}
