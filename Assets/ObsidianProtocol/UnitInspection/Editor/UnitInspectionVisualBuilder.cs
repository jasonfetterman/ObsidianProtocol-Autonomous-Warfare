using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class UnitInspectionVisualBuilder
{
    private const string ScenePath =
        "Assets/Scenes/SCN-22 UNIT INSPECTION/Unit_Inspection.unity";

    private static Font BuiltinFont;

    [MenuItem("Obsidian Protocol/Build/SCN-22 UNIT INSPECTION - FULL VISUAL")]
    public static void Build()
    {
        VerifyScene();

        Scene scene =
            EditorSceneManager.OpenScene(
                ScenePath,
                OpenSceneMode.Single);

        ClearScene();

        GameObject root =
            new GameObject(
                "19. UNIT INSPECTION / CONFIGURATION");

        BuildPhysicalFacility(root.transform);
        BuildUnitDisplay(root.transform);
        BuildInspectionStations(root.transform);
        BuildConditionWall(root.transform);
        BuildActionStations(root.transform);
        BuildWorldLabels(root.transform);
        BuildLighting(root.transform);
        CreateCamera();
        CreateEventSystem();
        BuildHUD();

        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "SCN-22 UNIT INSPECTION / CONFIGURATION BUILD COMPLETE: " +
            ScenePath);
    }

    private static void VerifyScene()
    {
        string absolute =
            Path.Combine(
                Directory.GetParent(
                    Application.dataPath).FullName,
                ScenePath.Replace(
                    "/",
                    Path.DirectorySeparatorChar.ToString()));

        if (!File.Exists(absolute))
        {
            throw new Exception(
                "EXACT SCENE DOES NOT EXIST: " +
                absolute);
        }
    }

    private static void ClearScene()
    {
        GameObject[] roots =
            SceneManager
                .GetActiveScene()
                .GetRootGameObjects();

        foreach (GameObject root in roots)
        {
            UnityEngine.Object.DestroyImmediate(root);
        }
    }

    // ========================================================
    // PHYSICAL FACILITY
    // ========================================================

    private static void BuildPhysicalFacility(
        Transform parent)
    {
        GameObject facility =
            new GameObject(
                "[PHYSICAL] UNIT INSPECTION FACILITY");

        facility.transform.SetParent(parent);
        facility.transform.localPosition =
            Vector3.zero;

        Cube(
            "[STRUCTURE] FLOOR",
            new Vector3(0f, -0.25f, 0f),
            new Vector3(68f, 0.5f, 60f),
            new Color(
                0.018f,
                0.026f,
                0.035f),
            facility.transform);

        Cube(
            "[STRUCTURE] REAR WALL",
            new Vector3(0f, 9f, 30f),
            new Vector3(68f, 18f, 0.5f),
            new Color(
                0.022f,
                0.034f,
                0.046f),
            facility.transform);

        Cube(
            "[STRUCTURE] LEFT WALL",
            new Vector3(-34f, 9f, 0f),
            new Vector3(0.5f, 18f, 60f),
            new Color(
                0.022f,
                0.034f,
                0.046f),
            facility.transform);

        Cube(
            "[STRUCTURE] RIGHT WALL",
            new Vector3(34f, 9f, 0f),
            new Vector3(0.5f, 18f, 60f),
            new Color(
                0.022f,
                0.034f,
                0.046f),
            facility.transform);

        Cube(
            "[STRUCTURE] CEILING",
            new Vector3(0f, 18f, 0f),
            new Vector3(68f, 0.4f, 60f),
            new Color(
                0.012f,
                0.018f,
                0.025f),
            facility.transform);

        // ----------------------------------------------------
        // FLOOR DATA GRID
        // ----------------------------------------------------

        for (int x = -30; x <= 30; x += 10)
        {
            Cube(
                "[DATA GRID] X",
                new Vector3(
                    x,
                    0.02f,
                    0f),
                new Vector3(
                    0.045f,
                    0.035f,
                    58f),
                new Color(
                    0.025f,
                    0.18f,
                    0.26f),
                facility.transform);
        }

        for (int z = -25; z <= 25; z += 10)
        {
            Cube(
                "[DATA GRID] Z",
                new Vector3(
                    0f,
                    0.025f,
                    z),
                new Vector3(
                    66f,
                    0.035f,
                    0.045f),
                new Color(
                    0.025f,
                    0.18f,
                    0.26f),
                facility.transform);
        }

        // ----------------------------------------------------
        // STRUCTURAL COLUMNS
        // ----------------------------------------------------

        for (int x = -30; x <= 30; x += 10)
        {
            Cylinder(
                "[STRUCTURAL] COLUMN",
                new Vector3(
                    x,
                    9f,
                    28.5f),
                new Vector3(
                    0.45f,
                    9f,
                    0.45f),
                new Color(
                    0.045f,
                    0.06f,
                    0.075f),
                facility.transform);
        }

        // ----------------------------------------------------
        // CEILING BEAMS
        // ----------------------------------------------------

        for (int x = -30; x <= 30; x += 10)
        {
            Cube(
                "[STRUCTURAL] CEILING BEAM",
                new Vector3(
                    x,
                    17f,
                    0f),
                new Vector3(
                    0.32f,
                    0.32f,
                    58f),
                new Color(
                    0.03f,
                    0.05f,
                    0.065f),
                facility.transform);
        }

        // ----------------------------------------------------
        // RAISED INSPECTION PLATFORM
        // ----------------------------------------------------

        Cube(
            "[INSPECTION PLATFORM] RAISED FLOOR",
            new Vector3(
                0f,
                0.35f,
                7f),
            new Vector3(
                24f,
                0.7f,
                18f),
            new Color(
                0.028f,
                0.04f,
                0.052f),
            facility.transform);

        Cube(
            "[INSPECTION PLATFORM] INNER PAD",
            new Vector3(
                0f,
                0.72f,
                7f),
            new Vector3(
                18f,
                0.12f,
                12f),
            new Color(
                0.018f,
                0.085f,
                0.12f),
            facility.transform);

        Cube(
            "[INSPECTION PLATFORM] REAR RAIL",
            new Vector3(
                0f,
                1.8f,
                13f),
            new Vector3(
                24f,
                0.22f,
                0.22f),
            new Color(
                0.04f,
                0.16f,
                0.21f),
            facility.transform);

        Cube(
            "[INSPECTION PLATFORM] LEFT RAIL",
            new Vector3(
                -12f,
                1.8f,
                7f),
            new Vector3(
                0.22f,
                0.22f,
                12f),
            new Color(
                0.04f,
                0.16f,
                0.21f),
            facility.transform);

        Cube(
            "[INSPECTION PLATFORM] RIGHT RAIL",
            new Vector3(
                12f,
                1.8f,
                7f),
            new Vector3(
                0.22f,
                0.22f,
                12f),
            new Color(
                0.04f,
                0.16f,
                0.21f),
            facility.transform);
    }

    // ========================================================
    // UNIT MODEL
    // ========================================================

    private static void BuildUnitDisplay(
        Transform parent)
    {
        GameObject unit =
            new GameObject(
                "[UNIT] WARDEN-01 // SELECTED");

        unit.transform.SetParent(parent);
        unit.transform.localPosition =
            new Vector3(
                0f,
                2.2f,
                7f);

        Cube(
            "[UNIT] MAIN CHASSIS",
            new Vector3(
                0f,
                1.2f,
                0f),
            new Vector3(
                7f,
                1.6f,
                4.2f),
            new Color(
                0.10f,
                0.12f,
                0.14f),
            unit.transform);

        Cube(
            "[UNIT] UPPER ARMOR",
            new Vector3(
                0f,
                2.15f,
                0f),
            new Vector3(
                5.2f,
                0.55f,
                3.1f),
            new Color(
                0.14f,
                0.16f,
                0.18f),
            unit.transform);

        Cube(
            "[UNIT] FRONT SENSOR HOUSING",
            new Vector3(
                0f,
                2.2f,
                2.05f),
            new Vector3(
                2.2f,
                0.65f,
                0.55f),
            new Color(
                0.025f,
                0.18f,
                0.24f),
            unit.transform);

        Cube(
            "[UNIT] REAR POWER HOUSING",
            new Vector3(
                0f,
                2f,
                -2f),
            new Vector3(
                3f,
                0.7f,
                0.55f),
            new Color(
                0.065f,
                0.075f,
                0.085f),
            unit.transform);

        float[] wheelX =
        {
            -3f,
            3f
        };

        float[] wheelZ =
        {
            -1.55f,
            1.55f
        };

        foreach (float x in wheelX)
        {
            foreach (float z in wheelZ)
            {
                GameObject wheel =
                    CylinderObject(
                        "[UNIT] MOBILITY WHEEL",
                        new Vector3(
                            x,
                            0.55f,
                            z),
                        new Vector3(
                            1.15f,
                            0.48f,
                            1.15f),
                        new Color(
                            0.035f,
                            0.04f,
                            0.045f),
                        unit.transform);

                wheel.transform.localRotation =
                    Quaternion.Euler(
                        0f,
                        0f,
                        90f);
            }
        }

        Cylinder(
            "[UNIT] SENSOR MAST",
            new Vector3(
                0f,
                3.7f,
                -0.2f),
            new Vector3(
                0.25f,
                1.5f,
                0.25f),
            new Color(
                0.055f,
                0.09f,
                0.11f),
            unit.transform);

        Cylinder(
            "[UNIT] SENSOR HEAD",
            new Vector3(
                0f,
                5.15f,
                -0.2f),
            new Vector3(
                0.75f,
                0.32f,
                0.75f),
            new Color(
                0.025f,
                0.20f,
                0.27f),
            unit.transform);

        Cube(
            "[UNIT] STATUS LIGHT",
            new Vector3(
                -2.8f,
                2.5f,
                1.85f),
            new Vector3(
                0.28f,
                0.18f,
                0.12f),
            new Color(
                0.05f,
                0.75f,
                0.9f),
            unit.transform);

        // Unit designation above the model.
        Sign(
            "WARDEN-01",
            new Vector3(
                0f,
                6.4f,
                7f),
            0.9f,
            new Color(
                0.55f,
                0.9f,
                1f),
            Quaternion.identity,
            parent);

        Sign(
            "AUTONOMOUS RECON / INSPECTION MODE",
            new Vector3(
                0f,
                5.55f,
                7f),
            0.38f,
            new Color(
                0.45f,
                0.68f,
                0.75f),
            Quaternion.identity,
            parent);

        // Sensor scan pedestals.
        for (int i = 0; i < 4; i++)
        {
            float x =
                -9f +
                i * 6f;

            Cylinder(
                "[SCAN] SENSOR PEDESTAL",
                new Vector3(
                    x,
                    1.1f,
                    0.5f),
                new Vector3(
                    0.35f,
                    1.1f,
                    0.35f),
                new Color(
                    0.025f,
                    0.10f,
                    0.13f),
                parent);

            Cube(
                "[SCAN] SENSOR LIGHT",
                new Vector3(
                    x,
                    2.25f,
                    0.5f),
                new Vector3(
                    0.65f,
                    0.08f,
                    0.65f),
                new Color(
                    0.02f,
                    0.22f,
                    0.3f),
                parent);
        }
    }

    // ========================================================
    // INSPECTION STATIONS
    // ========================================================

    private static void BuildInspectionStations(
        Transform parent)
    {
        BuildStation(
            "[STATION] PERFORMANCE",
            new Vector3(
                -25f,
                3.5f,
                20f),
            "PERFORMANCE / MOBILITY",
            parent);

        BuildStation(
            "[STATION] SENSORS",
            new Vector3(
                -13f,
                3.5f,
                20f),
            "SENSORS / COMMUNICATIONS",
            parent);

        BuildStation(
            "[STATION] EQUIPMENT",
            new Vector3(
                13f,
                3.5f,
                20f),
            "EQUIPMENT / DEFENSE",
            parent);

        BuildStation(
            "[STATION] AI",
            new Vector3(
                25f,
                3.5f,
                20f),
            "AI / HISTORY",
            parent);
    }

    private static void BuildStation(
        string name,
        Vector3 position,
        string subtitle,
        Transform parent)
    {
        GameObject station =
            new GameObject(name);

        station.transform.SetParent(parent);
        station.transform.localPosition =
            position;

        Cube(
            "[STATION] CONSOLE",
            Vector3.zero,
            new Vector3(
                9f,
                3.8f,
                1.5f),
            new Color(
                0.025f,
                0.04f,
                0.052f),
            station.transform);

        Cube(
            "[STATION] SCREEN",
            new Vector3(
                0f,
                2.7f,
                -0.15f),
            new Vector3(
                7.5f,
                3.2f,
                0.18f),
            new Color(
                0.018f,
                0.09f,
                0.12f),
            station.transform);

        Sign(
            name.Replace(
                "[STATION] ",
                ""),
            position +
                new Vector3(
                    0f,
                    5f,
                    -0.8f),
            0.55f,
            new Color(
                0.45f,
                0.85f,
                0.95f),
            Quaternion.Euler(
                0f,
                180f,
                0f),
            parent);

        Sign(
            subtitle,
            position +
                new Vector3(
                    0f,
                    2.7f,
                    -0.3f),
            0.26f,
            new Color(
                0.55f,
                0.62f,
                0.67f),
            Quaternion.Euler(
                0f,
                180f,
                0f),
            parent);
    }

    // ========================================================
    // CONDITION WALL
    // ========================================================

    private static void BuildConditionWall(
        Transform parent)
    {
        GameObject wall =
            new GameObject(
                "[PANEL] UNIT CONDITION DISPLAY");

        wall.transform.SetParent(parent);
        wall.transform.localPosition =
            new Vector3(
                0f,
                6.5f,
                28.8f);

        Cube(
            "[DISPLAY] CONDITION WALL",
            Vector3.zero,
            new Vector3(
                38f,
                10f,
                0.35f),
            new Color(
                0.018f,
                0.03f,
                0.042f),
            wall.transform);

        string[] labels =
        {
            "HULL 98%",
            "ENGINE 94%",
            "PROPULSION 97%",
            "SENSORS 91%",
            "COMMUNICATIONS 99%",
            "COOLING 96%",
            "ENERGY 88%"
        };

        for (int i = 0;
             i < labels.Length;
             i++)
        {
            float x =
                -15f +
                i * 5f;

            Color c =
                i == 6
                    ? new Color(
                        0.95f,
                        0.65f,
                        0.22f)
                    : new Color(
                        0.45f,
                        0.9f,
                        0.72f);

            Sign(
                labels[i],
                new Vector3(
                    x,
                    6.6f,
                    28.35f),
                0.28f,
                c,
                Quaternion.Euler(
                    0f,
                    180f,
                    0f),
                parent);
        }
    }

    // ========================================================
    // ACTION STATIONS
    // ========================================================

    private static void BuildActionStations(
        Transform parent)
    {
        string[] actions =
        {
            "REPAIR",
            "CUSTOMIZE",
            "UPGRADE",
            "EQUIP",
            "DEPLOY"
        };

        for (int i = 0;
             i < actions.Length;
             i++)
        {
            float x =
                -20f +
                i * 10f;

            Cube(
                "[ACTION] " +
                actions[i],
                new Vector3(
                    x,
                    1.15f,
                    -19f),
                new Vector3(
                    7.5f,
                    2.3f,
                    2f),
                new Color(
                    0.025f,
                    0.045f,
                    0.06f),
                parent);

            Sign(
                actions[i],
                new Vector3(
                    x,
                    2.2f,
                    -20.05f),
                0.42f,
                new Color(
                    0.55f,
                    0.82f,
                    0.92f),
                Quaternion.Euler(
                    0f,
                    180f,
                    0f),
                parent);
        }
    }

    // ========================================================
    // WORLD LABELS
    // ========================================================

    private static void BuildWorldLabels(
        Transform parent)
    {
        Sign(
            "UNIT INSPECTION / CONFIGURATION",
            new Vector3(
                0f,
                15.5f,
                29f),
            0.9f,
            new Color(
                0.55f,
                0.9f,
                1f),
            Quaternion.Euler(
                0f,
                180f,
                0f),
            parent);

        Sign(
            "SELECTED UNIT // WARDEN-01",
            new Vector3(
                -23f,
                9.5f,
                28.6f),
            0.45f,
            new Color(
                0.4f,
                0.75f,
                0.85f),
            Quaternion.Euler(
                0f,
                180f,
                0f),
            parent);

        Sign(
            "OWNED / OPERATIONAL",
            new Vector3(
                23f,
                9.5f,
                28.6f),
            0.45f,
            new Color(
                0.45f,
                0.9f,
                0.7f),
            Quaternion.Euler(
                0f,
                180f,
                0f),
            parent);

        Sign(
            "INSPECTION PLATFORM",
            new Vector3(
                0f,
                0.9f,
                -1.5f),
            0.42f,
            new Color(
                0.35f,
                0.7f,
                0.78f),
            Quaternion.identity,
            parent);
    }

    // ========================================================
    // LIGHTING
    // ========================================================

    private static void BuildLighting(
        Transform parent)
    {
        GameObject lightRoot =
            new GameObject(
                "[SYSTEM] INSPECTION LIGHTING");

        lightRoot.transform.SetParent(parent);

        GameObject key =
            new GameObject(
                "[LIGHT] KEY",
                typeof(Light));

        key.transform.SetParent(
            lightRoot.transform);

        key.transform.position =
            new Vector3(
                0f,
                16f,
                -8f);

        key.transform.rotation =
            Quaternion.Euler(
                48f,
                0f,
                0f);

        Light keyLight =
            key.GetComponent<Light>();

        keyLight.type =
            LightType.Directional;

        keyLight.intensity =
            1.0f;

        keyLight.color =
            new Color(
                0.72f,
                0.86f,
                1f);

        CreatePointLight(
            lightRoot.transform,
            new Vector3(
                0f,
                9f,
                7f),
            new Color(
                0.18f,
                0.65f,
                0.9f),
            850f,
            18f);

        CreatePointLight(
            lightRoot.transform,
            new Vector3(
                -24f,
                8f,
                20f),
            new Color(
                0.15f,
                0.55f,
                0.75f),
            500f,
            15f);

        CreatePointLight(
            lightRoot.transform,
            new Vector3(
                24f,
                8f,
                20f),
            new Color(
                0.15f,
                0.55f,
                0.75f),
            500f,
            15f);

        CreatePointLight(
            lightRoot.transform,
            new Vector3(
                0f,
                7f,
                -20f),
            new Color(
                0.1f,
                0.4f,
                0.55f),
            450f,
            14f);
    }

    private static void CreatePointLight(
        Transform parent,
        Vector3 position,
        Color color,
        float range,
        float intensity)
    {
        GameObject obj =
            new GameObject(
                "[LIGHT] POINT",
                typeof(Light));

        obj.transform.SetParent(parent);
        obj.transform.position =
            position;

        Light light =
            obj.GetComponent<Light>();

        light.type =
            LightType.Point;

        light.color =
            color;

        light.range =
            range;

        light.intensity =
            intensity;
    }

    // ========================================================
    // CAMERA
    // ========================================================

    private static void CreateCamera()
    {
        GameObject cameraObject =
            new GameObject(
                "UNIT INSPECTION CAMERA",
                typeof(Camera),
                typeof(AudioListener));

        Camera camera =
            cameraObject.GetComponent<Camera>();

        camera.clearFlags =
            CameraClearFlags.SolidColor;

        camera.backgroundColor =
            new Color(
                0.008f,
                0.012f,
                0.018f,
                1f);

        camera.fieldOfView =
            67f;

        camera.nearClipPlane =
            0.05f;

        camera.farClipPlane =
            500f;

        camera.depth =
            -100f;

        camera.tag =
            "MainCamera";

        cameraObject.transform.position =
            new Vector3(
                0f,
                8.5f,
                -27f);

        LookAt(
            cameraObject.transform,
            new Vector3(
                0f,
                5f,
                8f));
    }

    // ========================================================
    // INPUT SYSTEM EVENT SYSTEM
    // ========================================================

    private static void CreateEventSystem()
    {
        GameObject[] systems =
            UnityEngine.Object.FindObjectsByType<GameObject>(
                FindObjectsSortMode.None);

        foreach (GameObject obj in systems)
        {
            if (obj.GetComponent<EventSystem>() != null)
            {
                UnityEngine.Object.DestroyImmediate(
                    obj);
            }
        }

        new GameObject(
            "EVENT SYSTEM",
            typeof(EventSystem),
            typeof(InputSystemUIInputModule));
    }

    // ========================================================
    // HUD
    // ========================================================

    private static void BuildHUD()
    {
        GameObject canvasObject =
            new GameObject(
                "[HUD] UNIT INSPECTION HUD",
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));

        Canvas canvas =
            canvasObject.GetComponent<Canvas>();

        canvas.renderMode =
            RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler =
            canvasObject.GetComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(
                1920f,
                1080f);

        scaler.matchWidthOrHeight =
            0.5f;

        BuildHUDHeader(
            canvasObject.transform);

        BuildUnitHeaderPanel(
            canvasObject.transform);

        BuildInformationPanel(
            canvasObject.transform);

        BuildConditionPanel(
            canvasObject.transform);

        BuildActionsPanel(
            canvasObject.transform);

        BuildRepairPanel(
            canvasObject.transform);

        BuildCustomizationPanel(
            canvasObject.transform);

        BuildUpgradePanel(
            canvasObject.transform);

        BuildEquipmentPanel(
            canvasObject.transform);

        BuildDeploymentPanel(
            canvasObject.transform);

        BuildBottomStatus(
            canvasObject.transform);
    }

    // ========================================================
    // HUD HEADER
    // ========================================================

    private static void BuildHUDHeader(
        Transform canvas)
    {
        GameObject panel =
            UIPanel(
                "[PANEL] HUD HEADER",
                canvas,
                new Vector2(
                    0.5f,
                    0.955f),
                new Vector2(
                    1840f,
                    65f));

        UILabel(
            "OBSIDIAN PROTOCOL",
            panel.transform,
            new Vector2(
                0.12f,
                0.5f),
            new Vector2(
                300f,
                35f),
            20,
            new Color(
                0.45f,
                0.78f,
                0.88f));

        UILabel(
            "19. UNIT INSPECTION / CONFIGURATION",
            panel.transform,
            new Vector2(
                0.50f,
                0.5f),
            new Vector2(
                650f,
                35f),
            19,
            new Color(
                0.68f,
                0.86f,
                0.92f));

        UILabel(
            "INSPECTION LINK: ONLINE",
            panel.transform,
            new Vector2(
                0.86f,
                0.5f),
            new Vector2(
                330f,
                35f),
            16,
            new Color(
                0.35f,
                0.9f,
                0.65f));
    }

    // ========================================================
    // UNIT HEADER
    // ========================================================

    private static void BuildUnitHeaderPanel(
        Transform canvas)
    {
        GameObject panel =
            UIPanel(
                "[PANEL] UNIT HEADER",
                canvas,
                new Vector2(
                    0.5f,
                    0.83f),
                new Vector2(
                    1160f,
                    125f));

        UILabel(
            "[DISPLAY] WARDEN-01",
            panel.transform,
            new Vector2(
                0.18f,
                0.60f),
            new Vector2(
                320f,
                42f),
            28,
            new Color(
                0.65f,
                0.9f,
                1f));

        UILabel(
            "[DISPLAY] AIR / AUTONOMOUS RECON",
            panel.transform,
            new Vector2(
                0.45f,
                0.60f),
            new Vector2(
                360f,
                35f),
            19,
            new Color(
                0.55f,
                0.72f,
                0.78f));

        UILabel(
            "[DISPLAY] STATUS: OPERATIONAL",
            panel.transform,
            new Vector2(
                0.73f,
                0.60f),
            new Vector2(
                330f,
                35f),
            18,
            new Color(
                0.35f,
                0.9f,
                0.65f));

        UILabel(
            "[DISPLAY] OWNED / AVAILABLE FOR CONFIGURATION",
            panel.transform,
            new Vector2(
                0.50f,
                0.20f),
            new Vector2(
                700f,
                30f),
            15,
            new Color(
                0.42f,
                0.56f,
                0.64f));
    }

    // ========================================================
    // UNIT INFORMATION
    // ========================================================

    private static void BuildInformationPanel(
        Transform canvas)
    {
        GameObject panel =
            UIPanel(
                "[PANEL] UNIT INFORMATION",
                canvas,
                new Vector2(
                    0.13f,
                    0.48f),
                new Vector2(
                    430f,
                    540f));

        string[] tabs =
        {
            "OVERVIEW",
            "PERFORMANCE",
            "MOBILITY",
            "SENSORS",
            "COMMUNICATIONS",
            "EQUIPMENT",
            "ARMOR / DEFENSE",
            "AI",
            "HISTORY"
        };

        for (int i = 0;
             i < tabs.Length;
             i++)
        {
            UIButton(
                "[BUTTON] " + tabs[i],
                panel.transform,
                new Vector2(
                    0.5f,
                    0.89f -
                    i * 0.092f),
                new Vector2(
                    350f,
                    42f),
                tabs[i],
                new Color(
                    0.025f,
                    0.08f,
                    0.105f));
        }
    }

    // ========================================================
    // CONDITION
    // ========================================================

    private static void BuildConditionPanel(
        Transform canvas)
    {
        GameObject panel =
            UIPanel(
                "[PANEL] UNIT CONDITION",
                canvas,
                new Vector2(
                    0.87f,
                    0.48f),
                new Vector2(
                    430f,
                    540f));

        string[] condition =
        {
            "Hull                         98%",
            "Engine                       94%",
            "Propulsion                   97%",
            "Sensors                      91%",
            "Communications               99%",
            "Cooling                      96%",
            "Energy                       88%"
        };

        for (int i = 0;
             i < condition.Length;
             i++)
        {
            Color c =
                i == 6
                    ? new Color(
                        0.95f,
                        0.65f,
                        0.2f)
                    : new Color(
                        0.4f,
                        0.9f,
                        0.68f);

            UILabel(
                "[DISPLAY] " +
                condition[i],
                panel.transform,
                new Vector2(
                    0.5f,
                    0.82f -
                    i * 0.09f),
                new Vector2(
                    360f,
                    40f),
                19,
                c);
        }

        UILabel(
            "[DISPLAY] MAINTENANCE WINDOW: 12.4 HR",
            panel.transform,
            new Vector2(
                0.5f,
                0.11f),
            new Vector2(
                370f,
                35f),
            16,
            new Color(
                0.55f,
                0.65f,
                0.72f));
    }

    // ========================================================
    // UNIT ACTIONS
    // ========================================================

    private static void BuildActionsPanel(
        Transform canvas)
    {
        GameObject panel =
            UIPanel(
                "[PANEL] UNIT ACTIONS",
                canvas,
                new Vector2(
                    0.5f,
                    0.13f),
                new Vector2(
                    1220f,
                    185f));

        string[] actions =
        {
            "REPAIR",
            "CUSTOMIZE",
            "UPGRADE",
            "EQUIP",
            "DEPLOY"
        };

        for (int i = 0;
             i < actions.Length;
             i++)
        {
            UIButton(
                "[BUTTON] " +
                actions[i],
                panel.transform,
                new Vector2(
                    0.10f +
                    i * 0.20f,
                    0.57f),
                new Vector2(
                    190f,
                    62f),
                actions[i],
                new Color(
                    0.025f,
                    0.085f,
                    0.11f));
        }

        UILabel(
            "[DISPLAY] ACTIONS REQUIRE CONFIRMATION BEFORE COMMIT",
            panel.transform,
            new Vector2(
                0.5f,
                0.16f),
            new Vector2(
                850f,
                30f),
            15,
            new Color(
                0.42f,
                0.55f,
                0.62f));
    }

    // ========================================================
    // REPAIR HUD
    // ========================================================

    private static void BuildRepairPanel(
        Transform canvas)
    {
        GameObject panel =
            UIPanel(
                "[HUD] REPAIR",
                canvas,
                new Vector2(
                    0.35f,
                    0.67f),
                new Vector2(
                    360f,
                    190f));

        UILabel(
            "REPAIR",
            panel.transform,
            new Vector2(
                0.5f,
                0.78f),
            new Vector2(
                300f,
                32f),
            20,
            new Color(
                0.55f,
                0.85f,
                0.92f));

        UILabel(
            "DAMAGE: 12%",
            panel.transform,
            new Vector2(
                0.5f,
                0.53f),
            new Vector2(
                250f,
                30f),
            16,
            new Color(
                0.75f,
                0.78f,
                0.8f));

        UILabel(
            "EST. COST: 420 CREDITS",
            panel.transform,
            new Vector2(
                0.5f,
                0.30f),
            new Vector2(
                280f,
                30f),
            15,
            new Color(
                0.55f,
                0.65f,
                0.72f));
    }

    // ========================================================
    // CUSTOMIZATION HUD
    // ========================================================

    private static void BuildCustomizationPanel(
        Transform canvas)
    {
        GameObject panel =
            UIPanel(
                "[HUD] CUSTOMIZATION",
                canvas,
                new Vector2(
                    0.50f,
                    0.67f),
                new Vector2(
                    360f,
                    190f));

        UILabel(
            "CUSTOMIZATION",
            panel.transform,
            new Vector2(
                0.5f,
                0.78f),
            new Vector2(
                300f,
                32f),
            20,
            new Color(
                0.55f,
                0.85f,
                0.92f));

        UILabel(
            "APPEARANCE / MARKINGS",
            panel.transform,
            new Vector2(
                0.5f,
                0.53f),
            new Vector2(
                280f,
                30f),
            16,
            new Color(
                0.75f,
                0.78f,
                0.8f));

        UILabel(
            "COSMETIC SYSTEM",
            panel.transform,
            new Vector2(
                0.5f,
                0.30f),
            new Vector2(
                280f,
                30f),
            15,
            new Color(
                0.55f,
                0.65f,
                0.72f));
    }

    // ========================================================
    // UPGRADE HUD
    // ========================================================

    private static void BuildUpgradePanel(
        Transform canvas)
    {
        GameObject panel =
            UIPanel(
                "[HUD] UPGRADE",
                canvas,
                new Vector2(
                    0.65f,
                    0.67f),
                new Vector2(
                    360f,
                    190f));

        UILabel(
            "UPGRADE",
            panel.transform,
            new Vector2(
                0.5f,
                0.78f),
            new Vector2(
                300f,
                32f),
            20,
            new Color(
                0.55f,
                0.85f,
                0.92f));

        UILabel(
            "MOBILITY / SENSOR / AI",
            panel.transform,
            new Vector2(
                0.5f,
                0.53f),
            new Vector2(
                300f,
                30f),
            16,
            new Color(
                0.75f,
                0.78f,
                0.8f));

        UILabel(
            "RESEARCH REQUIRED",
            panel.transform,
            new Vector2(
                0.5f,
                0.30f),
            new Vector2(
                280f,
                30f),
            15,
            new Color(
                0.55f,
                0.65f,
                0.72f));
    }

    // ========================================================
    // EQUIPMENT HUD
    // ========================================================

    private static void BuildEquipmentPanel(
        Transform canvas)
    {
        GameObject panel =
            UIPanel(
                "[HUD] EQUIPMENT",
                canvas,
                new Vector2(
                    0.80f,
                    0.67f),
                new Vector2(
                    360f,
                    190f));

        UILabel(
            "EQUIPMENT",
            panel.transform,
            new Vector2(
                0.5f,
                0.78f),
            new Vector2(
                300f,
                32f),
            20,
            new Color(
                0.55f,
                0.85f,
                0.92f));

        UILabel(
            "SLOTS: 4 / 6",
            panel.transform,
            new Vector2(
                0.5f,
                0.53f),
            new Vector2(
                260f,
                30f),
            16,
            new Color(
                0.75f,
                0.78f,
                0.8f));

        UILabel(
            "LOADOUT READY",
            panel.transform,
            new Vector2(
                0.5f,
                0.30f),
            new Vector2(
                280f,
                30f),
            15,
            new Color(
                0.55f,
                0.65f,
                0.72f));
    }

    // ========================================================
    // DEPLOYMENT HUD
    // ========================================================

    private static void BuildDeploymentPanel(
        Transform canvas)
    {
        GameObject panel =
            UIPanel(
                "[HUD] DEPLOYMENT",
                canvas,
                new Vector2(
                    0.94f,
                    0.67f),
                new Vector2(
                    260f,
                    190f));

        UILabel(
            "DEPLOY",
            panel.transform,
            new Vector2(
                0.5f,
                0.78f),
            new Vector2(
                220f,
                32f),
            20,
            new Color(
                0.55f,
                0.85f,
                0.92f));

        UILabel(
            "10,000 DP LIMIT",
            panel.transform,
            new Vector2(
                0.5f,
                0.53f),
            new Vector2(
                220f,
                30f),
            16,
            new Color(
                0.75f,
                0.78f,
                0.8f));

        UILabel(
            "COMPETITIVE LIMIT ENFORCED",
            panel.transform,
            new Vector2(
                0.5f,
                0.30f),
            new Vector2(
                240f,
                30f),
            13,
            new Color(
                0.55f,
                0.65f,
                0.72f));
    }

    // ========================================================
    // BOTTOM STATUS
    // ========================================================

    private static void BuildBottomStatus(
        Transform canvas)
    {
        GameObject panel =
            UIPanel(
                "[PANEL] INSPECTION STATUS",
                canvas,
                new Vector2(
                    0.5f,
                    0.035f),
                new Vector2(
                    1840f,
                    45f));

        UILabel(
            "UNIT READY  //  INSPECTION LINK ACTIVE  //  CONFIGURATION DATA SYNCHRONIZED",
            panel.transform,
            new Vector2(
                0.5f,
                0.5f),
            new Vector2(
                1300f,
                30f),
            15,
            new Color(
                0.38f,
                0.65f,
                0.72f));
    }

    // ========================================================
    // UI PANEL
    // ========================================================

    private static GameObject UIPanel(
        string name,
        Transform parent,
        Vector2 anchor,
        Vector2 size)
    {
        GameObject obj =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image));

        obj.transform.SetParent(parent);

        RectTransform rect =
            obj.GetComponent<RectTransform>();

        rect.anchorMin =
            anchor;

        rect.anchorMax =
            anchor;

        rect.pivot =
            new Vector2(
                0.5f,
                0.5f);

        rect.sizeDelta =
            size;

        rect.anchoredPosition =
            Vector2.zero;

        Image image =
            obj.GetComponent<Image>();

        image.color =
            new Color(
                0.008f,
                0.025f,
                0.035f,
                0.94f);

        return obj;
    }

    // ========================================================
    // UI LABEL
    // ========================================================

    private static void UILabel(
        string text,
        Transform parent,
        Vector2 anchor,
        Vector2 size,
        int fontSize,
        Color color)
    {
        GameObject obj =
            new GameObject(
                "[DISPLAY] " + text,
                typeof(RectTransform),
                typeof(Text));

        obj.transform.SetParent(parent);

        RectTransform rect =
            obj.GetComponent<RectTransform>();

        rect.anchorMin =
            anchor;

        rect.anchorMax =
            anchor;

        rect.pivot =
            new Vector2(
                0.5f,
                0.5f);

        rect.sizeDelta =
            size;

        rect.anchoredPosition =
            Vector2.zero;

        Text label =
            obj.GetComponent<Text>();

        label.text =
            text;

        label.font =
            GetFont();

        label.fontSize =
            fontSize;

        label.color =
            color;

        label.alignment =
            TextAnchor.MiddleCenter;

        label.horizontalOverflow =
            HorizontalWrapMode.Overflow;

        label.verticalOverflow =
            VerticalWrapMode.Overflow;
    }

    // ========================================================
    // UI BUTTON
    // ========================================================

    private static void UIButton(
        string name,
        Transform parent,
        Vector2 anchor,
        Vector2 size,
        string label,
        Color color)
    {
        GameObject obj =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image),
                typeof(Button));

        obj.transform.SetParent(parent);

        RectTransform rect =
            obj.GetComponent<RectTransform>();

        rect.anchorMin =
            anchor;

        rect.anchorMax =
            anchor;

        rect.pivot =
            new Vector2(
                0.5f,
                0.5f);

        rect.sizeDelta =
            size;

        rect.anchoredPosition =
            Vector2.zero;

        Image image =
            obj.GetComponent<Image>();

        image.color =
            color;

        Button button =
            obj.GetComponent<Button>();

        ColorBlock colors =
            button.colors;

        colors.normalColor =
            color;

        colors.highlightedColor =
            new Color(
                0.05f,
                0.22f,
                0.28f);

        colors.pressedColor =
            new Color(
                0.08f,
                0.30f,
                0.36f);

        colors.selectedColor =
            new Color(
                0.04f,
                0.18f,
                0.24f);

        colors.fadeDuration =
            0.08f;

        button.colors =
            colors;

        GameObject textObject =
            new GameObject(
                "[LABEL] " + label,
                typeof(RectTransform),
                typeof(Text));

        textObject.transform.SetParent(
            obj.transform);

        RectTransform textRect =
            textObject.GetComponent<RectTransform>();

        textRect.anchorMin =
            Vector2.zero;

        textRect.anchorMax =
            Vector2.one;

        textRect.offsetMin =
            Vector2.zero;

        textRect.offsetMax =
            Vector2.zero;

        Text text =
            textObject.GetComponent<Text>();

        text.text =
            label;

        text.font =
            GetFont();

        text.fontSize =
            17;

        text.fontStyle =
            FontStyle.Bold;

        text.color =
            new Color(
                0.7f,
                0.9f,
                0.95f);

        text.alignment =
            TextAnchor.MiddleCenter;
    }

    // ========================================================
    // 3D CUBE
    // ========================================================

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

        obj.name =
            name;

        if (parent != null)
        {
            obj.transform.SetParent(
                parent);
        }

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

    // ========================================================
    // 3D CYLINDER
    // ========================================================

    private static void Cylinder(
        string name,
        Vector3 position,
        Vector3 scale,
        Color color,
        Transform parent)
    {
        CylinderObject(
            name,
            position,
            scale,
            color,
            parent);
    }

    private static GameObject CylinderObject(
        string name,
        Vector3 position,
        Vector3 scale,
        Color color,
        Transform parent)
    {
        GameObject obj =
            GameObject.CreatePrimitive(
                PrimitiveType.Cylinder);

        obj.name =
            name;

        if (parent != null)
        {
            obj.transform.SetParent(
                parent);
        }

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

    // ========================================================
    // MATERIAL
    // ========================================================

    private static Material Material(
        Color color)
    {
        Shader shader =
            Shader.Find(
                "Universal Render Pipeline/Lit");

        if (shader == null)
        {
            shader =
                Shader.Find(
                    "Standard");
        }

        Material material =
            new Material(shader);

        material.color =
            color;

        if (material.HasProperty(
            "_BaseColor"))
        {
            material.SetColor(
                "_BaseColor",
                color);
        }

        if (material.HasProperty(
            "_Metallic"))
        {
            material.SetFloat(
                "_Metallic",
                0.35f);
        }

        if (material.HasProperty(
            "_Smoothness"))
        {
            material.SetFloat(
                "_Smoothness",
                0.78f);
        }

        return material;
    }

    // ========================================================
    // WORLD TEXT
    // ========================================================

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
        {
            obj.transform.SetParent(
                parent);
        }

        obj.transform.position =
            position;

        obj.transform.rotation =
            rotation;

        TextMesh mesh =
            obj.AddComponent<TextMesh>();

        mesh.text =
            text;

        mesh.font =
            GetFont();

        mesh.fontSize =
            64;

        mesh.characterSize =
            size * 0.10f;

        mesh.anchor =
            TextAnchor.MiddleCenter;

        mesh.alignment =
            TextAlignment.Center;

        mesh.color =
            color;
    }

    // ========================================================
    // FONT
    // ========================================================

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

    // ========================================================
    // CAMERA LOOK
    // ========================================================

    private static void LookAt(
        Transform target,
        Vector3 worldPosition)
    {
        Vector3 direction =
            worldPosition -
            target.position;

        target.rotation =
            Quaternion.LookRotation(
                direction.normalized,
                Vector3.up);
    }
}
