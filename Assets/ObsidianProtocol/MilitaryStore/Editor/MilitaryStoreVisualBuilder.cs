using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#else
using UnityEngine.EventSystems;
#endif

public class OPAWMilitaryStoreVisualBuilder
{
    private const string SCENE =
        "Assets/Scenes/SCN-16 MILITARY STORE/Store.unity";

    private Transform world;
    private Transform hud;
    private Camera storeCamera;
    private Font font;

    private Material floorMat;
    private Material wallMat;
    private Material darkMat;
    private Material metalMat;
    private Material cyanMat;
    private Material yellowMat;
    private Material redMat;
    private Material greenMat;
    private Material glassMat;
    private Material hologramMat;

    private readonly List<CatalogItem> catalog =
        new List<CatalogItem>();

    private readonly Dictionary<string, GameObject> windows =
        new Dictionary<string, GameObject>();

    private Color bg =
        new Color(0.008f, 0.014f, 0.021f, 1f);

    private Color panel =
        new Color(0.025f, 0.042f, 0.055f, 0.97f);

    private Color panel2 =
        new Color(0.045f, 0.070f, 0.085f, 1f);

    private Color cyan =
        new Color(0.12f, 0.78f, 0.88f, 1f);

    private Color cyanDark =
        new Color(0.035f, 0.25f, 0.31f, 1f);

    private Color white =
        new Color(0.86f, 0.93f, 0.95f, 1f);

    private Color muted =
        new Color(0.45f, 0.56f, 0.61f, 1f);

    private Color yellow =
        new Color(0.95f, 0.72f, 0.22f, 1f);

    private Color green =
        new Color(0.20f, 0.82f, 0.48f, 1f);

    private Color red =
        new Color(0.85f, 0.18f, 0.18f, 1f);

    [Serializable]
    private class CatalogItem
    {
        public string Category;
        public string Name;
        public int Cost;

        public CatalogItem(
            string category,
            string name,
            int cost)
        {
            Category = category;
            Name = name;
            Cost = cost;
        }
    }

#if UNITY_EDITOR

    [MenuItem("Obsidian Protocol/Build/MILITARY STORE - GARAGE")]
    public static void BuildMilitaryStore()
    {
        Scene scene =
            EditorSceneManager.OpenScene(
                SCENE,
                OpenSceneMode.Single
            );

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            UnityEngine.Object.DestroyImmediate(root);
        }

        OPAWMilitaryStoreVisualBuilder b =
            new OPAWMilitaryStoreVisualBuilder();

        b.Build();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "OBSIDIAN PROTOCOL â€” FULL MILITARY STORE BUILD COMPLETE"
        );
    }

#endif

    private void Build()
    {
        Debug.Log("MILITARY STORE BUILD: START - SCN-04 GARAGE / GARAGE.unity");

        try
        {
            font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            if (font == null)
            {
                Debug.LogWarning(
                    "MILITARY STORE: LegacyRuntime.ttf was not found. UI will use Unity defaults."
                );
            }

            Debug.Log("MILITARY STORE BUILD: CATALOG");
            BuildCatalog();

            Debug.Log("MILITARY STORE BUILD: MATERIALS");
            BuildMaterials();

            Debug.Log("MILITARY STORE BUILD: WORLD");
            BuildWorld();

            Debug.Log("MILITARY STORE BUILD: CAMERA");
            BuildCamera();

            Debug.Log("MILITARY STORE BUILD: EVENT SYSTEM");
            BuildEventSystem();

            Debug.Log("MILITARY STORE BUILD: HUD");
            BuildHUD();

            Debug.Log("MILITARY STORE BUILD: COMPLETE - EXISTING GARAGE SCENE SAVED");
        }
        catch (Exception ex)
        {
            Debug.LogError(
                "MILITARY STORE BUILD FAILED:\n" +
                ex
            );

            throw;
        }
    }

    // =========================================================
    // MATERIALS
    // =========================================================

    private void BuildMaterials()
    {
        floorMat =
            CreateMaterial(
                "MAT_STORE_FLOOR",
                new Color(0.025f, 0.032f, 0.037f)
            );

        wallMat =
            CreateMaterial(
                "MAT_STORE_WALL",
                new Color(0.055f, 0.070f, 0.078f)
            );

        darkMat =
            CreateMaterial(
                "MAT_STORE_DARK",
                new Color(0.012f, 0.018f, 0.022f)
            );

        metalMat =
            CreateMaterial(
                "MAT_STORE_METAL",
                new Color(0.12f, 0.14f, 0.15f)
            );

        cyanMat =
            CreateMaterial(
                "MAT_STORE_CYAN",
                cyan
            );

        yellowMat =
            CreateMaterial(
                "MAT_STORE_YELLOW",
                yellow
            );

        redMat =
            CreateMaterial(
                "MAT_STORE_RED",
                red
            );

        greenMat =
            CreateMaterial(
                "MAT_STORE_GREEN",
                green
            );

        glassMat =
            CreateMaterial(
                "MAT_STORE_GLASS",
                new Color(0.08f, 0.22f, 0.25f)
            );

        hologramMat =
            CreateMaterial(
                "MAT_STORE_HOLOGRAM",
                new Color(0.05f, 0.65f, 0.78f)
            );

        hologramMat.SetFloat(
            "_Surface",
            1f
        );

        hologramMat.SetFloat(
            "_AlphaClip",
            0f
        );

        hologramMat.color =
            new Color(
                0.05f,
                0.65f,
                0.78f,
                0.55f
            );
    }

    private Material CreateMaterial(
        string name,
        Color color)
    {
        Material m =
            new Material(
                Shader.Find(
                    "Universal Render Pipeline/Lit"
                )
            );

        m.name = name;
        m.color = color;

        return m;
    }

    // =========================================================
    // WORLD
    // =========================================================

    private void BuildWorld()
    {
        GameObject root =
            new GameObject(
                "MILITARY STORE FACILITY"
            );

        world = root.transform;

        BuildFloor();
        BuildWalls();
        BuildCeiling();
        BuildEntry();
        BuildProcurementFloor();
        BuildUnitShowroom();
        BuildEquipmentSection();
        BuildCustomizationSection();
        BuildConvenienceSection();
        BuildFacilitySection();
        BuildCommandTerminal();
        BuildHolographicSigns();
        BuildLighting();
        BuildDecorations();
    }

    private void BuildFloor()
    {
        Cube(
            "STORE FLOOR",
            new Vector3(0, -0.25f, 0),
            new Vector3(80, 0.5f, 60),
            floorMat
        );

        for (int x = -36; x <= 36; x += 4)
        {
            Cube(
                "FLOOR GRID X",
                new Vector3(x, 0.01f, 0),
                new Vector3(0.035f, 0.015f, 58),
                darkMat
            );
        }

        for (int z = -26; z <= 26; z += 4)
        {
            Cube(
                "FLOOR GRID Z",
                new Vector3(0, 0.012f, z),
                new Vector3(78, 0.015f, 0.035f),
                darkMat
            );
        }
    }

    private void BuildWalls()
    {
        Cube(
            "NORTH WALL",
            new Vector3(0, 8, 30),
            new Vector3(80, 16, 0.6f),
            wallMat
        );

        Cube(
            "SOUTH WALL",
            new Vector3(0, 8, -30),
            new Vector3(80, 16, 0.6f),
            wallMat
        );

        Cube(
            "WEST WALL",
            new Vector3(-40, 8, 0),
            new Vector3(0.6f, 16, 60),
            wallMat
        );

        Cube(
            "EAST WALL",
            new Vector3(40, 8, 0),
            new Vector3(0.6f, 16, 60),
            wallMat
        );

        // Wall structural columns.
        for (int x = -36; x <= 36; x += 12)
        {
            Cube(
                "NORTH STRUCTURAL COLUMN",
                new Vector3(x, 8, 29.4f),
                new Vector3(0.65f, 16, 0.9f),
                metalMat
            );

            Cube(
                "SOUTH STRUCTURAL COLUMN",
                new Vector3(x, 8, -29.4f),
                new Vector3(0.65f, 16, 0.9f),
                metalMat
            );
        }
    }

    private void BuildCeiling()
    {
        Cube(
            "CEILING",
            new Vector3(0, 16, 0),
            new Vector3(80, 0.5f, 60),
            darkMat
        );

        for (int x = -30; x <= 30; x += 10)
        {
            Cube(
                "CEILING LIGHT",
                new Vector3(x, 15.65f, 0),
                new Vector3(0.3f, 0.08f, 45),
                cyanMat
            );
        }
    }

    private void BuildEntry()
    {
        Cube(
            "ENTRY PLATFORM",
            new Vector3(0, 0.05f, -25),
            new Vector3(16, 0.12f, 5),
            metalMat
        );

        Cube(
            "ENTRY ARCH LEFT",
            new Vector3(-8, 5, -27),
            new Vector3(1, 10, 1),
            metalMat
        );

        Cube(
            "ENTRY ARCH RIGHT",
            new Vector3(8, 5, -27),
            new Vector3(1, 10, 1),
            metalMat
        );

        Cube(
            "ENTRY ARCH TOP",
            new Vector3(0, 10, -27),
            new Vector3(17, 1, 1),
            metalMat
        );

        CreateWorldText(
            "MILITARY STORE",
            new Vector3(0, 8.7f, -26.35f),
            1.2f,
            cyan
        );
    }

    // =========================================================
    // PROCUREMENT FLOOR
    // =========================================================

    private void BuildProcurementFloor()
    {
        BuildKiosk(
            "AIR PROCUREMENT",
            new Vector3(-27, 0, 16),
            cyanMat
        );

        BuildKiosk(
            "GROUND PROCUREMENT",
            new Vector3(-9, 0, 16),
            greenMat
        );

        BuildKiosk(
            "NAVAL PROCUREMENT",
            new Vector3(9, 0, 16),
            cyanMat
        );

        BuildKiosk(
            "COMMAND PROCUREMENT",
            new Vector3(27, 0, 16),
            yellowMat
        );

        BuildKiosk(
            "EXPERIMENTAL",
            new Vector3(-27, 0, 2),
            yellowMat
        );

        BuildKiosk(
            "EQUIPMENT",
            new Vector3(-9, 0, 2),
            cyanMat
        );

        BuildKiosk(
            "CUSTOMIZATION",
            new Vector3(9, 0, 2),
            yellowMat
        );

        BuildKiosk(
            "CONVENIENCE",
            new Vector3(27, 0, 2),
            greenMat
        );
    }

    private void BuildKiosk(
        string label,
        Vector3 position,
        Material accent)
    {
        Cube(
            label + " BASE",
            position + new Vector3(0, 0.65f, 0),
            new Vector3(7, 1.3f, 5),
            darkMat
        );

        Cube(
            label + " BACK",
            position + new Vector3(0, 3.2f, 2),
            new Vector3(7, 5, 0.35f),
            wallMat
        );

        Cube(
            label + " COUNTER",
            position + new Vector3(0, 2, -0.8f),
            new Vector3(6.5f, 0.35f, 2),
            metalMat
        );

        Cube(
            label + " SCREEN",
            position + new Vector3(0, 3.7f, 1.75f),
            new Vector3(5.2f, 2.4f, 0.08f),
            accent
        );

        CreateWorldText(
            label,
            position + new Vector3(0, 5.2f, 1.5f),
            0.48f,
            accent.color
        );

        for (int x = -2; x <= 2; x += 2)
        {
            Cube(
                label + " TERMINAL",
                position +
                new Vector3(x, 2.8f, -1.2f),
                new Vector3(0.75f, 1.1f, 0.55f),
                metalMat
            );

            Sphere(
                label + " TERMINAL LIGHT",
                position +
                new Vector3(x, 3.35f, -1.18f),
                0.12f,
                accent
            );
        }
    }

    // =========================================================
    // UNIT SHOWROOM
    // =========================================================

    private void BuildUnitShowroom()
    {
        CreateWorldText(
            "UNIT SHOWROOM",
            new Vector3(0, 8.2f, 27.2f),
            0.95f,
            cyan
        );

        BuildDisplayBay(
            "WARDEN DISPLAY",
            new Vector3(-27, 0, 24),
            "WARDEN",
            cyanMat,
            true
        );

        BuildDisplayBay(
            "BULLDOG DISPLAY",
            new Vector3(-9, 0, 24),
            "BULLDOG",
            greenMat,
            false
        );

        BuildDisplayBay(
            "TIDEBREAKER DISPLAY",
            new Vector3(9, 0, 24),
            "TIDEBREAKER",
            cyanMat,
            false
        );

        BuildDisplayBay(
            "COMMAND CORE DISPLAY",
            new Vector3(27, 0, 24),
            "COMMAND CORE",
            yellowMat,
            false
        );
    }

    private void BuildDisplayBay(
        string name,
        Vector3 position,
        string unitName,
        Material accent,
        bool drone)
    {
        Cube(
            name + " PLATFORM",
            position + new Vector3(0, 0.3f, 0),
            new Vector3(10, 0.6f, 7),
            metalMat
        );

        Cube(
            name + " BACK",
            position + new Vector3(0, 4, 3),
            new Vector3(10, 8, 0.25f),
            wallMat
        );

        Cube(
            name + " LEFT",
            position + new Vector3(-4.8f, 4, 0),
            new Vector3(0.25f, 8, 6),
            metalMat
        );

        Cube(
            name + " RIGHT",
            position + new Vector3(4.8f, 4, 0),
            new Vector3(0.25f, 8, 6),
            metalMat
        );

        GameObject unit;

        if (drone)
            unit = BuildDrone(
                unitName,
                position +
                new Vector3(0, 3, 0),
                accent
            );
        else
            unit = BuildVehicle(
                unitName,
                position +
                new Vector3(0, 1.7f, 0),
                accent
            );

        unit.transform.SetParent(
            world,
            true
        );

        CreateWorldText(
            unitName,
            position +
            new Vector3(0, 7.4f, 2.7f),
            0.6f,
            accent.color
        );

        CreateWorldText(
            "AVAILABLE FOR PROCUREMENT",
            position +
            new Vector3(0, 0.9f, 2.75f),
            0.25f,
            green
        );

        for (int i = -3; i <= 3; i++)
        {
            Cube(
                name + " LIGHT " + i,
                position +
                new Vector3(i * 1.2f, 0.65f, 2.9f),
                new Vector3(0.7f, 0.06f, 0.08f),
                accent
            );
        }
    }

    private GameObject BuildDrone(
        string name,
        Vector3 position,
        Material accent)
    {
        GameObject root =
            new GameObject(
                name + " 3D MODEL"
            );

        root.transform.position =
            position;

        Cube(
            name + " BODY",
            position,
            new Vector3(2.2f, 0.7f, 2.8f),
            metalMat,
            root.transform
        );

        Cube(
            name + " CORE",
            position +
            new Vector3(0, 0.25f, 0),
            new Vector3(1.3f, 0.35f, 1.5f),
            accent,
            root.transform
        );

        for (int i = 0; i < 4; i++)
        {
            float a =
                i * Mathf.PI * 0.5f;

            Vector3 p =
                position +
                new Vector3(
                    Mathf.Cos(a) * 1.8f,
                    0,
                    Mathf.Sin(a) * 1.8f
                );

            Cube(
                name + " ARM " + i,
                p,
                new Vector3(1.2f, 0.18f, 0.25f),
                metalMat,
                root.transform
            );

            Sphere(
                name + " MOTOR " + i,
                p +
                new Vector3(0, 0.2f, 0),
                0.35f,
                accent,
                root.transform
            );
        }

        Sphere(
            name + " SENSOR",
            position +
            new Vector3(0, -0.25f, 0),
            0.32f,
            glassMat,
            root.transform
        );

        return root;
    }

    private GameObject BuildVehicle(
        string name,
        Vector3 position,
        Material accent)
    {
        GameObject root =
            new GameObject(
                name + " 3D MODEL"
            );

        root.transform.position =
            position;

        Cube(
            name + " CHASSIS",
            position,
            new Vector3(4.2f, 1.3f, 5.5f),
            metalMat,
            root.transform
        );

        Cube(
            name + " UPPER BODY",
            position +
            new Vector3(0, 0.9f, 0.2f),
            new Vector3(3, 1, 3),
            wallMat,
            root.transform
        );

        Cube(
            name + " SENSOR",
            position +
            new Vector3(0, 1.7f, 0.2f),
            new Vector3(1, 0.7f, 1),
            accent,
            root.transform
        );

        for (int side = -1; side <= 1; side += 2)
        {
            for (int z = -2; z <= 2; z += 2)
            {
                Cylinder(
                    name + " WHEEL",
                    position +
                    new Vector3(
                        side * 2.15f,
                        0,
                        z
                    ),
                    0.8f,
                    0.45f,
                    darkMat,
                    root.transform,
                    new Vector3(0, 0, 90)
                );
            }
        }

        Cube(
            name + " FRONT LIGHT",
            position +
            new Vector3(0, 0.2f, -2.8f),
            new Vector3(1.2f, 0.25f, 0.1f),
            accent,
            root.transform
        );

        return root;
    }

    // =========================================================
    // EQUIPMENT / OTHER SECTIONS
    // =========================================================

    private void BuildEquipmentSection()
    {
        BuildRack(
            "SENSOR EQUIPMENT",
            new Vector3(-30, 0, -9),
            cyanMat
        );

        BuildRack(
            "COMMUNICATION EQUIPMENT",
            new Vector3(-18, 0, -9),
            cyanMat
        );

        BuildRack(
            "DEFENSIVE EQUIPMENT",
            new Vector3(-6, 0, -9),
            greenMat
        );
    }

    private void BuildCustomizationSection()
    {
        BuildRack(
            "PAINT",
            new Vector3(7, 0, -9),
            yellowMat
        );

        BuildRack(
            "SKINS",
            new Vector3(19, 0, -9),
            yellowMat
        );

        BuildRack(
            "MARKINGS",
            new Vector3(31, 0, -9),
            yellowMat
        );
    }

    private void BuildConvenienceSection()
    {
        BuildRack(
            "GARAGE SLOTS",
            new Vector3(-30, 0, -19),
            greenMat
        );

        BuildRack(
            "REPAIRS",
            new Vector3(-18, 0, -19),
            greenMat
        );

        BuildRack(
            "FABRICATION",
            new Vector3(-6, 0, -19),
            greenMat
        );

        BuildRack(
            "CAMPAIGN RESOURCES",
            new Vector3(7, 0, -19),
            greenMat
        );

        BuildRack(
            "STORAGE",
            new Vector3(19, 0, -19),
            greenMat
        );

        BuildRack(
            "FLEET MANAGEMENT",
            new Vector3(31, 0, -19),
            yellowMat
        );
    }

    private void BuildFacilitySection()
    {
        CreateWorldText(
            "FACILITY PROCUREMENT",
            new Vector3(0, 6.7f, -27),
            0.7f,
            cyan
        );

        BuildFacilityBay(
            "GARAGE EXPANSION",
            new Vector3(-28, 0, -27),
            cyanMat
        );

        BuildFacilityBay(
            "AIR / DRONE BAY",
            new Vector3(-14, 0, -27),
            cyanMat
        );

        BuildFacilityBay(
            "NAVAL BAY",
            new Vector3(0, 0, -27),
            cyanMat
        );

        BuildFacilityBay(
            "REPAIR STATION",
            new Vector3(14, 0, -27),
            greenMat
        );

        BuildFacilityBay(
            "FABRICATION",
            new Vector3(28, 0, -27),
            yellowMat
        );
    }

    private void BuildFacilityBay(
        string name,
        Vector3 position,
        Material accent)
    {
        Cube(
            name + " FLOOR",
            position +
            new Vector3(0, 0.3f, 0),
            new Vector3(11, 0.6f, 4),
            metalMat
        );

        for (int x = -4; x <= 4; x += 2)
        {
            Cube(
                name + " LIGHT",
                position +
                new Vector3(x, 0.65f, 1.7f),
                new Vector3(1.2f, 0.06f, 0.1f),
                accent
            );
        }

        CreateWorldText(
            name,
            position +
            new Vector3(0, 1.7f, 1.7f),
            0.36f,
            accent.color
        );
    }

    private void BuildRack(
        string name,
        Vector3 position,
        Material accent)
    {
        Cube(
            name + " RACK",
            position +
            new Vector3(0, 2.2f, 0),
            new Vector3(9, 4.4f, 1.4f),
            wallMat
        );

        for (int i = -3; i <= 3; i++)
        {
            Cube(
                name + " ITEM " + i,
                position +
                new Vector3(i * 1.1f, 2.2f, -0.75f),
                new Vector3(0.65f, 1.3f, 0.3f),
                metalMat
            );

            Sphere(
                name + " INDICATOR " + i,
                position +
                new Vector3(i * 1.1f, 3.15f, -0.8f),
                0.08f,
                accent
            );
        }

        CreateWorldText(
            name,
            position +
            new Vector3(0, 4.9f, -0.8f),
            0.34f,
            accent.color
        );
    }

    // =========================================================
    // COMMAND TERMINAL
    // =========================================================

    private void BuildCommandTerminal()
    {
        Vector3 p =
            new Vector3(
                0,
                0,
                9
            );

        Cube(
            "CENTRAL PROCUREMENT DESK",
            p +
            new Vector3(0, 1.2f, 0),
            new Vector3(14, 2.4f, 4),
            darkMat
        );

        Cube(
            "CENTRAL SCREEN",
            p +
            new Vector3(0, 4.5f, 1.6f),
            new Vector3(10, 4, 0.15f),
            cyanMat
        );

        CreateWorldText(
            "PROCUREMENT COMMAND",
            p +
            new Vector3(0, 6.9f, 1.35f),
            0.65f,
            cyan
        );

        CreateWorldText(
            "SELECT ITEM // REVIEW // PURCHASE // CONFIGURE",
            p +
            new Vector3(0, 6.1f, 1.35f),
            0.28f,
            white
        );

        for (int x = -5; x <= 5; x += 2)
        {
            Cube(
                "COMMAND TERMINAL",
                p +
                new Vector3(x, 2.6f, -1),
                new Vector3(1.1f, 1.2f, 0.7f),
                metalMat
            );

            Sphere(
                "COMMAND STATUS",
                p +
                new Vector3(x, 3.25f, -1),
                0.1f,
                greenMat
            );
        }
    }

    // =========================================================
    // SIGNS / LIGHTS / DECOR
    // =========================================================

    private void BuildHolographicSigns()
    {
        string[] signs =
        {
            "UNIT PROCUREMENT",
            "EQUIPMENT",
            "CUSTOMIZATION",
            "CONVENIENCE",
            "FACILITIES",
            "DEPLOYMENT BUDGET",
            "OWNERSHIP â‰  COMBAT POWER"
        };

        Vector3[] positions =
        {
            new Vector3(-27, 10, 29),
            new Vector3(-9, 10, 29),
            new Vector3(9, 10, 29),
            new Vector3(27, 10, 29),
            new Vector3(-22, 9, -29),
            new Vector3(0, 10, -29),
            new Vector3(22, 9, -29)
        };

        for (int i = 0; i < signs.Length; i++)
        {
            CreateWorldText(
                signs[i],
                positions[i],
                0.42f,
                cyan
            );
        }
    }

    private void BuildLighting()
    {
        GameObject lighting =
            new GameObject(
                "STORE LIGHTING"
            );

        Light main =
            lighting.AddComponent<Light>();

        main.type =
            LightType.Directional;

        main.intensity = 0.8f;

        main.transform.rotation =
            Quaternion.Euler(
                55,
                -30,
                0
            );

        for (int x = -30; x <= 30; x += 15)
        {
            GameObject obj =
                new GameObject(
                    "AREA LIGHT"
                );

            obj.transform.position =
                new Vector3(
                    x,
                    12,
                    0
                );

            Light l =
                obj.AddComponent<Light>();

            l.type =
                LightType.Point;

            l.range = 22;
            l.intensity = 3.5f;
            l.color = cyan;
        }
    }

    private void BuildDecorations()
    {
        for (int x = -35; x <= 35; x += 10)
        {
            Cube(
                "WALL LIGHT",
                new Vector3(
                    x,
                    6,
                    29.3f
                ),
                new Vector3(
                    3,
                    0.08f,
                    0.08f
                ),
                cyanMat
            );
        }

        for (int z = -25; z <= 25; z += 10)
        {
            Cube(
                "SIDE LIGHT",
                new Vector3(
                    39.3f,
                    6,
                    z
                ),
                new Vector3(
                    0.08f,
                    3,
                    0.08f
                ),
                cyanMat
            );
        }
    }

    // =========================================================
    // CAMERA
    // =========================================================

    private void BuildCamera()
    {
        GameObject obj =
            new GameObject(
                "MILITARY STORE CAMERA"
            );

        storeCamera =
            obj.AddComponent<Camera>();

        storeCamera.tag =
            "MainCamera";

        storeCamera.enabled = true;

        storeCamera.clearFlags =
            CameraClearFlags.SolidColor;

        storeCamera.backgroundColor =
            bg;

        /*
         * The Military Store is an 80m x 60m facility.
         *
         * The previous camera was only 38m from the store
         * and used a 58 degree FOV, which caused the facility
         * to appear badly framed.
         *
         * Use a controlled perspective camera positioned
         * above the entrance and aimed at the actual center
         * of the facility.
         */

        storeCamera.orthographic = false;

        storeCamera.fieldOfView = 72f;

        storeCamera.nearClipPlane = 0.05f;

        storeCamera.farClipPlane = 500f;

        obj.transform.position =
            new Vector3(
                0f,
                20f,
                -68f
            );

        LookAt(
            obj.transform,
            new Vector3(
                0f,
                5f,
                5f
            )
        );

        Debug.Log(
            "MILITARY STORE CAMERA: CENTERED AND FRAMED"
        );
    }

    // =========================================================
    // EVENT SYSTEM
    // =========================================================

    private void BuildEventSystem()
    {
        GameObject old =
            GameObject.Find(
                "EventSystem"
            );

        if (old != null)
            UnityEngine.Object.DestroyImmediate(old);

        GameObject obj =
            new GameObject(
                "EventSystem"
            );

        obj.AddComponent<
            UnityEngine.EventSystems.EventSystem>();

#if ENABLE_INPUT_SYSTEM
        obj.AddComponent<
            InputSystemUIInputModule>();
#else
        obj.AddComponent<
            StandaloneInputModule>();
#endif
    }

    // =========================================================
    // HUD
    // =========================================================

    private Canvas canvas;

    private void BuildHUD()
    {
        GameObject obj =
            new GameObject(
                "MILITARY STORE HUD"
            );

        hud = obj.transform;

        canvas =
            obj.AddComponent<Canvas>();

        canvas.renderMode =
            RenderMode.ScreenSpaceOverlay;

        canvas.sortingOrder = 100;

        CanvasScaler scaler =
            obj.AddComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(
                1920,
                1080
            );

        scaler.screenMatchMode =
            CanvasScaler.ScreenMatchMode.Expand;

        obj.AddComponent<
            GraphicRaycaster>();

        Image overlay =
            CreateImage(
                "HUD VIGNETTE",
                hud,
                new Color(
                    0,
                    0,
                    0,
                    0.18f
                ),
                Vector2.zero,
                Vector2.one
            );

        BuildHUDTop();
        BuildHUDLeft();
        BuildHUDCenter();
        BuildHUDRight();
        BuildHUDBottom();
        BuildHUDWindows();
    }

    private void BuildHUDTop()
    {
        GameObject top =
            CreatePanel(
                "STORE HEADER",
                hud,
                panel,
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(0, -88),
                new Vector2(0, 0)
            );

        AddText(
            top,
            "TITLE",
            "OBSIDIAN PROTOCOL",
            24,
            cyan,
            TextAnchor.MiddleLeft,
            new Vector2(25, 35),
            new Vector2(400, -8)
        );

        AddText(
            top,
            "SUBTITLE",
            "MILITARY STORE // PROCUREMENT TERMINAL",
            11,
            muted,
            TextAnchor.MiddleLeft,
            new Vector2(27, 7),
            new Vector2(500, 35)
        );

        AddText(
            top,
            "CREDITS",
            "CREDITS  14,000",
            17,
            yellow,
            TextAnchor.MiddleRight,
            new Vector2(-250, 20),
            new Vector2(-25, 58)
        );

        AddText(
            top,
            "DEPLOYMENT",
            "DEPLOYMENT  10,000 / 10,000",
            13,
            green,
            TextAnchor.MiddleRight,
            new Vector2(-520, 0),
            new Vector2(-25, 25)
        );
    }

    private void BuildHUDLeft()
    {
        GameObject left =
            CreatePanel(
                "STORE NAVIGATION",
                hud,
                panel,
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(12, 100),
                new Vector2(275, -100)
            );

        AddText(
            left,
            "TITLE",
            "PROCUREMENT",
            15,
            cyan,
            TextAnchor.UpperLeft,
            new Vector2(16, -18),
            new Vector2(250, 20)
        );

        string[] categories =
        {
            "UNIT STORE",
            "EQUIPMENT",
            "CUSTOMIZATION",
            "CONVENIENCE",
            "FACILITIES",
            "GARAGE SLOTS",
            "MODULES",
            "UPGRADES",
            "GENERIC EQUIPMENT",
            "WEAPONS",
            "COSMETICS",
            "AI PERSONALIZATION",
            "REPAIR",
            "FABRICATION",
            "STORAGE",
            "FLEET MANAGEMENT"
        };

        float y = -48;

        foreach (string category in categories)
        {
            Button b =
                CreateButton(
                    left.transform,
                    category,
                    panel2,
                    white,
                    new Vector2(15, y - 34),
                    new Vector2(-15, y)
                );

            string selected = category;

            b.onClick.AddListener(
                () =>
                {
                    Debug.Log(
                        "STORE CATEGORY: " +
                        selected
                    );
                }
            );

            y -= 39;
        }
    }

    private void BuildHUDCenter()
    {
        GameObject center =
            CreatePanel(
                "STORE CENTER",
                hud,
                new Color(
                    0.01f,
                    0.017f,
                    0.023f,
                    0.82f
                ),
                new Vector2(0, 0),
                new Vector2(1, 1),
                new Vector2(288, 100),
                new Vector2(-635, -100)
            );

        AddText(
            center,
            "TITLE",
            "UNIT PROCUREMENT",
            22,
            white,
            TextAnchor.UpperLeft,
            new Vector2(20, -18),
            new Vector2(500, -50)
        );

        AddText(
            center,
            "DESCRIPTION",
            "SELECT A UNIT TO INSPECT ITS MODEL, SPECIFICATIONS, EQUIPMENT, AI CAPABILITIES AND DEPLOYMENT INFORMATION.",
            11,
            muted,
            TextAnchor.UpperLeft,
            new Vector2(20, -52),
            new Vector2(-20, -90)
        );

        BuildUnitTabs(center.transform);
        BuildUnitCards(center.transform);
    }

    private void BuildUnitTabs(
        Transform parent)
    {
        GameObject tabs =
            CreatePanel(
                "UNIT TABS",
                parent,
                panel,
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(15, -130),
                new Vector2(-15, -88)
            );

        string[] labels =
        {
            "AIR UNITS",
            "GROUND UNITS",
            "NAVAL UNITS",
            "COMMAND UNITS",
            "EXPERIMENTAL"
        };

        float w =
            1f / labels.Length;

        for (int i = 0; i < labels.Length; i++)
        {
            Button b =
                CreateButton(
                    tabs.transform,
                    labels[i],
                    i == 0
                        ? cyanDark
                        : panel2,
                    white,
                    new Vector2(
                        i * w,
                        0
                    ),
                    new Vector2(
                        (i + 1) * w,
                        1
                    ),
                    Vector2.zero,
                    Vector2.zero
                );

            string category =
                labels[i];

            b.onClick.AddListener(
                () =>
                {
                    Debug.Log(
                        "UNIT TAB: " +
                        category
                    );
                }
            );
        }
    }

    private void BuildUnitCards(
        Transform parent)
    {
        GameObject grid =
            CreatePanel(
                "UNIT CARD GRID",
                parent,
                panel,
                new Vector2(0, 0),
                new Vector2(1, 1),
                new Vector2(15, 20),
                new Vector2(-15, -140)
            );

        string[] names =
        {
            "Warden",
            "Beacon",
            "Drop",
            "Iris",
            "Sentinel",
            "ScoutEye",
            "Bulldog",
            "Forge",
            "Hammer",
            "Ironwalker",
            "Surveyor Mk1",
            "Current",
            "Sonar",
            "Depthwatch",
            "Archive",
            "Worldmap",
            "Command Core",
            "Fusion",
            "Echo",
            "Nullpoint",
            "Specter",
            "Shadowgrid"
        };

        int columns = 2;

        for (int i = 0; i < names.Length; i++)
        {
            int col =
                i % columns;

            int row =
                i / columns;

            float w =
                1f / columns;

            float top =
                1f -
                row * 0.108f;

            float bottom =
                top -
                0.098f;

            GameObject card =
                CreatePanel(
                    "CARD " + names[i],
                    grid.transform,
                    panel2,
                    new Vector2(
                        col * w + 0.008f,
                        bottom
                    ),
                    new Vector2(
                        (col + 1) * w - 0.008f,
                        top
                    ),
                    Vector2.zero,
                    Vector2.zero
                );

            CatalogItem item =
                FindItem(names[i]);

            int price =
                item != null
                    ? item.Cost
                    : 0;

            AddText(
                card,
                "NAME",
                names[i],
                13,
                white,
                TextAnchor.MiddleLeft,
                new Vector2(10, 16),
                new Vector2(-125, -16)
            );

            AddText(
                card,
                "PRICE",
                price.ToString("N0") +
                " C",
                12,
                yellow,
                TextAnchor.MiddleRight,
                new Vector2(-120, 16),
                new Vector2(-70, -16)
            );

            Button view =
                CreateButton(
                    card.transform,
                    "INSPECT",
                    cyanDark,
                    white,
                    new Vector2(-66, 7),
                    new Vector2(-10, 29)
                );

            string unit =
                names[i];

            view.onClick.AddListener(
                () =>
                {
                    ShowUnitDetail(unit);
                }
            );
        }
    }

    private void BuildHUDRight()
    {
        GameObject right =
            CreatePanel(
                "DETAIL PANEL",
                hud,
                panel,
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(-620, 100),
                new Vector2(-12, -100)
            );

        AddText(
            right,
            "TITLE",
            "UNIT DETAIL",
            20,
            cyan,
            TextAnchor.UpperLeft,
            new Vector2(18, -18),
            new Vector2(-18, -52)
        );

        GameObject viewer =
            CreatePanel(
                "MODEL VIEWER",
                right.transform,
                new Color(
                    0.006f,
                    0.012f,
                    0.018f,
                    1f
                ),
                new Vector2(0, 0.55f),
                new Vector2(1, 1),
                new Vector2(18, 12),
                new Vector2(-18, -62)
            );

        AddText(
            viewer,
            "MODEL",
            "3D UNIT VIEWER",
            13,
            muted,
            TextAnchor.MiddleCenter,
            new Vector2(0, 45),
            new Vector2(0, 75)
        );

        AddText(
            viewer,
            "MODEL NAME",
            "WARDEN",
            25,
            cyan,
            TextAnchor.MiddleCenter,
            new Vector2(0, -20),
            new Vector2(0, 30)
        );

        AddText(
            viewer,
            "MODEL STATUS",
            "PROCUREMENT READY",
            11,
            green,
            TextAnchor.MiddleCenter,
            new Vector2(0, -65),
            new Vector2(0, -35)
        );

        AddText(
            right,
            "UNIT",
            "WARDEN",
            22,
            white,
            TextAnchor.UpperLeft,
            new Vector2(18, -335),
            new Vector2(-18, -300)
        );

        AddText(
            right,
            "TYPE",
            "AIR // RECONNAISSANCE",
            11,
            muted,
            TextAnchor.UpperLeft,
            new Vector2(18, -365),
            new Vector2(-18, -340)
        );

        AddText(
            right,
            "STATUS",
            "AVAILABLE FOR PURCHASE",
            12,
            green,
            TextAnchor.UpperLeft,
            new Vector2(18, -398),
            new Vector2(-18, -370)
        );

        AddText(
            right,
            "PRICE",
            "750 CREDITS",
            18,
            yellow,
            TextAnchor.UpperLeft,
            new Vector2(18, -430),
            new Vector2(-18, -400)
        );

        string[] actions =
        {
            "VIEW MODEL",
            "SPECIFICATIONS",
            "EQUIPMENT",
            "AI CAPABILITIES",
            "DEPLOYMENT INFO",
            "PURCHASE"
        };

        float y = -455;

        foreach (string action in actions)
        {
            Button b =
                CreateButton(
                    right.transform,
                    action,
                    action == "PURCHASE"
                        ? cyanDark
                        : panel2,
                    white,
                    new Vector2(18, y - 34),
                    new Vector2(-18, y)
                );

            string selected =
                action;

            b.onClick.AddListener(
                () =>
                {
                    if (selected ==
                        "PURCHASE")
                    {
                        ShowWindow(
                            "PURCHASE"
                        );
                    }
                    else
                    {
                        ShowWindow(
                            selected
                        );
                    }
                }
            );

            y -= 39;
        }
    }

    private void BuildHUDBottom()
    {
        GameObject bottom =
            CreatePanel(
                "BOTTOM STATUS",
                hud,
                panel,
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(288, 12),
                new Vector2(-12, 90)
            );

        AddText(
            bottom,
            "RULE",
            "OWNERSHIP â‰  COMBAT POWER",
            13,
            cyan,
            TextAnchor.MiddleLeft,
            new Vector2(15, 5),
            new Vector2(320, 38)
        );

        AddText(
            bottom,
            "RULE2",
            "BATTLEFIELD POWER IS LIMITED BY DEPLOYMENT BUDGET",
            11,
            muted,
            TextAnchor.MiddleCenter,
            new Vector2(320, 5),
            new Vector2(-320, 38)
        );

        AddText(
            bottom,
            "STATUS",
            "STORE ONLINE",
            12,
            green,
            TextAnchor.MiddleRight,
            new Vector2(-180, 5),
            new Vector2(-15, 38)
        );
    }

    private void BuildHUDWindows()
    {
        CreateWindow(
            "VIEW MODEL",
            "3D UNIT VIEWER",
            "Rotate, inspect and review the selected unit before purchase."
        );

        CreateWindow(
            "SPECIFICATIONS",
            "SPECIFICATIONS",
            "Dimensions, mass, mobility, speed, propulsion, sensors and operating characteristics."
        );

        CreateWindow(
            "EQUIPMENT",
            "EQUIPMENT COMPATIBILITY",
            "Compatible sensors, communications, defensive equipment, modules and payload systems."
        );

        CreateWindow(
            "AI CAPABILITIES",
            "AI CAPABILITIES",
            "Autonomy, doctrine, personality, command behavior and AI system compatibility."
        );

        CreateWindow(
            "DEPLOYMENT INFO",
            "DEPLOYMENT INFORMATION",
            "Purchase price represents ownership. Competitive deployment remains restricted by the active Deployment Budget."
        );

        CreatePurchaseWindow();
    }

    private void CreateWindow(
        string id,
        string title,
        string description)
    {
        GameObject window =
            CreatePanel(
                id,
                hud,
                new Color(
                    0.012f,
                    0.022f,
                    0.031f,
                    0.995f
                ),
                new Vector2(
                    0.5f,
                    0.5f
                ),
                new Vector2(
                    0.5f,
                    0.5f
                ),
                new Vector2(
                    -360,
                    -220
                ),
                new Vector2(
                    360,
                    220
                )
            );

        windows[id] =
            window;

        AddText(
            window,
            "TITLE",
            title,
            23,
            cyan,
            TextAnchor.UpperLeft,
            new Vector2(25, -25),
            new Vector2(-25, -65)
        );

        AddText(
            window,
            "DESCRIPTION",
            description,
            14,
            white,
            TextAnchor.UpperLeft,
            new Vector2(25, -90),
            new Vector2(-25, 60)
        );

        AddText(
            window,
            "SYSTEM",
            "OBSIDIAN PROTOCOL // AUTHORIZED TERMINAL",
            10,
            muted,
            TextAnchor.LowerLeft,
            new Vector2(25, 20),
            new Vector2(350, 45)
        );

        Button close =
            CreateButton(
                window.transform,
                "CLOSE",
                panel2,
                white,
                new Vector2(-130, 20),
                new Vector2(-25, 62)
            );

        close.onClick.AddListener(
            () =>
            {
                window.SetActive(false);
            }
        );

        window.SetActive(false);
    }

    private void CreatePurchaseWindow()
    {
        GameObject window =
            CreatePanel(
                "PURCHASE",
                hud,
                new Color(
                    0.012f,
                    0.022f,
                    0.031f,
                    0.995f
                ),
                new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f),
                new Vector2(-370, -230),
                new Vector2(370, 230)
            );

        windows["PURCHASE"] =
            window;

        AddText(
            window,
            "TITLE",
            "PURCHASE CONFIRMATION",
            24,
            cyan,
            TextAnchor.UpperLeft,
            new Vector2(25, -25),
            new Vector2(-25, -65)
        );

        AddText(
            window,
            "UNIT",
            "WARDEN",
            30,
            white,
            TextAnchor.MiddleCenter,
            new Vector2(25, -125),
            new Vector2(-25, -75)
        );

        AddText(
            window,
            "PRICE",
            "750 CREDITS",
            21,
            yellow,
            TextAnchor.MiddleCenter,
            new Vector2(25, -175),
            new Vector2(-25, -130)
        );

        AddText(
            window,
            "BALANCE",
            "BALANCE AFTER PURCHASE: 13,250 CREDITS",
            12,
            muted,
            TextAnchor.MiddleCenter,
            new Vector2(25, -215),
            new Vector2(-25, -185)
        );

        AddText(
            window,
            "RULE",
            "OWNERSHIP DOES NOT INCREASE YOUR COMPETITIVE DEPLOYMENT BUDGET.",
            11,
            cyan,
            TextAnchor.MiddleCenter,
            new Vector2(25, 70),
            new Vector2(-25, 110)
        );

        Button confirm =
            CreateButton(
                window.transform,
                "CONFIRM PURCHASE",
                cyanDark,
                white,
                new Vector2(25, 20),
                new Vector2(220, 62)
            );

        confirm.onClick.AddListener(
            () =>
            {
                window.SetActive(false);
                ShowAfterPurchase();
            }
        );

        Button cancel =
            CreateButton(
                window.transform,
                "CANCEL",
                panel2,
                white,
                new Vector2(235, 20),
                new Vector2(365, 62)
            );

        cancel.onClick.AddListener(
            () =>
            {
                window.SetActive(false);
            }
        );

        window.SetActive(false);
    }

    private void ShowAfterPurchase()
    {
        GameObject window =
            CreatePanel(
                "AFTER PURCHASE",
                hud,
                new Color(
                    0.012f,
                    0.022f,
                    0.031f,
                    0.995f
                ),
                new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f),
                new Vector2(-350, -200),
                new Vector2(350, 200)
            );

        AddText(
            window,
            "TITLE",
            "PROCUREMENT COMPLETE",
            25,
            green,
            TextAnchor.UpperLeft,
            new Vector2(25, -25),
            new Vector2(-25, -65)
        );

        AddText(
            window,
            "MESSAGE",
            "WARDEN has been added to the owned fleet.\n\nThe unit is now available in the Garage for configuration.\n\nCompetitive deployment remains governed by the active Deployment Budget.",
            15,
            white,
            TextAnchor.UpperLeft,
            new Vector2(25, -90),
            new Vector2(-25, 55)
        );

        Button garage =
            CreateButton(
                window.transform,
                "VIEW IN GARAGE",
                cyanDark,
                white,
                new Vector2(25, 20),
                new Vector2(190, 62)
            );

        garage.onClick.AddListener(
            () =>
            {
                Debug.Log(
                    "MILITARY STORE -> GARAGE -> WARDEN"
                );
            }
        );

        Button configure =
            CreateButton(
                window.transform,
                "CONFIGURE",
                panel2,
                white,
                new Vector2(205, 20),
                new Vector2(350, 62)
            );

        configure.onClick.AddListener(
            () =>
            {
                Debug.Log(
                    "OPEN UNIT CONFIGURATION"
                );
            }
        );

        Button close =
            CreateButton(
                window.transform,
                "CLOSE",
                panel2,
                white,
                new Vector2(-120, 20),
                new Vector2(-20, 62)
            );

        close.onClick.AddListener(
            () =>
            {
                UnityEngine.Object.DestroyImmediate(window);
            }
        );
    }

    private void ShowWindow(
        string id)
    {
        foreach (
            GameObject window
            in windows.Values)
        {
            if (window != null)
                window.SetActive(false);
        }

        if (windows.ContainsKey(id))
            windows[id].SetActive(true);
    }

    private void ShowUnitDetail(
        string unit)
    {
        Debug.Log(
            "Selected unit: " +
            unit
        );
    }

    // =========================================================
    // CATALOG
    // =========================================================

    private void BuildCatalog()
    {
        catalog.Clear();

        Add("AIR", "Warden", 750);
        Add("AIR", "Beacon", 500);
        Add("AIR", "Drop", 1200);
        Add("AIR", "Iris", 650);
        Add("AIR", "Sentinel", 1000);
        Add("AIR", "ScoutEye", 450);
        Add("AIR", "Inspect", 400);
        Add("AIR", "Lidar", 700);
        Add("AIR", "Lifeline", 800);
        Add("AIR", "Link", 650);
        Add("AIR", "Locator", 500);
        Add("AIR", "Cartographer", 800);
        Add("AIR", "Mesh", 900);
        Add("AIR", "Nightguard", 950);
        Add("AIR", "Overseer", 1500);
        Add("AIR", "Trailblazer", 850);
        Add("AIR", "Relay", 750);
        Add("AIR", "Response", 850);
        Add("AIR", "SAR", 900);
        Add("AIR", "Scout", 400);
        Add("AIR", "NexusGrid", 1400);
        Add("AIR", "Spotter", 500);
        Add("AIR", "Survey", 550);
        Add("AIR", "Terrain", 650);
        Add("AIR", "Thermal", 700);
        Add("AIR", "Tracker", 650);
        Add("AIR", "Vector", 1100);
        Add("AIR", "Watchtower", 1000);
        Add("AIR", "Horizon", 1050);
        Add("AIR", "Skywatch", 1100);
        Add("AIR", "Insight", 1300);

        Add("GROUND", "Bulldog", 1250);
        Add("GROUND", "Forge", 1400);
        Add("GROUND", "Hammer", 1650);
        Add("GROUND", "Ironwalker", 2000);
        Add("GROUND", "Mule", 850);
        Add("GROUND", "Patrol", 900);
        Add("GROUND", "Rescue", 950);
        Add("GROUND", "Scout", 600);
        Add("GROUND", "Sentinel", 1100);
        Add("GROUND", "Rover", 650);
        Add("GROUND", "Crusher", 2200);
        Add("GROUND", "Hauler", 1000);

        Add("NAVAL", "Surveyor Mk1", 750);
        Add("NAVAL", "Current", 900);
        Add("NAVAL", "Rescue", 1000);
        Add("NAVAL", "Scout", 700);
        Add("NAVAL", "Sonar", 1100);
        Add("NAVAL", "Depthwatch", 1500);
        Add("NAVAL", "Harbor", 2200);
        Add("NAVAL", "Tidebreaker", 2800);

        Add("COMMAND", "Archive", 2500);
        Add("COMMAND", "Worldmap", 2750);
        Add("COMMAND", "Command Core", 3000);
        Add("COMMAND", "Fusion", 3500);
        Add("COMMAND", "Nexus", 3750);
        Add("COMMAND", "Insight", 3250);
        Add("COMMAND", "Pulse", 3000);
        Add("COMMAND", "Vector Core", 4000);

        Add("EXPERIMENTAL", "Echo", 7500);
        Add("EXPERIMENTAL", "Nullpoint", 10000);
        Add("EXPERIMENTAL", "Specter", 12500);
        Add("EXPERIMENTAL", "Shadowgrid", 15000);
        Add("EXPERIMENTAL", "Phantom", 17500);
        Add("EXPERIMENTAL", "Helix", 20000);

        Add("FACILITY", "Starter Garage", 0);
        Add("FACILITY", "Garage Expansion I", 2500);
        Add("FACILITY", "Garage Expansion II", 5000);
        Add("FACILITY", "Garage Expansion III", 10000);
        Add("FACILITY", "Garage Expansion IV", 20000);
        Add("FACILITY", "Premium Garage Bay", 4000);
        Add("FACILITY", "Heavy Vehicle Bay", 3500);
        Add("FACILITY", "Air / Drone Bay", 3000);
        Add("FACILITY", "Naval Bay", 4000);
        Add("FACILITY", "Command Unit Chamber", 5000);
        Add("FACILITY", "Experimental Containment Bay", 15000);
        Add("FACILITY", "Repair Station", 2500);
        Add("FACILITY", "Upgrade Station", 2500);
        Add("FACILITY", "Fabrication Station", 4000);
        Add("FACILITY", "Salvage Station", 2500);
        Add("FACILITY", "Recycling Station", 2500);
        Add("FACILITY", "Storage Facility", 2000);
        Add("FACILITY", "Fuel Storage Expansion", 1500);
        Add("FACILITY", "Electronics Storage Expansion", 2000);
        Add("FACILITY", "Resource Storage Expansion", 1500);
        Add("FACILITY", "Fleet Command Center", 7500);
        Add("FACILITY", "Deployment Staging Area", 3000);
        Add("FACILITY", "AI Diagnostics Station", 3000);
        Add("FACILITY", "AI Personality Station", 3000);
        Add("FACILITY", "Paint & Customization Bay", 2000);
        Add("FACILITY", "Equipment Station", 2500);
        Add("FACILITY", "Weapons Station", 3000);

        Add("SLOTS", "Additional Garage Slot Ã—1", 250);
        Add("SLOTS", "Additional Garage Slots Ã—5", 1000);
        Add("SLOTS", "Additional Garage Slots Ã—10", 1750);
        Add("SLOTS", "Additional Garage Slots Ã—25", 3500);
        Add("SLOTS", "Additional Garage Slots Ã—50", 6000);
        Add("SLOTS", "Air Unit Slot", 300);
        Add("SLOTS", "Ground Unit Slot", 300);
        Add("SLOTS", "Sea Unit Slot", 500);
        Add("SLOTS", "Command Unit Slot", 750);
        Add("SLOTS", "Experimental Unit Slot", 2000);
        Add("SLOTS", "Heavy Vehicle Slot", 500);
        Add("SLOTS", "Specialized Unit Slot", 750);
        Add("SLOTS", "Reserve Fleet Slot", 250);

        Add("WEAPON", "Light Weapon", 500);
        Add("WEAPON", "Medium Weapon", 900);
        Add("WEAPON", "Heavy Weapon", 1500);
        Add("WEAPON", "Turret Weapon", 750);
        Add("WEAPON", "Suppression Weapon", 850);
        Add("WEAPON", "Direct-Fire Weapon", 900);
        Add("WEAPON", "Projectile Weapon", 1000);
        Add("WEAPON", "Explosive Weapon", 1250);
        Add("WEAPON", "Area-of-Effect Weapon", 1500);
        Add("WEAPON", "Defensive Weapon", 750);
        Add("WEAPON", "Anti-Vehicle Weapon", 1500);
        Add("WEAPON", "Anti-Air Weapon", 1400);
        Add("WEAPON", "Naval Weapon", 1500);
        Add("WEAPON", "Experimental Weapon", 5000);

        Add("COSMETIC", "Basic Paint", 100);
        Add("COSMETIC", "Premium Paint", 250);
        Add("COSMETIC", "Metallic Finish", 300);
        Add("COSMETIC", "Matte Finish", 250);
        Add("COSMETIC", "Gloss Finish", 250);
        Add("COSMETIC", "Weathered Finish", 400);
        Add("COSMETIC", "Industrial Finish", 400);
        Add("COSMETIC", "Battle-Worn Finish", 500);
        Add("COSMETIC", "Unit Emblem", 150);
        Add("COSMETIC", "Faction Emblem", 250);
        Add("COSMETIC", "Player Emblem", 200);
        Add("COSMETIC", "Squad Emblem", 200);
        Add("COSMETIC", "Decal Pack", 200);
        Add("COSMETIC", "Number Decal Pack", 150);
        Add("COSMETIC", "Warning Decal Pack", 200);
        Add("COSMETIC", "Identification Decal Pack", 150);
        Add("COSMETIC", "Unit Marking Pack", 300);
        Add("COSMETIC", "Kill Marking Pack", 350);
        Add("COSMETIC", "Lighting Package", 400);
        Add("COSMETIC", "Exterior Light Kit", 300);
        Add("COSMETIC", "Status Light Kit", 250);
        Add("COSMETIC", "Accent Light Kit", 300);
        Add("COSMETIC", "Antenna Variant", 150);
        Add("COSMETIC", "Sensor Housing Variant", 300);
        Add("COSMETIC", "Armor Appearance Variant", 500);
        Add("COSMETIC", "Wheel Appearance Variant", 300);
        Add("COSMETIC", "Panel Appearance Variant", 250);
        Add("COSMETIC", "Chassis Appearance Variant", 500);

        Add("AI", "AI Personality Package", 500);
        Add("AI", "Aggressive Personality", 500);
        Add("AI", "Defensive Personality", 500);
        Add("AI", "Recon Personality", 500);
        Add("AI", "Cautious Personality", 500);
        Add("AI", "Adaptive Personality", 750);
        Add("AI", "Logistics Personality", 600);
        Add("AI", "Support Personality", 600);
        Add("AI", "Command Personality", 750);
        Add("AI", "Voice Pack", 750);
        Add("AI", "Command Voice Pack", 1000);
        Add("AI", "Warning Voice Pack", 500);
        Add("AI", "Status Voice Pack", 500);
        Add("AI", "AI Core Appearance", 250);
        Add("AI", "AI Core Housing", 400);
        Add("AI", "AI Diagnostic Theme", 250);
        Add("AI", "Command Interface Theme", 300);

        Add("REPAIR", "Emergency Repair", 150);
        Add("REPAIR", "Standard Repair", 300);
        Add("REPAIR", "Advanced Repair", 500);
        Add("REPAIR", "Full Restoration", 750);
        Add("REPAIR", "Field Repair Kit", 250);
        Add("REPAIR", "Vehicle Repair Kit", 350);
        Add("REPAIR", "Drone Repair Kit", 250);
        Add("REPAIR", "Naval Repair Kit", 500);
        Add("REPAIR", "Command Repair Kit", 750);
        Add("REPAIR", "Maintenance Package", 300);
        Add("REPAIR", "Extended Maintenance Package", 600);
        Add("REPAIR", "Preventive Maintenance Package", 450);

        Add("STORAGE", "Storage Expansion", 750);
        Add("STORAGE", "Fuel Storage Expansion", 750);
        Add("STORAGE", "Electronics Storage Expansion", 1000);
        Add("STORAGE", "Alloy Storage Expansion", 1000);
        Add("STORAGE", "Component Storage Expansion", 750);
        Add("STORAGE", "Resource Storage Expansion", 750);
        Add("STORAGE", "Fabrication Queue Expansion", 1500);
        Add("STORAGE", "Repair Queue Expansion", 1000);
        Add("STORAGE", "Manufacturing Queue Expansion", 1500);
        Add("STORAGE", "Salvage Queue Expansion", 1000);
        Add("STORAGE", "Additional Production Slot", 2000);
        Add("STORAGE", "Additional Repair Slot", 1000);
        Add("STORAGE", "Additional Fabrication Slot", 1500);
        Add("STORAGE", "Additional Salvage Slot", 1000);

        Add("FLEET", "Deployment Configuration Slot", 250);
        Add("FLEET", "Army Preset Slot", 250);
        Add("FLEET", "Fleet Preset Slot", 250);
        Add("FLEET", "Squad Preset Slot", 150);
        Add("FLEET", "Loadout Preset Slot", 150);
        Add("FLEET", "Deployment Template Slot", 250);

        Add("MODULE", "Sensor Module", 300);
        Add("MODULE", "Recon Module", 350);
        Add("MODULE", "Thermal Module", 400);
        Add("MODULE", "LiDAR Module", 450);
        Add("MODULE", "Radar Module", 500);
        Add("MODULE", "Acoustic Sensor Module", 450);
        Add("MODULE", "Signal Detection Module", 400);
        Add("MODULE", "Communication Module", 350);
        Add("MODULE", "Mesh Relay Module", 500);
        Add("MODULE", "Command Relay Module", 750);
        Add("MODULE", "AI Control Module", 600);
        Add("MODULE", "Navigation Module", 350);
        Add("MODULE", "Stabilization Module", 300);
        Add("MODULE", "Power Module", 400);
        Add("MODULE", "Battery Module", 350);
        Add("MODULE", "Fuel Module", 350);
        Add("MODULE", "Armor Module", 600);
        Add("MODULE", "Mobility Module", 500);
        Add("MODULE", "Payload Module", 500);
        Add("MODULE", "Weapon Mount Module", 650);
        Add("MODULE", "Countermeasure Module", 550);
        Add("MODULE", "Stealth Module", 1000);
        Add("MODULE", "Electronic Warfare Module", 1250);
        Add("MODULE", "Medical Module", 700);
        Add("MODULE", "Cargo Module", 500);
        Add("MODULE", "Repair Module", 750);
        Add("MODULE", "Recovery Module", 650);
    }

    private void Add(
        string category,
        string name,
        int cost)
    {
        catalog.Add(
            new CatalogItem(
                category,
                name,
                cost
            )
        );
    }

    private CatalogItem FindItem(
        string name)
    {
        foreach (
            CatalogItem item
            in catalog)
        {
            if (
                item.Name.Equals(
                    name,
                    StringComparison.OrdinalIgnoreCase
                )
            )
                return item;
        }

        return null;
    }

    // =========================================================
    // 3D HELPERS
    // =========================================================

    private GameObject Cube(
        string name,
        Vector3 position,
        Vector3 scale,
        Material material,
        Transform parent = null)
    {
        GameObject obj =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        obj.name = name;
        obj.transform.position = position;
        obj.transform.localScale = scale;

        if (parent != null)
            obj.transform.SetParent(
                parent,
                true
            );

        Renderer r =
            obj.GetComponent<Renderer>();

        r.sharedMaterial =
            material;

        return obj;
    }

    private GameObject Sphere(
        string name,
        Vector3 position,
        float scale,
        Material material,
        Transform parent = null)
    {
        GameObject obj =
            GameObject.CreatePrimitive(
                PrimitiveType.Sphere
            );

        obj.name = name;
        obj.transform.position = position;
        obj.transform.localScale =
            Vector3.one * scale;

        if (parent != null)
            obj.transform.SetParent(
                parent,
                true
            );

        obj.GetComponent<Renderer>()
            .sharedMaterial = material;

        return obj;
    }

    private GameObject Cylinder(
        string name,
        Vector3 position,
        float radius,
        float height,
        Material material,
        Transform parent,
        Vector3 rotation)
    {
        GameObject obj =
            GameObject.CreatePrimitive(
                PrimitiveType.Cylinder
            );

        obj.name = name;
        obj.transform.position = position;

        obj.transform.localScale =
            new Vector3(
                radius,
                height,
                radius
            );

        obj.transform.rotation =
            Quaternion.Euler(
                rotation
            );

        obj.transform.SetParent(
            parent,
            true
        );

        obj.GetComponent<Renderer>()
            .sharedMaterial = material;

        return obj;
    }

    private void LookAt(
        Transform source,
        Vector3 target)
    {
        source.rotation =
            Quaternion.LookRotation(
                target -
                source.position
            );
    }

    private TextMesh CreateWorldText(
        string value,
        Vector3 position,
        float size,
        Color color)
    {
        GameObject obj =
            new GameObject(
                "SIGN // " + value
            );

        obj.transform.SetParent(
            world,
            true
        );

        obj.transform.position =
            position;

        TextMesh text =
            obj.AddComponent<TextMesh>();

        text.text = value;
        text.fontSize = 24;
        text.characterSize = size;
        text.anchor =
            TextAnchor.MiddleCenter;

        text.alignment =
            TextAlignment.Center;

        text.color =
            color;

        return text;
    }

    // =========================================================
    // UI HELPERS
    // =========================================================

    private GameObject CreatePanel(
        string name,
        Transform parent,
        Color color,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 offsetMin,
        Vector2 offsetMax)
    {
        GameObject obj =
            new GameObject(name);

        obj.transform.SetParent(
            parent,
            false
        );

        RectTransform rt =
            obj.AddComponent<RectTransform>();

        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.offsetMin = offsetMin;
        rt.offsetMax = offsetMax;

        Image image =
            obj.AddComponent<Image>();

        image.color =
            color;

        return obj;
    }

    private Image CreateImage(
        string name,
        Transform parent,
        Color color,
        Vector2 anchorMin,
        Vector2 anchorMax)
    {
        GameObject obj =
            new GameObject(name);

        obj.transform.SetParent(
            parent,
            false
        );

        RectTransform rt =
            obj.AddComponent<RectTransform>();

        rt.anchorMin =
            anchorMin;

        rt.anchorMax =
            anchorMax;

        rt.offsetMin =
            Vector2.zero;

        rt.offsetMax =
            Vector2.zero;

        Image image =
            obj.AddComponent<Image>();

        image.color =
            color;

        return image;
    }

    private Button CreateButton(
        Transform parent,
        string label,
        Color background,
        Color textColor,
        Vector2 offsetMin,
        Vector2 offsetMax)
    {
        return CreateButton(
            parent,
            label,
            background,
            textColor,
            Vector2.zero,
            Vector2.zero,
            offsetMin,
            offsetMax
        );
    }

    private Button CreateButton(
        Transform parent,
        string label,
        Color background,
        Color textColor,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 offsetMin,
        Vector2 offsetMax)
    {
        GameObject obj =
            new GameObject(
                "[BUTTON] " +
                label
            );

        obj.transform.SetParent(
            parent,
            false
        );

        RectTransform rt =
            obj.AddComponent<RectTransform>();

        rt.anchorMin =
            anchorMin;

        rt.anchorMax =
            anchorMax;

        rt.offsetMin =
            offsetMin;

        rt.offsetMax =
            offsetMax;

        Image image =
            obj.AddComponent<Image>();

        image.color =
            background;

        Button button =
            obj.AddComponent<Button>();

        ColorBlock cb =
            button.colors;

        cb.normalColor =
            background;

        cb.highlightedColor =
            new Color(
                Mathf.Min(
                    background.r + 0.12f,
                    1
                ),
                Mathf.Min(
                    background.g + 0.12f,
                    1
                ),
                Mathf.Min(
                    background.b + 0.12f,
                    1
                ),
                1
            );

        cb.pressedColor =
            cyan;

        button.colors =
            cb;

        GameObject textObject =
            new GameObject(
                "TEXT"
            );

        textObject.transform.SetParent(
            obj.transform,
            false
        );

        RectTransform textRT =
            textObject.AddComponent<
                RectTransform>();

        textRT.anchorMin =
            Vector2.zero;

        textRT.anchorMax =
            Vector2.one;

        textRT.offsetMin =
            new Vector2(
                6,
                3
            );

        textRT.offsetMax =
            new Vector2(
                -6,
                -3
            );

        Text text =
            textObject.AddComponent<
                Text>();

        text.font =
            font;

        text.text =
            label;

        text.fontSize =
            11;

        text.color =
            textColor;

        text.alignment =
            TextAnchor.MiddleCenter;

        text.horizontalOverflow =
            HorizontalWrapMode.Wrap;

        text.verticalOverflow =
            VerticalWrapMode.Overflow;

        return button;
    }

    private Text AddText(
        GameObject parent,
        string name,
        string value,
        int size,
        Color color,
        TextAnchor alignment,
        Vector2 offsetMin,
        Vector2 offsetMax)
    {
        GameObject obj =
            new GameObject(
                name
            );

        obj.transform.SetParent(
            parent.transform,
            false
        );

        RectTransform rt =
            obj.AddComponent<
                RectTransform>();

        rt.anchorMin =
            new Vector2(
                0,
                1
            );

        rt.anchorMax =
            new Vector2(
                1,
                1
            );

        rt.offsetMin =
            offsetMin;

        rt.offsetMax =
            offsetMax;

        Text text =
            obj.AddComponent<
                Text>();

        text.font =
            font;

        text.text =
            value;

        text.fontSize =
            Mathf.RoundToInt(size * 0.70f);

        text.color =
            color;

        text.alignment =
            alignment;

        text.horizontalOverflow =
            HorizontalWrapMode.Wrap;

        text.verticalOverflow =
            VerticalWrapMode.Overflow;

        return text;
    }
}



