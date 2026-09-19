using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#else
using UnityEngine.EventSystems;
#endif

public static class OPAWGarageVisualBuilder
{
    // ============================================================
    // COLORS
    // ============================================================

    private static readonly Color Background =
        new Color(0.008f, 0.012f, 0.018f, 1f);

    private static readonly Color Panel =
        new Color(0.025f, 0.035f, 0.045f, 0.97f);

    private static readonly Color PanelLight =
        new Color(0.045f, 0.060f, 0.072f, 0.98f);

    private static readonly Color Cyan =
        new Color(0.05f, 0.78f, 0.92f, 1f);

    private static readonly Color CyanDark =
        new Color(0.03f, 0.25f, 0.32f, 1f);

    private static readonly Color Green =
        new Color(0.18f, 0.88f, 0.42f, 1f);

    private static readonly Color Amber =
        new Color(1f, 0.62f, 0.12f, 1f);

    private static readonly Color White =
        new Color(0.88f, 0.93f, 0.96f, 1f);

    private static readonly Color Gray =
        new Color(0.45f, 0.53f, 0.58f, 1f);

    // ============================================================
    // ENTRY
    // ============================================================

    [MenuItem("Obsidian Protocol/Build/GARAGE")]
    public static void BuildGarage()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (!scene.IsValid())
        {
            Debug.LogError("GARAGE: No valid active scene.");
            return;
        }

        Debug.Log("GARAGE: Starting complete fresh rebuild.");

        ClearScene();

        BuildWorld();
        BuildUnits();
        BuildStorage();
        BuildTerminals();
        BuildLighting();
        BuildCamera();
        BuildEventSystem();
        BuildHUD();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Selection.activeGameObject =
            GameObject.Find("GARAGE_HUD");

        SceneView.RepaintAll();

        Debug.Log("GARAGE: COMPLETE FRESH BUILD FINISHED.");
    }

    // ============================================================
    // CLEAR
    // ============================================================

    private static void ClearScene()
    {
        Scene scene = SceneManager.GetActiveScene();

        List<GameObject> roots = new List<GameObject>();
        scene.GetRootGameObjects(roots);

        foreach (GameObject root in roots)
        {
            Object.DestroyImmediate(root);
        }
    }

    // ============================================================
    // WORLD
    // ============================================================

    private static void BuildWorld()
    {
        GameObject root =
            NewObject("GARAGE_WORLD");

        // Main floor
        Cube(
            "GARAGE_FLOOR",
            root.transform,
            new Vector3(0f, -0.5f, 0f),
            new Vector3(100f, 1f, 70f),
            new Color(0.035f, 0.045f, 0.055f));

        // Walls
        Cube(
            "NORTH_WALL",
            root.transform,
            new Vector3(0f, 8f, 34f),
            new Vector3(100f, 16f, 1f),
            new Color(0.018f, 0.024f, 0.030f));

        Cube(
            "SOUTH_WALL",
            root.transform,
            new Vector3(0f, 8f, -34f),
            new Vector3(100f, 16f, 1f),
            new Color(0.018f, 0.024f, 0.030f));

        Cube(
            "WEST_WALL",
            root.transform,
            new Vector3(-50f, 8f, 0f),
            new Vector3(1f, 16f, 70f),
            new Color(0.018f, 0.024f, 0.030f));

        Cube(
            "EAST_WALL",
            root.transform,
            new Vector3(50f, 8f, 0f),
            new Vector3(1f, 16f, 70f),
            new Color(0.018f, 0.024f, 0.030f));

        // Main central roadway
        Cube(
            "CENTRAL_GARAGE_DECK",
            root.transform,
            new Vector3(0f, 0.02f, 0f),
            new Vector3(14f, 0.12f, 64f),
            new Color(0.06f, 0.07f, 0.075f));

        Cube(
            "CROSS_DECK",
            root.transform,
            new Vector3(0f, 0.025f, 0f),
            new Vector3(94f, 0.12f, 12f),
            new Color(0.06f, 0.07f, 0.075f));

        // Zones
        Zone(
            root.transform,
            "COMMAND_CENTER_FLEET_HQ",
            new Vector3(-34f, 0f, 24f),
            new Vector3(28f, 0.2f, 17f),
            "COMMAND CENTER / FLEET HQ");

        Zone(
            root.transform,
            "MAIN_HANGAR",
            new Vector3(0f, 0f, 24f),
            new Vector3(32f, 0.2f, 17f),
            "MAIN HANGAR");

        Zone(
            root.transform,
            "AIR_OPERATIONS",
            new Vector3(34f, 0f, 24f),
            new Vector3(28f, 0.2f, 17f),
            "AIR OPERATIONS");

        Zone(
            root.transform,
            "NAVAL_OPERATIONS",
            new Vector3(-34f, 0f, 5f),
            new Vector3(28f, 0.2f, 16f),
            "NAVAL OPERATIONS");

        Zone(
            root.transform,
            "EXPERIMENTAL_CONTAINMENT",
            new Vector3(0f, 0f, 5f),
            new Vector3(32f, 0.2f, 16f),
            "EXPERIMENTAL CONTAINMENT");

        Zone(
            root.transform,
            "COMMAND_UNIT_CHAMBER",
            new Vector3(34f, 0f, 5f),
            new Vector3(28f, 0.2f, 16f),
            "COMMAND UNIT CHAMBER");

        Zone(
            root.transform,
            "REPAIR_MAINTENANCE",
            new Vector3(-34f, 0f, -12f),
            new Vector3(28f, 0.2f, 14f),
            "REPAIR / MAINTENANCE");

        Zone(
            root.transform,
            "CUSTOMIZATION",
            new Vector3(0f, 0f, -12f),
            new Vector3(32f, 0.2f, 14f),
            "CUSTOMIZATION");

        Zone(
            root.transform,
            "WEAPONS_EQUIPMENT",
            new Vector3(34f, 0f, -12f),
            new Vector3(28f, 0.2f, 14f),
            "WEAPONS & EQUIPMENT");

        Zone(
            root.transform,
            "UPGRADE_RESEARCH",
            new Vector3(-36f, 0f, -27f),
            new Vector3(20f, 0.2f, 9f),
            "UPGRADE / RESEARCH");

        Zone(
            root.transform,
            "AI_CORE_PERSONALITY_LAB",
            new Vector3(-13f, 0f, -27f),
            new Vector3(24f, 0.2f, 9f),
            "AI CORE / PERSONALITY LAB");

        Zone(
            root.transform,
            "FABRICATION",
            new Vector3(12f, 0f, -27f),
            new Vector3(22f, 0.2f, 9f),
            "FABRICATION");

        Zone(
            root.transform,
            "SALVAGE_RECYCLING",
            new Vector3(34f, 0f, -27f),
            new Vector3(20f, 0.2f, 9f),
            "SALVAGE / RECYCLING");

        Zone(
            root.transform,
            "STORAGE",
            new Vector3(-18f, 0f, -20f),
            new Vector3(28f, 0.2f, 5f),
            "STORAGE");

        Zone(
            root.transform,
            "DEPLOYMENT_STAGING",
            new Vector3(20f, 0f, -20f),
            new Vector3(42f, 0.2f, 5f),
            "DEPLOYMENT STAGING");

        // Hangar doors
        for (int i = -4; i <= 4; i++)
        {
            Cube(
                "FLOOR_LIGHT",
                root.transform,
                new Vector3(i * 8f, 0.08f, 0f),
                new Vector3(4f, 0.04f, 0.12f),
                Cyan);
        }
    }

    // ============================================================
    // UNITS
    // ============================================================

    private static void BuildUnits()
    {
        GameObject root =
            NewObject("GARAGE_UNITS");

        // Ground showroom
        GroundUnit(
            root.transform,
            new Vector3(-8f, 1f, 25f),
            "BULLDOG");

        GroundUnit(
            root.transform,
            new Vector3(8f, 1f, 25f),
            "FORGE");

        // Air showroom
        Drone(
            root.transform,
            new Vector3(28f, 7f, 24f),
            "WARDEN");

        Drone(
            root.transform,
            new Vector3(39f, 6f, 24f),
            "BEACON");

        // Naval
        NavalUnit(
            root.transform,
            new Vector3(-34f, 1f, 5f),
            "SURVEYOR MK1");

        // Experimental
        string[] experimental =
        {
            "ECHO",
            "NULLPOINT",
            "SPECTER",
            "SHADOWGRID",
            "PHANTOM",
            "HELIX"
        };

        for (int i = 0; i < experimental.Length; i++)
        {
            int column = i % 3;
            int row = i / 3;

            float x = -11f + column * 11f;
            float z = 1f + row * 7f;

            ContainmentPod(
                root.transform,
                new Vector3(x, 1f, z),
                experimental[i]);
        }

        // Command units
        string[] command =
        {
            "ARCHIVE",
            "WORLDMAP",
            "COMMAND CORE",
            "FUSION",
            "NEXUS",
            "INSIGHT",
            "PULSE",
            "VECTOR CORE"
        };

        for (int i = 0; i < command.Length; i++)
        {
            int column = i % 4;
            int row = i / 4;

            float x = 23f + column * 7f;
            float z = 2f + row * 7f;

            CommandUnit(
                root.transform,
                new Vector3(x, 1f, z),
                command[i]);
        }
    }

    // ============================================================
    // STORAGE
    // ============================================================

    private static void BuildStorage()
    {
        GameObject root =
            NewObject("GARAGE_STORAGE");

        string[] resources =
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

        for (int i = 0; i < resources.Length; i++)
        {
            int column = i % 4;
            int row = i / 4;

            float x = -28f + column * 7f;
            float z = -20f + row * 2.5f;

            ResourceCrate(
                root.transform,
                new Vector3(x, 1f, z),
                resources[i]);
        }
    }

    // ============================================================
    // TERMINALS
    // ============================================================

    private static void BuildTerminals()
    {
        GameObject root =
            NewObject("GARAGE_TERMINALS");

        string[] command =
        {
            "FLEET MANAGEMENT",
            "AI COMMAND",
            "RESEARCH",
            "LOGISTICS",
            "FINANCE",
            "DEPLOYMENT"
        };

        for (int i = 0; i < command.Length; i++)
        {
            Terminal(
                root.transform,
                new Vector3(
                    -44f + i * 5.5f,
                    1f,
                    27f),
                command[i]);
        }

        string[] deployment =
        {
            "SELECT OPERATION",
            "SELECT FORCE",
            "DEPLOYMENT BUDGET",
            "FORMATION",
            "DOCTRINE",
            "COMMAND INTENT",
            "RULES OF ENGAGEMENT",
            "FINAL DEPLOYMENT"
        };

        for (int i = 0; i < deployment.Length; i++)
        {
            int column = i % 4;
            int row = i / 4;

            Terminal(
                root.transform,
                new Vector3(
                    2f + column * 10f,
                    1f,
                    -20f + row * 2.5f),
                deployment[i]);
        }
    }

    // ============================================================
    // LIGHTING
    // ============================================================

    private static void BuildLighting()
    {
        GameObject root =
            NewObject("GARAGE_LIGHTING");

        GameObject sun =
            NewObject("GARAGE_SUN", root.transform);

        Light directional =
            sun.AddComponent<Light>();

        directional.type =
            LightType.Directional;

        directional.intensity = 0.75f;
        directional.shadows =
            LightShadows.Soft;

        sun.transform.rotation =
            Quaternion.Euler(50f, -30f, 0f);

        Vector3[] lights =
        {
            new Vector3(-30f, 10f, 25f),
            new Vector3(0f, 10f, 25f),
            new Vector3(30f, 10f, 25f),
            new Vector3(-30f, 10f, 5f),
            new Vector3(0f, 10f, 5f),
            new Vector3(30f, 10f, 5f),
            new Vector3(-30f, 10f, -12f),
            new Vector3(0f, 10f, -12f),
            new Vector3(30f, 10f, -12f),
            new Vector3(0f, 10f, -27f)
        };

        foreach (Vector3 position in lights)
        {
            GameObject lightObject =
                NewObject(
                    "GARAGE_LIGHT",
                    root.transform);

            lightObject.transform.position =
                position;

            Light light =
                lightObject.AddComponent<Light>();

            light.type =
                LightType.Point;

            light.range = 24f;
            light.intensity = 7f;
            light.color = Cyan;
        }
    }

    // ============================================================
    // CAMERA
    // ============================================================

    private static void BuildCamera()
    {
        GameObject root =
            NewObject("GARAGE_CAMERA_SYSTEM");

        GameObject cameraObject =
            NewObject(
                "GARAGE_CAMERA",
                root.transform);

        Camera camera =
            cameraObject.AddComponent<Camera>();

        camera.clearFlags =
            CameraClearFlags.SolidColor;

        camera.backgroundColor =
            Background;

        camera.fieldOfView = 52f;
        camera.nearClipPlane = 0.1f;
        camera.farClipPlane = 300f;

        // IMPORTANT:
        // Camera is positioned HIGH and FAR enough
        // to see the entire garage.
        cameraObject.transform.position =
            new Vector3(0f, 62f, -82f);

        Vector3 target =
            new Vector3(0f, 0f, 2f);

        cameraObject.transform.rotation =
            Quaternion.LookRotation(
                target - cameraObject.transform.position,
                Vector3.up);

        cameraObject.tag =
            "MainCamera";
    }

    // ============================================================
    // EVENT SYSTEM
    // ============================================================

    private static void BuildEventSystem()
    {
        GameObject eventSystem =
            NewObject("GARAGE_EVENT_SYSTEM");

#if ENABLE_INPUT_SYSTEM
        eventSystem.AddComponent<InputSystemUIInputModule>();
#else
        eventSystem.AddComponent<StandaloneInputModule>();
#endif
    }

    // ============================================================
    // HUD
    // ============================================================

    private static void BuildHUD()
    {
        GameObject hud =
            NewObject("GARAGE_HUD");

        Canvas canvas =
            hud.AddComponent<Canvas>();

        canvas.renderMode =
            RenderMode.ScreenSpaceOverlay;

        canvas.sortingOrder = 100;

        CanvasScaler scaler =
            hud.AddComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(1920f, 1080f);

        scaler.screenMatchMode =
            CanvasScaler.ScreenMatchMode.Expand;

        hud.AddComponent<GraphicRaycaster>();

        Image background =
            hud.AddComponent<Image>();

        background.color =
            new Color(0f, 0f, 0f, 0f);

        BuildTopBar(hud.transform);
        BuildLeftNavigation(hud.transform);
        BuildCenterPanel(hud.transform);
        BuildRightPanel(hud.transform);
        BuildBottomBar(hud.transform);
    }

    // ============================================================
    // TOP BAR
    // ============================================================

    private static void BuildTopBar(Transform parent)
    {
        GameObject panel =
            UIBox(
                parent,
                "GARAGE_TOP_BAR",
                new Vector2(0f, 1f),
                new Vector2(1f, 1f),
                new Vector2(0f, -78f),
                Vector2.zero,
                Panel);

        UILabel(
            panel.transform,
            "TITLE",
            "OBSIDIAN PROTOCOL",
            28,
            White,
            new Vector2(24f, -50f),
            new Vector2(400f, 45f));

        UILabel(
            panel.transform,
            "SUBTITLE",
            "GARAGE / MILITARY FACILITY",
            16,
            Cyan,
            new Vector2(350f, -48f),
            new Vector2(450f, 35f));

        UILabel(
            panel.transform,
            "ONLINE",
            "FACILITY ONLINE",
            15,
            Green,
            new Vector2(820f, -48f),
            new Vector2(200f, 35f));

        UILabel(
            panel.transform,
            "CREDITS",
            "CREDITS  14,000",
            16,
            White,
            new Vector2(1280f, -48f),
            new Vector2(180f, 35f));

        UILabel(
            panel.transform,
            "ENERGY",
            "ENERGY 84%",
            16,
            Green,
            new Vector2(1470f, -48f),
            new Vector2(150f, 35f));

        UILabel(
            panel.transform,
            "ALERTS",
            "ALERTS 0",
            16,
            White,
            new Vector2(1630f, -48f),
            new Vector2(150f, 35f));
    }

    // ============================================================
    // LEFT NAV
    // ============================================================

    private static void BuildLeftNavigation(
        Transform parent)
    {
        GameObject panel =
            UIBox(
                parent,
                "GARAGE_NAVIGATION",
                new Vector2(0f, 0f),
                new Vector2(0f, 1f),
                new Vector2(0f, 84f),
                new Vector2(300f, -78f),
                Panel);

        UILabel(
            panel.transform,
            "HEADER",
            "GARAGE NAVIGATION",
            19,
            Cyan,
            new Vector2(18f, -18f),
            new Vector2(260f, 35f));

        string[] items =
        {
            "FLEET",
            "STORE",
            "REPAIR",
            "CUSTOMIZE",
            "EQUIPMENT",
            "UPGRADE",
            "AI CORE",
            "FABRICATION",
            "STORAGE",
            "SALVAGE",
            "DEPLOYMENT",
            "COMMAND CENTER"
        };

        for (int i = 0; i < items.Length; i++)
        {
            ButtonUI(
                panel.transform,
                "BUTTON_" + Clean(items[i]),
                items[i],
                new Vector2(16f, -62f - i * 58f),
                new Vector2(268f, 48f));
        }
    }

    // ============================================================
    // CENTER
    // ============================================================

    private static void BuildCenterPanel(
        Transform parent)
    {
        GameObject panel =
            UIBox(
                parent,
                "PHYSICAL_GARAGE_CONNECTIONS",
                new Vector2(0f, 0f),
                new Vector2(1f, 1f),
                new Vector2(315f, 84f),
                new Vector2(-345f, -78f),
                Background);

        UILabel(
            panel.transform,
            "HEADER",
            "PHYSICAL GARAGE",
            24,
            Cyan,
            new Vector2(22f, -18f),
            new Vector2(400f, 40f));

        UILabel(
            panel.transform,
            "SUBHEADER",
            "SELECT A FACILITY AREA",
            13,
            Gray,
            new Vector2(22f, -52f),
            new Vector2(350f, 30f));

        string[] zones =
        {
            "COMMAND CENTER / FLEET HQ",
            "MAIN HANGAR",
            "AIR OPERATIONS",
            "NAVAL OPERATIONS",
            "EXPERIMENTAL CONTAINMENT",
            "COMMAND UNIT CHAMBER",
            "REPAIR / MAINTENANCE",
            "CUSTOMIZATION",
            "WEAPONS & EQUIPMENT",
            "UPGRADE / RESEARCH",
            "AI CORE / PERSONALITY LAB",
            "FABRICATION",
            "SALVAGE / RECYCLING",
            "STORAGE",
            "DEPLOYMENT STAGING"
        };

        for (int i = 0; i < zones.Length; i++)
        {
            int column = i % 3;
            int row = i / 3;

            float x = 18f + column * 220f;
            float y = -92f - row * 112f;

            GameObject card =
                UIBox(
                    panel.transform,
                    "ZONE_" + Clean(zones[i]),
                    new Vector2(0f, 1f),
                    new Vector2(0f, 1f),
                    new Vector2(x, y - 100f),
                    new Vector2(x + 205f, y),
                    PanelLight);

            UILabel(
                card.transform,
                "NAME",
                zones[i],
                13,
                White,
                new Vector2(10f, -12f),
                new Vector2(185f, 38f));

            UILabel(
                card.transform,
                "STATUS",
                "SYSTEM ONLINE",
                11,
                Green,
                new Vector2(10f, -48f),
                new Vector2(180f, 25f));
        }
    }

    // ============================================================
    // RIGHT PANEL
    // ============================================================

    private static void BuildRightPanel(
        Transform parent)
    {
        GameObject panel =
            UIBox(
                parent,
                "FACILITY_INFORMATION",
                new Vector2(1f, 0f),
                new Vector2(1f, 1f),
                new Vector2(-330f, 84f),
                new Vector2(0f, -78f),
                Panel);

        UILabel(
            panel.transform,
            "HEADER",
            "FACILITY STATUS",
            21,
            Cyan,
            new Vector2(18f, -18f),
            new Vector2(280f, 35f));

        StatusBlock(
            panel.transform,
            "ACTIVE ZONE",
            "MAIN HANGAR",
            15,
            White,
            -70f);

        StatusBlock(
            panel.transform,
            "UNIT STATUS",
            "READY       28\nDAMAGED      02\nMAINTENANCE  04",
            14,
            Green,
            -145f);

        StatusBlock(
            panel.transform,
            "STORAGE",
            "IRON          82%\nALLOY         64%\nELECTRONICS   71%\nFUEL          88%\nENERGY        84%",
            13,
            White,
            -265f);

        GameObject deployment =
            UIBox(
                panel.transform,
                "DEPLOYMENT_BUDGET",
                new Vector2(0f, 0f),
                new Vector2(1f, 0f),
                new Vector2(12f, 18f),
                new Vector2(-12f, 210f),
                new Color(0.035f, 0.075f, 0.085f, 1f));

        UILabel(
            deployment.transform,
            "TITLE",
            "DEPLOYMENT BUDGET",
            15,
            Cyan,
            new Vector2(14f, -18f),
            new Vector2(270f, 30f));

        UILabel(
            deployment.transform,
            "VALUE",
            "10,000 / 10,000",
            24,
            Green,
            new Vector2(14f, -58f),
            new Vector2(270f, 38f));

        UILabel(
            deployment.transform,
            "RULE",
            "OWNERSHIP DOES NOT EQUAL\nCOMBAT POWER",
            11,
            Gray,
            new Vector2(14f, -105f),
            new Vector2(270f, 45f));

        // IMPORTANT:
        // There is deliberately NO ACTION STATUS panel.
        // No red X.
        // No error icon.
        // No placeholder object.
    }

    // ============================================================
    // BOTTOM
    // ============================================================

    private static void BuildBottomBar(
        Transform parent)
    {
        GameObject panel =
            UIBox(
                parent,
                "GARAGE_BOTTOM_BAR",
                new Vector2(0f, 0f),
                new Vector2(1f, 0f),
                new Vector2(315f, 0f),
                new Vector2(-330f, 72f),
                Panel);

        UILabel(
            panel.transform,
            "STATUS",
            "GARAGE SYSTEMS NOMINAL",
            14,
            Green,
            new Vector2(18f, 18f),
            new Vector2(300f, 35f));

        UILabel(
            panel.transform,
            "HELP",
            "SELECT A PHYSICAL AREA TO INSPECT OR CONFIGURE",
            13,
            Gray,
            new Vector2(340f, 18f),
            new Vector2(550f, 35f));

        ButtonUI(
            panel.transform,
            "BACK",
            "BACK",
            new Vector2(720f, 12f),
            new Vector2(100f, 46f));

        ButtonUI(
            panel.transform,
            "COMMAND_CENTER",
            "COMMAND CENTER",
            new Vector2(830f, 12f),
            new Vector2(155f, 46f));
    }

    // ============================================================
    // UI HELPERS
    // ============================================================

    private static GameObject UIBox(
        Transform parent,
        string name,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 offsetMin,
        Vector2 offsetMax,
        Color color)
    {
        GameObject obj =
            NewObject(name, parent);

        RectTransform rect =
            obj.AddComponent<RectTransform>();

        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = offsetMin;
        rect.offsetMax = offsetMax;

        Image image =
            obj.AddComponent<Image>();

        image.color = color;

        return obj;
    }

    private static Text UILabel(
        Transform parent,
        string name,
        string value,
        int fontSize,
        Color color,
        Vector2 position,
        Vector2 size)
    {
        GameObject obj =
            NewObject(name, parent);

        RectTransform rect =
            obj.AddComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0f, 1f);

        rect.anchorMax =
            new Vector2(0f, 1f);

        rect.pivot =
            new Vector2(0f, 1f);

        rect.anchoredPosition =
            position;

        rect.sizeDelta =
            size;

        Text text =
            obj.AddComponent<Text>();

        text.text = value;

        text.font =
            Resources.GetBuiltinResource<Font>(
                "LegacyRuntime.ttf");

        text.fontSize =
            fontSize;

        text.color =
            color;

        text.alignment =
            TextAnchor.UpperLeft;

        text.horizontalOverflow =
            HorizontalWrapMode.Wrap;

        text.verticalOverflow =
            VerticalWrapMode.Overflow;

        return text;
    }

    private static void StatusBlock(
        Transform parent,
        string title,
        string value,
        int fontSize,
        Color color,
        float y)
    {
        UILabel(
            parent,
            "TITLE_" + Clean(title),
            title,
            12,
            Cyan,
            new Vector2(18f, y),
            new Vector2(280f, 25f));

        UILabel(
            parent,
            "VALUE_" + Clean(title),
            value,
            fontSize,
            color,
            new Vector2(18f, y - 28f),
            new Vector2(280f, 100f));
    }

    private static GameObject ButtonUI(
        Transform parent,
        string name,
        string label,
        Vector2 position,
        Vector2 size)
    {
        GameObject buttonObject =
            NewObject(name, parent);

        RectTransform rect =
            buttonObject.AddComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0f, 1f);

        rect.anchorMax =
            new Vector2(0f, 1f);

        rect.pivot =
            new Vector2(0f, 1f);

        rect.anchoredPosition =
            position;

        rect.sizeDelta =
            size;

        Image image =
            buttonObject.AddComponent<Image>();

        image.color =
            new Color(0.055f, 0.075f, 0.085f, 1f);

        Button button =
            buttonObject.AddComponent<Button>();

        ColorBlock colors =
            button.colors;

        colors.normalColor =
            new Color(0.055f, 0.075f, 0.085f, 1f);

        colors.highlightedColor =
            new Color(0.08f, 0.22f, 0.27f, 1f);

        colors.pressedColor =
            new Color(0.04f, 0.45f, 0.55f, 1f);

        button.colors =
            colors;

        Text text =
            UILabel(
                buttonObject.transform,
                "TEXT",
                label,
                13,
                White,
                Vector2.zero,
                size);

        RectTransform textRect =
            text.GetComponent<RectTransform>();

        textRect.anchorMin =
            Vector2.zero;

        textRect.anchorMax =
            Vector2.one;

        textRect.offsetMin =
            new Vector2(10f, 0f);

        textRect.offsetMax =
            new Vector2(-10f, 0f);

        text.alignment =
            TextAnchor.MiddleCenter;

        return buttonObject;
    }

    // ============================================================
    // 3D HELPERS
    // ============================================================

    private static void Zone(
        Transform parent,
        string name,
        Vector3 position,
        Vector3 scale,
        string label)
    {
        GameObject zone =
            NewObject(name, parent);

        Cube(
            "FLOOR",
            zone.transform,
            position,
            scale,
            new Color(0.055f, 0.065f, 0.075f));

        WorldLabel(
            zone.transform,
            position + new Vector3(
                0f,
                0.45f,
                -scale.z * 0.42f),
            label);
    }

    private static void GroundUnit(
        Transform parent,
        Vector3 position,
        string label)
    {
        GameObject unit =
            NewObject(
                "GROUND_" + Clean(label),
                parent);

        Cube(
            "BODY",
            unit.transform,
            position,
            new Vector3(7f, 1.8f, 4f),
            new Color(0.09f, 0.11f, 0.12f));

        Cube(
            "CAB",
            unit.transform,
            position + new Vector3(0f, 1.5f, 0f),
            new Vector3(3f, 1.4f, 2.5f),
            new Color(0.12f, 0.14f, 0.15f));

        WorldLabel(
            unit.transform,
            position + new Vector3(0f, 3.2f, 0f),
            label);
    }

    private static void Drone(
        Transform parent,
        Vector3 position,
        string label)
    {
        GameObject drone =
            NewObject(
                "AIR_" + Clean(label),
                parent);

        Sphere(
            "BODY",
            drone.transform,
            position,
            new Vector3(3f, 1f, 3f),
            new Color(0.08f, 0.11f, 0.13f));

        Cube(
            "ARM_LEFT",
            drone.transform,
            position + new Vector3(-2.5f, 0f, 0f),
            new Vector3(2.5f, 0.15f, 0.3f),
            Cyan);

        Cube(
            "ARM_RIGHT",
            drone.transform,
            position + new Vector3(2.5f, 0f, 0f),
            new Vector3(2.5f, 0.15f, 0.3f),
            Cyan);

        WorldLabel(
            drone.transform,
            position + new Vector3(0f, 2f, 0f),
            label);
    }

    private static void NavalUnit(
        Transform parent,
        Vector3 position,
        string label)
    {
        GameObject boat =
            NewObject(
                "NAVAL_" + Clean(label),
                parent);

        Cube(
            "HULL",
            boat.transform,
            position,
            new Vector3(9f, 1.2f, 3f),
            new Color(0.07f, 0.10f, 0.12f));

        Cube(
            "CABIN",
            boat.transform,
            position + new Vector3(0f, 1.3f, 0f),
            new Vector3(3f, 1.7f, 2f),
            new Color(0.12f, 0.14f, 0.15f));

        WorldLabel(
            boat.transform,
            position + new Vector3(0f, 2.8f, 0f),
            label);
    }

    private static void ContainmentPod(
        Transform parent,
        Vector3 position,
        string label)
    {
        GameObject pod =
            NewObject(
                "EXPERIMENTAL_" + Clean(label),
                parent);

        Cube(
            "FRAME",
            pod.transform,
            position,
            new Vector3(4f, 4f, 3.5f),
            new Color(0.07f, 0.08f, 0.09f));

        Sphere(
            "CORE",
            pod.transform,
            position + new Vector3(0f, 0.5f, 0f),
            new Vector3(1.7f, 2.2f, 1.7f),
            new Color(0.20f, 0.06f, 0.28f));

        WorldLabel(
            pod.transform,
            position + new Vector3(0f, 3f, 0f),
            label);
    }

    private static void CommandUnit(
        Transform parent,
        Vector3 position,
        string label)
    {
        GameObject unit =
            NewObject(
                "COMMAND_" + Clean(label),
                parent);

        Sphere(
            "CORE",
            unit.transform,
            position,
            new Vector3(2f, 2f, 2f),
            new Color(0.08f, 0.15f, 0.18f));

        Cube(
            "BASE",
            unit.transform,
            position + new Vector3(0f, -1.2f, 0f),
            new Vector3(3f, 0.3f, 3f),
            Cyan);

        WorldLabel(
            unit.transform,
            position + new Vector3(0f, 2.5f, 0f),
            label);
    }

    private static void ResourceCrate(
        Transform parent,
        Vector3 position,
        string label)
    {
        GameObject crate =
            NewObject(
                "RESOURCE_" + Clean(label),
                parent);

        Cube(
            "CRATE",
            crate.transform,
            position,
            new Vector3(5f, 1.5f, 2f),
            new Color(0.10f, 0.11f, 0.12f));

        WorldLabel(
            crate.transform,
            position + new Vector3(0f, 1.6f, 0f),
            label);
    }

    private static void Terminal(
        Transform parent,
        Vector3 position,
        string label)
    {
        GameObject terminal =
            NewObject(
                "TERMINAL_" + Clean(label),
                parent);

        Cube(
            "BASE",
            terminal.transform,
            position,
            new Vector3(2.4f, 0.8f, 1.6f),
            new Color(0.055f, 0.065f, 0.075f));

        Cube(
            "DISPLAY",
            terminal.transform,
            position + new Vector3(0f, 1.4f, 0f),
            new Vector3(2f, 1.5f, 0.15f),
            Cyan);

        WorldLabel(
            terminal.transform,
            position + new Vector3(0f, 2.6f, 0f),
            label);
    }

    // ============================================================
    // PRIMITIVES
    // ============================================================

    private static GameObject Cube(
        string name,
        Transform parent,
        Vector3 position,
        Vector3 scale,
        Color color)
    {
        GameObject obj =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube);

        obj.name = name;
        obj.transform.SetParent(parent);
        obj.transform.position = position;
        obj.transform.localScale = scale;

        Renderer renderer =
            obj.GetComponent<Renderer>();

        renderer.sharedMaterial =
            Material(color);

        return obj;
    }

    private static GameObject Sphere(
        string name,
        Transform parent,
        Vector3 position,
        Vector3 scale,
        Color color)
    {
        GameObject obj =
            GameObject.CreatePrimitive(
                PrimitiveType.Sphere);

        obj.name = name;
        obj.transform.SetParent(parent);
        obj.transform.position = position;
        obj.transform.localScale = scale;

        Renderer renderer =
            obj.GetComponent<Renderer>();

        renderer.sharedMaterial =
            Material(color);

        return obj;
    }

    private static Material Material(Color color)
    {
        Shader shader =
            Shader.Find(
                "Universal Render Pipeline/Lit");

        if (shader == null)
            shader = Shader.Find("Standard");

        Material material =
            new Material(shader);

        material.color =
            color;

        return material;
    }

    private static void WorldLabel(
        Transform parent,
        Vector3 position,
        string value)
    {
        GameObject obj =
            NewObject(
                "LABEL_" + Clean(value),
                parent);

        obj.transform.position =
            position;

        obj.transform.rotation =
            Quaternion.Euler(90f, 0f, 0f);

        TextMesh text =
            obj.AddComponent<TextMesh>();

        text.text = value;
        text.fontSize = 32;
        text.characterSize = 0.09f;
        text.anchor = TextAnchor.MiddleCenter;
        text.alignment = TextAlignment.Center;
        text.color = Cyan;
    }

    private static GameObject NewObject(
        string name,
        Transform parent = null)
    {
        GameObject obj =
            new GameObject(name);

        if (parent != null)
            obj.transform.SetParent(parent);

        return obj;
    }

    private static string Clean(string value)
    {
        return value
            .Replace("/", "_")
            .Replace("&", "AND")
            .Replace("-", "_")
            .Replace(" ", "_");
    }
}
