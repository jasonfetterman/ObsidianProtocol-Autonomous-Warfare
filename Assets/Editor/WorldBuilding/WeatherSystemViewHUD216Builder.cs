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

public static class WeatherSystemViewHUD216Builder
{
    private const int Width = 1920;
    private const int Height = 1080;

    private static readonly Color BackgroundColor = new Color(0.008f, 0.018f, 0.028f, 1f);
    private static readonly Color PanelColor = new Color(0.025f, 0.055f, 0.075f, 0.97f);
    private static readonly Color PanelAltColor = new Color(0.035f, 0.075f, 0.095f, 0.97f);
    private static readonly Color TextColor = new Color(0.78f, 0.92f, 0.96f, 1f);
    private static readonly Color DimTextColor = new Color(0.46f, 0.66f, 0.72f, 1f);
    private static readonly Color AccentColor = new Color(0.20f, 0.78f, 0.92f, 1f);
    private static readonly Color WarningColor = new Color(0.95f, 0.68f, 0.20f, 1f);
    private static readonly Color DangerColor = new Color(0.92f, 0.25f, 0.22f, 1f);
    private static readonly Color GreenColor = new Color(0.25f, 0.82f, 0.48f, 1f);

    private static readonly Dictionary<string, GameObject> Layers =
        new Dictionary<string, GameObject>();

    private static TextMeshProUGUI selectedZone;
    private static TextMeshProUGUI selectedType;
    private static TextMeshProUGUI selectedTemperature;
    private static TextMeshProUGUI selectedHumidity;
    private static TextMeshProUGUI selectedWind;
    private static TextMeshProUGUI selectedPrecipitation;
    private static TextMeshProUGUI selectedStorm;
    private static TextMeshProUGUI selectedVisibility;
    private static TextMeshProUGUI selectedPressure;
    private static TextMeshProUGUI alertStatus;

    [MenuItem("Obsidian Protocol/Worlds/BUILD HUD-216 WEATHER SYSTEM")]
    public static void Build()
    {
        Layers.Clear();

        string scenePath =
            "Assets/Scenes/SCN75_WEATHER_SYSTEM/Weather_System.unity";

        EnsureFolder("Assets/Scenes");
        EnsureFolder("Assets/Scenes/SCN75_WEATHER_SYSTEM");

        EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene,
            NewSceneMode.Single);

        BuildWorld();

        GameObject canvasObject = new GameObject(
            "HUD-216 WeatherSystemViewHUD",
            typeof(RectTransform),
            typeof(Canvas),
            typeof(CanvasScaler),
            typeof(GraphicRaycaster));

        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(Width, Height);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

        GameObject eventSystem = new GameObject(
            "EventSystem",
            typeof(EventSystem),
            typeof(InputSystemUIInputModule));

        BuildTopBar(canvasObject.transform);
        BuildNavigationPanel(canvasObject.transform);
        BuildWeatherLayersPanel(canvasObject.transform);
        BuildWeatherLegendPanel(canvasObject.transform);
        BuildWeatherInfoPanel(canvasObject.transform);
        BuildWeatherImpactPanel(canvasObject.transform);
        BuildForecastPanel(canvasObject.transform);
        BuildQuickActionsPanel(canvasObject.transform);
        BuildAlertsPanel(canvasObject.transform);
        BuildBottomBar(canvasObject.transform);

        WireLayerButtons();
        WireNavigationButtons();
        WireQuickActions();
        WireBottomButtons();

        Scene scene = SceneManager.GetActiveScene();
        scene.name = "Weather_System";

        EditorSceneManager.SaveScene(scene, scenePath);

        AddBuildScene(scenePath);

        Debug.Log("[HUD-216] Weather System HUD built: " + scenePath);
    }

    private static void EnsureFolder(string path)
    {
        string[] parts = path.Split('/');
        string current = parts[0];

        for (int i = 1; i < parts.Length; i++)
        {
            string next = current + "/" + parts[i];

            if (!AssetDatabase.IsValidFolder(next))
                AssetDatabase.CreateFolder(current, parts[i]);

            current = next;
        }
    }

    private static void AddBuildScene(string scenePath)
    {
        List<EditorBuildSettingsScene> scenes =
            new List<EditorBuildSettingsScene>(
                EditorBuildSettings.scenes);

        for (int i = 0; i < scenes.Count; i++)
        {
            if (scenes[i].path == scenePath)
                return;
        }

        scenes.Add(
            new EditorBuildSettingsScene(scenePath, true));

        EditorBuildSettings.scenes = scenes.ToArray();
    }

    private static void BuildWorld()
    {
        GameObject root = new GameObject("WEATHER SYSTEM ENVIRONMENT");

        GameObject terrain = GameObject.CreatePrimitive(
            PrimitiveType.Plane);

        terrain.name = "WEATHER MAP SURFACE";
        terrain.transform.SetParent(root.transform);
        terrain.transform.position = Vector3.zero;
        terrain.transform.localScale = new Vector3(10f, 1f, 6f);

        Renderer terrainRenderer = terrain.GetComponent<Renderer>();
        terrainRenderer.sharedMaterial =
            MakeMaterial(new Color(0.035f, 0.09f, 0.11f, 1f));

        CreateWeatherRegion(
            root.transform,
            "CLEAR REGION",
            new Vector3(-22f, 0.12f, 12f),
            new Vector3(14f, 0.08f, 8f),
            new Color(0.18f, 0.52f, 0.58f, 0.32f));

        CreateWeatherRegion(
            root.transform,
            "STORM REGION",
            new Vector3(20f, 0.14f, 13f),
            new Vector3(17f, 0.08f, 10f),
            new Color(0.35f, 0.18f, 0.48f, 0.48f));

        CreateWeatherRegion(
            root.transform,
            "PRECIPITATION REGION",
            new Vector3(5f, 0.13f, -10f),
            new Vector3(23f, 0.08f, 11f),
            new Color(0.12f, 0.32f, 0.65f, 0.38f));

        CreateWeatherRegion(
            root.transform,
            "HIGH PRESSURE REGION",
            new Vector3(-25f, 0.13f, -13f),
            new Vector3(12f, 0.08f, 8f),
            new Color(0.30f, 0.55f, 0.30f, 0.32f));

        for (int i = 0; i < 16; i++)
        {
            float x = -42f + (i % 8) * 12f;
            float z = -22f + (i / 8) * 38f;

            GameObject node = GameObject.CreatePrimitive(
                PrimitiveType.Cylinder);

            node.name = "WEATHER OBSERVATION NODE " + (i + 1);
            node.transform.SetParent(root.transform);
            node.transform.position =
                new Vector3(x, 0.35f, z);
            node.transform.localScale =
                new Vector3(0.65f, 0.35f, 0.65f);

            node.GetComponent<Renderer>().sharedMaterial =
                MakeMaterial(AccentColor);
        }

        GameObject cameraObject = new GameObject(
            "Weather System Camera");

        Camera cam = cameraObject.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = 38f;
        cam.transform.position = new Vector3(0f, 45f, 0f);
        cam.transform.rotation =
            Quaternion.Euler(90f, 0f, 0f);
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = BackgroundColor;
    }

    private static void CreateWeatherRegion(
        Transform parent,
        string name,
        Vector3 position,
        Vector3 scale,
        Color color)
    {
        GameObject region = GameObject.CreatePrimitive(
            PrimitiveType.Cube);

        region.name = name;
        region.transform.SetParent(parent);
        region.transform.position = position;
        region.transform.localScale = scale;
        region.GetComponent<Renderer>().sharedMaterial =
            MakeMaterial(color);
    }

    private static Material MakeMaterial(Color color)
    {
        Shader shader =
            Shader.Find("Universal Render Pipeline/Lit");

        if (shader == null)
            shader = Shader.Find("Standard");

        Material material = new Material(shader);
        material.color = color;
        return material;
    }

    private static GameObject Panel(
        Transform parent,
        string name,
        Vector2 position,
        Vector2 size)
    {
        GameObject go = new GameObject(
            name,
            typeof(RectTransform),
            typeof(Image));

        go.transform.SetParent(parent, false);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(.5f, .5f);
        rt.anchorMax = new Vector2(.5f, .5f);
        rt.pivot = new Vector2(.5f, .5f);
        rt.anchoredPosition = position;
        rt.sizeDelta = size;

        go.GetComponent<Image>().color = PanelColor;

        return go;
    }

    private static TextMeshProUGUI Text(
        Transform parent,
        string name,
        string value,
        Vector2 position,
        Vector2 size,
        int fontSize = 18)
    {
        GameObject go = new GameObject(
            name,
            typeof(RectTransform),
            typeof(TextMeshProUGUI));

        go.transform.SetParent(parent, false);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(.5f, .5f);
        rt.anchorMax = new Vector2(.5f, .5f);
        rt.pivot = new Vector2(.5f, .5f);
        rt.anchoredPosition = position;
        rt.sizeDelta = size;

        TextMeshProUGUI tmp =
            go.GetComponent<TextMeshProUGUI>();

        tmp.text = value;
        tmp.fontSize = fontSize;
        tmp.color = TextColor;
        tmp.alignment = TextAlignmentOptions.MidlineLeft;
        tmp.textWrappingMode = TextWrappingModes.Normal;

        return tmp;
    }

    private static Button Button(
        Transform parent,
        string name,
        string label,
        Vector2 position,
        Vector2 size)
    {
        GameObject go = new GameObject(
            name,
            typeof(RectTransform),
            typeof(Image),
            typeof(Button));

        go.transform.SetParent(parent, false);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(.5f, .5f);
        rt.anchorMax = new Vector2(.5f, .5f);
        rt.pivot = new Vector2(.5f, .5f);
        rt.anchoredPosition = position;
        rt.sizeDelta = size;

        Image image = go.GetComponent<Image>();
        image.color = PanelAltColor;

        Button button = go.GetComponent<Button>();

        ColorBlock colors = button.colors;
        colors.normalColor = PanelAltColor;
        colors.highlightedColor = AccentColor;
        colors.pressedColor = new Color(
            AccentColor.r,
            AccentColor.g,
            AccentColor.b,
            0.65f);
        colors.selectedColor = AccentColor;
        button.colors = colors;

        Text(
            go.transform,
            name + "_Label",
            label,
            Vector2.zero,
            size - new Vector2(12f, 6f),
            14);

        return button;
    }

    private static void Header(
        Transform parent,
        string name,
        string title,
        Vector2 position,
        Vector2 size)
    {
        Text(
            parent,
            name,
            title,
            position,
            size,
            16);
    }

    private static void BuildTopBar(Transform root)
    {
        GameObject p = Panel(
            root,
            "HUD-216-01 TopBar",
            new Vector2(0f, 475f),
            new Vector2(1880f, 70f));

        Text(
            p.transform,
            "HUD-216-01A PlanetNameDisplay",
            "PLANET // AURELIA",
            new Vector2(-790f, 0f),
            new Vector2(230f, 50f),
            20);

        Text(
            p.transform,
            "HUD-216-01B ContinentNameDisplay",
            "CONTINENT // NORTHERN CONTINENT",
            new Vector2(-530f, 0f),
            new Vector2(290f, 50f),
            16);

        Text(
            p.transform,
            "HUD-216-01C RegionNameDisplay",
            "REGION // KESTREL BASIN",
            new Vector2(-215f, 0f),
            new Vector2(260f, 50f),
            16);

        Text(
            p.transform,
            "HUD-216-01D WeatherSystemDisplay",
            "WEATHER SYSTEM // ACTIVE",
            new Vector2(90f, 0f),
            new Vector2(280f, 50f),
            17);

        Text(
            p.transform,
            "HUD-216-01E OwnerFactionEmblem",
            "[ ADF ]",
            new Vector2(370f, 0f),
            new Vector2(100f, 50f),
            18);

        Text(
            p.transform,
            "HUD-216-01F PlayerResourcesPanel",
            "CR 84,250   MAT 12,640   EN 8,420",
            new Vector2(610f, 0f),
            new Vector2(360f, 50f),
            15);

        Text(
            p.transform,
            "HUD-216-01G TimeControlPanel",
            "T+ 184:22:17   ? 1X",
            new Vector2(810f, 0f),
            new Vector2(220f, 50f),
            15);
    }

    private static void BuildNavigationPanel(Transform root)
    {
        GameObject p = Panel(
            root,
            "HUD-216-02 NavigationPanel",
            new Vector2(-770f, 230f),
            new Vector2(250f, 430f));

        string[] names =
        {
            "ReturnToLogisticsNetworkMapButton",
            "ReturnToResourceDistributionMapButton",
            "ReturnToStrategicHeatmapButton",
            "ReturnToFactionTerritoryOverlayButton",
            "ReturnToFactionControlMapButton",
            "ReturnToWarfareProfileButton",
            "ReturnToEnvironmentProfileButton",
            "ReturnToIslandMapButton",
            "ReturnToBiomeMapButton",
            "ReturnToRegionMapButton",
            "ReturnToContinentMapButton",
            "ReturnToSurfaceMapButton",
            "ReturnToPlanetOverviewButton"
        };

        string[] labels =
        {
            "LOGISTICS NETWORK",
            "RESOURCE DISTRIBUTION",
            "STRATEGIC HEATMAP",
            "FACTION TERRITORY",
            "FACTION CONTROL",
            "WARFARE PROFILE",
            "ENVIRONMENT PROFILE",
            "ISLAND MAP",
            "BIOME MAP",
            "REGION MAP",
            "CONTINENT MAP",
            "SURFACE MAP",
            "PLANET OVERVIEW"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Button(
                p.transform,
                "HUD-216-02" +
                (char)('A' + i) +
                " " + names[i],
                labels[i],
                new Vector2(0f, 178f - i * 28f),
                new Vector2(225f, 24f));
        }
    }

    private static void BuildWeatherLayersPanel(Transform root)
    {
        GameObject p = Panel(
            root,
            "HUD-216-03 WeatherLayersPanel",
            new Vector2(-530f, 190f),
            new Vector2(300f, 470f));

        string[] names =
        {
            "WeatherSystems",
            "Temperature",
            "Humidity",
            "WindSpeed",
            "Precipitation",
            "StormCells",
            "Visibility",
            "AtmosphericPressure",
            "FactionControl",
            "WarfareZones",
            "LogisticsRoutes",
            "TerrainModifiers"
        };

        string[] labels =
        {
            "WEATHER SYSTEMS",
            "TEMPERATURE",
            "HUMIDITY",
            "WIND SPEED",
            "PRECIPITATION",
            "STORM CELLS",
            "VISIBILITY",
            "ATMOSPHERIC PRESSURE",
            "FACTION CONTROL",
            "WARFARE ZONES",
            "LOGISTICS ROUTES",
            "TERRAIN MODIFIERS"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Button b = Button(
                p.transform,
                "HUD-216-03" +
                (char)('A' + i) +
                " LayerToggle_" + names[i],
                "? " + labels[i],
                new Vector2(0f, 195f - i * 32f),
                new Vector2(270f, 27f));

            Layers[names[i]] = b.gameObject;
        }
    }

    private static void BuildWeatherLegendPanel(Transform root)
    {
        GameObject p = Panel(
            root,
            "HUD-216-04 WeatherLegendPanel",
            new Vector2(-160f, 190f),
            new Vector2(330f, 330f));

        string[] names =
        {
            "TemperatureLegend",
            "HumidityLegend",
            "WindSpeedLegend",
            "PrecipitationLegend",
            "StormCellIntensityLegend",
            "VisibilityLegend",
            "AtmosphericPressureLegend"
        };

        string[] values =
        {
            "TEMPERATURE  -20C ... +45C",
            "HUMIDITY     0 ... 100%",
            "WIND SPEED   0 ... 160 KM/H",
            "PRECIPITATION 0 ... 180 MM/H",
            "STORM INTENSITY  I ... V",
            "VISIBILITY   0.2 ... 40 KM",
            "PRESSURE     960 ... 1040 HPA"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Text(
                p.transform,
                "HUD-216-04" +
                (char)('A' + i) +
                " " + names[i],
                values[i],
                new Vector2(0f, 125f - i * 38f),
                new Vector2(300f, 32f),
                14);
        }
    }

    private static void BuildWeatherInfoPanel(Transform root)
    {
        GameObject p = Panel(
            root,
            "HUD-216-05 WeatherInfoPanel",
            new Vector2(650f, 245f),
            new Vector2(410f, 370f));

        selectedZone = Text(
            p.transform,
            "HUD-216-05A SelectedWeatherZoneName",
            "ZONE // KESTREL STORM FRONT",
            new Vector2(0f, 145f),
            new Vector2(380f, 35f),
            18);

        selectedType = Text(
            p.transform,
            "HUD-216-05B SelectedWeatherType",
            "TYPE // SEVERE THUNDERSTORM",
            new Vector2(0f, 112f),
            new Vector2(380f, 30f),
            15);

        selectedTemperature = Text(
            p.transform,
            "HUD-216-05C SelectedTemperature",
            "TEMPERATURE // 17 C",
            new Vector2(0f, 78f),
            new Vector2(380f, 30f),
            15);

        selectedHumidity = Text(
            p.transform,
            "HUD-216-05D SelectedHumidity",
            "HUMIDITY // 82 %",
            new Vector2(0f, 48f),
            new Vector2(380f, 30f),
            15);

        selectedWind = Text(
            p.transform,
            "HUD-216-05E SelectedWindSpeed",
            "WIND // 74 KM/H",
            new Vector2(0f, 18f),
            new Vector2(380f, 30f),
            15);

        selectedPrecipitation = Text(
            p.transform,
            "HUD-216-05F SelectedPrecipitationLevel",
            "PRECIPITATION // 42 MM/H",
            new Vector2(0f, -12f),
            new Vector2(380f, 30f),
            15);

        selectedStorm = Text(
            p.transform,
            "HUD-216-05G SelectedStormCellIntensity",
            "STORM CELL // IV",
            new Vector2(0f, -42f),
            new Vector2(380f, 30f),
            15);

        selectedVisibility = Text(
            p.transform,
            "HUD-216-05H SelectedVisibility",
            "VISIBILITY // 4.2 KM",
            new Vector2(0f, -72f),
            new Vector2(380f, 30f),
            15);

        selectedPressure = Text(
            p.transform,
            "HUD-216-05I SelectedAtmosphericPressure",
            "PRESSURE // 992 HPA",
            new Vector2(0f, -102f),
            new Vector2(380f, 30f),
            15);

        Button(
            p.transform,
            "HUD-216-05J EnterWeatherZoneButton",
            "ENTER WEATHER ZONE",
            new Vector2(0f, -145f),
            new Vector2(250f, 34f));
    }

    private static void BuildWeatherImpactPanel(Transform root)
    {
        GameObject p = Panel(
            root,
            "HUD-216-06 WeatherImpactPanel",
            new Vector2(650f, -20f),
            new Vector2(410f, 230f));

        string[] names =
        {
            "MobilityImpactDisplay",
            "ReconVisibilityImpactDisplay",
            "SensorInterferenceImpactDisplay",
            "LogisticsDifficultyImpactDisplay",
            "CombatEffectivenessImpactDisplay",
            "EnvironmentalHazardLevelDisplay"
        };

        string[] values =
        {
            "MOBILITY IMPACT             -18%",
            "RECON VISIBILITY IMPACT     -34%",
            "SENSOR INTERFERENCE         +27%",
            "LOGISTICS DIFFICULTY         +21%",
            "COMBAT EFFECTIVENESS        -12%",
            "ENVIRONMENTAL HAZARD        HIGH"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Text(
                p.transform,
                "HUD-216-06" +
                (char)('A' + i) +
                " " + names[i],
                values[i],
                new Vector2(0f, 85f - i * 29f),
                new Vector2(380f, 25f),
                14);
        }
    }

    private static void BuildForecastPanel(Transform root)
    {
        GameObject p = Panel(
            root,
            "HUD-216-07 ForecastPanel",
            new Vector2(160f, -120f),
            new Vector2(650f, 210f));

        Text(
            p.transform,
            "HUD-216-07A ShortTermForecastDisplay",
            "SHORT TERM // 0-6H     Storm front intensifying; precipitation increasing.",
            new Vector2(0f, 72f),
            new Vector2(610f, 35f),
            14);

        Text(
            p.transform,
            "HUD-216-07B MidTermForecastDisplay",
            "MID TERM // 6-24H      Front moving northeast; visibility recovering.",
            new Vector2(0f, 30f),
            new Vector2(610f, 35f),
            14);

        Text(
            p.transform,
            "HUD-216-07C LongTermForecastDisplay",
            "LONG TERM // 24-72H    Pressure stabilizing; temperatures rising.",
            new Vector2(0f, -12f),
            new Vector2(610f, 35f),
            14);

        Text(
            p.transform,
            "HUD-216-07D StormTrajectoryDisplay",
            "STORM TRAJECTORY       NNE / 38 KM/H",
            new Vector2(0f, -54f),
            new Vector2(610f, 35f),
            14);

        Text(
            p.transform,
            "HUD-216-07E WeatherShiftProbabilityDisplay",
            "WEATHER SHIFT          67% WITHIN 12 HOURS",
            new Vector2(0f, -88f),
            new Vector2(610f, 35f),
            14);
    }

    private static void BuildQuickActionsPanel(Transform root)
    {
        GameObject p = Panel(
            root,
            "HUD-216-08 QuickActionsPanel",
            new Vector2(650f, -245f),
            new Vector2(410f, 190f));

        string[] names =
        {
            "DeployReconButton",
            "DeployLogisticsTeamButton",
            "DeployWeatherMonitoringTeamButton",
            "OpenIntelNetworkButton",
            "OpenDoctrineManagerButton",
            "OpenLogisticsOverviewButton",
            "OpenFactionDiplomacyButton"
        };

        string[] labels =
        {
            "DEPLOY RECON",
            "DEPLOY LOGISTICS TEAM",
            "DEPLOY WEATHER TEAM",
            "OPEN INTEL NETWORK",
            "OPEN DOCTRINE MANAGER",
            "OPEN LOGISTICS",
            "OPEN FACTION DIPLOMACY"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Button(
                p.transform,
                "HUD-216-08" +
                (char)('A' + i) +
                " " + names[i],
                labels[i],
                new Vector2(
                    i % 2 == 0 ? -95f : 95f,
                    65f - (i / 2) * 34f),
                new Vector2(175f, 28f));
        }
    }

    private static void BuildAlertsPanel(Transform root)
    {
        GameObject p = Panel(
            root,
            "HUD-216-09 AlertsPanel",
            new Vector2(-500f, -235f),
            new Vector2(480f, 220f));

        string[] names =
        {
            "StrategicAlertEntry",
            "WeatherAlertEntry",
            "EnvironmentalHazardAlertEntry",
            "LogisticsAlertEntry",
            "WarfareAlertEntry",
            "StormTrajectoryAlertEntry",
            "VisibilityDropAlertEntry"
        };

        string[] values =
        {
            "STRATEGIC // Storm system approaching northern corridor",
            "WEATHER // Severe cell cluster detected",
            "ENVIRONMENT // Flood risk elevated",
            "LOGISTICS // Route efficiency reduced 18%",
            "WARFARE // Air operations weather restricted",
            "STORM // Trajectory intersects Kestrel Basin",
            "VISIBILITY // Recon visibility falling below threshold"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Text(
                p.transform,
                "HUD-216-09" +
                (char)('A' + i) +
                " " + names[i],
                values[i],
                new Vector2(0f, 80f - i * 27f),
                new Vector2(450f, 24f),
                13);
        }

        alertStatus = Text(
            p.transform,
            "HUD-216 AlertStatus",
            "WEATHER MONITORING // NOMINAL",
            new Vector2(0f, -95f),
            new Vector2(450f, 25f),
            13);
    }

    private static void BuildBottomBar(Transform root)
    {
        GameObject p = Panel(
            root,
            "HUD-216-10 BottomBar",
            new Vector2(0f, -485f),
            new Vector2(1880f, 55f));

        Button(
            p.transform,
            "HUD-216-10A BackButton",
            "BACK",
            new Vector2(-760f, 0f),
            new Vector2(180f, 34f));

        Button(
            p.transform,
            "HUD-216-10B ExitToCommandCenterButton",
            "COMMAND CENTER",
            new Vector2(-540f, 0f),
            new Vector2(220f, 34f));

        Button(
            p.transform,
            "HUD-216-10C SettingsButton",
            "SETTINGS",
            new Vector2(620f, 0f),
            new Vector2(180f, 34f));

        Button(
            p.transform,
            "HUD-216-10D HelpButton",
            "HELP",
            new Vector2(820f, 0f),
            new Vector2(180f, 34f));
    }

    private static void WireLayerButtons()
    {
        foreach (KeyValuePair<string, GameObject> pair in Layers)
        {
            Button button = pair.Value.GetComponent<Button>();
            string layerName = pair.Key;

            button.onClick.AddListener(() =>
            {
                bool active = !pair.Value.GetComponent<Image>().enabled;
                pair.Value.GetComponent<Image>().enabled = active;

                if (alertStatus != null)
                    alertStatus.text =
                        "WEATHER LAYER // " +
                        layerName.ToUpper() +
                        (active ? " ENABLED" : " DISABLED");
            });
        }
    }

    private static void WireNavigationButtons()
    {
        WireSceneButton(
            "HUD-216-02A ReturnToLogisticsNetworkMapButton",
            "Assets/Scenes/SCN74_LOGISTICS_NETWORK/Logistics_Network.unity");

        WireSceneButton(
            "HUD-216-02B ReturnToResourceDistributionMapButton",
            "Assets/Scenes/SCN73_RESOURCE_DISTRIBUTION/Resource_Distribution.unity");

        WireSceneButton(
            "HUD-216-02C ReturnToStrategicHeatmapButton",
            "Assets/Scenes/SCN72_STRATEGIC_HEATMAP/Strategic_Heatmap.unity");

        WireSceneButton(
            "HUD-216-02D ReturnToFactionTerritoryOverlayButton",
            "Assets/Scenes/SCN71_FACTION_TERRITORY_OVERLAY/Faction_Territory_Overlay.unity");

        WireSceneButton(
            "HUD-216-02E ReturnToFactionControlMapButton",
            "Assets/Scenes/SCN70_FACTION_CONTROL/Faction_Control.unity");

        WireSceneButton(
            "HUD-216-02F ReturnToWarfareProfileButton",
            "Assets/Scenes/SCN69_WARFARE_PROFILE/Warfare_Profile.unity");

        WireSceneButton(
            "HUD-216-02G ReturnToEnvironmentProfileButton",
            "Assets/Scenes/SCN68_ENVIRONMENT_PROFILE/Environment_Profile.unity");

        WireSceneButton(
            "HUD-216-02H ReturnToIslandMapButton",
            "Assets/Scenes/SCN67_ISLAND_MAP/Island_Map.unity");

        WireSceneButton(
            "HUD-216-02I ReturnToBiomeMapButton",
            "Assets/Scenes/SCN66_BIOME_MAP/Biome_Map.unity");

        WireSceneButton(
            "HUD-216-02J ReturnToRegionMapButton",
            "Assets/Scenes/SCN65_REGION_MAP/Region_Map.unity");

        WireSceneButton(
            "HUD-216-02K ReturnToContinentMapButton",
            "Assets/Scenes/SCN64_CONTINENT_MAP/Continent_Map.unity");

        WireSceneButton(
            "HUD-216-02L ReturnToSurfaceMapButton",
            "Assets/Scenes/SCN63_PLANET_SURFACE/Planet_Surface_Map.unity");

        WireSceneButton(
            "HUD-216-02M ReturnToPlanetOverviewButton",
            "Assets/Scenes/SCN62_PLANET_OVERVIEW/Planet_Overview.unity");
    }

    private static void WireQuickActions()
    {
        Button enter = FindButton(
            "HUD-216-05J EnterWeatherZoneButton");

        if (enter != null)
        {
            enter.onClick.AddListener(() =>
            {
                if (alertStatus != null)
                    alertStatus.text =
                        "WEATHER ZONE // ACCESS GRANTED";
            });
        }

        string[] actionNames =
        {
            "HUD-216-08A DeployReconButton",
            "HUD-216-08B DeployLogisticsTeamButton",
            "HUD-216-08C DeployWeatherMonitoringTeamButton",
            "HUD-216-08D OpenIntelNetworkButton",
            "HUD-216-08E OpenDoctrineManagerButton",
            "HUD-216-08F OpenLogisticsOverviewButton",
            "HUD-216-08G OpenFactionDiplomacyButton"
        };

        string[] messages =
        {
            "RECON TEAM // DEPLOYMENT QUEUED",
            "LOGISTICS TEAM // DEPLOYMENT QUEUED",
            "WEATHER TEAM // DEPLOYMENT QUEUED",
            "INTEL NETWORK // OPENING",
            "DOCTRINE MANAGER // OPENING",
            "LOGISTICS OVERVIEW // OPENING",
            "FACTION DIPLOMACY // OPENING"
        };

        for (int i = 0; i < actionNames.Length; i++)
        {
            Button b = FindButton(actionNames[i]);
            if (b == null)
                continue;

            string message = messages[i];

            b.onClick.AddListener(() =>
            {
                if (alertStatus != null)
                    alertStatus.text = message;
            });
        }
    }

    private static void WireBottomButtons()
    {
        Button back = FindButton(
            "HUD-216-10A BackButton");

        if (back != null)
        {
            back.onClick.AddListener(() =>
            {
                if (alertStatus != null)
                    alertStatus.text = "BACK // PREVIOUS MAP";
            });
        }

        Button command = FindButton(
            "HUD-216-10B ExitToCommandCenterButton");

        if (command != null)
        {
            command.onClick.AddListener(() =>
            {
                if (alertStatus != null)
                    alertStatus.text =
                        "COMMAND CENTER // REQUESTED";
            });
        }

        Button settings = FindButton(
            "HUD-216-10C SettingsButton");

        if (settings != null)
        {
            settings.onClick.AddListener(() =>
            {
                if (alertStatus != null)
                    alertStatus.text = "SETTINGS // REQUESTED";
            });
        }

        Button help = FindButton(
            "HUD-216-10D HelpButton");

        if (help != null)
        {
            help.onClick.AddListener(() =>
            {
                if (alertStatus != null)
                    alertStatus.text =
                        "HELP // WEATHER SYSTEM INTERFACE";
            });
        }
    }

    private static Button FindButton(string exactName)
    {
        GameObject go = GameObject.Find(exactName);

        if (go == null)
            return null;

        return go.GetComponent<Button>();
    }

    private static void WireSceneButton(
        string buttonName,
        string scenePath)
    {
        Button button = FindButton(buttonName);

        if (button == null)
            return;

        button.onClick.AddListener(() =>
        {
            if (!System.IO.File.Exists(scenePath))
            {
                Debug.LogWarning(
                    "[HUD-216] Scene not found: " +
                    scenePath);

                if (alertStatus != null)
                    alertStatus.text =
                        "NAVIGATION // SCENE NOT FOUND";

                return;
            }

            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

            SceneManager.LoadScene(
                System.IO.Path.GetFileNameWithoutExtension(scenePath));
        });
    }
}
