
using System;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
#endif

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#endif

public class OPAWFinanceOfficeVisualBuilder
{
    private const string SCENE =
        "Assets/Scenes/SCN-07  FINANCE OFFICE/[HUD] FINANCE HUD/Finance_ HUD.unity";

    private const float OFFICE_WIDTH = 70f;
    private const float OFFICE_DEPTH = 55f;
    private const float OFFICE_HEIGHT = 15f;

    private Font font;

    private Color bg =
        new Color(0.007f, 0.012f, 0.020f, 1f);

    private Color floorColor =
        new Color(0.018f, 0.024f, 0.032f, 1f);

    private Color wallColor =
        new Color(0.026f, 0.038f, 0.050f, 1f);

    private Color dark =
        new Color(0.006f, 0.012f, 0.018f, 1f);

    private Color panel =
        new Color(0.025f, 0.042f, 0.058f, 0.97f);

    // Dedicated darker HUD panel color.
    // This fixes all panelDark references in the HUD.
    private Color panelDark =
        new Color(0.012f, 0.022f, 0.032f, 0.98f);

    private Color cyan =
        new Color(0.18f, 0.82f, 1f, 1f);

    private Color cyanDark =
        new Color(0.06f, 0.30f, 0.42f, 1f);

    private Color green =
        new Color(0.24f, 1f, 0.55f, 1f);

    private Color yellow =
        new Color(1f, 0.72f, 0.18f, 1f);

    private Color red =
        new Color(1f, 0.25f, 0.28f, 1f);

    private Color white =
        new Color(0.84f, 0.92f, 0.96f, 1f);

    private Material floorMat;
    private Material wallMat;
    private Material darkMat;
    private Material panelMat;
    private Material cyanMat;
    private Material greenMat;
    private Material yellowMat;
    private Material redMat;

    private Camera financeCamera;

#if UNITY_EDITOR

    [MenuItem("Obsidian Protocol/Build/FINANCE OFFICE - FULL VISUAL")]
    public static void BuildFinanceOffice()
    {
        Debug.Log(
            "FINANCE OFFICE BUILD: START - SCN-07 FINANCE OFFICE"
        );

        EnsureSceneExists();

        Scene scene =
            EditorSceneManager.OpenScene(
                SCENE,
                OpenSceneMode.Single
            );

        if (!scene.IsValid())
        {
            throw new Exception(
                "Unable to open Finance Office scene:\n" +
                SCENE
            );
        }

        GameObject[] roots =
            scene.GetRootGameObjects();

        foreach (GameObject root in roots)
        {
            UnityEngine.Object.DestroyImmediate(root);
        }

        OPAWFinanceOfficeVisualBuilder builder =
            new OPAWFinanceOfficeVisualBuilder();

        builder.Build();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();

        Debug.Log(
            "FINANCE OFFICE BUILD: COMPLETE - EXISTING FINANCE SCENE SAVED"
        );
    }

    private static void EnsureSceneExists()
    {
        string sceneFolder =
            "Assets/Scenes/SCN-07  FINANCE OFFICE";

        string hudFolder =
            sceneFolder + "/[HUD] FINANCE HUD";

        if (!AssetDatabase.IsValidFolder(sceneFolder))
        {
            AssetDatabase.CreateFolder(
                "Assets/Scenes",
                "SCN-07  FINANCE OFFICE"
            );
        }

        if (!AssetDatabase.IsValidFolder(hudFolder))
        {
            AssetDatabase.CreateFolder(
                sceneFolder,
                "[HUD] FINANCE HUD"
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
        font =
            Resources.GetBuiltinResource<Font>(
                "LegacyRuntime.ttf"
            );

        BuildMaterials();
        BuildWorld();
        BuildFinancialOperationsFloor();
        BuildFinancialOverviewCenter();
        BuildBudgetCommandArea();
        BuildTransactionArea();
        BuildInvestmentArea();
        BuildFinancialControls();
        BuildPurchaseSystem();
        BuildDeploymentEconomy();
        BuildFinanceTerminalWall();
        BuildCamera();
        BuildLighting();
        BuildEventSystem();
        BuildHUD();
    }

    // =========================================================
    // MATERIALS
    // =========================================================

    private void BuildMaterials()
    {
        floorMat =
            CreateMaterial(
                "FINANCE FLOOR",
                floorColor
            );

        wallMat =
            CreateMaterial(
                "FINANCE WALL",
                wallColor
            );

        darkMat =
            CreateMaterial(
                "FINANCE DARK",
                dark
            );

        panelMat =
            CreateMaterial(
                "FINANCE PANEL",
                panel
            );

        cyanMat =
            CreateMaterial(
                "FINANCE CYAN",
                cyan
            );

        greenMat =
            CreateMaterial(
                "FINANCE GREEN",
                green
            );

        yellowMat =
            CreateMaterial(
                "FINANCE GOLD",
                yellow
            );

        redMat =
            CreateMaterial(
                "FINANCE RED",
                red
            );
    }

    private Material CreateMaterial(
        string name,
        Color color
    )
    {
        Shader shader =
            Shader.Find(
                "Universal Render Pipeline/Lit"
            );

        if (shader == null)
        {
            shader =
                Shader.Find("Standard");
        }

        Material material =
            new Material(shader);

        material.name =
            name;

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
            "FINANCE OFFICE FLOOR",
            new Vector3(
                0f,
                -0.25f,
                0f
            ),
            new Vector3(
                OFFICE_WIDTH,
                0.5f,
                OFFICE_DEPTH
            ),
            floorMat
        );

        CreateCube(
            "NORTH WALL",
            new Vector3(
                0f,
                OFFICE_HEIGHT / 2f,
                OFFICE_DEPTH / 2f
            ),
            new Vector3(
                OFFICE_WIDTH,
                OFFICE_HEIGHT,
                0.5f
            ),
            wallMat
        );

        CreateCube(
            "SOUTH WALL",
            new Vector3(
                0f,
                OFFICE_HEIGHT / 2f,
                -OFFICE_DEPTH / 2f
            ),
            new Vector3(
                OFFICE_WIDTH,
                OFFICE_HEIGHT,
                0.5f
            ),
            wallMat
        );

        CreateCube(
            "WEST WALL",
            new Vector3(
                -OFFICE_WIDTH / 2f,
                OFFICE_HEIGHT / 2f,
                0f
            ),
            new Vector3(
                0.5f,
                OFFICE_HEIGHT,
                OFFICE_DEPTH
            ),
            wallMat
        );

        CreateCube(
            "EAST WALL",
            new Vector3(
                OFFICE_WIDTH / 2f,
                OFFICE_HEIGHT / 2f,
                0f
            ),
            new Vector3(
                0.5f,
                OFFICE_HEIGHT,
                OFFICE_DEPTH
            ),
            wallMat
        );

        CreateCube(
            "FINANCE CEILING",
            new Vector3(
                0f,
                OFFICE_HEIGHT,
                0f
            ),
            new Vector3(
                OFFICE_WIDTH,
                0.4f,
                OFFICE_DEPTH
            ),
            darkMat
        );

        CreateAccentLine(
            new Vector3(
                0f,
                0.03f,
                -26f
            ),
            new Vector3(
                55f,
                0.05f,
                0.08f
            )
        );

        CreateAccentLine(
            new Vector3(
                0f,
                0.03f,
                26f
            ),
            new Vector3(
                55f,
                0.05f,
                0.08f
            )
        );

        AddWorldText(
            "OBSIDIAN PROTOCOL",
            new Vector3(
                0f,
                11f,
                26.5f
            ),
            0.12f,
            cyan
        );

        AddWorldText(
            "FINANCE OFFICE",
            new Vector3(
                0f,
                9.7f,
                26.4f
            ),
            0.085f,
            white
        );

        AddWorldText(
            "ECONOMY // RESOURCE CONTROL // FLEET FINANCE",
            new Vector3(
                0f,
                8.6f,
                26.3f
            ),
            0.052f,
            cyan
        );
    }

    // =========================================================
    // FINANCIAL OPERATIONS FLOOR
    // =========================================================

    private void BuildFinancialOperationsFloor()
    {
        CreatePlatform(
            "FINANCIAL OPERATIONS",
            new Vector3(
                0f,
                0.15f,
                1f
            ),
            new Vector3(
                32f,
                0.3f,
                20f
            )
        );

        AddWorldText(
            "FINANCIAL OPERATIONS",
            new Vector3(
                0f,
                5.7f,
                8.5f
            ),
            0.065f,
            cyan
        );

        CreateDesk(
            "FINANCE DESK A",
            new Vector3(
                -11f,
                0f,
                0f
            )
        );

        CreateDesk(
            "FINANCE DESK B",
            new Vector3(
                0f,
                0f,
                0f
            )
        );

        CreateDesk(
            "FINANCE DESK C",
            new Vector3(
                11f,
                0f,
                0f
            )
        );
    }

    private void CreateDesk(
        string name,
        Vector3 position
    )
    {
        CreateCube(
            name,
            position +
            new Vector3(
                0f,
                1.2f,
                0f
            ),
            new Vector3(
                7f,
                2.4f,
                3f
            ),
            darkMat
        );

        CreateCube(
            name + " SCREEN",
            position +
            new Vector3(
                0f,
                2.6f,
                -1.55f
            ),
            new Vector3(
                5.5f,
                2f,
                0.08f
            ),
            cyanMat
        );

        CreateCube(
            name + " STATUS",
            position +
            new Vector3(
                0f,
                0.55f,
                -1.6f
            ),
            new Vector3(
                4f,
                0.15f,
                0.08f
            ),
            greenMat
        );

        AddWorldText(
            name,
            position +
            new Vector3(
                0f,
                4f,
                0f
            ),
            0.042f,
            white
        );
    }

    // =========================================================
    // FINANCIAL OVERVIEW CENTER
    // =========================================================

    private void BuildFinancialOverviewCenter()
    {
        CreateCube(
            "FINANCIAL OVERVIEW WALL",
            new Vector3(
                0f,
                5f,
                24.8f
            ),
            new Vector3(
                27f,
                9f,
                0.4f
            ),
            darkMat
        );

        string[] displays =
        {
            "CURRENT CREDITS",
            "INCOME RATE",
            "EXPENDITURE RATE",
            "NET BALANCE",
            "FORECAST"
        };

        for (int i = 0; i < displays.Length; i++)
        {
            float x =
                -10.5f +
                i * 5.25f;

            CreateFinancialDisplay(
                displays[i],
                new Vector3(
                    x,
                    5f,
                    24.45f
                ),
                i == 3
                    ? greenMat
                    : cyanMat
            );
        }

        AddWorldText(
            "FINANCIAL OVERVIEW",
            new Vector3(
                0f,
                10.5f,
                24.1f
            ),
            0.065f,
            cyan
        );
    }

    private void CreateFinancialDisplay(
        string title,
        Vector3 position,
        Material screen
    )
    {
        CreateCube(
            title + " DISPLAY",
            position,
            new Vector3(
                4.4f,
                4.2f,
                0.15f
            ),
            darkMat
        );

        CreateCube(
            title + " SCREEN",
            position +
            new Vector3(
                0f,
                0f,
                -0.1f
            ),
            new Vector3(
                3.5f,
                2.4f,
                0.08f
            ),
            screen
        );

        AddWorldText(
            title,
            position +
            new Vector3(
                0f,
                1.8f,
                -0.25f
            ),
            0.038f,
            white
        );
    }

    // =========================================================
    // BUDGET COMMAND AREA
    // =========================================================

    private void BuildBudgetCommandArea()
    {
        CreateArea(
            "BUDGET ALLOCATION",
            new Vector3(
                -20f,
                0f,
                15f
            ),
            new Vector3(
                15f,
                9f,
                10f
            )
        );

        string[] budgets =
        {
            "OPERATIONS",
            "RESEARCH",
            "LOGISTICS",
            "FLEET MAINTENANCE",
            "EXPERIMENTAL"
        };

        for (int i = 0; i < budgets.Length; i++)
        {
            float z =
                18f -
                i * 1.8f;

            CreateCube(
                "BUDGET " + budgets[i],
                new Vector3(
                    -20f,
                    1.2f,
                    z
                ),
                new Vector3(
                    11f,
                    1.1f,
                    1.2f
                ),
                darkMat
            );

            CreateCube(
                "BUDGET BAR " + budgets[i],
                new Vector3(
                    -20f,
                    1.75f,
                    z - 0.58f
                ),
                new Vector3(
                    8f,
                    0.18f,
                    0.08f
                ),
                cyanMat
            );

            AddWorldText(
                budgets[i],
                new Vector3(
                    -20f,
                    2.1f,
                    z
                ),
                0.035f,
                white
            );
        }
    }

    // =========================================================
    // TRANSACTIONS
    // =========================================================

    private void BuildTransactionArea()
    {
        CreateArea(
            "TRANSACTIONS",
            new Vector3(
                20f,
                0f,
                15f
            ),
            new Vector3(
                15f,
                9f,
                10f
            )
        );

        string[] transactions =
        {
            "+14,000 CREDITS",
            "-2,400 FABRICATION",
            "-850 REPAIRS",
            "-1,200 RESEARCH",
            "+6,500 CREDIT PACK"
        };

        for (int i = 0; i < transactions.Length; i++)
        {
            float z =
                18f -
                i * 1.8f;

            CreateCube(
                "TRANSACTION " + i,
                new Vector3(
                    20f,
                    1.1f,
                    z
                ),
                new Vector3(
                    11f,
                    1.1f,
                    1.2f
                ),
                darkMat
            );

            AddWorldText(
                transactions[i],
                new Vector3(
                    20f,
                    1.6f,
                    z
                ),
                0.038f,
                i == 0 || i == 4
                    ? green
                    : yellow
            );
        }
    }

    // =========================================================
    // INVESTMENTS
    // =========================================================

    private void BuildInvestmentArea()
    {
        CreateArea(
            "INVESTMENTS",
            new Vector3(
                -20f,
                0f,
                -16f
            ),
            new Vector3(
                15f,
                8f,
                10f
            )
        );

        string[] investments =
        {
            "ACTIVE INVESTMENTS",
            "ROI",
            "RISK LEVEL",
            "FLEET EXPANSION"
        };

        for (int i = 0; i < investments.Length; i++)
        {
            float z =
                -12f -
                i * 2f;

            CreateCube(
                "INVESTMENT " + investments[i],
                new Vector3(
                    -20f,
                    1.2f,
                    z
                ),
                new Vector3(
                    11f,
                    1.3f,
                    1.4f
                ),
                darkMat
            );

            CreateCube(
                "INVESTMENT SCREEN " + i,
                new Vector3(
                    -20f,
                    2f,
                    z - 0.72f
                ),
                new Vector3(
                    8f,
                    0.12f,
                    0.08f
                ),
                i == 2
                    ? yellowMat
                    : greenMat
            );

            AddWorldText(
                investments[i],
                new Vector3(
                    -20f,
                    2f,
                    z
                ),
                0.038f,
                white
            );
        }
    }

    // =========================================================
    // FINANCIAL CONTROLS
    // =========================================================

    private void BuildFinancialControls()
    {
        CreateArea(
            "FINANCIAL CONTROLS",
            new Vector3(
                20f,
                0f,
                -16f
            ),
            new Vector3(
                15f,
                8f,
                10f
            )
        );

        string[] controls =
        {
            "ADJUST BUDGET",
            "APPROVE EXPENDITURE",
            "AUDIT",
            "FORECAST"
        };

        for (int i = 0; i < controls.Length; i++)
        {
            float z =
                -12f -
                i * 2f;

            CreateCube(
                "CONTROL " + controls[i],
                new Vector3(
                    20f,
                    1.4f,
                    z
                ),
                new Vector3(
                    11f,
                    1.5f,
                    1.6f
                ),
                darkMat
            );

            CreateCube(
                "CONTROL LIGHT " + i,
                new Vector3(
                    14.7f,
                    1.4f,
                    z
                ),
                new Vector3(
                    0.12f,
                    1.0f,
                    0.8f
                ),
                i == 2
                    ? yellowMat
                    : cyanMat
            );

            AddWorldText(
                controls[i],
                new Vector3(
                    20f,
                    2.1f,
                    z
                ),
                0.040f,
                white
            );
        }
    }

    // =========================================================
    // PURCHASE SYSTEM
    // =========================================================

    private void BuildPurchaseSystem()
    {
        CreateCube(
            "PURCHASE SYSTEM WALL",
            new Vector3(
                -26f,
                5f,
                -1f
            ),
            new Vector3(
                0.5f,
                9f,
                14f
            ),
            darkMat
        );

        string[] purchaseTypes =
        {
            "UNIT PURCHASES",
            "EQUIPMENT PURCHASES",
            "CUSTOMIZATION",
            "CONVENIENCE"
        };

        for (int i = 0; i < purchaseTypes.Length; i++)
        {
            float z =
                -7f +
                i * 4f;

            CreateCube(
                "PURCHASE " + purchaseTypes[i],
                new Vector3(
                    -25.5f,
                    4f,
                    z
                ),
                new Vector3(
                    0.12f,
                    2.2f,
                    3f
                ),
                yellowMat
            );

            AddWorldText(
                purchaseTypes[i],
                new Vector3(
                    -25.1f,
                    4f,
                    z
                ),
                0.038f,
                white
            );
        }

        AddWorldText(
            "PURCHASE SYSTEM",
            new Vector3(
                -25f,
                10.5f,
                0f
            ),
            0.060f,
            yellow
        );
    }

    // =========================================================
    // DEPLOYMENT ECONOMY
    // =========================================================

    private void BuildDeploymentEconomy()
    {
        CreateCube(
            "DEPLOYMENT ECONOMY WALL",
            new Vector3(
                26f,
                5f,
                2f
            ),
            new Vector3(
                0.5f,
                9f,
                17f
            ),
            darkMat
        );

        string[] displays =
        {
            "UNIT DEPLOYMENT COST",
            "DEPLOYMENT BUDGET",
            "AVAILABLE DEPLOYMENT",
            "COMPETITIVE RESTRICTIONS"
        };

        for (int i = 0; i < displays.Length; i++)
        {
            float z =
                -5f +
                i * 4.5f;

            CreateCube(
                "DEPLOYMENT " + displays[i],
                new Vector3(
                    25.5f,
                    4.5f,
                    z
                ),
                new Vector3(
                    0.12f,
                    3.2f,
                    3.5f
                ),
                i == 3
                    ? redMat
                    : greenMat
            );

            AddWorldText(
                displays[i],
                new Vector3(
                    25.1f,
                    4.5f,
                    z
                ),
                0.038f,
                white
            );
        }

        AddWorldText(
            "DEPLOYMENT ECONOMY",
            new Vector3(
                25f,
                10.5f,
                2f
            ),
            0.060f,
            green
        );
    }

    // =========================================================
    // FINANCE TERMINAL WALL
    // =========================================================

    private void BuildFinanceTerminalWall()
    {
        CreateCube(
            "FINANCE TERMINAL WALL",
            new Vector3(
                0f,
                5f,
                -26f
            ),
            new Vector3(
                32f,
                9f,
                0.4f
            ),
            darkMat
        );

        string[] terminals =
        {
            "CREDITS",
            "RESOURCE VALUE",
            "MANUFACTURING COST",
            "MAINTENANCE COST",
            "RESEARCH COST",
            "FLEET VALUE"
        };

        for (int i = 0; i < terminals.Length; i++)
        {
            float x =
                -13f +
                i * 5.2f;

            CreateCube(
                "FINANCE TERMINAL " + terminals[i],
                new Vector3(
                    x,
                    4.8f,
                    -25.7f
                ),
                new Vector3(
                    4.3f,
                    5f,
                    0.12f
                ),
                darkMat
            );

            CreateCube(
                "FINANCE SCREEN " + terminals[i],
                new Vector3(
                    x,
                    5.1f,
                    -25.55f
                ),
                new Vector3(
                    3.4f,
                    2.5f,
                    0.08f
                ),
                i == 0
                    ? yellowMat
                    : cyanMat
            );

            AddWorldText(
                terminals[i],
                new Vector3(
                    x,
                    2.7f,
                    -25.5f
                ),
                0.035f,
                white
            );
        }

        AddWorldText(
            "ECONOMIC COMMAND NETWORK",
            new Vector3(
                0f,
                10.5f,
                -25.2f
            ),
            0.060f,
            cyan
        );
    }

    // =========================================================
    // CAMERA
    // =========================================================

    private void BuildCamera()
    {
        GameObject obj =
            new GameObject(
                "FINANCE OFFICE CAMERA"
            );

        financeCamera =
            obj.AddComponent<Camera>();

        financeCamera.tag =
            "MainCamera";

        financeCamera.enabled =
            true;

        financeCamera.clearFlags =
            CameraClearFlags.SolidColor;

        financeCamera.backgroundColor =
            bg;

        financeCamera.fieldOfView =
            68f;

        financeCamera.nearClipPlane =
            0.05f;

        financeCamera.farClipPlane =
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
                3f
            )
        );
    }

    // =========================================================
    // LIGHTING
    // =========================================================

    private void BuildLighting()
    {
        GameObject mainLightObject =
            new GameObject(
                "FINANCE MAIN LIGHT"
            );

        Light mainLight =
            mainLightObject.AddComponent<Light>();

        mainLight.type =
            LightType.Directional;

        mainLight.intensity =
            0.75f;

        mainLight.color =
            new Color(
                0.74f,
                0.84f,
                1f
            );

        mainLightObject.transform.rotation =
            Quaternion.Euler(
                45f,
                -30f,
                0f
            );

        for (int i = 0; i < 6; i++)
        {
            GameObject lightObject =
                new GameObject(
                    "FINANCE CEILING LIGHT " + i
                );

            Light light =
                lightObject.AddComponent<Light>();

            light.type =
                LightType.Point;

            light.range =
                17f;

            light.intensity =
                2.8f;

            light.color =
                new Color(
                    0.35f,
                    0.72f,
                    1f
                );

            lightObject.transform.position =
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
        GameObject canvasObject =
            new GameObject(
                "FINANCE HUD"
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

        CreateHeader(
            canvasObject.transform
        );

        CreateLeftPanel(
            canvasObject.transform
        );

        CreateCenterPanel(
            canvasObject.transform
        );

        CreateRightPanel(
            canvasObject.transform
        );

        CreateBottomPanel(
            canvasObject.transform
        );

        CreatePurchaseConfirmation(
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
            new Color(
                0.004f,
                0.009f,
                0.015f,
                0.84f
            )
        );
    }

    // =========================================================
    // HEADER
    // =========================================================

    private void CreateHeader(
        Transform parent
    )
    {
        CreatePanel(
            parent,
            "FINANCE HEADER",
            new Vector2(
                0.5f,
                0.945f
            ),
            new Vector2(
                0.96f,
                0.085f
            ),
            new Color(
                0.010f,
                0.025f,
                0.038f,
                0.98f
            )
        );

        AddText(
            parent,
            "13. ECONOMY",
            new Vector2(
                0.04f,
                0.945f
            ),
            new Vector2(
                0.20f,
                0.05f
            ),
            25,
            cyan,
            TextAnchor.MiddleLeft
        );

        AddText(
            parent,
            "FINANCE OFFICE // ECONOMIC COMMAND",
            new Vector2(
                0.50f,
                0.945f
            ),
            new Vector2(
                0.34f,
                0.04f
            ),
            16,
            white,
            TextAnchor.MiddleCenter
        );

        AddText(
            parent,
            "CREDITS  84,250",
            new Vector2(
                0.73f,
                0.945f
            ),
            new Vector2(
                0.12f,
                0.04f
            ),
            14,
            yellow,
            TextAnchor.MiddleCenter
        );

        AddText(
            parent,
            "NET +2,840",
            new Vector2(
                0.84f,
                0.945f
            ),
            new Vector2(
                0.10f,
                0.04f
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
                0.945f
            ),
            new Vector2(
                0.07f,
                0.04f
            ),
            12,
            green,
            TextAnchor.MiddleCenter
        );
    }

    // =========================================================
    // LEFT PANEL
    // =========================================================

    private void CreateLeftPanel(
        Transform parent
    )
    {
        CreatePanel(
            parent,
            "ECONOMY SYSTEMS",
            new Vector2(
                0.16f,
                0.51f
            ),
            new Vector2(
                0.27f,
                0.73f
            ),
            panel
        );

        AddText(
            parent,
            "ECONOMY SYSTEMS",
            new Vector2(
                0.16f,
                0.825f
            ),
            new Vector2(
                0.22f,
                0.04f
            ),
            18,
            cyan,
            TextAnchor.MiddleLeft
        );

        string[] systems =
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

        for (int i = 0; i < systems.Length; i++)
        {
            CreateButton(
                parent,
                "SYSTEM " + systems[i],
                systems[i],
                new Vector2(
                    0.16f,
                    0.765f -
                    i * 0.060f
                ),
                new Vector2(
                    0.22f,
                    0.044f
                ),
                i == 0
                    ? cyanDark
                    : panelDark
            );
        }

        AddText(
            parent,
            "ECONOMIC STATUS",
            new Vector2(
                0.16f,
                0.235f
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
            "CURRENT CREDITS       84,250",
            new Vector2(
                0.16f,
                0.195f
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
            "INCOME RATE          +4,920",
            new Vector2(
                0.16f,
                0.160f
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
            "EXPENDITURE RATE     -2,080",
            new Vector2(
                0.16f,
                0.125f
            ),
            new Vector2(
                0.22f,
                0.03f
            ),
            11,
            red,
            TextAnchor.MiddleLeft
        );

        AddText(
            parent,
            "NET BALANCE          +2,840",
            new Vector2(
                0.16f,
                0.090f
            ),
            new Vector2(
                0.22f,
                0.03f
            ),
            11,
            green,
            TextAnchor.MiddleLeft
        );
    }

    // =========================================================
    // CENTER PANEL
    // =========================================================

    private void CreateCenterPanel(
        Transform parent
    )
    {
        CreatePanel(
            parent,
            "FINANCIAL OVERVIEW",
            new Vector2(
                0.505f,
                0.52f
            ),
            new Vector2(
                0.40f,
                0.70f
            ),
            panelDark
        );

        AddText(
            parent,
            "FINANCIAL OVERVIEW",
            new Vector2(
                0.505f,
                0.825f
            ),
            new Vector2(
                0.34f,
                0.04f
            ),
            19,
            cyan,
            TextAnchor.MiddleLeft
        );

        AddFinancialCard(
            parent,
            "CURRENT CREDITS",
            "84,250",
            new Vector2(
                0.405f,
                0.735f
            ),
            yellow
        );

        AddFinancialCard(
            parent,
            "INCOME RATE",
            "+4,920 / HR",
            new Vector2(
                0.605f,
                0.735f
            ),
            green
        );

        AddFinancialCard(
            parent,
            "EXPENDITURE RATE",
            "-2,080 / HR",
            new Vector2(
                0.405f,
                0.610f
            ),
            red
        );

        AddFinancialCard(
            parent,
            "NET BALANCE",
            "+2,840 / HR",
            new Vector2(
                0.605f,
                0.610f
            ),
            green
        );

        AddText(
            parent,
            "BUDGET ALLOCATION",
            new Vector2(
                0.505f,
                0.505f
            ),
            new Vector2(
                0.34f,
                0.035f
            ),
            15,
            cyan,
            TextAnchor.MiddleLeft
        );

        string[] budgetLabels =
        {
            "OPERATIONS",
            "RESEARCH",
            "LOGISTICS",
            "FLEET MAINTENANCE",
            "EXPERIMENTAL"
        };

        float[] budgetValues =
        {
            0.80f,
            0.62f,
            0.54f,
            0.72f,
            0.28f
        };

        for (int i = 0; i < budgetLabels.Length; i++)
        {
            float y =
                0.455f -
                i * 0.055f;

            AddText(
                parent,
                budgetLabels[i],
                new Vector2(
                    0.415f,
                    y
                ),
                new Vector2(
                    0.15f,
                    0.03f
                ),
                10,
                white,
                TextAnchor.MiddleLeft
            );

            CreateProgressBar(
                parent,
                new Vector2(
                    0.555f,
                    y
                ),
                new Vector2(
                    0.20f,
                    0.020f
                ),
                budgetValues[i],
                i == 4
                    ? yellow
                    : cyan
            );

            AddText(
                parent,
                Mathf.RoundToInt(
                    budgetValues[i] * 100f
                ) + "%",
                new Vector2(
                    0.675f,
                    y
                ),
                new Vector2(
                    0.055f,
                    0.03f
                ),
                10,
                white,
                TextAnchor.MiddleRight
            );
        }

        AddText(
            parent,
            "TRANSACTIONS",
            new Vector2(
                0.505f,
                0.175f
            ),
            new Vector2(
                0.34f,
                0.035f
            ),
            15,
            cyan,
            TextAnchor.MiddleLeft
        );

        AddText(
            parent,
            "+14,000   CREDIT PACK",
            new Vector2(
                0.505f,
                0.135f
            ),
            new Vector2(
                0.34f,
                0.03f
            ),
            11,
            green,
            TextAnchor.MiddleLeft
        );

        AddText(
            parent,
            "-2,400    FABRICATION",
            new Vector2(
                0.505f,
                0.100f
            ),
            new Vector2(
                0.34f,
                0.03f
            ),
            11,
            red,
            TextAnchor.MiddleLeft
        );

        AddText(
            parent,
            "-850      REPAIRS",
            new Vector2(
                0.505f,
                0.065f
            ),
            new Vector2(
                0.34f,
                0.03f
            ),
            11,
            red,
            TextAnchor.MiddleLeft
        );
    }

    private void AddFinancialCard(
        Transform parent,
        string title,
        string value,
        Vector2 position,
        Color accent
    )
    {
        CreatePanel(
            parent,
            "FINANCIAL CARD " + title,
            position,
            new Vector2(
                0.17f,
                0.105f
            ),
            panel
        );

        AddText(
            parent,
            title,
            position +
            new Vector2(
                0f,
                0.022f
            ),
            new Vector2(
                0.15f,
                0.025f
            ),
            9,
            white,
            TextAnchor.MiddleCenter
        );

        AddText(
            parent,
            value,
            position -
            new Vector2(
                0f,
                0.025f
            ),
            new Vector2(
                0.15f,
                0.035f
            ),
            16,
            accent,
            TextAnchor.MiddleCenter
        );
    }

    // =========================================================
    // RIGHT PANEL
    // =========================================================

    private void CreateRightPanel(
        Transform parent
    )
    {
        CreatePanel(
            parent,
            "FINANCE CONTROL PANEL",
            new Vector2(
                0.845f,
                0.52f
            ),
            new Vector2(
                0.27f,
                0.70f
            ),
            panel
        );

        AddText(
            parent,
            "INVESTMENTS",
            new Vector2(
                0.845f,
                0.825f
            ),
            new Vector2(
                0.22f,
                0.04f
            ),
            17,
            yellow,
            TextAnchor.MiddleLeft
        );

        AddText(
            parent,
            "ACTIVE INVESTMENTS     06",
            new Vector2(
                0.845f,
                0.775f
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
            "ROI                     +18.4%",
            new Vector2(
                0.845f,
                0.735f
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
            "RISK LEVEL              LOW",
            new Vector2(
                0.845f,
                0.695f
            ),
            new Vector2(
                0.22f,
                0.03f
            ),
            11,
            green,
            TextAnchor.MiddleLeft
        );

        CreateButton(
            parent,
            "MANAGE INVESTMENTS",
            "MANAGE INVESTMENTS",
            new Vector2(
                0.845f,
                0.645f
            ),
            new Vector2(
                0.22f,
                0.042f
            ),
            cyanDark
        );

        AddText(
            parent,
            "FINANCIAL CONTROLS",
            new Vector2(
                0.845f,
                0.575f
            ),
            new Vector2(
                0.22f,
                0.035f
            ),
            15,
            cyan,
            TextAnchor.MiddleLeft
        );

        string[] controls =
        {
            "ADJUST BUDGET",
            "APPROVE EXPENDITURE",
            "AUDIT",
            "FORECAST"
        };

        for (int i = 0; i < controls.Length; i++)
        {
            CreateButton(
                parent,
                "CONTROL " + controls[i],
                controls[i],
                new Vector2(
                    0.845f,
                    0.525f -
                    i * 0.050f
                ),
                new Vector2(
                    0.22f,
                    0.038f
                ),
                panelDark
            );
        }

        AddText(
            parent,
            "DEPLOYMENT ECONOMY",
            new Vector2(
                0.845f,
                0.305f
            ),
            new Vector2(
                0.22f,
                0.035f
            ),
            15,
            green,
            TextAnchor.MiddleLeft
        );

        AddText(
            parent,
            "BATTLE BUDGET",
            new Vector2(
                0.845f,
                0.255f
            ),
            new Vector2(
                0.12f,
                0.03f
            ),
            10,
            white,
            TextAnchor.MiddleLeft
        );

        AddText(
            parent,
            "10,000 DP",
            new Vector2(
                0.91f,
                0.255f
            ),
            new Vector2(
                0.09f,
                0.03f
            ),
            12,
            green,
            TextAnchor.MiddleRight
        );

        AddText(
            parent,
            "AVAILABLE DEPLOYMENT",
            new Vector2(
                0.845f,
                0.210f
            ),
            new Vector2(
                0.16f,
                0.03f
            ),
            10,
            white,
            TextAnchor.MiddleLeft
        );

        AddText(
            parent,
            "10,000 DP",
            new Vector2(
                0.91f,
                0.210f
            ),
            new Vector2(
                0.09f,
                0.03f
            ),
            12,
            green,
            TextAnchor.MiddleRight
        );

        AddText(
            parent,
            "COMPETITIVE POWER LIMIT",
            new Vector2(
                0.845f,
                0.165f
            ),
            new Vector2(
                0.16f,
                0.03f
            ),
            10,
            white,
            TextAnchor.MiddleLeft
        );

        AddText(
            parent,
            "ENFORCED",
            new Vector2(
                0.91f,
                0.165f
            ),
            new Vector2(
                0.09f,
                0.03f
            ),
            11,
            green,
            TextAnchor.MiddleRight
        );

        CreateButton(
            parent,
            "OPEN DEPLOYMENT ECONOMY",
            "VIEW DEPLOYMENT",
            new Vector2(
                0.845f,
                0.105f
            ),
            new Vector2(
                0.22f,
                0.042f
            ),
            green
        );
    }

    // =========================================================
    // BOTTOM PANEL
    // =========================================================

    private void CreateBottomPanel(
        Transform parent
    )
    {
        CreatePanel(
            parent,
            "FINANCE BOTTOM BAR",
            new Vector2(
                0.5f,
                0.035f
            ),
            new Vector2(
                0.96f,
                0.055f
            ),
            panelDark
        );

        string[] links =
        {
            "LOGISTICS",
            "RESEARCH",
            "OPERATIONS",
            "COMMAND CENTER"
        };

        for (int i = 0; i < links.Length; i++)
        {
            CreateButton(
                parent,
                "FINANCE LINK " + links[i],
                links[i],
                new Vector2(
                    0.11f +
                    i * 0.115f,
                    0.035f
                ),
                new Vector2(
                    0.10f,
                    0.035f
                ),
                panelDark
            );
        }

        AddText(
            parent,
            "ECONOMY NETWORK: ONLINE",
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
    // PURCHASE CONFIRMATION
    // =========================================================

    private void CreatePurchaseConfirmation(
        Transform parent
    )
    {
        CreatePanel(
            parent,
            "PURCHASE CONFIRMATION",
            new Vector2(
                0.5f,
                0.5f
            ),
            new Vector2(
                0.36f,
                0.31f
            ),
            new Color(
                0.008f,
                0.018f,
                0.028f,
                0.98f
            )
        );

        AddText(
            parent,
            "PURCHASE CONFIRMATION",
            new Vector2(
                0.5f,
                0.605f
            ),
            new Vector2(
                0.30f,
                0.04f
            ),
            19,
            yellow,
            TextAnchor.MiddleCenter
        );

        AddText(
            parent,
            "SELECTED PURCHASE",
            new Vector2(
                0.5f,
                0.555f
            ),
            new Vector2(
                0.30f,
                0.035f
            ),
            11,
            cyan,
            TextAnchor.MiddleCenter
        );

        AddText(
            parent,
            "ADVANCED FABRICATION PACKAGE",
            new Vector2(
                0.5f,
                0.510f
            ),
            new Vector2(
                0.30f,
                0.035f
            ),
            14,
            white,
            TextAnchor.MiddleCenter
        );

        AddText(
            parent,
            "COST: 2,400 CREDITS",
            new Vector2(
                0.5f,
                0.465f
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
            "REMAINING: 81,850 CREDITS",
            new Vector2(
                0.5f,
                0.425f
            ),
            new Vector2(
                0.30f,
                0.035f
            ),
            11,
            green,
            TextAnchor.MiddleCenter
        );

        CreateButton(
            parent,
            "CONFIRM PURCHASE",
            "CONFIRM",
            new Vector2(
                0.44f,
                0.370f
            ),
            new Vector2(
                0.12f,
                0.045f
            ),
            green
        );

        CreateButton(
            parent,
            "CANCEL PURCHASE",
            "CANCEL",
            new Vector2(
                0.56f,
                0.370f
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
        Color color
    )
    {
        GameObject obj =
            new GameObject(name);

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
                    fontSize * 0.60f
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
        string name,
        string label,
        Vector2 anchor,
        Vector2 size,
        Color color
    )
    {
        GameObject obj =
            new GameObject(name);

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
        float progress,
        Color color
    )
    {
        CreatePanel(
            parent,
            "PROGRESS BACKGROUND",
            anchor,
            size,
            new Color(
                0.012f,
                0.025f,
                0.035f,
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
            color
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

    private void CreatePlatform(
        string name,
        Vector3 position,
        Vector3 scale
    )
    {
        CreateCube(
            name,
            position,
            scale,
            panelMat
        );
    }

    private void CreateArea(
        string name,
        Vector3 position,
        Vector3 scale
    )
    {
        CreateCube(
            name + " PLATFORM",
            position +
            new Vector3(
                0f,
                0.2f,
                0f
            ),
            new Vector3(
                scale.x,
                0.4f,
                scale.z
            ),
            panelMat
        );

        CreateCube(
            name + " BACK WALL",
            position +
            new Vector3(
                0f,
                scale.y / 2f,
                scale.z / 2f
            ),
            new Vector3(
                scale.x,
                scale.y,
                0.25f
            ),
            darkMat
        );

        AddWorldText(
            name,
            position +
            new Vector3(
                0f,
                scale.y + 0.5f,
                scale.z / 2f - 0.5f
            ),
            0.050f,
            cyan
        );
    }

    private void CreateAccentLine(
        Vector3 position,
        Vector3 scale
    )
    {
        CreateCube(
            "FINANCE ACCENT",
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
