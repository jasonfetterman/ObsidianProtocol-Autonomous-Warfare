using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class IntelligenceVisualBuilder
{
    private const string SCENE =
        "Assets/Scenes/SCN-18 INTELLIGENCE/Intellegence.unity";

    private static readonly Color Black =
        new Color(0.006f, 0.010f, 0.014f, 1f);

    private static readonly Color Floor =
        new Color(0.015f, 0.022f, 0.029f, 1f);

    private static readonly Color Wall =
        new Color(0.030f, 0.040f, 0.050f, 1f);

    private static readonly Color Panel =
        new Color(0.008f, 0.016f, 0.024f, 0.97f);

    private static readonly Color Panel2 =
        new Color(0.016f, 0.027f, 0.038f, 0.98f);

    private static readonly Color White =
        new Color(0.86f, 0.91f, 0.95f, 1f);

    private static readonly Color Dim =
        new Color(0.45f, 0.54f, 0.61f, 1f);

    private static readonly Color Cyan =
        new Color(0.08f, 0.72f, 0.92f, 1f);

    private static readonly Color Blue =
        new Color(0.18f, 0.42f, 0.90f, 1f);

    private static readonly Color Green =
        new Color(0.16f, 0.82f, 0.48f, 1f);

    private static readonly Color Amber =
        new Color(0.94f, 0.63f, 0.16f, 1f);

    private static readonly Color Red =
        new Color(0.88f, 0.18f, 0.20f, 1f);

    private static Transform root;
    private static Transform architecture;
    private static Transform intelligence;
    private static Transform sensors;
    private static Transform network;
    private static Transform surveillance;
    private static Transform lighting;

    [MenuItem("Obsidian Protocol/Build/INTELLIGENCE - FULL VISUAL")]
    public static void Build()
    {
        if (!File.Exists(
            Path.Combine(
                Directory.GetCurrentDirectory(),
                SCENE.Replace("/", Path.DirectorySeparatorChar.ToString()))))
        {
            Debug.LogError(
                "INTELLIGENCE BUILD ABORTED.\n\n" +
                "Required existing scene was not found:\n" +
                SCENE +
                "\n\nNo duplicate scene was created.");

            return;
        }

        Scene scene =
            EditorSceneManager.OpenScene(
                SCENE,
                OpenSceneMode.Single);

        ClearScene();

        CreateRoots();

        BuildFacility();
        BuildContactCommand();
        BuildSensorArray();
        BuildNetworkCenter();
        BuildThreatAnalysis();
        BuildSurveillanceCenter();
        BuildReportArchive();
        BuildStrategicMap();
        BuildOperationsLink();
        BuildCamera();
        BuildLighting();
        BuildEventSystem();
        BuildHUD();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "====================================================\n" +
            "INTELLIGENCE FULL VISUAL BUILD COMPLETE\n" +
            "====================================================\n" +
            "Scene:\n" +
            SCENE);
    }

    // ============================================================
    // ROOTS
    // ============================================================

    private static void CreateRoots()
    {
        root =
            new GameObject(
                "07. INTELLIGENCE").transform;

        architecture =
            CreateRoot(
                "INTELLIGENCE FACILITY",
                root);

        intelligence =
            CreateRoot(
                "INTELLIGENCE SYSTEMS",
                root);

        sensors =
            CreateRoot(
                "SENSOR NETWORK",
                root);

        network =
            CreateRoot(
                "COMMUNICATIONS NETWORK",
                root);

        surveillance =
            CreateRoot(
                "SURVEILLANCE",
                root);

        lighting =
            CreateRoot(
                "LIGHTING",
                root);
    }

    private static Transform CreateRoot(
        string name,
        Transform parent)
    {
        GameObject go =
            new GameObject(name);

        go.transform.SetParent(parent);

        return go.transform;
    }

    private static void ClearScene()
    {
        Scene scene =
            SceneManager.GetActiveScene();

        foreach (GameObject obj in scene.GetRootGameObjects())
            Object.DestroyImmediate(obj);
    }

    // ============================================================
    // FACILITY
    // ============================================================

    private static void BuildFacility()
    {
        Cube(
            "INTELLIGENCE FLOOR",
            new Vector3(0f, -0.5f, 5f),
            new Vector3(86f, 1f, 66f),
            Floor,
            architecture);

        Cube(
            "NORTH WALL",
            new Vector3(0f, 8f, 38f),
            new Vector3(86f, 17f, 1f),
            Wall,
            architecture);

        Cube(
            "SOUTH WALL",
            new Vector3(0f, 8f, -28f),
            new Vector3(86f, 17f, 1f),
            Wall,
            architecture);

        Cube(
            "WEST WALL",
            new Vector3(-43f, 8f, 5f),
            new Vector3(1f, 17f, 66f),
            Wall,
            architecture);

        Cube(
            "EAST WALL",
            new Vector3(43f, 8f, 5f),
            new Vector3(1f, 17f, 66f),
            Wall,
            architecture);

        Cube(
            "CEILING",
            new Vector3(0f, 17f, 5f),
            new Vector3(86f, 1f, 66f),
            Black,
            architecture);

        // Central intelligence floor
        Cube(
            "TACTICAL FLOOR",
            new Vector3(0f, 0.05f, 5f),
            new Vector3(16f, 0.08f, 56f),
            new Color(0.018f, 0.060f, 0.075f, 1f),
            architecture);

        // Data lanes
        for (int x = -36; x <= 36; x += 6)
        {
            Cube(
                "DATA FLOOR LINE",
                new Vector3(x, 0.04f, 5f),
                new Vector3(0.10f, 0.05f, 58f),
                new Color(0.035f, 0.15f, 0.19f, 1f),
                architecture);
        }

        // Structural columns
        for (int x = -36; x <= 36; x += 12)
        {
            Cube(
                "NORTH STRUCTURAL COLUMN",
                new Vector3(x, 7.5f, 36f),
                new Vector3(1.2f, 15f, 1.2f),
                Panel2,
                architecture);

            Cube(
                "SOUTH STRUCTURAL COLUMN",
                new Vector3(x, 7.5f, -26f),
                new Vector3(1.2f, 15f, 1.2f),
                Panel2,
                architecture);
        }

        Sign(
            "07. INTELLIGENCE",
            new Vector3(0f, 12f, 37.3f),
            3.2f,
            White,
            architecture);

        Sign(
            "INTELLIGENCE / SURVEILLANCE / ANALYSIS COMMAND",
            new Vector3(0f, 9.3f, 37.2f),
            1.15f,
            Cyan,
            architecture);

        Sign(
            "CONTACTS",
            new Vector3(-30f, 9f, 36.7f),
            1.3f,
            Cyan,
            architecture);

        Sign(
            "SENSORS",
            new Vector3(-10f, 9f, 36.7f),
            1.3f,
            Green,
            architecture);

        Sign(
            "NETWORK",
            new Vector3(10f, 9f, 36.7f),
            1.3f,
            Blue,
            architecture);

        Sign(
            "ANALYSIS",
            new Vector3(30f, 9f, 36.7f),
            1.3f,
            Amber,
            architecture);
    }

    // ============================================================
    // CONTACT COMMAND
    // ============================================================

    private static void BuildContactCommand()
    {
        Transform r =
            CreateRoot(
                "CONTACTS",
                intelligence);

        WallConsole(
            "CONTACT COMMAND",
            new Vector3(-30f, 4.5f, 30.5f),
            new Vector3(18f, 8f, 1f),
            Cyan,
            r);

        Screen(
            "UNKNOWN CONTACTS",
            new Vector3(-35f, 6.5f, 29.8f),
            new[]
            {
                "UNKNOWN CONTACTS       17",
                "UNCLASSIFIED           11",
                "UNCONFIRMED             6",
                "LAST DETECTION       00:04"
            },
            Cyan,
            r);

        Screen(
            "IDENTIFIED CONTACTS",
            new Vector3(-25f, 6.5f, 29.8f),
            new[]
            {
                "IDENTIFIED             42",
                "FRIENDLY               26",
                "HOSTILE                11",
                "NEUTRAL                 5"
            },
            Green,
            r);

        Terminal(
            "UNKNOWN",
            new Vector3(-35f, 1.4f, 26f),
            Cyan,
            r);

        Terminal(
            "SUSPECTED",
            new Vector3(-30f, 1.4f, 26f),
            Amber,
            r);

        Terminal(
            "IDENTIFIED",
            new Vector3(-25f, 1.4f, 26f),
            Green,
            r);

        Terminal(
            "TRACKED",
            new Vector3(-20f, 1.4f, 26f),
            Blue,
            r);
    }

    // ============================================================
    // SENSOR ARRAY
    // ============================================================

    private static void BuildSensorArray()
    {
        Transform r =
            CreateRoot(
                "SENSORS",
                sensors);

        WallConsole(
            "SENSOR COMMAND",
            new Vector3(-10f, 4.5f, 30.5f),
            new Vector3(18f, 8f, 1f),
            Green,
            r);

        Screen(
            "ACTIVE SENSORS",
            new Vector3(-15f, 6.5f, 29.8f),
            new[]
            {
                "ACTIVE SENSORS         38",
                "GROUND                 17",
                "AIR                    15",
                "NAVAL                   6"
            },
            Green,
            r);

        Screen(
            "SENSOR COVERAGE",
            new Vector3(-5f, 6.5f, 29.8f),
            new[]
            {
                "DETECTION RANGE      18.6 KM",
                "COVERAGE              84%",
                "OVERLAP                71%",
                "BLIND ZONES             4"
            },
            Cyan,
            r);

        Terminal(
            "SENSOR ARRAY A",
            new Vector3(-15f, 1.4f, 26f),
            Green,
            r);

        Terminal(
            "SENSOR ARRAY B",
            new Vector3(-10f, 1.4f, 26f),
            Green,
            r);

        Terminal(
            "SENSOR ARRAY C",
            new Vector3(-5f, 1.4f, 26f),
            Green,
            r);

        Terminal(
            "DAMAGE MONITOR",
            new Vector3(0f, 1.4f, 26f),
            Amber,
            r);
    }

    // ============================================================
    // NETWORK
    // ============================================================

    private static void BuildNetworkCenter()
    {
        Transform r =
            CreateRoot(
                "NETWORK",
                network);

        WallConsole(
            "NETWORK COMMAND",
            new Vector3(11f, 4.5f, 30.5f),
            new Vector3(20f, 8f, 1f),
            Blue,
            r);

        Screen(
            "COMMUNICATIONS",
            new Vector3(6f, 6.5f, 29.8f),
            new[]
            {
                "COMMUNICATIONS         ONLINE",
                "CHANNELS                64",
                "SECURE CHANNELS         41",
                "ENCRYPTION             ACTIVE"
            },
            Blue,
            r);

        Screen(
            "DATA LINKS",
            new Vector3(16f, 6.5f, 29.8f),
            new[]
            {
                "ACTIVE DATA LINKS       92",
                "RELAYS                  28",
                "SIGNAL STRENGTH         94%",
                "LATENCY                 38 MS"
            },
            Cyan,
            r);

        Terminal(
            "COMMS",
            new Vector3(6f, 1.4f, 26f),
            Blue,
            r);

        Terminal(
            "RELAYS",
            new Vector3(11f, 1.4f, 26f),
            Blue,
            r);

        Terminal(
            "DATA LINKS",
            new Vector3(16f, 1.4f, 26f),
            Cyan,
            r);

        Terminal(
            "FAILURE MONITOR",
            new Vector3(21f, 1.4f, 26f),
            Red,
            r);
    }

    // ============================================================
    // THREAT ANALYSIS
    // ============================================================

    private static void BuildThreatAnalysis()
    {
        Transform r =
            CreateRoot(
                "THREAT ANALYSIS",
                intelligence);

        WallConsole(
            "THREAT ANALYSIS CENTER",
            new Vector3(31f, 4.5f, 30.5f),
            new Vector3(18f, 8f, 1f),
            Amber,
            r);

        Screen(
            "THREAT STATUS",
            new Vector3(26f, 6.5f, 29.8f),
            new[]
            {
                "ACTIVE THREATS          9",
                "EMERGING                4",
                "NEUTRALIZED            17",
                "ARCHIVED                83"
            },
            Red,
            r);

        Screen(
            "ANALYSIS CONFIDENCE",
            new Vector3(36f, 6.5f, 29.8f),
            new[]
            {
                "THREAT LEVEL           HIGH",
                "CONFIDENCE              91%",
                "SOURCE RELIABILITY      87%",
                "ANALYST CONSENSUS       84%"
            },
            Amber,
            r);

        Terminal(
            "ACTIVE THREATS",
            new Vector3(26f, 1.4f, 26f),
            Red,
            r);

        Terminal(
            "EMERGING",
            new Vector3(31f, 1.4f, 26f),
            Amber,
            r);

        Terminal(
            "NEUTRALIZED",
            new Vector3(36f, 1.4f, 26f),
            Green,
            r);
    }

    // ============================================================
    // SURVEILLANCE
    // ============================================================

    private static void BuildSurveillanceCenter()
    {
        Transform r =
            CreateRoot(
                "SURVEILLANCE CENTER",
                surveillance);

        Cube(
            "SURVEILLANCE PLATFORM",
            new Vector3(0f, 0.3f, 8f),
            new Vector3(42f, 0.6f, 18f),
            Panel2,
            r);

        Screen(
            "SURVEILLANCE FEEDS",
            new Vector3(-12f, 7f, 17f),
            new[]
            {
                "ACTIVE FEEDS             24",
                "GROUND FEEDS              9",
                "AIR FEEDS                 8",
                "NAVAL FEEDS               4",
                "SATELLITE FEEDS           3"
            },
            Cyan,
            r);

        Screen(
            "SIGNAL INTEGRITY",
            new Vector3(0f, 7f, 17f),
            new[]
            {
                "PRIMARY SIGNAL            98%",
                "SECONDARY SIGNAL          91%",
                "DATA QUALITY              94%",
                "INTERFERENCE               3%"
            },
            Green,
            r);

        Screen(
            "ANALYSIS QUEUE",
            new Vector3(12f, 7f, 17f),
            new[]
            {
                "PENDING ANALYSIS          18",
                "PROCESSING                 7",
                "COMPLETED                241",
                "AUTOMATED                 92%"
            },
            Blue,
            r);

        Terminal(
            "FEED CONTROL",
            new Vector3(-12f, 1.5f, 5f),
            Cyan,
            r);

        Terminal(
            "SIGNAL CONTROL",
            new Vector3(-6f, 1.5f, 5f),
            Green,
            r);

        Terminal(
            "ANALYSIS CONTROL",
            new Vector3(0f, 1.5f, 5f),
            Blue,
            r);

        Terminal(
            "DATA PROCESSOR",
            new Vector3(6f, 1.5f, 5f),
            Cyan,
            r);

        Terminal(
            "ARCHIVE",
            new Vector3(12f, 1.5f, 5f),
            Amber,
            r);
    }

    // ============================================================
    // REPORT ARCHIVE
    // ============================================================

    private static void BuildReportArchive()
    {
        Transform r =
            CreateRoot(
                "INTELLIGENCE REPORTS",
                intelligence);

        WallConsole(
            "INTELLIGENCE REPORT ARCHIVE",
            new Vector3(-27f, 4f, 1f),
            new Vector3(24f, 8f, 1f),
            Cyan,
            r);

        Screen(
            "RECENT REPORTS",
            new Vector3(-34f, 6.5f, 0.3f),
            new[]
            {
                "IR-2047   HOSTILE MOVEMENT",
                "IR-2046   SENSOR ANOMALY",
                "IR-2045   SUPPLY ROUTE",
                "IR-2044   AIR CONTACT"
            },
            Cyan,
            r);

        Screen(
            "ARCHIVED REPORTS",
            new Vector3(-24f, 6.5f, 0.3f),
            new[]
            {
                "ARCHIVED REPORTS       684",
                "LAST 24 HOURS           31",
                "CLASSIFIED              117",
                "DECLASSIFIED              8"
            },
            Blue,
            r);

        Terminal(
            "REPORT TERMINAL A",
            new Vector3(-34f, 1.2f, -3.5f),
            Cyan,
            r);

        Terminal(
            "REPORT TERMINAL B",
            new Vector3(-28f, 1.2f, -3.5f),
            Blue,
            r);

        Terminal(
            "REPORT TERMINAL C",
            new Vector3(-22f, 1.2f, -3.5f),
            Cyan,
            r);
    }

    // ============================================================
    // STRATEGIC MAP
    // ============================================================

    private static void BuildStrategicMap()
    {
        Transform r =
            CreateRoot(
                "STRATEGIC INTELLIGENCE MAP",
                intelligence);

        Cube(
            "MAP PLATFORM",
            new Vector3(15f, 0.6f, -7f),
            new Vector3(42f, 1.2f, 22f),
            new Color(0.012f, 0.025f, 0.035f, 1f),
            r);

        Cube(
            "MAP TABLE",
            new Vector3(15f, 2.3f, -7f),
            new Vector3(35f, 0.25f, 16f),
            new Color(0.008f, 0.030f, 0.040f, 1f),
            r);

        for (int x = -2; x <= 32; x += 5)
        {
            Cube(
                "MAP GRID X",
                new Vector3(x, 2.46f, -7f),
                new Vector3(0.06f, 0.03f, 15f),
                new Color(0.04f, 0.22f, 0.27f, 1f),
                r);
        }

        for (int z = -14; z <= 1; z += 5)
        {
            Cube(
                "MAP GRID Z",
                new Vector3(15f, 2.47f, z),
                new Vector3(34f, 0.03f, 0.06f),
                new Color(0.04f, 0.22f, 0.27f, 1f),
                r);
        }

        MapContact(
            "HOSTILE",
            new Vector3(23f, 2.7f, -5f),
            Red,
            r);

        MapContact(
            "HOSTILE",
            new Vector3(7f, 2.7f, -10f),
            Red,
            r);

        MapContact(
            "FRIENDLY",
            new Vector3(15f, 2.7f, -3f),
            Green,
            r);

        MapContact(
            "TRACKED",
            new Vector3(26f, 2.7f, -11f),
            Amber,
            r);

        MapContact(
            "SENSOR",
            new Vector3(5f, 2.7f, -3f),
            Cyan,
            r);

        Sign(
            "STRATEGIC INTELLIGENCE MAP",
            new Vector3(15f, 5.3f, -18f),
            1.25f,
            Cyan,
            r);

        Sign(
            "LIVE INTELLIGENCE DATA",
            new Vector3(15f, 3.9f, -18f),
            0.75f,
            Green,
            r);
    }

    private static void MapContact(
        string label,
        Vector3 position,
        Color color,
        Transform parent)
    {
        Cube(
            label + " CONTACT",
            position,
            new Vector3(0.8f, 0.12f, 0.8f),
            color,
            parent);

        CreatePointLight(
            "MAP CONTACT LIGHT",
            position + Vector3.up,
            color,
            1.5f,
            4f);
    }

    // ============================================================
    // OPERATIONS LINK
    // ============================================================

    private static void BuildOperationsLink()
    {
        Transform r =
            CreateRoot(
                "OPERATIONS LINK",
                intelligence);

        Cube(
            "OPERATIONS COMMAND PLATFORM",
            new Vector3(-14f, 0.5f, -14f),
            new Vector3(28f, 1f, 11f),
            Panel2,
            r);

        Sign(
            "INTELLIGENCE → OPERATIONS",
            new Vector3(-14f, 5.8f, -19f),
            1.25f,
            Cyan,
            r);

        Sign(
            "FORWARD VERIFIED INTELLIGENCE",
            new Vector3(-14f, 4.2f, -19f),
            0.70f,
            Dim,
            r);

        CommandStation(
            "FORWARD TO COMMAND",
            new Vector3(-22f, 1.5f, -14f),
            Cyan,
            r);

        CommandStation(
            "FORWARD TO FLEET",
            new Vector3(-14f, 1.5f, -14f),
            Blue,
            r);

        CommandStation(
            "FORWARD TO RESEARCH",
            new Vector3(-6f, 1.5f, -14f),
            Green,
            r);
    }

    // ============================================================
    // CAMERA
    // ============================================================

    private static void BuildCamera()
    {
        GameObject cameraObject =
            new GameObject(
                "INTELLIGENCE CAMERA",
                typeof(Camera),
                typeof(AudioListener));

        Camera camera =
            cameraObject.GetComponent<Camera>();

        camera.clearFlags =
            CameraClearFlags.SolidColor;

        camera.backgroundColor =
            Black;

        camera.fieldOfView = 68f;
        camera.nearClipPlane = 0.05f;
        camera.farClipPlane = 1000f;
        camera.depth = -100f;

        cameraObject.transform.position =
            new Vector3(0f, 25f, -72f);

        cameraObject.transform.rotation =
            Quaternion.LookRotation(
                new Vector3(0f, 6f, 8f) -
                cameraObject.transform.position);

        camera.tag = "MainCamera";
    }

    // ============================================================
    // LIGHTING
    // ============================================================

    private static void BuildLighting()
    {
        RenderSettings.ambientMode =
            UnityEngine.Rendering.AmbientMode.Flat;

        RenderSettings.ambientLight =
            new Color(0.018f, 0.028f, 0.038f, 1f);

        GameObject directional =
            new GameObject(
                "INTELLIGENCE KEY LIGHT",
                typeof(Light));

        Light dl =
            directional.GetComponent<Light>();

        dl.type =
            LightType.Directional;

        dl.intensity =
            0.75f;

        directional.transform.rotation =
            Quaternion.Euler(48f, -25f, 0f);

        directional.transform.SetParent(lighting);

        CreatePointLight(
            "CONTACT CYAN LIGHT",
            new Vector3(-30f, 11f, 30f),
            Cyan,
            7f,
            24f);

        CreatePointLight(
            "SENSOR GREEN LIGHT",
            new Vector3(-10f, 11f, 30f),
            Green,
            6f,
            22f);

        CreatePointLight(
            "NETWORK BLUE LIGHT",
            new Vector3(12f, 11f, 30f),
            Blue,
            6f,
            22f);

        CreatePointLight(
            "ANALYSIS AMBER LIGHT",
            new Vector3(31f, 11f, 30f),
            Amber,
            6f,
            22f);

        CreatePointLight(
            "MAP LIGHT",
            new Vector3(15f, 8f, -7f),
            Cyan,
            5f,
            20f);

        for (int x = -36; x <= 36; x += 12)
        {
            CreatePointLight(
                "CEILING INTELLIGENCE LIGHT",
                new Vector3(x, 13f, 7f),
                new Color(0.05f, 0.20f, 0.28f, 1f),
                2.5f,
                18f);
        }
    }

    private static void CreatePointLight(
        string name,
        Vector3 position,
        Color color,
        float intensity,
        float range)
    {
        GameObject go =
            new GameObject(
                name,
                typeof(Light));

        Light light =
            go.GetComponent<Light>();

        light.type =
            LightType.Point;

        light.color =
            color;

        light.intensity =
            intensity;

        light.range =
            range;

        go.transform.position =
            position;

        go.transform.SetParent(lighting);
    }

    // ============================================================
    // EVENT SYSTEM
    // ============================================================

    private static void BuildEventSystem()
    {
        GameObject go =
            new GameObject(
                "EVENT SYSTEM",
                typeof(EventSystem),
                typeof(InputSystemUIInputModule));

        go.transform.SetParent(root);
    }

    // ============================================================
    // HUD
    // ============================================================

    private static void BuildHUD()
    {
        GameObject canvas =
            new GameObject(
                "INTELLIGENCE HUD",
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));

        Canvas c =
            canvas.GetComponent<Canvas>();

        c.renderMode =
            RenderMode.ScreenSpaceOverlay;

        c.sortingOrder = 100;

        CanvasScaler scaler =
            canvas.GetComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(1920f, 1080f);

        scaler.screenMatchMode =
            CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        scaler.matchWidthOrHeight = 0.5f;

        Header(canvas.transform);
        ContactsPanel(canvas.transform);
        SensorsPanel(canvas.transform);
        NetworkPanel(canvas.transform);
        ThreatPanel(canvas.transform);
        ReportsPanel(canvas.transform);
        SurveillancePanel(canvas.transform);
        AnalysisPanel(canvas.transform);
        OperationsPanel(canvas.transform);
        StrategicMapOverlay(canvas.transform);
    }

    // ============================================================
    // HEADER
    // ============================================================

    private static void Header(Transform parent)
    {
        GameObject p =
            UIPanel(
                "HEADER",
                parent,
                new Vector2(0.5f, 1f),
                new Vector2(0f, -50f),
                new Vector2(1920f, 100f),
                Panel);

        UIText(
            "TITLE",
            p.transform,
            "07. INTELLIGENCE",
            34,
            White,
            TextAnchor.MiddleLeft,
            new Vector2(32f, 5f),
            new Vector2(500f, 55f));

        UIText(
            "SUBTITLE",
            p.transform,
            "INTELLIGENCE / SURVEILLANCE / ANALYSIS COMMAND",
            16,
            Cyan,
            TextAnchor.MiddleLeft,
            new Vector2(35f, -27f),
            new Vector2(720f, 35f));

        UIText(
            "STATUS",
            p.transform,
            "INTELLIGENCE NETWORK ONLINE    |    42 IDENTIFIED    |    17 UNKNOWN    |    9 ACTIVE THREATS",
            18,
            Green,
            TextAnchor.MiddleRight,
            new Vector2(-35f, 0f),
            new Vector2(1050f, 45f));
    }

    // ============================================================
    // CONTACTS
    // ============================================================

    private static void ContactsPanel(Transform parent)
    {
        GameObject p =
            UIPanel(
                "CONTACTS",
                parent,
                new Vector2(0f, 0.5f),
                new Vector2(195f, 20f),
                new Vector2(365f, 760f),
                Panel);

        UIText(
            "TITLE",
            p.transform,
            "CONTACTS",
            24,
            Cyan,
            TextAnchor.MiddleLeft,
            new Vector2(20f, 340f),
            new Vector2(300f, 45f));

        string[] buttons =
        {
            "UNKNOWN",
            "SUSPECTED",
            "IDENTIFIED",
            "TRACKED",
            "LOST CONTACT"
        };

        for (int i = 0; i < buttons.Length; i++)
        {
            UIButton(
                buttons[i],
                p.transform,
                new Vector2(0f, 275f - i * 62f),
                new Vector2(315f, 48f),
                i == 0 ? Cyan : Dim);
        }

        UIText(
            "CONTACT SUMMARY",
            p.transform,
            "CONTACT SUMMARY\n\nUNKNOWN             17\nSUSPECTED             9\nIDENTIFIED           42\nTRACKED               31\nLOST CONTACT           6\n\nTOTAL CONTACTS       105",
            15,
            White,
            TextAnchor.UpperLeft,
            new Vector2(25f, -90f),
            new Vector2(300f, 230f));
    }

    // ============================================================
    // SENSORS
    // ============================================================

    private static void SensorsPanel(Transform parent)
    {
        GameObject p =
            UIPanel(
                "SENSORS",
                parent,
                new Vector2(0f, 0.5f),
                new Vector2(585f, 215f),
                new Vector2(330f, 370f),
                Panel);

        UIText(
            "TITLE",
            p.transform,
            "SENSORS",
            22,
            Green,
            TextAnchor.MiddleLeft,
            new Vector2(20f, 145f),
            new Vector2(270f, 42f));

        UIText(
            "DATA",
            p.transform,
            "ACTIVE SENSORS\n\n38 ACTIVE\n\nDETECTION RANGE\n18.6 KM\n\nCOVERAGE\n84%\n\nSENSOR DAMAGE\n3%",
            15,
            White,
            TextAnchor.UpperLeft,
            new Vector2(25f, 95f),
            new Vector2(270f, 250f));

        UIButton(
            "SENSOR CONTROL",
            p.transform,
            new Vector2(0f, -140f),
            new Vector2(270f, 44f),
            Green);
    }

    // ============================================================
    // NETWORK
    // ============================================================

    private static void NetworkPanel(Transform parent)
    {
        GameObject p =
            UIPanel(
                "NETWORK",
                parent,
                new Vector2(0f, 0.5f),
                new Vector2(585f, -190f),
                new Vector2(330f, 370f),
                Panel);

        UIText(
            "TITLE",
            p.transform,
            "NETWORK",
            22,
            Blue,
            TextAnchor.MiddleLeft,
            new Vector2(20f, 145f),
            new Vector2(270f, 42f));

        UIText(
            "DATA",
            p.transform,
            "COMMUNICATIONS\nONLINE\n\nRELAYS\n28 ACTIVE\n\nDATA LINKS\n92 ACTIVE\n\nSIGNAL STRENGTH\n94%",
            15,
            White,
            TextAnchor.UpperLeft,
            new Vector2(25f, 95f),
            new Vector2(270f, 250f));

        UIButton(
            "NETWORK CONTROL",
            p.transform,
            new Vector2(0f, -140f),
            new Vector2(270f, 44f),
            Blue);
    }

    // ============================================================
    // CENTER THREAT / MAP
    // ============================================================

    private static void ThreatPanel(Transform parent)
    {
        GameObject p =
            UIPanel(
                "THREAT ANALYSIS",
                parent,
                new Vector2(0.5f, 0.5f),
                new Vector2(170f, 120f),
                new Vector2(670f, 400f),
                Panel);

        UIText(
            "TITLE",
            p.transform,
            "THREAT ANALYSIS",
            24,
            Amber,
            TextAnchor.MiddleLeft,
            new Vector2(25f, 165f),
            new Vector2(500f, 42f));

        UIText(
            "THREAT",
            p.transform,
            "THREAT LEVEL             HIGH\n\nACTIVE THREATS             9\nEMERGING                   4\nNEUTRALIZED               17\nARCHIVED                  83\n\nCONFIDENCE                 91%\nSOURCE RELIABILITY         87%",
            16,
            White,
            TextAnchor.UpperLeft,
            new Vector2(-255f, 120f),
            new Vector2(330f, 240f));

        UIButton(
            "GENERATE SUMMARY",
            p.transform,
            new Vector2(150f, -140f),
            new Vector2(220f, 48f),
            Amber);

        UIButton(
            "ANALYZE CONTACT",
            p.transform,
            new Vector2(-100f, -140f),
            new Vector2(220f, 48f),
            Cyan);
    }

    // ============================================================
    // REPORTS
    // ============================================================

    private static void ReportsPanel(Transform parent)
    {
        GameObject p =
            UIPanel(
                "INTELLIGENCE REPORTS",
                parent,
                new Vector2(0.5f, 0.5f),
                new Vector2(170f, -160f),
                new Vector2(670f, 350f),
                Panel);

        UIText(
            "TITLE",
            p.transform,
            "INTELLIGENCE REPORTS",
            22,
            Cyan,
            TextAnchor.MiddleLeft,
            new Vector2(25f, 140f),
            new Vector2(500f, 40f));

        UIText(
            "RECENT",
            p.transform,
            "RECENT REPORTS\n\nIR-2047  HOSTILE MOVEMENT\nIR-2046  SENSOR ANOMALY\nIR-2045  SUPPLY ROUTE\nIR-2044  AIR CONTACT",
            14,
            White,
            TextAnchor.UpperLeft,
            new Vector2(-250f, 95f),
            new Vector2(310f, 180f));

        UIText(
            "ARCHIVE",
            p.transform,
            "ARCHIVED REPORTS\n\n684 TOTAL\n31 LAST 24 HOURS\n117 CLASSIFIED\n8 DECLASSIFIED",
            14,
            Dim,
            TextAnchor.UpperLeft,
            new Vector2(80f, 95f),
            new Vector2(250f, 180f));

        UIButton(
            "OPEN REPORT",
            p.transform,
            new Vector2(0f, -130f),
            new Vector2(220f, 46f),
            Cyan);
    }

    // ============================================================
    // SURVEILLANCE
    // ============================================================

    private static void SurveillancePanel(Transform parent)
    {
        GameObject p =
            UIPanel(
                "SURVEILLANCE",
                parent,
                new Vector2(1f, 0.5f),
                new Vector2(-205f, 185f),
                new Vector2(370f, 370f),
                Panel);

        UIText(
            "TITLE",
            p.transform,
            "SURVEILLANCE",
            22,
            Cyan,
            TextAnchor.MiddleLeft,
            new Vector2(20f, 145f),
            new Vector2(320f, 42f));

        UIText(
            "DATA",
            p.transform,
            "ACTIVE FEEDS             24\n\nSIGNAL INTEGRITY         94%\n\nPRIMARY SIGNAL           98%\nSECONDARY SIGNAL         91%\n\nANALYSIS QUEUE           18\nPROCESSING                7",
            15,
            White,
            TextAnchor.UpperLeft,
            new Vector2(25f, 95f),
            new Vector2(310f, 210f));

        UIButton(
            "VIEW FEED",
            p.transform,
            new Vector2(-80f, -125f),
            new Vector2(140f, 45f),
            Cyan);

        UIButton(
            "ANALYZE DATA",
            p.transform,
            new Vector2(85f, -125f),
            new Vector2(150f, 45f),
            Blue);
    }

    // ============================================================
    // ANALYSIS
    // ============================================================

    private static void AnalysisPanel(Transform parent)
    {
        GameObject p =
            UIPanel(
                "ANALYSIS",
                parent,
                new Vector2(1f, 0.5f),
                new Vector2(-205f, -185f),
                new Vector2(370f, 370f),
                Panel);

        UIText(
            "TITLE",
            p.transform,
            "ANALYSIS",
            22,
            Amber,
            TextAnchor.MiddleLeft,
            new Vector2(20f, 145f),
            new Vector2(320f, 42f));

        UIText(
            "DATA",
            p.transform,
            "THREAT LEVEL\nHIGH\n\nCONFIDENCE\n91%\n\nSOURCE RELIABILITY\n87%\n\nANALYST CONSENSUS\n84%",
            16,
            White,
            TextAnchor.UpperLeft,
            new Vector2(25f, 95f),
            new Vector2(310f, 220f));

        UIButton(
            "GENERATE SUMMARY",
            p.transform,
            new Vector2(0f, -125f),
            new Vector2(260f, 46f),
            Amber);
    }

    // ============================================================
    // OPERATIONS LINK
    // ============================================================

    private static void OperationsPanel(Transform parent)
    {
        GameObject p =
            UIPanel(
                "OPERATIONS LINK",
                parent,
                new Vector2(0.5f, 0f),
                new Vector2(165f, 75f),
                new Vector2(680f, 110f),
                Panel);

        UIText(
            "TITLE",
            p.transform,
            "OPERATIONS LINK",
            19,
            Cyan,
            TextAnchor.MiddleLeft,
            new Vector2(-290f, 28f),
            new Vector2(240f, 35f));

        UIButton(
            "FORWARD TO COMMAND",
            p.transform,
            new Vector2(-125f, -20f),
            new Vector2(190f, 44f),
            Cyan);

        UIButton(
            "FORWARD TO FLEET",
            p.transform,
            new Vector2(80f, -20f),
            new Vector2(170f, 44f),
            Blue);

        UIButton(
            "FORWARD TO RESEARCH",
            p.transform,
            new Vector2(265f, -20f),
            new Vector2(190f, 44f),
            Green);
    }

    // ============================================================
    // STRATEGIC MAP OVERLAY
    // ============================================================

    private static void StrategicMapOverlay(
        Transform parent)
    {
        GameObject p =
            UIPanel(
                "STRATEGIC INTELLIGENCE MAP",
                parent,
                new Vector2(0.5f, 0.5f),
                new Vector2(0f, 5f),
                new Vector2(900f, 760f),
                new Color(0.005f, 0.012f, 0.018f, 0.97f));

        UIText(
            "MAP TITLE",
            p.transform,
            "STRATEGIC INTELLIGENCE MAP",
            25,
            Cyan,
            TextAnchor.MiddleLeft,
            new Vector2(25f, 335f),
            new Vector2(600f, 45f));

        // Map field
        GameObject map =
            UIPanel(
                "MAP FIELD",
                p.transform,
                new Vector2(0.5f, 0.5f),
                new Vector2(0f, 35f),
                new Vector2(820f, 560f),
                new Color(0.006f, 0.028f, 0.038f, 1f));

        // Grid
        for (int i = -7; i <= 7; i++)
        {
            Image vertical =
                UIImage(
                    "MAP VERTICAL GRID",
                    map.transform,
                    new Vector2(i * 50f, 0f),
                    new Vector2(1f, 530f),
                    new Color(0.04f, 0.18f, 0.22f, 0.55f));

            Image horizontal =
                UIImage(
                    "MAP HORIZONTAL GRID",
                    map.transform,
                    new Vector2(0f, i * 35f),
                    new Vector2(810f, 1f),
                    new Color(0.04f, 0.18f, 0.22f, 0.55f));
        }

        MapMarker(
            map.transform,
            "HOSTILE",
            new Vector2(210f, 130f),
            Red);

        MapMarker(
            map.transform,
            "HOSTILE",
            new Vector2(-220f, -100f),
            Red);

        MapMarker(
            map.transform,
            "FRIENDLY",
            new Vector2(0f, 80f),
            Green);

        MapMarker(
            map.transform,
            "TRACKED",
            new Vector2(270f, -120f),
            Amber);

        MapMarker(
            map.transform,
            "SENSOR",
            new Vector2(-280f, 160f),
            Cyan);

        UIText(
            "MAP LEGEND",
            p.transform,
            "LIVE DATA    •    HOSTILE    •    FRIENDLY    •    TRACKED    •    SENSOR",
            14,
            Dim,
            TextAnchor.MiddleCenter,
            new Vector2(0f, -285f),
            new Vector2(780f, 35f));

        UIButton(
            "OPEN FULL MAP",
            p.transform,
            new Vector2(-120f, -320f),
            new Vector2(220f, 48f),
            Cyan);

        UIButton(
            "CLOSE MAP",
            p.transform,
            new Vector2(125f, -320f),
            new Vector2(180f, 48f),
            Dim);
    }

    // ============================================================
    // PHYSICAL OBJECT HELPERS
    // ============================================================

    private static void WallConsole(
        string name,
        Vector3 position,
        Vector3 scale,
        Color accent,
        Transform parent)
    {
        Cube(
            name + " BODY",
            position,
            scale,
            Panel2,
            parent);

        Cube(
            name + " ACCENT",
            position + new Vector3(
                0f,
                -scale.y * 0.47f,
                -0.55f),
            new Vector3(
                scale.x * 0.90f,
                0.12f,
                0.08f),
            accent,
            parent);
    }

    private static void Screen(
        string title,
        Vector3 position,
        string[] lines,
        Color accent,
        Transform parent)
    {
        Cube(
            title + " DISPLAY",
            position,
            new Vector3(8.2f, 3.7f, 0.18f),
            new Color(0.005f, 0.018f, 0.028f, 1f),
            parent);

        Cube(
            title + " FRAME",
            position + new Vector3(0f, 0f, 0.12f),
            new Vector3(8.5f, 4.0f, 0.08f),
            new Color(
                accent.r * 0.35f,
                accent.g * 0.35f,
                accent.b * 0.35f,
                1f),
            parent);

        Sign(
            title,
            position + new Vector3(0f, 1.25f, -0.18f),
            0.68f,
            accent,
            parent);

        string text = "";

        foreach (string line in lines)
            text += line + "\n";

        Sign(
            text,
            position + new Vector3(0f, -0.2f, -0.18f),
            0.43f,
            White,
            parent);
    }

    private static void Terminal(
        string name,
        Vector3 position,
        Color accent,
        Transform parent)
    {
        Cube(
            name + " TERMINAL BODY",
            position,
            new Vector3(2.8f, 1.8f, 2.2f),
            Panel2,
            parent);

        Cube(
            name + " TERMINAL SCREEN",
            position + new Vector3(0f, 1.1f, 0.25f),
            new Vector3(2.3f, 1.3f, 0.16f),
            new Color(0.005f, 0.018f, 0.027f, 1f),
            parent);

        Cube(
            name + " TERMINAL LIGHT",
            position + new Vector3(0f, 0.25f, -1.15f),
            new Vector3(1.8f, 0.12f, 0.08f),
            accent,
            parent);
    }

    private static void CommandStation(
        string name,
        Vector3 position,
        Color accent,
        Transform parent)
    {
        Cube(
            name + " DESK",
            position,
            new Vector3(6f, 1.2f, 3.2f),
            Panel2,
            parent);

        Cube(
            name + " DISPLAY",
            position + new Vector3(0f, 1.5f, 0.75f),
            new Vector3(4.8f, 2.3f, 0.18f),
            new Color(0.005f, 0.018f, 0.027f, 1f),
            parent);

        Sign(
            name,
            position + new Vector3(0f, 1.5f, 0.48f),
            0.45f,
            accent,
            parent);
    }

    private static void Sign(
        string text,
        Vector3 position,
        float size,
        Color color,
        Transform parent)
    {
        GameObject go =
            new GameObject(
                "SIGN - " + text.Replace("\n", " "));

        go.transform.SetParent(parent);
        go.transform.position = position;
        go.transform.rotation =
            Quaternion.Euler(0f, 180f, 0f);

        TextMesh tm =
            go.AddComponent<TextMesh>();

        tm.text = text;
        tm.fontSize =
            Mathf.Max(12, Mathf.RoundToInt(42f * size));

        tm.characterSize =
            0.025f * size;

        tm.anchor =
            TextAnchor.MiddleCenter;

        tm.alignment =
            TextAlignment.Center;

        tm.color =
            color;
    }

    private static GameObject Cube(
        string name,
        Vector3 position,
        Vector3 scale,
        Color color,
        Transform parent)
    {
        GameObject go =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube);

        go.name = name;
        go.transform.SetParent(parent);
        go.transform.position = position;
        go.transform.localScale = scale;

        Renderer renderer =
            go.GetComponent<Renderer>();

        Shader shader =
            Shader.Find(
                "Universal Render Pipeline/Lit");

        if (shader == null)
            shader = Shader.Find("Standard");

        Material material =
            new Material(shader);

        material.color = color;

        renderer.sharedMaterial =
            material;

        return go;
    }

    // ============================================================
    // UI HELPERS
    // ============================================================

    private static GameObject UIPanel(
        string name,
        Transform parent,
        Vector2 anchor,
        Vector2 position,
        Vector2 size,
        Color color)
    {
        GameObject go =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image));

        go.transform.SetParent(parent, false);

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        rect.pivot =
            new Vector2(0.5f, 0.5f);

        rect.anchoredPosition =
            position;

        rect.sizeDelta =
            size;

        Image image =
            go.GetComponent<Image>();

        image.color =
            color;

        return go;
    }

    private static Image UIImage(
        string name,
        Transform parent,
        Vector2 position,
        Vector2 size,
        Color color)
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

        rect.anchorMin =
            new Vector2(0.5f, 0.5f);

        rect.anchorMax =
            new Vector2(0.5f, 0.5f);

        rect.pivot =
            new Vector2(0.5f, 0.5f);

        rect.anchoredPosition =
            position;

        rect.sizeDelta =
            size;

        Image image =
            go.GetComponent<Image>();

        image.color =
            color;

        return image;
    }

    private static Text UIText(
        string name,
        Transform parent,
        string text,
        int fontSize,
        Color color,
        TextAnchor alignment,
        Vector2 position,
        Vector2 size)
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

        rect.anchorMin =
            new Vector2(0.5f, 0.5f);

        rect.anchorMax =
            new Vector2(0.5f, 0.5f);

        rect.pivot =
            new Vector2(0.5f, 0.5f);

        rect.anchoredPosition =
            position;

        rect.sizeDelta =
            size;

        Text textComponent =
            go.GetComponent<Text>();

        textComponent.text =
            text;

        textComponent.font =
            Resources.GetBuiltinResource<Font>(
                "LegacyRuntime.ttf");

        textComponent.fontSize =
            fontSize;

        textComponent.color =
            color;

        textComponent.alignment =
            alignment;

        textComponent.horizontalOverflow =
            HorizontalWrapMode.Wrap;

        textComponent.verticalOverflow =
            VerticalWrapMode.Overflow;

        return textComponent;
    }

    private static Button UIButton(
        string label,
        Transform parent,
        Vector2 position,
        Vector2 size,
        Color accent)
    {
        GameObject go =
            new GameObject(
                "BUTTON - " + label,
                typeof(RectTransform),
                typeof(Image),
                typeof(Button));

        go.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0.5f, 0.5f);

        rect.anchorMax =
            new Vector2(0.5f, 0.5f);

        rect.pivot =
            new Vector2(0.5f, 0.5f);

        rect.anchoredPosition =
            position;

        rect.sizeDelta =
            size;

        Image image =
            go.GetComponent<Image>();

        image.color =
            new Color(
                accent.r * 0.12f,
                accent.g * 0.12f,
                accent.b * 0.12f,
                0.98f);

        Button button =
            go.GetComponent<Button>();

        ColorBlock colors =
            button.colors;

        colors.normalColor =
            image.color;

        colors.highlightedColor =
            new Color(
                accent.r * 0.30f,
                accent.g * 0.30f,
                accent.b * 0.30f,
                1f);

        colors.pressedColor =
            new Color(
                accent.r * 0.50f,
                accent.g * 0.50f,
                accent.b * 0.50f,
                1f);

        colors.selectedColor =
            colors.highlightedColor;

        button.colors =
            colors;

        UIText(
            "LABEL",
            go.transform,
            label,
            15,
            White,
            TextAnchor.MiddleCenter,
            Vector2.zero,
            size - new Vector2(10f, 8f));

        return button;
    }

    private static void MapMarker(
        Transform parent,
        string label,
        Vector2 position,
        Color color)
    {
        GameObject marker =
            new GameObject(
                "MAP MARKER - " + label,
                typeof(RectTransform),
                typeof(Image));

        marker.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            marker.GetComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0.5f, 0.5f);

        rect.anchorMax =
            new Vector2(0.5f, 0.5f);

        rect.pivot =
            new Vector2(0.5f, 0.5f);

        rect.anchoredPosition =
            position;

        rect.sizeDelta =
            new Vector2(18f, 18f);

        Image image =
            marker.GetComponent<Image>();

        image.color =
            color;

        UIText(
            "LABEL",
            marker.transform,
            label,
            10,
            color,
            TextAnchor.MiddleCenter,
            new Vector2(0f, -20f),
            new Vector2(100f, 22f));
    }
}
