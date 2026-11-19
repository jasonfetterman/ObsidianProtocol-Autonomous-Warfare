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
#endif

public class OPAWResearchLabVisualBuilder
{
    private const string SCENE =
        "Assets/Scenes/SCN-06  RESEARCH LAB/[HUD] RESEARCH HUD/Reseaarch_Lab.unity";

    private const float LAB_WIDTH = 70f;
    private const float LAB_DEPTH = 55f;
    private const float LAB_HEIGHT = 15f;

    private Font font;

    private Color bg =
        new Color(0.008f, 0.014f, 0.022f, 1f);

    private Color panel =
        new Color(0.025f, 0.040f, 0.058f, 0.96f);

    private Color panelDark =
        new Color(0.012f, 0.022f, 0.034f, 0.98f);

    private Color line =
        new Color(0.12f, 0.34f, 0.48f, 1f);

    private Color cyan =
        new Color(0.18f, 0.82f, 1f, 1f);

    private Color cyanSoft =
        new Color(0.10f, 0.45f, 0.62f, 1f);

    private Color green =
        new Color(0.25f, 1f, 0.60f, 1f);

    private Color yellow =
        new Color(1f, 0.72f, 0.22f, 1f);

    private Color red =
        new Color(1f, 0.25f, 0.28f, 1f);

    private Color white =
        new Color(0.82f, 0.91f, 0.96f, 1f);

    private Material floorMat;
    private Material wallMat;
    private Material darkMat;
    private Material panelMat;
    private Material cyanMat;
    private Material greenMat;
    private Material yellowMat;
    private Material redMat;

    private Camera researchCamera;

    // =========================================================
    // MENU
    // =========================================================

#if UNITY_EDITOR

    [MenuItem("Obsidian Protocol/Build/RESEARCH LAB - FULL VISUAL")]
    public static void BuildResearchLab()
    {
        Debug.Log(
            "RESEARCH LAB BUILD: START - SCN-06 RESEARCH LAB"
        );

        EnsureSceneFolder();

        Scene scene =
            EditorSceneManager.OpenScene(
                SCENE,
                OpenSceneMode.Single
            );

        if (!scene.IsValid())
        {
            throw new Exception(
                "Unable to open Research Lab scene:\n" +
                SCENE
            );
        }

        GameObject[] roots =
            scene.GetRootGameObjects();

        foreach (GameObject root in roots)
        {
            UnityEngine.Object.DestroyImmediate(root);
        }

        OPAWResearchLabVisualBuilder builder =
            new OPAWResearchLabVisualBuilder();

        builder.Build();

        EditorSceneManager.MarkSceneDirty(scene);

        EditorSceneManager.SaveScene(scene);

        AssetDatabase.SaveAssets();

        Debug.Log(
            "RESEARCH LAB BUILD: COMPLETE - FULL VISUAL RESEARCH LAB SAVED"
        );

        Selection.activeObject = null;
    }

    private static void EnsureSceneFolder()
    {
        string folder =
            "Assets/Scenes/SCN-06  RESEARCH LAB";

        string hudFolder =
            folder + "/[HUD] RESEARCH HUD";

        if (!AssetDatabase.IsValidFolder(folder))
        {
            AssetDatabase.CreateFolder(
                "Assets/Scenes",
                "SCN-06  RESEARCH LAB"
            );
        }

        if (!AssetDatabase.IsValidFolder(hudFolder))
        {
            AssetDatabase.CreateFolder(
                folder,
                "[HUD] RESEARCH HUD"
            );
        }

        if (!System.IO.File.Exists(SCENE))
        {
            Scene newScene =
                EditorSceneManager.NewScene(
                    NewSceneSetup.EmptyScene,
                    NewSceneMode.Single
                );

            EditorSceneManager.SaveScene(
                newScene,
                SCENE
            );
        }
    }

#endif

    // =========================================================
    // MASTER BUILD
    // =========================================================

    private void Build()
    {
        Debug.Log("RESEARCH LAB BUILD: MATERIALS");

        BuildMaterials();

        Debug.Log("RESEARCH LAB BUILD: WORLD");

        BuildWorld();

        Debug.Log("RESEARCH LAB BUILD: RESEARCH FACILITY");

        BuildResearchFacility();

        Debug.Log("RESEARCH LAB BUILD: TECHNOLOGY TREE");

        BuildTechnologyTreeWall();

        Debug.Log("RESEARCH LAB BUILD: AI RESEARCH");

        BuildAIResearchArea();

        Debug.Log("RESEARCH LAB BUILD: EXPERIMENTAL AREA");

        BuildExperimentalArea();

        Debug.Log("RESEARCH LAB BUILD: RESEARCH TERMINALS");

        BuildResearchTerminals();

        Debug.Log("RESEARCH LAB BUILD: CAMERA");

        BuildCamera();

        Debug.Log("RESEARCH LAB BUILD: LIGHTING");

        BuildLighting();

        Debug.Log("RESEARCH LAB BUILD: EVENT SYSTEM");

        BuildEventSystem();

        Debug.Log("RESEARCH LAB BUILD: HUD");

        BuildHUD();
    }

    // =========================================================
    // MATERIALS
    // =========================================================

    private void BuildMaterials()
    {
        floorMat =
            CreateMaterial(
                "RESEARCH FLOOR",
                new Color(0.018f, 0.025f, 0.032f)
            );

        wallMat =
            CreateMaterial(
                "RESEARCH WALL",
                new Color(0.028f, 0.040f, 0.050f)
            );

        darkMat =
            CreateMaterial(
                "RESEARCH DARK",
                new Color(0.006f, 0.012f, 0.018f)
            );

        panelMat =
            CreateMaterial(
                "RESEARCH PANEL",
                new Color(0.035f, 0.060f, 0.078f)
            );

        cyanMat =
            CreateMaterial(
                "RESEARCH CYAN",
                cyan
            );

        greenMat =
            CreateMaterial(
                "RESEARCH GREEN",
                green
            );

        yellowMat =
            CreateMaterial(
                "RESEARCH YELLOW",
                yellow
            );

        redMat =
            CreateMaterial(
                "RESEARCH RED",
                red
            );
    }

    private Material CreateMaterial(
        string materialName,
        Color color
    )
    {
        Material material =
            new Material(
                Shader.Find("Universal Render Pipeline/Lit")
            );

        if (material.shader == null)
        {
            material =
                new Material(
                    Shader.Find("Standard")
                );
        }

        material.name =
            materialName;

        material.color =
            color;

        return material;
    }

    // =========================================================
    // WORLD
    // =========================================================

    private void BuildWorld()
    {
        CreateCube(
            "RESEARCH FLOOR",
            new Vector3(0f, -0.25f, 0f),
            new Vector3(
                LAB_WIDTH,
                0.5f,
                LAB_DEPTH
            ),
            floorMat
        );

        CreateCube(
            "NORTH WALL",
            new Vector3(
                0f,
                LAB_HEIGHT / 2f,
                LAB_DEPTH / 2f
            ),
            new Vector3(
                LAB_WIDTH,
                LAB_HEIGHT,
                0.5f
            ),
            wallMat
        );

        CreateCube(
            "SOUTH WALL",
            new Vector3(
                0f,
                LAB_HEIGHT / 2f,
                -LAB_DEPTH / 2f
            ),
            new Vector3(
                LAB_WIDTH,
                LAB_HEIGHT,
                0.5f
            ),
            wallMat
        );

        CreateCube(
            "WEST WALL",
            new Vector3(
                -LAB_WIDTH / 2f,
                LAB_HEIGHT / 2f,
                0f
            ),
            new Vector3(
                0.5f,
                LAB_HEIGHT,
                LAB_DEPTH
            ),
            wallMat
        );

        CreateCube(
            "EAST WALL",
            new Vector3(
                LAB_WIDTH / 2f,
                LAB_HEIGHT / 2f,
                0f
            ),
            new Vector3(
                0.5f,
                LAB_HEIGHT,
                LAB_DEPTH
            ),
            wallMat
        );

        CreateCube(
            "CEILING",
            new Vector3(
                0f,
                LAB_HEIGHT,
                0f
            ),
            new Vector3(
                LAB_WIDTH,
                0.4f,
                LAB_DEPTH
            ),
            darkMat
        );

        CreateCube(
            "CENTRAL RESEARCH PLATFORM",
            new Vector3(
                0f,
                0.05f,
                4f
            ),
            new Vector3(
                30f,
                0.15f,
                18f
            ),
            panelMat
        );

        AddWorldText(
            "OBSIDIAN PROTOCOL",
            new Vector3(
                0f,
                10.8f,
                26.7f
            ),
            0.12f,
            cyan
        );

        AddWorldText(
            "RESEARCH & TECHNOLOGY",
            new Vector3(
                0f,
                9.6f,
                26.6f
            ),
            0.085f,
            white
        );

        AddWorldText(
            "AUTHORIZED RESEARCH FACILITY // LEVEL 12",
            new Vector3(
                0f,
                8.6f,
                26.5f
            ),
            0.055f,
            cyanSoft
        );

        CreateAccentLine(
            new Vector3(0f, 0.02f, -26f),
            new Vector3(55f, 0.05f, 0.08f)
        );

        CreateAccentLine(
            new Vector3(0f, 0.02f, 26f),
            new Vector3(55f, 0.05f, 0.08f)
        );

        CreateAccentLine(
            new Vector3(-32f, 0.02f, 0f),
            new Vector3(0.08f, 0.05f, 45f)
        );

        CreateAccentLine(
            new Vector3(32f, 0.02f, 0f),
            new Vector3(0.08f, 0.05f, 45f)
        );
    }

    // =========================================================
    // MAIN RESEARCH FACILITY
    // =========================================================

    private void BuildResearchFacility()
    {
        CreateResearchCore(
            new Vector3(0f, 0f, 3f)
        );

        CreateConsoleBank(
            new Vector3(-22f, 0f, 3f),
            "RESEARCH CONTROL"
        );

        CreateConsoleBank(
            new Vector3(22f, 0f, 3f),
            "DATA ANALYSIS"
        );

        CreateResearchPods();

        CreateResourceMachines();

        AddWorldText(
            "RESEARCH COMMAND CENTER",
            new Vector3(
                0f,
                7.5f,
                12f
            ),
            0.09f,
            cyan
        );

        AddWorldText(
            "ACTIVE RESEARCH NETWORK",
            new Vector3(
                0f,
                6.7f,
                12f
            ),
            0.055f,
            green
        );
    }

    private void CreateResearchCore(
        Vector3 position
    )
    {
        GameObject core =
            CreateCube(
                "RESEARCH COMMAND CORE",
                position +
                new Vector3(0f, 1.5f, 0f),
                new Vector3(
                    8f,
                    3f,
                    5f
                ),
                darkMat
            );

        CreateCube(
            "CORE DISPLAY",
            position +
            new Vector3(
                0f,
                3.2f,
                -2.55f
            ),
            new Vector3(
                6.5f,
                2.8f,
                0.12f
            ),
            cyanMat
        );

        CreateCube(
            "CORE BASE",
            position +
            new Vector3(
                0f,
                0.3f,
                0f
            ),
            new Vector3(
                9f,
                0.4f,
                6f
            ),
            panelMat
        );

        for (int i = 0; i < 4; i++)
        {
            float x =
                -3f +
                i * 2f;

            CreateCube(
                "CORE DATA NODE " + i,
                position +
                new Vector3(
                    x,
                    1f,
                    -2.9f
                ),
                new Vector3(
                    1.1f,
                    0.7f,
                    0.15f
                ),
                cyanMat
            );
        }

        AddWorldText(
            "RESEARCH CORE",
            position +
            new Vector3(
                0f,
                5.2f,
                -2.8f
            ),
            0.06f,
            cyan
        );
    }

    private void CreateConsoleBank(
        Vector3 position,
        string title
    )
    {
        CreateCube(
            title + " PLATFORM",
            position +
            new Vector3(0f, 0.35f, 0f),
            new Vector3(
                13f,
                0.7f,
                4f
            ),
            panelMat
        );

        for (int i = 0; i < 4; i++)
        {
            float x =
                -5f +
                i * 3.3f;

            CreateCube(
                title + " CONSOLE " + i,
                position +
                new Vector3(
                    x,
                    1.35f,
                    0f
                ),
                new Vector3(
                    2.3f,
                    2f,
                    1.3f
                ),
                darkMat
            );

            CreateCube(
                title + " SCREEN " + i,
                position +
                new Vector3(
                    x,
                    2f,
                    -0.7f
                ),
                new Vector3(
                    1.8f,
                    1.1f,
                    0.08f
                ),
                cyanMat
            );

            CreateCube(
                title + " STATUS " + i,
                position +
                new Vector3(
                    x,
                    0.9f,
                    -0.72f
                ),
                new Vector3(
                    1.1f,
                    0.12f,
                    0.08f
                ),
                greenMat
            );
        }

        AddWorldText(
            title,
            position +
            new Vector3(
                0f,
                3.3f,
                0f
            ),
            0.055f,
            white
        );
    }

    private void CreateResearchPods()
    {
        for (int i = 0; i < 5; i++)
        {
            float x =
                -24f +
                i * 12f;

            CreateCube(
                "RESEARCH POD " + (i + 1),
                new Vector3(
                    x,
                    2.5f,
                    20f
                ),
                new Vector3(
                    8f,
                    5f,
                    4f
                ),
                darkMat
            );

            CreateCube(
                "POD GLASS " + (i + 1),
                new Vector3(
                    x,
                    2.7f,
                    17.85f
                ),
                new Vector3(
                    6f,
                    3.5f,
                    0.08f
                ),
                cyanMat
            );

            CreateCube(
                "POD STATUS " + (i + 1),
                new Vector3(
                    x,
                    0.6f,
                    17.7f
                ),
                new Vector3(
                    4f,
                    0.15f,
                    0.12f
                ),
                greenMat
            );
        }

        AddWorldText(
            "AUTONOMOUS SYSTEMS RESEARCH",
            new Vector3(
                0f,
                7.2f,
                17.5f
            ),
            0.06f,
            cyan
        );
    }

    private void CreateResourceMachines()
    {
        string[] names =
        {
            "SCIENTIST NETWORK",
            "FUNDING NODE",
            "MATERIALS PROCESSOR",
            "ENERGY ALLOCATION"
        };

        for (int i = 0; i < names.Length; i++)
        {
            float x =
                -21f +
                i * 14f;

            CreateCube(
                names[i],
                new Vector3(
                    x,
                    1.5f,
                    -17f
                ),
                new Vector3(
                    10f,
                    3f,
                    4f
                ),
                darkMat
            );

            CreateCube(
                names[i] + " DISPLAY",
                new Vector3(
                    x,
                    3f,
                    -19.1f
                ),
                new Vector3(
                    7f,
                    1.8f,
                    0.08f
                ),
                cyanMat
            );

            AddWorldText(
                names[i],
                new Vector3(
                    x,
                    5f,
                    -18.9f
                ),
                0.045f,
                white
            );
        }
    }

    // =========================================================
    // TECHNOLOGY TREE WALL
    // =========================================================

    private void BuildTechnologyTreeWall()
    {
        CreateCube(
            "TECHNOLOGY TREE WALL",
            new Vector3(
                -29.5f,
                5.5f,
                8f
            ),
            new Vector3(
                0.5f,
                10f,
                30f
            ),
            darkMat
        );

        string[] branches =
        {
            "MOBILITY",
            "PROPULSION",
            "SENSORS",
            "COMMUNICATIONS",
            "DEFENSE",
            "WEAPONS",
            "LOGISTICS",
            "AI",
            "EXPERIMENTAL"
        };

        for (int i = 0; i < branches.Length; i++)
        {
            float z =
                -8f +
                i * 4f;

            CreateCube(
                "TECH NODE " + branches[i],
                new Vector3(
                    -29.1f,
                    5.5f,
                    z
                ),
                new Vector3(
                    0.15f,
                    2.2f,
                    2.8f
                ),
                i == 7
                    ? greenMat
                    : cyanMat
            );

            AddWorldText(
                branches[i],
                new Vector3(
                    -28.8f,
                    5.5f,
                    z
                ),
                0.045f,
                white
            );
        }

        AddWorldText(
            "TECHNOLOGY TREE",
            new Vector3(
                -28.8f,
                11.8f,
                8f
            ),
            0.065f,
            cyan
        );
    }

    // =========================================================
    // AI RESEARCH
    // =========================================================

    private void BuildAIResearchArea()
    {
        CreateCube(
            "AI RESEARCH PLATFORM",
            new Vector3(
                22f,
                0.3f,
                -10f
            ),
            new Vector3(
                15f,
                0.6f,
                11f
            ),
            panelMat
        );

        string[] systems =
        {
            "AUTONOMY",
            "DECISION MAKING",
            "COORDINATION",
            "PERSONALITY"
        };

        for (int i = 0; i < systems.Length; i++)
        {
            float x =
                17f +
                (i % 2) * 10f;

            float z =
                -13f +
                (i / 2) * 6f;

            CreateCube(
                "AI " + systems[i],
                new Vector3(
                    x,
                    1.5f,
                    z
                ),
                new Vector3(
                    7f,
                    2.5f,
                    4f
                ),
                darkMat
            );

            CreateCube(
                "AI SCREEN " + systems[i],
                new Vector3(
                    x,
                    2.8f,
                    z - 2.05f
                ),
                new Vector3(
                    5.5f,
                    1.7f,
                    0.08f
                ),
                greenMat
            );

            AddWorldText(
                systems[i],
                new Vector3(
                    x,
                    4.4f,
                    z
                ),
                0.045f,
                white
            );
        }

        AddWorldText(
            "AI RESEARCH",
            new Vector3(
                22f,
                6.5f,
                -10f
            ),
            0.065f,
            green
        );
    }

    // =========================================================
    // EXPERIMENTAL AREA
    // =========================================================

    private void BuildExperimentalArea()
    {
        CreateCube(
            "EXPERIMENTAL CHAMBER",
            new Vector3(
                -18f,
                3f,
                -12f
            ),
            new Vector3(
                12f,
                6f,
                9f
            ),
            darkMat
        );

        CreateCube(
            "EXPERIMENTAL GLASS",
            new Vector3(
                -18f,
                3f,
                -16.6f
            ),
            new Vector3(
                9f,
                4.5f,
                0.08f
            ),
            redMat
        );

        CreateCube(
            "EXPERIMENTAL CORE",
            new Vector3(
                -18f,
                3f,
                -12f
            ),
            new Vector3(
                3f,
                4f,
                3f
            ),
            cyanMat
        );

        for (int i = 0; i < 6; i++)
        {
            float angle =
                i * 60f;

            float rad =
                angle * Mathf.Deg2Rad;

            CreateCube(
                "EXPERIMENTAL RING " + i,
                new Vector3(
                    -18f +
                    Mathf.Cos(rad) * 3.5f,
                    3f,
                    -12f +
                    Mathf.Sin(rad) * 3.5f
                ),
                new Vector3(
                    0.25f,
                    0.25f,
                    2.5f
                ),
                yellowMat
            );
        }

        AddWorldText(
            "EXPERIMENTAL SYSTEMS",
            new Vector3(
                -18f,
                7.2f,
                -16f
            ),
            0.055f,
            red
        );
    }

    // =========================================================
    // RESEARCH TERMINALS
    // =========================================================

    private void BuildResearchTerminals()
    {
        CreateTerminal(
            "ACTIVE RESEARCH",
            new Vector3(
                -10f,
                1f,
                11f
            ),
            greenMat
        );

        CreateTerminal(
            "RESEARCH QUEUE",
            new Vector3(
                10f,
                1f,
                11f
            ),
            cyanMat
        );

        CreateTerminal(
            "RESEARCH RESOURCES",
            new Vector3(
                0f,
                1f,
                -7f
            ),
            yellowMat
        );
    }

    private void CreateTerminal(
        string title,
        Vector3 position,
        Material screenMaterial
    )
    {
        CreateCube(
            title + " TERMINAL",
            position +
            new Vector3(0f, 1.3f, 0f),
            new Vector3(
                5f,
                2.6f,
                2.2f
            ),
            darkMat
        );

        CreateCube(
            title + " SCREEN",
            position +
            new Vector3(
                0f,
                2.6f,
                -1.15f
            ),
            new Vector3(
                4f,
                1.8f,
                0.08f
            ),
            screenMaterial
        );

        CreateCube(
            title + " BASE",
            position +
            new Vector3(
                0f,
                0.25f,
                0f
            ),
            new Vector3(
                5.5f,
                0.5f,
                2.8f
            ),
            panelMat
        );

        AddWorldText(
            title,
            position +
            new Vector3(
                0f,
                4.2f,
                0f
            ),
            0.05f,
            white
        );
    }

    // =========================================================
    // CAMERA
    // =========================================================

    private void BuildCamera()
    {
        GameObject obj =
            new GameObject(
                "RESEARCH LAB CAMERA"
            );

        researchCamera =
            obj.AddComponent<Camera>();

        researchCamera.tag =
            "MainCamera";

        researchCamera.enabled =
            true;

        researchCamera.clearFlags =
            CameraClearFlags.SolidColor;

        researchCamera.backgroundColor =
            bg;

        researchCamera.fieldOfView =
            68f;

        researchCamera.nearClipPlane =
            0.05f;

        researchCamera.farClipPlane =
            500f;

        obj.transform.position =
            new Vector3(
                0f,
                24f,
                -70f
            );

        LookAt(
            obj.transform,
            new Vector3(
                0f,
                4f,
                4f
            )
        );
    }

    // =========================================================
    // LIGHTING
    // =========================================================

    private void BuildLighting()
    {
        GameObject lightObject =
            new GameObject(
                "RESEARCH LAB MAIN LIGHT"
            );

        Light light =
            lightObject.AddComponent<Light>();

        light.type =
            LightType.Directional;

        light.intensity =
            0.75f;

        light.color =
            new Color(
                0.72f,
                0.86f,
                1f
            );

        lightObject.transform.rotation =
            Quaternion.Euler(
                45f,
                -30f,
                0f
            );

        for (int i = 0; i < 6; i++)
        {
            GameObject ceilingLight =
                new GameObject(
                    "RESEARCH CEILING LIGHT " + i
                );

            Light point =
                ceilingLight.AddComponent<Light>();

            point.type =
                LightType.Point;

            point.range =
                16f;

            point.intensity =
                3f;

            point.color =
                new Color(
                    0.35f,
                    0.75f,
                    1f
                );

            ceilingLight.transform.position =
                new Vector3(
                    -25f +
                    (i % 3) * 25f,
                    12f,
                    -15f +
                    (i / 3) * 30f
                );
        }

        RenderSettings.ambientIntensity =
            0.35f;
    }

    // =========================================================
    // EVENT SYSTEM
    // =========================================================

    private void BuildEventSystem()
    {
        GameObject existing =
            GameObject.Find(
                "EventSystem"
            );

        if (existing != null)
        {
            UnityEngine.Object.DestroyImmediate(
                existing
            );
        }

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
            UnityEngine.EventSystems.StandaloneInputModule>();
#endif
    }

    // =========================================================
    // HUD
    // =========================================================

    private void BuildHUD()
    {
        font =
            Resources.GetBuiltinResource<Font>(
                "LegacyRuntime.ttf"
            );

        GameObject canvasObject =
            new GameObject(
                "RESEARCH HUD"
            );

        Canvas canvas =
            canvasObject.AddComponent<Canvas>();

        canvas.renderMode =
            RenderMode.ScreenSpaceOverlay;

        canvas.sortingOrder =
            100;

        CanvasScaler scaler =
            canvasObject.AddComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(
                1920f,
                1080f
            );

        scaler.screenMatchMode =
            CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        scaler.matchWidthOrHeight =
            0.5f;

        canvasObject.AddComponent<
            GraphicRaycaster>();

        CreateHUDBackground(
            canvasObject.transform
        );

        CreateTopBar(
            canvasObject.transform
        );

        CreateLeftResearchPanel(
            canvasObject.transform
        );

        CreateCenterTechnologyPanel(
            canvasObject.transform
        );

        CreateRightResearchPanel(
            canvasObject.transform
        );

        CreateBottomBar(
            canvasObject.transform
        );

        CreateConfirmation(
            canvasObject.transform
        );
    }

    // =========================================================
    // HUD BACKGROUND
    // =========================================================

    private void CreateHUDBackground(
        Transform parent
    )
    {
        CreatePanel(
            parent,
            "HUD BACKGROUND",
            new Vector2(
                0.5f,
                0.5f
            ),
            new Vector2(
                1f,
                1f
            ),
            Vector2.zero,
            Vector2.zero,
            new Color(
                0.005f,
                0.010f,
                0.017f,
                0.86f
            )
        );
    }

    // =========================================================
    // TOP BAR
    // =========================================================

    private void CreateTopBar(
        Transform parent
    )
    {
        CreatePanel(
            parent,
            "TOP BAR",
            new Vector2(
                0.5f,
                0.94f
            ),
            new Vector2(
                0.96f,
                0.10f
            ),
            Vector2.zero,
            Vector2.zero,
            panelDark
        );

        AddText(
            parent,
            "12. RESEARCH & TECHNOLOGY",
            new Vector2(
                0.04f,
                0.94f
            ),
            new Vector2(
                0.30f,
                0.055f
            ),
            25,
            cyan,
            TextAnchor.MiddleLeft
        );

        AddText(
            parent,
            "RESEARCH LAB // LEVEL 12",
            new Vector2(
                0.50f,
                0.94f
            ),
            new Vector2(
                0.25f,
                0.045f
            ),
            16,
            white,
            TextAnchor.MiddleCenter
        );

        AddText(
            parent,
            "FUNDING  84,250",
            new Vector2(
                0.72f,
                0.94f
            ),
            new Vector2(
                0.12f,
                0.045f
            ),
            14,
            yellow,
            TextAnchor.MiddleCenter
        );

        AddText(
            parent,
            "ENERGY  91%",
            new Vector2(
                0.84f,
                0.94f
            ),
            new Vector2(
                0.10f,
                0.045f
            ),
            14,
            green,
            TextAnchor.MiddleCenter
        );

        AddText(
            parent,
            "ONLINE",
            new Vector2(
                0.94f,
                0.94f
            ),
            new Vector2(
                0.07f,
                0.045f
            ),
            13,
            green,
            TextAnchor.MiddleCenter
        );
    }

    // =========================================================
    // LEFT PANEL
    // =========================================================

    private void CreateLeftResearchPanel(
        Transform parent
    )
    {
        CreatePanel(
            parent,
            "TECHNOLOGY TREE PANEL",
            new Vector2(
                0.17f,
                0.51f
            ),
            new Vector2(
                0.29f,
                0.73f
            ),
            Vector2.zero,
            Vector2.zero,
            panel
        );

        AddText(
            parent,
            "TECHNOLOGY TREE",
            new Vector2(
                0.17f,
                0.825f
            ),
            new Vector2(
                0.25f,
                0.045f
            ),
            18,
            cyan,
            TextAnchor.MiddleLeft
        );

        string[] tabs =
        {
            "MOBILITY",
            "PROPULSION",
            "SENSORS",
            "COMMUNICATIONS",
            "DEFENSE",
            "WEAPONS",
            "LOGISTICS",
            "AI",
            "EXPERIMENTAL"
        };

        for (int i = 0; i < tabs.Length; i++)
        {
            float y =
                0.775f -
                i * 0.060f;

            CreateButton(
                parent,
                "TAB " + tabs[i],
                tabs[i],
                new Vector2(
                    0.17f,
                    y
                ),
                new Vector2(
                    0.24f,
                    0.045f
                ),
                i == 0
                    ? cyanSoft
                    : panelDark
            );
        }

        AddText(
            parent,
            "BRANCH STATUS",
            new Vector2(
                0.17f,
                0.215f
            ),
            new Vector2(
                0.22f,
                0.035f
            ),
            13,
            cyan,
            TextAnchor.MiddleLeft
        );

        AddText(
            parent,
            "UNLOCKED TECHNOLOGIES     42",
            new Vector2(
                0.17f,
                0.175f
            ),
            new Vector2(
                0.24f,
                0.035f
            ),
            11,
            white,
            TextAnchor.MiddleLeft
        );

        AddText(
            parent,
            "AVAILABLE RESEARCH          18",
            new Vector2(
                0.17f,
                0.140f
            ),
            new Vector2(
                0.24f,
                0.035f
            ),
            11,
            green,
            TextAnchor.MiddleLeft
        );

        CreateButton(
            parent,
            "OPEN TECHNOLOGY TREE",
            "OPEN TREE",
            new Vector2(
                0.17f,
                0.085f
            ),
            new Vector2(
                0.24f,
                0.045f
            ),
            cyanSoft
        );
    }

    // =========================================================
    // CENTER TECHNOLOGY PANEL
    // =========================================================

    private void CreateCenterTechnologyPanel(
        Transform parent
    )
    {
        CreatePanel(
            parent,
            "CENTER TECHNOLOGY PANEL",
            new Vector2(
                0.505f,
                0.51f
            ),
            new Vector2(
                0.36f,
                0.73f
            ),
            Vector2.zero,
            Vector2.zero,
            panelDark
        );

        AddText(
            parent,
            "TECHNOLOGY TREE // MOBILITY",
            new Vector2(
                0.505f,
                0.825f
            ),
            new Vector2(
                0.31f,
                0.045f
            ),
            18,
            cyan,
            TextAnchor.MiddleLeft
        );

        AddText(
            parent,
            "SELECT A TECHNOLOGY NODE TO VIEW REQUIREMENTS",
            new Vector2(
                0.505f,
                0.785f
            ),
            new Vector2(
                0.31f,
                0.035f
            ),
            11,
            white,
            TextAnchor.MiddleLeft
        );

        CreateTechnologyNode(
            parent,
            "MOBILITY I",
            new Vector2(
                0.39f,
                0.68f
            ),
            green
        );

        CreateTechnologyNode(
            parent,
            "MOBILITY II",
            new Vector2(
                0.50f,
                0.68f
            ),
            cyan
        );

        CreateTechnologyNode(
            parent,
            "MOBILITY III",
            new Vector2(
                0.61f,
                0.68f
            ),
            cyan
        );

        CreateTechnologyNode(
            parent,
            "ADVANCED MOBILITY",
            new Vector2(
                0.50f,
                0.53f
            ),
            yellow
        );

        CreateTechnologyNode(
            parent,
            "AUTONOMOUS MOVEMENT",
            new Vector2(
                0.50f,
                0.38f
            ),
            green
        );

        AddText(
            parent,
            "UNLOCK REQUIREMENTS",
            new Vector2(
                0.505f,
                0.255f
            ),
            new Vector2(
                0.31f,
                0.035f
            ),
            13,
            cyan,
            TextAnchor.MiddleLeft
        );

        AddText(
            parent,
            "• MOBILITY II COMPLETE",
            new Vector2(
                0.505f,
                0.215f
            ),
            new Vector2(
                0.31f,
                0.030f
            ),
            11,
            green,
            TextAnchor.MiddleLeft
        );

        AddText(
            parent,
            "• 12,000 RESEARCH POINTS",
            new Vector2(
                0.505f,
                0.180f
            ),
            new Vector2(
                0.31f,
                0.030f
            ),
            11,
            green,
            TextAnchor.MiddleLeft
        );

        AddText(
            parent,
            "• ADVANCED PROPULSION",
            new Vector2(
                0.505f,
                0.145f
            ),
            new Vector2(
                0.31f,
                0.030f
            ),
            11,
            yellow,
            TextAnchor.MiddleLeft
        );

        CreateButton(
            parent,
            "RESEARCH SELECTED",
            "START RESEARCH",
            new Vector2(
                0.505f,
                0.085f
            ),
            new Vector2(
                0.25f,
                0.050f
            ),
            greenMat.color
        );
    }

    private void CreateTechnologyNode(
        Transform parent,
        string title,
        Vector2 position,
        Color color
    )
    {
        CreateButton(
            parent,
            "TECH " + title,
            title,
            position,
            new Vector2(
                0.095f,
                0.080f
            ),
            color
        );
    }

    // =========================================================
    // RIGHT PANEL
    // =========================================================

    private void CreateRightResearchPanel(
        Transform parent
    )
    {
        CreatePanel(
            parent,
            "RIGHT RESEARCH PANEL",
            new Vector2(
                0.84f,
                0.51f
            ),
            new Vector2(
                0.28f,
                0.73f
            ),
            Vector2.zero,
            Vector2.zero,
            panel
        );

        AddText(
            parent,
            "ACTIVE RESEARCH",
            new Vector2(
                0.84f,
                0.825f
            ),
            new Vector2(
                0.23f,
                0.04f
            ),
            17,
            green,
            TextAnchor.MiddleLeft
        );

        AddText(
            parent,
            "AUTONOMOUS MOVEMENT III",
            new Vector2(
                0.84f,
                0.775f
            ),
            new Vector2(
                0.23f,
                0.04f
            ),
            14,
            white,
            TextAnchor.MiddleLeft
        );

        AddText(
            parent,
            "PROGRESS",
            new Vector2(
                0.84f,
                0.735f
            ),
            new Vector2(
                0.10f,
                0.03f
            ),
            10,
            cyan,
            TextAnchor.MiddleLeft
        );

        CreateProgressBar(
            parent,
            new Vector2(
                0.84f,
                0.695f
            ),
            new Vector2(
                0.22f,
                0.025f
            ),
            0.68f
        );

        AddText(
            parent,
            "68%",
            new Vector2(
                0.84f,
                0.660f
            ),
            new Vector2(
                0.22f,
                0.03f
            ),
            11,
            green,
            TextAnchor.MiddleRight
        );

        AddText(
            parent,
            "COMPLETION  03:42:18",
            new Vector2(
                0.84f,
                0.620f
            ),
            new Vector2(
                0.22f,
                0.03f
            ),
            11,
            white,
            TextAnchor.MiddleLeft
        );

        CreateButton(
            parent,
            "VIEW DETAILS",
            "VIEW DETAILS",
            new Vector2(
                0.84f,
                0.575f
            ),
            new Vector2(
                0.22f,
                0.040f
            ),
            cyanSoft
        );

        AddText(
            parent,
            "RESEARCH PROJECTS",
            new Vector2(
                0.84f,
                0.510f
            ),
            new Vector2(
                0.22f,
                0.035f
            ),
            15,
            cyan,
            TextAnchor.MiddleLeft
        );

        string[] projects =
        {
            "ACTIVE",
            "QUEUED",
            "COMPLETED",
            "AVAILABLE"
        };

        for (int i = 0; i < projects.Length; i++)
        {
            CreateButton(
                parent,
                "PROJECT TAB " + projects[i],
                projects[i],
                new Vector2(
                    0.84f,
                    0.465f -
                    i * 0.040f
                ),
                new Vector2(
                    0.22f,
                    0.032f
                ),
                i == 0
                    ? cyanSoft
                    : panelDark
            );
        }

        AddText(
            parent,
            "RESEARCH RESOURCES",
            new Vector2(
                0.84f,
                0.295f
            ),
            new Vector2(
                0.22f,
                0.035f
            ),
            15,
            yellow,
            TextAnchor.MiddleLeft
        );

        AddText(
            parent,
            "SCIENTISTS       24 / 30",
            new Vector2(
                0.84f,
                0.255f
            ),
            new Vector2(
                0.22f,
                0.03f
            ),
            11,
            white,
            TextAnchor.MiddleLeft
        );

        AddText(
            parent,
            "FUNDING          84,250",
            new Vector2(
                0.84f,
                0.220f
            ),
            new Vector2(
                0.22f,
                0.03f
            ),
            11,
            yellow,
            TextAnchor.MiddleLeft
        );

        AddText(
            parent,
            "MATERIALS        72%",
            new Vector2(
                0.84f,
                0.185f
            ),
            new Vector2(
                0.22f,
                0.03f
            ),
            11,
            white,
            TextAnchor.MiddleLeft
        );

        AddText(
            parent,
            "ENERGY ALLOCATION  91%",
            new Vector2(
                0.84f,
                0.150f
            ),
            new Vector2(
                0.22f,
                0.03f
            ),
            11,
            green,
            TextAnchor.MiddleLeft
        );

        AddText(
            parent,
            "AI RESEARCH",
            new Vector2(
                0.84f,
                0.105f
            ),
            new Vector2(
                0.22f,
                0.03f
            ),
            13,
            green,
            TextAnchor.MiddleLeft
        );

        CreateButton(
            parent,
            "AI AUTONOMY",
            "AUTONOMY",
            new Vector2(
                0.78f,
                0.065f
            ),
            new Vector2(
                0.10f,
                0.030f
            ),
            panelDark
        );

        CreateButton(
            parent,
            "AI PERSONALITY",
            "PERSONALITY",
            new Vector2(
                0.90f,
                0.065f
            ),
            new Vector2(
                0.10f,
                0.030f
            ),
            panelDark
        );
    }

    // =========================================================
    // BOTTOM BAR
    // =========================================================

    private void CreateBottomBar(
        Transform parent
    )
    {
        CreatePanel(
            parent,
            "BOTTOM BAR",
            new Vector2(
                0.5f,
                0.035f
            ),
            new Vector2(
                0.96f,
                0.055f
            ),
            Vector2.zero,
            Vector2.zero,
            panelDark
        );

        string[] links =
        {
            "GARAGE",
            "FINANCE",
            "OPERATIONS",
            "COMMAND CENTER"
        };

        for (int i = 0; i < links.Length; i++)
        {
            CreateButton(
                parent,
                "LINK " + links[i],
                links[i],
                new Vector2(
                    0.12f +
                    i * 0.11f,
                    0.035f
                ),
                new Vector2(
                    0.095f,
                    0.035f
                ),
                panelDark
            );
        }

        AddText(
            parent,
            "RESEARCH NETWORK: ONLINE",
            new Vector2(
                0.78f,
                0.035f
            ),
            new Vector2(
                0.25f,
                0.035f
            ),
            11,
            green,
            TextAnchor.MiddleCenter
        );
    }

    // =========================================================
    // CONFIRMATION
    // =========================================================

    private void CreateConfirmation(
        Transform parent
    )
    {
        CreatePanel(
            parent,
            "RESEARCH CONFIRMATION",
            new Vector2(
                0.5f,
                0.5f
            ),
            new Vector2(
                0.36f,
                0.30f
            ),
            Vector2.zero,
            Vector2.zero,
            new Color(
                0.008f,
                0.020f,
                0.030f,
                0.98f
            )
        );

        AddText(
            parent,
            "RESEARCH CONFIRMATION",
            new Vector2(
                0.5f,
                0.605f
            ),
            new Vector2(
                0.30f,
                0.045f
            ),
            19,
            cyan,
            TextAnchor.MiddleCenter
        );

        AddText(
            parent,
            "START AUTONOMOUS MOVEMENT III?",
            new Vector2(
                0.5f,
                0.555f
            ),
            new Vector2(
                0.30f,
                0.040f
            ),
            14,
            white,
            TextAnchor.MiddleCenter
        );

        AddText(
            parent,
            "COST: 12,000 RESEARCH POINTS",
            new Vector2(
                0.5f,
                0.505f
            ),
            new Vector2(
                0.30f,
                0.035f
            ),
            12,
            yellow,
            TextAnchor.MiddleCenter
        );

        AddText(
            parent,
            "ESTIMATED TIME: 04:30:00",
            new Vector2(
                0.5f,
                0.465f
            ),
            new Vector2(
                0.30f,
                0.035f
            ),
            12,
            white,
            TextAnchor.MiddleCenter
        );

        CreateButton(
            parent,
            "CONFIRM RESEARCH",
            "CONFIRM",
            new Vector2(
                0.44f,
                0.410f
            ),
            new Vector2(
                0.12f,
                0.045f
            ),
            greenMat.color
        );

        CreateButton(
            parent,
            "CANCEL RESEARCH",
            "CANCEL",
            new Vector2(
                0.56f,
                0.410f
            ),
            new Vector2(
                0.12f,
                0.045f
            ),
            panelDark
        );
    }

    // =========================================================
    // UI HELPERS
    // =========================================================

    private GameObject CreatePanel(
        Transform parent,
        string name,
        Vector2 anchor,
        Vector2 size,
        Vector2 offset,
        Vector2 pivot,
        Color color
    )
    {
        GameObject obj =
            new GameObject(
                name
            );

        obj.transform.SetParent(
            parent,
            false
        );

        RectTransform rect =
            obj.AddComponent<RectTransform>();

        rect.anchorMin =
            anchor;

        rect.anchorMax =
            anchor;

        rect.pivot =
            new Vector2(
                0.5f,
                0.5f
            );

        rect.sizeDelta =
            new Vector2(
                size.x * 1920f,
                size.y * 1080f
            );

        rect.anchoredPosition =
            offset;

        Image image =
            obj.AddComponent<Image>();

        image.color =
            color;

        return obj;
    }

    private Text AddText(
        Transform parent,
        string value,
        Vector2 anchor,
        Vector2 size,
        int fontSize,
        Color color,
        TextAnchor alignment
    )
    {
        GameObject obj =
            new GameObject(
                "TEXT " + value
            );

        obj.transform.SetParent(
            parent,
            false
        );

        RectTransform rect =
            obj.AddComponent<RectTransform>();

        rect.anchorMin =
            anchor;

        rect.anchorMax =
            anchor;

        rect.pivot =
            new Vector2(
                0.5f,
                0.5f
            );

        rect.sizeDelta =
            new Vector2(
                size.x * 1920f,
                size.y * 1080f
            );

        Text text =
            obj.AddComponent<Text>();

        text.text =
            value;

        text.font =
            font;

        text.fontSize =
            Mathf.Max(
                8,
                Mathf.RoundToInt(
                    fontSize * 0.62f
                )
            );

        text.color =
            color;

        text.alignment =
            alignment;

        text.horizontalOverflow =
            HorizontalWrapMode.Overflow;

        text.verticalOverflow =
            VerticalWrapMode.Overflow;

        text.resizeTextForBestFit =
            false;

        return text;
    }

    private Button CreateButton(
        Transform parent,
        string objectName,
        string label,
        Vector2 anchor,
        Vector2 size,
        Color color
    )
    {
        GameObject obj =
            new GameObject(
                objectName
            );

        obj.transform.SetParent(
            parent,
            false
        );

        RectTransform rect =
            obj.AddComponent<RectTransform>();

        rect.anchorMin =
            anchor;

        rect.anchorMax =
            anchor;

        rect.pivot =
            new Vector2(
                0.5f,
                0.5f
            );

        rect.sizeDelta =
            new Vector2(
                size.x * 1920f,
                size.y * 1080f
            );

        Image image =
            obj.AddComponent<Image>();

        image.color =
            color;

        Button button =
            obj.AddComponent<Button>();

        ColorBlock colors =
            button.colors;

        colors.normalColor =
            color;

        colors.highlightedColor =
            Color.Lerp(
                color,
                Color.white,
                0.18f
            );

        colors.pressedColor =
            Color.Lerp(
                color,
                Color.black,
                0.18f
            );

        colors.selectedColor =
            colors.highlightedColor;

        button.colors =
            colors;

        Text text =
            AddText(
                obj.transform,
                label,
                new Vector2(
                    0.5f,
                    0.5f
                ),
                new Vector2(
                    0.92f,
                    0.80f
                ),
                13,
                white,
                TextAnchor.MiddleCenter
            );

        text.fontSize =
            9;

        return button;
    }

    private void CreateProgressBar(
        Transform parent,
        Vector2 anchor,
        Vector2 size,
        float progress
    )
    {
        CreatePanel(
            parent,
            "PROGRESS BACKGROUND",
            anchor,
            size,
            Vector2.zero,
            Vector2.zero,
            new Color(
                0.015f,
                0.030f,
                0.040f,
                1f
            )
        );

        CreatePanel(
            parent,
            "PROGRESS VALUE",
            new Vector2(
                anchor.x -
                size.x / 2f +
                (size.x * progress) / 2f,
                anchor.y
            ),
            new Vector2(
                size.x * progress,
                size.y
            ),
            Vector2.zero,
            Vector2.zero,
            green
        );
    }

    // =========================================================
    // WORLD HELPERS
    // =========================================================

    private GameObject CreateCube(
        string name,
        Vector3 position,
        Vector3 scale,
        Material material
    )
    {
        GameObject obj =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        obj.name =
            name;

        obj.transform.position =
            position;

        obj.transform.localScale =
            scale;

        Renderer renderer =
            obj.GetComponent<Renderer>();

        renderer.sharedMaterial =
            material;

        return obj;
    }

    private void CreateAccentLine(
        Vector3 position,
        Vector3 scale
    )
    {
        CreateCube(
            "RESEARCH ACCENT",
            position,
            scale,
            cyanMat
        );
    }

    private void AddWorldText(
        string value,
        Vector3 position,
        float size,
        Color color
    )
    {
        GameObject obj =
            new GameObject(
                "WORLD TEXT " + value
            );

        obj.transform.position =
            position;

        TextMesh text =
            obj.AddComponent<TextMesh>();

        text.text =
            value;

        text.fontSize =
            24;

        text.characterSize =
            size;

        text.color =
            color;

        text.anchor =
            TextAnchor.MiddleCenter;

        text.alignment =
            TextAlignment.Center;
    }

    private void LookAt(
        Transform transform,
        Vector3 target
    )
    {
        transform.rotation =
            Quaternion.LookRotation(
                target -
                transform.position
            );
    }
}
