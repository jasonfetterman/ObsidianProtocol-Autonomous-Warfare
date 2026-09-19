using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class UniversalHUDNavigationVisualBuilder
{
    private const string ScenePath =
        "Assets/Scenes/SCN-21 UNIVERSAL HUD NAVIGATION/UniversalHUDNavigation.unity";

    private static readonly Color Black =
        new Color(0.002f, 0.005f, 0.009f, 1f);

    private static readonly Color Floor =
        new Color(0.018f, 0.026f, 0.036f, 1f);

    private static readonly Color Wall =
        new Color(0.042f, 0.058f, 0.072f, 1f);

    private static readonly Color Dark =
        new Color(0.005f, 0.011f, 0.017f, 1f);

    private static readonly Color Panel =
        new Color(0.010f, 0.038f, 0.058f, 1f);

    private static readonly Color Glass =
        new Color(0.015f, 0.105f, 0.140f, 0.72f);

    private static readonly Color Cyan =
        new Color(0.00f, 0.90f, 1.00f, 1f);

    private static readonly Color Green =
        new Color(0.10f, 1.00f, 0.35f, 1f);

    private static readonly Color Amber =
        new Color(1.00f, 0.60f, 0.05f, 1f);

    private static readonly Color Red =
        new Color(1.00f, 0.08f, 0.08f, 1f);

    private static readonly Color White =
        new Color(0.90f, 0.96f, 1.00f, 1f);

    private static Font BuiltinFont;

    private static Canvas HudCanvas;
    private static Text StatusText;
    private static Text ActiveDestination;

    [MenuItem("Obsidian Protocol/Build/SCN-21 UNIVERSAL HUD NAVIGATION - FULL VISUAL")]
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
        CreateUniversalCommandPlatform();
        CreateNavigationWall();
        CreateCommandPortal();
        CreateGaragePortal();
        CreateFleetPortal();
        CreateStorePortal();
        CreateResearchPortal();
        CreateOperationsPortal();
        CreateIntelligencePortal();
        CreateLogisticsPortal();
        CreateDeploymentPortal();
        CreateProfilePortal();
        CreateSettingsPortal();
        CreateNavigationDataCore();
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
        Debug.Log("SCN-21 UNIVERSAL HUD NAVIGATION BUILD COMPLETE");
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
        {
            throw new Exception(
                "EXACT SCENE DOES NOT EXIST: " + absolute);
        }
    }

    private static void ClearScene()
    {
        GameObject[] roots =
            SceneManager.GetActiveScene()
                .GetRootGameObjects();

        foreach (GameObject root in roots)
        {
            UnityEngine.Object.DestroyImmediate(root);
        }
    }

    private static void CreateSystems()
    {
        GameObject root =
            new GameObject("18. UNIVERSAL HUD NAVIGATION");

        GameObject navigation =
            new GameObject(
                "[PANEL] UNIVERSAL NAVIGATION BAR");

        navigation.transform.SetParent(
            root.transform);

        string[] buttons =
        {
            "COMMAND",
            "GARAGE",
            "FLEET",
            "STORE",
            "RESEARCH",
            "OPERATIONS",
            "INTELLIGENCE",
            "LOGISTICS",
            "DEPLOY",
            "PROFILE",
            "SETTINGS"
        };

        foreach (string buttonName in buttons)
        {
            GameObject button =
                new GameObject(
                    "[BUTTON] " + buttonName);

            button.transform.SetParent(
                navigation.transform);

            string destination =
                buttonName == "COMMAND"
                    ? "[HUD] COMMAND CENTER"
                    : buttonName == "GARAGE"
                        ? "[HUD] GARAGE"
                        : buttonName == "FLEET"
                            ? "[HUD] FLEET"
                            : buttonName == "STORE"
                                ? "[HUD] STORE"
                                : buttonName == "RESEARCH"
                                    ? "[HUD] RESEARCH"
                                    : buttonName == "OPERATIONS"
                                        ? "[HUD] OPERATIONS"
                                        : buttonName == "INTELLIGENCE"
                                            ? "[HUD] INTELLIGENCE"
                                            : buttonName == "LOGISTICS"
                                                ? "[HUD] LOGISTICS"
                                                : buttonName == "DEPLOY"
                                                    ? "[HUD] DEPLOYMENT"
                                                    : buttonName == "PROFILE"
                                                        ? "[HUD] COMMANDER PROFILE"
                                                        : "[HUD] SETTINGS";

            GameObject hud =
                new GameObject(destination);

            hud.transform.SetParent(
                button.transform);
        }
    }

    private static void CreateFacility()
    {
        GameObject root =
            new GameObject(
                "[PHYSICAL] UNIVERSAL COMMAND HUB");

        Cube(
            "FLOOR",
            new Vector3(0,-0.5f,0),
            new Vector3(140,1,110),
            Floor,
            root.transform);

        Cube(
            "NORTH WALL",
            new Vector3(0,17,54),
            new Vector3(140,34,1),
            Wall,
            root.transform);

        Cube(
            "WEST WALL",
            new Vector3(-69,17,0),
            new Vector3(1,34,110),
            Wall,
            root.transform);

        Cube(
            "EAST WALL",
            new Vector3(69,17,0),
            new Vector3(1,34,110),
            Wall,
            root.transform);

        Cube(
            "SOUTH WALL LEFT",
            new Vector3(-52,17,-54),
            new Vector3(34,34,1),
            Wall,
            root.transform);

        Cube(
            "SOUTH WALL RIGHT",
            new Vector3(52,17,-54),
            new Vector3(34,34,1),
            Wall,
            root.transform);

        Cube(
            "SOUTH ENTRANCE HEADER",
            new Vector3(0,30,-54),
            new Vector3(70,8,1.5f),
            Dark,
            root.transform);

        Sign(
            "UNIVERSAL HUD NAVIGATION",
            new Vector3(0,25,-53.2f),
            3.1f,
            Cyan,
            Quaternion.identity,
            root.transform);

        Sign(
            "COMMAND NETWORK // GLOBAL SYSTEM NAVIGATION",
            new Vector3(0,21,-53.2f),
            0.95f,
            White,
            Quaternion.identity,
            root.transform);

        for (int x = -60; x <= 60; x += 20)
        {
            Column(
                new Vector3(x,15,44),
                root.transform);

            Column(
                new Vector3(x,15,-44),
                root.transform);

            Cube(
                "CEILING BEAM",
                new Vector3(x,32,0),
                new Vector3(1.5f,1.5f,104),
                Dark,
                root.transform);
        }

        for (int z = -40; z <= 40; z += 20)
        {
            Cube(
                "CROSS CEILING BEAM",
                new Vector3(0,31,z),
                new Vector3(136,1,1),
                Dark,
                root.transform);
        }
    }

    private static void Column(
        Vector3 position,
        Transform parent)
    {
        Cube(
            "STRUCTURAL COLUMN",
            position,
            new Vector3(2,30,2),
            Dark,
            parent);

        Cube(
            "COLUMN DATA LIGHT",
            position +
            new Vector3(1.05f,0,-1.05f),
            new Vector3(0.12f,25,0.12f),
            Cyan,
            parent);
    }

    private static void CreateUniversalCommandPlatform()
    {
        GameObject root =
            new GameObject(
                "[PHYSICAL] UNIVERSAL COMMAND");

        root.transform.position =
            new Vector3(0,0,30);

        Cube(
            "COMMAND PLATFORM",
            new Vector3(0,0.8f,0),
            new Vector3(82,1.6f,24),
            Dark,
            root.transform);

        Cube(
            "COMMAND TABLE",
            new Vector3(0,2.2f,0),
            new Vector3(48,2.2f,12),
            Panel,
            root.transform);

        Cube(
            "COMMAND TABLE GLASS",
            new Vector3(0,3.45f,0),
            new Vector3(43,0.25f,10),
            Glass,
            root.transform);

        Sign(
            "UNIVERSAL COMMAND",
            new Vector3(0,10,5),
            1.8f,
            Cyan,
            Quaternion.Euler(0,180,0),
            root.transform);

        Sign(
            "GLOBAL HUD NAVIGATION // ALL SYSTEMS LINKED",
            new Vector3(0,7.8f,5),
            0.72f,
            White,
            Quaternion.Euler(0,180,0),
            root.transform);

        string[] labels =
        {
            "COMMAND",
            "GARAGE",
            "FLEET",
            "STORE",
            "RESEARCH",
            "OPERATIONS",
            "INTELLIGENCE",
            "LOGISTICS",
            "DEPLOY",
            "PROFILE",
            "SETTINGS"
        };

        for (int i = 0; i < labels.Length; i++)
        {
            float x =
                -35f +
                i * 7f;

            Cube(
                "NAVIGATION CONSOLE",
                new Vector3(x,4.5f,0),
                new Vector3(5.5f,2.5f,4),
                Panel,
                root.transform);

            Cube(
                "NAVIGATION LIGHT",
                new Vector3(x,5.9f,0),
                new Vector3(3.8f,0.10f,2.6f),
                i == 0 ? Cyan : Green,
                root.transform);

            Sign(
                labels[i],
                new Vector3(x,7.2f,2.15f),
                0.65f,
                i == 0 ? Cyan : White,
                Quaternion.Euler(0,180,0),
                root.transform);
        }
    }

    private static void CreateNavigationWall()
    {
        GameObject root =
            new GameObject(
                "[PHYSICAL] UNIVERSAL NAVIGATION WALL");

        root.transform.position =
            new Vector3(0,0,52.5f);

        Cube(
            "NAVIGATION WALL FRAME",
            new Vector3(0,14,0),
            new Vector3(112,27,1.2f),
            Dark,
            root.transform);

        Cube(
            "NAVIGATION WALL DISPLAY",
            new Vector3(0,14,-0.75f),
            new Vector3(108,24,0.3f),
            Panel,
            root.transform);

        Sign(
            "UNIVERSAL HUD NAVIGATION",
            new Vector3(0,23,-1.1f),
            2.1f,
            Cyan,
            Quaternion.Euler(0,180,0),
            root.transform);

        Sign(
            "SELECT DESTINATION",
            new Vector3(0,20.2f,-1.1f),
            0.85f,
            White,
            Quaternion.Euler(0,180,0),
            root.transform);

        string[] labels =
        {
            "COMMAND",
            "GARAGE",
            "FLEET",
            "STORE",
            "RESEARCH",
            "OPERATIONS",
            "INTELLIGENCE",
            "LOGISTICS",
            "DEPLOY",
            "PROFILE",
            "SETTINGS"
        };

        for (int i = 0; i < labels.Length; i++)
        {
            int row =
                i < 6 ? 0 : 1;

            int column =
                i < 6 ? i : i - 6;

            float x =
                -42f +
                column * 16.8f;

            float y =
                row == 0 ? 15f : 7.5f;

            Cube(
                "NAVIGATION WALL BUTTON",
                new Vector3(x,y,-1.3f),
                new Vector3(13,4.8f,0.35f),
                Dark,
                root.transform);

            Cube(
                "NAVIGATION WALL ACTIVE BAR",
                new Vector3(x,y - 2.0f,-1.55f),
                new Vector3(10,0.12f,0.08f),
                i == 0 ? Cyan : Green,
                root.transform);

            Sign(
                labels[i],
                new Vector3(x,y + 0.1f,-1.8f),
                0.90f,
                i == 0 ? Cyan : White,
                Quaternion.Euler(0,180,0),
                root.transform);
        }
    }

    private static void CreateCommandPortal()
    {
        Portal(
            "COMMAND",
            new Vector3(-54,0,28),
            Cyan,
            "COMMAND CENTER",
            "COMMAND // OVERVIEW // FLEET // AI",
            "[HUD] COMMAND CENTER");
    }

    private static void CreateGaragePortal()
    {
        Portal(
            "GARAGE",
            new Vector3(-18,0,15),
            Green,
            "GARAGE",
            "PHYSICAL FACILITY // UNIT CONFIGURATION",
            "[HUD] GARAGE");
    }

    private static void CreateFleetPortal()
    {
        Portal(
            "FLEET",
            new Vector3(18,0,15),
            Cyan,
            "FLEET",
            "FLEET STATUS // UNIT HISTORY // ASSIGNMENT",
            "[HUD] FLEET");
    }

    private static void CreateStorePortal()
    {
        Portal(
            "STORE",
            new Vector3(54,0,28),
            Amber,
            "STORE",
            "PURCHASES // EQUIPMENT // CUSTOMIZATION",
            "[HUD] STORE");
    }

    private static void CreateResearchPortal()
    {
        Portal(
            "RESEARCH",
            new Vector3(-54,0,-5),
            Cyan,
            "RESEARCH",
            "TECHNOLOGY // AI // EXPERIMENTAL",
            "[HUD] RESEARCH");
    }

    private static void CreateOperationsPortal()
    {
        Portal(
            "OPERATIONS",
            new Vector3(-18,0,-12),
            Red,
            "OPERATIONS",
            "BATTLEFIELD // ORDERS // OBJECTIVES",
            "[HUD] OPERATIONS");
    }

    private static void CreateIntelligencePortal()
    {
        Portal(
            "INTELLIGENCE",
            new Vector3(18,0,-12),
            Amber,
            "INTELLIGENCE",
            "CONTACTS // THREATS // SURVEILLANCE",
            "[HUD] INTELLIGENCE");
    }

    private static void CreateLogisticsPortal()
    {
        Portal(
            "LOGISTICS",
            new Vector3(54,0,-5),
            Green,
            "LOGISTICS",
            "RESOURCES // SUPPLY // REPAIR",
            "[HUD] LOGISTICS");
    }

    private static void CreateDeploymentPortal()
    {
        Portal(
            "DEPLOY",
            new Vector3(-42,0,-34),
            Green,
            "DEPLOYMENT",
            "DEPLOYMENT BUDGET // ASSIGNMENT",
            "[HUD] DEPLOYMENT");
    }

    private static void CreateProfilePortal()
    {
        Portal(
            "PROFILE",
            new Vector3(0,0,-40),
            Cyan,
            "COMMANDER PROFILE",
            "IDENTITY // PROGRESSION // HISTORY",
            "[HUD] COMMANDER PROFILE");
    }

    private static void CreateSettingsPortal()
    {
        Portal(
            "SETTINGS",
            new Vector3(42,0,-34),
            Amber,
            "SETTINGS",
            "SYSTEM // AUDIO // DISPLAY // CONTROLS",
            "[HUD] SETTINGS");
    }

    private static void Portal(
        string name,
        Vector3 position,
        Color accent,
        string title,
        string subtitle,
        string destination)
    {
        GameObject root =
            new GameObject(
                "[PORTAL] " + name);

        root.transform.position =
            position;

        Cube(
            "PORTAL PLATFORM",
            new Vector3(0,0.7f,0),
            new Vector3(18,1.4f,13),
            Dark,
            root.transform);

        Cube(
            "PORTAL LEFT",
            new Vector3(-7,8,0),
            new Vector3(1.5f,15,2),
            accent,
            root.transform);

        Cube(
            "PORTAL RIGHT",
            new Vector3(7,8,0),
            new Vector3(1.5f,15,2),
            accent,
            root.transform);

        Cube(
            "PORTAL TOP",
            new Vector3(0,15.3f,0),
            new Vector3(15.5f,1.5f,2),
            accent,
            root.transform);

        Cube(
            "PORTAL INTERFACE",
            new Vector3(0,8,0),
            new Vector3(12.5f,13,0.35f),
            Glass,
            root.transform);

        Sign(
            title,
            new Vector3(0,18.2f,1.3f),
            1.35f,
            accent,
            Quaternion.identity,
            root.transform);

        Sign(
            subtitle,
            new Vector3(0,16.2f,1.3f),
            0.58f,
            White,
            Quaternion.identity,
            root.transform);

        Sign(
            destination,
            new Vector3(0,2.4f,6.7f),
            0.55f,
            accent,
            Quaternion.Euler(0,180,0),
            root.transform);

        for (int i = 0; i < 5; i++)
        {
            Cube(
                "PORTAL DATA",
                new Vector3(
                    -4 + i * 2,
                    7 + Mathf.Sin(i) * 2,
                    -0.3f),
                new Vector3(
                    0.5f,
                    1.0f + i * 0.5f,
                    0.15f),
                accent,
                root.transform);
        }
    }

    private static void CreateNavigationDataCore()
    {
        GameObject root =
            new GameObject(
                "[SYSTEM] NAVIGATION DATA CORE");

        root.transform.position =
            new Vector3(0,0,0);

        Cylinder(
            "CORE BASE",
            new Vector3(0,1,0),
            new Vector3(9,2,9),
            Dark,
            root.transform);

        Cylinder(
            "CORE RING",
            new Vector3(0,2.4f,0),
            new Vector3(7,0.35f,7),
            Cyan,
            root.transform);

        GameObject core =
            GameObject.CreatePrimitive(
                PrimitiveType.Sphere);

        core.name =
            "UNIVERSAL NAVIGATION CORE";

        core.transform.SetParent(
            root.transform);

        core.transform.localPosition =
            new Vector3(0,8,0);

        core.transform.localScale =
            new Vector3(8,8,8);

        core.GetComponent<Renderer>()
            .sharedMaterial =
            Material(Glass);

        for (int i = 0; i < 12; i++)
        {
            float angle =
                i * Mathf.PI * 2f / 12f;

            float x =
                Mathf.Cos(angle) * 6f;

            float z =
                Mathf.Sin(angle) * 6f;

            Cube(
                "CORE DATA BEAM",
                new Vector3(x,8,z),
                new Vector3(0.12f,12,0.12f),
                i % 2 == 0 ? Cyan : Green,
                root.transform);
        }

        Sign(
            "NAVIGATION CORE",
            new Vector3(0,15,0),
            1.1f,
            Cyan,
            Quaternion.identity,
            root.transform);

        Sign(
            "11 SYSTEMS LINKED",
            new Vector3(0,13,0),
            0.65f,
            Green,
            Quaternion.identity,
            root.transform);
    }

    private static void CreateServerBanks()
    {
        GameObject root =
            new GameObject(
                "[PHYSICAL] UNIVERSAL NAVIGATION NETWORK");

        ServerRack(
            new Vector3(-64,0,8),
            Cyan,
            root.transform);

        ServerRack(
            new Vector3(64,0,8),
            Green,
            root.transform);
    }

    private static void ServerRack(
        Vector3 position,
        Color accent,
        Transform parent)
    {
        for (int i = -2; i <= 2; i++)
        {
            Cube(
                "NAVIGATION SERVER",
                position +
                new Vector3(0,5,i * 3),
                new Vector3(5,10,2),
                Dark,
                parent);

            Cube(
                "SERVER STATUS",
                position +
                new Vector3(-2.6f,5,i * 3),
                new Vector3(0.12f,7,0.12f),
                accent,
                parent);
        }
    }

    private static void CreateFloorGrid()
    {
        GameObject root =
            new GameObject(
                "[VISUAL] UNIVERSAL NAVIGATION FLOOR");

        for (int x = -60; x <= 60; x += 10)
        {
            Cube(
                "FLOOR DATA LINE",
                new Vector3(x,0.03f,0),
                new Vector3(0.08f,0.05f,104),
                Cyan,
                root.transform);
        }

        for (int z = -40; z <= 40; z += 10)
        {
            Cube(
                "FLOOR CROSS LINE",
                new Vector3(0,0.04f,z),
                new Vector3(134,0.05f,0.08f),
                Cyan,
                root.transform);
        }

        Cylinder(
            "CENTRAL NAVIGATION PLATFORM",
            new Vector3(0,0.12f,0),
            new Vector3(18,0.24f,18),
            Dark,
            root.transform);

        Sign(
            "OP // UNIVERSAL NAVIGATION",
            new Vector3(0,0.28f,0),
            1.35f,
            Cyan,
            Quaternion.Euler(90,0,0),
            root.transform);
    }

    private static void CreateLighting()
    {
        GameObject root =
            new GameObject(
                "[LIGHTING] UNIVERSAL NAVIGATION");

        GameObject directionalObject =
            new GameObject(
                "MAIN FACILITY LIGHT",
                typeof(Light));

        directionalObject.transform.SetParent(
            root.transform);

        directionalObject.transform.rotation =
            Quaternion.Euler(48,-30,0);

        Light directional =
            directionalObject.GetComponent<Light>();

        directional.type =
            LightType.Directional;

        directional.intensity = 1.25f;

        directional.color =
            new Color(0.72f,0.82f,1f);

        directional.shadows =
            LightShadows.Soft;

        Vector3[] positions =
        {
            new Vector3(-55,26,-35),
            new Vector3(-28,26,-35),
            new Vector3(0,26,-35),
            new Vector3(28,26,-35),
            new Vector3(55,26,-35),
            new Vector3(-55,26,-5),
            new Vector3(-28,26,-5),
            new Vector3(0,26,-5),
            new Vector3(28,26,-5),
            new Vector3(55,26,-5),
            new Vector3(-55,26,25),
            new Vector3(-28,26,25),
            new Vector3(0,26,25),
            new Vector3(28,26,25),
            new Vector3(55,26,25)
        };

        foreach (Vector3 position in positions)
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
            light.intensity = 8;
            light.color = Cyan;
        }
    }

    private static void CreateCamera()
    {
        GameObject obj =
            new GameObject(
                "UNIVERSAL HUD CAMERA",
                typeof(Camera),
                typeof(AudioListener));

        obj.transform.position =
            new Vector3(0,12,-39);

        Vector3 target =
            new Vector3(0,9,26);

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
        camera.farClipPlane = 600f;
        camera.depth = -100;

        camera.tag =
            "MainCamera";

        Debug.Log(
            "UNIVERSAL HUD CAMERA CREATED AT " +
            obj.transform.position);
    }

    private static void CreateEventSystem()
    {
        new GameObject(
            "EVENT SYSTEM",
            typeof(EventSystem),
            typeof(InputSystemUIInputModule));
    }

    private static void CreateHUD()
    {
        GameObject root =
            new GameObject(
                "[HUD] UNIVERSAL NAVIGATION BAR",
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
            new Vector2(0,-34),
            new Vector2(1880,72));

        TextUI(
            "18. UNIVERSAL HUD NAVIGATION",
            root.transform,
            new Vector2(-700,-34),
            new Vector2(700,52),
            27,
            Cyan,
            TextAnchor.MiddleLeft);

        TextUI(
            "GLOBAL COMMAND NETWORK // ONLINE",
            root.transform,
            new Vector2(670,-34),
            new Vector2(500,52),
            18,
            Green,
            TextAnchor.MiddleRight);

        PanelUI(
            "NAVIGATION BAR",
            root.transform,
            new Vector2(0,-100),
            new Vector2(1820,82));

        string[] labels =
        {
            "COMMAND",
            "GARAGE",
            "FLEET",
            "STORE",
            "RESEARCH",
            "OPERATIONS",
            "INTELLIGENCE",
            "LOGISTICS",
            "DEPLOY",
            "PROFILE",
            "SETTINGS"
        };

        for (int i = 0; i < labels.Length; i++)
        {
            float x =
                -800f +
                i * 160f;

            string destination =
                labels[i] == "COMMAND"
                    ? "COMMAND CENTER"
                    : labels[i] == "GARAGE"
                        ? "GARAGE"
                        : labels[i] == "FLEET"
                            ? "FLEET"
                            : labels[i] == "STORE"
                                ? "STORE"
                                : labels[i] == "RESEARCH"
                                    ? "RESEARCH"
                                    : labels[i] == "OPERATIONS"
                                        ? "OPERATIONS"
                                        : labels[i] == "INTELLIGENCE"
                                            ? "INTELLIGENCE"
                                            : labels[i] == "LOGISTICS"
                                                ? "LOGISTICS"
                                                : labels[i] == "DEPLOY"
                                                    ? "DEPLOYMENT"
                                                    : labels[i] == "PROFILE"
                                                        ? "COMMANDER PROFILE"
                                                        : "SETTINGS";

            ButtonUI(
                labels[i],
                root.transform,
                new Vector2(x,-100),
                () =>
                {
                    ActiveDestination.text =
                        "ACTIVE DESTINATION // " +
                        destination;

                    StatusText.text =
                        "NAVIGATION REQUEST // " +
                        destination +
                        " // LINK READY";
                });
        }

        PanelUI(
            "LEFT NAVIGATION",
            root.transform,
            new Vector2(-755,-390),
            new Vector2(400,430));

        TextUI(
            "SYSTEM NAVIGATION",
            root.transform,
            new Vector2(-755,-225),
            new Vector2(350,45),
            23,
            Cyan,
            TextAnchor.MiddleLeft);

        TextUI(
            "COMMAND CENTER\n\nGARAGE\n\nFLEET\n\nSTORE\n\nRESEARCH\n\nOPERATIONS\n\nINTELLIGENCE\n\nLOGISTICS",
            root.transform,
            new Vector2(-755,-420),
            new Vector2(350,340),
            16,
            White,
            TextAnchor.UpperLeft);

        PanelUI(
            "RIGHT NAVIGATION",
            root.transform,
            new Vector2(755,-390),
            new Vector2(400,430));

        TextUI(
            "COMMAND ACCESS",
            root.transform,
            new Vector2(755,-225),
            new Vector2(350,45),
            23,
            Cyan,
            TextAnchor.MiddleLeft);

        TextUI(
            "DEPLOYMENT\n\nCOMMANDER PROFILE\n\nSETTINGS\n\n\nNETWORK STATUS\n\nGLOBAL LINK       ONLINE\n\nSYSTEMS LINKED   11\n\nNAVIGATION        READY\n\nVR SUPPORT       ENABLED",
            root.transform,
            new Vector2(755,-410),
            new Vector2(350,340),
            16,
            White,
            TextAnchor.UpperLeft);

        PanelUI(
            "CENTER STATUS",
            root.transform,
            new Vector2(0,-250),
            new Vector2(620,120));

        ActiveDestination =
            TextUI(
                "ACTIVE DESTINATION // COMMAND CENTER",
                root.transform,
                new Vector2(0,-225),
                new Vector2(570,42),
                20,
                Cyan,
                TextAnchor.MiddleCenter);

        TextUI(
            "UNIVERSAL NAVIGATION // ALL MAJOR SYSTEMS ACCESSIBLE",
            root.transform,
            new Vector2(0,-272),
            new Vector2(570,34),
            13,
            White,
            TextAnchor.MiddleCenter);

        PanelUI(
            "BOTTOM STATUS",
            root.transform,
            new Vector2(0,500),
            new Vector2(1880,58));

        StatusText =
            TextUI(
                "SYSTEM READY // UNIVERSAL HUD NAVIGATION ONLINE",
                root.transform,
                new Vector2(-720,500),
                new Vector2(1020,42),
                16,
                Cyan,
                TextAnchor.MiddleLeft);

        ButtonUI(
            "COMMAND",
            root.transform,
            new Vector2(330,500),
            () =>
            {
                ActiveDestination.text =
                    "ACTIVE DESTINATION // COMMAND CENTER";

                StatusText.text =
                    "COMMAND CENTER // ACCESS READY";
            });

        ButtonUI(
            "GARAGE",
            root.transform,
            new Vector2(470,500),
            () =>
            {
                ActiveDestination.text =
                    "ACTIVE DESTINATION // GARAGE";

                StatusText.text =
                    "GARAGE // ACCESS READY";
            });

        ButtonUI(
            "FLEET",
            root.transform,
            new Vector2(610,500),
            () =>
            {
                ActiveDestination.text =
                    "ACTIVE DESTINATION // FLEET";

                StatusText.text =
                    "FLEET // ACCESS READY";
            });

        ButtonUI(
            "DEPLOY",
            root.transform,
            new Vector2(750,500),
            () =>
            {
                ActiveDestination.text =
                    "ACTIVE DESTINATION // DEPLOYMENT";

                StatusText.text =
                    "DEPLOYMENT // 10,000 DP LIMIT ENFORCED";
            });

        ButtonUI(
            "SETTINGS",
            root.transform,
            new Vector2(890,500),
            () =>
            {
                ActiveDestination.text =
                    "ACTIVE DESTINATION // SETTINGS";

                StatusText.text =
                    "SETTINGS // SYSTEM CONFIGURATION READY";
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

        rt.sizeDelta =
            size;

        rt.anchoredPosition =
            position;

        Image image =
            obj.GetComponent<Image>();

        image.color =
            new Color(
                0.005f,
                0.018f,
                0.030f,
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

        rt.sizeDelta =
            size;

        rt.anchoredPosition =
            position;

        Text component =
            obj.GetComponent<Text>();

        component.text =
            text;

        component.font =
            GetFont();

        component.fontSize =
            fontSize;

        component.color =
            color;

        component.alignment =
            alignment;

        component.horizontalOverflow =
            HorizontalWrapMode.Overflow;

        component.verticalOverflow =
            VerticalWrapMode.Overflow;

        return component;
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
            new Vector2(145,46);

        rt.anchoredPosition =
            position;

        Image image =
            obj.GetComponent<Image>();

        image.color =
            new Color(
                0.012f,
                0.085f,
                0.120f,
                1f);

        Button button =
            obj.GetComponent<Button>();

        button.onClick.AddListener(
            () =>
            {
                action();
            });

        ColorBlock colors =
            button.colors;

        colors.normalColor =
            new Color(
                0.012f,
                0.085f,
                0.120f,
                1f);

        colors.highlightedColor =
            new Color(
                0.02f,
                0.22f,
                0.28f,
                1f);

        colors.pressedColor =
            new Color(
                0.04f,
                0.35f,
                0.40f,
                1f);

        colors.selectedColor =
            colors.highlightedColor;

        button.colors =
            colors;

        TextUI(
            label,
            obj.transform,
            Vector2.zero,
            new Vector2(135,42),
            13,
            White,
            TextAnchor.MiddleCenter);
    }

    
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

    obj.name = name;

    if (parent != null)
    {
        obj.transform.SetParent(parent);
    }

    obj.transform.localPosition = position;
    obj.transform.localScale = scale;

    Renderer renderer =
        obj.GetComponent<Renderer>();

    renderer.sharedMaterial =
        Material(color);
}
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
                0.40f);
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

        mesh.transform.localScale =
            Vector3.one;
    }
}

