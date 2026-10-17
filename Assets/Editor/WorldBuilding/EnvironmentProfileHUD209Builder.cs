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

public static class EnvironmentProfileHUD209Builder
{
    private const string ScenePath =
        "Assets/Scenes/SCN68_ENVIRONMENT_PROFILE/Environment_Profile.unity";

    private static readonly Color PanelColor =
        new Color(0.035f, 0.055f, 0.075f, 0.94f);

    private static readonly Color PanelBorder =
        new Color(0.18f, 0.28f, 0.34f, 1f);

    private static readonly Color AccentColor =
        new Color(0.20f, 0.78f, 0.86f, 1f);

    private static readonly Color TextColor =
        new Color(0.88f, 0.94f, 0.96f, 1f);

    private static readonly Color MutedColor =
        new Color(0.56f, 0.66f, 0.70f, 1f);

    [MenuItem("Obsidian Protocol/Worlds/BUILD HUD-209 ENVIRONMENT PROFILE")]
    public static void Build()
    {
        EnsureFolders();

        EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene,
            NewSceneMode.Single);

        BuildEnvironment();
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
            "HUD-209 Environment Profile built successfully: " +
            ScenePath);
    }

    private static void EnsureFolders()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Scenes"))
            AssetDatabase.CreateFolder("Assets", "Scenes");

        if (!AssetDatabase.IsValidFolder(
                "Assets/Scenes/SCN68_ENVIRONMENT_PROFILE"))
        {
            AssetDatabase.CreateFolder(
                "Assets/Scenes",
                "SCN68_ENVIRONMENT_PROFILE");
        }
    }

    private static void BuildEnvironment()
    {
        GameObject root = new GameObject(
            "ENVIRONMENT PROFILE ENVIRONMENT");

        GameObject water = GameObject.CreatePrimitive(
            PrimitiveType.Plane);

        water.name = "ENVIRONMENT_WATER";

        water.transform.SetParent(
            root.transform,
            false);

        water.transform.localScale =
            new Vector3(12f, 1f, 12f);

        water.transform.position =
            new Vector3(0f, -1.2f, 0f);

        Renderer waterRenderer =
            water.GetComponent<Renderer>();

        if (waterRenderer != null)
        {
            waterRenderer.sharedMaterial =
                MakeMaterial(
                    "MAT_ENVIRONMENT_WATER",
                    new Color(0.025f, 0.11f, 0.16f, 1f));
        }

        for (int i = 0; i < 24; i++)
        {
            float angle =
                i * Mathf.PI * 2f / 24f;

            float radius =
                4.5f + (i % 5) * 1.25f;

            float x =
                Mathf.Cos(angle) * radius;

            float z =
                Mathf.Sin(angle) * radius;

            float height =
                0.35f +
                (i % 4) * 0.32f;

            GameObject terrain =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cylinder);

            terrain.name =
                "ENVIRONMENT_TERRAIN_" +
                (i + 1).ToString("00");

            terrain.transform.SetParent(
                root.transform,
                false);

            terrain.transform.position =
                new Vector3(x, height * 0.5f, z);

            terrain.transform.localScale =
                new Vector3(
                    1.1f + (i % 3) * 0.35f,
                    height,
                    1.1f + (i % 2) * 0.4f);

            Renderer renderer =
                terrain.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.sharedMaterial =
                    MakeMaterial(
                        "MAT_TERRAIN_" + i,
                        new Color(
                            0.16f + (i % 3) * 0.025f,
                            0.22f + (i % 4) * 0.02f,
                            0.20f,
                            1f));
            }
        }

        for (int i = 0; i < 8; i++)
        {
            float angle =
                i * Mathf.PI * 2f / 8f;

            float radius = 6.8f;

            GameObject node =
                GameObject.CreatePrimitive(
                    PrimitiveType.Sphere);

            node.name =
                "RESOURCE_NODE_" +
                (i + 1).ToString("00");

            node.transform.SetParent(
                root.transform,
                false);

            node.transform.position =
                new Vector3(
                    Mathf.Cos(angle) * radius,
                    0.35f,
                    Mathf.Sin(angle) * radius);

            node.transform.localScale =
                Vector3.one * 0.28f;

            Renderer renderer =
                node.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.sharedMaterial =
                    MakeMaterial(
                        "MAT_RESOURCE_" + i,
                        new Color(
                            0.65f,
                            0.42f,
                            0.12f,
                            1f));
            }
        }

        for (int i = 0; i < 6; i++)
        {
            float angle =
                i * Mathf.PI * 2f / 6f +
                0.35f;

            float radius = 5.5f;

            GameObject threat =
                GameObject.CreatePrimitive(
                    PrimitiveType.Sphere);

            threat.name =
                "THREAT_ZONE_" +
                (i + 1).ToString("00");

            threat.transform.SetParent(
                root.transform,
                false);

            threat.transform.position =
                new Vector3(
                    Mathf.Cos(angle) * radius,
                    0.18f,
                    Mathf.Sin(angle) * radius);

            threat.transform.localScale =
                Vector3.one * 0.55f;

            Renderer renderer =
                threat.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.sharedMaterial =
                    MakeMaterial(
                        "MAT_THREAT_" + i,
                        new Color(
                            0.55f,
                            0.10f,
                            0.10f,
                            1f));
            }
        }

        for (int i = 0; i < 5; i++)
        {
            GameObject weather =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cylinder);

            weather.name =
                "WEATHER_SYSTEM_" +
                (i + 1).ToString("00");

            weather.transform.SetParent(
                root.transform,
                false);

            weather.transform.position =
                new Vector3(
                    -7f + i * 3.5f,
                    0.15f,
                    7f);

            weather.transform.localScale =
                new Vector3(
                    1.1f,
                    0.08f,
                    0.55f);

            Renderer renderer =
                weather.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.sharedMaterial =
                    MakeMaterial(
                        "MAT_WEATHER_" + i,
                        new Color(
                            0.30f,
                            0.48f,
                            0.62f,
                            1f));
            }
        }

        for (int i = 0; i < 7; i++)
        {
            GameObject route =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube);

            route.name =
                "LOGISTICS_ROUTE_" +
                (i + 1).ToString("00");

            route.transform.SetParent(
                root.transform,
                false);

            route.transform.position =
                new Vector3(
                    -6f + i * 2f,
                    0.08f,
                    -6f);

            route.transform.localScale =
                new Vector3(
                    1.6f,
                    0.04f,
                    0.12f);

            Renderer renderer =
                route.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.sharedMaterial =
                    MakeMaterial(
                        "MAT_ROUTE_" + i,
                        new Color(
                            0.25f,
                            0.46f,
                            0.48f,
                            1f));
            }
        }

        GameObject atmosphere =
            GameObject.CreatePrimitive(
                PrimitiveType.Sphere);

        atmosphere.name =
            "ATMOSPHERIC_REFERENCE";

        atmosphere.transform.SetParent(
            root.transform,
            false);

        atmosphere.transform.position =
            new Vector3(0f, 1.5f, 0f);

        atmosphere.transform.localScale =
            new Vector3(18f, 0.45f, 18f);

        Renderer atmosphereRenderer =
            atmosphere.GetComponent<Renderer>();

        if (atmosphereRenderer != null)
        {
            atmosphereRenderer.sharedMaterial =
                MakeMaterial(
                    "MAT_ATMOSPHERE",
                    new Color(
                        0.08f,
                        0.16f,
                        0.19f,
                        1f));
        }
    }

    private static Material MakeMaterial(
        string materialName,
        Color color)
    {
        Shader shader =
            Shader.Find("Universal Render Pipeline/Lit");

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
            new GameObject("EnvironmentProfileCamera");

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
            new Color(0.008f, 0.014f, 0.02f, 1f);

        camera.nearClipPlane = 0.1f;
        camera.farClipPlane = 100f;
    }

    private static void BuildEventSystem()
    {
        GameObject eventSystemObject =
            new GameObject(
                "EventSystem",
                typeof(EventSystem),
                typeof(InputSystemUIInputModule));

        InputSystemUIInputModule module =
            eventSystemObject.GetComponent<InputSystemUIInputModule>();

        if (module == null)
            eventSystemObject.AddComponent<InputSystemUIInputModule>();

        // Do not assign EventSystem.current manually.
    }

    private static void BuildHUD()
    {
        GameObject canvasObject =
            new GameObject(
                "HUD-209 EnvironmentProfileHUD",
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
        BuildEnvironmentLayers(canvasObject.transform);
        BuildEnvironmentStats(canvasObject.transform);
        BuildObjectInfo(canvasObject.transform);
        BuildWarfare(canvasObject.transform);
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
        int size = 15,
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
        text.fontSize = size;
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
                0.06f,
                0.11f,
                0.14f,
                0.96f);

        Button button =
            buttonObject.GetComponent<Button>();

        ColorBlock colors =
            button.colors;

        colors.normalColor =
            new Color(
                0.06f,
                0.11f,
                0.14f,
                0.96f);

        colors.highlightedColor =
            new Color(
                0.10f,
                0.19f,
                0.23f,
                1f);

        colors.pressedColor =
            AccentColor;

        colors.selectedColor =
            new Color(
                0.10f,
                0.19f,
                0.23f,
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
        text.fontSize = 13;
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

    private static void AddRows(
        GameObject panel,
        string[] names,
        string[] values)
    {
        for (int i = 0; i < names.Length; i++)
        {
            GameObject row =
                new GameObject(
                    names[i],
                    typeof(RectTransform));

            row.transform.SetParent(
                panel.transform,
                false);

            LayoutElement layout =
                row.AddComponent<LayoutElement>();

            layout.minHeight = 24f;
            layout.preferredHeight = 24f;

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
                "HUD-209-01 TopBar",
                canvas,
                new Vector2(0f, 0.90f),
                new Vector2(1f, 1f),
                new Vector2(8f, -8f),
                new Vector2(-8f, -8f));

        HorizontalLayoutGroup layout =
            panel.AddComponent<HorizontalLayoutGroup>();

        layout.padding =
            new RectOffset(10, 10, 8, 8);

        layout.spacing = 8f;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;

        Label(
            "HUD-209-01A PlanetNameDisplay",
            panel.transform,
            "PLANET // ACHERON",
            18);

        Label(
            "HUD-209-01B RegionNameDisplay",
            panel.transform,
            "REGION // CINDERFALL",
            16);

        Label(
            "HUD-209-01C IslandNameDisplay",
            panel.transform,
            "ISLAND // BLACKWATER",
            16);

        Label(
            "HUD-209-01D EnvironmentTypeDisplay",
            panel.transform,
            "ENVIRONMENT // TEMPERATE COASTAL",
            14);

        Label(
            "HUD-209-01E OwnerFactionEmblem",
            panel.transform,
            "FACTION // OBSIDIAN",
            14);

        Label(
            "HUD-209-01F PlayerResourcesPanel",
            panel.transform,
            "CREDITS 84,250  |  ENERGY 7,840",
            14);

        Label(
            "HUD-209-01G TimeControlPanel",
            panel.transform,
            "TIME  1.0x",
            14);
    }

    private static void BuildNavigation(
        Transform canvas)
    {
        GameObject panel =
            Panel(
                "HUD-209-02 NavigationPanel",
                canvas,
                new Vector2(0f, 0.28f),
                new Vector2(0.17f, 0.89f),
                new Vector2(8f, 8f),
                new Vector2(-8f, -8f));

        ConfigureVertical(panel);

        Button(
            "HUD-209-02A ReturnToIslandMapButton",
            panel.transform,
            "RETURN TO ISLAND MAP");

        Button(
            "HUD-209-02B ReturnToBiomeMapButton",
            panel.transform,
            "RETURN TO BIOME MAP");

        Button(
            "HUD-209-02C ReturnToRegionMapButton",
            panel.transform,
            "RETURN TO REGION MAP");

        Button(
            "HUD-209-02D ReturnToContinentMapButton",
            panel.transform,
            "RETURN TO CONTINENT MAP");

        Button(
            "HUD-209-02E ReturnToSurfaceMapButton",
            panel.transform,
            "RETURN TO SURFACE MAP");

        Button(
            "HUD-209-02F ReturnToPlanetOverviewButton",
            panel.transform,
            "PLANET OVERVIEW");
    }

    private static void BuildEnvironmentLayers(
        Transform canvas)
    {
        GameObject panel =
            Panel(
                "HUD-209-03 EnvironmentLayersPanel",
                canvas,
                new Vector2(0.17f, 0.28f),
                new Vector2(0.33f, 0.89f),
                new Vector2(4f, 8f),
                new Vector2(-4f, -8f));

        ConfigureVertical(panel);

        string[] names =
        {
            "HUD-209-03A LayerToggle_Terrain",
            "HUD-209-03B LayerToggle_Elevation",
            "HUD-209-03C LayerToggle_Heatmap",
            "HUD-209-03D LayerToggle_Waterways",
            "HUD-209-03E LayerToggle_FactionControl",
            "HUD-209-03F LayerToggle_WarfareZones",
            "HUD-209-03G LayerToggle_ResourceNodes",
            "HUD-209-03H LayerToggle_LogisticsRoutes",
            "HUD-209-03I LayerToggle_WeatherSystems",
            "HUD-209-03J LayerToggle_EnvironmentHazards",
            "HUD-209-03K LayerToggle_ThreatZones",
            "HUD-209-03L LayerToggle_Traversability"
        };

        foreach (string name in names)
        {
            string label =
                name
                    .Replace("HUD-209-03", "")
                    .Replace("LayerToggle_", "")
                    .Replace("_", " ");

            Button(
                name,
                panel.transform,
                label);
        }
    }

    private static void BuildEnvironmentStats(
        Transform canvas)
    {
        GameObject panel =
            Panel(
                "HUD-209-04 EnvironmentStatsPanel",
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
                "HUD-209-04A TemperatureDisplay",
                "HUD-209-04B HumidityDisplay",
                "HUD-209-04C WindSpeedDisplay",
                "HUD-209-04D VisibilityDisplay",
                "HUD-209-04E TerrainStabilityDisplay",
                "HUD-209-04F HazardLevelDisplay",
                "HUD-209-04G WeatherPatternDisplay"
            },
            new[]
            {
                "TEMPERATURE     18 C",
                "HUMIDITY        62 %",
                "WIND SPEED      24 km/h",
                "VISIBILITY      11.8 km",
                "TERRAIN         STABLE",
                "HAZARD LEVEL    MODERATE",
                "WEATHER         COASTAL STORM FRONT"
            });
    }

    private static void BuildObjectInfo(
        Transform canvas)
    {
        GameObject panel =
            Panel(
                "HUD-209-05 EnvironmentObjectInfoPanel",
                canvas,
                new Vector2(0.70f, 0.27f),
                new Vector2(0.995f, 0.49f),
                new Vector2(4f, 4f),
                new Vector2(-8f, -4f));

        ConfigureVertical(panel);

        AddRows(
            panel,
            new[]
            {
                "HUD-209-05A SelectedObjectName",
                "HUD-209-05B SelectedObjectType",
                "HUD-209-05C SelectedObjectOwnerFaction",
                "HUD-209-05D SelectedObjectStatus",
                "HUD-209-05E SelectedObjectThreatLevel",
                "HUD-209-05F SelectedObjectEnvironmentType",
                "HUD-209-05G SelectedObjectWarfareFocus"
            },
            new[]
            {
                "BLACKWATER FACILITY",
                "FORWARD OPERATIONS SITE",
                "OBSIDIAN PROTOCOL",
                "OPERATIONAL",
                "THREAT LEVEL 64%",
                "COASTAL TEMPERATE",
                "DEFENSIVE / RECON"
            });

        Button(
            "HUD-209-05H EnterObjectButton",
            panel.transform,
            "ENTER SELECTED OBJECT");
    }

    private static void BuildWarfare(
        Transform canvas)
    {
        GameObject panel =
            Panel(
                "HUD-209-06 WarfarePanel",
                canvas,
                new Vector2(0.70f, 0.04f),
                new Vector2(0.995f, 0.26f),
                new Vector2(4f, 4f),
                new Vector2(-8f, -4f));

        ConfigureVertical(panel);

        AddRows(
            panel,
            new[]
            {
                "HUD-209-06A WarfareFocusDisplay",
                "HUD-209-06B EngagementRangeDisplay",
                "HUD-209-06C MobilityRatingDisplay",
                "HUD-209-06D ReconVisibilityDisplay",
                "HUD-209-06E SensorInterferenceDisplay",
                "HUD-209-06F AmbushPotentialDisplay",
                "HUD-209-06G DefensivePositionRatingDisplay"
            },
            new[]
            {
                "WARFARE FOCUS   DEFENSE",
                "ENGAGEMENT     1.8 km",
                "MOBILITY       72 / 100",
                "RECON          81 / 100",
                "INTERFERENCE   34 / 100",
                "AMBUSH         68 / 100",
                "DEFENSE        84 / 100"
            });
    }

    private static void BuildQuickActions(
        Transform canvas)
    {
        GameObject panel =
            Panel(
                "HUD-209-07 QuickActionsPanel",
                canvas,
                new Vector2(0.33f, 0.04f),
                new Vector2(0.52f, 0.27f),
                new Vector2(4f, 4f),
                new Vector2(-4f, -4f));

        ConfigureVertical(panel);

        Button(
            "HUD-209-07A DeployReconButton",
            panel.transform,
            "DEPLOY RECON");

        Button(
            "HUD-209-07B DeployLogisticsTeamButton",
            panel.transform,
            "DEPLOY LOGISTICS TEAM");

        Button(
            "HUD-209-07C DeployCombatGroupButton",
            panel.transform,
            "DEPLOY COMBAT GROUP");

        Button(
            "HUD-209-07D OpenIntelNetworkButton",
            panel.transform,
            "OPEN INTEL NETWORK");

        Button(
            "HUD-209-07E OpenDoctrineManagerButton",
            panel.transform,
            "OPEN DOCTRINE MANAGER");

        Button(
            "HUD-209-07F OpenWarfareProfileButton",
            panel.transform,
            "OPEN WARFARE PROFILE");
    }

    private static void BuildAlerts(
        Transform canvas)
    {
        GameObject panel =
            Panel(
                "HUD-209-08 AlertsPanel",
                canvas,
                new Vector2(0.52f, 0.04f),
                new Vector2(0.70f, 0.27f),
                new Vector2(4f, 4f),
                new Vector2(-4f, -4f));

        ConfigureVertical(panel);

        AddRows(
            panel,
            new[]
            {
                "HUD-209-08A StrategicAlertEntry",
                "HUD-209-08B WarfareAlertEntry",
                "HUD-209-08C FactionAlertEntry",
                "HUD-209-08D ResourceAlertEntry",
                "HUD-209-08E WeatherAlertEntry",
                "HUD-209-08F EnvironmentalHazardAlertEntry"
            },
            new[]
            {
                "STRATEGIC // ISLAND CONTROL CONTESTED",
                "WARFARE // HOSTILE PATROLS DETECTED",
                "FACTION // ENEMY SENSOR ACTIVITY",
                "RESOURCE // NODE 04 OUTPUT REDUCED",
                "WEATHER // STORM FRONT APPROACHING",
                "ENVIRONMENT // FLOOD RISK MODERATE"
            });
    }

    private static void BuildBottomBar(
        Transform canvas)
    {
        GameObject panel =
            Panel(
                "HUD-209-09 BottomBar",
                canvas,
                new Vector2(0f, 0f),
                new Vector2(1f, 0.045f),
                new Vector2(8f, 4f),
                new Vector2(-8f, -4f));

        HorizontalLayoutGroup layout =
            panel.AddComponent<HorizontalLayoutGroup>();

        layout.padding =
            new RectOffset(8, 8, 4, 4);

        layout.spacing = 8f;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;

        Button(
            "HUD-209-09A BackButton",
            panel.transform,
            "BACK",
            28f);

        Button(
            "HUD-209-09B ExitToCommandCenterButton",
            panel.transform,
            "COMMAND CENTER",
            28f);

        Button(
            "HUD-209-09C SettingsButton",
            panel.transform,
            "SETTINGS",
            28f);

        Button(
            "HUD-209-09D HelpButton",
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
