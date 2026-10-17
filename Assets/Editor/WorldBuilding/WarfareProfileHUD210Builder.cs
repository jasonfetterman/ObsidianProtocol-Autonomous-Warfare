using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public static class WarfareProfileHUD210Builder
{
    private const string ScenePath =
        "Assets/Scenes/SCN69_WARFARE_PROFILE/Warfare_Profile.unity";

    private static readonly Color PanelColor =
        new Color(0.045f, 0.035f, 0.055f, 0.95f);

    private static readonly Color TextColor =
        new Color(0.90f, 0.92f, 0.96f, 1f);

    private static readonly Color MutedColor =
        new Color(0.58f, 0.62f, 0.70f, 1f);

    private static readonly Color AccentColor =
        new Color(0.70f, 0.28f, 0.86f, 1f);

    [MenuItem("Obsidian Protocol/Worlds/BUILD HUD-210 WARFARE PROFILE")]
    public static void Build()
    {
        EnsureFolders();

        EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene,
            NewSceneMode.Single);

        BuildWarfareEnvironment();
        BuildCamera();
        BuildEventSystem();
        BuildHUD();

        EditorSceneManager.SaveScene(
            SceneManager.GetActiveScene(),
            ScenePath);

        AddToBuildSettings();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "HUD-210 Warfare Profile built successfully: " +
            ScenePath);
    }

    private static void EnsureFolders()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Scenes"))
            AssetDatabase.CreateFolder("Assets", "Scenes");

        if (!AssetDatabase.IsValidFolder(
                "Assets/Scenes/SCN69_WARFARE_PROFILE"))
        {
            AssetDatabase.CreateFolder(
                "Assets/Scenes",
                "SCN69_WARFARE_PROFILE");
        }
    }

    private static void BuildWarfareEnvironment()
    {
        GameObject root =
            new GameObject("WARFARE PROFILE ENVIRONMENT");

        GameObject ground =
            GameObject.CreatePrimitive(PrimitiveType.Plane);

        ground.name = "WARFARE TERRAIN";

        ground.transform.SetParent(
            root.transform,
            false);

        ground.transform.localScale =
            new Vector3(11f, 1f, 11f);

        Renderer groundRenderer =
            ground.GetComponent<Renderer>();

        if (groundRenderer != null)
        {
            groundRenderer.sharedMaterial =
                MakeMaterial(
                    "MAT_WARFARE_GROUND",
                    new Color(
                        0.075f,
                        0.065f,
                        0.085f,
                        1f));
        }

        for (int i = 0; i < 20; i++)
        {
            float angle =
                i * Mathf.PI * 2f / 20f;

            float radius =
                2.5f + (i % 5) * 1.5f;

            GameObject position =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube);

            position.name =
                "DEFENSIVE_POSITION_" +
                (i + 1).ToString("00");

            position.transform.SetParent(
                root.transform,
                false);

            position.transform.position =
                new Vector3(
                    Mathf.Cos(angle) * radius,
                    0.25f,
                    Mathf.Sin(angle) * radius);

            position.transform.localScale =
                new Vector3(
                    0.65f,
                    0.5f,
                    0.65f);

            Renderer renderer =
                position.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.sharedMaterial =
                    MakeMaterial(
                        "MAT_DEFENSIVE_" + i,
                        new Color(
                            0.22f,
                            0.15f,
                            0.28f,
                            1f));
            }
        }

        for (int i = 0; i < 12; i++)
        {
            float angle =
                i * Mathf.PI * 2f / 12f +
                0.2f;

            GameObject threat =
                GameObject.CreatePrimitive(
                    PrimitiveType.Sphere);

            threat.name =
                "WARFARE_ZONE_" +
                (i + 1).ToString("00");

            threat.transform.SetParent(
                root.transform,
                false);

            threat.transform.position =
                new Vector3(
                    Mathf.Cos(angle) * 6.8f,
                    0.18f,
                    Mathf.Sin(angle) * 6.8f);

            threat.transform.localScale =
                Vector3.one * 0.65f;

            Renderer renderer =
                threat.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.sharedMaterial =
                    MakeMaterial(
                        "MAT_WARFARE_ZONE_" + i,
                        new Color(
                            0.55f,
                            0.08f,
                            0.16f,
                            1f));
            }
        }

        for (int i = 0; i < 10; i++)
        {
            float angle =
                i * Mathf.PI * 2f / 10f;

            GameObject sensor =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cylinder);

            sensor.name =
                "RECON_PATH_" +
                (i + 1).ToString("00");

            sensor.transform.SetParent(
                root.transform,
                false);

            sensor.transform.position =
                new Vector3(
                    Mathf.Cos(angle) * 5f,
                    0.08f,
                    Mathf.Sin(angle) * 5f);

            sensor.transform.localScale =
                new Vector3(
                    0.16f,
                    0.08f,
                    1.8f);

            sensor.transform.rotation =
                Quaternion.Euler(
                    0f,
                    -angle * Mathf.Rad2Deg,
                    0f);

            Renderer renderer =
                sensor.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.sharedMaterial =
                    MakeMaterial(
                        "MAT_RECON_" + i,
                        new Color(
                            0.20f,
                            0.40f,
                            0.58f,
                            1f));
            }
        }

        for (int i = 0; i < 8; i++)
        {
            float angle =
                i * Mathf.PI * 2f / 8f;

            GameObject logistics =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube);

            logistics.name =
                "LOGISTICS_ROUTE_" +
                (i + 1).ToString("00");

            logistics.transform.SetParent(
                root.transform,
                false);

            logistics.transform.position =
                new Vector3(
                    Mathf.Cos(angle) * 3.8f,
                    0.05f,
                    Mathf.Sin(angle) * 3.8f);

            logistics.transform.localScale =
                new Vector3(
                    2.2f,
                    0.06f,
                    0.14f);

            logistics.transform.rotation =
                Quaternion.Euler(
                    0f,
                    -angle * Mathf.Rad2Deg,
                    0f);

            Renderer renderer =
                logistics.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.sharedMaterial =
                    MakeMaterial(
                        "MAT_LOGISTICS_" + i,
                        new Color(
                            0.32f,
                            0.27f,
                            0.12f,
                            1f));
            }
        }

        for (int i = 0; i < 7; i++)
        {
            GameObject support =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cylinder);

            support.name =
                "FIRE_SUPPORT_POINT_" +
                (i + 1).ToString("00");

            support.transform.SetParent(
                root.transform,
                false);

            support.transform.position =
                new Vector3(
                    -7f + i * 2.3f,
                    0.3f,
                    -7f);

            support.transform.localScale =
                new Vector3(
                    0.4f,
                    0.6f,
                    0.4f);

            Renderer renderer =
                support.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.sharedMaterial =
                    MakeMaterial(
                        "MAT_FIRE_SUPPORT_" + i,
                        new Color(
                            0.35f,
                            0.22f,
                            0.10f,
                            1f));
            }
        }

        GameObject command =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube);

        command.name = "WARFARE_COMMAND_NODE";

        command.transform.SetParent(
            root.transform,
            false);

        command.transform.position =
            new Vector3(0f, 0.65f, 0f);

        command.transform.localScale =
            new Vector3(1.8f, 1.3f, 1.8f);

        Renderer commandRenderer =
            command.GetComponent<Renderer>();

        if (commandRenderer != null)
        {
            commandRenderer.sharedMaterial =
                MakeMaterial(
                    "MAT_COMMAND_NODE",
                    new Color(
                        0.18f,
                        0.10f,
                        0.25f,
                        1f));
        }
    }

    private static Material MakeMaterial(
        string materialName,
        Color color)
    {
        Shader shader =
            Shader.Find(
                "Universal Render Pipeline/Lit");

        if (shader == null)
            shader = Shader.Find("Standard");

        Material material =
            new Material(shader);

        material.name = materialName;
        material.color = color;

        return material;
    }

    private static void BuildCamera()
    {
        GameObject cameraObject =
            new GameObject(
                "WarfareProfileCamera");

        Camera camera =
            cameraObject.AddComponent<Camera>();

        cameraObject.tag = "MainCamera";

        camera.orthographic = true;
        camera.orthographicSize = 12f;

        camera.transform.position =
            new Vector3(0f, 18f, 0f);

        camera.transform.rotation =
            Quaternion.Euler(90f, 0f, 0f);

        camera.clearFlags =
            CameraClearFlags.SolidColor;

        camera.backgroundColor =
            new Color(
                0.008f,
                0.006f,
                0.012f,
                1f);

        camera.nearClipPlane = 0.1f;
        camera.farClipPlane = 100f;
    }

    private static void BuildEventSystem()
    {
        GameObject eventSystem =
            new GameObject(
                "EventSystem",
                typeof(EventSystem),
                typeof(InputSystemUIInputModule));

        InputSystemUIInputModule module =
            eventSystem.GetComponent<
                InputSystemUIInputModule>();

        if (module == null)
            eventSystem.AddComponent<
                InputSystemUIInputModule>();

        // Do not assign EventSystem.current manually.
    }

    private static void BuildHUD()
    {
        GameObject canvasObject =
            new GameObject(
                "HUD-210 WarfareProfileHUD",
                typeof(RectTransform),
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
            new Vector2(1920f, 1080f);

        scaler.screenMatchMode =
            CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        scaler.matchWidthOrHeight = 0.5f;

        BuildTopBar(canvasObject.transform);
        BuildNavigation(canvasObject.transform);
        BuildWarfareLayers(canvasObject.transform);
        BuildWarfareStats(canvasObject.transform);
        BuildUnitPerformance(canvasObject.transform);
        BuildTacticalModifiers(canvasObject.transform);
        BuildQuickActions(canvasObject.transform);
        BuildAlerts(canvasObject.transform);
        BuildBottomBar(canvasObject.transform);
    }

    private static GameObject Panel(
        string name,
        Transform parent,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 offsetMin,
        Vector2 offsetMax)
    {
        GameObject panel =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image));

        panel.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            panel.GetComponent<RectTransform>();

        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = offsetMin;
        rect.offsetMax = offsetMax;

        Image image =
            panel.GetComponent<Image>();

        image.color = PanelColor;

        return panel;
    }

    private static TextMeshProUGUI Label(
        string name,
        Transform parent,
        string value,
        int fontSize = 14,
        TextAlignmentOptions alignment =
            TextAlignmentOptions.Left)
    {
        GameObject objectValue =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(TextMeshProUGUI));

        objectValue.transform.SetParent(
            parent,
            false);

        TextMeshProUGUI text =
            objectValue.GetComponent<TextMeshProUGUI>();

        text.text = value;
        text.fontSize = fontSize;
        text.color = TextColor;
        text.alignment = alignment;
        text.raycastTarget = false;
        text.textWrappingMode =
            TextWrappingModes.Normal;

        RectTransform rect =
            objectValue.GetComponent<RectTransform>();

        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin =
            new Vector2(8f, 3f);
        rect.offsetMax =
            new Vector2(-8f, -3f);

        return text;
    }

    private static Button Button(
        string name,
        Transform parent,
        string label,
        float height = 30f)
    {
        GameObject buttonObject =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image),
                typeof(Button));

        buttonObject.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            buttonObject.GetComponent<RectTransform>();

        rect.sizeDelta =
            new Vector2(0f, height);

        Image image =
            buttonObject.GetComponent<Image>();

        image.color =
            new Color(
                0.075f,
                0.055f,
                0.10f,
                0.96f);

        Button button =
            buttonObject.GetComponent<Button>();

        ColorBlock colors =
            button.colors;

        colors.normalColor =
            new Color(
                0.075f,
                0.055f,
                0.10f,
                0.96f);

        colors.highlightedColor =
            new Color(
                0.15f,
                0.09f,
                0.19f,
                1f);

        colors.pressedColor =
            AccentColor;

        colors.selectedColor =
            new Color(
                0.15f,
                0.09f,
                0.19f,
                1f);

        button.colors = colors;

        GameObject labelObject =
            new GameObject(
                "LABEL",
                typeof(RectTransform),
                typeof(TextMeshProUGUI));

        labelObject.transform.SetParent(
            buttonObject.transform,
            false);

        TextMeshProUGUI text =
            labelObject.GetComponent<TextMeshProUGUI>();

        text.text = label;
        text.fontSize = 12;
        text.color = TextColor;
        text.alignment =
            TextAlignmentOptions.Center;
        text.raycastTarget = false;
        text.textWrappingMode =
            TextWrappingModes.Normal;

        RectTransform labelRect =
            labelObject.GetComponent<RectTransform>();

        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.offsetMin = Vector2.zero;
        labelRect.offsetMax = Vector2.zero;

        return button;
    }

    private static void ConfigureVertical(
        GameObject panel,
        float spacing = 4f)
    {
        VerticalLayoutGroup layout =
            panel.AddComponent<VerticalLayoutGroup>();

        layout.padding =
            new RectOffset(8, 8, 8, 8);

        layout.spacing = spacing;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;
    }

    private static void ConfigureHorizontal(
        GameObject panel)
    {
        HorizontalLayoutGroup layout =
            panel.AddComponent<HorizontalLayoutGroup>();

        layout.padding =
            new RectOffset(10, 10, 8, 8);

        layout.spacing = 8f;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
    }

    private static void AddRows(
        GameObject panel,
        string[] names,
        string[] values)
    {
        for (int i = 0;
             i < names.Length && i < values.Length;
             i++)
        {
            GameObject row =
                new GameObject(
                    names[i],
                    typeof(RectTransform));

            row.transform.SetParent(
                panel.transform,
                false);

            LayoutElement element =
                row.AddComponent<LayoutElement>();

            element.minHeight = 24f;
            element.preferredHeight = 24f;

            Label(
                "VALUE",
                row.transform,
                values[i],
                13);
        }
    }

    private static void BuildTopBar(
        Transform canvas)
    {
        GameObject panel =
            Panel(
                "HUD-210-01 TopBar",
                canvas,
                new Vector2(0f, 0.90f),
                new Vector2(1f, 1f),
                new Vector2(8f, -8f),
                new Vector2(-8f, -8f));

        ConfigureHorizontal(panel);

        Label(
            "HUD-210-01A PlanetNameDisplay",
            panel.transform,
            "PLANET // ACHERON",
            17);

        Label(
            "HUD-210-01B RegionNameDisplay",
            panel.transform,
            "REGION // CINDERFALL",
            15);

        Label(
            "HUD-210-01C IslandNameDisplay",
            panel.transform,
            "ISLAND // BLACKWATER",
            15);

        Label(
            "HUD-210-01D WarfareFocusDisplay",
            panel.transform,
            "WARFARE // DEFENSIVE",
            15);

        Label(
            "HUD-210-01E OwnerFactionEmblem",
            panel.transform,
            "FACTION // OBSIDIAN",
            14);

        Label(
            "HUD-210-01F PlayerResourcesPanel",
            panel.transform,
            "CREDITS 84,250 | ENERGY 7,840",
            14);

        Label(
            "HUD-210-01G TimeControlPanel",
            panel.transform,
            "TIME 1.0x",
            14);
    }

    private static void BuildNavigation(
        Transform canvas)
    {
        GameObject panel =
            Panel(
                "HUD-210-02 NavigationPanel",
                canvas,
                new Vector2(0f, 0.27f),
                new Vector2(0.17f, 0.89f),
                new Vector2(8f, 8f),
                new Vector2(-8f, -8f));

        ConfigureVertical(panel);

        Button(
            "HUD-210-02A ReturnToEnvironmentProfileButton",
            panel.transform,
            "ENVIRONMENT PROFILE");

        Button(
            "HUD-210-02B ReturnToIslandMapButton",
            panel.transform,
            "ISLAND MAP");

        Button(
            "HUD-210-02C ReturnToBiomeMapButton",
            panel.transform,
            "BIOME MAP");

        Button(
            "HUD-210-02D ReturnToRegionMapButton",
            panel.transform,
            "REGION MAP");

        Button(
            "HUD-210-02E ReturnToContinentMapButton",
            panel.transform,
            "CONTINENT MAP");

        Button(
            "HUD-210-02F ReturnToSurfaceMapButton",
            panel.transform,
            "SURFACE MAP");

        Button(
            "HUD-210-02G ReturnToPlanetOverviewButton",
            panel.transform,
            "PLANET OVERVIEW");
    }

    private static void BuildWarfareLayers(
        Transform canvas)
    {
        GameObject panel =
            Panel(
                "HUD-210-03 WarfareLayersPanel",
                canvas,
                new Vector2(0.17f, 0.27f),
                new Vector2(0.33f, 0.89f),
                new Vector2(4f, 8f),
                new Vector2(-4f, -8f));

        ConfigureVertical(panel);

        string[] names =
        {
            "HUD-210-03A LayerToggle_WarfareZones",
            "HUD-210-03B LayerToggle_FactionControl",
            "HUD-210-03C LayerToggle_ThreatZones",
            "HUD-210-03D LayerToggle_EngagementRanges",
            "HUD-210-03E LayerToggle_LogisticsRoutes",
            "HUD-210-03F LayerToggle_ReconPaths",
            "HUD-210-03G LayerToggle_SensorInterference",
            "HUD-210-03H LayerToggle_DefensivePositions",
            "HUD-210-03I LayerToggle_WeatherImpact",
            "HUD-210-03J LayerToggle_TerrainModifiers"
        };

        foreach (string name in names)
        {
            string label =
                name
                    .Replace(
                        "HUD-210-03",
                        "")
                    .Replace(
                        "LayerToggle_",
                        "")
                    .Replace(
                        "_",
                        " ");

            Button(
                name,
                panel.transform,
                label);
        }
    }

    private static void BuildWarfareStats(
        Transform canvas)
    {
        GameObject panel =
            Panel(
                "HUD-210-04 WarfareStatsPanel",
                canvas,
                new Vector2(0.70f, 0.50f),
                new Vector2(0.995f, 0.89f),
                new Vector2(4f, 4f),
                new Vector2(-8f, -4f));

        ConfigureVertical(panel);

        AddRows(
            panel,
            new[]
            {
                "HUD-210-04A EngagementRangeDisplay",
                "HUD-210-04B MobilityRatingDisplay",
                "HUD-210-04C ReconVisibilityDisplay",
                "HUD-210-04D SensorInterferenceDisplay",
                "HUD-210-04E AmbushPotentialDisplay",
                "HUD-210-04F DefensivePositionRatingDisplay",
                "HUD-210-04G FireSupportAvailabilityDisplay",
                "HUD-210-04H LogisticsDifficultyDisplay",
                "HUD-210-04I EnvironmentalImpactDisplay"
            },
            new[]
            {
                "ENGAGEMENT RANGE        1.8 km",
                "MOBILITY RATING         72 / 100",
                "RECON VISIBILITY        81 / 100",
                "SENSOR INTERFERENCE     34 / 100",
                "AMBUSH POTENTIAL        68 / 100",
                "DEFENSIVE POSITION      84 / 100",
                "FIRE SUPPORT             76 / 100",
                "LOGISTICS DIFFICULTY     41 / 100",
                "ENVIRONMENTAL IMPACT     53 / 100"
            });
    }

    private static void BuildUnitPerformance(
        Transform canvas)
    {
        GameObject panel =
            Panel(
                "HUD-210-05 UnitPerformancePanel",
                canvas,
                new Vector2(0.33f, 0.50f),
                new Vector2(0.52f, 0.89f),
                new Vector2(4f, 4f),
                new Vector2(-4f, -4f));

        ConfigureVertical(panel);

        AddRows(
            panel,
            new[]
            {
                "HUD-210-05A InfantryEffectivenessDisplay",
                "HUD-210-05B ArmorEffectivenessDisplay",
                "HUD-210-05C AirEffectivenessDisplay",
                "HUD-210-05D NavalEffectivenessDisplay",
                "HUD-210-05E AutonomousUnitEffectivenessDisplay",
                "HUD-210-05F EW_UnitEffectivenessDisplay"
            },
            new[]
            {
                "INFANTRY                 91 / 100",
                "ARMOR                    78 / 100",
                "AIR                      69 / 100",
                "NAVAL                    82 / 100",
                "AUTONOMOUS               88 / 100",
                "ELECTRONIC WARFARE       94 / 100"
            });
    }

    private static void BuildTacticalModifiers(
        Transform canvas)
    {
        GameObject panel =
            Panel(
                "HUD-210-06 TacticalModifiersPanel",
                canvas,
                new Vector2(0.52f, 0.27f),
                new Vector2(0.70f, 0.50f),
                new Vector2(4f, 4f),
                new Vector2(-4f, -4f));

        ConfigureVertical(panel);

        AddRows(
            panel,
            new[]
            {
                "HUD-210-06A TerrainModifierDisplay",
                "HUD-210-06B WeatherModifierDisplay",
                "HUD-210-06C VisibilityModifierDisplay",
                "HUD-210-06D CoverModifierDisplay",
                "HUD-210-06E HazardModifierDisplay"
            },
            new[]
            {
                "TERRAIN                  +12% DEFENSE",
                "WEATHER                  -08% VISIBILITY",
                "VISIBILITY               -14% RECON",
                "COVER                    +21% INFANTRY",
                "HAZARDS                  -06% MOBILITY"
            });
    }

    private static void BuildQuickActions(
        Transform canvas)
    {
        GameObject panel =
            Panel(
                "HUD-210-07 QuickActionsPanel",
                canvas,
                new Vector2(0.33f, 0.04f),
                new Vector2(0.52f, 0.26f),
                new Vector2(4f, 4f),
                new Vector2(-4f, -4f));

        ConfigureVertical(panel);

        Button(
            "HUD-210-07A DeployReconButton",
            panel.transform,
            "DEPLOY RECON");

        Button(
            "HUD-210-07B DeployCombatGroupButton",
            panel.transform,
            "DEPLOY COMBAT GROUP");

        Button(
            "HUD-210-07C DeployEW_TeamButton",
            panel.transform,
            "DEPLOY EW TEAM");

        Button(
            "HUD-210-07D OpenIntelNetworkButton",
            panel.transform,
            "OPEN INTEL NETWORK");

        Button(
            "HUD-210-07E OpenDoctrineManagerButton",
            panel.transform,
            "OPEN DOCTRINE MANAGER");

        Button(
            "HUD-210-07F OpenLogisticsOverviewButton",
            panel.transform,
            "OPEN LOGISTICS OVERVIEW");
    }

    private static void BuildAlerts(
        Transform canvas)
    {
        GameObject panel =
            Panel(
                "HUD-210-08 AlertsPanel",
                canvas,
                new Vector2(0.52f, 0.04f),
                new Vector2(0.70f, 0.26f),
                new Vector2(4f, 4f),
                new Vector2(-4f, -4f));

        ConfigureVertical(panel);

        AddRows(
            panel,
            new[]
            {
                "HUD-210-08A StrategicAlertEntry",
                "HUD-210-08B WarfareAlertEntry",
                "HUD-210-08C FactionAlertEntry",
                "HUD-210-08D ResourceAlertEntry",
                "HUD-210-08E WeatherAlertEntry",
                "HUD-210-08F EnvironmentalHazardAlertEntry",
                "HUD-210-08G TacticalShiftAlertEntry"
            },
            new[]
            {
                "STRATEGIC // SECTOR CONTROL CONTESTED",
                "WARFARE // HOSTILE FORMATION DETECTED",
                "FACTION // ENEMY CONTROL +4%",
                "RESOURCE // AMMUNITION STOCKPILE STABLE",
                "WEATHER // STORM FRONT ACTIVE",
                "ENVIRONMENT // TERRAIN HAZARD MODERATE",
                "TACTICAL // ENEMY POSTURE SHIFT DETECTED"
            });
    }

    private static void BuildBottomBar(
        Transform canvas)
    {
        GameObject panel =
            Panel(
                "HUD-210-09 BottomBar",
                canvas,
                new Vector2(0f, 0f),
                new Vector2(1f, 0.045f),
                new Vector2(8f, 4f),
                new Vector2(-8f, -4f));

        ConfigureHorizontal(panel);

        Button(
            "HUD-210-09A BackButton",
            panel.transform,
            "BACK",
            28f);

        Button(
            "HUD-210-09B ExitToCommandCenterButton",
            panel.transform,
            "COMMAND CENTER",
            28f);

        Button(
            "HUD-210-09C SettingsButton",
            panel.transform,
            "SETTINGS",
            28f);

        Button(
            "HUD-210-09D HelpButton",
            panel.transform,
            "HELP",
            28f);
    }

    private static void AddToBuildSettings()
    {
        List<EditorBuildSettingsScene> scenes =
            new List<EditorBuildSettingsScene>(
                EditorBuildSettings.scenes);

        bool exists = false;

        foreach (EditorBuildSettingsScene scene in scenes)
        {
            if (scene.path == ScenePath)
            {
                exists = true;
                break;
            }
        }

        if (!exists)
        {
            scenes.Add(
                new EditorBuildSettingsScene(
                    ScenePath,
                    true));

            EditorBuildSettings.scenes =
                scenes.ToArray();
        }
    }
}
