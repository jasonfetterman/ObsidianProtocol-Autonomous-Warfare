using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class EconomyVisualBuilder
{
    private const string ScenePath =
        "Assets/Scenes/SCN-19 ECONOMY/Economy.unity";

    private static readonly Color Black =
        new Color(0.004f,0.008f,0.012f,1f);

    private static readonly Color Floor =
        new Color(0.025f,0.035f,0.045f,1f);

    private static readonly Color Wall =
        new Color(0.055f,0.070f,0.085f,1f);

    private static readonly Color Dark =
        new Color(0.008f,0.015f,0.022f,1f);

    private static readonly Color Panel =
        new Color(0.018f,0.045f,0.065f,1f);

    private static readonly Color Cyan =
        new Color(0.00f,0.90f,1.00f,1f);

    private static readonly Color Green =
        new Color(0.10f,1.00f,0.35f,1f);

    private static readonly Color Amber =
        new Color(1.00f,0.60f,0.05f,1f);

    private static readonly Color Red =
        new Color(1.00f,0.10f,0.10f,1f);

    private static readonly Color White =
        new Color(0.90f,0.96f,1.00f,1f);

    private static Font BuiltinFont;

    private static Canvas HudCanvas;
    private static Text Status;
    private static Text Credits;

    [MenuItem("Obsidian Protocol/Build/SCN-19 ECONOMY - FULL VISUAL")]
    public static void BuildFromMenu()
    {
        Build();
    }

    public static void Build()
    {
        VerifyScene();

        Scene scene =
            EditorSceneManager.OpenScene(
                ScenePath,
                OpenSceneMode.Single);

        ClearScene();

        CreateSystems();
        CreateFacility();
        CreateMainDisplay();
        CreateCommandDesks();
        CreateEconomicStations();
        CreateIndustrialStations();
        CreatePurchaseSystem();
        CreateDeploymentEconomy();
        CreateFleetValuation();
        CreateServerBanks();
        CreateFloorGrid();
        CreateLighting();
        CreateCamera();
        CreateEventSystem();
        CreateHUD();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("============================================================");
        Debug.Log("ECONOMY BUILD COMPLETE");
        Debug.Log(ScenePath);
        Debug.Log("============================================================");
    }

    private static void VerifyScene()
    {
        string absolute =
            System.IO.Path.Combine(
                System.IO.Directory.GetParent(
                    Application.dataPath).FullName,
                ScenePath.Replace(
                    "/",
                    System.IO.Path.DirectorySeparatorChar.ToString()));

        if (!System.IO.File.Exists(absolute))
            throw new Exception(
                "EXACT SCENE DOES NOT EXIST: " + absolute);
    }

    private static void ClearScene()
    {
        GameObject[] roots =
            SceneManager.GetActiveScene()
                .GetRootGameObjects();

        foreach (GameObject root in roots)
            UnityEngine.Object.DestroyImmediate(root);
    }

    private static void CreateSystems()
    {
        GameObject root =
            new GameObject("13. ECONOMY");

        GameObject system =
            new GameObject("[SYSTEM] ECONOMY");

        system.transform.SetParent(root.transform);

        string[] names =
        {
            "RESOURCES",
            "MANUFACTURING",
            "MAINTENANCE",
            "RESEARCH",
            "FLEET VALUE",
            "CREDITS",
            "PURCHASES",
            "DEPLOYMENT RESTRICTIONS"
        };

        foreach (string name in names)
        {
            GameObject obj =
                new GameObject("[SYSTEM] " + name);

            obj.transform.SetParent(system.transform);
        }
    }

    private static void CreateFacility()
    {
        GameObject root =
            new GameObject("[FACILITY] ECONOMY COMMAND CENTER");

        Cube(
            "FLOOR",
            new Vector3(0,-0.5f,0),
            new Vector3(120,1,100),
            Floor,
            root.transform);

        Cube(
            "NORTH WALL",
            new Vector3(0,15,49),
            new Vector3(120,30,1),
            Wall,
            root.transform);

        Cube(
            "WEST WALL",
            new Vector3(-59,15,0),
            new Vector3(1,30,100),
            Wall,
            root.transform);

        Cube(
            "EAST WALL",
            new Vector3(59,15,0),
            new Vector3(1,30,100),
            Wall,
            root.transform);

        Cube(
            "SOUTH WALL LEFT",
            new Vector3(-43,15,-49),
            new Vector3(32,30,1),
            Wall,
            root.transform);

        Cube(
            "SOUTH WALL RIGHT",
            new Vector3(43,15,-49),
            new Vector3(32,30,1),
            Wall,
            root.transform);

        for (int x=-50; x<=50; x+=20)
        {
            Cube(
                "CEILING BEAM",
                new Vector3(x,29,0),
                new Vector3(1.5f,1.5f,96),
                Dark,
                root.transform);
        }

        for (int z=-40; z<=40; z+=20)
        {
            Cube(
                "CROSS BEAM",
                new Vector3(0,28,z),
                new Vector3(116,1,1),
                Dark,
                root.transform);
        }

        for (int x=-50; x<=50; x+=20)
        {
            Column(
                new Vector3(x,12,40),
                root.transform);

            Column(
                new Vector3(x,12,-40),
                root.transform);
        }

        Sign(
            "ECONOMY COMMAND",
            new Vector3(0,23,-48.2f),
            3.0f,
            Cyan,
            Quaternion.identity,
            root.transform);

        Sign(
            "FINANCE // RESOURCE // INDUSTRIAL CONTROL",
            new Vector3(0,19,-48.2f),
            1.15f,
            White,
            Quaternion.identity,
            root.transform);
    }

    private static void Column(
        Vector3 position,
        Transform parent)
    {
        Cube(
            "STRUCTURAL COLUMN",
            position,
            new Vector3(2,24,2),
            Dark,
            parent);

        Cube(
            "COLUMN LIGHT",
            position + new Vector3(1.05f,0,-1.05f),
            new Vector3(0.12f,21,0.12f),
            Cyan,
            parent);
    }

    private static void CreateMainDisplay()
    {
        GameObject root =
            new GameObject("[DISPLAY] ECONOMIC COMMAND WALL");

        root.transform.position =
            new Vector3(0,0,47.5f);

        Cube(
            "DISPLAY FRAME",
            new Vector3(0,13,0),
            new Vector3(92,23,1),
            Dark,
            root.transform);

        Cube(
            "DISPLAY",
            new Vector3(0,13,-0.7f),
            new Vector3(88,20,0.25f),
            Panel,
            root.transform);

        Sign(
            "ECONOMIC COMMAND",
            new Vector3(0,20,-1.0f),
            2.2f,
            Cyan,
            Quaternion.identity,
            root.transform);

        Sign(
            "CREDITS              84,250",
            new Vector3(-23,15,-1),
            1.35f,
            Green,
            Quaternion.identity,
            root.transform);

        Sign(
            "INCOME / HR           +4,920",
            new Vector3(-23,11,-1),
            1.05f,
            Green,
            Quaternion.identity,
            root.transform);

        Sign(
            "EXPENDITURE / HR      -2,080",
            new Vector3(23,15,-1),
            1.35f,
            Amber,
            Quaternion.identity,
            root.transform);

        Sign(
            "NET / HR              +2,840",
            new Vector3(23,11,-1),
            1.05f,
            Cyan,
            Quaternion.identity,
            root.transform);

        for (int i=0;i<8;i++)
        {
            Cube(
                "DISPLAY DATA BAR",
                new Vector3(
                    -34 + i*9.7f,
                    6.5f,
                    -1),
                new Vector3(7,0.12f,0.08f),
                Cyan,
                root.transform);
        }
    }

    private static void CreateCommandDesks()
    {
        Station(
            "FINANCIAL OVERVIEW",
            new Vector3(-37,0,28),
            new Vector3(18,7,9),
            Cyan);

        Station(
            "BUDGET ALLOCATION",
            new Vector3(-17,0,28),
            new Vector3(18,7,9),
            Green);

        Station(
            "TRANSACTIONS",
            new Vector3(17,0,28),
            new Vector3(18,7,9),
            Amber);

        Station(
            "INVESTMENTS",
            new Vector3(37,0,28),
            new Vector3(18,7,9),
            Cyan);
    }

    private static void CreateEconomicStations()
    {
        Station(
            "FINANCIAL CONTROLS",
            new Vector3(-40,0,12),
            new Vector3(22,7,11),
            Red);

        Station(
            "RESOURCE VALUE",
            new Vector3(-13,0,12),
            new Vector3(22,7,11),
            Green);

        Station(
            "MANUFACTURING COSTS",
            new Vector3(13,0,12),
            new Vector3(22,7,11),
            Amber);

        Station(
            "MAINTENANCE COSTS",
            new Vector3(40,0,12),
            new Vector3(22,7,11),
            Red);
    }

    private static void CreateIndustrialStations()
    {
        Station(
            "RESEARCH FUNDING",
            new Vector3(-39,0,-6),
            new Vector3(23,7,12),
            Cyan);

        Station(
            "RESOURCE BANK",
            new Vector3(-12,0,-6),
            new Vector3(23,7,12),
            Green);

        Station(
            "FLEET VALUE",
            new Vector3(15,0,-6),
            new Vector3(23,7,12),
            Cyan);

        Station(
            "ECONOMIC DATA",
            new Vector3(42,0,-6),
            new Vector3(20,7,12),
            Amber);
    }

    private static void Station(
        string name,
        Vector3 position,
        Vector3 size,
        Color accent)
    {
        GameObject root =
            new GameObject("[ZONE] " + name);

        root.transform.position =
            position;

        Cube(
            "BASE",
            new Vector3(0,0.6f,0),
            new Vector3(size.x,1.2f,size.z),
            Dark,
            root.transform);

        Cube(
            "CONSOLE",
            new Vector3(0,2.3f,-size.z*0.25f),
            new Vector3(size.x*0.72f,2.8f,1.8f),
            Panel,
            root.transform);

        Cube(
            "SCREEN FRAME",
            new Vector3(0,6,0.5f),
            new Vector3(size.x*0.82f,4.2f,0.7f),
            Dark,
            root.transform);

        Cube(
            "SCREEN",
            new Vector3(0,6,-0.1f),
            new Vector3(size.x*0.76f,3.6f,0.18f),
            Panel,
            root.transform);

        Sign(
            name,
            new Vector3(0,6.5f,-0.35f),
            Mathf.Clamp(size.x/8f,0.8f,1.6f),
            accent,
            Quaternion.identity,
            root.transform);

        Cube(
            "ACCENT",
            new Vector3(
                0,
                0.12f,
                size.z*0.45f),
            new Vector3(
                size.x*0.75f,
                0.10f,
                0.10f),
            accent,
            root.transform);
    }

    private static void CreatePurchaseSystem()
    {
        GameObject root =
            new GameObject("[WINDOW] PURCHASE SYSTEM");

        root.transform.position =
            new Vector3(-26,0,-28);

        Station(
            "PURCHASE SYSTEM",
            root.transform.position,
            new Vector3(28,8,13),
            Amber);

        string[] options =
        {
            "UNIT PURCHASES",
            "EQUIPMENT",
            "CUSTOMIZATION",
            "CONVENIENCE"
        };

        for (int i=0;i<4;i++)
        {
            float x = -9 + i*6;

            Cube(
                "PURCHASE TERMINAL",
                new Vector3(
                    root.transform.position.x+x,
                    4,
                    root.transform.position.z-4),
                new Vector3(4,5,2),
                Panel,
                null);

            Sign(
                options[i],
                new Vector3(
                    root.transform.position.x+x,
                    6.8f,
                    root.transform.position.z-5.1f),
                0.65f,
                Amber,
                Quaternion.identity,
                null);
        }
    }

    private static void CreateDeploymentEconomy()
    {
        GameObject root =
            new GameObject("[PANEL] DEPLOYMENT ECONOMY");

        root.transform.position =
            new Vector3(6,0,-28);

        Cube(
            "DEPLOYMENT PLATFORM",
            new Vector3(6,1,-28),
            new Vector3(30,2,13),
            Dark,
            null);

        Cube(
            "DEPLOYMENT SCREEN",
            new Vector3(6,8,-28),
            new Vector3(28,10,0.8f),
            Panel,
            null);

        Sign(
            "DEPLOYMENT ECONOMY",
            new Vector3(6,10,-28.6f),
            1.7f,
            Cyan,
            Quaternion.identity,
            null);

        Sign(
            "BATTLE BUDGET  10,000 DP",
            new Vector3(6,7.5f,-28.6f),
            1.15f,
            Green,
            Quaternion.identity,
            null);

        Sign(
            "AVAILABLE 10,000 DP",
            new Vector3(6,5.4f,-28.6f),
            1.0f,
            Green,
            Quaternion.identity,
            null);

        Sign(
            "POWER LIMIT ENFORCED",
            new Vector3(6,3.5f,-28.6f),
            0.9f,
            Red,
            Quaternion.identity,
            null);
    }

    private static void CreateFleetValuation()
    {
        Station(
            "FLEET VALUATION",
            new Vector3(39,0,-28),
            new Vector3(24,8,13),
            Cyan);
    }

    private static void CreateServerBanks()
    {
        ServerRack(
            new Vector3(-51,0,1),
            Cyan);

        ServerRack(
            new Vector3(51,0,1),
            Green);
    }

    private static void ServerRack(
        Vector3 position,
        Color accent)
    {
        for (int i=-2;i<=2;i++)
        {
            Cube(
                "SERVER RACK",
                position +
                new Vector3(
                    0,
                    4,
                    i*3),
                new Vector3(4,8,2),
                Dark,
                null);

            Cube(
                "SERVER STATUS LIGHT",
                position +
                new Vector3(
                    -2.1f,
                    4,
                    i*3),
                new Vector3(
                    0.10f,
                    5.5f,
                    0.10f),
                accent,
                null);
        }
    }

    private static void CreateFloorGrid()
    {
        GameObject root =
            new GameObject(
                "[VISUAL] ECONOMY DATA GRID");

        for (int x=-50;x<=50;x+=10)
        {
            Cube(
                "FLOOR DATA LINE",
                new Vector3(x,0.03f,0),
                new Vector3(0.10f,0.05f,94),
                Cyan,
                root.transform);
        }

        for (int z=-40;z<=40;z+=10)
        {
            Cube(
                "FLOOR CROSS LINE",
                new Vector3(0,0.04f,z),
                new Vector3(114,0.05f,0.10f),
                Cyan,
                root.transform);
        }

        Cube(
            "CENTRAL PLATFORM",
            new Vector3(0,0.08f,0),
            new Vector3(18,0.12f,18),
            Dark,
            root.transform);

        Sign(
            "OP // ECONOMY",
            new Vector3(0,0.22f,0),
            1.5f,
            Cyan,
            Quaternion.Euler(90,0,0),
            root.transform);
    }

    private static void CreateLighting()
    {
        GameObject root =
            new GameObject(
                "[LIGHTING] ECONOMY");

        GameObject sun =
            new GameObject(
                "MAIN LIGHT",
                typeof(Light));

        sun.transform.SetParent(
            root.transform);

        sun.transform.rotation =
            Quaternion.Euler(50,-30,0);

        Light dl =
            sun.GetComponent<Light>();

        dl.type =
            LightType.Directional;

        dl.intensity = 1.4f;
        dl.color =
            new Color(
                0.75f,
                0.85f,
                1f);

        dl.shadows =
            LightShadows.Soft;

        Vector3[] points =
        {
            new Vector3(-40,24,-30),
            new Vector3(-20,24,-30),
            new Vector3(0,24,-30),
            new Vector3(20,24,-30),
            new Vector3(40,24,-30),
            new Vector3(-40,24,0),
            new Vector3(0,24,0),
            new Vector3(40,24,0),
            new Vector3(-40,24,30),
            new Vector3(0,24,30),
            new Vector3(40,24,30)
        };

        foreach (Vector3 position in points)
        {
            GameObject lightObject =
                new GameObject(
                    "CEILING LIGHT",
                    typeof(Light));

            lightObject.transform.SetParent(
                root.transform);

            lightObject.transform.position =
                position;

            Light light =
                lightObject.GetComponent<Light>();

            light.type =
                LightType.Point;

            light.range = 30;
            light.intensity = 9;
            light.color = Cyan;
        }
    }

    private static void CreateCamera()
    {
        GameObject obj =
            new GameObject(
                "ECONOMY CAMERA",
                typeof(Camera),
                typeof(AudioListener));

        obj.transform.position =
            new Vector3(
                0,
                10,
                -37);

        Vector3 target =
            new Vector3(
                0,
                8,
                25);

        obj.transform.rotation =
            Quaternion.LookRotation(
                target - obj.transform.position,
                Vector3.up);

        Camera camera =
            obj.GetComponent<Camera>();

        camera.clearFlags =
            CameraClearFlags.SolidColor;

        camera.backgroundColor =
            Black;

        camera.fieldOfView = 72;
        camera.nearClipPlane = 0.1f;
        camera.farClipPlane = 500;

        camera.tag = "MainCamera";

        Debug.Log(
            "CAMERA POSITION: " +
            obj.transform.position);
    }

    private static void CreateEventSystem()
    {
        new GameObject(
            "EVENT SYSTEM",
            typeof(EventSystem),
            typeof(InputSystemUIInputModule));
    }

    // =========================================================
    // HUD
    // =========================================================

    private static void CreateHUD()
    {
        GameObject root =
            new GameObject(
                "[HUD] FINANCE HUD",
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));

        HudCanvas =
            root.GetComponent<Canvas>();

        HudCanvas.renderMode =
            RenderMode.ScreenSpaceOverlay;

        HudCanvas.sortingOrder = 100;

        CanvasScaler scaler =
            root.GetComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(1920,1080);

        PanelUI(
            "TOP BAR",
            root.transform,
            new Vector2(0,-30),
            new Vector2(1900,70));

        TextUI(
            "13. ECONOMY // FINANCE COMMAND",
            root.transform,
            new Vector2(-650,-30),
            new Vector2(700,50),
            28,
            Cyan,
            TextAnchor.MiddleLeft);

        Credits =
            TextUI(
                "CREDITS 84,250",
                root.transform,
                new Vector2(650,-30),
                new Vector2(350,50),
                24,
                Green,
                TextAnchor.MiddleRight);

        PanelUI(
            "FINANCE OVERVIEW",
            root.transform,
            new Vector2(-700,-230),
            new Vector2(420,300));

        TextUI(
            "FINANCIAL OVERVIEW",
            root.transform,
            new Vector2(-700,-120),
            new Vector2(360,45),
            22,
            Cyan,
            TextAnchor.MiddleLeft);

        TextUI(
            "CURRENT CREDITS       84,250\n\nINCOME RATE       +4,920 / HR\n\nEXPENDITURE       -2,080 / HR\n\nNET BALANCE       +2,840 / HR\n\nFORECAST             STABLE",
            root.transform,
            new Vector2(-700,-260),
            new Vector2(370,230),
            17,
            White,
            TextAnchor.UpperLeft);

        PanelUI(
            "DEPLOYMENT ECONOMY",
            root.transform,
            new Vector2(700,-230),
            new Vector2(420,300));

        TextUI(
            "DEPLOYMENT ECONOMY",
            root.transform,
            new Vector2(700,-120),
            new Vector2(370,45),
            22,
            Cyan,
            TextAnchor.MiddleLeft);

        TextUI(
            "UNIT DEPLOYMENT COST\nVARIES BY UNIT / LOADOUT\n\nBATTLE BUDGET     10,000 DP\n\nAVAILABLE         10,000 DP\n\nPOWER LIMIT ENFORCED",
            root.transform,
            new Vector2(700,-260),
            new Vector2(370,230),
            17,
            White,
            TextAnchor.UpperLeft);

        PanelUI(
            "BOTTOM STATUS",
            root.transform,
            new Vector2(0,500),
            new Vector2(1900,60));

        Status =
            TextUI(
                "SYSTEM READY // ECONOMIC NETWORK NOMINAL",
                root.transform,
                new Vector2(-800,500),
                new Vector2(1100,45),
                17,
                Cyan,
                TextAnchor.MiddleLeft);

        ButtonUI(
            "PURCHASE",
            root.transform,
            new Vector2(400,500),
            () =>
            {
                Status.text =
                    "PURCHASE SYSTEM // CONFIRMATION REQUIRED";
            });

        ButtonUI(
            "AUDIT",
            root.transform,
            new Vector2(550,500),
            () =>
            {
                Status.text =
                    "AUDIT COMPLETE // NO EXCEPTIONS";
            });

        ButtonUI(
            "FORECAST",
            root.transform,
            new Vector2(700,500),
            () =>
            {
                Status.text =
                    "FORECAST UPDATED // ECONOMIC STATUS STABLE";
            });
    }

    private static GameObject PanelUI(
        string name,
        Transform parent,
        Vector2 position,
        Vector2 size)
    {
        GameObject obj =
            new GameObject(
                "[PANEL] " + name,
                typeof(RectTransform),
                typeof(Image));

        obj.transform.SetParent(
            parent,
            false);

        RectTransform rt =
            obj.GetComponent<RectTransform>();

        rt.sizeDelta = size;
        rt.anchoredPosition = position;

        Image image =
            obj.GetComponent<Image>();

        image.color =
            new Color(
                0.008f,
                0.022f,
                0.034f,
                0.94f);

        return obj;
    }

    private static Text TextUI(
        string text,
        Transform parent,
        Vector2 position,
        Vector2 size,
        int fontSize,
        Color color,
        TextAnchor alignment)
    {
        GameObject obj =
            new GameObject(
                "DISPLAY",
                typeof(RectTransform),
                typeof(Text));

        obj.transform.SetParent(
            parent,
            false);

        RectTransform rt =
            obj.GetComponent<RectTransform>();

        rt.sizeDelta = size;
        rt.anchoredPosition = position;

        Text t =
            obj.GetComponent<Text>();

        t.text = text;
        t.font = GetFont();
        t.fontSize = fontSize;
        t.color = color;
        t.alignment = alignment;
        t.horizontalOverflow =
            HorizontalWrapMode.Overflow;
        t.verticalOverflow =
            VerticalWrapMode.Overflow;

        return t;
    }

    private static void ButtonUI(
        string label,
        Transform parent,
        Vector2 position,
        Action action)
    {
        GameObject obj =
            new GameObject(
                "[BUTTON] " + label,
                typeof(RectTransform),
                typeof(Image),
                typeof(Button));

        obj.transform.SetParent(
            parent,
            false);

        RectTransform rt =
            obj.GetComponent<RectTransform>();

        rt.sizeDelta =
            new Vector2(130,42);

        rt.anchoredPosition =
            position;

        Image image =
            obj.GetComponent<Image>();

        image.color =
            new Color(
                0.02f,
                0.12f,
                0.16f,
                1f);

        Button button =
            obj.GetComponent<Button>();

        button.onClick.AddListener(
            () => action());

        TextUI(
            label,
            obj.transform,
            Vector2.zero,
            new Vector2(120,38),
            14,
            White,
            TextAnchor.MiddleCenter);
    }

    // =========================================================
    // 3D HELPERS
    // =========================================================

    private static GameObject Cube(
        string name,
        Vector3 position,
        Vector3 scale,
        Color color,
        Transform parent)
    {
        GameObject obj =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube);

        obj.name = name;

        if (parent != null)
            obj.transform.SetParent(
                parent);

        obj.transform.localPosition =
            position;

        obj.transform.localScale =
            scale;

        Renderer renderer =
            obj.GetComponent<Renderer>();

        renderer.sharedMaterial =
            Material(color);

        return obj;
    }

    private static Material Material(
        Color color)
    {
        Shader shader =
            Shader.Find(
                "Universal Render Pipeline/Lit");

        if (shader == null)
            shader =
                Shader.Find("Standard");

        Material material =
            new Material(shader);

        material.color =
            color;

        if (material.HasProperty("_BaseColor"))
            material.SetColor(
                "_BaseColor",
                color);

        if (material.HasProperty("_Metallic"))
            material.SetFloat(
                "_Metallic",
                0.45f);

        if (material.HasProperty("_Smoothness"))
            material.SetFloat(
                "_Smoothness",
                0.75f);

        return material;
    }

    private static void Sign(
        string text,
        Vector3 position,
        float size,
        Color color,
        Quaternion rotation,
        Transform parent)
    {
        GameObject obj =
            new GameObject(
                "[SIGN] " + text);

        if (parent != null)
            obj.transform.SetParent(
                parent);

        obj.transform.position =
            position;

        obj.transform.rotation =
            rotation;

        TextMesh mesh =
            obj.AddComponent<TextMesh>();

        mesh.text = text;
        mesh.font = GetFont();
        mesh.fontSize = 64;
        mesh.characterSize =
            size * 0.10f;
        mesh.anchor =
            TextAnchor.MiddleCenter;
        mesh.alignment =
            TextAlignment.Center;
        mesh.color =
            color;
    }

    private static Font GetFont()
    {
        if (BuiltinFont == null)
        {
            BuiltinFont =
                Resources.GetBuiltinResource<Font>(
                    "LegacyRuntime.ttf");
        }

        return BuiltinFont;
    }
}


