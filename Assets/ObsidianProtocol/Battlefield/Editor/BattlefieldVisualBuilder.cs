using System;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ObsidianProtocol.Game.Command.Autonomy;

public static class BattlefieldVisualBuilder
{
    private const string ScenePath = "Assets/Scenes/SCN03  BATTLEFIELD/[HUD] BATTLEFIELD HUD/Battlefield_HUD.unity";

    private static Font BuiltinFont;

    // ========================================================
    // MAIN BUILD
    // ========================================================

    [MenuItem("Obsidian Protocol/Build/SCN-03 BATTLEFIELD - FULL VISUAL")]
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
                "10. BATTLEFIELD COMMAND");

        BuildBattlefield(root.transform);
        BuildCommandCenter(root.transform);
        BuildTacticalDisplays(root.transform);
        BuildUnitGroups(root.transform);
        BuildLighting(root.transform);
        CreateCamera();
        CreateEventSystem();
        BuildHUD();

        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "SCN-03 BATTLEFIELD COMMAND BUILD COMPLETE: " +
            ScenePath);
    }

    // ========================================================
    // VERIFY EXACT SCENE
    // ========================================================

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
                "EXACT BATTLEFIELD SCENE DOES NOT EXIST: " +
                absolute);
        }
    }

    // ========================================================
    // CLEAR EXISTING SCENE
    // ========================================================

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
    // PHYSICAL BATTLEFIELD
    // ========================================================

    private static void BuildBattlefield(
        Transform parent)
    {
        GameObject battlefield =
            new GameObject(
                "[PHYSICAL] BATTLEFIELD");

        battlefield.transform.SetParent(parent);

        // Ground.
        Cube(
            "[TERRAIN] BATTLEFIELD GROUND",
            new Vector3(
                0f,
                -0.5f,
                20f),
            new Vector3(
                150f,
                1f,
                130f),
            new Color(
                0.018f,
                0.028f,
                0.024f),
            battlefield.transform);

        // Tactical grid.
        for (
            int x = -70;
            x <= 70;
            x += 10)
        {
            Cube(
                "[TACTICAL GRID] X",
                new Vector3(
                    x,
                    0.04f,
                    20f),
                new Vector3(
                    0.045f,
                    0.025f,
                    120f),
                new Color(
                    0.02f,
                    0.13f,
                    0.12f),
                battlefield.transform);
        }

        for (
            int z = -40;
            z <= 80;
            z += 10)
        {
            Cube(
                "[TACTICAL GRID] Z",
                new Vector3(
                    0f,
                    0.045f,
                    z),
                new Vector3(
                    140f,
                    0.025f,
                    0.045f),
                new Color(
                    0.02f,
                    0.13f,
                    0.12f),
                battlefield.transform);
        }

        // Battlefield sectors.
        BuildSector(
            "ALPHA",
            new Vector3(
                -45f,
                0f,
                48f),
            battlefield.transform);

        BuildSector(
            "BRAVO",
            new Vector3(
                45f,
                0f,
                48f),
            battlefield.transform);

        BuildSector(
            "CHARLIE",
            new Vector3(
                -45f,
                0f,
                -12f),
            battlefield.transform);

        BuildSector(
            "DELTA",
            new Vector3(
                45f,
                0f,
                -12f),
            battlefield.transform);

        // Defensive structures.
        BuildFortification(
            new Vector3(
                -25f,
                2f,
                38f),
            battlefield.transform);

        BuildFortification(
            new Vector3(
                25f,
                2f,
                38f),
            battlefield.transform);

        BuildFortification(
            new Vector3(
                -35f,
                2f,
                -10f),
            battlefield.transform);

        BuildFortification(
            new Vector3(
                35f,
                2f,
                -10f),
            battlefield.transform);

        // Roads.
        Cube(
            "[ROAD] NORTH SOUTH",
            new Vector3(
                0f,
                0.08f,
                20f),
            new Vector3(
                8f,
                0.06f,
                120f),
            new Color(
                0.032f,
                0.038f,
                0.038f),
            battlefield.transform);

        Cube(
            "[ROAD] EAST WEST",
            new Vector3(
                0f,
                0.09f,
                20f),
            new Vector3(
                140f,
                0.06f,
                8f),
            new Color(
                0.032f,
                0.038f,
                0.038f),
            battlefield.transform);

        // Command markers.
        Marker(
            "COMMAND",
            new Vector3(
                0f,
                0.4f,
                42f),
            new Color(
                0.2f,
                0.75f,
                0.95f),
            battlefield.transform);

        Marker(
            "OBJECTIVE A",
            new Vector3(
                -40f,
                0.4f,
                60f),
            new Color(
                0.95f,
                0.65f,
                0.2f),
            battlefield.transform);

        Marker(
            "OBJECTIVE B",
            new Vector3(
                40f,
                0.4f,
                60f),
            new Color(
                0.95f,
                0.65f,
                0.2f),
            battlefield.transform);

        Marker(
            "HOSTILE CONTACT",
            new Vector3(
                30f,
                0.4f,
                5f),
            new Color(
                0.95f,
                0.2f,
                0.2f),
            battlefield.transform);

        // World labels.
        Sign(
            "BATTLEFIELD COMMAND",
            new Vector3(
                0f,
                14f,
                78f),
            1.1f,
            new Color(
                0.5f,
                0.88f,
                1f),
            Quaternion.Euler(
                0f,
                180f,
                0f),
            battlefield.transform);

        Sign(
            "SECTOR ALPHA",
            new Vector3(
                -45f,
                2.2f,
                48f),
            0.48f,
            new Color(
                0.3f,
                0.7f,
                0.75f),
            Quaternion.identity,
            battlefield.transform);

        Sign(
            "SECTOR BRAVO",
            new Vector3(
                45f,
                2.2f,
                48f),
            0.48f,
            new Color(
                0.3f,
                0.7f,
                0.75f),
            Quaternion.identity,
            battlefield.transform);
    }

    // ========================================================
    // SECTOR
    // ========================================================

    private static void BuildSector(
        string name,
        Vector3 position,
        Transform parent)
    {
        GameObject sector =
            new GameObject(
                "[SECTOR] " + name);

        sector.transform.SetParent(parent);
        sector.transform.localPosition =
            position;

        Cube(
            "[SECTOR] FOUNDATION",
            Vector3.zero,
            new Vector3(
                24f,
                0.5f,
                20f),
            new Color(
                0.026f,
                0.038f,
                0.035f),
            sector.transform);

        for (
            int x = -9;
            x <= 9;
            x += 9)
        {
            Cube(
                "[SECTOR] BARRIER",
                new Vector3(
                    x,
                    2f,
                    8f),
                new Vector3(
                    0.7f,
                    4f,
                    0.7f),
                new Color(
                    0.045f,
                    0.06f,
                    0.055f),
                sector.transform);
        }

        for (
            int z = -6;
            z <= 6;
            z += 6)
        {
            Cube(
                "[SECTOR] COVER",
                new Vector3(
                    -8f,
                    1f,
                    z),
                new Vector3(
                    3f,
                    2f,
                    1.4f),
                new Color(
                    0.055f,
                    0.065f,
                    0.06f),
                sector.transform);
        }
    }

    // ========================================================
    // FORTIFICATION
    // ========================================================

    private static void BuildFortification(
        Vector3 position,
        Transform parent)
    {
        GameObject fort =
            new GameObject(
                "[FORTIFICATION]");

        fort.transform.SetParent(parent);
        fort.transform.localPosition =
            position;

        Cube(
            "[FORTIFICATION] CORE",
            Vector3.zero,
            new Vector3(
                8f,
                3f,
                6f),
            new Color(
                0.045f,
                0.055f,
                0.052f),
            fort.transform);

        Cube(
            "[FORTIFICATION] TOP",
            new Vector3(
                0f,
                2f,
                0f),
            new Vector3(
                5f,
                0.5f,
                4f),
            new Color(
                0.065f,
                0.075f,
                0.07f),
            fort.transform);

        Cylinder(
            "[FORTIFICATION] SENSOR",
            new Vector3(
                0f,
                4.3f,
                0f),
            new Vector3(
                0.35f,
                1.3f,
                0.35f),
            new Color(
                0.04f,
                0.16f,
                0.17f),
            fort.transform);
    }

    // ========================================================
    // COMMAND CENTER
    // ========================================================

    private static void BuildCommandCenter(
        Transform parent)
    {
        GameObject command =
            new GameObject(
                "[PHYSICAL] FIELD COMMAND CENTER");

        command.transform.SetParent(parent);
        command.transform.localPosition =
            new Vector3(
                0f,
                0f,
                62f);

        Cube(
            "[COMMAND] PLATFORM",
            Vector3.zero,
            new Vector3(
                34f,
                1f,
                18f),
            new Color(
                0.026f,
                0.042f,
                0.05f),
            command.transform);

        Cube(
            "[COMMAND] OPERATIONS WALL",
            new Vector3(
                0f,
                7f,
                8f),
            new Vector3(
                32f,
                14f,
                0.8f),
            new Color(
                0.018f,
                0.032f,
                0.042f),
            command.transform);

        // Command consoles.
        for (
            int x = -12;
            x <= 12;
            x += 6)
        {
            Cube(
                "[COMMAND] CONSOLE",
                new Vector3(
                    x,
                    2.2f,
                    1f),
                new Vector3(
                    4.5f,
                    3f,
                    2f),
                new Color(
                    0.028f,
                    0.055f,
                    0.07f),
                command.transform);

            Cube(
                "[COMMAND] DISPLAY",
                new Vector3(
                    x,
                    4.2f,
                    0f),
                new Vector3(
                    3.8f,
                    1.8f,
                    0.18f),
                new Color(
                    0.018f,
                    0.12f,
                    0.16f),
                command.transform);
        }

        Sign(
            "FIELD COMMAND",
            new Vector3(
                0f,
                11f,
                8f),
            0.75f,
            new Color(
                0.5f,
                0.86f,
                0.96f),
            Quaternion.Euler(
                0f,
                180f,
                0f),
            command.transform);

        Sign(
            "COMMAND INTENT // AUTONOMOUS EXECUTION",
            new Vector3(
                0f,
                9.6f,
                7.3f),
            0.32f,
            new Color(
                0.4f,
                0.65f,
                0.72f),
            Quaternion.Euler(
                0f,
                180f,
                0f),
            command.transform);
    }

    // ========================================================
    // TACTICAL DISPLAYS
    // ========================================================

    private static void BuildTacticalDisplays(
        Transform parent)
    {
        GameObject displays =
            new GameObject(
                "[PHYSICAL] TACTICAL DISPLAY ARRAY");

        displays.transform.SetParent(parent);
        displays.transform.localPosition =
            new Vector3(
                0f,
                0f,
                48f);

        Cube(
            "[TACTICAL DISPLAY] MASTER MAP",
            new Vector3(
                0f,
                8f,
                8f),
            new Vector3(
                28f,
                12f,
                0.5f),
            new Color(
                0.012f,
                0.055f,
                0.07f),
            displays.transform);

        // Map lines.
        for (
            int x = -12;
            x <= 12;
            x += 6)
        {
            Cube(
                "[MAP DISPLAY] VERTICAL",
                new Vector3(
                    x,
                    8f,
                    7.7f),
                new Vector3(
                    0.06f,
                    11f,
                    0.05f),
                new Color(
                    0.08f,
                    0.35f,
                    0.38f),
                displays.transform);
        }

        for (
            int y = 3;
            y <= 13;
            y += 5)
        {
            Cube(
                "[MAP DISPLAY] HORIZONTAL",
                new Vector3(
                    0f,
                    y,
                    7.7f),
                new Vector3(
                    27f,
                    0.06f,
                    0.05f),
                new Color(
                    0.08f,
                    0.35f,
                    0.38f),
                displays.transform);
        }

        // Map contact markers.
        DisplayMarker(
            new Vector3(
                -8f,
                10f,
                7.2f),
            new Color(
                0.2f,
                0.85f,
                0.95f),
            displays.transform);

        DisplayMarker(
            new Vector3(
                7f,
                5f,
                7.2f),
            new Color(
                0.95f,
                0.22f,
                0.22f),
            displays.transform);

        DisplayMarker(
            new Vector3(
                10f,
                11f,
                7.2f),
            new Color(
                0.95f,
                0.65f,
                0.18f),
            displays.transform);

        Sign(
            "LIVE TACTICAL FEED",
            new Vector3(
                0f,
                15.5f,
                7.3f),
            0.48f,
            new Color(
                0.5f,
                0.85f,
                0.92f),
            Quaternion.Euler(
                0f,
                180f,
                0f),
            displays.transform);
    }

    // ========================================================
    // UNIT GROUPS
    // ========================================================

    private static void BuildUnitGroups(
        Transform parent)
    {
        List<SelectableUnit> commandedUnits =
            new List<SelectableUnit>();

        BuildUnitGroup(
            "ALPHA SQUAD",
            new Vector3(
                -22f,
                0f,
                24f),
            4,
            new Color(
                0.25f,
                0.75f,
                0.85f),
            parent,
            commandedUnits);

        BuildUnitGroup(
            "BRAVO SQUAD",
            new Vector3(
                22f,
                0f,
                30f),
            4,
            new Color(
                0.25f,
                0.75f,
                0.85f),
            parent,
            commandedUnits);

        BuildUnitGroup(
            "CONTACT GROUP",
            new Vector3(
                30f,
                0f,
                2f),
            3,
            new Color(
                0.9f,
                0.18f,
                0.18f),
            parent,
            commandedUnits);

        BuildArchive(
            parent,
            commandedUnits);
    }

    private static void BuildUnitGroup(
        string name,
        Vector3 position,
        int count,
        Color color,
        Transform parent,
        List<SelectableUnit> commandedUnits)
    {
        GameObject group =
            new GameObject(
                "[FORCE] " + name);

        group.transform.SetParent(parent);
        group.transform.localPosition =
            position;

        for (
            int i = 0;
            i < count;
            i++)
        {
            float x =
                (i % 2) * 4f;

            float z =
                (i / 2) * 4f;

            GameObject unit =
                Cube(
                    "[UNIT] " + name,
                    new Vector3(
                        x,
                        1f,
                        z),
                    new Vector3(
                        2.4f,
                        1.5f,
                        3f),
                    new Color(
                        0.055f,
                        0.065f,
                        0.065f),
                    group.transform);

            SelectableUnit selectable =
                unit.GetComponent<SelectableUnit>();

            if (selectable == null)
            {
                selectable =
                    unit.AddComponent<SelectableUnit>();
            }

            if (unit.GetComponent<UnitAutonomy>() == null)
            {
                unit.AddComponent<UnitAutonomy>();
            }

            commandedUnits.Add(selectable);

            Cube(
                "[UNIT] STATUS",
                new Vector3(
                    x,
                    1.85f,
                    z + 1.2f),
                new Vector3(
                    1.1f,
                    0.08f,
                    0.12f),
                color,
                group.transform);
        }

        Sign(
            name,
            position +
                new Vector3(
                    2f,
                    3.2f,
                    0f),
            0.32f,
            color,
            Quaternion.identity,
            parent);
    }

    private static void BuildArchive(
        Transform parent,
        List<SelectableUnit> commandedUnits)
    {
        GameObject archive =
            Cube(
                "[COMMAND] ARCHIVE",
                new Vector3(
                    0f,
                    2f,
                    48f),
                new Vector3(
                    5f,
                    4f,
                    5f),
                new Color(
                    0.03f,
                    0.12f,
                    0.16f),
                parent);

        archive.AddComponent<SelectableUnit>();

        archive.AddComponent<CommandUnit>();

        archive.AddComponent<ArchiveCommandBrain>();

        if (archive.GetComponent<UnitAutonomy>() == null)
        {
            archive.AddComponent<UnitAutonomy>();
        }

        CommandUnit commandUnit =
            archive.GetComponent<CommandUnit>();

        foreach (SelectableUnit unit in commandedUnits)
        {
            commandUnit.RegisterCommandedUnit(unit);
        }

        Cube(
            "[COMMAND] ARCHIVE STATUS",
            new Vector3(
                0f,
                4.25f,
                50.55f),
            new Vector3(
                2.2f,
                0.12f,
                0.18f),
            new Color(
                0.25f,
                0.75f,
                0.85f),
            parent);

        Sign(
            "ARCHIVE // COMMAND UNIT",
            new Vector3(
                0f,
                7f,
                48f),
            0.42f,
            new Color(
                0.5f,
                0.86f,
                0.96f),
            Quaternion.identity,
            parent);
    }

    // ========================================================
    // LIGHTING
    // ========================================================

    private static void BuildLighting(
        Transform parent)
    {
        GameObject lights =
            new GameObject(
                "[SYSTEM] BATTLEFIELD LIGHTING");

        lights.transform.SetParent(parent);

        GameObject directional =
            new GameObject(
                "[LIGHT] MOON / COMMAND",
                typeof(Light));

        directional.transform.SetParent(
            lights.transform);

        directional.transform.rotation =
            Quaternion.Euler(
                48f,
                -25f,
                0f);

        Light sun =
            directional.GetComponent<Light>();

        sun.type =
            LightType.Directional;

        sun.intensity =
            0.72f;

        sun.color =
            new Color(
                0.48f,
                0.62f,
                0.72f);

        CreatePointLight(
            lights.transform,
            new Vector3(
                0f,
                8f,
                62f),
            new Color(
                0.15f,
                0.55f,
                0.75f),
            1000f,
            18f);

        CreatePointLight(
            lights.transform,
            new Vector3(
                -45f,
                7f,
                48f),
            new Color(
                0.1f,
                0.45f,
                0.55f),
            500f,
            8f);

        CreatePointLight(
            lights.transform,
            new Vector3(
                45f,
                7f,
                48f),
            new Color(
                0.1f,
                0.45f,
                0.55f),
            500f,
            8f);
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
                "BATTLEFIELD CAMERA",
                typeof(Camera),
                typeof(RTSCameraController),
                typeof(AudioListener));

        Camera camera =
            cameraObject.GetComponent<Camera>();

        camera.clearFlags =
            CameraClearFlags.SolidColor;

        camera.backgroundColor =
            new Color(
                0.006f,
                0.01f,
                0.014f,
                1f);

        camera.fieldOfView =
            65f;

        camera.nearClipPlane =
            0.05f;

        camera.farClipPlane =
            1000f;

        camera.depth =
            -100f;

        camera.tag =
            "MainCamera";

        cameraObject.transform.position =
            new Vector3(
                0f,
                12f,
                -42f);

        LookAt(
            cameraObject.transform,
            new Vector3(
                0f,
                5f,
                35f));
    }

    // ========================================================
    // EVENT SYSTEM
    // ========================================================

    private static void CreateEventSystem()
    {
        EventSystem[] existing =
            UnityEngine.Object.FindObjectsByType<EventSystem>(
                FindObjectsSortMode.None);

        foreach (EventSystem system in existing)
        {
            UnityEngine.Object.DestroyImmediate(
                system.gameObject);
        }

        new GameObject(
            "EVENT SYSTEM",
            typeof(EventSystem),
            typeof(InputSystemUIInputModule));
    }

    // ========================================================
    // COMPLETE HUD
    // ========================================================

    private static void BuildHUD()
    {
        GameObject canvasObject =
            new GameObject(
                "[HUD] BATTLEFIELD HUD",
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

        BuildTopBar(canvasObject.transform);
        BuildLeftCommandPanel(canvasObject.transform);
        BuildCenterTacticalPanel(canvasObject.transform);
        BuildRightInformationPanel(canvasObject.transform);
        BuildBottomCommandBar(canvasObject.transform);
        BuildUnitTacticalDetails(canvasObject.transform);
        BuildPauseMenu(canvasObject.transform);
        BuildCommandIntent(canvasObject.transform);
        BuildObjectivePanel(canvasObject.transform);
        BuildForceOverview(canvasObject.transform);
        BuildFormationManagement(canvasObject.transform);
        BuildDoctrinePanel(canvasObject.transform);
        BuildOrdersPanel(canvasObject.transform);
        BuildContactDetails(canvasObject.transform);
        BuildNetworkPanel(canvasObject.transform);
        BuildSensorPanel(canvasObject.transform);
        BuildLogisticsPanel(canvasObject.transform);
        BuildAlertPanel(canvasObject.transform);
        BuildCameraMenu(canvasObject.transform);
        BuildStatusOverlay(canvasObject.transform);
    }

    // ========================================================
    // TOP BAR
    // ========================================================

    private static void BuildTopBar(
        Transform canvas)
    {
        GameObject panel =
            UIPanel(
                "[PANEL] TOP BAR",
                canvas,
                new Vector2(
                    0.5f,
                    0.955f),
                new Vector2(
                    1840f,
                    68f));

        UILabel(
            "[DISPLAY] MISSION: OPERATION NIGHTFALL",
            panel.transform,
            new Vector2(
                0.14f,
                0.5f),
            new Vector2(
                420f,
                35f),
            17,
            new Color(
                0.5f,
                0.82f,
                0.92f));

        UILabel(
            "[DISPLAY] TIME 02:14:36",
            panel.transform,
            new Vector2(
                0.37f,
                0.5f),
            new Vector2(
                220f,
                35f),
            17,
            new Color(
                0.72f,
                0.82f,
                0.86f));

        UILabel(
            "[DISPLAY] OBJECTIVES 3 / 5",
            panel.transform,
            new Vector2(
                0.53f,
                0.5f),
            new Vector2(
                260f,
                35f),
            17,
            new Color(
                0.55f,
                0.9f,
                0.68f));

        UILabel(
            "[NOTIFICATION] ALERTS 02",
            panel.transform,
            new Vector2(
                0.67f,
                0.5f),
            new Vector2(
                240f,
                35f),
            17,
            new Color(
                0.95f,
                0.65f,
                0.22f));

        UILabel(
            "[DISPLAY] NETWORK: ONLINE",
            panel.transform,
            new Vector2(
                0.82f,
                0.5f),
            new Vector2(
                260f,
                35f),
            16,
            new Color(
                0.35f,
                0.88f,
                0.68f));

        UIButton(
            "[BUTTON] PAUSE",
            panel.transform,
            new Vector2(
                0.95f,
                0.5f),
            new Vector2(
                100f,
                42f),
            "PAUSE",
            new Color(
                0.08f,
                0.06f,
                0.045f));
    }

    // ========================================================
    // LEFT COMMAND
    // ========================================================

    private static void BuildLeftCommandPanel(
        Transform canvas)
    {
        GameObject panel =
            UIPanel(
                "[PANEL] LEFT COMMAND PANEL",
                canvas,
                new Vector2(
                    0.12f,
                    0.53f),
                new Vector2(
                    340f,
                    650f));

        string[] buttons =
        {
            "COMMAND",
            "OBJECTIVES",
            "FORCES",
            "FORMATION",
            "DOCTRINE",
            "ORDERS"
        };

        for (
            int i = 0;
            i < buttons.Length;
            i++)
        {
            UIButton(
                "[BUTTON] " + buttons[i],
                panel.transform,
                new Vector2(
                    0.5f,
                    0.84f -
                    i * 0.12f),
                new Vector2(
                    275f,
                    54f),
                buttons[i],
                new Color(
                    0.025f,
                    0.075f,
                    0.095f));
        }

        UILabel(
            "[DISPLAY] COMMAND INTENT READY",
            panel.transform,
            new Vector2(
                0.5f,
                0.075f),
            new Vector2(
                290f,
                35f),
            14,
            new Color(
                0.35f,
                0.78f,
                0.82f));
    }

    // ========================================================
    // CENTER TACTICAL
    // ========================================================

    private static void BuildCenterTacticalPanel(
        Transform canvas)
    {
        GameObject panel =
            UIPanel(
                "[PANEL] CENTER TACTICAL PANEL",
                canvas,
                new Vector2(
                    0.5f,
                    0.52f),
                new Vector2(
                    760f,
                    650f));

        GameObject map =
            UIPanel(
                "[MAP] TACTICAL MAP",
                panel.transform,
                new Vector2(
                    0.5f,
                    0.56f),
                new Vector2(
                    690f,
                    470f));

        // Tactical map grid.
        for (
            int i = 0;
            i < 7;
            i++)
        {
            GameObject line =
                new GameObject(
                    "[MAP GRID] V",
                    typeof(RectTransform),
                    typeof(Image));

            line.transform.SetParent(
                map.transform);

            RectTransform rect =
                line.GetComponent<RectTransform>();

            rect.anchorMin =
                new Vector2(
                    0.12f +
                    i * 0.12f,
                    0.05f);

            rect.anchorMax =
                new Vector2(
                    0.12f +
                    i * 0.12f,
                    0.95f);

            rect.sizeDelta =
                new Vector2(
                    2f,
                    0f);

            Image image =
                line.GetComponent<Image>();

            image.color =
                new Color(
                    0.05f,
                    0.25f,
                    0.27f,
                    0.7f);
        }

        for (
            int i = 0;
            i < 6;
            i++)
        {
            GameObject line =
                new GameObject(
                    "[MAP GRID] H",
                    typeof(RectTransform),
                    typeof(Image));

            line.transform.SetParent(
                map.transform);

            RectTransform rect =
                line.GetComponent<RectTransform>();

            rect.anchorMin =
                new Vector2(
                    0.05f,
                    0.14f +
                    i * 0.14f);

            rect.anchorMax =
                new Vector2(
                    0.95f,
                    0.14f +
                    i * 0.14f);

            rect.sizeDelta =
                new Vector2(
                    0f,
                    2f);

            Image image =
                line.GetComponent<Image>();

            image.color =
                new Color(
                    0.05f,
                    0.25f,
                    0.27f,
                    0.7f);
        }

        MapMarkerUI(
            map.transform,
            new Vector2(
                0.28f,
                0.7f),
            "A",
            new Color(
                0.3f,
                0.85f,
                0.9f));

        MapMarkerUI(
            map.transform,
            new Vector2(
                0.72f,
                0.64f),
            "B",
            new Color(
                0.95f,
                0.25f,
                0.22f));

        MapMarkerUI(
            map.transform,
            new Vector2(
                0.55f,
                0.34f),
            "C",
            new Color(
                0.95f,
                0.65f,
                0.2f));

        UIButton(
            "[BUTTON] SELECT CONTACT",
            panel.transform,
            new Vector2(
                0.20f,
                0.08f),
            new Vector2(
                190f,
                50f),
            "SELECT CONTACT",
            new Color(
                0.025f,
                0.08f,
                0.10f));

        UIButton(
            "[BUTTON] SELECT UNIT",
            panel.transform,
            new Vector2(
                0.50f,
                0.08f),
            new Vector2(
                170f,
                50f),
            "SELECT UNIT",
            new Color(
                0.025f,
                0.08f,
                0.10f));

        UIButton(
            "[BUTTON] SELECT FORMATION",
            panel.transform,
            new Vector2(
                0.80f,
                0.08f),
            new Vector2(
                210f,
                50f),
            "SELECT FORMATION",
            new Color(
                0.025f,
                0.08f,
                0.10f));
    }

    // ========================================================
    // RIGHT INFORMATION
    // ========================================================

    private static void BuildRightInformationPanel(
        Transform canvas)
    {
        GameObject panel =
            UIPanel(
                "[PANEL] RIGHT INFORMATION PANEL",
                canvas,
                new Vector2(
                    0.88f,
                    0.53f),
                new Vector2(
                    340f,
                    650f));

        string[] buttons =
        {
            "INTELLIGENCE",
            "COMMUNICATIONS",
            "SENSORS",
            "LOGISTICS",
            "ALERTS"
        };

        for (
            int i = 0;
            i < buttons.Length;
            i++)
        {
            UIButton(
                "[BUTTON] " + buttons[i],
                panel.transform,
                new Vector2(
                    0.5f,
                    0.82f -
                    i * 0.13f),
                new Vector2(
                    275f,
                    54f),
                buttons[i],
                new Color(
                    0.025f,
                    0.075f,
                    0.095f));
        }

        UILabel(
            "[DISPLAY] SENSOR COVERAGE 82%",
            panel.transform,
            new Vector2(
                0.5f,
                0.11f),
            new Vector2(
                290f,
                30f),
            14,
            new Color(
                0.35f,
                0.78f,
                0.82f));
    }

    // ========================================================
    // BOTTOM COMMAND BAR
    // ========================================================

    private static void BuildBottomCommandBar(
        Transform canvas)
    {
        GameObject panel =
            UIPanel(
                "[PANEL] BOTTOM COMMAND BAR",
                canvas,
                new Vector2(
                    0.5f,
                    0.075f),
                new Vector2(
                    1100f,
                    100f));

        string[] buttons =
        {
            "MAP",
            "UNITS",
            "COMMAND",
            "OBJECTIVES",
            "CAMERAS",
            "PAUSE"
        };

        for (
            int i = 0;
            i < buttons.Length;
            i++)
        {
            UIButton(
                "[BUTTON] " + buttons[i],
                panel.transform,
                new Vector2(
                    0.083f +
                    i * 0.167f,
                    0.5f),
                new Vector2(
                    140f,
                    52f),
                buttons[i],
                new Color(
                    0.025f,
                    0.08f,
                    0.10f));
        }
    }

    // ========================================================
    // UNIT TACTICAL DETAILS
    // ========================================================

    private static void BuildUnitTacticalDetails(
        Transform canvas)
    {
        GameObject panel =
            UIPanel(
                "[HUD] UNIT TACTICAL DETAILS",
                canvas,
                new Vector2(
                    0.5f,
                    0.50f),
                new Vector2(
                    560f,
                    500f));

        UILabel(
            "UNIT TACTICAL DETAILS",
            panel.transform,
            new Vector2(
                0.5f,
                0.89f),
            new Vector2(
                450f,
                35f),
            22,
            new Color(
                0.55f,
                0.88f,
                0.95f));

        UILabel(
            "WARDEN-01 // AUTONOMOUS RECON",
            panel.transform,
            new Vector2(
                0.5f,
                0.79f),
            new Vector2(
                450f,
                30f),
            17,
            new Color(
                0.65f,
                0.75f,
                0.8f));

        UILabel(
            "STATUS: OPERATIONAL",
            panel.transform,
            new Vector2(
                0.5f,
                0.68f),
            new Vector2(
                400f,
                30f),
            16,
            new Color(
                0.35f,
                0.9f,
                0.65f));

        UILabel(
            "CONDITION  98%",
            panel.transform,
            new Vector2(
                0.5f,
                0.57f),
            new Vector2(
                400f,
                30f),
            16,
            new Color(
                0.55f,
                0.8f,
                0.85f));

        UILabel(
            "OBJECTIVE: HOLD SECTOR ALPHA",
            panel.transform,
            new Vector2(
                0.5f,
                0.46f),
            new Vector2(
                430f,
                30f),
            16,
            new Color(
                0.95f,
                0.7f,
                0.25f));

        UIButton(
            "[BUTTON] CHANGE OBJECTIVE",
            panel.transform,
            new Vector2(
                0.5f,
                0.32f),
            new Vector2(
                300f,
                48f),
            "CHANGE OBJECTIVE",
            new Color(
                0.025f,
                0.08f,
                0.10f));

        UIButton(
            "[BUTTON] WITHDRAW",
            panel.transform,
            new Vector2(
                0.5f,
                0.20f),
            new Vector2(
                260f,
                48f),
            "WITHDRAW",
            new Color(
                0.10f,
                0.055f,
                0.04f));
    }

    // ========================================================
    // COMMAND INTENT
    // ========================================================

    private static void BuildCommandIntent(
        Transform canvas)
    {
        WindowPanel(
            "[WINDOW] COMMAND INTENT",
            canvas,
            "COMMAND INTENT",
            "DEFINE OBJECTIVE / PRIORITY / ROE / AUTONOMY");
    }

    private static void BuildObjectivePanel(
        Transform canvas)
    {
        WindowPanel(
            "[WINDOW] OBJECTIVE PANEL",
            canvas,
            "OBJECTIVES",
            "PRIMARY / SECONDARY / PRIORITY");
    }

    private static void BuildForceOverview(
        Transform canvas)
    {
        WindowPanel(
            "[WINDOW] FORCE OVERVIEW",
            canvas,
            "FORCE OVERVIEW",
            "12 UNITS ACTIVE / 4 RESERVE / 2 DAMAGED");
    }

    private static void BuildFormationManagement(
        Transform canvas)
    {
        WindowPanel(
            "[WINDOW] FORMATION MANAGEMENT",
            canvas,
            "FORMATION MANAGEMENT",
            "LINE / COLUMN / WEDGE / SCREEN");
    }

    private static void BuildDoctrinePanel(
        Transform canvas)
    {
        WindowPanel(
            "[WINDOW] DOCTRINE",
            canvas,
            "DOCTRINE",
            "AGGRESSIVE / BALANCED / CONSERVATIVE");
    }

    private static void BuildOrdersPanel(
        Transform canvas)
    {
        WindowPanel(
            "[WINDOW] OPERATIONAL ORDERS",
            canvas,
            "OPERATIONAL ORDERS",
            "MOVE / HOLD / ATTACK / DEFEND / RECON");
    }

    private static void BuildContactDetails(
        Transform canvas)
    {
        WindowPanel(
            "[WINDOW] CONTACT DETAILS",
            canvas,
            "CONTACT DETAILS",
            "UNKNOWN / SUSPECTED / IDENTIFIED / TRACKED");
    }

    private static void BuildNetworkPanel(
        Transform canvas)
    {
        WindowPanel(
            "[WINDOW] NETWORK HUD",
            canvas,
            "NETWORK",
            "SIGNAL 96% / RELAYS 8 / DATA LINKS 14");
    }

    private static void BuildSensorPanel(
        Transform canvas)
    {
        WindowPanel(
            "[WINDOW] SENSOR COVERAGE",
            canvas,
            "SENSOR COVERAGE",
            "ACTIVE RANGE 12.4 KM / COVERAGE 82%");
    }

    private static void BuildLogisticsPanel(
        Transform canvas)
    {
        WindowPanel(
            "[WINDOW] BATTLEFIELD LOGISTICS",
            canvas,
            "BATTLEFIELD LOGISTICS",
            "FUEL 84% / AMMO 91% / SUPPLY 76%");
    }

    private static void BuildAlertPanel(
        Transform canvas)
    {
        WindowPanel(
            "[WINDOW] ALERT DETAILS",
            canvas,
            "ALERT DETAILS",
            "2 ACTIVE ALERTS / 1 PRIORITY THREAT");
    }

    private static void BuildCameraMenu(
        Transform canvas)
    {
        WindowPanel(
            "[MENU] CAMERA CONTROLS",
            canvas,
            "CAMERA CONTROLS",
            "TACTICAL / COMMAND / UNIT / CONTACT");
    }

    // ========================================================
    // PAUSE MENU
    // ========================================================

    private static void BuildPauseMenu(
        Transform canvas)
    {
        GameObject panel =
            UIPanel(
                "[MENU] PAUSE MENU",
                canvas,
                new Vector2(
                    0.5f,
                    0.5f),
                new Vector2(
                    520f,
                    420f));

        UILabel(
            "PAUSE MENU",
            panel.transform,
            new Vector2(
                0.5f,
                0.84f),
            new Vector2(
                420f,
                40f),
            27,
            new Color(
                0.65f,
                0.88f,
                0.95f));

        UIButton(
            "[BUTTON] RESUME",
            panel.transform,
            new Vector2(
                0.5f,
                0.62f),
            new Vector2(
                330f,
                55f),
            "RESUME",
            new Color(
                0.025f,
                0.09f,
                0.11f));

        UIButton(
            "[BUTTON] SETTINGS",
            panel.transform,
            new Vector2(
                0.5f,
                0.47f),
            new Vector2(
                330f,
                55f),
            "SETTINGS",
            new Color(
                0.025f,
                0.09f,
                0.11f));

        UIButton(
            "[BUTTON] EXIT",
            panel.transform,
            new Vector2(
                0.5f,
                0.32f),
            new Vector2(
                330f,
                55f),
            "EXIT OPERATION",
            new Color(
                0.10f,
                0.045f,
                0.04f));
    }

    // ========================================================
    // STATUS OVERLAY
    // ========================================================

    private static void BuildStatusOverlay(
        Transform canvas)
    {
        GameObject panel =
            UIPanel(
                "[DISPLAY] BATTLEFIELD STATUS",
                canvas,
                new Vector2(
                    0.5f,
                    0.16f),
                new Vector2(
                    700f,
                    50f));

        UILabel(
            "DEPLOYMENT READY // 12 UNITS ASSIGNED // BATTLE BUDGET 10,000 DP",
            panel.transform,
            new Vector2(
                0.5f,
                0.5f),
            new Vector2(
                650f,
                30f),
            15,
            new Color(
                0.42f,
                0.82f,
                0.86f));
    }

    // ========================================================
    // GENERIC WINDOW
    // ========================================================

    private static void WindowPanel(
        string name,
        Transform canvas,
        string title,
        string detail)
    {
        GameObject panel =
            UIPanel(
                name,
                canvas,
                new Vector2(
                    0.5f,
                    0.5f),
                new Vector2(
                    470f,
                    280f));

        panel.SetActive(false);

        UILabel(
            title,
            panel.transform,
            new Vector2(
                0.5f,
                0.72f),
            new Vector2(
                400f,
                40f),
            22,
            new Color(
                0.55f,
                0.88f,
                0.95f));

        UILabel(
            detail,
            panel.transform,
            new Vector2(
                0.5f,
                0.50f),
            new Vector2(
                400f,
                70f),
            16,
            new Color(
                0.62f,
                0.72f,
                0.76f));
    }

    // ========================================================
    // UI MAP MARKER
    // ========================================================

    private static void MapMarkerUI(
        Transform parent,
        Vector2 anchor,
        string text,
        Color color)
    {
        GameObject marker =
            new GameObject(
                "[MAP MARKER] " + text,
                typeof(RectTransform),
                typeof(Image));

        marker.transform.SetParent(parent);

        RectTransform rect =
            marker.GetComponent<RectTransform>();

        rect.anchorMin =
            anchor;

        rect.anchorMax =
            anchor;

        rect.sizeDelta =
            new Vector2(
                36f,
                36f);

        Image image =
            marker.GetComponent<Image>();

        image.color =
            color;

        UILabel(
            text,
            marker.transform,
            new Vector2(
                0.5f,
                0.5f),
            new Vector2(
                32f,
                32f),
            14,
            Color.black);
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

        obj.transform.SetParent(
            parent);

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

        Image image = obj.GetComponent<Image>();

        image.raycastTarget = false;

        image.color =
            new Color(
                0.006f,
                0.022f,
                0.031f,
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
                text,
                typeof(RectTransform),
                typeof(Text));

        obj.transform.SetParent(
            parent);

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

        obj.transform.SetParent(
            parent);

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

        Image image = obj.GetComponent<Image>();

        image.raycastTarget = false;

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
            16;

        text.fontStyle =
            FontStyle.Bold;

        text.color =
            new Color(
                0.72f,
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

        obj.GetComponent<Renderer>()
            .sharedMaterial =
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

        obj.GetComponent<Renderer>()
            .sharedMaterial =
            Material(color);
    }

    // ========================================================
    // 3D MARKER
    // ========================================================

    private static void Marker(
        string name,
        Vector3 position,
        Color color,
        Transform parent)
    {
        Cylinder(
            "[MARKER] " + name,
            position,
            new Vector3(
                1.5f,
                0.12f,
                1.5f),
            color,
            parent);

        Sign(
            name,
            position +
                new Vector3(
                    0f,
                    1.5f,
                    0f),
            0.28f,
            color,
            Quaternion.identity,
            parent);
    }

    // ========================================================
    // DISPLAY MARKER
    // ========================================================

    private static void DisplayMarker(
        Vector3 position,
        Color color,
        Transform parent)
    {
        Cylinder(
            "[DISPLAY MARKER]",
            position,
            new Vector3(
                0.7f,
                0.08f,
                0.7f),
            color,
            parent);
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
                0.75f);
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










