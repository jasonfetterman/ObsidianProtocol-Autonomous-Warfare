using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class DeploymentVisualBuilder
{
    private const string ScenePath =
        "Assets/Scenes/SCN-12 DEPLOYMENT/Deployment.unity";

    private static readonly Color Black =
        new Color(0.006f, 0.009f, 0.013f, 1f);

    private static readonly Color Floor =
        new Color(0.018f, 0.026f, 0.036f, 1f);

    private static readonly Color Wall =
        new Color(0.028f, 0.040f, 0.054f, 1f);

    private static readonly Color Panel =
        new Color(0.012f, 0.022f, 0.032f, 0.97f);

    private static readonly Color Panel2 =
        new Color(0.020f, 0.034f, 0.047f, 0.98f);

    private static readonly Color Cyan =
        new Color(0.10f, 0.78f, 0.95f, 1f);

    private static readonly Color Green =
        new Color(0.20f, 0.90f, 0.52f, 1f);

    private static readonly Color Amber =
        new Color(0.95f, 0.66f, 0.16f, 1f);

    private static readonly Color Red =
        new Color(0.95f, 0.18f, 0.20f, 1f);

    private static readonly Color White =
        new Color(0.86f, 0.92f, 0.96f, 1f);

    private static readonly Color Muted =
        new Color(0.42f, 0.52f, 0.60f, 1f);

    private static Transform WorldRoot;
    private static Transform FacilityRoot;
    private static Transform SystemsRoot;
    private static Transform HudRoot;

    private static Font BuiltinFont;

    private static GameObject MapOverlay;
    private static GameObject FleetWindow;
    private static GameObject InspectionWindow;
    private static GameObject ConfirmationWindow;
    private static GameObject FinalReviewWindow;

    private static Text BudgetText;
    private static Text RemainingText;
    private static Text UnitCostText;
    private static Text ForceText;
    private static Text StatusText;

    private static int deploymentBudget = 10000;
    private static int remainingBudget = 10000;
    private static int selectedUnitCost = 0;

    private static readonly List<string> SelectedUnits =
        new List<string>();

    private static readonly List<int> SelectedCosts =
        new List<int>();

    [MenuItem("Obsidian Protocol/Build/DEPLOYMENT - FULL VISUAL")]
    public static void Build()
    {
        Debug.Log("====================================================");
        Debug.Log("OBSIDIAN PROTOCOL - DEPLOYMENT BUILD");
        Debug.Log("Target: " + ScenePath);
        Debug.Log("====================================================");

        if (!System.IO.File.Exists(
                System.IO.Path.Combine(
                    Application.dataPath,
                    "../" + ScenePath)))
        {
            throw new Exception(
                "DEPLOYMENT BUILD ABORTED. Exact target scene does not exist: "
                + ScenePath);
        }

        Scene scene = EditorSceneManager.OpenScene(
            ScenePath,
            OpenSceneMode.Single);

        ClearScene(scene);

        CreateWorld();

        CreateFacility();

        CreatePhysicalDeploymentStations();

        CreatePhysicalTacticalMap();

        CreatePhysicalCommandDeck();

        CreateLighting();

        CreateCamera();

        CreateEventSystem();

        CreateHUD();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("DEPLOYMENT BUILD COMPLETE.");
        Debug.Log("Scene saved: " + ScenePath);
    }

    private static void ClearScene(Scene scene)
    {
        GameObject[] objects = scene.GetRootGameObjects();

        foreach (GameObject obj in objects)
        {
            UnityEngine.Object.DestroyImmediate(obj);
        }
    }

    private static void CreateWorld()
    {
        GameObject world =
            new GameObject("09. DEPLOYMENT");

        WorldRoot = world.transform;

        GameObject facility =
            new GameObject("[FACILITY] DEPLOYMENT COMMAND CENTER");

        FacilityRoot = facility.transform;
        FacilityRoot.SetParent(WorldRoot);

        GameObject systems =
            new GameObject("[SYSTEM] DEPLOYMENT SYSTEMS");

        SystemsRoot = systems.transform;
        SystemsRoot.SetParent(WorldRoot);

        CreateEmpty(
            "[SYSTEM] OPERATION SELECTION",
            SystemsRoot);

        CreateEmpty(
            "[SYSTEM] FORCE COMPOSITION",
            SystemsRoot);

        CreateEmpty(
            "[SYSTEM] DEPLOYMENT BUDGET",
            SystemsRoot);

        CreateEmpty(
            "[SYSTEM] FORMATION",
            SystemsRoot);

        CreateEmpty(
            "[SYSTEM] COMMAND STRUCTURE",
            SystemsRoot);

        CreateEmpty(
            "[SYSTEM] COMMAND INTENT",
            SystemsRoot);

        CreateEmpty(
            "[SYSTEM] DOCTRINE",
            SystemsRoot);

        CreateEmpty(
            "[SYSTEM] RULES OF ENGAGEMENT",
            SystemsRoot);

        CreateEmpty(
            "[SYSTEM] FINAL DEPLOYMENT",
            SystemsRoot);
    }

    private static void CreateFacility()
    {
        float width = 84f;
        float depth = 64f;

        CreateCube(
            "[FACILITY] FLOOR",
            new Vector3(0f, -0.5f, 5f),
            new Vector3(width, 1f, depth),
            Floor,
            FacilityRoot);

        CreateCube(
            "[FACILITY] NORTH WALL",
            new Vector3(0f, 7f, 37f),
            new Vector3(width, 15f, 1f),
            Wall,
            FacilityRoot);

        CreateCube(
            "[FACILITY] SOUTH WALL",
            new Vector3(0f, 7f, -27f),
            new Vector3(width, 15f, 1f),
            Wall,
            FacilityRoot);

        CreateCube(
            "[FACILITY] EAST WALL",
            new Vector3(42f, 7f, 5f),
            new Vector3(1f, 15f, depth),
            Wall,
            FacilityRoot);

        CreateCube(
            "[FACILITY] WEST WALL",
            new Vector3(-42f, 7f, 5f),
            new Vector3(1f, 15f, depth),
            Wall,
            FacilityRoot);

        CreateCube(
            "[FACILITY] CEILING",
            new Vector3(0f, 14.5f, 5f),
            new Vector3(width, 1f, depth),
            new Color(0.010f, 0.015f, 0.021f, 1f),
            FacilityRoot);

        for (int x = -35; x <= 35; x += 10)
        {
            CreateCube(
                "STRUCTURAL COLUMN",
                new Vector3(x, 7f, 34f),
                new Vector3(0.8f, 14f, 0.8f),
                new Color(0.055f, 0.075f, 0.09f, 1f),
                FacilityRoot);

            CreateCube(
                "STRUCTURAL COLUMN",
                new Vector3(x, 7f, -24f),
                new Vector3(0.8f, 14f, 0.8f),
                new Color(0.055f, 0.075f, 0.09f, 1f),
                FacilityRoot);
        }

        CreateSign(
            "DEPLOYMENT COMMAND",
            new Vector3(0f, 11f, 36.2f),
            6f,
            Cyan);

        CreateSign(
            "FORCE ASSEMBLY",
            new Vector3(-27f, 8f, 18f),
            3f,
            Green);

        CreateSign(
            "COMMAND INTENT",
            new Vector3(0f, 8f, 18f),
            3f,
            Amber);

        CreateSign(
            "FINAL DEPLOYMENT",
            new Vector3(27f, 8f, 18f),
            3f,
            Red);

        CreateGridFloor();
    }

    private static void CreateGridFloor()
    {
        Transform grid =
            CreateEmpty(
                "[VISUAL] DEPLOYMENT GRID",
                FacilityRoot);

        for (int x = -40; x <= 40; x += 4)
        {
            CreateCube(
                "GRID LINE",
                new Vector3(x, 0.02f, 5f),
                new Vector3(0.025f, 0.02f, 58f),
                new Color(0.03f, 0.11f, 0.14f, 1f),
                grid);
        }

        for (int z = -23; z <= 33; z += 4)
        {
            CreateCube(
                "GRID LINE",
                new Vector3(0f, 0.021f, z),
                new Vector3(78f, 0.02f, 0.025f),
                new Color(0.03f, 0.11f, 0.14f, 1f),
                grid);
        }
    }

    private static void CreatePhysicalDeploymentStations()
    {
        CreateStation(
            "OPERATION SELECTION TERMINAL",
            new Vector3(-27f, 0f, 20f),
            new Vector3(14f, 4f, 6f),
            Cyan);

        CreateStation(
            "FORCE COMPOSITION TERMINAL",
            new Vector3(-9f, 0f, 20f),
            new Vector3(14f, 4f, 6f),
            Green);

        CreateStation(
            "DEPLOYMENT BUDGET TERMINAL",
            new Vector3(9f, 0f, 20f),
            new Vector3(14f, 4f, 6f),
            Amber);

        CreateStation(
            "COMMAND INTENT TERMINAL",
            new Vector3(27f, 0f, 20f),
            new Vector3(14f, 4f, 6f),
            Cyan);

        CreateStation(
            "FORMATION CONTROL",
            new Vector3(-27f, 0f, 7f),
            new Vector3(14f, 4f, 6f),
            Green);

        CreateStation(
            "COMMAND STRUCTURE",
            new Vector3(-9f, 0f, 7f),
            new Vector3(14f, 4f, 6f),
            Cyan);

        CreateStation(
            "DOCTRINE CONTROL",
            new Vector3(9f, 0f, 7f),
            new Vector3(14f, 4f, 6f),
            Amber);

        CreateStation(
            "RULES OF ENGAGEMENT",
            new Vector3(27f, 0f, 7f),
            new Vector3(14f, 4f, 6f),
            Red);
    }

    private static void CreateStation(
        string title,
        Vector3 position,
        Vector3 size,
        Color accent)
    {
        Transform station =
            CreateEmpty(
                "[STATION] " + title,
                FacilityRoot);

        CreateCube(
            "[STRUCTURE] CONSOLE",
            position + new Vector3(0f, 1.2f, 0f),
            new Vector3(size.x, 2.4f, size.z),
            Panel2,
            station);

        CreateCube(
            "[DISPLAY] HOLOGRAPHIC SCREEN",
            position + new Vector3(0f, 3.1f, -0.9f),
            new Vector3(size.x - 1f, 2.7f, 0.25f),
            new Color(
                accent.r * 0.08f,
                accent.g * 0.08f,
                accent.b * 0.08f,
                1f),
            station);

        CreateCube(
            "[LIGHT] STATUS STRIP",
            position + new Vector3(0f, 0.15f, 0f),
            new Vector3(size.x - 1f, 0.08f, size.z - 0.5f),
            new Color(
                accent.r * 0.25f,
                accent.g * 0.25f,
                accent.b * 0.25f,
                1f),
            station);

        CreateSign(
            title,
            position + new Vector3(0f, 4.8f, 0f),
            1.5f,
            accent);
    }

    private static void CreatePhysicalTacticalMap()
    {
        Transform map =
            CreateEmpty(
                "[TACTICAL MAP] STRATEGIC DEPLOYMENT TABLE",
                FacilityRoot);

        CreateCube(
            "[MAP] TABLE",
            new Vector3(0f, 2f, -10f),
            new Vector3(44f, 2f, 15f),
            Panel2,
            map);

        CreateCube(
            "[MAP] DISPLAY SURFACE",
            new Vector3(0f, 3.1f, -10f),
            new Vector3(40f, 0.15f, 12f),
            new Color(0.008f, 0.035f, 0.048f, 1f),
            map);

        for (int x = -18; x <= 18; x += 6)
        {
            CreateCube(
                "TACTICAL GRID",
                new Vector3(x, 3.2f, -10f),
                new Vector3(0.035f, 0.03f, 11f),
                new Color(0.04f, 0.25f, 0.30f, 1f),
                map);
        }

        for (int z = -15; z <= -5; z += 2)
        {
            CreateCube(
                "TACTICAL GRID",
                new Vector3(0f, 3.21f, z),
                new Vector3(39f, 0.03f, 0.035f),
                new Color(0.04f, 0.25f, 0.30f, 1f),
                map);
        }

        Vector3[] markers =
        {
            new Vector3(-13f,3.35f,-12f),
            new Vector3(-4f,3.35f,-7f),
            new Vector3(6f,3.35f,-13f),
            new Vector3(15f,3.35f,-8f),
            new Vector3(3f,3.35f,-10f)
        };

        foreach (Vector3 marker in markers)
        {
            CreateSphere(
                "CONTACT MARKER",
                marker,
                0.35f,
                Cyan,
                map);
        }

        CreateSign(
            "STRATEGIC DEPLOYMENT MAP",
            new Vector3(0f, 5.4f, -10f),
            2.2f,
            Cyan);
    }

    private static void CreatePhysicalCommandDeck()
    {
        Transform deck =
            CreateEmpty(
                "[COMMAND DECK] FINAL AUTHORIZATION",
                FacilityRoot);

        CreateCube(
            "COMMAND DECK",
            new Vector3(0f, 0.5f, -22f),
            new Vector3(30f, 1f, 8f),
            Panel2,
            deck);

        CreateCube(
            "AUTHORIZATION CONSOLE",
            new Vector3(0f, 2.2f, -22f),
            new Vector3(18f, 3f, 3f),
            Panel,
            deck);

        CreateCube(
            "AUTHORIZATION DISPLAY",
            new Vector3(0f, 4.3f, -23.2f),
            new Vector3(14f, 4f, 0.3f),
            new Color(0.025f, 0.06f, 0.07f, 1f),
            deck);

        CreateSign(
            "FINAL DEPLOYMENT AUTHORIZATION",
            new Vector3(0f, 7f, -22f),
            2.2f,
            Red);
    }

    private static void CreateLighting()
    {
        GameObject lightObject =
            new GameObject(
                "[LIGHTING] DEPLOYMENT KEY LIGHT",
                typeof(Light));

        Light light =
            lightObject.GetComponent<Light>();

        light.type = LightType.Directional;
        light.intensity = 0.75f;
        light.color =
            new Color(0.55f, 0.72f, 0.82f);

        lightObject.transform.rotation =
            Quaternion.Euler(42f, -28f, 0f);

        lightObject.transform.SetParent(WorldRoot);

        Vector3[] positions =
        {
            new Vector3(-30f,10f,20f),
            new Vector3(-10f,10f,20f),
            new Vector3(10f,10f,20f),
            new Vector3(30f,10f,20f),
            new Vector3(0f,10f,-12f),
            new Vector3(0f,8f,-22f)
        };

        foreach (Vector3 position in positions)
        {
            GameObject p =
                new GameObject(
                    "[LIGHTING] FACILITY NODE",
                    typeof(Light));

            Light point =
                p.GetComponent<Light>();

            point.type = LightType.Point;
            point.range = 18f;
            point.intensity = 35f;
            point.color = Cyan;

            p.transform.position = position;
            p.transform.SetParent(WorldRoot);
        }
    }

    private static void CreateCamera()
    {
        GameObject cameraObject =
            new GameObject(
                "DEPLOYMENT CAMERA",
                typeof(Camera),
                typeof(AudioListener));

        Camera camera =
            cameraObject.GetComponent<Camera>();

        camera.clearFlags =
            CameraClearFlags.SolidColor;

        camera.backgroundColor = Black;

        camera.fieldOfView = 68f;
        camera.nearClipPlane = 0.1f;
        camera.farClipPlane = 1000f;

        cameraObject.transform.position =
            new Vector3(0f, 27f, -70f);

        cameraObject.transform.LookAt(
            new Vector3(0f, 5f, 5f));

        camera.tag = "MainCamera";

        cameraObject.transform.SetParent(WorldRoot);
    }

    private static void CreateEventSystem()
    {
        GameObject existing =
            GameObject.Find("EVENT SYSTEM");

        if (existing != null)
            UnityEngine.Object.DestroyImmediate(existing);

        new GameObject(
            "EVENT SYSTEM",
            typeof(EventSystem),
            typeof(InputSystemUIInputModule));
    }

    private static void CreateHUD()
    {
        GameObject canvasObject =
            new GameObject(
                "[HUD] DEPLOYMENT HUD",
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));

        HudRoot = canvasObject.transform;

        Canvas canvas =
            canvasObject.GetComponent<Canvas>();

        canvas.renderMode =
            RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler =
            canvasObject.GetComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(1920f, 1080f);

        scaler.screenMatchMode =
            CanvasScaler.ScreenMatchMode.Expand;

        CreateHeader(canvasObject.transform);

        CreateDeploymentProcess(canvasObject.transform);

        CreateForceList(canvasObject.transform);

        CreateBottomNavigation(canvasObject.transform);

        CreateMapOverlay(canvasObject.transform);

        CreateFleetWindow(canvasObject.transform);

        CreateInspectionWindow(canvasObject.transform);

        CreateFinalReviewWindow(canvasObject.transform);

        CreateConfirmationWindow(canvasObject.transform);
    }

    private static void CreateHeader(Transform parent)
    {
        GameObject panel =
            CreateUIPanel(
                "[PANEL] TOP BAR",
                parent,
                new Vector2(0.02f,0.91f),
                new Vector2(0.98f,0.985f),
                Panel);

        CreateUIText(
            "[DISPLAY] Operation",
            panel.transform,
            "OPERATION // BLACK HORIZON",
            new Vector2(0.025f,0.35f),
            new Vector2(0.25f,0.85f),
            23,
            Cyan);

        CreateUIText(
            "[DISPLAY] Map",
            panel.transform,
            "MAP // THEATER 07",
            new Vector2(0.30f,0.35f),
            new Vector2(0.48f,0.85f),
            20,
            White);

        CreateUIText(
            "[DISPLAY] Player",
            panel.transform,
            "COMMANDER // OPERATOR",
            new Vector2(0.51f,0.35f),
            new Vector2(0.72f,0.85f),
            20,
            White);

        BudgetText =
            CreateUIText(
                "[DISPLAY] Deployment Limit",
                panel.transform,
                "DEPLOYMENT LIMIT // 10,000 DP",
                new Vector2(0.74f,0.35f),
                new Vector2(0.98f,0.85f),
                21,
                Green);
    }

    private static void CreateDeploymentProcess(Transform parent)
    {
        GameObject root =
            CreateUIPanel(
                "[PANEL] DEPLOYMENT PROCESS",
                parent,
                new Vector2(0.02f,0.20f),
                new Vector2(0.69f,0.89f),
                new Color(0.006f,0.012f,0.018f,0.94f));

        CreateSection(
            "[WINDOW] SELECT OPERATION",
            root.transform,
            0.83f,
            "AVAILABLE OPERATIONS\nBLACK HORIZON\nIRON VEIL\nNIGHT VECTOR\nSILENT CURRENT",
            "SELECT OPERATION",
            () => SetStatus("OPERATION SELECTED // BLACK HORIZON"));

        CreateSection(
            "[WINDOW] SELECT FORCE",
            root.transform,
            0.68f,
            "AVAILABLE UNITS\nWARDEN // BEACON // BULLDOG\nFORGE // SENTINEL // SCOUT",
            "OPEN FLEET",
            () => ShowWindow(FleetWindow));

        GameObject budget =
            CreateUIPanel(
                "[WINDOW] DEPLOYMENT BUDGET",
                root.transform,
                new Vector2(0.51f,0.64f),
                new Vector2(0.98f,0.82f),
                Panel2);

        CreateUIText(
            "[DISPLAY] Available Deployment",
            budget.transform,
            "AVAILABLE DEPLOYMENT",
            new Vector2(0.04f,0.62f),
            new Vector2(0.52f,0.90f),
            16,
            Muted);

        BudgetText =
            CreateUIText(
                "[STAT] 10,000 / 10,000",
                budget.transform,
                "10,000 / 10,000",
                new Vector2(0.52f,0.62f),
                new Vector2(0.96f,0.90f),
                24,
                Green);

        CreateUIText(
            "[DISPLAY] Unit Cost",
            budget.transform,
            "UNIT COST",
            new Vector2(0.04f,0.30f),
            new Vector2(0.30f,0.56f),
            16,
            Muted);

        UnitCostText =
            CreateUIText(
                "UNIT COST VALUE",
                budget.transform,
                "0 DP",
                new Vector2(0.30f,0.30f),
                new Vector2(0.50f,0.56f),
                19,
                White);

        CreateUIText(
            "[DISPLAY] Remaining Budget",
            budget.transform,
            "REMAINING",
            new Vector2(0.52f,0.30f),
            new Vector2(0.75f,0.56f),
            16,
            Muted);

        RemainingText =
            CreateUIText(
                "REMAINING BUDGET",
                budget.transform,
                "10,000 DP",
                new Vector2(0.75f,0.30f),
                new Vector2(0.96f,0.56f),
                19,
                Green);

        CreateSection(
            "[WINDOW] FORMATION",
            root.transform,
            0.49f,
            "FORMATION\nLINE / COLUMN / SCREEN\nCURRENT: AUTONOMOUS SCREEN",
            "CONFIGURE FORMATION",
            () => SetStatus("FORMATION CONFIGURATION OPEN"));

        CreateSection(
            "[WINDOW] COMMAND STRUCTURE",
            root.transform,
            0.34f,
            "COMMAND STRUCTURE\nPRIMARY COMMANDER\nSECONDARY // AI COORDINATION",
            "CONFIGURE COMMAND",
            () => SetStatus("COMMAND STRUCTURE CONFIGURATION OPEN"));

        CreateSection(
            "[WINDOW] COMMAND INTENT",
            root.transform,
            0.19f,
            "OBJECTIVE // SECURE AND HOLD\nPRIORITY // MISSION\nAUTONOMY // HIGH",
            "CONFIGURE INTENT",
            () => SetStatus("COMMAND INTENT CONFIGURATION OPEN"));

        CreateSection(
            "[WINDOW] RULES OF ENGAGEMENT",
            root.transform,
            0.04f,
            "ROE // RETURN FIRE\nCOLLATERAL LIMIT // RESTRICTED",
            "CONFIGURE ROE",
            () => SetStatus("RULES OF ENGAGEMENT CONFIGURATION OPEN"));
    }

    private static void CreateSection(
        string title,
        Transform parent,
        float y,
        string body,
        string buttonText,
        Action action)
    {
        float height = 0.135f;

        GameObject panel =
            CreateUIPanel(
                title,
                parent,
                new Vector2(0.02f,y),
                new Vector2(0.98f,y + height),
                Panel2);

        CreateUIText(
            title,
            panel.transform,
            title,
            new Vector2(0.025f,0.66f),
            new Vector2(0.54f,0.95f),
            15,
            Cyan);

        CreateUIText(
            "[DISPLAY]",
            panel.transform,
            body,
            new Vector2(0.025f,0.08f),
            new Vector2(0.70f,0.64f),
            12,
            White);

        CreateUIButton(
            "[BUTTON] " + buttonText,
            panel.transform,
            buttonText,
            new Vector2(0.72f,0.18f),
            new Vector2(0.97f,0.78f),
            12,
            action);
    }

    private static void CreateForceList(Transform parent)
    {
        GameObject panel =
            CreateUIPanel(
                "[PANEL] FORCE LIST",
                parent,
                new Vector2(0.72f,0.20f),
                new Vector2(0.98f,0.89f),
                Panel);

        CreateUIText(
            "[LIST] SELECTED FORCE",
            panel.transform,
            "SELECTED FORCE",
            new Vector2(0.05f,0.91f),
            new Vector2(0.95f,0.98f),
            20,
            Green);

        ForceText =
            CreateUIText(
                "[LIST] SELECTED FORCE",
                panel.transform,
                "NO UNITS SELECTED\n\nFORCE STRENGTH // 0\nUNIT COUNT // 0",
                new Vector2(0.05f,0.45f),
                new Vector2(0.95f,0.88f),
                17,
                White);

        CreateUIButton(
            "[BUTTON] ADD UNIT",
            panel.transform,
            "ADD UNIT",
            new Vector2(0.05f,0.36f),
            new Vector2(0.46f,0.43f),
            13,
            () => ShowWindow(FleetWindow));

        CreateUIButton(
            "[BUTTON] REMOVE UNIT",
            panel.transform,
            "REMOVE",
            new Vector2(0.54f,0.36f),
            new Vector2(0.95f,0.43f),
            13,
            RemoveLastUnit);

        CreateUIButton(
            "[BUTTON] UNIT DETAILS",
            panel.transform,
            "UNIT DETAILS",
            new Vector2(0.05f,0.27f),
            new Vector2(0.95f,0.34f),
            13,
            () => ShowWindow(InspectionWindow));

        StatusText =
            CreateUIText(
                "[STATUS]",
                panel.transform,
                "STATUS // DEPLOYMENT READY",
                new Vector2(0.05f,0.05f),
                new Vector2(0.95f,0.23f),
                13,
                Cyan);
    }

    private static void CreateBottomNavigation(Transform parent)
    {
        GameObject panel =
            CreateUIPanel(
                "[PANEL] BOTTOM NAVIGATION",
                parent,
                new Vector2(0.02f,0.04f),
                new Vector2(0.98f,0.16f),
                new Color(0.006f,0.012f,0.018f,0.98f));

        CreateUIButton(
            "[BUTTON] BACK",
            panel.transform,
            "BACK",
            new Vector2(0.02f,0.22f),
            new Vector2(0.16f,0.78f),
            15,
            () => SetStatus("BACK // COMMAND CENTER"));

        CreateUIButton(
            "[BUTTON] SAVE LOADOUT",
            panel.transform,
            "SAVE LOADOUT",
            new Vector2(0.18f,0.22f),
            new Vector2(0.36f,0.78f),
            15,
            () => SetStatus("LOADOUT SAVED"));

        CreateUIButton(
            "[BUTTON] CLEAR FORCE",
            panel.transform,
            "CLEAR FORCE",
            new Vector2(0.38f,0.22f),
            new Vector2(0.56f,0.78f),
            15,
            ClearForce);

        CreateUIButton(
            "[BUTTON] FINAL REVIEW",
            panel.transform,
            "FINAL REVIEW",
            new Vector2(0.58f,0.22f),
            new Vector2(0.76f,0.78f),
            15,
            () => ShowWindow(FinalReviewWindow));

        CreateUIButton(
            "[BUTTON] DEPLOY",
            panel.transform,
            "DEPLOY",
            new Vector2(0.78f,0.22f),
            new Vector2(0.98f,0.78f),
            16,
            () => ShowWindow(ConfirmationWindow));
    }

    private static void CreateMapOverlay(Transform parent)
    {
        MapOverlay =
            CreateUIPanel(
                "[MAP] STRATEGIC DEPLOYMENT MAP",
                parent,
                new Vector2(0.12f,0.12f),
                new Vector2(0.88f,0.88f),
                new Color(0.004f,0.018f,0.024f,0.98f));

        CreateUIText(
            "MAP TITLE",
            MapOverlay.transform,
            "STRATEGIC DEPLOYMENT MAP // THEATER 07",
            new Vector2(0.04f,0.92f),
            new Vector2(0.96f,0.98f),
            23,
            Cyan);

        for (int i = 0; i < 9; i++)
        {
            float x = 0.10f + i * 0.10f;

            CreateUIImage(
                "MAP VERTICAL GRID",
                MapOverlay.transform,
                new Color(0.04f,0.18f,0.22f,0.45f),
                new Vector2(x,0.10f),
                new Vector2(x + 0.001f,0.90f));
        }

        for (int i = 0; i < 7; i++)
        {
            float y = 0.15f + i * 0.11f;

            CreateUIImage(
                "MAP HORIZONTAL GRID",
                MapOverlay.transform,
                new Color(0.04f,0.18f,0.22f,0.45f),
                new Vector2(0.08f,y),
                new Vector2(0.92f,y + 0.001f));
        }

        CreateUIButton(
            "[BUTTON] CLOSE MAP",
            MapOverlay.transform,
            "CLOSE MAP",
            new Vector2(0.75f,0.92f),
            new Vector2(0.96f,0.98f),
            14,
            () => MapOverlay.SetActive(false));

        MapOverlay.SetActive(false);
    }

    private static void CreateFleetWindow(Transform parent)
    {
        FleetWindow =
            CreateUIPanel(
                "[WINDOW] FLEET SELECTION",
                parent,
                new Vector2(0.22f,0.18f),
                new Vector2(0.78f,0.82f),
                new Color(0.005f,0.012f,0.018f,0.99f));

        CreateUIText(
            "FLEET TITLE",
            FleetWindow.transform,
            "FLEET SELECTION // AVAILABLE UNITS",
            new Vector2(0.05f,0.90f),
            new Vector2(0.95f,0.98f),
            22,
            Cyan);

        CreateFleetButton(
            "WARDEN",
            1200,
            0.72f);

        CreateFleetButton(
            "BEACON",
            850,
            0.58f);

        CreateFleetButton(
            "BULLDOG",
            1400,
            0.44f);

        CreateFleetButton(
            "FORGE",
            1700,
            0.30f);

        CreateFleetButton(
            "SENTINEL",
            1100,
            0.16f);

        CreateUIButton(
            "CLOSE FLEET",
            FleetWindow.transform,
            "CLOSE",
            new Vector2(0.76f,0.04f),
            new Vector2(0.95f,0.11f),
            14,
            () => FleetWindow.SetActive(false));

        FleetWindow.SetActive(false);
    }

    private static void CreateFleetButton(
        string unit,
        int cost,
        float y)
    {
        CreateUIButton(
            "[BUTTON] " + unit,
            FleetWindow.transform,
            unit + " // " + cost.ToString("N0") + " DP",
            new Vector2(0.08f,y),
            new Vector2(0.92f,y + 0.085f),
            16,
            () => AddUnit(unit,cost));
    }

    private static void CreateInspectionWindow(Transform parent)
    {
        InspectionWindow =
            CreateUIPanel(
                "[HUD] UNIT INSPECTION",
                parent,
                new Vector2(0.28f,0.24f),
                new Vector2(0.72f,0.76f),
                new Color(0.004f,0.012f,0.018f,0.99f));

        CreateUIText(
            "INSPECTION",
            InspectionWindow.transform,
            "UNIT INSPECTION",
            new Vector2(0.06f,0.86f),
            new Vector2(0.94f,0.96f),
            23,
            Cyan);

        CreateUIText(
            "INSPECTION DATA",
            InspectionWindow.transform,
            "UNIT ID // OP-0001\nSTATUS // READY\nCONDITION // 100%\nROLE // AUTONOMOUS COMBAT\nVETERAN STATUS // STANDARD\nDEPLOYMENT COST // VARIABLE\nAI AUTONOMY // HIGH",
            new Vector2(0.08f,0.38f),
            new Vector2(0.92f,0.82f),
            17,
            White);

        CreateUIButton(
            "CLOSE INSPECTION",
            InspectionWindow.transform,
            "CLOSE",
            new Vector2(0.30f,0.08f),
            new Vector2(0.70f,0.17f),
            15,
            () => InspectionWindow.SetActive(false));

        InspectionWindow.SetActive(false);
    }

    private static void CreateFinalReviewWindow(Transform parent)
    {
        FinalReviewWindow =
            CreateUIPanel(
                "[WINDOW] FINAL REVIEW",
                parent,
                new Vector2(0.16f,0.10f),
                new Vector2(0.84f,0.90f),
                new Color(0.004f,0.010f,0.016f,0.99f));

        CreateUIText(
            "FINAL REVIEW TITLE",
            FinalReviewWindow.transform,
            "FINAL DEPLOYMENT REVIEW",
            new Vector2(0.04f,0.92f),
            new Vector2(0.96f,0.98f),
            24,
            Cyan);

        CreateReviewPanel(
            "FORCE SUMMARY",
            "UNIT COUNT // 0\nTOTAL STRENGTH // 0",
            0.72f);

        CreateReviewPanel(
            "BUDGET SUMMARY",
            "DEPLOYMENT BUDGET // 10,000 DP\nREMAINING // 10,000 DP",
            0.56f);

        CreateReviewPanel(
            "FORMATION SUMMARY",
            "FORMATION // AUTONOMOUS SCREEN",
            0.40f);

        CreateReviewPanel(
            "COMMAND SUMMARY",
            "COMMAND // PRIMARY COMMANDER\nAI COORDINATION // ENABLED",
            0.24f);

        CreateReviewPanel(
            "INTENT / ROE SUMMARY",
            "OBJECTIVE // SECURE AND HOLD\nROE // RETURN FIRE",
            0.08f);

        CreateUIButton(
            "CLOSE REVIEW",
            FinalReviewWindow.transform,
            "CLOSE REVIEW",
            new Vector2(0.76f,0.92f),
            new Vector2(0.96f,0.98f),
            13,
            () => FinalReviewWindow.SetActive(false));

        FinalReviewWindow.SetActive(false);
    }

    private static void CreateReviewPanel(
        string title,
        string body,
        float y)
    {
        GameObject panel =
            CreateUIPanel(
                "[PANEL] " + title,
                FinalReviewWindow.transform,
                new Vector2(0.05f,y),
                new Vector2(0.95f,y + 0.13f),
                Panel2);

        CreateUIText(
            title,
            panel.transform,
            title,
            new Vector2(0.04f,0.62f),
            new Vector2(0.35f,0.92f),
            15,
            Green);

        CreateUIText(
            body,
            panel.transform,
            body,
            new Vector2(0.36f,0.15f),
            new Vector2(0.96f,0.90f),
            13,
            White);
    }

    private static void CreateConfirmationWindow(Transform parent)
    {
        ConfirmationWindow =
            CreateUIPanel(
                "[CONFIRMATION] FINAL DEPLOYMENT",
                parent,
                new Vector2(0.30f,0.30f),
                new Vector2(0.70f,0.70f),
                new Color(0.010f,0.012f,0.016f,0.995f));

        CreateUIText(
            "FINAL DEPLOYMENT",
            ConfirmationWindow.transform,
            "FINAL DEPLOYMENT",
            new Vector2(0.08f,0.78f),
            new Vector2(0.92f,0.94f),
            24,
            Red);

        CreateUIText(
            "WARNING",
            ConfirmationWindow.transform,
            "CONFIRM LAUNCH OF OPERATION?\n\nCOMMAND INTENT WILL BE LOCKED.\nFORCE WILL BE COMMITTED TO THE OPERATION.\nCOMPETITIVE DEPLOYMENT LIMIT: 10,000 DP.",
            new Vector2(0.08f,0.36f),
            new Vector2(0.92f,0.75f),
            16,
            White);

        CreateUIButton(
            "CONFIRM DEPLOYMENT",
            ConfirmationWindow.transform,
            "CONFIRM DEPLOYMENT",
            new Vector2(0.08f,0.12f),
            new Vector2(0.43f,0.25f),
            13,
            ConfirmDeployment);

        CreateUIButton(
            "CANCEL DEPLOYMENT",
            ConfirmationWindow.transform,
            "CANCEL",
            new Vector2(0.57f,0.12f),
            new Vector2(0.92f,0.25f),
            13,
            () => ConfirmationWindow.SetActive(false));

        ConfirmationWindow.SetActive(false);
    }

    private static GameObject CreateUIPanel(
        string name,
        Transform parent,
        Vector2 min,
        Vector2 max,
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

        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image image =
            go.GetComponent<Image>();

        image.color = color;

        return go;
    }

    private static GameObject CreateUIImage(
        string name,
        Transform parent,
        Color color,
        Vector2 min,
        Vector2 max)
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

        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        go.GetComponent<Image>().color = color;

        return go;
    }

    private static Text CreateUIText(
        string name,
        Transform parent,
        string value,
        Vector2 min,
        Vector2 max,
        int size,
        Color color)
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

        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Text text =
            go.GetComponent<Text>();

        text.font =
            GetBuiltinFont();

        text.text = value;
        text.fontSize = size;
        text.color = color;
        text.alignment =
            TextAnchor.MiddleLeft;

        text.horizontalOverflow =
            HorizontalWrapMode.Wrap;

        text.verticalOverflow =
            VerticalWrapMode.Overflow;

        return text;
    }

    private static Button CreateUIButton(
        string name,
        Transform parent,
        string label,
        Vector2 min,
        Vector2 max,
        int size,
        Action action)
    {
        GameObject go =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image),
                typeof(Button));

        go.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image image =
            go.GetComponent<Image>();

        image.color =
            new Color(
                0.025f,
                0.075f,
                0.095f,
                0.98f);

        Button button =
            go.GetComponent<Button>();

        ColorBlock colors =
            button.colors;

        colors.normalColor =
            new Color(0.025f,0.075f,0.095f,1f);

        colors.highlightedColor =
            new Color(0.08f,0.24f,0.29f,1f);

        colors.pressedColor =
            new Color(0.12f,0.38f,0.44f,1f);

        colors.selectedColor =
            colors.highlightedColor;

        button.colors = colors;

        button.onClick.AddListener(
            () => action());

        Text text =
            CreateUIText(
                "[LABEL]",
                go.transform,
                label,
                new Vector2(0.03f,0.02f),
                new Vector2(0.97f,0.98f),
                size,
                White);

        text.alignment =
            TextAnchor.MiddleCenter;

        return button;
    }

    private static Font GetBuiltinFont()
    {
        if (BuiltinFont != null)
            return BuiltinFont;

        BuiltinFont =
            Resources.GetBuiltinResource<Font>(
                "LegacyRuntime.ttf");

        return BuiltinFont;
    }

    private static Transform CreateEmpty(
        string name,
        Transform parent)
    {
        GameObject go =
            new GameObject(name);

        go.transform.SetParent(
            parent,
            false);

        return go.transform;
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
        go.transform.position = position;
        go.transform.localScale = scale;
        go.transform.SetParent(parent);

        Renderer renderer =
            go.GetComponent<Renderer>();

        renderer.sharedMaterial =
            CreateMaterial(color);

        return go;
    }

    private static GameObject CreateSphere(
        string name,
        Vector3 position,
        float scale,
        Color color,
        Transform parent)
    {
        GameObject go =
            GameObject.CreatePrimitive(
                PrimitiveType.Sphere);

        go.name = name;
        go.transform.position = position;
        go.transform.localScale =
            Vector3.one * scale;

        go.transform.SetParent(parent);

        Renderer renderer =
            go.GetComponent<Renderer>();

        renderer.sharedMaterial =
            CreateMaterial(color);

        return go;
    }

    private static void CreateSign(
        string text,
        Vector3 position,
        float size,
        Color color)
    {
        GameObject go =
            new GameObject(
                "[SIGNAGE] " + text,
                typeof(TextMesh));

        go.transform.position = position;

        TextMesh mesh =
            go.GetComponent<TextMesh>();

        mesh.text = text;
        mesh.fontSize = 48;
        mesh.characterSize =
            size * 0.035f;

        mesh.anchor =
            TextAnchor.MiddleCenter;

        mesh.alignment =
            TextAlignment.Center;

        mesh.color = color;

        Renderer renderer =
            go.GetComponent<Renderer>();

        renderer.sharedMaterial =
            CreateMaterial(color);

        go.transform.SetParent(
            FacilityRoot);
    }

    private static Material CreateMaterial(
        Color color)
    {
        Material material =
            new Material(
                Shader.Find("Universal Render Pipeline/Lit"));

        material.color = color;

        return material;
    }

    private static void ShowWindow(
        GameObject window)
    {
        if (window != null)
            window.SetActive(true);
    }

    private static void SetStatus(
        string status)
    {
        if (StatusText != null)
            StatusText.text =
                "STATUS // " + status;
    }

    private static void AddUnit(
        string unit,
        int cost)
    {
        if (remainingBudget < cost)
        {
            SetStatus(
                "INSUFFICIENT DEPLOYMENT BUDGET");

            return;
        }

        SelectedUnits.Add(unit);
        SelectedCosts.Add(cost);

        remainingBudget -= cost;

        selectedUnitCost = cost;

        UpdateBudgetUI();
        UpdateForceUI();

        if (FleetWindow != null)
            FleetWindow.SetActive(false);

        SetStatus(
            unit + " ADDED // " +
            cost.ToString("N0") +
            " DP");
    }

    private static void RemoveLastUnit()
    {
        if (SelectedUnits.Count == 0)
        {
            SetStatus("NO UNIT TO REMOVE");
            return;
        }

        int index =
            SelectedUnits.Count - 1;

        remainingBudget +=
            SelectedCosts[index];

        string unit =
            SelectedUnits[index];

        SelectedUnits.RemoveAt(index);
        SelectedCosts.RemoveAt(index);

        UpdateBudgetUI();
        UpdateForceUI();

        SetStatus(
            unit + " REMOVED");
    }

    private static void ClearForce()
    {
        SelectedUnits.Clear();
        SelectedCosts.Clear();

        remainingBudget =
            deploymentBudget;

        selectedUnitCost = 0;

        UpdateBudgetUI();
        UpdateForceUI();

        SetStatus("FORCE CLEARED");
    }

    private static void UpdateBudgetUI()
    {
        if (BudgetText != null)
        {
            BudgetText.text =
                deploymentBudget.ToString("N0") +
                " / " +
                deploymentBudget.ToString("N0");
        }

        if (RemainingText != null)
        {
            RemainingText.text =
                remainingBudget.ToString("N0") +
                " DP";
        }

        if (UnitCostText != null)
        {
            UnitCostText.text =
                selectedUnitCost.ToString("N0") +
                " DP";
        }
    }

    private static void UpdateForceUI()
    {
        if (ForceText == null)
            return;

        if (SelectedUnits.Count == 0)
        {
            ForceText.text =
                "NO UNITS SELECTED\n\n" +
                "FORCE STRENGTH // 0\n" +
                "UNIT COUNT // 0";

            return;
        }

        string text =
            "SELECTED FORCE\n\n";

        foreach (string unit in SelectedUnits)
        {
            text +=
                "◆ " + unit + "\n";
        }

        text +=
            "\nFORCE STRENGTH // " +
            (SelectedUnits.Count * 100).ToString("N0");

        text +=
            "\nUNIT COUNT // " +
            SelectedUnits.Count.ToString();

        ForceText.text = text;
    }

    private static void ConfirmDeployment()
    {
        ConfirmationWindow.SetActive(false);

        if (SelectedUnits.Count == 0)
        {
            SetStatus(
                "DEPLOYMENT BLOCKED // NO FORCE SELECTED");

            return;
        }

        SetStatus(
            "DEPLOYMENT AUTHORIZED // OPERATION LAUNCHING");

        Debug.Log(
            "DEPLOYMENT AUTHORIZED. Units: " +
            SelectedUnits.Count +
            " Remaining DP: " +
            remainingBudget);
    }
}
