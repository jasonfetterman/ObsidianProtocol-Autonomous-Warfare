using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.IO;

public static class BuildBattlefieldHUDAAA
{
    private const string OutputFolder = "Assets/Scenes/SCN03_BATTLEFIELD_AAA";
    private const string OutputScene = OutputFolder + "/SCN03_BATTLEFIELD_AAA.unity";

    private static readonly Color Background = new Color(0.012f, 0.024f, 0.033f, 0.96f);
    private static readonly Color PanelColor = new Color(0.022f, 0.047f, 0.058f, 0.96f);
    private static readonly Color PanelAlt = new Color(0.035f, 0.066f, 0.077f, 0.98f);
    private static readonly Color Cyan = new Color(0.12f, 0.86f, 0.88f, 1f);
    private static readonly Color Muted = new Color(0.43f, 0.62f, 0.66f, 1f);
    private static readonly Color White = new Color(0.83f, 0.91f, 0.91f, 1f);
    private static readonly Color Amber = new Color(1f, 0.64f, 0.23f, 1f);
    private static readonly Color Red = new Color(0.95f, 0.25f, 0.23f, 1f);

    [MenuItem("OPAW/AAA HUD/Create New Battlefield HUD Scene")]
    public static void Build()
    {
        if (!EditorUtility.DisplayDialog(
            "Create AAA Battlefield HUD",
            "Create a separate HUD prototype scene? The existing battlefield scene will not be opened or modified.",
            "Create New Scene", "Cancel"))
            return;

        if (!AssetDatabase.IsValidFolder(OutputFolder))
        {
            if (!AssetDatabase.IsValidFolder("Assets/Scenes"))
                AssetDatabase.CreateFolder("Assets", "Scenes");
            AssetDatabase.CreateFolder("Assets/Scenes", "SCN03_BATTLEFIELD_AAA");
        }

        if (File.Exists(OutputScene))
        {
            bool overwrite = EditorUtility.DisplayDialog(
                "Scene Already Exists",
                "The new HUD prototype scene already exists. Replace that prototype? The original battlefield scene remains untouched.",
                "Replace Prototype", "Cancel");
            if (!overwrite) return;
        }

        Scene scene = EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene, NewSceneMode.Single);

        scene.name = "SCN03_BATTLEFIELD_AAA";

        GameObject canvasObject = new GameObject(
            "[HUD] AAA BATTLEFIELD", typeof(Canvas),
            typeof(CanvasScaler), typeof(GraphicRaycaster));

        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.pixelPerfect = false;

        CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;

        Transform systems = Group("[SYSTEMS]", canvasObject.transform);
        Group("[LAYER] TOP STATUS", canvasObject.transform);
        Group("[LAYER] LEFT COMMANDS", canvasObject.transform);
        Group("[LAYER] BATTLEFIELD OVERLAY", canvasObject.transform);
        Group("[LAYER] RIGHT INTELLIGENCE", canvasObject.transform);
        Group("[LAYER] BOTTOM CONTROLS", canvasObject.transform);
        Group("[WINDOWS]", canvasObject.transform);

        CreateEventSystem();
        BuildTop(canvasObject.transform);
        BuildLeft(canvasObject.transform);
        BuildBattlefield(canvasObject.transform);
        BuildRight(canvasObject.transform);
        BuildBottom(canvasObject.transform);
        BuildWindows(canvasObject.transform);

        GameObject readme = new GameObject("[INFO] HUD PROTOTYPE");
        readme.transform.SetParent(systems, false);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene, OutputScene);
        AssetDatabase.Refresh();

        Selection.activeGameObject = canvasObject;
        EditorUtility.DisplayDialog(
            "HUD Prototype Created",
            "Created a separate AAA HUD prototype at:\n" + OutputScene +
            "\n\nThis is a visual prototype. Existing gameplay buttons and deployment systems have not been reconnected yet.",
            "OK");
    }

    private static Transform Group(string name, Transform parent)
    {
        GameObject go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        return go.transform;
    }

    private static void CreateEventSystem()
    {
        if (Object.FindObjectOfType<EventSystem>() != null) return;
        new GameObject("[SYSTEM] UI EVENT SYSTEM",
            typeof(EventSystem), typeof(StandaloneInputModule));
    }

    private static RectTransform Rect(
        string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax,
        Vector2 position, Vector2 size)
    {
        GameObject go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = position;
        rt.sizeDelta = size;
        return rt;
    }

    private static Image Box(
        string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax,
        Vector2 position, Vector2 size, Color color)
    {
        RectTransform rt = Rect(name, parent, anchorMin, anchorMax, position, size);
        Image image = rt.gameObject.AddComponent<Image>();
        image.color = color;
        image.raycastTarget = false;
        return image;
    }

    private static Text Label(
        string name, Transform parent, string value, int size,
        Color color, TextAnchor alignment,
        Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 dimensions)
    {
        RectTransform rt = Rect(name, parent, anchorMin, anchorMax, position, dimensions);
        Text text = rt.gameObject.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = size;
        text.color = color;
        text.alignment = alignment;
        text.text = value;
        text.raycastTarget = false;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Truncate;
        return text;
    }

    private static void Button(
        string name, Transform parent, string caption, Color accent,
        Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 size)
    {
        RectTransform rt = Rect(name, parent, anchorMin, anchorMax, position, size);
        Image image = rt.gameObject.AddComponent<Image>();
        image.color = PanelAlt;

        Button button = rt.gameObject.AddComponent<Button>();
        ColorBlock colors = button.colors;
        colors.normalColor = PanelAlt;
        colors.highlightedColor = new Color(accent.r, accent.g, accent.b, 0.25f);
        colors.pressedColor = new Color(accent.r, accent.g, accent.b, 0.45f);
        colors.selectedColor = PanelAlt;
        button.colors = colors;
        button.transition = Selectable.Transition.ColorTint;

        Box("[ACCENT]", rt, Vector2.zero, new Vector2(0, 1),
            Vector2.zero, new Vector2(3, 0), accent);

        Label("[LABEL]", rt, caption, 15, White, TextAnchor.MiddleCenter,
            Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        // Intentionally no gameplay listener yet. Connections come after the visual build.
    }

    private static void Header(Transform parent, string title, string subtitle)
    {
        Label("[TITLE]", parent, title, 17, Cyan, TextAnchor.MiddleLeft,
            new Vector2(0, 1), new Vector2(1, 1),
            new Vector2(0, -25), new Vector2(-30, 32));

        Label("[SUBTITLE]", parent, subtitle, 10, Muted, TextAnchor.MiddleLeft,
            new Vector2(0, 1), new Vector2(1, 1),
            new Vector2(0, -48), new Vector2(-30, 20));

        Box("[HEADER RULE]", parent, new Vector2(0, 1), new Vector2(1, 1),
            new Vector2(0, -63), new Vector2(-28, 1), Cyan);
    }

    private static Transform Panel(
        string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax,
        Vector2 position, Vector2 size, string title, string subtitle)
    {
        Image image = Box(name, parent, anchorMin, anchorMax, position, size, PanelColor);
        Header(image.transform, title, subtitle);
        return image.transform;
    }

    private static void BuildTop(Transform root)
    {
        Transform layer = root.Find("[LAYER] TOP STATUS");
        Box("[PANEL] TOP BAR", layer, new Vector2(0.01f, 0.91f),
            new Vector2(0.99f, 0.99f), Vector2.zero, Vector2.zero, Background);

        Label("[BRAND]", layer, "OBSIDIAN  /  PROTOCOL", 20, Cyan,
            TextAnchor.MiddleLeft, new Vector2(0.025f, 0.93f),
            new Vector2(0.25f, 0.98f), Vector2.zero, Vector2.zero);

        Label("[OPERATION]", layer, "AUTONOMOUS WARFARE  //  BATTLEFIELD COMMAND",
            12, White, TextAnchor.MiddleLeft, new Vector2(0.28f, 0.93f),
            new Vector2(0.64f, 0.98f), Vector2.zero, Vector2.zero);

        Label("[NETWORK]", layer, "NETWORK STATUS  /  NOT CONNECTED", 12, Cyan,
            TextAnchor.MiddleCenter, new Vector2(0.69f, 0.93f),
            new Vector2(0.82f, 0.98f), Vector2.zero, Vector2.zero);

        Label("[READINESS]", layer, "FORCE READINESS  /  NO DATA", 12, Amber,
            TextAnchor.MiddleCenter, new Vector2(0.82f, 0.93f),
            new Vector2(0.96f, 0.98f), Vector2.zero, Vector2.zero);

        Label("[TELEMETRY]", layer, "OPERATIONAL STATUS  /  SIMULATION PROTOTYPE",
            10, Muted, TextAnchor.MiddleLeft, new Vector2(0.025f, 0.905f),
            new Vector2(0.55f, 0.925f), Vector2.zero, Vector2.zero);
    }

    private static void BuildLeft(Transform root)
    {
        Transform layer = root.Find("[LAYER] LEFT COMMANDS");
        if (layer == null)
        {
            Debug.LogError("[AAA HUD] LEFT COMMANDS layer not found.");
            return;
        }

        Transform panel = Panel("[PANEL] FORCE COMMAND",
            layer, new Vector2(0.015f, 0.24f), new Vector2(0.215f, 0.895f),
            Vector2.zero, Vector2.zero, "UNIT DATABASE", "REGISTERED UNIT DEFINITIONS");

        ObsidianProtocol.Game.Units.UnitDatabase database =
            UnityEditor.AssetDatabase.LoadAssetAtPath<
                ObsidianProtocol.Game.Units.UnitDatabase>(
                    "Assets/Game/Units/Database/UnitDatabase.asset");

        if (database == null)
        {
            Label("[DATABASE ERROR]", panel,
                "UNIT DATABASE UNAVAILABLE",
                11, Amber, TextAnchor.UpperLeft,
                new Vector2(0, 1), new Vector2(1, 1),
                new Vector2(0, -110), new Vector2(-32, 48));

            Debug.LogError(
                "[AAA HUD] Could not load Assets/Game/Units/Database/UnitDatabase.asset.");
        }
        else
        {
            int total = database.Units.Count;
            int visibleLimit = 8;
            int visibleCount = Mathf.Min(total, visibleLimit);

            Label("[DATABASE SUMMARY]", panel,
                "REGISTERED DEFINITIONS   " + total +
                "\nLISTED BELOW             " + visibleCount,
                11, White, TextAnchor.UpperLeft,
                new Vector2(0, 1), new Vector2(1, 1),
                new Vector2(0, -104), new Vector2(-32, 48));

            int rowIndex = 0;

            foreach (ObsidianProtocol.Game.Units.UnitDefinition unit in database.Units)
            {
                if (unit == null)
                    continue;

                if (rowIndex >= visibleLimit)
                    break;

                float y = -160 - rowIndex * 35;

                Box("[DATABASE UNIT ROW " + rowIndex + "]", panel,
                    new Vector2(0, 1), new Vector2(1, 1),
                    new Vector2(0, y), new Vector2(-28, 30),
                    rowIndex == 0
                        ? new Color(0.04f, 0.14f, 0.15f, 1)
                        : PanelAlt);

                string displayName = string.IsNullOrWhiteSpace(unit.DisplayName)
                    ? "UNNAMED UNIT"
                    : unit.DisplayName.Trim();

                string category = unit.Category.ToString().ToUpperInvariant();

                Label("[DATABASE UNIT LABEL " + rowIndex + "]", panel,
                    displayName + "  /  " + category,
                    10, rowIndex == 0 ? Cyan : White,
                    TextAnchor.MiddleLeft,
                    new Vector2(0, 1), new Vector2(1, 1),
                    new Vector2(0, y), new Vector2(-44, 28));

                Box("[DATABASE UNIT INDICATOR " + rowIndex + "]", panel,
                    new Vector2(1, 1), new Vector2(1, 1),
                    new Vector2(-18, y), new Vector2(5, 5), Cyan);

                rowIndex++;
            }

            if (total > visibleLimit)
            {
                Label("[DATABASE MORE UNITS]", panel,
                    "+" + (total - visibleLimit) +
                    " MORE DEFINITIONS IN DATABASE",
                    9, Muted, TextAnchor.MiddleLeft,
                    new Vector2(0, 1), new Vector2(1, 1),
                    new Vector2(0, -160 - visibleLimit * 35),
                    new Vector2(-32, 26));
            }

            if (total == 0)
            {
                Label("[DATABASE EMPTY]", panel,
                    "NO UNIT DEFINITIONS REGISTERED",
                    10, Amber, TextAnchor.MiddleLeft,
                    new Vector2(0, 1), new Vector2(1, 1),
                    new Vector2(0, -160), new Vector2(-32, 32));
            }
        }

        Button("[BUTTON] FORCE OVERVIEW", panel, "FORCE OVERVIEW", Cyan,
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(0, 106), new Vector2(-28, 40));

        Button("[BUTTON] FORMATIONS", panel, "FORMATIONS", Cyan,
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(0, 60), new Vector2(-28, 40));

        Button("[BUTTON] COMMAND INTENT", panel, "COMMAND INTENT", Amber,
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(0, 14), new Vector2(-28, 40));
    }
    private static void BuildBattlefield(Transform root)
    {
        Transform layer = root.Find("[LAYER] BATTLEFIELD OVERLAY");
        Transform map = Panel("[PANEL] TACTICAL DISPLAY",
            layer, new Vector2(0.225f, 0.245f), new Vector2(0.775f, 0.895f),
            Vector2.zero, Vector2.zero, "TACTICAL OPERATIONS", "SECTOR GRID  /  LIVE BATTLESPACE");

        Box("[MAP FIELD]", map, Vector2.zero, Vector2.one,
            Vector2.zero, new Vector2(-18, -86),
            new Color(0.018f, 0.048f, 0.053f, 0.99f));

        Transform grid = Group("[TACTICAL GRID]", map);
        for (int i = 1; i < 10; i++)
        {
            float x = i / 10f;
            Box("[GRID V " + i + "]", grid, new Vector2(x, 0.02f),
                new Vector2(x, 0.96f), Vector2.zero, new Vector2(1, 0),
                new Color(Cyan.r, Cyan.g, Cyan.b, 0.11f));
        }
        for (int i = 1; i < 8; i++)
        {
            float y = i / 8f;
            Box("[GRID H " + i + "]", grid, new Vector2(0.02f, y),
                new Vector2(0.98f, y), Vector2.zero, new Vector2(0, 1),
                new Color(Cyan.r, Cyan.g, Cyan.b, 0.11f));
        }



        Label("[MAP LEGEND]", map,
            "MAP OVERLAY  /  NOT CONNECTED     //     GRID: LOCAL",
            10, Muted, TextAnchor.MiddleLeft, new Vector2(0, 0),
            new Vector2(1, 0), new Vector2(0, 19), new Vector2(-32, 24));

        Label("[MAP PLACEHOLDER]", map,
            "TACTICAL MAP PLACEHOLDER  //  CONNECT BATTLEFIELD WORLD VIEW",
            11, Amber, TextAnchor.MiddleCenter, new Vector2(0.12f, 0.42f),
            new Vector2(0.88f, 0.58f), Vector2.zero, Vector2.zero);
    }

    private static void BuildRight(Transform root)
    {
        Transform layer = root.Find("[LAYER] RIGHT INTELLIGENCE");
        Transform panel = Panel("[PANEL] INTELLIGENCE",
            layer, new Vector2(0.785f, 0.24f), new Vector2(0.985f, 0.895f),
            Vector2.zero, Vector2.zero, "BATTLESPACE INTELLIGENCE", "SENSORS  /  CONTACTS  /  THREAT");

        Label("[CONTACT SUMMARY]", panel,
            "TRACKED CONTACTS   —\nCONFIRMED HOSTILES  —\nUNIDENTIFIED        —\nSIGNAL INTEGRITY    NO DATA",
            11, White, TextAnchor.UpperLeft, new Vector2(0, 1),
            new Vector2(1, 1), new Vector2(0, -104), new Vector2(-32, 90));

        Box("[INTEL RULE]", panel, new Vector2(0, 1), new Vector2(1, 1),
            new Vector2(0, -211), new Vector2(-28, 1), Cyan);

        Label("[SELECTED CONTACT]", panel, "SELECTED CONTACT", 12, Cyan,
            TextAnchor.MiddleLeft, new Vector2(0, 1), new Vector2(1, 1),
            new Vector2(0, -237), new Vector2(-30, 28));

        Label("[CONTACT DATA]", panel,
            "DESIGNATION   NO CONTACT SELECTED\nCLASSIFICATION NOT AVAILABLE\nLAST UPDATE   NO DATA\nCONFIDENCE     NO DATA",
            11, White, TextAnchor.UpperLeft, new Vector2(0, 1),
            new Vector2(1, 1), new Vector2(0, -288), new Vector2(-32, 80));

        Button("[BUTTON] CONTACT DETAILS", panel, "CONTACT DETAILS", Cyan,
            new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, -390), new Vector2(-28, 38));
        Button("[BUTTON] SENSOR COVERAGE", panel, "SENSOR COVERAGE", Cyan,
            new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, -434), new Vector2(-28, 38));
        Button("[BUTTON] BATTLEFIELD LOGISTICS", panel, "LOGISTICS", Amber,
            new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, -478), new Vector2(-28, 38));
        Button("[BUTTON] OPERATIONAL ORDERS", panel, "OPERATIONAL ORDERS", Cyan,
            new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, -522), new Vector2(-28, 38));
    }

    private static void BuildBottom(Transform root)
    {
        Transform layer = root.Find("[LAYER] BOTTOM CONTROLS");

        Box("[PANEL] BOTTOM BAR", layer, new Vector2(0.225f, 0.045f),
            new Vector2(0.775f, 0.225f), Vector2.zero, Vector2.zero, Background);

        Label("[CONTEXT TITLE]", layer, "COMMAND CONSOLE", 15, Cyan,
            TextAnchor.MiddleLeft, new Vector2(0.24f, 0.183f),
            new Vector2(0.42f, 0.218f), Vector2.zero, Vector2.zero);

        Label("[CONTEXT TEXT]", layer,
            "Select a unit or objective to view available actions.",
            11, Muted, TextAnchor.MiddleLeft, new Vector2(0.24f, 0.15f),
            new Vector2(0.73f, 0.18f), Vector2.zero, Vector2.zero);

        string[] actions = { "SELECT", "MOVE", "ENGAGE", "RECON", "ORDERS" };
        for (int i = 0; i < actions.Length; i++)
        {
            float x = 0.245f + i * 0.103f;
            Button("[BUTTON] ACTION " + actions[i], layer, actions[i], i == 2 ? Amber : Cyan,
                new Vector2(x, 0.075f), new Vector2(x + 0.09f, 0.135f),
                Vector2.zero, Vector2.zero);
        }

        Label("[BOTTOM TELEMETRY]", layer,
            "SIMULATION // VISUAL PROTOTYPE // GAMEPLAY CONNECTIONS PENDING",
            9, Muted, TextAnchor.MiddleRight, new Vector2(0.44f, 0.045f),
            new Vector2(0.765f, 0.068f), Vector2.zero, Vector2.zero);
    }

    private static void BuildWindows(Transform root)
    {
        Transform windows = root.Find("[WINDOWS]");
        string[] names = {
            "OBJECTIVES", "FORCE OVERVIEW", "COMMAND INTENT",
            "FORMATION MANAGEMENT", "DOCTRINE", "CONTACT DETAILS",
            "SENSOR COVERAGE", "NETWORK STATUS", "BATTLEFIELD LOGISTICS",
            "OPERATIONAL ORDERS", "ALERT DETAILS"
        };

        foreach (string name in names)
        {
            Transform window = Panel("[WINDOW] " + name, windows,
                new Vector2(0.28f, 0.25f), new Vector2(0.72f, 0.75f),
                Vector2.zero, Vector2.zero, name, "TACTICAL SYSTEMS  /  PROTOTYPE PANEL");

            Box("[WINDOW CONTENT]", window, new Vector2(0, 0),
                new Vector2(1, 1), new Vector2(0, -24),
                new Vector2(-36, -100), PanelAlt);

            Label("[WINDOW PLACEHOLDER]", window,
                "SYSTEM PANEL READY FOR DATA AND GAMEPLAY CONNECTION",
                13, Muted, TextAnchor.MiddleCenter, new Vector2(0.05f, 0.15f),
                new Vector2(0.95f, 0.75f), Vector2.zero, Vector2.zero);

            Button("[BUTTON] CLOSE " + name, window, "CLOSE", Red,
                new Vector2(1, 1), new Vector2(1, 1),
                new Vector2(-66, -27), new Vector2(92, 34));

            window.gameObject.SetActive(false);
        }
    }
}





