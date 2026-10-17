using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public static class OPAWFinanceOfficeVisualBuilder
{
    private const string ScenePath =
        "Assets/Scenes/SCN-07  FINANCE OFFICE/[HUD] FINANCE HUD/Finance_ HUD.unity";

    private static Font Font()
    {
        return Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
    }

    [MenuItem("Obsidian Protocol/Build/FINANCE OFFICE - FULL VISUAL")]
    public static void Build()
    {
        var scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

        GameObject hud = GameObject.Find("FINANCE HUD");
        if (hud == null)
        {
            Debug.LogError("[FINANCE] FINANCE HUD not found.");
            return;
        }

        Canvas canvas = hud.GetComponent<Canvas>();
        if (canvas == null)
            canvas = hud.AddComponent<Canvas>();

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.pixelPerfect = false;

        CanvasScaler scaler = hud.GetComponent<CanvasScaler>();
        if (scaler == null)
            scaler = hud.AddComponent<CanvasScaler>();

        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        GraphicRaycaster raycaster = hud.GetComponent<GraphicRaycaster>();
        if (raycaster == null)
            hud.AddComponent<GraphicRaycaster>();

        ClearGenerated(hud.transform);

        // ============================================================
        // BACKGROUND
        // ============================================================

        GameObject background = Panel(
            hud.transform,
            "HUD BACKGROUND",
            0, 0, 1920, 1080,
            new Color32(7, 10, 15, 255)
        );

        // ============================================================
        // HEADER
        // ============================================================

        GameObject header = Panel(
            hud.transform,
            "FINANCE HEADER",
            30, 20, 1860, 90,
            new Color32(15, 21, 30, 255)
        );

        Text(header.transform, "FINANCE OFFICE",
            28, 20, 360, 42, 30, FontStyle.Bold);

        Text(header.transform,
            "COMMAND ECONOMY // FINANCIAL MANAGEMENT",
            28, 58, 620, 22, 15, FontStyle.Normal);

        GameObject status = Panel(
            header.transform,
            "STATUS",
            1510, 22, 300, 48,
            new Color32(20, 34, 39, 255)
        );

        Text(status.transform,
            "● SYSTEM ONLINE",
            15, 8, 270, 30, 16, FontStyle.Bold,
            TextAnchor.MiddleCenter);

        // ============================================================
        // LEFT COLUMN
        // ============================================================

        GameObject left = Panel(
            hud.transform,
            "ECONOMY SYSTEMS",
            30, 130, 410, 820,
            new Color32(13, 19, 27, 255)
        );

        Text(left.transform,
            "ECONOMY SYSTEMS",
            24, 20, 360, 35, 22, FontStyle.Bold);

        Text(left.transform,
            "ACTIVE FINANCIAL CHANNELS",
            24, 55, 360, 24, 13, FontStyle.Normal);

        SystemRow(left.transform, "OPERATIONS", "+1,920 / HR", 105);
        SystemRow(left.transform, "LOGISTICS", "+1,140 / HR", 175);
        SystemRow(left.transform, "RESEARCH", "+760 / HR", 245);
        SystemRow(left.transform, "FLEET MAINTENANCE", "-1,080 / HR", 315);
        SystemRow(left.transform, "MANUFACTURING", "-620 / HR", 385);
        SystemRow(left.transform, "INFRASTRUCTURE", "-380 / HR", 455);

        GameObject allocation = Panel(
            left.transform,
            "BUDGET ALLOCATION",
            20, 540, 370, 245,
            new Color32(18, 25, 35, 255)
        );

        Text(allocation.transform,
            "BUDGET ALLOCATION",
            18, 16, 330, 30, 18, FontStyle.Bold);

        AllocationRow(allocation.transform, "OPERATIONS", "38%", 58);
        AllocationRow(allocation.transform, "LOGISTICS", "24%", 94);
        AllocationRow(allocation.transform, "RESEARCH", "18%", 130);
        AllocationRow(allocation.transform, "FLEET", "12%", 166);
        AllocationRow(allocation.transform, "EXPERIMENTAL", "8%", 202);

        // ============================================================
        // CENTER COLUMN
        // ============================================================

        GameObject center = Panel(
            hud.transform,
            "FINANCIAL OVERVIEW",
            460, 130, 1000, 820,
            new Color32(13, 19, 27, 255)
        );

        Text(center.transform,
            "FINANCIAL OVERVIEW",
            24, 20, 450, 35, 22, FontStyle.Bold);

        Text(center.transform,
            "CURRENT COMMAND ECONOMY",
            24, 55, 450, 24, 13, FontStyle.Normal);

        // TOP CARDS

        Card(
            center.transform,
            "CURRENT CREDITS",
            "84,250 CR",
            20, 100, 290, 155
        );

        Card(
            center.transform,
            "NET BALANCE",
            "+2,840 / HR",
            330, 100, 290, 155
        );

        Card(
            center.transform,
            "INCOME RATE",
            "+4,920 / HR",
            640, 100, 320, 155
        );

        // SECOND ROW

        Card(
            center.transform,
            "EXPENDITURE RATE",
            "-2,080 / HR",
            20, 275, 450, 145
        );

        Card(
            center.transform,
            "FORECAST",
            "STABLE",
            490, 275, 470, 145
        );

        // DEPLOYMENT ECONOMY

        GameObject deployment = Panel(
            center.transform,
            "DEPLOYMENT ECONOMY",
            20, 445, 940, 190,
            new Color32(18, 25, 35, 255)
        );

        Text(deployment.transform,
            "DEPLOYMENT ECONOMY",
            20, 15, 400, 30, 19, FontStyle.Bold);

        Text(deployment.transform,
            "BATTLE DEPLOYMENT BUDGET",
            20, 58, 320, 25, 14, FontStyle.Normal);

        Text(deployment.transform,
            "10,000 DP",
            20, 88, 320, 38, 28, FontStyle.Bold);

        Text(deployment.transform,
            "COMPETITIVE POWER LIMIT ENFORCED",
            20, 132, 420, 24, 13, FontStyle.Normal);

        Text(deployment.transform,
            "REMAINING CREDITS",
            520, 58, 260, 25, 14, FontStyle.Normal);

        Text(deployment.transform,
            "81,850 CR",
            520, 88, 360, 38, 28, FontStyle.Bold);

        Button(
            deployment.transform,
            "OPEN DEPLOYMENT ECONOMY",
            690, 128, 220, 42
        );

        // TRANSACTIONS

        GameObject transactions = Panel(
            center.transform,
            "RECENT TRANSACTIONS",
            20, 655, 940, 145,
            new Color32(18, 25, 35, 255)
        );

        Text(transactions.transform,
            "RECENT TRANSACTIONS",
            20, 14, 300, 28, 18, FontStyle.Bold);

        Transaction(transactions.transform,
            "Fleet maintenance", "-1,080 CR", 52);

        Transaction(transactions.transform,
            "Operations revenue", "+1,920 CR", 78);

        Transaction(transactions.transform,
            "Research allocation", "-760 CR", 104);

        // ============================================================
        // RIGHT COLUMN
        // ============================================================

        GameObject right = Panel(
            hud.transform,
            "FINANCE CONTROL PANEL",
            1580, 130, 310, 820,
            new Color32(13, 19, 27, 255)
        );

        Text(right.transform,
            "FINANCE CONTROL",
            20, 20, 270, 32, 21, FontStyle.Bold);

        Text(right.transform,
            "COMMAND ACTIONS",
            20, 54, 270, 22, 13, FontStyle.Normal);

        Button(right.transform, "ADJUST BUDGET", 20, 105, 270, 52);
        Button(right.transform, "INVESTMENTS", 20, 170, 270, 52);
        Button(right.transform, "PURCHASES", 20, 235, 270, 52);
        Button(right.transform, "TRANSACTIONS", 20, 300, 270, 52);
        Button(right.transform, "FINANCIAL REPORT", 20, 365, 270, 52);

        GameObject limits = Panel(
            right.transform,
            "FINANCIAL LIMITS",
            20, 455, 270, 160,
            new Color32(18, 25, 35, 255)
        );

        Text(limits.transform,
            "FINANCIAL LIMITS",
            15, 15, 240, 25, 16, FontStyle.Bold);

        Text(limits.transform,
            "CREDIT RESERVE",
            15, 50, 150, 22, 12);

        Text(limits.transform,
            "84,250 CR",
            15, 73, 220, 27, 18, FontStyle.Bold);

        Text(limits.transform,
            "POWER LIMIT",
            15, 108, 150, 22, 12);

        Text(limits.transform,
            "ENFORCED",
            15, 131, 220, 22, 15, FontStyle.Bold);

        // ============================================================
        // BOTTOM NAVIGATION
        // ============================================================

        GameObject bottom = Panel(
            hud.transform,
            "FINANCE BOTTOM BAR",
            30, 970, 1860, 80,
            new Color32(15, 21, 30, 255)
        );

        Text(bottom.transform,
            "COMMAND NAVIGATION",
            20, 10, 220, 24, 13, FontStyle.Bold);

        Button(bottom.transform, "COMMAND CENTER", 270, 12, 190, 48);
        Button(bottom.transform, "OPERATIONS", 470, 12, 160, 48);
        Button(bottom.transform, "LOGISTICS", 640, 12, 160, 48);
        Button(bottom.transform, "GARAGE", 810, 12, 140, 48);
        Button(bottom.transform, "RESEARCH", 960, 12, 160, 48);
        Button(bottom.transform, "INTELLIGENCE", 1130, 12, 170, 48);
        Button(bottom.transform, "STORE", 1310, 12, 140, 48);

        Text(bottom.transform,
            "FINANCE",
            1510, 12, 150, 48, 17, FontStyle.Bold,
            TextAnchor.MiddleCenter);

        Button(bottom.transform, "CLOSE", 1690, 12, 140, 48);

        EnsureEventSystem();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[FINANCE] COMPLETE VISUAL REBUILD FINISHED.");
    }

    // ================================================================
    // HELPERS
    // ================================================================

    private static void ClearGenerated(Transform root)
    {
        for (int i = root.childCount - 1; i >= 0; i--)
            Object.DestroyImmediate(root.GetChild(i).gameObject);
    }

    private static GameObject Panel(
        Transform parent,
        string name,
        float x,
        float y,
        float width,
        float height,
        Color color)
    {
        GameObject go = new GameObject(name,
            typeof(RectTransform),
            typeof(Image));

        go.transform.SetParent(parent, false);

        RectTransform rt = go.GetComponent<RectTransform>();

        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1);

        rt.anchoredPosition = new Vector2(x, -y);
        rt.sizeDelta = new Vector2(width, height);

        Image image = go.GetComponent<Image>();
        image.color = color;
        image.raycastTarget = false;

        return go;
    }

    private static Text Text(
        Transform parent,
        string value,
        float x,
        float y,
        float width,
        float height,
        int size,
        FontStyle style = FontStyle.Normal,
        TextAnchor anchor = TextAnchor.UpperLeft)
    {
        GameObject go = new GameObject(
            "TEXT_" + value.Replace(" ", "_"),
            typeof(RectTransform),
            typeof(Text));

        go.transform.SetParent(parent, false);

        RectTransform rt = go.GetComponent<RectTransform>();

        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1);

        rt.anchoredPosition = new Vector2(x, -y);
        rt.sizeDelta = new Vector2(width, height);

        Text text = go.GetComponent<Text>();

        text.font = Font();
        text.text = value;
        text.fontSize = size;
        text.fontStyle = style;
        text.alignment = anchor;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Truncate;
        text.color = Color.white;
        text.raycastTarget = false;

        return text;
    }

    private static void Button(
        Transform parent,
        string label,
        float x,
        float y,
        float width,
        float height)
    {
        GameObject go = new GameObject(
            "BUTTON_" + label.Replace(" ", "_"),
            typeof(RectTransform),
            typeof(Image),
            typeof(Button));

        go.transform.SetParent(parent, false);

        RectTransform rt = go.GetComponent<RectTransform>();

        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1);

        rt.anchoredPosition = new Vector2(x, -y);
        rt.sizeDelta = new Vector2(width, height);

        Image image = go.GetComponent<Image>();
        image.color = new Color32(28, 38, 50, 255);
        image.raycastTarget = true;

        Button button = go.GetComponent<Button>();
        ColorBlock colors = button.colors;
        colors.normalColor = new Color32(28, 38, 50, 255);
        colors.highlightedColor = new Color32(48, 63, 80, 255);
        colors.pressedColor = new Color32(18, 27, 37, 255);
        button.colors = colors;

        Text(
            go.transform,
            label,
            8,
            0,
            width - 16,
            height,
            14,
            FontStyle.Bold,
            TextAnchor.MiddleCenter
        );
    }

    private static void Card(
        Transform parent,
        string title,
        string value,
        float x,
        float y,
        float width,
        float height)
    {
        GameObject card = Panel(
            parent,
            "FINANCIAL CARD " + title,
            x, y, width, height,
            new Color32(18, 25, 35, 255)
        );

        Text(
            card.transform,
            title,
            18, 16,
            width - 36, 28,
            15,
            FontStyle.Bold
        );

        Text(
            card.transform,
            value,
            18, 55,
            width - 36, 55,
            27,
            FontStyle.Bold
        );
    }

    private static void SystemRow(
        Transform parent,
        string name,
        string value,
        float y)
    {
        GameObject row = Panel(
            parent,
            "SYSTEM_" + name,
            20, y, 370, 55,
            new Color32(18, 25, 35, 255)
        );

        Text(
            row.transform,
            name,
            14, 9,
            220, 34,
            14,
            FontStyle.Bold,
            TextAnchor.MiddleLeft
        );

        Text(
            row.transform,
            value,
            220, 9,
            135, 34,
            14,
            FontStyle.Bold,
            TextAnchor.MiddleRight
        );
    }

    private static void AllocationRow(
        Transform parent,
        string name,
        string value,
        float y)
    {
        Text(
            parent,
            name,
            15, y,
            220, 25,
            13,
            FontStyle.Normal
        );

        Text(
            parent,
            value,
            250, y,
            90, 25,
            13,
            FontStyle.Bold,
            TextAnchor.MiddleRight
        );
    }

    private static void Transaction(
        Transform parent,
        string name,
        string value,
        float y)
    {
        Text(
            parent,
            name,
            20, y,
            500, 22,
            13
        );

        Text(
            parent,
            value,
            650, y,
            240, 22,
            13,
            FontStyle.Bold,
            TextAnchor.MiddleRight
        );
    }

    private static void EnsureEventSystem()
    {
        if (Object.FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>() != null)
            return;

        GameObject es = new GameObject(
            "EventSystem",
            typeof(UnityEngine.EventSystems.EventSystem),
            typeof(UnityEngine.EventSystems.StandaloneInputModule));

        Object.DontDestroyOnLoad(es);
    }
}
