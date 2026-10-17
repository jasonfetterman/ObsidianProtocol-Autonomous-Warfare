using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class MultiplayerLobbyPanelsBuilder
{
    [MenuItem("Obsidian Protocol/Multiplayer/BUILD ALL REAL INTERACTION PANELS")]
    public static void Build()
    {
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("[MULTIPLAYER] No Canvas found.");
            return;
        }

        CreateMatchSettingsPanel(canvas.transform);

        CreateTeamPanel(canvas.transform);

        CreatePanel(
            canvas.transform,
            "DEPLOYMENT_PANEL",
            "DEPLOYMENT STAGING",
            "FINAL COMMAND CHECK",
            new[]
            {
                "DEPLOYMENT BUDGET // 10,000 DP",
                "STANDARD UNIT LIMITS",
                "NO PAY-TO-WIN ADVANTAGES",
                "READY FOR DEPLOYMENT"
            });

        CreatePanel(
            canvas.transform,
            "READY_PANEL",
            "COMMANDER STATUS",
            "READY STATE",
            new[]
            {
                "COMMANDER-01",
                "READY STATUS",
                "TOGGLE COMMANDER READINESS"
            });

        Debug.Log("[MULTIPLAYER] REAL INTERACTION PANELS BUILT.");
    }

    private static void CreateMatchSettingsPanel(Transform canvas)
    {
        Transform existing =
            canvas.Find("MATCH_SETTINGS_PANEL");

        GameObject panel;

        if (existing != null)
        {
            panel = existing.gameObject;
            ClearChildren(panel.transform);
        }
        else
        {
            panel = new GameObject(
                "MATCH_SETTINGS_PANEL",
                typeof(RectTransform),
                typeof(Image));

            panel.transform.SetParent(canvas, false);
        }

        RectTransform rect =
            panel.GetComponent<RectTransform>();

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(900, 620);
        rect.anchoredPosition = Vector2.zero;

        Image background =
            panel.GetComponent<Image>();

        background.color =
            new Color(0.025f, 0.035f, 0.05f, 0.97f);

        AddText(
            panel.transform,
            "TITLE",
            "MATCH SETTINGS",
            40,
            new Vector2(0, 240));

        AddText(
            panel.transform,
            "SUBTITLE",
            "OPERATION CONFIGURATION",
            23,
            new Vector2(0, 195));

        CreateButton(
            panel.transform,
            "RANKED_BUTTON",
            "RANKED OPERATIONS",
            new Vector2(0, 125));

        CreateButton(
            panel.transform,
            "RULESET_BUTTON",
            "STANDARD RULESET",
            new Vector2(0, 55));

        CreateButton(
            panel.transform,
            "COMMANDER_COUNT_BUTTON",
            "4 COMMANDERS",
            new Vector2(0, -15));

        AddText(
            panel.transform,
            "DEPLOYMENT_BUDGET",
            "DEPLOYMENT BUDGET // 10,000 DP",
            27,
            new Vector2(0, -85));

        AddText(
            panel.transform,
            "RANKED_VALUE",
            "RANKED",
            18,
            new Vector2(0, 125));

        AddText(
            panel.transform,
            "RULESET_VALUE",
            "STANDARD",
            18,
            new Vector2(0, 55));

        AddText(
            panel.transform,
            "COMMANDER_COUNT_VALUE",
            "4",
            18,
            new Vector2(0, -15));

        CreateButton(
            panel.transform,
            "CLOSE_BUTTON",
            "CLOSE",
            new Vector2(0, -245));

        panel.SetActive(false);

        Debug.Log(
            "[MULTIPLAYER] MATCH SETTINGS PANEL BUILT."
        );
    }

    private static void CreateTeamPanel(Transform canvas)
    {
        Transform existing =
            canvas.Find("TEAM_PANEL");

        GameObject panel;

        if (existing != null)
        {
            panel = existing.gameObject;
            ClearChildren(panel.transform);
        }
        else
        {
            panel = new GameObject(
                "TEAM_PANEL",
                typeof(RectTransform),
                typeof(Image));

            panel.transform.SetParent(canvas, false);
        }

        RectTransform rect =
            panel.GetComponent<RectTransform>();

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(900, 620);
        rect.anchoredPosition = Vector2.zero;

        Image background =
            panel.GetComponent<Image>();

        background.color =
            new Color(0.025f, 0.035f, 0.05f, 0.97f);

        AddText(
            panel.transform,
            "TITLE",
            "TEAM FORMATION",
            40,
            new Vector2(0, 240));

        AddText(
            panel.transform,
            "SUBTITLE",
            "PRIVATE MATCH COMMANDER GROUP",
            23,
            new Vector2(0, 195));

        AddText(
            panel.transform,
            "TEAM_LEADER",
            "COMMANDER-01 // TEAM LEADER",
            27,
            new Vector2(0, 125));

        AddText(
            panel.transform,
            "TEAM_SLOT_1",
            "SLOT 01 // COMMANDER-01",
            25,
            new Vector2(0, 65));

        AddText(
            panel.transform,
            "TEAM_SLOT_2",
            "SLOT 02 // OPEN",
            25,
            new Vector2(0, 15));

        AddText(
            panel.transform,
            "TEAM_SLOT_3",
            "SLOT 03 // OPEN",
            25,
            new Vector2(0, -35));

        AddText(
            panel.transform,
            "TEAM_SLOT_4",
            "SLOT 04 // OPEN",
            25,
            new Vector2(0, -85));

        AddText(
            panel.transform,
            "TEAM_CAPACITY",
            "TEAM SIZE // 1 / 8",
            22,
            new Vector2(0, -140));

        CreateButton(
            panel.transform,
            "INVITE_BUTTON",
            "INVITE COMMANDER",
            new Vector2(-150, -205));

        CreateButton(
            panel.transform,
            "LEAVE_BUTTON",
            "LEAVE TEAM",
            new Vector2(150, -205));

        CreateButton(
            panel.transform,
            "CLOSE_BUTTON",
            "CLOSE",
            new Vector2(0, -275));

        panel.SetActive(false);
    }

    private static void CreatePanel(
        Transform canvas,
        string panelName,
        string title,
        string subtitle,
        string[] lines)
    {
        Transform existing =
            canvas.Find(panelName);

        GameObject panel;

        if (existing != null)
        {
            panel = existing.gameObject;
            ClearChildren(panel.transform);
        }
        else
        {
            panel = new GameObject(
                panelName,
                typeof(RectTransform),
                typeof(Image));

            panel.transform.SetParent(canvas, false);
        }

        RectTransform rect =
            panel.GetComponent<RectTransform>();

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(900, 620);
        rect.anchoredPosition = Vector2.zero;

        Image background =
            panel.GetComponent<Image>();

        background.color =
            new Color(0.025f, 0.035f, 0.05f, 0.97f);

        AddText(
            panel.transform,
            "TITLE",
            title,
            40,
            new Vector2(0, 240));

        AddText(
            panel.transform,
            "SUBTITLE",
            subtitle,
            23,
            new Vector2(0, 195));

        float y = 125;

        foreach (string line in lines)
        {
            AddText(
                panel.transform,
                line.Replace(" ", "_"),
                line,
                27,
                new Vector2(0, y));

            y -= 58;
        }

        CreateButton(
            panel.transform,
            "CLOSE_BUTTON",
            "CLOSE",
            new Vector2(0, -245));

        panel.SetActive(false);
    }

    private static void AddText(
        Transform parent,
        string name,
        string value,
        int size,
        Vector2 position)
    {
        GameObject obj =
            new GameObject(
                name,
                typeof(RectTransform));

        obj.transform.SetParent(parent, false);

        RectTransform rect =
            obj.GetComponent<RectTransform>();

        rect.sizeDelta =
            new Vector2(760, 48);

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

        rect.sizeDelta =
            new Vector2(360, 60);

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

        AddText(
            button.transform,
            "LABEL",
            label,
            25,
            Vector2.zero);
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
