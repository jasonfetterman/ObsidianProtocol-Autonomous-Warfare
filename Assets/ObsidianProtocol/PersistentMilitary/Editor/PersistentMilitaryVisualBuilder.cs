using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class PersistentMilitaryVisualBuilder
{
    private const string SCENE =
        "Assets/Scenes/SCN-17 PERSISTENT MILITARY/Persistent_Military.unity";

    private static readonly Color Black =
        new Color(0.008f, 0.012f, 0.016f, 1f);

    private static readonly Color Floor =
        new Color(0.018f, 0.025f, 0.032f, 1f);

    private static readonly Color Wall =
        new Color(0.035f, 0.045f, 0.055f, 1f);

    private static readonly Color Panel =
        new Color(0.012f, 0.020f, 0.028f, 0.97f);

    private static readonly Color Panel2 =
        new Color(0.020f, 0.030f, 0.040f, 0.98f);

    private static readonly Color White =
        new Color(0.84f, 0.90f, 0.94f, 1f);

    private static readonly Color Dim =
        new Color(0.48f, 0.56f, 0.62f, 1f);

    private static readonly Color Cyan =
        new Color(0.10f, 0.72f, 0.90f, 1f);

    private static readonly Color Green =
        new Color(0.18f, 0.82f, 0.48f, 1f);

    private static readonly Color Amber =
        new Color(0.92f, 0.62f, 0.18f, 1f);

    private static readonly Color Red =
        new Color(0.86f, 0.20f, 0.20f, 1f);

    private static readonly Color Blue =
        new Color(0.18f, 0.42f, 0.82f, 1f);

    private static Transform facilityRoot;
    private static Transform architectureRoot;
    private static Transform systemsRoot;
    private static Transform recordsRoot;
    private static Transform developmentRoot;
    private static Transform lightingRoot;

    [MenuItem("Obsidian Protocol/Build/PERSISTENT MILITARY - FULL VISUAL")]
    public static void Build()
    {
        if (!EnsureSceneExists())
        {
            Debug.LogError(
                "PERSISTENT MILITARY: Required scene was not found at " + SCENE +
                ". No duplicate scene was created.");
            return;
        }

        Scene scene = EditorSceneManager.OpenScene(
            SCENE,
            OpenSceneMode.Single);

        ClearScene();

        CreateRoots();
        BuildFacility();
        BuildMilitaryRecords();
        BuildVeteranArchive();
        BuildDamageRepairArchive();
        BuildLossesMemorial();
        BuildEquipmentHistory();
        BuildOperationalHistory();
        BuildMilitaryDevelopment();
        BuildCommandCenter();
        BuildCamera();
        BuildLighting();
        BuildEventSystem();
        BuildHUD();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "PERSISTENT MILITARY FULL VISUAL BUILD COMPLETE\n" +
            "Scene: " + SCENE);
    }

    private static bool EnsureSceneExists()
    {
        if (File.Exists(
            Path.Combine(
                Directory.GetCurrentDirectory(),
                SCENE.Replace("/", Path.DirectorySeparatorChar.ToString()))))
        {
            return true;
        }

        Debug.LogError(
            "Required Persistent Military scene does not exist:\n" +
            SCENE +
            "\n\nNo replacement or duplicate scene will be created.");

        return false;
    }

    private static void ClearScene()
    {
        Scene scene = SceneManager.GetActiveScene();

        GameObject[] objects = scene.GetRootGameObjects();

        foreach (GameObject obj in objects)
        {
            UnityEngine.Object.DestroyImmediate(obj);
        }
    }

    private static void CreateRoots()
    {
        facilityRoot =
            new GameObject("16. PERSISTENT MILITARY").transform;

        architectureRoot =
            CreateRoot("FACILITY ARCHITECTURE", facilityRoot);

        systemsRoot =
            CreateRoot("PERSISTENT MILITARY SYSTEMS", facilityRoot);

        recordsRoot =
            CreateRoot("MILITARY RECORDS", facilityRoot);

        developmentRoot =
            CreateRoot("MILITARY DEVELOPMENT", facilityRoot);

        lightingRoot =
            CreateRoot("LIGHTING", facilityRoot);
    }

    private static Transform CreateRoot(
        string name,
        Transform parent)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent);
        return go.transform;
    }

    // ============================================================
    // FACILITY
    // ============================================================

    private static void BuildFacility()
    {
        CreateCube(
            "FACILITY FLOOR",
            new Vector3(0f, -0.5f, 6f),
            new Vector3(82f, 1f, 64f),
            Floor,
            architectureRoot);

        CreateCube(
            "NORTH WALL",
            new Vector3(0f, 8f, 38f),
            new Vector3(82f, 17f, 1f),
            Wall,
            architectureRoot);

        CreateCube(
            "SOUTH WALL",
            new Vector3(0f, 8f, -26f),
            new Vector3(82f, 17f, 1f),
            Wall,
            architectureRoot);

        CreateCube(
            "WEST WALL",
            new Vector3(-41f, 8f, 6f),
            new Vector3(1f, 17f, 64f),
            Wall,
            architectureRoot);

        CreateCube(
            "EAST WALL",
            new Vector3(41f, 8f, 6f),
            new Vector3(1f, 17f, 64f),
            Wall,
            architectureRoot);

        CreateCube(
            "CEILING",
            new Vector3(0f, 17f, 6f),
            new Vector3(82f, 1f, 64f),
            new Color(0.012f, 0.018f, 0.024f, 1f),
            architectureRoot);

        // Main aisle
        CreateCube(
            "CENTRAL COMMAND AISLE",
            new Vector3(0f, 0.03f, 7f),
            new Vector3(11f, 0.08f, 55f),
            new Color(0.025f, 0.065f, 0.080f, 1f),
            architectureRoot);

        // Floor strips
        for (int x = -36; x <= 36; x += 6)
        {
            CreateCube(
                "FLOOR DATA STRIP",
                new Vector3(x, 0.04f, 7f),
                new Vector3(0.12f, 0.05f, 57f),
                new Color(0.04f, 0.16f, 0.20f, 1f),
                architectureRoot);
        }

        // Structural columns
        for (int x = -36; x <= 36; x += 12)
        {
            CreateCube(
                "STRUCTURAL COLUMN",
                new Vector3(x, 7.5f, 36.5f),
                new Vector3(1.2f, 15f, 1.2f),
                new Color(0.025f, 0.035f, 0.045f, 1f),
                architectureRoot);

            CreateCube(
                "STRUCTURAL COLUMN",
                new Vector3(x, 7.5f, -24.5f),
                new Vector3(1.2f, 15f, 1.2f),
                new Color(0.025f, 0.035f, 0.045f, 1f),
                architectureRoot);
        }

        CreateSign(
            "PERSISTENT MILITARY",
            new Vector3(0f, 11f, 37.35f),
            3.4f,
            Cyan,
            architectureRoot);

        CreateSign(
            "PERMANENT MILITARY RECORDS COMMAND",
            new Vector3(0f, 8.7f, 37.30f),
            1.25f,
            Dim,
            architectureRoot);

        CreateSign(
            "UNIT HISTORY",
            new Vector3(-30f, 9f, 36.9f),
            1.4f,
            Cyan,
            architectureRoot);

        CreateSign(
            "VETERAN ARCHIVE",
            new Vector3(-10f, 9f, 36.9f),
            1.4f,
            Green,
            architectureRoot);

        CreateSign(
            "DAMAGE / REPAIR",
            new Vector3(10f, 9f, 36.9f),
            1.4f,
            Amber,
            architectureRoot);

        CreateSign(
            "MILITARY DEVELOPMENT",
            new Vector3(30f, 9f, 36.9f),
            1.4f,
            Blue,
            architectureRoot);
    }

    // ============================================================
    // UNIT HISTORY
    // ============================================================

    private static void BuildMilitaryRecords()
    {
        Transform root =
            CreateRoot("UNIT HISTORY ARCHIVE", recordsRoot);

        CreateWallConsole(
            "UNIT HISTORY COMMAND WALL",
            new Vector3(-30f, 4f, 31f),
            new Vector3(17f, 8f, 1f),
            Cyan,
            root);

        CreateTerminal(
            "UNIT HISTORY TERMINAL A",
            new Vector3(-34f, 1.4f, 27f),
            Cyan,
            root);

        CreateTerminal(
            "UNIT HISTORY TERMINAL B",
            new Vector3(-29f, 1.4f, 27f),
            Cyan,
            root);

        CreateTerminal(
            "UNIT HISTORY TERMINAL C",
            new Vector3(-24f, 1.4f, 27f),
            Cyan,
            root);

        CreateRecordScreen(
            "UNIT RECORD — BULLDOG-024",
            new Vector3(-30f, 6.5f, 30.3f),
            new[]
            {
                "UNIT ID       BULLDOG-024",
                "TYPE          GROUND / HEAVY",
                "STATUS        ACTIVE",
                "MISSIONS      47",
                "VICTORIES     31",
                "OBJECTIVES    86",
                "SURVIVAL      91%",
                "SERVICE       184 HOURS"
            },
            Cyan,
            root);

        CreateRecordScreen(
            "COMBAT HISTORY",
            new Vector3(-20f, 4.5f, 30.3f),
            new[]
            {
                "OPERATIONS    47",
                "COMBAT        39",
                "SUPPORT       8",
                "CONTACTS      126",
                "CONFIRMED      91",
                "WITHDRAWALS     3"
            },
            Cyan,
            root);
    }

    // ============================================================
    // VETERAN ARCHIVE
    // ============================================================

    private static void BuildVeteranArchive()
    {
        Transform root =
            CreateRoot("VETERAN UNIT ARCHIVE", recordsRoot);

        CreateWallConsole(
            "VETERAN UNIT ARCHIVE",
            new Vector3(-10f, 4f, 31f),
            new Vector3(18f, 8f, 1f),
            Green,
            root);

        CreateVeteranDisplay(
            "VETERAN RANK",
            new Vector3(-15f, 6.5f, 30.3f),
            "BULLDOG-024\nVETERAN II\n184 SERVICE HOURS\n47 OPERATIONS",
            root);

        CreateVeteranDisplay(
            "SERVICE HISTORY",
            new Vector3(-5f, 6.5f, 30.3f),
            "COMBAT EXPERIENCE\n31 VICTORIES\n86 OBJECTIVES\n91% SURVIVAL",
            root);

        CreateTerminal(
            "VETERAN TERMINAL A",
            new Vector3(-14f, 1.4f, 27f),
            Green,
            root);

        CreateTerminal(
            "VETERAN TERMINAL B",
            new Vector3(-9f, 1.4f, 27f),
            Green,
            root);

        CreateTerminal(
            "VETERAN TERMINAL C",
            new Vector3(-4f, 1.4f, 27f),
            Green,
            root);
    }

    // ============================================================
    // DAMAGE / REPAIR
    // ============================================================

    private static void BuildDamageRepairArchive()
    {
        Transform root =
            CreateRoot("DAMAGE AND REPAIR ARCHIVE", recordsRoot);

        CreateWallConsole(
            "DAMAGE / REPAIR HISTORY",
            new Vector3(11f, 4f, 31f),
            new Vector3(19f, 8f, 1f),
            Amber,
            root);

        CreateRecordScreen(
            "DAMAGE HISTORY",
            new Vector3(6f, 6.5f, 30.3f),
            new[]
            {
                "BULLDOG-024",
                "HULL DAMAGE       12%",
                "TRACK DAMAGE       4%",
                "SENSOR DAMAGE      0%",
                "WEAPON DAMAGE      8%",
                "LAST INCIDENT      OP-047"
            },
            Amber,
            root);

        CreateRecordScreen(
            "REPAIR HISTORY",
            new Vector3(16f, 6.5f, 30.3f),
            new[]
            {
                "REPAIRS             9",
                "MAJOR REPAIRS       2",
                "MINOR REPAIRS       7",
                "TOTAL MATERIALS     428",
                "TOTAL FUEL          61",
                "CURRENT CONDITION   96%"
            },
            Green,
            root);

        CreateTerminal(
            "REPAIR RECORD A",
            new Vector3(7f, 1.4f, 27f),
            Amber,
            root);

        CreateTerminal(
            "REPAIR RECORD B",
            new Vector3(12f, 1.4f, 27f),
            Amber,
            root);

        CreateTerminal(
            "REPAIR RECORD C",
            new Vector3(17f, 1.4f, 27f),
            Amber,
            root);
    }

    // ============================================================
    // LOSSES / MEMORIAL
    // ============================================================

    private static void BuildLossesMemorial()
    {
        Transform root =
            CreateRoot("LOSSES AND MEMORIAL", systemsRoot);

        CreateWallConsole(
            "MILITARY LOSSES",
            new Vector3(30f, 4f, 31f),
            new Vector3(17f, 8f, 1f),
            Red,
            root);

        CreateRecordScreen(
            "LOSS RECORD",
            new Vector3(26f, 6.5f, 30.3f),
            new[]
            {
                "TOTAL UNITS LOST     14",
                "GROUND                6",
                "AIR                   5",
                "NAVAL                 2",
                "EXPERIMENTAL          1",
                "RECOVERABLE            3"
            },
            Red,
            root);

        CreateRecordScreen(
            "RECOVERY STATUS",
            new Vector3(35f, 6.5f, 30.3f),
            new[]
            {
                "RECOVERY QUEUE        3",
                "RECOVERED              8",
                "DESTROYED              6",
                "MISSING                0",
                "MEMORIAL RECORD       ACTIVE"
            },
            Amber,
            root);

        CreateMemorial(
            new Vector3(30f, 1.5f, 26.5f),
            root);
    }

    // ============================================================
    // EQUIPMENT HISTORY
    // ============================================================

    private static void BuildEquipmentHistory()
    {
        Transform root =
            CreateRoot("EQUIPMENT HISTORY", systemsRoot);

        CreateWallConsole(
            "EQUIPMENT HISTORY",
            new Vector3(-30f, 4f, 13f),
            new Vector3(17f, 8f, 1f),
            Blue,
            root);

        CreateEquipmentRack(
            new Vector3(-35f, 1.5f, 9f),
            "PRIMARY SYSTEMS",
            root);

        CreateEquipmentRack(
            new Vector3(-29f, 1.5f, 9f),
            "SENSORS",
            root);

        CreateEquipmentRack(
            new Vector3(-23f, 1.5f, 9f),
            "DEFENSE",
            root);

        CreateRecordScreen(
            "EQUIPMENT TIMELINE",
            new Vector3(-30f, 6.5f, 12.3f),
            new[]
            {
                "CURRENT LOADOUT",
                "ARMOR MK-IV",
                "SENSOR ARRAY S-9",
                "COMMS LINK C-7",
                "DEFENSE GRID D-4",
                "UPGRADES LOGGED      18"
            },
            Blue,
            root);
    }

    // ============================================================
    // OPERATIONAL HISTORY
    // ============================================================

    private static void BuildOperationalHistory()
    {
        Transform root =
            CreateRoot("OPERATIONAL HISTORY", systemsRoot);

        CreateWallConsole(
            "OPERATIONAL HISTORY",
            new Vector3(-10f, 4f, 13f),
            new Vector3(18f, 8f, 1f),
            Cyan,
            root);

        CreateRecordScreen(
            "MISSION HISTORY",
            new Vector3(-15f, 6.5f, 12.3f),
            new[]
            {
                "OPERATIONS            47",
                "SUCCESSFUL            39",
                "PARTIAL                6",
                "FAILED                  2",
                "OBJECTIVES            86",
                "TACTICAL ORDERS      214"
            },
            Cyan,
            root);

        CreateRecordScreen(
            "OPERATIONAL EXPERIENCE",
            new Vector3(-5f, 6.5f, 12.3f),
            new[]
            {
                "COMMAND EXPERIENCE",
                "FIELD EXPERIENCE",
                "LOGISTICS EXPERIENCE",
                "RECON EXPERIENCE",
                "AUTONOMOUS EXPERIENCE",
                "LEVEL                27"
            },
            Green,
            root);

        CreateTerminal(
            "MISSION ARCHIVE A",
            new Vector3(-14f, 1.4f, 9f),
            Cyan,
            root);

        CreateTerminal(
            "MISSION ARCHIVE B",
            new Vector3(-9f, 1.4f, 9f),
            Cyan,
            root);

        CreateTerminal(
            "MISSION ARCHIVE C",
            new Vector3(-4f, 1.4f, 9f),
            Cyan,
            root);
    }

    // ============================================================
    // MILITARY DEVELOPMENT
    // ============================================================

    private static void BuildMilitaryDevelopment()
    {
        Transform root =
            CreateRoot("MILITARY DEVELOPMENT CENTER", developmentRoot);

        CreateWallConsole(
            "MILITARY DEVELOPMENT",
            new Vector3(12f, 4f, 13f),
            new Vector3(40f, 8f, 1f),
            Blue,
            root);

        string[] names =
        {
            "FLEET GROWTH",
            "TECHNOLOGY GROWTH",
            "AI DEVELOPMENT",
            "DOCTRINE DEVELOPMENT",
            "OPERATIONAL EXPERIENCE",
            "COMMANDER DEVELOPMENT"
        };

        Color[] colors =
        {
            Cyan,
            Blue,
            Green,
            Amber,
            Cyan,
            Green
        };

        for (int i = 0; i < names.Length; i++)
        {
            float x = 0f + (i % 3) * 13f;
            float z = 8f - (i / 3) * 8f;

            CreateDevelopmentNode(
                names[i],
                new Vector3(x, 2.8f, z),
                colors[i],
                root);
        }

        CreateRecordScreen(
            "FLEET GROWTH",
            new Vector3(3f, 6.5f, 12.3f),
            new[]
            {
                "TOTAL OWNED           126",
                "AIR                    54",
                "GROUND                 48",
                "NAVAL                  16",
                "COMMAND                 8",
                "EXPERIMENTAL            6"
            },
            Cyan,
            root);

        CreateRecordScreen(
            "TECHNOLOGY GROWTH",
            new Vector3(13f, 6.5f, 12.3f),
            new[]
            {
                "TECHNOLOGIES           84",
                "UNLOCKED               71",
                "RESEARCH ACTIVE         3",
                "RESEARCH QUEUED         5",
                "EXPERIMENTAL             9"
            },
            Blue,
            root);

        CreateRecordScreen(
            "AI DEVELOPMENT",
            new Vector3(23f, 6.5f, 12.3f),
            new[]
            {
                "AUTONOMY                82%",
                "DECISION MAKING        76%",
                "COORDINATION           88%",
                "PERSONALITY             64%",
                "TACTICAL LEARNING       79%"
            },
            Green,
            root);

        CreateRecordScreen(
            "DOCTRINE DEVELOPMENT",
            new Vector3(3f, -1f, 4.3f),
            new[]
            {
                "PRIMARY DOCTRINE",
                "AUTONOMOUS MANEUVER",
                "DEFENSIVE PRIORITY",
                "LOGISTICS PRESERVATION",
                "MISSION COMPLETION"
            },
            Amber,
            root);

        CreateRecordScreen(
            "OPERATIONAL EXPERIENCE",
            new Vector3(13f, -1f, 4.3f),
            new[]
            {
                "OPERATIONS             47",
                "COMMAND ORDERS        214",
                "TACTICAL CONTACTS     126",
                "SUCCESS RATE            83%",
                "EXPERIENCE LEVEL         27"
            },
            Cyan,
            root);

        CreateRecordScreen(
            "COMMANDER DEVELOPMENT",
            new Vector3(23f, -1f, 4.3f),
            new[]
            {
                "COMMAND LEVEL           19",
                "OPERATIONS               47",
                "OBJECTIVES COMPLETED    86",
                "FLEET DIRECTED          126",
                "DOCTRINE MASTERED         6"
            },
            Green,
            root);
    }

    // ============================================================
    // COMMAND CENTER
    // ============================================================

    private static void BuildCommandCenter()
    {
        Transform root =
            CreateRoot("PERSISTENT MILITARY COMMAND CENTER", systemsRoot);

        CreateCube(
            "COMMAND PLATFORM",
            new Vector3(0f, 0.35f, -11f),
            new Vector3(34f, 0.7f, 13f),
            new Color(0.025f, 0.038f, 0.048f, 1f),
            root);

        CreateCube(
            "COMMAND PLATFORM EDGE",
            new Vector3(0f, 0.72f, -17f),
            new Vector3(34f, 0.15f, 0.3f),
            Cyan,
            root);

        CreateCommandDesk(
            new Vector3(-10f, 1.2f, -10f),
            "UNIT RECORDS",
            Cyan,
            root);

        CreateCommandDesk(
            new Vector3(0f, 1.2f, -10f),
            "MILITARY DEVELOPMENT",
            Blue,
            root);

        CreateCommandDesk(
            new Vector3(10f, 1.2f, -10f),
            "ARCHIVE CONTROL",
            Green,
            root);

        CreateSign(
            "PERMANENT RECORDS COMMAND",
            new Vector3(0f, 8f, -24.5f),
            1.7f,
            Cyan,
            root);

        CreateSign(
            "ALL UNIT HISTORY IS PERSISTENT",
            new Vector3(0f, 5.8f, -24.4f),
            1.05f,
            Dim,
            root);

        CreateRecordScreen(
            "SYSTEM STATUS",
            new Vector3(0f, 4.2f, -23.8f),
            new[]
            {
                "DATABASE               ONLINE",
                "UNIT RECORDS           126",
                "VETERAN UNITS           34",
                "REPAIR RECORDS         218",
                "OPERATION RECORDS       47",
                "DEVELOPMENT TRACKING   ACTIVE"
            },
            Cyan,
            root);
    }

    // ============================================================
    // CAMERA
    // ============================================================

    private static void BuildCamera()
    {
        GameObject cameraObject =
            new GameObject(
                "PERSISTENT MILITARY CAMERA",
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
            new Vector3(0f, 24f, -69f);

        cameraObject.transform.rotation =
            Quaternion.LookRotation(
                new Vector3(0f, 5f, 10f) -
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
            new Color(0.025f, 0.035f, 0.045f, 1f);

        GameObject key =
            new GameObject(
                "FACILITY KEY LIGHT",
                typeof(Light));

        Light keyLight =
            key.GetComponent<Light>();

        keyLight.type =
            LightType.Directional;

        keyLight.intensity = 0.8f;

        key.transform.rotation =
            Quaternion.Euler(48f, -25f, 0f);

        key.transform.SetParent(lightingRoot);

        CreatePointLight(
            "CYAN COMMAND LIGHT",
            new Vector3(0f, 12f, 31f),
            Cyan,
            7f,
            22f);

        CreatePointLight(
            "GREEN VETERAN LIGHT",
            new Vector3(-10f, 10f, 30f),
            Green,
            5f,
            18f);

        CreatePointLight(
            "AMBER REPAIR LIGHT",
            new Vector3(11f, 10f, 30f),
            Amber,
            5f,
            18f);

        CreatePointLight(
            "BLUE DEVELOPMENT LIGHT",
            new Vector3(20f, 10f, 13f),
            Blue,
            6f,
            22f);

        for (int x = -30; x <= 30; x += 10)
        {
            CreatePointLight(
                "CEILING LIGHT",
                new Vector3(x, 13f, 2f),
                new Color(0.08f, 0.30f, 0.38f, 1f),
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
            new GameObject(name, typeof(Light));

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

        go.transform.SetParent(lightingRoot);
    }

    // ============================================================
    // EVENT SYSTEM
    // ============================================================

    private static void BuildEventSystem()
    {
        GameObject eventSystem =
            new GameObject(
                "EVENT SYSTEM",
                typeof(EventSystem),
                typeof(InputSystemUIInputModule));

        eventSystem.transform.SetParent(facilityRoot);
    }

    // ============================================================
    // HUD
    // ============================================================

    private static void BuildHUD()
    {
        GameObject canvasObject =
            new GameObject(
                "PERSISTENT MILITARY HUD",
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));

        Canvas canvas =
            canvasObject.GetComponent<Canvas>();

        canvas.renderMode =
            RenderMode.ScreenSpaceOverlay;

        canvas.sortingOrder = 100;

        CanvasScaler scaler =
            canvasObject.GetComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(1920f, 1080f);

        scaler.screenMatchMode =
            CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        scaler.matchWidthOrHeight = 0.5f;

        BuildHUDHeader(canvas.transform);
        BuildHUDLeft(canvas.transform);
        BuildHUDCenter(canvas.transform);
        BuildHUDRight(canvas.transform);
        BuildHUDBottom(canvas.transform);
        BuildPersistentRecordOverlay(canvas.transform);
    }

    private static void BuildHUDHeader(
        Transform parent)
    {
        GameObject panel =
            CreateUIPanel(
                "HUD HEADER",
                parent,
                new Vector2(0.5f, 1f),
                new Vector2(0f, -52f),
                new Vector2(1920f, 104f),
                Panel);

        CreateUIText(
            "TITLE",
            panel.transform,
            "16. PERSISTENT MILITARY",
            34,
            White,
            TextAnchor.MiddleLeft,
            new Vector2(32f, 0f),
            new Vector2(620f, 80f));

        CreateUIText(
            "SUBTITLE",
            panel.transform,
            "PERMANENT MILITARY RECORDS COMMAND",
            17,
            Dim,
            TextAnchor.MiddleLeft,
            new Vector2(34f, -31f),
            new Vector2(650f, 34f));

        CreateUIText(
            "STATUS",
            panel.transform,
            "DATABASE ONLINE    |    126 UNITS    |    34 VETERANS",
            20,
            Green,
            TextAnchor.MiddleRight,
            new Vector2(-35f, 0f),
            new Vector2(900f, 50f));
    }

    private static void BuildHUDLeft(
        Transform parent)
    {
        GameObject panel =
            CreateUIPanel(
                "SYSTEM NAVIGATION",
                parent,
                new Vector2(0f, 0.5f),
                new Vector2(188f, 8f),
                new Vector2(350f, 820f),
                Panel);

        CreateUIText(
            "SYSTEMS",
            panel.transform,
            "PERSISTENT SYSTEMS",
            24,
            Cyan,
            TextAnchor.MiddleLeft,
            new Vector2(22f, 360f),
            new Vector2(300f, 50f));

        string[] items =
        {
            "UNIT HISTORY",
            "VETERAN UNITS",
            "DAMAGE HISTORY",
            "REPAIRS",
            "LOSSES",
            "EQUIPMENT HISTORY",
            "OPERATIONAL HISTORY",
            "MILITARY DEVELOPMENT"
        };

        for (int i = 0; i < items.Length; i++)
        {
            CreateUIButton(
                items[i],
                panel.transform,
                new Vector2(0f, 300f - i * 73f),
                new Vector2(315f, 58f),
                i == 0 ? Cyan : Dim);
        }

        CreateUIText(
            "DATABASE",
            panel.transform,
            "DATABASE\n\nUNIT RECORDS     126\nVETERAN UNITS      34\nREPAIR RECORDS    218\nLOSS RECORDS       14\nMISSIONS           47",
            16,
            White,
            TextAnchor.UpperLeft,
            new Vector2(25f, -285f),
            new Vector2(300f, 230f));
    }

    private static void BuildHUDCenter(
        Transform parent)
    {
        GameObject panel =
            CreateUIPanel(
                "PERSISTENT UNIT RECORD",
                parent,
                new Vector2(0.5f, 0.5f),
                new Vector2(-70f, 15f),
                new Vector2(820f, 820f),
                Panel);

        CreateUIText(
            "RECORD HEADER",
            panel.transform,
            "PERSISTENT UNIT RECORD",
            25,
            Cyan,
            TextAnchor.MiddleLeft,
            new Vector2(28f, 350f),
            new Vector2(700f, 50f));

        CreateUIText(
            "UNIT ID",
            panel.transform,
            "BULLDOG-024",
            31,
            White,
            TextAnchor.MiddleLeft,
            new Vector2(28f, 295f),
            new Vector2(500f, 55f));

        CreateUIText(
            "VETERAN",
            panel.transform,
            "VETERAN II    •    ACTIVE SERVICE",
            17,
            Green,
            TextAnchor.MiddleLeft,
            new Vector2(30f, 258f),
            new Vector2(500f, 40f));

        CreateRecordBlock(
            panel.transform,
            "UNIT IDENTITY",
            "TYPE             GROUND / HEAVY\nROLE             ARMORED ASSAULT\nSTATUS           ACTIVE\nCURRENT ASSIGNMENT OPERATIONS",
            190f,
            Cyan);

        CreateRecordBlock(
            panel.transform,
            "UNIT HISTORY",
            "SERVICE HOURS      184\nOPERATIONS           47\nMISSIONS             47\nDEPLOYMENTS          47\nRETURN RATE          91%",
            60f,
            Blue);

        CreateRecordBlock(
            panel.transform,
            "COMBAT HISTORY",
            "VICTORIES            31\nOBJECTIVES COMPLETED  86\nCONTACTS             126\nTACTICAL ORDERS      214\nSUCCESS RATE          83%",
            -75f,
            Green);

        CreateRecordBlock(
            panel.transform,
            "DAMAGE / REPAIR",
            "CURRENT CONDITION    96%\nDAMAGE INCIDENTS       9\nMAJOR REPAIRS          2\nMINOR REPAIRS          7\nLAST REPAIR           OP-047",
            -210f,
            Amber);

        CreateRecordBlock(
            panel.transform,
            "EQUIPMENT HISTORY",
            "ARMOR               MK-IV\nSENSOR ARRAY          S-9\nCOMMS LINK             C-7\nDEFENSE GRID           D-4\nUPGRADES               18",
            -345f,
            Blue);
    }

    private static void BuildHUDRight(
        Transform parent)
    {
        GameObject panel =
            CreateUIPanel(
                "MILITARY DEVELOPMENT HUD",
                parent,
                new Vector2(1f, 0.5f),
                new Vector2(-205f, 15f),
                new Vector2(390f, 820f),
                Panel);

        CreateUIText(
            "DEVELOPMENT",
            panel.transform,
            "MILITARY DEVELOPMENT",
            23,
            Blue,
            TextAnchor.MiddleLeft,
            new Vector2(20f, 355f),
            new Vector2(350f, 50f));

        CreateDevelopmentHUDItem(
            panel.transform,
            "FLEET GROWTH",
            "126 UNITS",
            285f,
            Cyan);

        CreateDevelopmentHUDItem(
            panel.transform,
            "TECHNOLOGY GROWTH",
            "71 / 84",
            215f,
            Blue);

        CreateDevelopmentHUDItem(
            panel.transform,
            "AI DEVELOPMENT",
            "82%",
            145f,
            Green);

        CreateDevelopmentHUDItem(
            panel.transform,
            "DOCTRINE DEVELOPMENT",
            "6 ACTIVE",
            75f,
            Amber);

        CreateDevelopmentHUDItem(
            panel.transform,
            "OPERATIONAL EXPERIENCE",
            "LEVEL 27",
            5f,
            Cyan);

        CreateDevelopmentHUDItem(
            panel.transform,
            "COMMANDER DEVELOPMENT",
            "LEVEL 19",
            -65f,
            Green);

        CreateUIText(
            "PERSISTENCE RULE",
            panel.transform,
            "PERSISTENT PROGRESSION\n\nUNIT EXPERIENCE IS RETAINED.\nDAMAGE IS RETAINED.\nEQUIPMENT HISTORY IS RETAINED.\nMISSION HISTORY IS RETAINED.\n\nCOMPETITIVE POWER REMAINS\nLIMITED BY DEPLOYMENT BUDGET.",
            15,
            White,
            TextAnchor.UpperLeft,
            new Vector2(20f, -125f),
            new Vector2(350f, 250f));
    }

    private static void BuildHUDBottom(
        Transform parent)
    {
        GameObject panel =
            CreateUIPanel(
                "BOTTOM COMMAND BAR",
                parent,
                new Vector2(0.5f, 0f),
                new Vector2(0f, 62f),
                new Vector2(1920f, 124f),
                Panel);

        string[] buttons =
        {
            "GARAGE",
            "OPERATIONS",
            "LOGISTICS",
            "RESEARCH",
            "AFTER ACTION",
            "COMMAND CENTER"
        };

        for (int i = 0; i < buttons.Length; i++)
        {
            CreateUIButton(
                buttons[i],
                panel.transform,
                new Vector2(-710f + i * 245f, 5f),
                new Vector2(215f, 54f),
                Cyan);
        }

        CreateUIText(
            "BATTLE BUDGET",
            panel.transform,
            "BATTLE BUDGET  10,000 DP    |    COMPETITIVE POWER LIMIT ENFORCED",
            18,
            Green,
            TextAnchor.MiddleRight,
            new Vector2(150f, -35f),
            new Vector2(700f, 35f));
    }

    private static void BuildPersistentRecordOverlay(
        Transform parent)
    {
        GameObject overlay =
            CreateUIPanel(
                "RECORD DETAIL WINDOW",
                parent,
                new Vector2(0.5f, 0.5f),
                new Vector2(0f, 0f),
                new Vector2(560f, 340f),
                new Color(0.008f, 0.015f, 0.022f, 0.99f));

        CreateUIText(
            "DETAIL",
            overlay.transform,
            "SELECTED RECORD",
            22,
            Cyan,
            TextAnchor.MiddleLeft,
            new Vector2(22f, 130f),
            new Vector2(500f, 42f));

        CreateUIText(
            "DETAIL TEXT",
            overlay.transform,
            "BULLDOG-024\n\nRecord loaded from Persistent Military Database.\n\nAll historical data is retained across operations.\n\nCURRENT ASSIGNMENT: OPERATIONS\nSTATUS: ACTIVE\nCONDITION: 96%",
            17,
            White,
            TextAnchor.UpperLeft,
            new Vector2(24f, 95f),
            new Vector2(500f, 190f));

        CreateUIButton(
            "VIEW FULL RECORD",
            overlay.transform,
            new Vector2(-110f, -125f),
            new Vector2(210f, 48f),
            Cyan);

        CreateUIButton(
            "CLOSE",
            overlay.transform,
            new Vector2(120f, -125f),
            new Vector2(150f, 48f),
            Dim);
    }

    // ============================================================
    // PHYSICAL VISUAL HELPERS
    // ============================================================

    private static void CreateWallConsole(
        string name,
        Vector3 position,
        Vector3 scale,
        Color accent,
        Transform parent)
    {
        CreateCube(
            name + " BODY",
            position,
            scale,
            Panel2,
            parent);

        CreateCube(
            name + " ACCENT",
            position + new Vector3(0f, -scale.y * 0.48f, -0.55f),
            new Vector3(scale.x * 0.92f, 0.12f, 0.08f),
            accent,
            parent);
    }

    private static void CreateRecordScreen(
        string title,
        Vector3 position,
        string[] lines,
        Color accent,
        Transform parent)
    {
        CreateCube(
            title + " SCREEN",
            position,
            new Vector3(8.2f, 3.8f, 0.18f),
            new Color(0.008f, 0.018f, 0.026f, 1f),
            parent);

        CreateCube(
            title + " SCREEN FRAME",
            position + new Vector3(0f, 0f, 0.12f),
            new Vector3(8.5f, 4.1f, 0.08f),
            new Color(
                accent.r * 0.35f,
                accent.g * 0.35f,
                accent.b * 0.35f,
                1f),
            parent);

        CreateSign(
            title,
            position + new Vector3(0f, 1.35f, -0.16f),
            0.72f,
            accent,
            parent);

        string text = "";

        foreach (string line in lines)
            text += line + "\n";

        CreateSign(
            text,
            position + new Vector3(0f, -0.15f, -0.18f),
            0.46f,
            White,
            parent);
    }

    private static void CreateVeteranDisplay(
        string title,
        Vector3 position,
        string text,
        Transform parent)
    {
        CreateCube(
            title + " DISPLAY",
            position,
            new Vector3(8f, 3.7f, 0.2f),
            new Color(0.012f, 0.025f, 0.018f, 1f),
            parent);

        CreateSign(
            title,
            position + new Vector3(0f, 1.15f, -0.15f),
            0.7f,
            Green,
            parent);

        CreateSign(
            text,
            position + new Vector3(0f, -0.25f, -0.16f),
            0.55f,
            White,
            parent);
    }

    private static void CreateTerminal(
        string name,
        Vector3 position,
        Color accent,
        Transform parent)
    {
        CreateCube(
            name + " BASE",
            position,
            new Vector3(2.8f, 1.8f, 2.2f),
            new Color(0.025f, 0.035f, 0.045f, 1f),
            parent);

        CreateCube(
            name + " SCREEN",
            position + new Vector3(0f, 1.15f, 0.25f),
            new Vector3(2.3f, 1.3f, 0.16f),
            new Color(0.008f, 0.020f, 0.028f, 1f),
            parent);

        CreateCube(
            name + " LIGHT",
            position + new Vector3(0f, 0.3f, -1.15f),
            new Vector3(1.8f, 0.12f, 0.08f),
            accent,
            parent);
    }

    private static void CreateMemorial(
        Vector3 position,
        Transform parent)
    {
        CreateCube(
            "MEMORIAL BASE",
            position,
            new Vector3(12f, 0.6f, 5f),
            new Color(0.02f, 0.025f, 0.03f, 1f),
            parent);

        CreateCube(
            "MEMORIAL MONUMENT",
            position + new Vector3(0f, 2.3f, 0f),
            new Vector3(3f, 4f, 1.2f),
            new Color(0.035f, 0.04f, 0.045f, 1f),
            parent);

        CreateSign(
            "REMEMBER THE LOST",
            position + new Vector3(0f, 4.5f, -0.7f),
            0.65f,
            Red,
            parent);
    }

    private static void CreateEquipmentRack(
        Vector3 position,
        string label,
        Transform parent)
    {
        CreateCube(
            label + " RACK",
            position,
            new Vector3(4.5f, 3.4f, 1.4f),
            new Color(0.028f, 0.038f, 0.048f, 1f),
            parent);

        CreateCube(
            label + " RACK LIGHT",
            position + new Vector3(0f, 1.4f, -0.75f),
            new Vector3(3.8f, 0.10f, 0.08f),
            Blue,
            parent);

        CreateSign(
            label,
            position + new Vector3(0f, 0f, -0.78f),
            0.48f,
            White,
            parent);
    }

    private static void CreateDevelopmentNode(
        string name,
        Vector3 position,
        Color accent,
        Transform parent)
    {
        CreateCube(
            name + " NODE",
            position,
            new Vector3(9.5f, 4.5f, 3.4f),
            new Color(0.018f, 0.030f, 0.042f, 1f),
            parent);

        CreateCube(
            name + " NODE ACCENT",
            position + new Vector3(0f, -2.05f, -1.75f),
            new Vector3(7.8f, 0.16f, 0.08f),
            accent,
            parent);

        CreateSign(
            name,
            position + new Vector3(0f, 0.9f, -1.78f),
            0.58f,
            accent,
            parent);

        CreateSign(
            "PERSISTENT PROGRESS",
            position + new Vector3(0f, -0.3f, -1.78f),
            0.42f,
            Dim,
            parent);
    }

    private static void CreateCommandDesk(
        Vector3 position,
        string label,
        Color accent,
        Transform parent)
    {
        CreateCube(
            label + " DESK",
            position,
            new Vector3(7f, 1.2f, 3.5f),
            new Color(0.025f, 0.035f, 0.045f, 1f),
            parent);

        CreateCube(
            label + " DISPLAY",
            position + new Vector3(0f, 1.6f, 0.8f),
            new Vector3(5.5f, 2.6f, 0.18f),
            new Color(0.008f, 0.018f, 0.026f, 1f),
            parent);

        CreateSign(
            label,
            position + new Vector3(0f, 1.6f, 0.55f),
            0.55f,
            accent,
            parent);
    }

    private static void CreateSign(
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

        TextMesh mesh =
            go.AddComponent<TextMesh>();

        mesh.text = text;
        mesh.fontSize = Mathf.RoundToInt(40f * size);
        mesh.characterSize = 0.025f * size;
        mesh.anchor = TextAnchor.MiddleCenter;
        mesh.alignment = TextAlignment.Center;
        mesh.color = color;

        go.transform.rotation =
            Quaternion.Euler(0f, 180f, 0f);
    }

    private static GameObject CreateCube(
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

        Material material =
            new Material(
                Shader.Find("Universal Render Pipeline/Lit"));

        if (material.shader == null)
        {
            material =
                new Material(
                    Shader.Find("Standard"));
        }

        material.color = color;
        renderer.sharedMaterial = material;

        return go;
    }

    // ============================================================
    // UI HELPERS
    // ============================================================

    private static GameObject CreateUIPanel(
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
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        Image image =
            go.GetComponent<Image>();

        image.color = color;

        return go;
    }

    private static Text CreateUIText(
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

        go.transform.SetParent(parent, false);

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

        Text uiText =
            go.GetComponent<Text>();

        uiText.text =
            text;

        uiText.font =
            Resources.GetBuiltinResource<Font>(
                "LegacyRuntime.ttf");

        uiText.fontSize =
            fontSize;

        uiText.color =
            color;

        uiText.alignment =
            alignment;

        uiText.horizontalOverflow =
            HorizontalWrapMode.Wrap;

        uiText.verticalOverflow =
            VerticalWrapMode.Overflow;

        return uiText;
    }

    private static Button CreateUIButton(
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

        go.transform.SetParent(parent, false);

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
                accent.r * 0.28f,
                accent.g * 0.28f,
                accent.b * 0.28f,
                1f);

        colors.pressedColor =
            new Color(
                accent.r * 0.45f,
                accent.g * 0.45f,
                accent.b * 0.45f,
                1f);

        colors.selectedColor =
            colors.highlightedColor;

        button.colors =
            colors;

        CreateUIText(
            "LABEL",
            go.transform,
            label,
            16,
            White,
            TextAnchor.MiddleCenter,
            Vector2.zero,
            size - new Vector2(12f, 8f));

        return button;
    }

    private static void CreateRecordBlock(
        Transform parent,
        string title,
        string text,
        float y,
        Color accent)
    {
        GameObject block =
            CreateUIPanel(
                title,
                parent,
                new Vector2(0.5f, 0.5f),
                new Vector2(0f, y),
                new Vector2(750f, 112f),
                new Color(0.018f, 0.028f, 0.038f, 0.95f));

        CreateUIText(
            "TITLE",
            block.transform,
            title,
            17,
            accent,
            TextAnchor.MiddleLeft,
            new Vector2(-335f, 31f),
            new Vector2(680f, 30f));

        CreateUIText(
            "DATA",
            block.transform,
            text,
            14,
            White,
            TextAnchor.UpperLeft,
            new Vector2(-330f, -8f),
            new Vector2(670f, 65f));
    }

    private static void CreateDevelopmentHUDItem(
        Transform parent,
        string title,
        string value,
        float y,
        Color accent)
    {
        GameObject item =
            CreateUIPanel(
                title,
                parent,
                new Vector2(0.5f, 0.5f),
                new Vector2(0f, y),
                new Vector2(350f, 58f),
                new Color(0.018f, 0.030f, 0.040f, 0.95f));

        CreateUIText(
            "TITLE",
            item.transform,
            title,
            14,
            Dim,
            TextAnchor.MiddleLeft,
            new Vector2(-125f, 0f),
            new Vector2(235f, 50f));

        CreateUIText(
            "VALUE",
            item.transform,
            value,
            17,
            accent,
            TextAnchor.MiddleRight,
            new Vector2(105f, 0f),
            new Vector2(105f, 50f));
    }
}
