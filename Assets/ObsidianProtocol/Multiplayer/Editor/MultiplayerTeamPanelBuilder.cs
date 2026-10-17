using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class MultiplayerTeamPanelBuilder
{
    [MenuItem("Obsidian Protocol/Multiplayer/REBUILD TEAM PANEL")]
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
            canvas.transform.Find("TEAM_PANEL");

        if (existing == null)
        {
            Debug.LogError(
                "[MULTIPLAYER] TEAM_PANEL not found.");

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
            new Vector2(900, 700);

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
            "TEAM FORMATION",
            40,
            new Vector2(0, 285),
            760);

        AddText(
            panel.transform,
            "SUBTITLE",
            "PRIVATE MATCH COMMANDER GROUP",
            23,
            new Vector2(0, 240),
            760);

        AddText(
            panel.transform,
            "TEAM_LEADER",
            "TEAM LEADER // COMMANDER-01",
            27,
            new Vector2(0, 185),
            760);

        AddText(
            panel.transform,
            "TEAM_SLOT_1",
            "SLOT 01 // COMMANDER-01",
            23,
            new Vector2(0, 135),
            760);

        AddText(
            panel.transform,
            "TEAM_SLOT_2",
            "SLOT 02 // OPEN",
            23,
            new Vector2(0, 90),
            760);

        AddText(
            panel.transform,
            "TEAM_SLOT_3",
            "SLOT 03 // OPEN",
            23,
            new Vector2(0, 45),
            760);

        AddText(
            panel.transform,
            "TEAM_SLOT_4",
            "SLOT 04 // OPEN",
            23,
            new Vector2(0, 0),
            760);

        AddText(
            panel.transform,
            "TEAM_SLOT_5",
            "SLOT 05 // OPEN",
            23,
            new Vector2(0, -45),
            760);

        AddText(
            panel.transform,
            "TEAM_SLOT_6",
            "SLOT 06 // OPEN",
            23,
            new Vector2(0, -90),
            760);

        AddText(
            panel.transform,
            "TEAM_SLOT_7",
            "SLOT 07 // OPEN",
            23,
            new Vector2(0, -135),
            760);

        AddText(
            panel.transform,
            "TEAM_SLOT_8",
            "SLOT 08 // OPEN",
            23,
            new Vector2(0, -180),
            760);

        AddText(
            panel.transform,
            "TEAM_CAPACITY",
            "TEAM SIZE // 1 / 8",
            22,
            new Vector2(0, -225),
            760);

        CreateButton(
            panel.transform,
            "INVITE_BUTTON",
            "INVITE COMMANDER",
            new Vector2(-150, -280));

        CreateButton(
            panel.transform,
            "LEAVE_BUTTON",
            "LEAVE TEAM",
            new Vector2(150, -280));

        CreateButton(
            panel.transform,
            "CLOSE_BUTTON",
            "CLOSE",
            new Vector2(0, -345));

        MultiplayerTeamPanelController controller =
            panel.GetComponent<
                MultiplayerTeamPanelController>();

        if (controller == null)
        {
            panel.AddComponent<
                MultiplayerTeamPanelController>();
        }

        panel.SetActive(false);

        EditorUtility.SetDirty(panel);

        Debug.Log(
            "[MULTIPLAYER] TEAM PANEL REBUILT.");
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
            new Vector2(280, 58);

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
                21,
                Vector2.zero,
                260);

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
            new Vector2(width, 42);

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
