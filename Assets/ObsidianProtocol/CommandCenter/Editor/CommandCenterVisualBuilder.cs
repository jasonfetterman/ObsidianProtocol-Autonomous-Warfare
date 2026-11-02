using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#endif

public static class OPAWCommandCenterVisualBuilder
{
    private const float W = 1920f;
    private const float H = 1080f;

    private static Font Font;

    private static Material FloorMat;
    private static Material WallMat;
    private static Material DarkMat;
    private static Material MetalMat;
    private static Material AccentMat;
    private static Material GlassMat;

    private static readonly Color BG =
        new Color(0.004f, 0.009f, 0.014f, 1f);

    private static readonly Color Panel =
        new Color(0.010f, 0.025f, 0.036f, 0.98f);

    private static readonly Color Panel2 =
        new Color(0.016f, 0.038f, 0.052f, 0.99f);

    private static readonly Color Panel3 =
        new Color(0.025f, 0.055f, 0.072f, 1f);

    private static readonly Color Accent =
        new Color(0.05f, 0.67f, 0.95f, 1f);

    private static readonly Color AccentDim =
        new Color(0.025f, 0.22f, 0.32f, 1f);

    private static readonly Color Success =
        new Color(0.15f, 0.82f, 0.42f, 1f);

    private static readonly Color Warning =
        new Color(0.96f, 0.62f, 0.12f, 1f);

    private static readonly Color Danger =
        new Color(0.94f, 0.18f, 0.20f, 1f);

    private static readonly Color Text =
        new Color(0.88f, 0.95f, 0.98f, 1f);

    private static readonly Color DimText =
        new Color(0.42f, 0.60f, 0.68f, 1f);

    // ============================================================
    // ENTRY
    // ============================================================

    [MenuItem("Obsidian Protocol/Build/COMMAND CENTER")]
    public static void BuildCommandCenter()
    {
        string scenePath = FindScene();

        if (string.IsNullOrEmpty(scenePath))
        {
            Debug.LogError(
                "[OPAW] Command_Center.unity could not be found.");
            return;
        }

        Scene scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single);

        ClearScene();
        CreateMaterials();

        BuildWorld();
        BuildLighting();
        BuildCamera();
        BuildEventSystem();
        BuildHUD();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        GameObject hud =
            GameObject.Find("COMMAND_CENTER_HUD");

        if (hud != null)
            Selection.activeGameObject = hud;

        Debug.Log(
            "[OPAW] COMMAND CENTER BUILD COMPLETE.");
    }

    // ============================================================
    // SCENE FIND
    // ============================================================

    private static string FindScene()
    {
        string[] guids =
            AssetDatabase.FindAssets("t:Scene");

        foreach (string guid in guids)
        {
            string path =
                AssetDatabase.GUIDToAssetPath(guid);

            string n =
                Normalize(path);

            if (n.Contains("COMMANDCENTER") &&
                n.Contains("COMMANDCENTERUNITY"))
            {
                return path;
            }
        }

        foreach (string guid in guids)
        {
            string path =
                AssetDatabase.GUIDToAssetPath(guid);

            string n =
                Normalize(path);

            if (n.Contains("COMMANDCENTER"))
                return path;
        }

        return null;
    }

    private static string Normalize(string value)
    {
        string result = "";

        foreach (char c in value.ToUpperInvariant())
        {
            if (char.IsLetterOrDigit(c))
                result += c;
        }

        return result;
    }

    private static void ClearScene()
    {
        GameObject[] roots =
            SceneManager.GetActiveScene()
                .GetRootGameObjects();

        for (int i = roots.Length - 1; i >= 0; i--)
            Object.DestroyImmediate(roots[i]);
    }

    // ============================================================
    // MATERIALS
    // ============================================================

    private static void CreateMaterials()
    {
        Shader shader =
            Shader.Find("Universal Render Pipeline/Lit");

        if (shader == null)
            shader = Shader.Find("Standard");

        FloorMat =
            Mat("CC_Floor", shader,
                new Color(0.010f, 0.017f, 0.024f));

        WallMat =
            Mat("CC_Wall", shader,
                new Color(0.020f, 0.032f, 0.042f));

        DarkMat =
            Mat("CC_Dark", shader,
                new Color(0.004f, 0.008f, 0.012f));

        MetalMat =
            Mat("CC_Metal", shader,
                new Color(0.065f, 0.085f, 0.105f));

        AccentMat =
            Mat("CC_Accent", shader, Accent);

        GlassMat =
            Mat("CC_Glass", shader,
                new Color(0.018f, 0.14f, 0.21f));

        Font =
            Resources.GetBuiltinResource<Font>(
                "LegacyRuntime.ttf");
    }

    private static Material Mat(
        string name,
        Shader shader,
        Color color)
    {
        Material material =
            new Material(shader);

        material.name = name;

        if (material.HasProperty("_BaseColor"))
            material.SetColor("_BaseColor", color);

        if (material.HasProperty("_Color"))
            material.SetColor("_Color", color);

        return material;
    }

    // ============================================================
    // WORLD
    // ============================================================

    private static void BuildWorld()
    {
        GameObject root =
            Empty("COMMAND_CENTER_WORLD");

        Cube(
            "COMMAND_FLOOR",
            root.transform,
            new Vector3(0, -0.15f, 0),
            new Vector3(38, 0.3f, 28),
            FloorMat);

        Cube(
            "BACK_WALL",
            root.transform,
            new Vector3(0, 6, 12.5f),
            new Vector3(38, 12, 0.5f),
            WallMat);

        Cube(
            "LEFT_WALL",
            root.transform,
            new Vector3(-18.75f, 6, 0),
            new Vector3(0.5f, 12, 27),
            WallMat);

        Cube(
            "RIGHT_WALL",
            root.transform,
            new Vector3(18.75f, 6, 0),
            new Vector3(0.5f, 12, 27),
            WallMat);

        Cube(
            "CEILING",
            root.transform,
            new Vector3(0, 12, 0),
            new Vector3(38, 0.3f, 28),
            DarkMat);

        BuildCommandCore(root.transform);
        BuildCommanderStation(root.transform);
        BuildStrategicMap(root.transform);
        BuildServerBanks(root.transform);
        BuildWallDisplays(root.transform);
        BuildCeiling(root.transform);
        BuildFloorDetails(root.transform);
    }

    private static void BuildCommandCore(Transform parent)
    {
        GameObject root =
            Empty("CENTRAL_COMMAND_CORE", parent);

        Cylinder(
            "CORE_BASE",
            root.transform,
            new Vector3(0, 0.35f, 2.5f),
            new Vector3(5.5f, 0.35f, 5.5f),
            MetalMat);

        Cylinder(
            "CORE_PLATFORM",
            root.transform,
            new Vector3(0, 0.72f, 2.5f),
            new Vector3(4.6f, 0.25f, 4.6f),
            DarkMat);

        Cylinder(
            "CORE_HOLOGRAPHIC_COLUMN",
            root.transform,
            new Vector3(0, 3.6f, 2.5f),
            new Vector3(2.2f, 5.4f, 2.2f),
            GlassMat);

        Cube(
            "CORE_LIGHT_X",
            root.transform,
            new Vector3(0, 2.5f, 2.5f),
            new Vector3(5.8f, 0.05f, 0.05f),
            AccentMat);

        Cube(
            "CORE_LIGHT_Z",
            root.transform,
            new Vector3(0, 2.5f, 2.5f),
            new Vector3(0.05f, 0.05f, 5.8f),
            AccentMat);
    }

    private static void BuildCommanderStation(Transform parent)
    {
        GameObject root =
            Empty("COMMANDER_STATION", parent);

        Cube(
            "COMMAND_DESK",
            root.transform,
            new Vector3(0, 1.15f, -5),
            new Vector3(14, 1, 2.4f),
            MetalMat);

        Cube(
            "COMMAND_DESK_TOP",
            root.transform,
            new Vector3(0, 1.72f, -5),
            new Vector3(14.5f, 0.12f, 2.65f),
            DarkMat);

        Cube(
            "COMMAND_DESK_ACCENT",
            root.transform,
            new Vector3(0, 1.84f, -5),
            new Vector3(11.5f, 0.04f, 0.04f),
            AccentMat);

        for (int i = -2; i <= 2; i++)
        {
            Cube(
                "COMMAND_CONSOLE_" + i,
                root.transform,
                new Vector3(i * 2.35f, 1.92f, -5),
                new Vector3(1.65f, 0.08f, 1.1f),
                GlassMat);
        }
    }

    private static void BuildStrategicMap(Transform parent)
    {
        GameObject root =
            Empty("STRATEGIC_MAP_TABLE", parent);

        Cube(
            "MAP_TABLE",
            root.transform,
            new Vector3(0, 1.25f, 7),
            new Vector3(15, 1, 5.5f),
            MetalMat);

        Cube(
            "MAP_SURFACE",
            root.transform,
            new Vector3(0, 1.8f, 7),
            new Vector3(14.2f, 0.08f, 4.7f),
            GlassMat);

        for (int x = -6; x <= 6; x++)
        {
            Cube(
                "MAP_GRID_X_" + x,
                root.transform,
                new Vector3(
                    x * 1.05f,
                    1.86f,
                    7),
                new Vector3(
                    0.012f,
                    0.012f,
                    4.25f),
                AccentMat);
        }

        for (int z = -3; z <= 3; z++)
        {
            Cube(
                "MAP_GRID_Z_" + z,
                root.transform,
                new Vector3(
                    0,
                    1.86f,
                    7 + z * 0.60f),
                new Vector3(
                    13.8f,
                    0.012f,
                    0.012f),
                AccentMat);
        }
    }

    private static void BuildServerBanks(Transform parent)
    {
        GameObject root =
            Empty("COMMAND_SERVER_BANKS", parent);

        for (int side = -1; side <= 1; side += 2)
        {
            for (int i = 0; i < 5; i++)
            {
                Cube(
                    "SERVER_RACK_" + side + "_" + i,
                    root.transform,
                    new Vector3(
                        side * (12.0f + i * 0.85f),
                        3.4f,
                        5.0f),
                    new Vector3(
                        0.55f,
                        6.2f,
                        3.2f),
                    DarkMat);

                Cube(
                    "SERVER_LIGHT_" + side + "_" + i,
                    root.transform,
                    new Vector3(
                        side * (11.68f + i * 0.85f),
                        4.0f,
                        3.38f),
                    new Vector3(
                        0.02f,
                        0.06f,
                        1.8f),
                    AccentMat);
            }
        }
    }

    private static void BuildWallDisplays(Transform parent)
    {
        GameObject root =
            Empty("WALL_DISPLAY_NETWORK", parent);

        Cube(
            "LEFT_DISPLAY",
            root.transform,
            new Vector3(-12.5f, 6.7f, 12.15f),
            new Vector3(7.0f, 5.0f, 0.12f),
            GlassMat);

        Cube(
            "CENTER_DISPLAY",
            root.transform,
            new Vector3(0, 7.6f, 12.15f),
            new Vector3(12.0f, 5.6f, 0.12f),
            GlassMat);

        Cube(
            "RIGHT_DISPLAY",
            root.transform,
            new Vector3(12.5f, 6.7f, 12.15f),
            new Vector3(7.0f, 5.0f, 0.12f),
            GlassMat);
    }

    private static void BuildCeiling(Transform parent)
    {
        GameObject root =
            Empty("CEILING_STRUCTURE", parent);

        for (int x = -3; x <= 3; x++)
        {
            Cube(
                "BEAM_X_" + x,
                root.transform,
                new Vector3(
                    x * 5,
                    11.5f,
                    0),
                new Vector3(
                    0.22f,
                    0.22f,
                    25),
                MetalMat);
        }

        for (int z = -2; z <= 2; z++)
        {
            Cube(
                "BEAM_Z_" + z,
                root.transform,
                new Vector3(
                    0,
                    11.35f,
                    z * 5),
                new Vector3(
                    37,
                    0.22f,
                    0.22f),
                MetalMat);
        }
    }

    private static void BuildFloorDetails(Transform parent)
    {
        GameObject root =
            Empty("FLOOR_DETAILS", parent);

        Cube(
            "FLOOR_ACCENT_FRONT",
            root.transform,
            new Vector3(0, 0.02f, -10),
            new Vector3(30, 0.03f, 0.08f),
            AccentMat);

        Cube(
            "FLOOR_ACCENT_LEFT",
            root.transform,
            new Vector3(-14, 0.02f, 0),
            new Vector3(0.08f, 0.03f, 20),
            AccentMat);

        Cube(
            "FLOOR_ACCENT_RIGHT",
            root.transform,
            new Vector3(14, 0.02f, 0),
            new Vector3(0.08f, 0.03f, 20),
            AccentMat);
    }

    // ============================================================
    // LIGHTING
    // ============================================================

    private static void BuildLighting()
    {
        GameObject root =
            Empty("COMMAND_CENTER_LIGHTING");

        GameObject directionalObject =
            Empty(
                "MAIN_DIRECTIONAL_LIGHT",
                root.transform);

        directionalObject.transform.rotation =
            Quaternion.Euler(45, -30, 0);

        Light directional =
            directionalObject.AddComponent<Light>();

        directional.type =
            LightType.Directional;

        directional.intensity =
            0.65f;

        directional.shadows =
            LightShadows.Soft;

        PointLight(
            root.transform,
            "CORE_LIGHT",
            new Vector3(0, 5, 3),
            15,
            5);

        PointLight(
            root.transform,
            "LEFT_LIGHT",
            new Vector3(-10, 5, 4),
            12,
            3);

        PointLight(
            root.transform,
            "RIGHT_LIGHT",
            new Vector3(10, 5, 4),
            12,
            3);
    }

    private static void PointLight(
        Transform parent,
        string name,
        Vector3 position,
        float range,
        float intensity)
    {
        GameObject obj =
            Empty(name, parent);

        obj.transform.position =
            position;

        Light light =
            obj.AddComponent<Light>();

        light.type =
            LightType.Point;

        light.range =
            range;

        light.intensity =
            intensity;
    }

    // ============================================================
    // CAMERA
    // ============================================================

    private static void BuildCamera()
    {
        GameObject obj =
            new GameObject(
                "COMMAND_CENTER_CAMERA");

        Camera camera =
            obj.AddComponent<Camera>();

        obj.tag =
            "MainCamera";

        camera.fieldOfView =
            62f;

        camera.nearClipPlane =
            0.1f;

        camera.farClipPlane =
            1000f;

        obj.transform.position =
            new Vector3(
                0,
                6.2f,
                -21f);

        obj.transform.rotation =
            Quaternion.Euler(
                8f,
                0f,
                0f);
    }

    // ============================================================
    // EVENT SYSTEM
    // ============================================================

    private static void BuildEventSystem()
    {
        GameObject obj =
            Empty("EVENT_SYSTEM");

        obj.AddComponent<EventSystem>();

#if ENABLE_INPUT_SYSTEM
        obj.AddComponent<InputSystemUIInputModule>();
#else
        obj.AddComponent<StandaloneInputModule>();
#endif
    }

    // ============================================================
    // HUD ROOT
    // ============================================================

    private static void BuildHUD()
    {
        GameObject hud =
            CanvasRoot(
                "COMMAND_CENTER_HUD");

        CreateFullPanel(
            "BACKGROUND",
            hud.transform,
            BG);

        BuildTopBar(hud.transform);
        BuildLeftNavigation(hud.transform);
        BuildMainCenter(hud.transform);
        BuildRightStatus(hud.transform);
        BuildBottomNavigation(hud.transform);
        BuildWindows(hud.transform);
    }

    // ============================================================
    // TOP COMMAND BAR
    // ============================================================

    private static void BuildTopBar(Transform parent)
    {
        GameObject bar =
            AnchoredPanel(
                "TOP_COMMAND_BAR",
                parent,
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(0, -82),
                Vector2.zero,
                Panel);

        LabelStretch(
            "TITLE",
            bar.transform,
            "02. COMMAND CENTER",
            new Vector2(0, 0.5f),
            new Vector2(0, 1),
            new Vector2(25, -29),
            new Vector2(390, 29),
            27,
            Text,
            TextAnchor.MiddleLeft);

        LabelStretch(
            "SUBTITLE",
            bar.transform,
            "OBSIDIAN PROTOCOL  //  COMMAND AUTHORITY",
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(400, 10),
            new Vector2(750, -10),
            11,
            DimText,
            TextAnchor.MiddleLeft);

        Label(
            "COMMANDER",
            bar.transform,
            "COMMANDER",
            new Vector2(0.43f, 0.5f),
            Vector2.zero,
            new Vector2(110, 32),
            12,
            DimText,
            TextAnchor.MiddleCenter);

        Label(
            "COMMANDER_VALUE",
            bar.transform,
            "OPERATOR",
            new Vector2(0.49f, 0.5f),
            Vector2.zero,
            new Vector2(125, 32),
            13,
            Text,
            TextAnchor.MiddleCenter);

        Label(
            "RANK",
            bar.transform,
            "RANK  //  COMMANDER",
            new Vector2(0.60f, 0.5f),
            Vector2.zero,
            new Vector2(180, 32),
            12,
            Accent,
            TextAnchor.MiddleCenter);

        Label(
            "CREDITS",
            bar.transform,
            "CREDITS  14,000",
            new Vector2(0.72f, 0.5f),
            Vector2.zero,
            new Vector2(170, 32),
            13,
            Text,
            TextAnchor.MiddleCenter);

        Label(
            "RESOURCES",
            bar.transform,
            "RESOURCES  82%",
            new Vector2(0.82f, 0.5f),
            Vector2.zero,
            new Vector2(170, 32),
            13,
            Success,
            TextAnchor.MiddleCenter);

        Label(
            "ALERTS",
            bar.transform,
            "ALERTS  03",
            new Vector2(0.94f, 0.5f),
            Vector2.zero,
            new Vector2(150, 32),
            13,
            Warning,
            TextAnchor.MiddleCenter);

        HorizontalLine(
            "TOP_ACCENT_LINE",
            bar.transform,
            Accent);
    }

    // ============================================================
    // LEFT NAVIGATION
    // ============================================================

    private static void BuildLeftNavigation(Transform parent)
    {
        GameObject panel =
            AnchoredPanel(
                "LEFT_NAVIGATION",
                parent,
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(0, 82),
                new Vector2(270, -82),
                Panel);

        LabelStretch(
            "HEADER",
            panel.transform,
            "COMMAND SYSTEMS",
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(15, -70),
            new Vector2(-15, -25),
            15,
            Accent,
            TextAnchor.MiddleCenter);

        string[] items =
        {
            "COMMAND",
            "FLEET",
            "INTELLIGENCE",
            "LOGISTICS",
            "RESEARCH",
            "OPERATIONS",
            "DEPLOY",
            "GARAGE",
            "STORE",
            "FINANCE"
        };

        for (int i = 0; i < items.Length; i++)
        {
            float top =
                -88f - (i * 61f);

            GameObject button =
                NavigationButton(
                    items[i],
                    panel.transform,
                    items[i],
                    i == 0);

            RectTransform r =
                button.GetComponent<RectTransform>();

            r.anchorMin =
                new Vector2(0, 1);

            r.anchorMax =
                new Vector2(1, 1);

            r.offsetMin =
                new Vector2(
                    14,
                    top - 48);

            r.offsetMax =
                new Vector2(
                    -14,
                    top);
        }
    }

    // ============================================================
    // MAIN CENTER
    // ============================================================

    private static void BuildMainCenter(Transform parent)
    {
        GameObject center =
            AnchoredPanel(
                "CENTER_CONTENT",
                parent,
                new Vector2(0, 0),
                new Vector2(1, 1),
                new Vector2(270, 82),
                new Vector2(-280, -82),
                BG);

        LabelStretch(
            "CENTER_TITLE",
            center.transform,
            "COMMAND OVERVIEW",
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(22, -70),
            new Vector2(-22, -22),
            23,
            Text,
            TextAnchor.MiddleLeft);

        LabelStretch(
            "CENTER_STATUS",
            center.transform,
            "ALL COMMAND SYSTEMS NOMINAL  //  AUTONOMY NETWORK ONLINE",
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(25, -98),
            new Vector2(-25, -72),
            11,
            Success,
            TextAnchor.MiddleLeft);

        ContentPanel(
            "ACTIVE_OPERATIONS",
            center.transform,
            "ACTIVE OPERATIONS",
            "IRON HORIZON",
            "SECURE SECTOR // AUTONOMOUS OPERATION",
            "24 UNITS",
            Accent,
            new Vector2(0.02f, 0.56f),
            new Vector2(0.49f, 0.89f));

        ContentPanel(
            "FLEET_STATUS",
            center.transform,
            "FLEET STATUS",
            "FLEET READINESS",
            "AIR  92%     GROUND  87%     NAVAL  81%",
            "READY",
            Success,
            new Vector2(0.51f, 0.56f),
            new Vector2(0.98f, 0.89f));

        ContentPanel(
            "INTELLIGENCE_STATUS",
            center.transform,
            "INTELLIGENCE STATUS",
            "THREAT NETWORK",
            "ACTIVE CONTACTS  012     HIGH RISK  003",
            "MONITORING",
            Warning,
            new Vector2(0.02f, 0.29f),
            new Vector2(0.49f, 0.53f));

        ContentPanel(
            "RESOURCE_STATUS",
            center.transform,
            "RESOURCE STATUS",
            "LOGISTICS RESERVE",
            "IRON  72%     ALLOY  64%     ELECTRONICS  81%",
            "STABLE",
            Success,
            new Vector2(0.51f, 0.29f),
            new Vector2(0.98f, 0.53f));

        ContentPanel(
            "RESEARCH_STATUS",
            center.transform,
            "RESEARCH STATUS",
            "ACTIVE RESEARCH",
            "AUTONOMOUS COORDINATION // 68% COMPLETE",
            "IN PROGRESS",
            Accent,
            new Vector2(0.02f, 0.02f),
            new Vector2(0.49f, 0.26f));

        ContentPanel(
            "DEPLOYMENT_STATUS",
            center.transform,
            "DEPLOYMENT STATUS",
            "BATTLE BUDGET",
            "AVAILABLE DEPLOYMENT  10,000 / 10,000",
            "READY",
            Success,
            new Vector2(0.51f, 0.02f),
            new Vector2(0.98f, 0.26f));
    }

    private static void ContentPanel(
        string name,
        Transform parent,
        string title,
        string primary,
        string detail,
        string status,
        Color accent,
        Vector2 anchorMin,
        Vector2 anchorMax)
    {
        GameObject panel =
            AnchoredPanel(
                name,
                parent,
                anchorMin,
                anchorMax,
                new Vector2(8, 8),
                new Vector2(-8, -8),
                Panel);

        ImageBorder(
            "ACCENT",
            panel.transform,
            accent);

        LabelStretch(
            "TITLE",
            panel.transform,
            title,
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(18, -48),
            new Vector2(-18, -12),
            13,
            accent,
            TextAnchor.MiddleLeft);

        LabelStretch(
            "PRIMARY",
            panel.transform,
            primary,
            new Vector2(0, 0.5f),
            new Vector2(1, 0.8f),
            new Vector2(20, 0),
            new Vector2(-20, 0),
            20,
            Text,
            TextAnchor.MiddleLeft);

        LabelStretch(
            "DETAIL",
            panel.transform,
            detail,
            new Vector2(0, 0.25f),
            new Vector2(1, 0.5f),
            new Vector2(20, 0),
            new Vector2(-20, 0),
            11,
            DimText,
            TextAnchor.MiddleLeft);

        LabelStretch(
            "STATUS",
            panel.transform,
            status,
            new Vector2(0, 0),
            new Vector2(1, 0.25f),
            new Vector2(20, 4),
            new Vector2(-20, 0),
            12,
            accent,
            TextAnchor.MiddleLeft);
    }

    // ============================================================
    // RIGHT STATUS
    // ============================================================

    private static void BuildRightStatus(Transform parent)
    {
        GameObject panel =
            AnchoredPanel(
                "RIGHT_COMMAND_PANEL",
                parent,
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(-280, 82),
                new Vector2(0, -82),
                Panel);

        LabelStretch(
            "HEADER",
            panel.transform,
            "COMMAND STATUS",
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(15, -70),
            new Vector2(-15, -25),
            15,
            Accent,
            TextAnchor.MiddleCenter);

        StatusBox(
            "COMMAND_ALERT",
            panel.transform,
            "COMMAND ALERT",
            "Priority command requires review.",
            Warning,
            -90);

        StatusBox(
            "UNIT_ALERT",
            panel.transform,
            "UNIT ALERT",
            "BULLDOG-017\nMobility damaged.",
            Danger,
            -240);

        StatusBox(
            "MISSION_ALERT",
            panel.transform,
            "MISSION ALERT",
            "IRON HORIZON\nAwaiting command review.",
            Accent,
            -390);

        StatusBox(
            "COMMUNICATION_STATUS",
            panel.transform,
            "COMMUNICATIONS",
            "NETWORK ONLINE\nSIGNAL 98%",
            Success,
            -540);

        StatusBox(
            "SYSTEM_STATUS",
            panel.transform,
            "SYSTEM STATUS",
            "AI CORE ONLINE\nFLEET LINK ONLINE\nLOGISTICS ONLINE",
            Success,
            -690);
    }

    private static void StatusBox(
        string name,
        Transform parent,
        string title,
        string message,
        Color accent,
        float y)
    {
        GameObject box =
            AnchoredPanel(
                name,
                parent,
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(12, y - 132),
                new Vector2(-12, y),
                Panel2);

        ImageBorder(
            "ACCENT",
            box.transform,
            accent);

        LabelStretch(
            "TITLE",
            box.transform,
            title,
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(12, -38),
            new Vector2(-12, -8),
            12,
            accent,
            TextAnchor.MiddleCenter);

        LabelStretch(
            "MESSAGE",
            box.transform,
            message,
            Vector2.zero,
            new Vector2(1, 1),
            new Vector2(15, 12),
            new Vector2(-15, -43),
            11,
            Text,
            TextAnchor.MiddleCenter);

        GameObject view =
            Button(
                "VIEW",
                box.transform,
                "VIEW DETAILS",
                accent);

        RectTransform vr =
            view.GetComponent<RectTransform>();

        vr.anchorMin =
            new Vector2(0.5f, 0);

        vr.anchorMax =
            new Vector2(0.5f, 0);

        vr.pivot =
            new Vector2(0.5f, 0);

        vr.anchoredPosition =
            new Vector2(0, 8);

        vr.sizeDelta =
            new Vector2(125, 25);
    }

    // ============================================================
    // BOTTOM NAVIGATION
    // ============================================================

    private static void BuildBottomNavigation(
        Transform parent)
    {
        GameObject bar =
            AnchoredPanel(
                "BOTTOM_NAVIGATION",
                parent,
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(270, 0),
                new Vector2(-280, 78),
                Panel);

        Label(
            "STATUS",
            bar.transform,
            "COMMAND NETWORK  //  ONLINE",
            new Vector2(0, 0.5f),
            new Vector2(25, 0),
            new Vector2(280, 35),
            11,
            Success,
            TextAnchor.MiddleLeft);

        BottomButton(
            "MAP",
            bar.transform,
            -190,
            Accent);

        BottomButton(
            "GARAGE",
            bar.transform,
            -45,
            Accent);

        BottomButton(
            "DEPLOY",
            bar.transform,
            100,
            Success);

        BottomButton(
            "PAUSE",
            bar.transform,
            245,
            Warning);
    }

    private static void BottomButton(
        string text,
        Transform parent,
        float x,
        Color accent)
    {
        GameObject button =
            Button(
                text,
                parent,
                text,
                accent);

        RectTransform r =
            button.GetComponent<RectTransform>();

        r.anchorMin =
            new Vector2(0.5f, 0.5f);

        r.anchorMax =
            new Vector2(0.5f, 0.5f);

        r.pivot =
            new Vector2(0.5f, 0.5f);

        r.anchoredPosition =
            new Vector2(x, 0);

        r.sizeDelta =
            new Vector2(125, 42);
    }

    // ============================================================
    // WINDOWS
    // ============================================================

    private static void BuildWindows(
        Transform parent)
    {
        Popup(
            "STRATEGIC_OVERVIEW",
            parent,
            "STRATEGIC OVERVIEW",
            "WORLD STATUS          STABLE\n\n" +
            "ACTIVE THREATS        012\n" +
            "STRATEGIC OBJECTIVES  008\n" +
            "MILITARY STATUS       OPERATIONAL\n" +
            "AUTONOMY NETWORK      ONLINE");

        Popup(
            "OPERATION_DETAILS",
            parent,
            "OPERATION DETAILS",
            "IRON HORIZON\n\n" +
            "STATUS       ACTIVE\n" +
            "COMMAND      AUTONOMOUS\n" +
            "OBJECTIVE    SECURE SECTOR\n" +
            "FORCE STATUS 87%\n" +
            "THREAT LEVEL MODERATE");

        Popup(
            "ALERT_DETAILS",
            parent,
            "COMMAND ALERT",
            "PRIORITY COMMAND ALERT\n\n" +
            "Operational review required.\n" +
            "Review command queue and\n" +
            "active operation assignments.");

        Popup(
            "UNIT_DETAILS",
            parent,
            "UNIT DETAILS",
            "BULLDOG-017\n\n" +
            "STATUS       DAMAGED\n" +
            "MOBILITY     62%\n" +
            "ARMOR        91%\n" +
            "PROPULSION   48%\n" +
            "REPAIR QUEUE ACTIVE");

        Popup(
            "MISSION_DETAILS",
            parent,
            "MISSION DETAILS",
            "IRON HORIZON\n\n" +
            "MISSION STATUS      ACTIVE\n" +
            "PRIMARY OBJECTIVE   SECURE\n" +
            "SECONDARY OBJECTIVE RECON\n" +
            "DEPLOYMENT          24 UNITS\n" +
            "BATTLE BUDGET       10,000");

        Popup(
            "COMMUNICATIONS",
            parent,
            "COMMUNICATIONS",
            "NETWORK STATUS       ONLINE\n\n" +
            "COMMAND CHANNEL      ACTIVE\n" +
            "FLEET CHANNEL        ACTIVE\n" +
            "INTELLIGENCE         ACTIVE\n" +
            "LOGISTICS            ACTIVE\n" +
            "OPERATIONS           ACTIVE\n" +
            "SIGNAL               98%");
    }

    private static void Popup(
        string name,
        Transform parent,
        string title,
        string content)
    {
        GameObject overlay =
            CreateFullPanel(
                name + "_OVERLAY",
                parent,
                new Color(0, 0, 0, 0.78f));

        GameObject window =
            AnchoredPanel(
                name,
                overlay.transform,
                new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f),
                new Vector2(-420, -285),
                new Vector2(420, 285),
                Panel2);

        ImageBorder(
            "TOP_ACCENT",
            window.transform,
            Accent);

        LabelStretch(
            "TITLE",
            window.transform,
            title,
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(25, -78),
            new Vector2(-25, -20),
            23,
            Accent,
            TextAnchor.MiddleCenter);

        LabelStretch(
            "CONTENT",
            window.transform,
            content,
            Vector2.zero,
            new Vector2(1, 1),
            new Vector2(40, 95),
            new Vector2(-40, -105),
            16,
            Text,
            TextAnchor.MiddleCenter);

        GameObject close =
            Button(
                "CLOSE",
                window.transform,
                "CLOSE",
                Accent);

        RectTransform cr =
            close.GetComponent<RectTransform>();

        cr.anchorMin =
            new Vector2(0.5f, 0);

        cr.anchorMax =
            new Vector2(0.5f, 0);

        cr.pivot =
            new Vector2(0.5f, 0);

        cr.anchoredPosition =
            new Vector2(0, 25);

        cr.sizeDelta =
            new Vector2(150, 42);

        overlay.SetActive(false);
    }

    // ============================================================
    // UI ROOT
    // ============================================================

    private static GameObject CanvasRoot(
        string name)
    {
        GameObject obj =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));

        Canvas canvas =
            obj.GetComponent<Canvas>();

        canvas.renderMode =
            RenderMode.ScreenSpaceOverlay;

        canvas.sortingOrder =
            100;

        CanvasScaler scaler =
            obj.GetComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(W, H);

        scaler.screenMatchMode =
            CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        scaler.matchWidthOrHeight =
            0.5f;

        return obj;
    }

    private static GameObject CreateFullPanel(
        string name,
        Transform parent,
        Color color)
    {
        GameObject obj =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image));

        obj.transform.SetParent(
            parent,
            false);

        RectTransform r =
            obj.GetComponent<RectTransform>();

        r.anchorMin =
            Vector2.zero;

        r.anchorMax =
            Vector2.one;

        r.offsetMin =
            Vector2.zero;

        r.offsetMax =
            Vector2.zero;

        Image image =
            obj.GetComponent<Image>();

        image.color =
            color;

        return obj;
    }

    private static GameObject AnchoredPanel(
        string name,
        Transform parent,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 offsetMin,
        Vector2 offsetMax,
        Color color)
    {
        GameObject obj =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image));

        obj.transform.SetParent(
            parent,
            false);

        RectTransform r =
            obj.GetComponent<RectTransform>();

        r.anchorMin =
            anchorMin;

        r.anchorMax =
            anchorMax;

        r.offsetMin =
            offsetMin;

        r.offsetMax =
            offsetMax;

        Image image =
            obj.GetComponent<Image>();

        image.color =
            color;

        return obj;
    }

    // ============================================================
    // TEXT
    // ============================================================

    private static GameObject Label(
        string name,
        Transform parent,
        string text,
        Vector2 anchor,
        Vector2 position,
        Vector2 size,
        int fontSize,
        Color color,
        TextAnchor alignment)
    {
        GameObject obj =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Text));

        obj.transform.SetParent(
            parent,
            false);

        RectTransform r =
            obj.GetComponent<RectTransform>();

        r.anchorMin =
            anchor;

        r.anchorMax =
            anchor;

        r.pivot =
            new Vector2(0.5f, 0.5f);

        r.anchoredPosition =
            position;

        r.sizeDelta =
            size;

        Text t =
            obj.GetComponent<Text>();

        t.text =
            text;

        t.font =
            Font;

        t.fontSize =
            fontSize;

        t.color =
            color;

        t.alignment =
            alignment;

        t.horizontalOverflow =
            HorizontalWrapMode.Wrap;

        t.verticalOverflow =
            VerticalWrapMode.Overflow;

        t.raycastTarget =
            false;

        return obj;
    }

    private static GameObject LabelStretch(
        string name,
        Transform parent,
        string text,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 offsetMin,
        Vector2 offsetMax,
        int fontSize,
        Color color,
        TextAnchor alignment)
    {
        GameObject obj =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Text));

        obj.transform.SetParent(
            parent,
            false);

        RectTransform r =
            obj.GetComponent<RectTransform>();

        r.anchorMin =
            anchorMin;

        r.anchorMax =
            anchorMax;

        r.offsetMin =
            offsetMin;

        r.offsetMax =
            offsetMax;

        Text t =
            obj.GetComponent<Text>();

        t.text =
            text;

        t.font =
            Font;

        t.fontSize =
            fontSize;

        t.color =
            color;

        t.alignment =
            alignment;

        t.horizontalOverflow =
            HorizontalWrapMode.Wrap;

        t.verticalOverflow =
            VerticalWrapMode.Overflow;

        t.raycastTarget =
            false;

        return obj;
    }

    // ============================================================
    // BUTTONS
    // ============================================================

    private static GameObject NavigationButton(
        string name,
        Transform parent,
        string text,
        bool selected)
    {
        GameObject obj =
            Button(
                name,
                parent,
                text,
                selected ? Accent : AccentDim);

        Image image =
            obj.GetComponent<Image>();

        if (selected)
        {
            image.color =
                new Color(
                    0.025f,
                    0.15f,
                    0.21f,
                    1f);
        }

        return obj;
    }

    private static GameObject Button(
        string name,
        Transform parent,
        string text,
        Color accent)
    {
        GameObject obj =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image),
                typeof(Button));

        obj.transform.SetParent(
            parent,
            false);

        Image image =
            obj.GetComponent<Image>();

        image.color =
            new Color(
                0.015f,
                0.045f,
                0.062f,
                1f);

        Button button =
            obj.GetComponent<Button>();

        ColorBlock colors =
            button.colors;

        colors.normalColor =
            new Color(
                0.015f,
                0.045f,
                0.062f,
                1f);

        colors.highlightedColor =
            new Color(
                accent.r * 0.40f,
                accent.g * 0.40f,
                accent.b * 0.40f,
                1f);

        colors.pressedColor =
            accent;

        colors.selectedColor =
            colors.highlightedColor;

        colors.fadeDuration =
            0.08f;

        button.colors =
            colors;

        LabelStretch(
            "TEXT",
            obj.transform,
            text,
            Vector2.zero,
            Vector2.one,
            Vector2.zero,
            Vector2.zero,
            12,
            Text,
            TextAnchor.MiddleCenter);

        return obj;
    }

    // ============================================================
    // DECORATION
    // ============================================================

    private static void ImageBorder(
        string name,
        Transform parent,
        Color color)
    {
        GameObject obj =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image));

        obj.transform.SetParent(
            parent,
            false);

        RectTransform r =
            obj.GetComponent<RectTransform>();

        r.anchorMin =
            new Vector2(0, 1);

        r.anchorMax =
            new Vector2(1, 1);

        r.offsetMin =
            new Vector2(0, -2);

        r.offsetMax =
            Vector2.zero;

        Image image =
            obj.GetComponent<Image>();

        image.color =
            color;

        image.raycastTarget =
            false;
    }

    private static void HorizontalLine(
        string name,
        Transform parent,
        Color color)
    {
        GameObject obj =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image));

        obj.transform.SetParent(
            parent,
            false);

        RectTransform r =
            obj.GetComponent<RectTransform>();

        r.anchorMin =
            new Vector2(0, 0);

        r.anchorMax =
            new Vector2(1, 0);

        r.offsetMin =
            new Vector2(0, 0);

        r.offsetMax =
            new Vector2(0, 2);

        Image image =
            obj.GetComponent<Image>();

        image.color =
            color;

        image.raycastTarget =
            false;
    }

    // ============================================================
    // 3D HELPERS
    // ============================================================

    private static GameObject Empty(
        string name,
        Transform parent = null)
    {
        GameObject obj =
            new GameObject(name);

        if (parent != null)
            obj.transform.SetParent(
                parent,
                false);

        return obj;
    }

    private static GameObject Cube(
        string name,
        Transform parent,
        Vector3 position,
        Vector3 scale,
        Material material)
    {
        GameObject obj =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube);

        obj.name =
            name;

        obj.transform.SetParent(
            parent,
            true);

        obj.transform.position =
            position;

        obj.transform.localScale =
            scale;

        Renderer renderer =
            obj.GetComponent<Renderer>();

        if (renderer != null)
            renderer.sharedMaterial =
                material;

        return obj;
    }

    private static GameObject Cylinder(
        string name,
        Transform parent,
        Vector3 position,
        Vector3 scale,
        Material material)
    {
        GameObject obj =
            GameObject.CreatePrimitive(
                PrimitiveType.Cylinder);

        obj.name =
            name;

        obj.transform.SetParent(
            parent,
            true);

        obj.transform.position =
            position;

        obj.transform.localScale =
            scale;

        Renderer renderer =
            obj.GetComponent<Renderer>();

        if (renderer != null)
            renderer.sharedMaterial =
                material;

        return obj;
    }
}
