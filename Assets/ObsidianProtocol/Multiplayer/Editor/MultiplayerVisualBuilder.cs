using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public static class MultiplayerVisualBuilder
{
    private static readonly Color Background =
        new Color(0.008f, 0.012f, 0.018f, 1f);

    private static readonly Color Panel =
        new Color(0.025f, 0.040f, 0.055f, 0.98f);

    private static readonly Color PanelDark =
        new Color(0.015f, 0.025f, 0.035f, 0.98f);

    private static readonly Color PanelLight =
        new Color(0.045f, 0.070f, 0.090f, 0.98f);

    private static readonly Color Border =
        new Color(0.10f, 0.25f, 0.32f, 1f);

    private static readonly Color Accent =
        new Color(0.08f, 0.72f, 0.84f, 1f);

    private static readonly Color AccentDark =
        new Color(0.03f, 0.28f, 0.34f, 1f);

    private static readonly Color Text =
        new Color(0.82f, 0.91f, 0.94f, 1f);

    private static readonly Color Muted =
        new Color(0.42f, 0.54f, 0.59f, 1f);

    private static readonly Color Green =
        new Color(0.18f, 0.82f, 0.46f, 1f);

    private static readonly Color Yellow =
        new Color(0.90f, 0.68f, 0.18f, 1f);

    private static readonly Color Red =
        new Color(0.88f, 0.22f, 0.24f, 1f);

    private static readonly Color White =
        new Color(0.95f, 0.98f, 1f, 1f);

    private static Font DefaultFont
    {
        get
        {
            return Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        }
    }

    // ============================================================
    // MENU
    // ============================================================

    [MenuItem("Obsidian Protocol/Multiplayer/Build Multiplayer HUD")]
    public static void BuildMultiplayerHUD()
    {
        string scenePath = FindMultiplayerHUDScene();

        if (string.IsNullOrEmpty(scenePath))
        {
            EditorUtility.DisplayDialog(
                "Multiplayer HUD",
                "Could not find MultiplayerHUD.unity anywhere under Assets.",
                "OK");

            Debug.LogError(
                "[MultiplayerVisualBuilder] MultiplayerHUD.unity was not found.");

            return;
        }

        Debug.Log(
            "[MultiplayerVisualBuilder] Found Multiplayer HUD scene: " +
            scenePath);

        Scene scene =
            EditorSceneManager.OpenScene(
                scenePath,
                OpenSceneMode.Single);

        if (!scene.IsValid())
        {
            EditorUtility.DisplayDialog(
                "Multiplayer HUD",
                "Unity could not open:\n\n" + scenePath,
                "OK");

            return;
        }

        BuildScene();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Selection.activeObject = GameObject.Find("MULTIPLAYER_HUD");

        Debug.Log(
            "[MultiplayerVisualBuilder] MULTIPLAYER HUD BUILD COMPLETE.\n" +
            "Scene: " + scenePath);
    }

    // ============================================================
    // FIND SCENE
    // ============================================================

    private static string FindMultiplayerHUDScene()
    {
        string[] guids =
            AssetDatabase.FindAssets(
                "MultiplayerHUD t:Scene");

        foreach (string guid in guids)
        {
            string path =
                AssetDatabase.GUIDToAssetPath(guid);

            if (string.IsNullOrEmpty(path))
                continue;

            if (path.EndsWith(
                "MultiplayerHUD.unity",
                System.StringComparison.OrdinalIgnoreCase))
            {
                return path;
            }
        }

        // Fallback: search every scene asset.
        string[] sceneGuids =
            AssetDatabase.FindAssets("t:Scene");

        foreach (string guid in sceneGuids)
        {
            string path =
                AssetDatabase.GUIDToAssetPath(guid);

            if (string.IsNullOrEmpty(path))
                continue;

            if (System.IO.Path.GetFileName(path)
                .Equals(
                    "MultiplayerHUD.unity",
                    System.StringComparison.OrdinalIgnoreCase))
            {
                return path;
            }
        }

        return null;
    }

    // ============================================================
    // BUILD SCENE
    // ============================================================

    private static void BuildScene()
    {
        GameObject old =
            GameObject.Find("MULTIPLAYER_HUD");

        if (old != null)
        {
            Object.DestroyImmediate(old);
        }

        GameObject canvasObject =
            GameObject.Find("MULTIPLAYER_CANVAS");

        if (canvasObject != null)
        {
            Object.DestroyImmediate(canvasObject);
        }

        GameObject canvasGO =
            new GameObject(
                "MULTIPLAYER_CANVAS",
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));

        Canvas canvas =
            canvasGO.GetComponent<Canvas>();

        canvas.renderMode =
            RenderMode.ScreenSpaceOverlay;

        canvas.pixelPerfect = false;

        CanvasScaler scaler =
            canvasGO.GetComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(1920f, 1080f);

        scaler.screenMatchMode =
            CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        scaler.matchWidthOrHeight = 0.5f;

        GameObject root =
            CreatePanel(
                "MULTIPLAYER_HUD",
                canvasGO.transform,
                Background,
                Vector2.zero,
                Vector2.one,
                Vector2.zero,
                Vector2.zero);

        // ========================================================
        // BACKGROUND
        // ========================================================

        CreatePanel(
            "HUD_BACKGROUND",
            root.transform,
            Background,
            Vector2.zero,
            Vector2.one,
            Vector2.zero,
            Vector2.zero);

        // ========================================================
        // TOP HEADER
        // ========================================================

        GameObject header =
            CreatePanel(
                "TOP_HEADER",
                root.transform,
                PanelDark,
                new Vector2(0f, 0.91f),
                new Vector2(1f, 1f),
                new Vector2(20f, -10f),
                new Vector2(-20f, -10f));

        CreateLabel(
            "TITLE",
            header.transform,
            "OBSIDIAN PROTOCOL",
            28,
            Text,
            TextAnchor.MiddleLeft,
            new Vector2(0f, 0.35f),
            new Vector2(0.45f, 1f),
            new Vector2(25f, 0f),
            new Vector2(0f, -5f));

        CreateLabel(
            "SUBTITLE",
            header.transform,
            "MULTIPLAYER COMMAND NETWORK",
            13,
            Accent,
            TextAnchor.MiddleLeft,
            new Vector2(0f, 0f),
            new Vector2(0.45f, 0.40f),
            new Vector2(25f, 5f),
            new Vector2(0f, 0f));

        CreateLabel(
            "NETWORK_STATUS",
            header.transform,
            "● NETWORK ONLINE",
            16,
            Green,
            TextAnchor.MiddleRight,
            new Vector2(0.72f, 0f),
            new Vector2(1f, 1f),
            new Vector2(-25f, 0f),
            new Vector2(-10f, 0f));

        // ========================================================
        // MAIN THREE-COLUMN AREA
        // ========================================================

        GameObject left =
            CreatePanel(
                "LEFT_COMMAND_COLUMN",
                root.transform,
                PanelDark,
                new Vector2(0f, 0.12f),
                new Vector2(0.31f, 0.90f),
                new Vector2(20f, 0f),
                new Vector2(-8f, -10f));

        GameObject center =
            CreatePanel(
                "CENTER_COMMAND_COLUMN",
                root.transform,
                PanelDark,
                new Vector2(0.31f, 0.12f),
                new Vector2(0.69f, 0.90f),
                new Vector2(8f, 0f),
                new Vector2(-8f, -10f));

        GameObject right =
            CreatePanel(
                "RIGHT_COMMAND_COLUMN",
                root.transform,
                PanelDark,
                new Vector2(0.69f, 0.12f),
                new Vector2(1f, 0.90f),
                new Vector2(8f, 0f),
                new Vector2(-20f, -10f));

        // ========================================================
        // LEFT — MATCHMAKING
        // ========================================================

        GameObject matchmaking =
            CreateSection(
                left.transform,
                "MATCHMAKING",
                0.75f);

        CreateButton(
            matchmaking.transform,
            "FIND MATCH",
            0.55f,
            Accent);

        CreateButton(
            matchmaking.transform,
            "QUEUE",
            0.30f,
            AccentDark);

        CreateWindow(
            matchmaking.transform,
            "MATCH SEARCH",
            new string[]
            {
                "SEARCHING FOR COMMANDERS",
                "REGION     GLOBAL",
                "LATENCY    42 ms",
                "EST. WAIT  00:24"
            },
            0.02f,
            0.27f);

        // ========================================================
        // LEFT — COMPETITIVE
        // ========================================================

        GameObject competitive =
            CreateSection(
                left.transform,
                "COMPETITIVE",
                0.54f);

        CreateButton(
            competitive.transform,
            "RANKED",
            0.55f,
            Accent);

        CreateButton(
            competitive.transform,
            "UNRANKED",
            0.30f,
            AccentDark);

        CreateWindow(
            competitive.transform,
            "COMPETITIVE RULES",
            new string[]
            {
                "DEPLOYMENT BUDGET ENFORCED",
                "STANDARD UNIT LIMITS",
                "MATCHMAKING RATING ACTIVE",
                "NO PAY-TO-WIN ADVANTAGES"
            },
            0.02f,
            0.27f);

        // ========================================================
        // LEFT — COOPERATIVE
        // ========================================================

        GameObject cooperative =
            CreateSection(
                left.transform,
                "COOPERATIVE",
                0.33f);

        CreateButton(
            cooperative.transform,
            "CO-OP OPERATIONS",
            0.55f,
            Accent);

        CreateButton(
            cooperative.transform,
            "TEAM OPERATIONS",
            0.30f,
            AccentDark);

        // ========================================================
        // LEFT — PRIVATE OPERATIONS
        // ========================================================

        GameObject privateOps =
            CreateSection(
                left.transform,
                "PRIVATE OPERATIONS",
                0.18f);

        CreateButton(
            privateOps.transform,
            "CREATE",
            0.55f,
            Accent);

        CreateButton(
            privateOps.transform,
            "INVITE",
            0.30f,
            AccentDark);

        CreateWindow(
            privateOps.transform,
            "PRIVATE RULES",
            new string[]
            {
                "INVITE ONLY",
                "CUSTOM RULESET",
                "HOST CONTROLLED"
            },
            0.02f,
            0.27f);

        // ========================================================
        // CENTER — LOBBY OVERVIEW
        // ========================================================

        GameObject lobby =
            CreateLargeSection(
                center.transform,
                "LOBBY OVERVIEW",
                0.68f);

        CreateInfoRow(
            lobby.transform,
            "PLAYER LIST",
            "COMMANDER-01     READY",
            0.78f);

        CreateInfoRow(
            lobby.transform,
            "MATCH TYPE",
            "RANKED OPERATIONS",
            0.63f);

        CreateInfoRow(
            lobby.transform,
            "MAP SELECTION",
            "BLACK MESA",
            0.48f);

        CreateButton(
            lobby.transform,
            "CREATE LOBBY",
            0.22f,
            Accent);

        // ========================================================
        // CENTER — MATCHMAKING
        // ========================================================

        GameObject centerMatch =
            CreateLargeSection(
                center.transform,
                "MATCHMAKING STATUS",
                0.40f);

        CreateInfoRow(
            centerMatch.transform,
            "QUEUE STATUS",
            "READY",
            0.78f);

        CreateInfoRow(
            centerMatch.transform,
            "ESTIMATED WAIT",
            "00:24",
            0.63f);

        CreateInfoRow(
            centerMatch.transform,
            "REGION",
            "NORTH AMERICA",
            0.48f);

        CreateButton(
            centerMatch.transform,
            "JOIN QUEUE",
            0.20f,
            Green);

        // ========================================================
        // CENTER — BATTLE BUDGET
        // ========================================================

        GameObject budget =
            CreateLargeSection(
                center.transform,
                "BATTLE BUDGET",
                0.16f);

        CreateLabel(
            "BUDGET_VALUE",
            budget.transform,
            "10,000 / 10,000 DEPLOYMENT POINTS",
            22,
            White,
            TextAnchor.MiddleCenter,
            new Vector2(0f, 0.35f),
            new Vector2(1f, 0.80f),
            new Vector2(10f, 0f),
            new Vector2(-10f, 0f));

        CreateLabel(
            "BUDGET_RULE",
            budget.transform,
            "OWNERSHIP DOES NOT INCREASE COMBAT BUDGET",
            11,
            Muted,
            TextAnchor.MiddleCenter,
            new Vector2(0f, 0f),
            new Vector2(1f, 0.35f),
            new Vector2(10f, 0f),
            new Vector2(-10f, 0f));

        // ========================================================
        // RIGHT — PLAYER PROFILE
        // ========================================================

        GameObject profile =
            CreateLargeSection(
                right.transform,
                "PLAYER PROFILE",
                0.68f);

        CreateLabel(
            "COMMANDER_NAME",
            profile.transform,
            "COMMANDER-01",
            22,
            White,
            TextAnchor.MiddleLeft,
            new Vector2(0.06f, 0.76f),
            new Vector2(0.94f, 0.96f),
            new Vector2(0f, 0f),
            new Vector2(0f, 0f));

        CreateInfoRow(
            profile.transform,
            "RANK",
            "COMMANDER",
            0.58f);

        CreateInfoRow(
            profile.transform,
            "RATING",
            "2,418",
            0.43f);

        CreateInfoRow(
            profile.transform,
            "WINS",
            "184",
            0.28f);

        CreateButton(
            profile.transform,
            "VIEW PROFILE",
            0.10f,
            Accent);

        // ========================================================
        // RIGHT — TEAM MANAGEMENT
        // ========================================================

        GameObject team =
            CreateLargeSection(
                right.transform,
                "TEAM MANAGEMENT",
                0.40f);

        CreateInfoRow(
            team.transform,
            "TEAM COMPOSITION",
            "4 / 4",
            0.78f);

        CreateInfoRow(
            team.transform,
            "ROLES",
            "COMMAND / SUPPORT / ASSAULT / RECON",
            0.61f);

        CreateInfoRow(
            team.transform,
            "READINESS",
            "ALL READY",
            0.44f);

        CreateButton(
            team.transform,
            "INVITE PLAYER",
            0.20f,
            Accent);

        // ========================================================
        // RIGHT — COMMUNICATION
        // ========================================================

        GameObject comm =
            CreateLargeSection(
                right.transform,
                "COMMUNICATION",
                0.16f);

        CreateInfoRow(
            comm.transform,
            "CHAT WINDOW",
            "ACTIVE",
            0.72f);

        CreateInfoRow(
            comm.transform,
            "VOICE CHANNEL",
            "TEAM CHANNEL",
            0.48f);

        CreateButton(
            comm.transform,
            "MUTE / UNMUTE",
            0.18f,
            AccentDark);

        // ========================================================
        // BOTTOM NAVIGATION
        // ========================================================

        GameObject nav =
            CreatePanel(
                "BOTTOM_NAVIGATION",
                root.transform,
                PanelDark,
                new Vector2(0f, 0f),
                new Vector2(1f, 0.115f),
                new Vector2(20f, 10f),
                new Vector2(-20f, -8f));

        CreateNavButton(
            nav.transform,
            "OPERATIONS",
            0.00f,
            0.16f);

        CreateNavButton(
            nav.transform,
            "FLEET",
            0.16f,
            0.32f);

        CreateNavButton(
            nav.transform,
            "GARAGE",
            0.32f,
            0.48f);

        CreateNavButton(
            nav.transform,
            "COMMAND CENTER",
            0.48f,
            0.68f);

        CreateNavButton(
            nav.transform,
            "MATCH RESULTS",
            0.68f,
            0.84f);

        CreateNavButton(
            nav.transform,
            "RANKINGS",
            0.84f,
            1.00f);

        // ========================================================
        // DECORATIVE CORNERS / STATUS
        // ========================================================

        CreateLabel(
            "BUILD_STATUS",
            root.transform,
            "MULTIPLAYER COMMAND INTERFACE // ONLINE",
            11,
            Muted,
            TextAnchor.LowerLeft,
            new Vector2(0f, 0f),
            new Vector2(0.30f, 0.06f),
            new Vector2(25f, 5f),
            new Vector2(0f, 0f));

        CreateLabel(
            "VERSION",
            root.transform,
            "OPAW // MULTIPLAYER",
            11,
            Muted,
            TextAnchor.LowerRight,
            new Vector2(0.70f, 0f),
            new Vector2(1f, 0.06f),
            new Vector2(0f, 5f),
            new Vector2(-25f, 0f));
    }

    // ============================================================
    // PANEL
    // ============================================================

    private static GameObject CreatePanel(
        string name,
        Transform parent,
        Color color,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 offsetMin,
        Vector2 offsetMax)
    {
        GameObject go =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image));

        go.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = offsetMin;
        rect.offsetMax = offsetMax;

        Image image =
            go.GetComponent<Image>();

        image.color = color;

        return go;
    }

    // ============================================================
    // SECTION
    // ============================================================

    private static GameObject CreateSection(
        Transform parent,
        string title,
        float top)
    {
        GameObject section =
            CreatePanel(
                title,
                parent,
                Panel,
                new Vector2(0f, top - 0.20f),
                new Vector2(1f, top),
                new Vector2(8f, 4f),
                new Vector2(-8f, -4f));

        CreateLabel(
            "TITLE",
            section.transform,
            title,
            15,
            Accent,
            TextAnchor.MiddleLeft,
            new Vector2(0f, 0.78f),
            new Vector2(1f, 1f),
            new Vector2(12f, 0f),
            new Vector2(-12f, 0f));

        return section;
    }

    private static GameObject CreateLargeSection(
        Transform parent,
        string title,
        float top)
    {
        GameObject section =
            CreatePanel(
                title,
                parent,
                Panel,
                new Vector2(0f, top - 0.28f),
                new Vector2(1f, top),
                new Vector2(8f, 4f),
                new Vector2(-8f, -4f));

        CreateLabel(
            "TITLE",
            section.transform,
            title,
            17,
            Accent,
            TextAnchor.MiddleLeft,
            new Vector2(0f, 0.86f),
            new Vector2(1f, 1f),
            new Vector2(14f, 0f),
            new Vector2(-14f, 0f));

        return section;
    }

    // ============================================================
    // WINDOW
    // ============================================================

    private static void CreateWindow(
        Transform parent,
        string title,
        string[] lines,
        float bottom,
        float top)
    {
        GameObject window =
            CreatePanel(
                title,
                parent,
                PanelDark,
                new Vector2(0.04f, bottom),
                new Vector2(0.96f, top),
                new Vector2(4f, 2f),
                new Vector2(-4f, -2f));

        CreateLabel(
            "WINDOW_TITLE",
            window.transform,
            title,
            10,
            Muted,
            TextAnchor.MiddleLeft,
            new Vector2(0f, 0.75f),
            new Vector2(1f, 1f),
            new Vector2(8f, 0f),
            new Vector2(-8f, 0f));

        float y = 0.57f;

        foreach (string line in lines)
        {
            CreateLabel(
                "LINE_" + line.GetHashCode(),
                window.transform,
                line,
                10,
                Text,
                TextAnchor.MiddleLeft,
                new Vector2(0f, y),
                new Vector2(1f, y + 0.18f),
                new Vector2(8f, 0f),
                new Vector2(-8f, 0f));

            y -= 0.18f;
        }
    }

    // ============================================================
    // BUTTON
    // ============================================================

    private static void CreateButton(
        Transform parent,
        string text,
        float y,
        Color color)
    {
        GameObject button =
            CreatePanel(
                "BUTTON_" + text.Replace(" ", "_"),
                parent,
                color,
                new Vector2(0.05f, y),
                new Vector2(0.95f, y + 0.12f),
                new Vector2(0f, 2f),
                new Vector2(0f, -2f));

        CreateLabel(
            "LABEL",
            button.transform,
            text,
            11,
            White,
            TextAnchor.MiddleCenter,
            Vector2.zero,
            Vector2.one,
            Vector2.zero,
            Vector2.zero);
    }

    // ============================================================
    // NAV BUTTON
    // ============================================================

    private static void CreateNavButton(
        Transform parent,
        string text,
        float minX,
        float maxX)
    {
        GameObject button =
            CreatePanel(
                "NAV_" + text.Replace(" ", "_"),
                parent,
                Panel,
                new Vector2(minX, 0.10f),
                new Vector2(maxX, 0.90f),
                new Vector2(3f, 3f),
                new Vector2(-3f, -3f));

        CreateLabel(
            "LABEL",
            button.transform,
            text,
            12,
            Text,
            TextAnchor.MiddleCenter,
            Vector2.zero,
            Vector2.one,
            Vector2.zero,
            Vector2.zero);
    }

    // ============================================================
    // INFO ROW
    // ============================================================

    private static void CreateInfoRow(
        Transform parent,
        string key,
        string value,
        float y)
    {
        GameObject row =
            CreatePanel(
                "ROW_" + key.Replace(" ", "_"),
                parent,
                PanelDark,
                new Vector2(0.04f, y),
                new Vector2(0.96f, y + 0.105f),
                new Vector2(4f, 1f),
                new Vector2(-4f, -1f));

        CreateLabel(
            "KEY",
            row.transform,
            key,
            10,
            Muted,
            TextAnchor.MiddleLeft,
            new Vector2(0f, 0f),
            new Vector2(0.40f, 1f),
            new Vector2(8f, 0f),
            new Vector2(0f, 0f));

        CreateLabel(
            "VALUE",
            row.transform,
            value,
            10,
            Text,
            TextAnchor.MiddleRight,
            new Vector2(0.40f, 0f),
            new Vector2(1f, 1f),
            new Vector2(0f, 0f),
            new Vector2(-8f, 0f));
    }

    // ============================================================
    // LABEL
    // ============================================================

    private static GameObject CreateLabel(
        string name,
        Transform parent,
        string text,
        int fontSize,
        Color color,
        TextAnchor alignment,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 offsetMin,
        Vector2 offsetMax)
    {
        GameObject go =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Text));

        go.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = offsetMin;
        rect.offsetMax = offsetMax;

        Text label =
            go.GetComponent<Text>();

        label.text = text;
        label.font = DefaultFont;
        label.fontSize = fontSize;
        label.color = color;
        label.alignment = alignment;
        label.horizontalOverflow =
            HorizontalWrapMode.Overflow;
        label.verticalOverflow =
            VerticalWrapMode.Overflow;
        label.raycastTarget = false;

        return go;
    }
}
