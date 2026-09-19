using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class OperationsVisualBuilder
{
    private const string SCENE_PATH =
        "Assets/Scenes/SCN-05  OPERATIONS/[HUD] OPERATIONS HUD/Operations.unity";

    private static readonly Color C_BG =
        new Color(0.008f, 0.012f, 0.018f, 1f);

    private static readonly Color C_PANEL =
        new Color(0.025f, 0.040f, 0.052f, 0.96f);

    private static readonly Color C_PANEL_DARK =
        new Color(0.012f, 0.022f, 0.030f, 0.96f);

    private static readonly Color C_LINE =
        new Color(0.12f, 0.35f, 0.40f, 0.85f);

    private static readonly Color C_CYAN =
        new Color(0.15f, 0.85f, 0.92f, 1f);

    private static readonly Color C_GREEN =
        new Color(0.20f, 0.85f, 0.48f, 1f);

    private static readonly Color C_YELLOW =
        new Color(0.95f, 0.72f, 0.18f, 1f);

    private static readonly Color C_RED =
        new Color(0.90f, 0.22f, 0.22f, 1f);

    private static readonly Color C_TEXT =
        new Color(0.86f, 0.92f, 0.94f, 1f);

    private static readonly Color C_TEXT_DIM =
        new Color(0.48f, 0.58f, 0.62f, 1f);

    private static readonly Color C_MAP =
        new Color(0.015f, 0.055f, 0.060f, 1f);

    private static Font Font
    {
        get
        {
            return Resources.GetBuiltinResource<Font>(
                "LegacyRuntime.ttf");
        }
    }

    // ============================================================
    // MENU
    // ============================================================

    [MenuItem(
        "Tools/Obsidian Protocol/Operations/BUILD OPERATIONS HUD",
        priority = 10)]
    public static void Build()
    {
        BuildScene();
    }

    // ============================================================
    // SCENE
    // ============================================================

    private static void BuildScene()
    {
        string directory =
            Path.GetDirectoryName(SCENE_PATH);

        string absoluteDirectory =
            Path.Combine(
                Directory.GetCurrentDirectory(),
                directory.Replace(
                    "/",
                    Path.DirectorySeparatorChar.ToString()));

        if (!Directory.Exists(absoluteDirectory))
            Directory.CreateDirectory(absoluteDirectory);

        BackupExistingScene();

        Scene scene =
            EditorSceneManager.NewScene(
                NewSceneSetup.EmptyScene,
                NewSceneMode.Single);

        CreateEnvironment();
        CreateEventSystem();

        GameObject canvas =
            CreateCanvas();

        BuildOperationsHUD(canvas.transform);

        EditorSceneManager.MarkSceneDirty(scene);

        bool saved =
            EditorSceneManager.SaveScene(
                scene,
                SCENE_PATH);

        if (!saved)
        {
            throw new Exception(
                "Failed to save Operations scene: " +
                SCENE_PATH);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Selection.activeObject = canvas;

        Debug.Log(
            "[Obsidian Protocol] Operations scene rebuilt successfully: " +
            SCENE_PATH);
    }

    private static void BackupExistingScene()
    {
        if (!File.Exists(SCENE_PATH))
            return;

        string backup =
            SCENE_PATH.Replace(
                ".unity",
                "_BACKUP_" +
                DateTime.Now.ToString("yyyyMMdd_HHmmss") +
                ".unity");

        File.Copy(
            SCENE_PATH,
            backup,
            false);

        AssetDatabase.Refresh();

        Debug.Log(
            "[Obsidian Protocol] Operations scene backup created: " +
            backup);
    }

    // ============================================================
    // ENVIRONMENT
    // ============================================================

    private static void CreateEnvironment()
    {
        GameObject environment =
            new GameObject("OPERATIONS_ENVIRONMENT");

        GameObject cameraObject =
            new GameObject("MainCamera");

        cameraObject.transform.SetParent(
            environment.transform,
            false);

        Camera camera =
            cameraObject.AddComponent<Camera>();

        cameraObject.tag = "MainCamera";

        camera.enabled = true;

        camera.clearFlags =
            CameraClearFlags.SolidColor;

        camera.backgroundColor =
            C_BG;

        camera.orthographic = false;
        camera.fieldOfView = 60f;
        camera.nearClipPlane = 0.1f;
        camera.farClipPlane = 2000f;
        camera.depth = 0f;

        camera.transform.position =
            new Vector3(0f, 12f, -20f);

        camera.transform.rotation =
            Quaternion.Euler(
                20f,
                0f,
                0f);

        GameObject lightObject =
            new GameObject("OperationsLight");

        lightObject.transform.SetParent(
            environment.transform,
            false);

        Light light =
            lightObject.AddComponent<Light>();

        light.type =
            LightType.Directional;

        light.intensity = 0.15f;

        light.transform.rotation =
            Quaternion.Euler(
                50f,
                -30f,
                0f);
    }

    // ============================================================
    // EVENT SYSTEM
    // ============================================================

    private static void CreateEventSystem()
    {
        GameObject eventSystem =
            new GameObject("EventSystem");

        eventSystem.AddComponent<EventSystem>();

        Type inputModuleType =
            Type.GetType(
                "UnityEngine.InputSystem.UI.InputSystemUIInputModule, Unity.InputSystem");

        if (inputModuleType != null)
        {
            eventSystem.AddComponent(
                inputModuleType);
        }
        else
        {
            eventSystem.AddComponent<
                StandaloneInputModule>();
        }
    }

    // ============================================================
    // CANVAS
    // ============================================================

    private static GameObject CreateCanvas()
    {
        GameObject canvasObject =
            new GameObject(
                "Canvas_OperationsHUD");

        Canvas canvas =
            canvasObject.AddComponent<Canvas>();

        canvas.renderMode =
            RenderMode.ScreenSpaceOverlay;

        canvas.sortingOrder = 100;

        CanvasScaler scaler =
            canvasObject.AddComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(1920f, 1080f);

        scaler.screenMatchMode =
            CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        scaler.matchWidthOrHeight = 0.5f;

        canvasObject.AddComponent<
            GraphicRaycaster>();

        return canvasObject;
    }

    // ============================================================
    // HUD
    // ============================================================

    private static void BuildOperationsHUD(
        Transform canvas)
    {
        CreatePanel(
            "BACKGROUND",
            canvas,
            0f,
            0f,
            1920f,
            1080f,
            C_BG);

        BuildTopBar(canvas);
        BuildLeftPanel(canvas);
        BuildBattlefield(canvas);
        BuildRightPanel(canvas);
        BuildBottomBar(canvas);
    }

    // ============================================================
    // TOP BAR
    // ============================================================

    private static void BuildTopBar(
        Transform root)
    {
        GameObject panel =
            CreatePanel(
                "TOP_COMMAND_BAR",
                root,
                20f,
                1000f,
                1880f,
                60f,
                C_PANEL);

        Label(
            "TITLE",
            panel.transform,
            "OBSIDIAN PROTOCOL",
            40f,
            1005f,
            1880f,
            32f,
            22,
            C_CYAN,
            TextAnchor.MiddleLeft);

        Label(
            "SUBTITLE",
            panel.transform,
            "OPERATIONS // AUTONOMOUS WARFARE",
            360f,
            1010f,
            650f,
            24f,
            12,
            C_TEXT_DIM,
            TextAnchor.MiddleLeft);

        Label(
            "MISSION",
            panel.transform,
            "OPERATION: IRON VEIL",
            1080f,
            1010f,
            300f,
            24f,
            12,
            C_TEXT,
            TextAnchor.MiddleLeft);

        Label(
            "STATUS",
            panel.transform,
            "ONLINE",
            1640f,
            1010f,
            220f,
            24f,
            12,
            C_GREEN,
            TextAnchor.MiddleRight);
    }

    // ============================================================
    // LEFT PANEL
    // ============================================================

    private static void BuildLeftPanel(
        Transform root)
    {
        GameObject panel =
            CreatePanel(
                "OPERATIONS_NAVIGATION",
                root,
                20f,
                130f,
                300f,
                850f,
                C_PANEL);

        Label(
            "HEADER",
            panel.transform,
            "OPERATIONS",
            18f,
            790f,
            264f,
            34f,
            18,
            C_CYAN,
            TextAnchor.MiddleLeft);

        string[] items =
        {
            "MISSION OVERVIEW",
            "BATTLEFIELD",
            "FORCE MANAGEMENT",
            "AI COMMAND",
            "RECONNAISSANCE",
            "LOGISTICS",
            "INTELLIGENCE",
            "DEPLOYMENT",
            "AFTER ACTION"
        };

        float y = 742f;

        for (int i = 0; i < items.Length; i++)
        {
            Color color =
                i == 0
                    ? C_CYAN
                    : C_TEXT;

            CreatePanel(
                "NAV_" + i,
                panel.transform,
                18f,
                y,
                264f,
                46f,
                i == 0
                    ? new Color(
                        0.05f,
                        0.14f,
                        0.16f,
                        1f)
                    : C_PANEL_DARK);

            Label(
                "NAV_LABEL_" + i,
                panel.transform,
                items[i],
                30f,
                y,
                236f,
                46f,
                11,
                color,
                TextAnchor.MiddleLeft);

            y -= 56f;
        }

        CreatePanel(
            "OBJECTIVES",
            panel.transform,
            18f,
            110f,
            264f,
            190f,
            C_PANEL_DARK);

        Label(
            "OBJECTIVES_TITLE",
            panel.transform,
            "MISSION OBJECTIVES",
            30f,
            260f,
            230f,
            28f,
            11,
            C_CYAN,
            TextAnchor.MiddleLeft);

        Label(
            "OBJECTIVES_TEXT",
            panel.transform,
            "01  SECURE SECTOR A\n" +
            "02  ESTABLISH SENSOR GRID\n" +
            "03  HOLD DEPLOYMENT ZONE\n" +
            "04  AWAIT COMMAND INTENT",
            30f,
            125f,
            230f,
            125f,
            10,
            C_TEXT,
            TextAnchor.UpperLeft);
    }

    // ============================================================
    // BATTLEFIELD
    // ============================================================

    private static void BuildBattlefield(
        Transform root)
    {
        GameObject panel =
            CreatePanel(
                "TACTICAL_BATTLEFIELD",
                root,
                340f,
                130f,
                1050f,
                850f,
                C_MAP);

        Label(
            "HEADER",
            panel.transform,
            "TACTICAL BATTLEFIELD",
            24f,
            790f,
            700f,
            34f,
            17,
            C_CYAN,
            TextAnchor.MiddleLeft);

        Label(
            "COORDINATES",
            panel.transform,
            "GRID  A-17 // LIVE OPERATIONS",
            700f,
            795f,
            300f,
            26f,
            10,
            C_TEXT_DIM,
            TextAnchor.MiddleRight);

        DrawGrid(
            panel.transform);

        DrawSector(
            panel.transform,
            70f,
            130f,
            360f,
            300f,
            "SECTOR A",
            C_GREEN);

        DrawSector(
            panel.transform,
            500f,
            460f,
            360f,
            260f,
            "SECTOR B",
            C_CYAN);

        DrawUnitMarker(
            panel.transform,
            270f,
            360f,
            "WARDEN",
            C_CYAN);

        DrawUnitMarker(
            panel.transform,
            620f,
            540f,
            "BULLDOG",
            C_GREEN);

        DrawUnitMarker(
            panel.transform,
            760f,
            250f,
            "SCOUT",
            C_YELLOW);

        DrawEnemyMarker(
            panel.transform,
            860f,
            610f,
            "CONTACT");

        DrawEnemyMarker(
            panel.transform,
            740f,
            180f,
            "CONTACT");

        Label(
            "MAP_STATUS",
            panel.transform,
            "SENSOR NETWORK: ONLINE    //    AUTONOMY: ACTIVE",
            40f,
            22f,
            900f,
            28f,
            10,
            C_GREEN,
            TextAnchor.MiddleLeft);
    }

    private static void DrawGrid(
        Transform parent)
    {
        for (int i = 0; i <= 10; i++)
        {
            float x =
                30f + i * 98f;

            CreatePanel(
                "GRID_V_" + i,
                parent,
                x,
                50f,
                1f,
                700f,
                new Color(
                    0.08f,
                    0.24f,
                    0.26f,
                    0.45f));
        }

        for (int i = 0; i <= 7; i++)
        {
            float y =
                50f + i * 92f;

            CreatePanel(
                "GRID_H_" + i,
                parent,
                30f,
                y,
                990f,
                1f,
                new Color(
                    0.08f,
                    0.24f,
                    0.26f,
                    0.45f));
        }
    }

    private static void DrawSector(
        Transform parent,
        float x,
        float y,
        float width,
        float height,
        string label,
        Color color)
    {
        CreatePanel(
            label,
            parent,
            x,
            y,
            width,
            height,
            new Color(
                color.r,
                color.g,
                color.b,
                0.035f));

        Label(
            label + "_LABEL",
            parent,
            label,
            x + 10f,
            y + height - 30f,
            width - 20f,
            24f,
            10,
            color,
            TextAnchor.MiddleLeft);
    }

    private static void DrawUnitMarker(
        Transform parent,
        float x,
        float y,
        string label,
        Color color)
    {
        CreatePanel(
            "UNIT_MARKER_" + label,
            parent,
            x,
            y,
            42f,
            42f,
            color);

        CreatePanel(
            "UNIT_CORE_" + label,
            parent,
            x + 11f,
            y + 11f,
            20f,
            20f,
            C_MAP);

        Label(
            "UNIT_LABEL_" + label,
            parent,
            label,
            x - 25f,
            y - 28f,
            100f,
            22f,
            9,
            color,
            TextAnchor.MiddleCenter);
    }

    private static void DrawEnemyMarker(
        Transform parent,
        float x,
        float y,
        string label)
    {
        CreatePanel(
            "ENEMY_MARKER_" + x + "_" + y,
            parent,
            x,
            y,
            30f,
            30f,
            C_RED);

        Label(
            "ENEMY_LABEL_" + x + "_" + y,
            parent,
            label,
            x - 20f,
            y - 25f,
            90f,
            20f,
            8,
            C_RED,
            TextAnchor.MiddleCenter);
    }

    // ============================================================
    // RIGHT PANEL
    // ============================================================

    private static void BuildRightPanel(
        Transform root)
    {
        GameObject panel =
            CreatePanel(
                "SELECTED_UNIT",
                root,
                1410f,
                130f,
                490f,
                850f,
                C_PANEL);

        Label(
            "HEADER",
            panel.transform,
            "SELECTED UNIT",
            22f,
            790f,
            440f,
            34f,
            17,
            C_CYAN,
            TextAnchor.MiddleLeft);

        Label(
            "UNIT_NAME",
            panel.transform,
            "WARDEN",
            22f,
            735f,
            440f,
            44f,
            24,
            C_TEXT,
            TextAnchor.MiddleLeft);

        Label(
            "UNIT_TYPE",
            panel.transform,
            "AIR // RECONNAISSANCE",
            22f,
            700f,
            440f,
            26f,
            11,
            C_TEXT_DIM,
            TextAnchor.MiddleLeft);

        CreatePanel(
            "STATUS_BOX",
            panel.transform,
            22f,
            625f,
            440f,
            55f,
            new Color(
                0.03f,
                0.14f,
                0.09f,
                1f));

        Label(
            "STATUS",
            panel.transform,
            "AUTONOMY ACTIVE",
            38f,
            635f,
            400f,
            34f,
            12,
            C_GREEN,
            TextAnchor.MiddleLeft);

        Label(
            "SPEC_HEADER",
            panel.transform,
            "SYSTEM STATUS",
            22f,
            580f,
            440f,
            28f,
            12,
            C_CYAN,
            TextAnchor.MiddleLeft);

        string[] specs =
        {
            "HULL CONDITION          100%",
            "PROPULSION               91%",
            "SENSORS                  96%",
            "ENERGY                   84%",
            "COMMUNICATION            98%",
            "AI CORE                  100%",
            "FUEL                     72%",
            "MOBILITY                 89%"
        };

        float y = 530f;

        for (int i = 0; i < specs.Length; i++)
        {
            Label(
                "SPEC_" + i,
                panel.transform,
                specs[i],
                28f,
                y,
                410f,
                30f,
                10,
                i == 6
                    ? C_YELLOW
                    : C_TEXT,
                TextAnchor.MiddleLeft);

            y -= 40f;
        }

        CreatePanel(
            "COMMAND_BOX",
            panel.transform,
            22f,
            135f,
            440f,
            190f,
            C_PANEL_DARK);

        Label(
            "COMMAND_TITLE",
            panel.transform,
            "COMMAND INTENT",
            38f,
            275f,
            400f,
            28f,
            12,
            C_CYAN,
            TextAnchor.MiddleLeft);

        Label(
            "COMMAND_TEXT",
            panel.transform,
            "RECON // HOLD POSITION\n" +
            "RULES OF ENGAGEMENT: DEFENSIVE\n" +
            "AUTONOMY: HIGH\n" +
            "FORMATION: SCOUT SCREEN",
            38f,
            160f,
            400f,
            105f,
            10,
            C_TEXT,
            TextAnchor.UpperLeft);
    }

    // ============================================================
    // BOTTOM BAR
    // ============================================================

    private static void BuildBottomBar(
        Transform root)
    {
        GameObject panel =
            CreatePanel(
                "OPERATIONS_STATUS_BAR",
                root,
                20f,
                30f,
                1880f,
                75f,
                C_PANEL);

        Label(
            "DEPLOYMENT",
            panel.transform,
            "DEPLOYMENT  10,000 / 10,000",
            25f,
            52f,
            400f,
            28f,
            13,
            C_GREEN,
            TextAnchor.MiddleLeft);

        Label(
            "ASSIGNED",
            panel.transform,
            "12 UNITS ASSIGNED",
            430f,
            52f,
            280f,
            28f,
            11,
            C_TEXT,
            TextAnchor.MiddleLeft);

        Label(
            "BUDGET",
            panel.transform,
            "BATTLE BUDGET  10,000",
            720f,
            52f,
            350f,
            28f,
            11,
            C_YELLOW,
            TextAnchor.MiddleLeft);

        Label(
            "NETWORK",
            panel.transform,
            "NETWORK: ONLINE",
            1120f,
            52f,
            260f,
            28f,
            11,
            C_GREEN,
            TextAnchor.MiddleLeft);

        Label(
            "TIMER",
            panel.transform,
            "OPERATION TIMER  02:14:36",
            1420f,
            52f,
            430f,
            28f,
            11,
            C_TEXT_DIM,
            TextAnchor.MiddleRight);
    }

    // ============================================================
    // UI HELPERS
    // ============================================================

    private static GameObject CreatePanel(
        string name,
        Transform parent,
        float x,
        float y,
        float width,
        float height,
        Color color)
    {
        GameObject go =
            new GameObject(name);

        go.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            go.AddComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0f, 0f);

        rect.anchorMax =
            new Vector2(0f, 0f);

        rect.pivot =
            new Vector2(0f, 0f);

        rect.anchoredPosition =
            new Vector2(x, y);

        rect.sizeDelta =
            new Vector2(width, height);

        Image image =
            go.AddComponent<Image>();

        image.color = color;

        return go;
    }

    private static Text Label(
        string name,
        Transform parent,
        string value,
        float x,
        float y,
        float width,
        float height,
        int size,
        Color color,
        TextAnchor alignment)
    {
        GameObject go =
            new GameObject(name);

        go.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            go.AddComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0f, 0f);

        rect.anchorMax =
            new Vector2(0f, 0f);

        rect.pivot =
            new Vector2(0f, 0f);

        rect.anchoredPosition =
            new Vector2(x, y);

        rect.sizeDelta =
            new Vector2(width, height);

        Text text =
            go.AddComponent<Text>();

        text.font =
            Font;

        text.text =
            value;

        text.fontSize =
            size;

        text.color =
            color;

        text.alignment =
            alignment;

        text.horizontalOverflow =
            HorizontalWrapMode.Wrap;

        text.verticalOverflow =
            VerticalWrapMode.Truncate;

        text.raycastTarget = false;

        return text;
    }
}
