using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class VRCommandVisualBuilder
{
    private const string ScenePath =
        "Assets/Scenes/SCN-20 VR COMMAND/VR_Command.unity";

    private static readonly Color Black =
        new Color(0.003f, 0.006f, 0.010f, 1f);

    private static readonly Color Floor =
        new Color(0.018f, 0.026f, 0.036f, 1f);

    private static readonly Color Wall =
        new Color(0.045f, 0.060f, 0.075f, 1f);

    private static readonly Color Dark =
        new Color(0.006f, 0.012f, 0.018f, 1f);

    private static readonly Color Panel =
        new Color(0.012f, 0.040f, 0.060f, 1f);

    private static readonly Color Glass =
        new Color(0.020f, 0.100f, 0.130f, 0.72f);

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

    [MenuItem("Obsidian Protocol/Build/SCN-20 VR COMMAND - FULL VISUAL")]
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
        CreateCommandDeck();
        CreateVRGaragePortal();
        CreateFleetInspection();
        CreateUnitInspection();
        CreateHolographicDisplays();
        CreateTacticalDisplays();
        CreateIntelligenceSystems();
        CreateDeploymentSystems();
        CreateMaintenanceSystems();
        CreateBattlefieldCommand();
        CreateHolographicGlobe();
        CreateVRPods();
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
        Debug.Log("SCN-20 VR COMMAND BUILD COMPLETE");
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
            new GameObject("17. VR COMMAND");

        GameObject hud =
            new GameObject("[HUD] VR COMMAND HUD");

        hud.transform.SetParent(root.transform);

        string[] panels =
        {
            "PHYSICAL GARAGE",
            "COMMAND CENTER",
            "FLEET INSPECTION",
            "UNIT INSPECTION",
            "HOLOGRAPHIC DISPLAYS",
            "TACTICAL DISPLAYS",
            "INTELLIGENCE SYSTEMS",
            "DEPLOYMENT SYSTEMS",
            "MAINTENANCE INTERACTION",
            "FULL VR BATTLEFIELD COMMAND"
        };

        foreach (string panelName in panels)
        {
            GameObject panel =
                new GameObject("[PANEL] " + panelName);

            panel.transform.SetParent(hud.transform);

            GameObject child =
                new GameObject(
                    panelName == "PHYSICAL GARAGE"
                        ? "[PHYSICAL] GARAGE"
                        : panelName == "HOLOGRAPHIC DISPLAYS"
                            ? "[WINDOW] HOLOGRAPHIC DISPLAY"
                            : "[HUD] " + panelName);

            child.transform.SetParent(panel.transform);
        }
    }

    private static void CreateFacility()
    {
        GameObject root =
            new GameObject(
                "[PHYSICAL] VR COMMAND FACILITY");

        Cube(
            "FLOOR",
            new Vector3(0,-0.5f,0),
            new Vector3(120,1,100),
            Floor,
            root.transform);

        Cube(
            "NORTH WALL",
            new Vector3(0,16,49),
            new Vector3(120,32,1),
            Wall,
            root.transform);

        Cube(
            "WEST WALL",
            new Vector3(-59,16,0),
            new Vector3(1,32,100),
            Wall,
            root.transform);

        Cube(
            "EAST WALL",
            new Vector3(59,16,0),
            new Vector3(1,32,100),
            Wall,
            root.transform);

        Cube(
            "SOUTH WALL LEFT",
            new Vector3(-46,16,-49),
            new Vector3(26,32,1),
            Wall,
            root.transform);

        Cube(
            "SOUTH WALL RIGHT",
            new Vector3(46,16,-49),
            new Vector3(26,32,1),
            Wall,
            root.transform);

        Cube(
            "SOUTH ENTRANCE HEADER",
            new Vector3(0,27,-49),
            new Vector3(66,10,1.5f),
            Dark,
            root.transform);

        Sign(
            "VR COMMAND",
            new Vector3(0,24,-48.3f),
            3.2f,
            Cyan,
            Quaternion.identity,
            root.transform);

        Sign(
            "IMMERSIVE AUTONOMOUS WARFARE COMMAND FACILITY",
            new Vector3(0,20,-48.3f),
            0.95f,
            White,
            Quaternion.identity,
            root.transform);

        for (int x = -50; x <= 50; x += 20)
        {
            Cube(
                "CEILING BEAM",
                new Vector3(x,30,0),
                new Vector3(1.5f,1.5f,96),
                Dark,
                root.transform);

            Column(
                new Vector3(x,14,40),
                root.transform);

            Column(
                new Vector3(x,14,-40),
                root.transform);
        }

        for (int z = -40; z <= 40; z += 20)
        {
            Cube(
                "CROSS CEILING BEAM",
                new Vector3(0,29,z),
                new Vector3(116,1,1),
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
            new Vector3(2,28,2),
            Dark,
            parent);

        Cube(
            "COLUMN LIGHT",
            position +
            new Vector3(1.05f,0,-1.05f),
            new Vector3(0.12f,23,0.12f),
            Cyan,
            parent);
    }

    private static void CreateCommandDeck()
    {
        GameObject root =
            new GameObject(
                "[PHYSICAL] COMMAND CENTER");

        root.transform.position =
            new Vector3(0,0,30);

        Cube(
            "COMMAND PLATFORM",
            new Vector3(0,0.8f,0),
            new Vector3(68,1.6f,22),
            Dark,
            root.transform);

        Cube(
            "COMMAND RAIL",
            new Vector3(-33,2.5f,0),
            new Vector3(1,3,20),
            Cyan,
            root.transform);

        Cube(
            "COMMAND RAIL",
            new Vector3(33,2.5f,0),
            new Vector3(1,3,20),
            Cyan,
            root.transform);

        Console(
            "COMMAND CENTER",
            new Vector3(0,2.5f,2),
            new Vector3(20,5,7),
            Cyan,
            root.transform);

        Screen(
            "COMMAND CENTER HUD",
            new Vector3(0,8,5.2f),
            new Vector3(24,9,0.6f),
            Cyan,
            root.transform);

        Sign(
            "COMMAND CENTER",
            new Vector3(0,11.8f,4.5f),
            1.6f,
            Cyan,
            Quaternion.Euler(0,180,0),
            root.transform);

        Sign(
            "COMMAND INTENT // AUTONOMOUS CONTROL",
            new Vector3(0,9.8f,4.5f),
            0.75f,
            White,
            Quaternion.Euler(0,180,0),
            root.transform);

        for (int i = -2; i <= 2; i++)
        {
            Console(
                "COMMAND STATION " + (i + 3),
                new Vector3(i * 11,2.4f,-5),
                new Vector3(9,4.5f,6),
                i == 0 ? Cyan : Green,
                root.transform);
        }
    }

    private static void CreateVRGaragePortal()
    {
        GameObject root =
            new GameObject(
                "[PHYSICAL] GARAGE PORTAL");

        root.transform.position =
            new Vector3(-45,0,28);

        Cube(
            "PORTAL FRAME LEFT",
            new Vector3(-8,8,0),
            new Vector3(2,16,3),
            Cyan,
            root.transform);

        Cube(
            "PORTAL FRAME RIGHT",
            new Vector3(8,8,0),
            new Vector3(2,16,3),
            Cyan,
            root.transform);

        Cube(
            "PORTAL FRAME TOP",
            new Vector3(0,16,0),
            new Vector3(18,2,3),
            Cyan,
            root.transform);

        Cube(
            "GARAGE PORTAL",
            new Vector3(0,8,0),
            new Vector3(15,15,0.5f),
            Glass,
            root.transform);

        Sign(
            "PHYSICAL GARAGE",
            new Vector3(0,18,1.2f),
            1.45f,
            Cyan,
            Quaternion.identity,
            root.transform);

        Sign(
            "ENTER GARAGE // FLEET ACCESS",
            new Vector3(0,15.8f,1.2f),
            0.72f,
            White,
            Quaternion.identity,
            root.transform);
    }

    private static void CreateFleetInspection()
    {
        ConsoleZone(
            "FLEET INSPECTION",
            new Vector3(-28,0,8),
            new Vector3(20,8,13),
            Cyan,
            new string[]
            {
                "FLEET STATUS",
                "ACTIVE UNITS",
                "VETERAN UNITS",
                "DAMAGED UNITS",
                "FLEET VALUE"
            });
    }

    private static void CreateUnitInspection()
    {
        ConsoleZone(
            "UNIT INSPECTION",
            new Vector3(0,0,8),
            new Vector3(20,8,13),
            Green,
            new string[]
            {
                "UNIT IDENTITY",
                "CONDITION",
                "EQUIPMENT",
                "AI PERSONALITY",
                "MISSION HISTORY"
            });
    }

    private static void CreateHolographicDisplays()
    {
        GameObject root =
            new GameObject(
                "[WINDOW] HOLOGRAPHIC DISPLAY");

        root.transform.position =
            new Vector3(28,0,8);

        Cube(
            "HOLO PLATFORM",
            new Vector3(0,0.6f,0),
            new Vector3(20,1.2f,13),
            Dark,
            root.transform);

        for (int i = -2; i <= 2; i++)
        {
            Cube(
                "HOLOGRAPHIC EMITTER",
                new Vector3(i * 3.5f,2,0),
                new Vector3(0.7f,2.5f,0.7f),
                Cyan,
                root.transform);
        }

        Cube(
            "HOLOGRAPHIC SCREEN",
            new Vector3(0,8,0),
            new Vector3(18,11,0.25f),
            Glass,
            root.transform);

        Sign(
            "HOLOGRAPHIC COMMAND DISPLAY",
            new Vector3(0,13.3f,0.8f),
            1.0f,
            Cyan,
            Quaternion.identity,
            root.transform);

        Sign(
            "FLEET // WORLD // COMMAND DATA",
            new Vector3(0,11.3f,0.8f),
            0.65f,
            White,
            Quaternion.identity,
            root.transform);

        for (int i = 0; i < 7; i++)
        {
            Cube(
                "HOLO DATA BAR",
                new Vector3(-7 + i * 2.3f,6,0.5f),
                new Vector3(1.3f,0.15f + (i % 3),0.12f),
                i % 2 == 0 ? Cyan : Green,
                root.transform);
        }
    }

    private static void CreateTacticalDisplays()
    {
        ConsoleZone(
            "BATTLEFIELD TACTICAL DISPLAY",
            new Vector3(-45,0,-8),
            new Vector3(23,8,14),
            Red,
            new string[]
            {
                "BATTLEFIELD",
                "TACTICAL MAP",
                "SELECT UNIT",
                "SELECT FORMATION",
                "ACTIVE CONTACTS"
            });

        GameObject map =
            new GameObject(
                "[HUD] BATTLEFIELD");

        map.transform.position =
            new Vector3(-45,8,-7);

        Cube(
            "TACTICAL SCREEN",
            new Vector3(0,0,0),
            new Vector3(21,11,0.6f),
            Panel,
            map.transform);

        for (int x = -8; x <= 8; x += 4)
        {
            Cube(
                "MAP GRID VERTICAL",
                new Vector3(x,0,-0.35f),
                new Vector3(0.08f,9,0.08f),
                Cyan,
                map.transform);
        }

        for (int y = -4; y <= 4; y += 2)
        {
            Cube(
                "MAP GRID HORIZONTAL",
                new Vector3(0,y,-0.35f),
                new Vector3(17,0.08f,0.08f),
                Cyan,
                map.transform);
        }

        Sign(
            "BATTLEFIELD COMMAND",
            new Vector3(0,6.3f,-0.5f),
            0.95f,
            Red,
            Quaternion.Euler(0,180,0),
            map.transform);

        for (int i = 0; i < 12; i++)
        {
            float x = -7 + (i % 6) * 2.8f;
            float y = -3 + (i / 6) * 5;

            Cube(
                "TACTICAL CONTACT",
                new Vector3(x,y,-0.5f),
                new Vector3(0.5f,0.5f,0.18f),
                i % 3 == 0 ? Red : Cyan,
                map.transform);
        }
    }

    private static void CreateIntelligenceSystems()
    {
        ConsoleZone(
            "INTELLIGENCE SYSTEMS",
            new Vector3(28,0,-10),
            new Vector3(22,8,14),
            Amber,
            new string[]
            {
                "CONTACTS",
                "THREATS",
                "SURVEILLANCE",
                "ANALYSIS",
                "INTELLIGENCE REPORTS"
            });
    }

    private static void CreateDeploymentSystems()
    {
        ConsoleZone(
            "DEPLOYMENT SYSTEMS",
            new Vector3(0,0,-12),
            new Vector3(24,8,14),
            Green,
            new string[]
            {
                "DEPLOYMENT READY",
                "BATTLE BUDGET 10,000 DP",
                "AVAILABLE 10,000 DP",
                "UNIT ASSIGNMENT",
                "COMPETITIVE LIMIT"
            });
    }

    private static void CreateMaintenanceSystems()
    {
        ConsoleZone(
            "MAINTENANCE INTERACTION",
            new Vector3(-28,0,-12),
            new Vector3(22,8,14),
            Amber,
            new string[]
            {
                "DAMAGED UNITS",
                "REPAIR STATIONS",
                "RECOVERY",
                "MAINTENANCE COST",
                "REPAIR QUEUE"
            });
    }

    private static void CreateBattlefieldCommand()
    {
        GameObject root =
            new GameObject(
                "[HUD] BATTLEFIELD COMMAND");

        root.transform.position =
            new Vector3(38,0,-30);

        Cube(
            "COMMAND PLATFORM",
            new Vector3(0,0.8f,0),
            new Vector3(28,1.6f,18),
            Dark,
            root.transform);

        Cube(
            "COMMAND TABLE",
            new Vector3(0,2.3f,0),
            new Vector3(18,2,10),
            Panel,
            root.transform);

        Cube(
            "TACTICAL GLASS",
            new Vector3(0,4.5f,0),
            new Vector3(16,0.25f,8),
            Glass,
            root.transform);

        for (int i = 0; i < 10; i++)
        {
            Cube(
                "TACTICAL MARKER",
                new Vector3(
                    -6 + (i % 5) * 3,
                    4.8f,
                    -3 + (i / 5) * 5),
                new Vector3(0.5f,0.25f,0.5f),
                i % 2 == 0 ? Cyan : Red,
                root.transform);
        }

        Sign(
            "FULL VR BATTLEFIELD COMMAND",
            new Vector3(0,10,5),
            1.15f,
            Cyan,
            Quaternion.Euler(0,180,0),
            root.transform);

        Sign(
            "COMMAND INTENT // FLEET // INTELLIGENCE // DEPLOYMENT",
            new Vector3(0,8,5),
            0.62f,
            White,
            Quaternion.Euler(0,180,0),
            root.transform);
    }

    private static void CreateHolographicGlobe()
    {
        GameObject root =
            new GameObject(
                "[PHYSICAL] STRATEGIC HOLOGRAPHIC GLOBE");

        root.transform.position =
            new Vector3(0,0,-30);

        Cylinder(
            "GLOBE BASE",
            new Vector3(0,1.2f,0),
            new Vector3(5,2.4f,5),
            Dark,
            root.transform);

        GameObject globe =
            GameObject.CreatePrimitive(
                PrimitiveType.Sphere);

        globe.name =
            "STRATEGIC HOLOGRAPHIC WORLD";

        globe.transform.SetParent(
            root.transform);

        globe.transform.localPosition =
            new Vector3(0,8,0);

        globe.transform.localScale =
            new Vector3(10,10,10);

        globe.GetComponent<Renderer>()
            .sharedMaterial =
            Material(Glass);

        for (int i = 0; i < 12; i++)
        {
            float angle =
                i * Mathf.PI * 2f / 12f;

            float x =
                Mathf.Cos(angle) * 5.4f;

            float z =
                Mathf.Sin(angle) * 5.4f;

            Cube(
                "GLOBE DATA BEAM",
                new Vector3(x,8,z),
                new Vector3(0.08f,10,0.08f),
                Cyan,
                root.transform);
        }

        Sign(
            "STRATEGIC WORLD",
            new Vector3(0,15,0),
            1.1f,
            Cyan,
            Quaternion.identity,
            root.transform);

        Sign(
            "LIVE COMMAND NETWORK",
            new Vector3(0,13,0),
            0.65f,
            Green,
            Quaternion.identity,
            root.transform);
    }

    private static void CreateVRPods()
    {
        GameObject root =
            new GameObject(
                "[PHYSICAL] VR COMMAND PODS");

        Vector3[] positions =
        {
            new Vector3(-45,0,-30),
            new Vector3(-35,0,-30),
            new Vector3(-25,0,-30),
            new Vector3(20,0,30),
            new Vector3(30,0,30),
            new Vector3(40,0,30)
        };

        for (int i = 0; i < positions.Length; i++)
        {
            VRPod(
                "VR COMMAND POD " + (i + 1),
                positions[i],
                i % 2 == 0 ? Cyan : Green,
                root.transform);
        }
    }

    private static void VRPod(
        string name,
        Vector3 position,
        Color accent,
        Transform parent)
    {
        GameObject root =
            new GameObject(name);

        root.transform.SetParent(parent);
        root.transform.localPosition = position;

        Cylinder(
            "POD BASE",
            new Vector3(0,0.8f,0),
            new Vector3(4.5f,1.6f,4.5f),
            Dark,
            root.transform);

        Cube(
            "POD CONSOLE",
            new Vector3(0,3,0),
            new Vector3(5,4,2),
            Panel,
            root.transform);

        Cube(
            "POD DISPLAY",
            new Vector3(0,6,1.5f),
            new Vector3(5,3.5f,0.25f),
            Glass,
            root.transform);

        Cube(
            "POD LIGHT LEFT",
            new Vector3(-2.5f,4,0),
            new Vector3(0.12f,5,0.12f),
            accent,
            root.transform);

        Cube(
            "POD LIGHT RIGHT",
            new Vector3(2.5f,4,0),
            new Vector3(0.12f,5,0.12f),
            accent,
            root.transform);

        Sign(
            "VR COMMAND",
            new Vector3(0,8.4f,1.8f),
            0.75f,
            accent,
            Quaternion.identity,
            root.transform);
    }

    private static void CreateServerBanks()
    {
        GameObject root =
            new GameObject(
                "[PHYSICAL] VR NETWORK CORE");

        ServerRack(
            new Vector3(-54,0,8),
            Cyan,
            root.transform);

        ServerRack(
            new Vector3(54,0,8),
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
                "NETWORK SERVER",
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
                "[VISUAL] VR COMMAND FLOOR GRID");

        for (int x = -50; x <= 50; x += 10)
        {
            Cube(
                "FLOOR DATA LINE",
                new Vector3(x,0.03f,0),
                new Vector3(0.08f,0.05f,94),
                Cyan,
                root.transform);
        }

        for (int z = -40; z <= 40; z += 10)
        {
            Cube(
                "FLOOR CROSS LINE",
                new Vector3(0,0.04f,z),
                new Vector3(114,0.05f,0.08f),
                Cyan,
                root.transform);
        }

        Cylinder(
            "CENTRAL VR PLATFORM",
            new Vector3(0,0.12f,0),
            new Vector3(15,0.24f,15),
            Dark,
            root.transform);

        Sign(
            "OP // VR COMMAND",
            new Vector3(0,0.27f,0),
            1.5f,
            Cyan,
            Quaternion.Euler(90,0,0),
            root.transform);
    }

    private static void CreateLighting()
    {
        GameObject root =
            new GameObject(
                "[LIGHTING] VR COMMAND");

        GameObject main =
            new GameObject(
                "MAIN FACILITY LIGHT",
                typeof(Light));

        main.transform.SetParent(root.transform);
        main.transform.rotation =
            Quaternion.Euler(48,-30,0);

        Light directional =
            main.GetComponent<Light>();

        directional.type =
            LightType.Directional;

        directional.intensity = 1.2f;
        directional.color =
            new Color(0.72f,0.82f,1f);

        directional.shadows =
            LightShadows.Soft;

        Vector3[] positions =
        {
            new Vector3(-45,25,-30),
            new Vector3(-20,25,-30),
            new Vector3(0,25,-30),
            new Vector3(25,25,-30),
            new Vector3(48,25,-30),
            new Vector3(-45,25,0),
            new Vector3(-20,25,0),
            new Vector3(0,25,0),
            new Vector3(25,25,0),
            new Vector3(48,25,0),
            new Vector3(-45,25,30),
            new Vector3(-20,25,30),
            new Vector3(0,25,30),
            new Vector3(25,25,30),
            new Vector3(48,25,30)
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

            light.range = 28;
            light.intensity = 8;
            light.color = Cyan;
        }

        Vector3[] redLights =
        {
            new Vector3(-50,7,-10),
            new Vector3(50,7,-10),
            new Vector3(-50,7,15),
            new Vector3(50,7,15)
        };

        foreach (Vector3 position in redLights)
        {
            GameObject lightObject =
                new GameObject(
                    "TACTICAL WARNING LIGHT",
                    typeof(Light));

            lightObject.transform.SetParent(
                root.transform);

            lightObject.transform.position =
                position;

            Light light =
                lightObject.GetComponent<Light>();

            light.type =
                LightType.Point;

            light.range = 18;
            light.intensity = 6;
            light.color = Red;
        }
    }

    private static void CreateCamera()
    {
        GameObject obj =
            new GameObject(
                "VR COMMAND CAMERA",
                typeof(Camera),
                typeof(AudioListener));

        obj.transform.position =
            new Vector3(0,11,-38);

        Vector3 target =
            new Vector3(0,8,24);

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
        camera.farClipPlane = 500f;
        camera.depth = -100;

        camera.tag = "MainCamera";

        Debug.Log(
            "VR COMMAND CAMERA CREATED AT " +
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
                "[HUD] VR COMMAND HUD",
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
            "17. VR COMMAND // IMMERSIVE COMMAND NETWORK",
            root.transform,
            new Vector2(-650,-34),
            new Vector2(900,52),
            26,
            Cyan,
            TextAnchor.MiddleLeft);

        TextUI(
            "VR LINK  ONLINE   //   NETWORK  NOMINAL",
            root.transform,
            new Vector2(650,-34),
            new Vector2(500,52),
            18,
            Green,
            TextAnchor.MiddleRight);

        PanelUI(
            "LEFT COMMAND",
            root.transform,
            new Vector2(-735,-310),
            new Vector2(390,430));

        TextUI(
            "COMMAND CENTER",
            root.transform,
            new Vector2(-735,-135),
            new Vector2(340,45),
            23,
            Cyan,
            TextAnchor.MiddleLeft);

        TextUI(
            "PHYSICAL GARAGE\n\nFLEET INSPECTION\n\nUNIT INSPECTION\n\nHOLOGRAPHIC DISPLAYS\n\nTACTICAL DISPLAYS\n\nINTELLIGENCE SYSTEMS\n\nDEPLOYMENT SYSTEMS",
            root.transform,
            new Vector2(-735,-330),
            new Vector2(340,330),
            16,
            White,
            TextAnchor.UpperLeft);

        PanelUI(
            "RIGHT STATUS",
            root.transform,
            new Vector2(735,-310),
            new Vector2(390,430));

        TextUI(
            "VR COMMAND STATUS",
            root.transform,
            new Vector2(735,-135),
            new Vector2(340,45),
            23,
            Cyan,
            TextAnchor.MiddleLeft);

        TextUI(
            "COMMAND LINK       NOMINAL\n\nFLEET NETWORK       NOMINAL\n\nINTELLIGENCE        ACTIVE\n\nTACTICAL FEEDS      12\n\nDEPLOYMENT READY    12 UNITS\n\nBATTLE BUDGET       10,000 DP\n\nAVAILABLE           10,000 DP\n\nPOWER LIMIT         ENFORCED",
            root.transform,
            new Vector2(735,-330),
            new Vector2(340,330),
            16,
            White,
            TextAnchor.UpperLeft);

        PanelUI(
            "CENTER TARGET",
            root.transform,
            new Vector2(0,-155),
            new Vector2(520,110));

        TextUI(
            "IMMERSIVE COMMAND MODE",
            root.transform,
            new Vector2(0,-130),
            new Vector2(470,38),
            21,
            Cyan,
            TextAnchor.MiddleCenter);

        TextUI(
            "PHYSICAL FACILITY // VR COMMAND ACTIVE",
            root.transform,
            new Vector2(0,-175),
            new Vector2(470,32),
            14,
            White,
            TextAnchor.MiddleCenter);

        PanelUI(
            "BOTTOM BAR",
            root.transform,
            new Vector2(0,500),
            new Vector2(1880,58));

        StatusText =
            TextUI(
                "SYSTEM READY // VR COMMAND NETWORK NOMINAL",
                root.transform,
                new Vector2(-730,500),
                new Vector2(1000,42),
                16,
                Cyan,
                TextAnchor.MiddleLeft);

        ButtonUI(
            "GARAGE",
            root.transform,
            new Vector2(300,500),
            () =>
            {
                StatusText.text =
                    "PHYSICAL GARAGE // FLEET ACCESS REQUESTED";
            });

        ButtonUI(
            "FLEET",
            root.transform,
            new Vector2(450,500),
            () =>
            {
                StatusText.text =
                    "FLEET INSPECTION // UNIT NETWORK OPEN";
            });

        ButtonUI(
            "INTEL",
            root.transform,
            new Vector2(600,500),
            () =>
            {
                StatusText.text =
                    "INTELLIGENCE SYSTEMS // LIVE DATA LINK";
            });

        ButtonUI(
            "DEPLOY",
            root.transform,
            new Vector2(750,500),
            () =>
            {
                StatusText.text =
                    "DEPLOYMENT SYSTEM // 10,000 DP LIMIT ENFORCED";
            });

        ButtonUI(
            "COMMAND",
            root.transform,
            new Vector2(900,500),
            () =>
            {
                StatusText.text =
                    "BATTLEFIELD COMMAND // IMMERSIVE CONTROL ACTIVE";
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
                0.006f,
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

        rt.sizeDelta = size;
        rt.anchoredPosition = position;

        Text textComponent =
            obj.GetComponent<Text>();

        textComponent.text = text;
        textComponent.font = GetFont();
        textComponent.fontSize = fontSize;
        textComponent.color = color;
        textComponent.alignment = alignment;
        textComponent.horizontalOverflow =
            HorizontalWrapMode.Overflow;
        textComponent.verticalOverflow =
            VerticalWrapMode.Overflow;

        return textComponent;
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
            new Vector2(125,42);

        rt.anchoredPosition =
            position;

        Image image =
            obj.GetComponent<Image>();

        image.color =
            new Color(
                0.015f,
                0.10f,
                0.14f,
                1f);

        Button button =
            obj.GetComponent<Button>();

        button.onClick.AddListener(
            () => action());

        TextUI(
            label,
            obj.transform,
            Vector2.zero,
            new Vector2(115,38),
            13,
            White,
            TextAnchor.MiddleCenter);
    }

    private static void ConsoleZone(
        string name,
        Vector3 position,
        Vector3 size,
        Color accent,
        string[] data)
    {
        GameObject root =
            new GameObject(
                "[ZONE] " + name);

        root.transform.position =
            position;

        Cube(
            "ZONE PLATFORM",
            new Vector3(0,0.7f,0),
            new Vector3(size.x,1.4f,size.z),
            Dark,
            root.transform);

        Cube(
            "COMMAND CONSOLE",
            new Vector3(0,2.5f,-size.z * 0.25f),
            new Vector3(size.x * 0.70f,3.5f,2.2f),
            Panel,
            root.transform);

        Screen(
            "ZONE DISPLAY",
            new Vector3(0,7,0.7f),
            new Vector3(size.x * 0.82f,4.8f,0.5f),
            accent,
            root.transform);

        Sign(
            name,
            new Vector3(0,10.2f,0.1f),
            Mathf.Clamp(size.x / 10f,0.85f,1.5f),
            accent,
            Quaternion.Euler(0,180,0),
            root.transform);

        for (int i = 0; i < data.Length; i++)
        {
            float x =
                -size.x * 0.30f +
                (i % 3) * size.x * 0.30f;

            float z =
                -size.z * 0.40f +
                (i / 3) * 2.5f;

            Cube(
                "DATA TERMINAL",
                new Vector3(x,4,z),
                new Vector3(3.8f,1.8f,1.2f),
                Panel,
                root.transform);
        }

        Cube(
            "ZONE ACCENT",
            new Vector3(0,0.15f,size.z * 0.44f),
            new Vector3(size.x * 0.75f,0.12f,0.12f),
            accent,
            root.transform);
    }

    private static void Console(
        string name,
        Vector3 position,
        Vector3 size,
        Color accent,
        Transform parent)
    {
        GameObject root =
            new GameObject(
                "[CONSOLE] " + name);

        root.transform.SetParent(parent);
        root.transform.localPosition =
            position;

        Cube(
            "CONSOLE BODY",
            Vector3.zero,
            size,
            Panel,
            root.transform);

        Cube(
            "CONSOLE TOP",
            new Vector3(0,size.y * 0.53f,0),
            new Vector3(size.x * 0.90f,0.25f,size.z * 0.90f),
            Dark,
            root.transform);

        Cube(
            "CONSOLE LIGHT",
            new Vector3(0,0,-size.z * 0.51f),
            new Vector3(size.x * 0.75f,0.10f,0.10f),
            accent,
            root.transform);
    }

    private static void Screen(
        string name,
        Vector3 position,
        Vector3 size,
        Color accent,
        Transform parent)
    {
        GameObject root =
            new GameObject(
                "[SCREEN] " + name);

        root.transform.SetParent(parent);
        root.transform.localPosition =
            position;

        Cube(
            "SCREEN FRAME",
            Vector3.zero,
            size,
            Dark,
            root.transform);

        Cube(
            "SCREEN SURFACE",
            new Vector3(0,0,-size.z * 0.55f),
            new Vector3(
                size.x * 0.92f,
                size.y * 0.88f,
                0.12f),
            Panel,
            root.transform);

        Cube(
            "SCREEN ACCENT",
            new Vector3(
                0,
                -size.y * 0.44f,
                -size.z * 0.62f),
            new Vector3(
                size.x * 0.75f,
                0.10f,
                0.08f),
            accent,
            root.transform);
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
            obj.transform.SetParent(parent);

        obj.transform.localPosition =
            position;

        obj.transform.localScale =
            scale;

        obj.GetComponent<Renderer>()
            .sharedMaterial =
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

        obj.name = name;

        if (parent != null)
            obj.transform.SetParent(parent);

        obj.transform.localPosition =
            position;

        obj.transform.localScale =
            scale;

        obj.GetComponent<Renderer>()
            .sharedMaterial =
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
        {
            material.SetColor(
                "_BaseColor",
                color);
        }

        if (material.HasProperty("_Metallic"))
        {
            material.SetFloat(
                "_Metallic",
                0.40f);
        }

        if (material.HasProperty("_Smoothness"))
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
            obj.transform.SetParent(parent);

        obj.transform.position =
            position;

        /*
         * IMPORTANT:
         * TextMesh readable side faces the local +Z direction.
         * The command-room signs that face the camera use identity.
         * The screens on the far side use 180 degrees because their
         * readable face points back toward the camera.
         *
         * Never flip the text itself with a negative scale.
         */

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

        mesh.transform.localScale =
            Vector3.one;
    }
}
