using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;

public static class LogisticsBayVisualBuilder
{
    private const string SCENE_PATH =
        "Assets/Scenes/SCN-08  LOGISTICS BAY/[HUD] LOGISTICS HUD/Logistics_Bay.unity";

    private static readonly Color C_BG =
        new Color(0.018f, 0.024f, 0.032f, 1f);

    private static readonly Color C_PANEL =
        new Color(0.035f, 0.048f, 0.062f, 0.97f);

    private static readonly Color C_PANEL_DARK =
        new Color(0.022f, 0.030f, 0.040f, 0.98f);

    private static readonly Color C_PANEL_LIGHT =
        new Color(0.055f, 0.070f, 0.088f, 1f);

    private static readonly Color C_LINE =
        new Color(0.15f, 0.25f, 0.31f, 1f);

    private static readonly Color C_TEXT =
        new Color(0.82f, 0.90f, 0.94f, 1f);

    private static readonly Color C_TEXT_DIM =
        new Color(0.48f, 0.58f, 0.63f, 1f);

    private static readonly Color C_GREEN =
        new Color(0.18f, 0.85f, 0.55f, 1f);

    private static readonly Color C_CYAN =
        new Color(0.20f, 0.72f, 0.90f, 1f);

    private static readonly Color C_YELLOW =
        new Color(0.92f, 0.72f, 0.25f, 1f);

    private static readonly Color C_ORANGE =
        new Color(0.95f, 0.42f, 0.18f, 1f);

    private static readonly Color C_RED =
        new Color(0.90f, 0.24f, 0.28f, 1f);

    private static Font FONT;

    private static GameObject CanvasRoot;
    private static GameObject OverlayRoot;

    private static Text HeaderStatus;

    // ============================================================
    // ENTRY
    // ============================================================

    [MenuItem("Obsidian Protocol/Build Logistics Bay")]
    public static void Build()
    {
        EnsureFolders();

        Scene scene =
            EditorSceneManager.NewScene(
                NewSceneSetup.EmptyScene,
                NewSceneMode.Single
            );

        CreateEventSystem();
        CreateCamera();
        CreateCanvas();

        BuildBackground();
        BuildHeader();
        BuildResourceOverview();
        BuildSupplyLines();
        BuildProduction();
        BuildMaintenance();
        BuildStorage();
        BuildLogisticsLinks();
        BuildFooter();

        CreateOverlaySystem();

        SaveScene(scene);

        Selection.activeObject = CanvasRoot;

        Debug.Log(
            "====================================================\n" +
            "OBSIDIAN PROTOCOL - LOGISTICS BAY BUILT\n" +
            "Scene: " + SCENE_PATH + "\n" +
            "===================================================="
        );
    }

    // ============================================================
    // FOLDERS
    // ============================================================

    private static void EnsureFolders()
    {
        string full =
            Path.Combine(
                Directory.GetCurrentDirectory(),
                SCENE_PATH.Replace(
                    "/",
                    Path.DirectorySeparatorChar.ToString()
                )
            );

        string directory =
            Path.GetDirectoryName(full);

        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        if (!AssetDatabase.IsValidFolder(
            "Assets/ObsidianProtocol/Logistics"))
        {
            AssetDatabase.CreateFolder(
                "Assets/ObsidianProtocol",
                "Logistics"
            );
        }

        if (!AssetDatabase.IsValidFolder(
            "Assets/ObsidianProtocol/Logistics/Editor"))
        {
            AssetDatabase.CreateFolder(
                "Assets/ObsidianProtocol/Logistics",
                "Editor"
            );
        }
    }

    // ============================================================
    // CANVAS
    // ============================================================

    private static void CreateEventSystem()
    {
        GameObject es =
            new GameObject(
                "EventSystem",
                typeof(EventSystem),
                typeof(InputSystemUIInputModule)
            );

        es.transform.SetParent(null);
    }

    private static void CreateCamera()
    {
        GameObject cameraObject =
            new GameObject(
                "LOGISTICS CAMERA",
                typeof(Camera),
                typeof(AudioListener)
            );

        Camera camera =
            cameraObject.GetComponent<Camera>();

        camera.clearFlags =
            CameraClearFlags.SolidColor;

        camera.backgroundColor =
            new Color(
                0.018f,
                0.024f,
                0.032f,
                1f
            );

        camera.orthographic = true;
        camera.orthographicSize = 5f;

        camera.nearClipPlane = 0.01f;
        camera.farClipPlane = 1000f;

        camera.depth = -100f;

        cameraObject.transform.position =
            new Vector3(
                0f,
                0f,
                -10f
            );

        cameraObject.transform.rotation =
            Quaternion.identity;

        camera.tag = "MainCamera";
    }
    private static void CreateCanvas()
    {
        CanvasRoot =
            new GameObject(
                "LOGISTICS HUD",
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster)
            );

        Canvas canvas =
            CanvasRoot.GetComponent<Canvas>();

        canvas.renderMode =
            RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler =
            CanvasRoot.GetComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(1920f, 1080f);

        scaler.screenMatchMode =
            CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        scaler.matchWidthOrHeight = 0.5f;

        FONT =
            Resources.GetBuiltinResource<Font>(
                "LegacyRuntime.ttf"
            );

        if (FONT == null)
        {
            FONT =
                Resources.GetBuiltinResource<Font>(
                    "Arial.ttf"
                );
        }
    }

    // ============================================================
    // BASIC UI
    // ============================================================

    private static GameObject Panel(
        string name,
        Transform parent,
        float x,
        float y,
        float w,
        float h,
        Color color
    )
    {
        GameObject go =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image)
            );

        go.transform.SetParent(
            parent,
            false
        );

        RectTransform rt =
            go.GetComponent<RectTransform>();

        rt.anchorMin =
            new Vector2(0f, 0f);

        rt.anchorMax =
            new Vector2(0f, 0f);

        rt.pivot =
            new Vector2(0f, 0f);

        rt.anchoredPosition =
            new Vector2(x, y);

        rt.sizeDelta =
            new Vector2(w, h);

        Image image =
            go.GetComponent<Image>();

        image.color = color;

        return go;
    }

    private static Text Label(
        string name,
        Transform parent,
        string text,
        float x,
        float y,
        float w,
        float h,
        int size,
        Color color,
        TextAnchor alignment = TextAnchor.MiddleLeft
    )
    {
        GameObject go =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Text)
            );

        go.transform.SetParent(
            parent,
            false
        );

        RectTransform rt =
            go.GetComponent<RectTransform>();

        rt.anchorMin =
            new Vector2(0f, 0f);

        rt.anchorMax =
            new Vector2(0f, 0f);

        rt.pivot =
            new Vector2(0f, 0f);

        rt.anchoredPosition =
            new Vector2(x, y);

        rt.sizeDelta =
            new Vector2(w, h);

        Text t =
            go.GetComponent<Text>();

        t.font = FONT;
        t.text = text;
        t.fontSize = size;
        t.color = color;
        t.alignment = alignment;
        t.horizontalOverflow =
            HorizontalWrapMode.Wrap;
        t.verticalOverflow =
            VerticalWrapMode.Truncate;

        return t;
    }

    private static Image Line(
        string name,
        Transform parent,
        float x,
        float y,
        float w,
        float h,
        Color color
    )
    {
        GameObject go =
            Panel(
                name,
                parent,
                x,
                y,
                w,
                h,
                color
            );

        return go.GetComponent<Image>();
    }

    private static Button Button(
        string name,
        Transform parent,
        string caption,
        float x,
        float y,
        float w,
        float h,
        Action action
    )
    {
        GameObject go =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image),
                typeof(Button)
            );

        go.transform.SetParent(
            parent,
            false
        );

        RectTransform rt =
            go.GetComponent<RectTransform>();

        rt.anchorMin =
            new Vector2(0f, 0f);

        rt.anchorMax =
            new Vector2(0f, 0f);

        rt.pivot =
            new Vector2(0f, 0f);

        rt.anchoredPosition =
            new Vector2(x, y);

        rt.sizeDelta =
            new Vector2(w, h);

        Image image =
            go.GetComponent<Image>();

        image.color =
            new Color(
                0.055f,
                0.075f,
                0.090f,
                1f
            );

        Button button =
            go.GetComponent<Button>();

        ColorBlock colors =
            button.colors;

        colors.normalColor =
            new Color(
                0.055f,
                0.075f,
                0.090f,
                1f
            );

        colors.highlightedColor =
            new Color(
                0.10f,
                0.18f,
                0.21f,
                1f
            );

        colors.pressedColor =
            new Color(
                0.12f,
                0.30f,
                0.32f,
                1f
            );

        colors.selectedColor =
            colors.highlightedColor;

        button.colors = colors;

        if (action != null)
            button.onClick.AddListener(
                () => action()
            );

        Label(
            "TEXT",
            go.transform,
            caption,
            8f,
            0f,
            w - 16f,
            h,
            11,
            C_TEXT,
            TextAnchor.MiddleCenter
        );

        return button;
    }

    // ============================================================
    // BACKGROUND
    // ============================================================

    private static void BuildBackground()
    {
        GameObject bg =
            Panel(
                "BACKGROUND",
                CanvasRoot.transform,
                0f,
                0f,
                1920f,
                1080f,
                C_BG
            );

        Line(
            "TOP_GRID",
            bg.transform,
            0f,
            1010f,
            1920f,
            1f,
            C_LINE
        );

        for (int i = 0; i < 10; i++)
        {
            Line(
                "GRID_H_" + i,
                bg.transform,
                0f,
                100f + i * 80f,
                1920f,
                1f,
                new Color(
                    0.08f,
                    0.12f,
                    0.15f,
                    0.30f
                )
            );
        }

        for (int i = 0; i < 18; i++)
        {
            Line(
                "GRID_V_" + i,
                bg.transform,
                100f + i * 100f,
                0f,
                1f,
                1000f,
                new Color(
                    0.08f,
                    0.12f,
                    0.15f,
                    0.22f
                )
            );
        }
    }

    // ============================================================
    // HEADER
    // ============================================================

    private static void BuildHeader()
    {
        GameObject header =
            Panel(
                "TOP BAR",
                CanvasRoot.transform,
                24f,
                1018f,
                1872f,
                48f,
                C_PANEL_DARK
            );

        Label(
            "TITLE",
            header.transform,
            "OBSIDIAN PROTOCOL",
            16f,
            5f,
            260f,
            34f,
            18,
            C_TEXT
        );

        Label(
            "SUBTITLE",
            header.transform,
            "LOGISTICS BAY // RESOURCE COMMAND",
            280f,
            8f,
            500f,
            28f,
            11,
            C_CYAN
        );

        HeaderStatus =
            Label(
                "STATUS",
                header.transform,
                "NETWORK ONLINE  //  SUPPLY SYSTEM NOMINAL",
                1120f,
                8f,
                500f,
                28f,
                10,
                C_GREEN,
                TextAnchor.MiddleRight
            );

        Button(
            "COMMAND_CENTER",
            header.transform,
            "COMMAND",
            1635f,
            7f,
            105f,
            34f,
            () =>
            {
                ShowModal(
                    "COMMAND CENTER",
                    "COMMAND CENTER LINK\n\n" +
                    "Strategic command interface ready.\n" +
                    "Logistics network synchronized."
                );
            }
        );

        Button(
            "CLOSE",
            header.transform,
            "X",
            1750f,
            7f,
            45f,
            34f,
            () =>
            {
                ShowModal(
                    "LOGISTICS BAY",
                    "Logistics interface close requested."
                );
            }
        );
    }

    // ============================================================
    // RESOURCE OVERVIEW
    // ============================================================

    private static void BuildResourceOverview()
    {
        GameObject panel =
            Panel(
                "RESOURCE OVERVIEW",
                CanvasRoot.transform,
                24f,
                760f,
                1872f,
                245f,
                C_PANEL
            );

        Label(
            "TITLE",
            panel.transform,
            "RESOURCE OVERVIEW",
            18f,
            208f,
            400f,
            26f,
            15,
            C_TEXT
        );

        Label(
            "CAPACITY",
            panel.transform,
            "TOTAL LOGISTICS CAPACITY  68%",
            1440f,
            208f,
            390f,
            26f,
            11,
            C_GREEN,
            TextAnchor.MiddleRight
        );

        Line(
            "TITLE_LINE",
            panel.transform,
            18f,
            202f,
            1836f,
            1f,
            C_LINE
        );

        string[] names =
        {
            "MEAT",
            "WOOD",
            "COAL",
            "IRON",
            "ALLOY",
            "ELECTRONICS",
            "FUEL",
            "ENERGY"
        };

        string[] values =
        {
            "8,420",
            "12,840",
            "6,280",
            "9,640",
            "4,320",
            "3,180",
            "18,450",
            "24,780"
        };

        string[] rates =
        {
            "+120/h",
            "+240/h",
            "+80/h",
            "+160/h",
            "+65/h",
            "+45/h",
            "+380/h",
            "+520/h"
        };

        for (int i = 0; i < 8; i++)
        {
            float x =
                18f + i * 229f;

            GameObject card =
                Panel(
                    names[i],
                    panel.transform,
                    x,
                    25f,
                    215f,
                    160f,
                    C_PANEL_DARK
                );

            Label(
                "NAME",
                card.transform,
                names[i],
                12f,
                118f,
                191f,
                22f,
                10,
                C_CYAN
            );

            Label(
                "VALUE",
                card.transform,
                values[i],
                12f,
                70f,
                191f,
                42f,
                23,
                C_TEXT
            );

            Label(
                "RATE",
                card.transform,
                rates[i],
                12f,
                42f,
                191f,
                20f,
                10,
                C_GREEN
            );

            float fill =
                0.35f + i * 0.065f;

            Panel(
                "BAR_BG",
                card.transform,
                12f,
                18f,
                191f,
                8f,
                new Color(
                    0.10f,
                    0.13f,
                    0.15f,
                    1f
                )
            );

            Panel(
                "BAR",
                card.transform,
                12f,
                18f,
                191f * fill,
                8f,
                C_GREEN
            );
        }
    }

    // ============================================================
    // SUPPLY LINES
    // ============================================================

    private static void BuildSupplyLines()
    {
        GameObject panel =
            Panel(
                "SUPPLY LINES",
                CanvasRoot.transform,
                24f,
                395f,
                910f,
                345f,
                C_PANEL
            );

        Label(
            "TITLE",
            panel.transform,
            "SUPPLY LINES",
            18f,
            310f,
            300f,
            25f,
            15,
            C_TEXT
        );

        Label(
            "SUMMARY",
            panel.transform,
            "ACTIVE ROUTES  14    //    EFFICIENCY  92%    //    RISK  LOW",
            280f,
            310f,
            600f,
            25f,
            10,
            C_GREEN,
            TextAnchor.MiddleRight
        );

        Line(
            "LINE",
            panel.transform,
            18f,
            302f,
            874f,
            1f,
            C_LINE
        );

        BuildRoute(
            panel.transform,
            "NORTH DEPOT",
            "COMMAND CENTER",
            "FUEL / ENERGY",
            "98%",
            "LOW",
            245f
        );

        BuildRoute(
            panel.transform,
            "IRON MINE",
            "FABRICATION",
            "IRON / ALLOY",
            "94%",
            "LOW",
            185f
        );

        BuildRoute(
            panel.transform,
            "ELECTRONICS",
            "COMMAND CENTER",
            "ELECTRONICS",
            "89%",
            "MED",
            125f
        );

        BuildRoute(
            panel.transform,
            "FOOD STORAGE",
            "FIELD FORCES",
            "MEAT",
            "91%",
            "LOW",
            65f
        );

        Button(
            "MANAGE_ROUTES",
            panel.transform,
            "MANAGE ROUTES",
            18f,
            16f,
            170f,
            34f,
            () =>
            {
                ShowModal(
                    "LOGISTICS FLEET",
                    "ACTIVE LOGISTICS FLEET\n\n" +
                    "14 ACTIVE ROUTES\n" +
                    "38 TRANSPORT ASSETS\n" +
                    "92% NETWORK EFFICIENCY\n\n" +
                    "Routes are operating within acceptable risk."
                );
            }
        );

        Button(
            "TRANSPORT",
            panel.transform,
            "TRANSPORT",
            198f,
            16f,
            145f,
            34f,
            () =>
            {
                ShowModal(
                    "LOGISTICS FLEET",
                    "TRANSPORT FLEET\n\n" +
                    "HEAVY HAULERS       12\n" +
                    "FUEL CARRIERS       8\n" +
                    "SUPPLY TRUCKS       14\n" +
                    "RECOVERY VEHICLES   4"
                );
            }
        );

        Button(
            "DISTRIBUTION",
            panel.transform,
            "DISTRIBUTION",
            355f,
            16f,
            155f,
            34f,
            () =>
            {
                ShowModal(
                    "RESOURCE ALLOCATION",
                    "RESOURCE ALLOCATION\n\n" +
                    "FRONTLINE FORCES     42%\n" +
                    "FABRICATION          24%\n" +
                    "RESEARCH              14%\n" +
                    "RESERVE                20%"
                );
            }
        );

        Button(
            "CONSUMPTION",
            panel.transform,
            "CONSUMPTION",
            522f,
            16f,
            155f,
            34f,
            () =>
            {
                ShowModal(
                    "RESOURCE USAGE",
                    "RESOURCE USAGE\n\n" +
                    "FUEL             380 / H\n" +
                    "ENERGY           520 / H\n" +
                    "MATERIALS        240 / H\n" +
                    "ELECTRONICS       45 / H"
                );
            }
        );
    }

    private static void BuildRoute(
        Transform parent,
        string from,
        string to,
        string cargo,
        string efficiency,
        string risk,
        float y
    )
    {
        GameObject row =
            Panel(
                "ROUTE_" + from,
                parent,
                18f,
                y,
                874f,
                48f,
                C_PANEL_DARK
            );

        Label(
            "FROM",
            row.transform,
            from,
            12f,
            20f,
            160f,
            20f,
            10,
            C_TEXT
        );

        Label(
            "ARROW",
            row.transform,
            "→",
            178f,
            20f,
            30f,
            20f,
            12,
            C_CYAN,
            TextAnchor.MiddleCenter
        );

        Label(
            "TO",
            row.transform,
            to,
            215f,
            20f,
            165f,
            20f,
            10,
            C_TEXT
        );

        Label(
            "CARGO",
            row.transform,
            cargo,
            395f,
            20f,
            165f,
            20f,
            9,
            C_TEXT_DIM
        );

        Label(
            "EFF",
            row.transform,
            efficiency,
            610f,
            20f,
            75f,
            20f,
            10,
            C_GREEN,
            TextAnchor.MiddleCenter
        );

        Label(
            "RISK",
            row.transform,
            risk,
            710f,
            20f,
            80f,
            20f,
            9,
            risk == "MED" ? C_YELLOW : C_GREEN,
            TextAnchor.MiddleCenter
        );

        Line(
            "STATUS",
            row.transform,
            810f,
            20f,
            45f,
            6f,
            C_GREEN
        );
    }

    // ============================================================
    // PRODUCTION
    // ============================================================

    private static void BuildProduction()
    {
        GameObject panel =
            Panel(
                "PRODUCTION",
                CanvasRoot.transform,
                954f,
                395f,
                942f,
                345f,
                C_PANEL
            );

        Label(
            "TITLE",
            panel.transform,
            "PRODUCTION",
            18f,
            310f,
            300f,
            25f,
            15,
            C_TEXT
        );

        Label(
            "RATE",
            panel.transform,
            "MANUFACTURING RATE  86%",
            600f,
            310f,
            315f,
            25f,
            10,
            C_GREEN,
            TextAnchor.MiddleRight
        );

        Line(
            "LINE",
            panel.transform,
            18f,
            302f,
            906f,
            1f,
            C_LINE
        );

        BuildProductionRow(
            panel.transform,
            "BULLDOG",
            "GROUND UNIT",
            "FABRICATION",
            "72%",
            245f
        );

        BuildProductionRow(
            panel.transform,
            "WARDEN",
            "AIR UNIT",
            "ASSEMBLY",
            "44%",
            185f
        );

        BuildProductionRow(
            panel.transform,
            "FIELD SUPPLY",
            "LOGISTICS",
            "PACKAGING",
            "91%",
            125f
        );

        BuildProductionRow(
            panel.transform,
            "ELECTRONICS",
            "COMPONENT",
            "FABRICATION",
            "63%",
            65f
        );

        Button(
            "FABRICATION",
            panel.transform,
            "OPEN FABRICATION",
            18f,
            16f,
            190f,
            34f,
            () =>
            {
                ShowModal(
                    "FABRICATION",
                    "FABRICATION QUEUE\n\n" +
                    "BULLDOG        72%\n" +
                    "WARDEN         44%\n" +
                    "FIELD SUPPLY   91%\n" +
                    "ELECTRONICS    63%\n\n" +
                    "AVAILABLE FABRICATION CAPACITY: 86%"
                );
            }
        );

        Button(
            "CONSTRUCTION",
            panel.transform,
            "CONSTRUCTION QUEUE",
            220f,
            16f,
            190f,
            34f,
            () =>
            {
                ShowModal(
                    "CONSTRUCTION QUEUE",
                    "CONSTRUCTION QUEUE\n\n" +
                    "STORAGE EXPANSION     61%\n" +
                    "REPAIR BAY             32%\n" +
                    "POWER NODE             78%"
                );
            }
        );
    }

    private static void BuildProductionRow(
        Transform parent,
        string unit,
        string type,
        string stage,
        string progress,
        float y
    )
    {
        GameObject row =
            Panel(
                "PRODUCTION_" + unit,
                parent,
                18f,
                y,
                906f,
                48f,
                C_PANEL_DARK
            );

        Label(
            "UNIT",
            row.transform,
            unit,
            12f,
            20f,
            170f,
            20f,
            10,
            C_TEXT
        );

        Label(
            "TYPE",
            row.transform,
            type,
            190f,
            20f,
            135f,
            20f,
            9,
            C_TEXT_DIM
        );

        Label(
            "STAGE",
            row.transform,
            stage,
            345f,
            20f,
            145f,
            20f,
            9,
            C_CYAN
        );

        Label(
            "PERCENT",
            row.transform,
            progress,
            805f,
            20f,
            70f,
            20f,
            10,
            C_GREEN,
            TextAnchor.MiddleRight
        );

        Panel(
            "BAR_BG",
            row.transform,
            500f,
            20f,
            270f,
            7f,
            new Color(
                0.10f,
                0.13f,
                0.15f,
                1f
            )
        );

        float p =
            float.Parse(
                progress.TrimEnd('%')
            ) / 100f;

        Panel(
            "BAR",
            row.transform,
            500f,
            20f,
            270f * p,
            7f,
            C_CYAN
        );
    }

    // ============================================================
    // MAINTENANCE
    // ============================================================

    private static void BuildMaintenance()
    {
        GameObject panel =
            Panel(
                "MAINTENANCE",
                CanvasRoot.transform,
                24f,
                82f,
                600f,
                285f,
                C_PANEL
            );

        Label(
            "TITLE",
            panel.transform,
            "MAINTENANCE",
            18f,
            250f,
            300f,
            25f,
            15,
            C_TEXT
        );

        Label(
            "STATUS",
            panel.transform,
            "SYSTEMS NOMINAL",
            330f,
            250f,
            245f,
            25f,
            10,
            C_GREEN,
            TextAnchor.MiddleRight
        );

        Line(
            "LINE",
            panel.transform,
            18f,
            242f,
            564f,
            1f,
            C_LINE
        );

        BuildMetric(
            panel.transform,
            "REPAIR QUEUE",
            "06 UNITS",
            205f
        );

        BuildMetric(
            panel.transform,
            "RECOVERY STATUS",
            "02 ACTIVE",
            165f
        );

        BuildMetric(
            panel.transform,
            "DIAGNOSTICS",
            "98% NOMINAL",
            125f
        );

        BuildMetric(
            panel.transform,
            "REPAIR CAPACITY",
            "74%",
            85f
        );

        Button(
            "DAMAGED_UNITS",
            panel.transform,
            "DAMAGED UNITS",
            18f,
            18f,
            170f,
            34f,
            () =>
            {
                ShowModal(
                    "REPAIR QUEUE",
                    "REPAIR QUEUE\n\n" +
                    "BULLDOG-07      HULL 64%\n" +
                    "WARDEN-12       PROPULSION 71%\n" +
                    "FORGE-03        ENGINE 43%\n" +
                    "SCOUT-19        SENSORS 82%\n" +
                    "HAULER-04       MOBILITY 55%\n" +
                    "MULE-11         COMMS 91%"
                );
            }
        );

        Button(
            "REPAIR_STATIONS",
            panel.transform,
            "REPAIR STATIONS",
            198f,
            18f,
            170f,
            34f,
            () =>
            {
                ShowModal(
                    "GARAGE REPAIR",
                    "GARAGE REPAIR\n\n" +
                    "STATION 01     AVAILABLE\n" +
                    "STATION 02     BUSY\n" +
                    "STATION 03     AVAILABLE\n" +
                    "STATION 04     MAINTENANCE"
                );
            }
        );

        Button(
            "RECOVERY",
            panel.transform,
            "RECOVERY",
            378f,
            18f,
            130f,
            34f,
            () =>
            {
                ShowModal(
                    "RECOVERY OPERATIONS",
                    "RECOVERY OPERATIONS\n\n" +
                    "ACTIVE RECOVERY TEAMS  02\n" +
                    "WAITING FOR RECOVERY   01\n" +
                    "COMPLETED TODAY        08"
                );
            }
        );
    }

    private static void BuildMetric(
        Transform parent,
        string name,
        string value,
        float y
    )
    {
        Label(
            name,
            parent,
            name,
            18f,
            y,
            220f,
            25f,
            10,
            C_TEXT_DIM
        );

        Label(
            name + "_VALUE",
            parent,
            value,
            310f,
            y,
            250f,
            25f,
            11,
            C_TEXT,
            TextAnchor.MiddleRight
        );
    }

    // ============================================================
    // STORAGE
    // ============================================================

    private static void BuildStorage()
    {
        GameObject panel =
            Panel(
                "STORAGE",
                CanvasRoot.transform,
                648f,
                82f,
                600f,
                285f,
                C_PANEL
            );

        Label(
            "TITLE",
            panel.transform,
            "STORAGE",
            18f,
            250f,
            250f,
            25f,
            15,
            C_TEXT
        );

        Label(
            "CAPACITY",
            panel.transform,
            "68% CAPACITY",
            350f,
            250f,
            230f,
            25f,
            10,
            C_GREEN,
            TextAnchor.MiddleRight
        );

        Line(
            "LINE",
            panel.transform,
            18f,
            242f,
            564f,
            1f,
            C_LINE
        );

        BuildStorageBar(
            panel.transform,
            "FUEL",
            "18,450",
            0.72f,
            205f
        );

        BuildStorageBar(
            panel.transform,
            "ENERGY",
            "24,780",
            0.61f,
            165f
        );

        BuildStorageBar(
            panel.transform,
            "MATERIALS",
            "41,500",
            0.54f,
            125f
        );

        BuildStorageBar(
            panel.transform,
            "ELECTRONICS",
            "3,180",
            0.38f,
            85f
        );

        Button(
            "VIEW_STORAGE",
            panel.transform,
            "VIEW STORAGE",
            18f,
            18f,
            170f,
            34f,
            () =>
            {
                ShowModal(
                    "STORAGE HUD",
                    "STORAGE INVENTORY\n\n" +
                    "FUEL             18,450\n" +
                    "ENERGY           24,780\n" +
                    "IRON              9,640\n" +
                    "ALLOY             4,320\n" +
                    "ELECTRONICS       3,180\n" +
                    "WOOD             12,840\n" +
                    "COAL              6,280\n" +
                    "MEAT              8,420"
                );
            }
        );
    }

    private static void BuildStorageBar(
        Transform parent,
        string name,
        string value,
        float percent,
        float y
    )
    {
        Label(
            name,
            parent,
            name,
            18f,
            y,
            115f,
            22f,
            9,
            C_TEXT
        );

        Panel(
            name + "_BG",
            parent,
            135f,
            y + 6f,
            300f,
            8f,
            new Color(
                0.10f,
                0.13f,
                0.15f,
                1f
            )
        );

        Panel(
            name + "_BAR",
            parent,
            135f,
            y + 6f,
            300f * percent,
            8f,
            C_CYAN
        );

        Label(
            name + "_VALUE",
            parent,
            value,
            450f,
            y,
            125f,
            22f,
            10,
            C_TEXT,
            TextAnchor.MiddleRight
        );
    }

    // ============================================================
    // LOGISTICS LINKS
    // ============================================================

    private static void BuildLogisticsLinks()
    {
        GameObject panel =
            Panel(
                "LOGISTICS LINKS",
                CanvasRoot.transform,
                1272f,
                82f,
                624f,
                285f,
                C_PANEL
            );

        Label(
            "TITLE",
            panel.transform,
            "LOGISTICS LINKS",
            18f,
            250f,
            300f,
            25f,
            15,
            C_TEXT
        );

        Label(
            "NETWORK",
            panel.transform,
            "INTERFACE NETWORK ONLINE",
            310f,
            250f,
            295f,
            25f,
            9,
            C_GREEN,
            TextAnchor.MiddleRight
        );

        Line(
            "LINE",
            panel.transform,
            18f,
            242f,
            588f,
            1f,
            C_LINE
        );

        Button(
            "FINANCE",
            panel.transform,
            "FINANCE",
            18f,
            180f,
            275f,
            42f,
            () =>
            {
                ShowModal(
                    "FINANCE LINK",
                    "FINANCE INTERFACE\n\n" +
                    "LOGISTICS BUDGET\n" +
                    "CURRENT ALLOCATION: 74%\n" +
                    "RESERVE: 26%"
                );
            }
        );

        Button(
            "RESEARCH",
            panel.transform,
            "RESEARCH",
            305f,
            180f,
            275f,
            42f,
            () =>
            {
                ShowModal(
                    "RESEARCH LINK",
                    "RESEARCH INTERFACE\n\n" +
                    "LOGISTICS TECHNOLOGY\n" +
                    "CURRENT RESEARCH STATUS: ACTIVE"
                );
            }
        );

        Button(
            "OPERATIONS",
            panel.transform,
            "OPERATIONS",
            18f,
            125f,
            275f,
            42f,
            () =>
            {
                ShowModal(
                    "OPERATIONS LINK",
                    "OPERATIONS INTERFACE\n\n" +
                    "FIELD SUPPLY NETWORK SYNCHRONIZED.\n" +
                    "14 ACTIVE ROUTES."
                );
            }
        );

        Button(
            "COMMAND",
            panel.transform,
            "COMMAND CENTER",
            305f,
            125f,
            275f,
            42f,
            () =>
            {
                ShowModal(
                    "COMMAND CENTER LINK",
                    "COMMAND CENTER\n\n" +
                    "GLOBAL LOGISTICS STATUS\n" +
                    "NETWORK: ONLINE\n" +
                    "SUPPLY: NOMINAL\n" +
                    "PRODUCTION: NOMINAL"
                );
            }
        );

        Button(
            "STORAGE_LINK",
            panel.transform,
            "STORAGE HUD",
            18f,
            65f,
            275f,
            42f,
            () =>
            {
                ShowModal(
                    "STORAGE HUD",
                    "STORAGE NETWORK\n\n" +
                    "8 RESOURCE TYPES TRACKED\n" +
                    "CAPACITY: 68%\n" +
                    "RESERVE STATUS: HEALTHY"
                );
            }
        );

        Button(
            "MAINTENANCE_LINK",
            panel.transform,
            "MAINTENANCE OVERVIEW",
            305f,
            65f,
            275f,
            42f,
            () =>
            {
                ShowModal(
                    "MAINTENANCE OVERVIEW",
                    "MAINTENANCE NETWORK\n\n" +
                    "REPAIR QUEUE: 06\n" +
                    "RECOVERY TEAMS: 02\n" +
                    "DIAGNOSTICS: 98%"
                );
            }
        );
    }

    // ============================================================
    // FOOTER
    // ============================================================

    private static void BuildFooter()
    {
        GameObject footer =
            Panel(
                "BOTTOM BAR",
                CanvasRoot.transform,
                24f,
                24f,
                1872f,
                42f,
                C_PANEL_DARK
            );

        Label(
            "LEFT",
            footer.transform,
            "LOGISTICS NETWORK // ALL SYSTEMS SYNCHRONIZED",
            15f,
            7f,
            600f,
            28f,
            9,
            C_TEXT_DIM
        );

        Label(
            "CENTER",
            footer.transform,
            "SUPPLY EFFICIENCY 92%   •   PRODUCTION 86%   •   STORAGE 68%",
            610f,
            7f,
            650f,
            28f,
            9,
            C_GREEN,
            TextAnchor.MiddleCenter
        );

        Button(
            "ALERTS",
            footer.transform,
            "ALERTS",
            1420f,
            5f,
            105f,
            32f,
            () =>
            {
                ShowModal(
                    "LOGISTICS ALERTS",
                    "NO CRITICAL LOGISTICS ALERTS.\n\n" +
                    "1 MEDIUM PRIORITY ROUTE WARNING.\n" +
                    "ALL OTHER SYSTEMS NOMINAL."
                );
            }
        );

        Button(
            "HELP",
            footer.transform,
            "SYSTEM",
            1535f,
            5f,
            105f,
            32f,
            () =>
            {
                ShowModal(
                    "LOGISTICS SYSTEM",
                    "LOGISTICS BAY COMMAND INTERFACE\n\n" +
                    "Manage resources, supply routes,\n" +
                    "production, maintenance and storage."
                );
            }
        );

        Button(
            "PAUSE",
            footer.transform,
            "PAUSE",
            1650f,
            5f,
            105f,
            32f,
            () =>
            {
                ShowModal(
                    "PAUSE",
                    "GAME PAUSE INTERFACE\n\n" +
                    "RESUME      RETURN TO OPERATIONS\n" +
                    "SETTINGS    SYSTEM SETTINGS\n" +
                    "EXIT        RETURN TO MAIN MENU"
                );
            }
        );
    }

    // ============================================================
    // OVERLAY
    // ============================================================

    private static void CreateOverlaySystem()
    {
        OverlayRoot =
            new GameObject(
                "WINDOWS",
                typeof(RectTransform)
            );

        OverlayRoot.transform.SetParent(
            CanvasRoot.transform,
            false
        );

        RectTransform rt =
            OverlayRoot.GetComponent<RectTransform>();

        rt.anchorMin =
            new Vector2(0f, 0f);

        rt.anchorMax =
            new Vector2(0f, 0f);

        rt.pivot =
            new Vector2(0f, 0f);

        rt.anchoredPosition =
            Vector2.zero;

        rt.sizeDelta =
            new Vector2(1920f, 1080f);

        OverlayRoot.SetActive(true);
    }

    private static void ShowModal(
        string title,
        string body
    )
    {
        for (int i = OverlayRoot.transform.childCount - 1;
             i >= 0;
             i--)
        {
            UnityEngine.Object.DestroyImmediate(
                OverlayRoot.transform.GetChild(i).gameObject
            );
        }

        GameObject shade =
            Panel(
                "MODAL_SHADE",
                OverlayRoot.transform,
                0f,
                0f,
                1920f,
                1080f,
                new Color(
                    0f,
                    0f,
                    0f,
                    0.72f
                )
            );

        GameObject modal =
            Panel(
                "MODAL",
                OverlayRoot.transform,
                510f,
                260f,
                900f,
                560f,
                C_PANEL
            );

        Line(
            "TOP_ACCENT",
            modal.transform,
            0f,
            554f,
            900f,
            6f,
            C_CYAN
        );

        Label(
            "TITLE",
            modal.transform,
            title,
            30f,
            490f,
            700f,
            42f,
            20,
            C_TEXT
        );

        Label(
            "SUBTITLE",
            modal.transform,
            "LOGISTICS COMMAND INTERFACE",
            30f,
            462f,
            600f,
            22f,
            9,
            C_CYAN
        );

        Line(
            "TITLE_LINE",
            modal.transform,
            30f,
            448f,
            840f,
            1f,
            C_LINE
        );

        Label(
            "BODY",
            modal.transform,
            body,
            40f,
            145f,
            820f,
            285f,
            13,
            C_TEXT,
            TextAnchor.UpperLeft
        );

        Button(
            "CLOSE",
            modal.transform,
            "CLOSE",
            680f,
            35f,
            180f,
            42f,
            () =>
            {
                for (
                    int i = OverlayRoot.transform.childCount - 1;
                    i >= 0;
                    i--
                )
                {
                    UnityEngine.Object.DestroyImmediate(
                        OverlayRoot.transform.GetChild(i).gameObject
                    );
                }
            }
        );

        Button(
            "BACK",
            modal.transform,
            "BACK",
            480f,
            35f,
            180f,
            42f,
            () =>
            {
                for (
                    int i = OverlayRoot.transform.childCount - 1;
                    i >= 0;
                    i--
                )
                {
                    UnityEngine.Object.DestroyImmediate(
                        OverlayRoot.transform.GetChild(i).gameObject
                    );
                }
            }
        );
    }

    // ============================================================
    // SAVE
    // ============================================================

    private static void SaveScene(Scene scene)
    {
        EditorSceneManager.MarkSceneDirty(scene);

        bool saved =
            EditorSceneManager.SaveScene(
                scene,
                SCENE_PATH
            );

        if (!saved)
        {
            throw new Exception(
                "Failed to save Logistics Bay scene: " +
                SCENE_PATH
            );
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}


