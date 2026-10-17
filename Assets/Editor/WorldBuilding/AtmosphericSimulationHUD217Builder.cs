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

public static class AtmosphericSimulationHUD217Builder
{
    private const string ScenePath =
        "Assets/Scenes/SCN75_ATMOSPHERIC_SIMULATION/Atmospheric_Simulation.unity";

    private static readonly Dictionary<string, GameObject> Layers =
        new Dictionary<string, GameObject>();

    private static float simulationTime = 0f;
    private static bool simulationRunning = false;
    private static float simulationSpeed = 1f;

    [MenuItem("Obsidian Protocol/Worlds/BUILD HUD-217 ATMOSPHERIC SIMULATION")]
    public static void Build()
    {
        EnsureFolder("Assets/Scenes");
        EnsureFolder("Assets/Scenes/SCN75_ATMOSPHERIC_SIMULATION");

        var scene = EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene,
            NewSceneMode.Single);

        Layers.Clear();
        simulationTime = 0f;
        simulationRunning = false;
        simulationSpeed = 1f;

        BuildWorld();
        BuildCamera();
        BuildEventSystem();
        BuildHUD();

        EditorSceneManager.SaveScene(scene, ScenePath);
        AddToBuildSettings(ScenePath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[HUD-217] Atmospheric Simulation build complete.");
    }

    private static void EnsureFolder(string path)
    {
        if (AssetDatabase.IsValidFolder(path))
            return;

        string parent = System.IO.Path.GetDirectoryName(path).Replace("\\", "/");
        string folder = System.IO.Path.GetFileName(path);

        if (!AssetDatabase.IsValidFolder(parent))
            EnsureFolder(parent);

        AssetDatabase.CreateFolder(parent, folder);
    }

    private static void AddToBuildSettings(string scenePath)
    {
        var scenes = new List<EditorBuildSettingsScene>(
            EditorBuildSettings.scenes);

        bool exists = false;

        foreach (var scene in scenes)
        {
            if (scene.path == scenePath)
            {
                exists = true;
                break;
            }
        }

        if (!exists)
        {
            scenes.Add(new EditorBuildSettingsScene(scenePath, true));
            EditorBuildSettings.scenes = scenes.ToArray();
        }
    }

    // ============================================================
    // WORLD
    // ============================================================

    private static void BuildWorld()
    {
        var world = new GameObject("ATMOSPHERIC SIMULATION WORLD");

        CreateGround(world.transform);
        CreateAtmosphere(world.transform);
        CreateCloudLayer(world.transform);
        CreateStormSystems(world.transform);
        CreateWindSystems(world.transform);
        CreateWeatherSystems(world.transform);
        CreateFactionZones(world.transform);
        CreateTerrainModifiers(world.transform);
        CreateStrategicGrid(world.transform);
    }

    private static void CreateGround(Transform parent)
    {
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "AtmosphericTerrain";
        ground.transform.SetParent(parent);
        ground.transform.position = Vector3.zero;
        ground.transform.localScale = new Vector3(20f, 1f, 20f);

        Material mat = NewMaterial(
            "Atmospheric Terrain",
            new Color(0.10f, 0.15f, 0.18f));

        ground.GetComponent<Renderer>().sharedMaterial = mat;

        GameObject[] markers =
        {
            CreateMarker("MountainMass_01", new Vector3(-35f, 1f, 25f), 10f, parent),
            CreateMarker("MountainMass_02", new Vector3(30f, 1f, 30f), 13f, parent),
            CreateMarker("MountainMass_03", new Vector3(0f, 1f, -30f), 8f, parent)
        };
    }

    private static GameObject CreateMarker(
        string name,
        Vector3 position,
        float scale,
        Transform parent)
    {
        GameObject marker = GameObject.CreatePrimitive(
            PrimitiveType.Sphere);

        marker.name = name;
        marker.transform.SetParent(parent);
        marker.transform.position = position;
        marker.transform.localScale =
            new Vector3(scale, scale * 0.45f, scale);

        marker.GetComponent<Renderer>().sharedMaterial =
            NewMaterial(name + " Material",
                new Color(0.18f, 0.23f, 0.26f));

        return marker;
    }

    private static void CreateAtmosphere(Transform parent)
    {
        GameObject atmosphere = new GameObject("ATMOSPHERE_LAYERS");
        atmosphere.transform.SetParent(parent);

        CreateAtmosphereRing(
            "TemperatureField",
            new Vector3(0f, 5f, 0f),
            65f,
            0.20f,
            new Color(0.85f, 0.35f, 0.20f),
            atmosphere.transform);

        CreateAtmosphereRing(
            "HumidityField",
            new Vector3(0f, 8f, 0f),
            60f,
            0.15f,
            new Color(0.20f, 0.55f, 0.90f),
            atmosphere.transform);

        CreateAtmosphereRing(
            "PressureField",
            new Vector3(0f, 12f, 0f),
            55f,
            0.12f,
            new Color(0.55f, 0.30f, 0.85f),
            atmosphere.transform);

        CreateAtmosphereRing(
            "AirDensityField",
            new Vector3(0f, 16f, 0f),
            50f,
            0.10f,
            new Color(0.25f, 0.75f, 0.65f),
            atmosphere.transform);
    }

    private static void CreateAtmosphereRing(
        string name,
        Vector3 position,
        float size,
        float thickness,
        Color color,
        Transform parent)
    {
        GameObject obj = GameObject.CreatePrimitive(
            PrimitiveType.Cylinder);

        obj.name = name;
        obj.transform.SetParent(parent);
        obj.transform.position = position;
        obj.transform.localScale =
            new Vector3(size, thickness, size);

        obj.GetComponent<Renderer>().sharedMaterial =
            NewMaterial(name + " Material", color);
    }

    private static void CreateCloudLayer(Transform parent)
    {
        GameObject clouds = new GameObject("CLOUD_LAYER");
        clouds.transform.SetParent(parent);

        for (int i = 0; i < 18; i++)
        {
            float angle = i * 20f * Mathf.Deg2Rad;
            float radius = 18f + (i % 5) * 7f;

            Vector3 pos = new Vector3(
                Mathf.Cos(angle) * radius,
                14f + (i % 4) * 2f,
                Mathf.Sin(angle) * radius);

            GameObject cloud = GameObject.CreatePrimitive(
                PrimitiveType.Sphere);

            cloud.name = "CloudMass_" + (i + 1).ToString("00");
            cloud.transform.SetParent(clouds.transform);
            cloud.transform.position = pos;
            cloud.transform.localScale =
                new Vector3(8f, 1.8f, 5f);

            cloud.GetComponent<Renderer>().sharedMaterial =
                NewMaterial(
                    cloud.name + " Material",
                    new Color(0.65f, 0.70f, 0.75f));
        }

        Layers["Visibility"] = clouds;
    }

    private static void CreateStormSystems(Transform parent)
    {
        GameObject storms = new GameObject("STORM_SYSTEMS");
        storms.transform.SetParent(parent);

        for (int i = 0; i < 7; i++)
        {
            float x = -40f + i * 13f;
            float z = 22f * Mathf.Sin(i * 1.3f);

            GameObject storm = GameObject.CreatePrimitive(
                PrimitiveType.Sphere);

            storm.name = "StormCell_" + (i + 1).ToString("00");
            storm.transform.SetParent(storms.transform);
            storm.transform.position =
                new Vector3(x, 19f + (i % 3) * 2f, z);
            storm.transform.localScale =
                new Vector3(5f, 2f + i % 3, 5f);

            storm.GetComponent<Renderer>().sharedMaterial =
                NewMaterial(
                    storm.name + " Material",
                    new Color(0.28f, 0.22f, 0.32f));
        }

        Layers["StormCells"] = storms;
        Layers["WeatherSystems"] = storms;
    }

    private static void CreateWindSystems(Transform parent)
    {
        GameObject wind = new GameObject("WIND_SYSTEMS");
        wind.transform.SetParent(parent);

        for (int i = 0; i < 20; i++)
        {
            float x = -45f + (i % 5) * 22f;
            float z = -40f + (i / 5) * 22f;

            GameObject arrow = GameObject.CreatePrimitive(
                PrimitiveType.Cube);

            arrow.name = "WindVector_" + (i + 1).ToString("00");
            arrow.transform.SetParent(wind.transform);
            arrow.transform.position =
                new Vector3(x, 3f, z);
            arrow.transform.localScale =
                new Vector3(7f, 0.18f, 0.55f);

            arrow.transform.rotation =
                Quaternion.Euler(0f, 20f + i * 7f, 0f);

            arrow.GetComponent<Renderer>().sharedMaterial =
                NewMaterial(
                    arrow.name + " Material",
                    new Color(0.15f, 0.65f, 0.85f));
        }

        Layers["WindSpeed"] = wind;
        Layers["TurbulenceZones"] = wind;
    }

    private static void CreateWeatherSystems(Transform parent)
    {
        GameObject weather = new GameObject("WEATHER_SYSTEMS");
        weather.transform.SetParent(parent);

        GameObject precipitation = new GameObject("PrecipitationZones");
        precipitation.transform.SetParent(weather.transform);

        for (int i = 0; i < 12; i++)
        {
            GameObject zone = GameObject.CreatePrimitive(
                PrimitiveType.Cylinder);

            zone.name = "PrecipitationZone_" +
                        (i + 1).ToString("00");

            zone.transform.SetParent(precipitation.transform);
            zone.transform.position =
                new Vector3(
                    -40f + i * 7f,
                    0.4f,
                    30f * Mathf.Sin(i * 0.8f));

            zone.transform.localScale =
                new Vector3(3f, 0.2f, 3f);

            zone.GetComponent<Renderer>().sharedMaterial =
                NewMaterial(
                    zone.name + " Material",
                    new Color(0.18f, 0.45f, 0.75f));
        }

        Layers["Precipitation"] = precipitation;
        Layers["WeatherSystems"] = weather;
    }

    private static void CreateFactionZones(Transform parent)
    {
        GameObject faction = new GameObject("FACTION_CONTROL");
        faction.transform.SetParent(parent);

        for (int i = 0; i < 6; i++)
        {
            GameObject zone = GameObject.CreatePrimitive(
                PrimitiveType.Cube);

            zone.name = "FactionAtmosphericZone_" +
                        (i + 1).ToString("00");

            zone.transform.SetParent(faction.transform);
            zone.transform.position =
                new Vector3(
                    -40f + i * 16f,
                    0.25f,
                    -30f + (i % 2) * 35f);

            zone.transform.localScale =
                new Vector3(14f, 0.15f, 12f);

            zone.GetComponent<Renderer>().sharedMaterial =
                NewMaterial(
                    zone.name + " Material",
                    new Color(0.35f, 0.25f, 0.55f));
        }

        Layers["FactionControl"] = faction;
    }

    private static void CreateTerrainModifiers(Transform parent)
    {
        GameObject terrain = new GameObject("TERRAIN_MODIFIERS");
        terrain.transform.SetParent(parent);

        for (int i = 0; i < 10; i++)
        {
            GameObject modifier = GameObject.CreatePrimitive(
                PrimitiveType.Cube);

            modifier.name =
                "TerrainModifier_" + (i + 1).ToString("00");

            modifier.transform.SetParent(terrain.transform);
            modifier.transform.position =
                new Vector3(
                    -45f + i * 10f,
                    0.15f,
                    -10f + Mathf.Sin(i) * 20f);

            modifier.transform.localScale =
                new Vector3(5f, 0.1f, 5f);

            modifier.GetComponent<Renderer>().sharedMaterial =
                NewMaterial(
                    modifier.name + " Material",
                    new Color(0.40f, 0.32f, 0.20f));
        }

        Layers["TerrainModifiers"] = terrain;
    }

    private static void CreateStrategicGrid(Transform parent)
    {
        GameObject grid = new GameObject("STRATEGIC_GRID");
        grid.transform.SetParent(parent);

        for (int i = -5; i <= 5; i++)
        {
            GameObject lineX = GameObject.CreatePrimitive(
                PrimitiveType.Cube);

            lineX.name = "GridX_" + i;
            lineX.transform.SetParent(grid.transform);
            lineX.transform.position =
                new Vector3(i * 10f, 0.08f, 0f);
            lineX.transform.localScale =
                new Vector3(0.04f, 0.03f, 100f);

            GameObject lineZ = GameObject.CreatePrimitive(
                PrimitiveType.Cube);

            lineZ.name = "GridZ_" + i;
            lineZ.transform.SetParent(grid.transform);
            lineZ.transform.position =
                new Vector3(0f, 0.08f, i * 10f);
            lineZ.transform.localScale =
                new Vector3(100f, 0.03f, 0.04f);
        }

        Layers["LogisticsRoutes"] = grid;
    }

    private static Material NewMaterial(string name, Color color)
    {
        Shader shader =
            Shader.Find("Universal Render Pipeline/Lit");

        if (shader == null)
            shader = Shader.Find("Standard");

        Material mat = new Material(shader);
        mat.name = name;
        mat.color = color;
        return mat;
    }

    // ============================================================
    // CAMERA / EVENT SYSTEM
    // ============================================================

    private static void BuildCamera()
    {
        GameObject cameraObject = new GameObject("AtmosphericSimulationCamera");
        Camera cam = cameraObject.AddComponent<Camera>();

        cameraObject.transform.position =
            new Vector3(0f, 100f, -80f);

        cameraObject.transform.rotation =
            Quaternion.Euler(48f, 0f, 0f);

        cam.orthographic = true;
        cam.orthographicSize = 72f;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor =
            new Color(0.015f, 0.025f, 0.04f);

        cam.tag = "MainCamera";
    }

    private static void BuildEventSystem()
    {
        if (EventSystem.current != null)
            return;

        GameObject es = new GameObject("EventSystem");
        es.AddComponent<EventSystem>();
        es.AddComponent<InputSystemUIInputModule>();
    }

    // ============================================================
    // HUD
    // ============================================================

    private static void BuildHUD()
    {
        GameObject canvasObject =
            new GameObject("HUD-217 AtmosphericSimulationHUD");

        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler =
            canvasObject.AddComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution =
            new Vector2(1920f, 1080f);

        canvasObject.AddComponent<GraphicRaycaster>();

        BuildTopBar(canvasObject.transform);
        BuildNavigation(canvasObject.transform);
        BuildAtmosphericLayers(canvasObject.transform);
        BuildSimulationControl(canvasObject.transform);
        BuildAtmosphericStats(canvasObject.transform);
        BuildAtmosphericInfo(canvasObject.transform);
        BuildImpactAnalysis(canvasObject.transform);
        BuildForecast(canvasObject.transform);
        BuildQuickActions(canvasObject.transform);
        BuildAlerts(canvasObject.transform);
        BuildBottomBar(canvasObject.transform);

        AddRuntimeController(canvasObject);
    }

    private static GameObject Panel(
        Transform parent,
        string name,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 offsetMin,
        Vector2 offsetMax)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);

        RectTransform rect =
            obj.AddComponent<RectTransform>();

        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = offsetMin;
        rect.offsetMax = offsetMax;

        Image image = obj.AddComponent<Image>();
        image.color =
            new Color(0.025f, 0.04f, 0.055f, 0.94f);

        return obj;
    }

    private static TextMeshProUGUI Text(
        Transform parent,
        string name,
        string value,
        int size,
        TextAlignmentOptions alignment = TextAlignmentOptions.Left)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);

        RectTransform rect =
            obj.AddComponent<RectTransform>();

        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin =
            new Vector2(8f, 3f);
        rect.offsetMax =
            new Vector2(-8f, -3f);

        TextMeshProUGUI tmp =
            obj.AddComponent<TextMeshProUGUI>();

        tmp.text = value;
        tmp.fontSize = size;
        tmp.alignment = alignment;
        tmp.color = Color.white;
        tmp.textWrappingMode =
            TextWrappingModes.Normal;

        return tmp;
    }

    private static Button Button(
        Transform parent,
        string name,
        string label,
        UnityEngine.Events.UnityAction action)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);

        RectTransform rect =
            obj.AddComponent<RectTransform>();

        Image image = obj.AddComponent<Image>();
        image.color =
            new Color(0.08f, 0.13f, 0.17f, 0.98f);

        Button button =
            obj.AddComponent<Button>();

        if (action != null)
            button.onClick.AddListener(action);

        Text(
            obj.transform,
            "Label",
            label,
            13,
            TextAlignmentOptions.Center);

        return button;
    }

    private static void ToggleButton(
        Transform parent,
        string name,
        string label,
        string layer)
    {
        Button b = Button(
            parent,
            name,
            label,
            () => ToggleLayer(layer));

        Layers.TryGetValue(layer, out GameObject target);

        if (target != null)
            target.SetActive(true);
    }

    private static void BuildTopBar(Transform root)
    {
        GameObject panel = Panel(
            root,
            "HUD-217-01 TopBar",
            new Vector2(0, .91f),
            new Vector2(1, 1),
            Vector2.zero,
            Vector2.zero);

        Text(panel.transform,
            "HUD-217-01A PlanetNameDisplay",
            "PLANET // AURELIA",
            22);

        Text(panel.transform,
            "HUD-217-01B RegionNameDisplay",
            "REGION // NORTHERN CONTINENT",
            15);

        Text(panel.transform,
            "HUD-217-01C AtmosphericModelDisplay",
            "ATMOSPHERIC MODEL // ATMO-X9",
            15);

        Text(panel.transform,
            "HUD-217-01D SimulationModeDisplay",
            "SIMULATION // REAL-TIME",
            15);

        Text(panel.transform,
            "HUD-217-01E OwnerFactionEmblem",
            "[ PLAYER FACTION ]",
            15);

        Text(panel.transform,
            "HUD-217-01F PlayerResourcesPanel",
            "RESOURCES  84,250   ENERGY  62,400",
            15);

        Text(panel.transform,
            "HUD-217-01G TimeControlPanel",
            "TIME  14:32:08",
            15);
    }

    private static void BuildNavigation(Transform root)
    {
        GameObject panel = Panel(
            root,
            "HUD-217-02 NavigationPanel",
            new Vector2(0, .25f),
            new Vector2(.18f, .90f),
            Vector2.zero,
            Vector2.zero);

        AddNav(panel.transform,
            "HUD-217-02A ReturnToWeatherSystemViewButton",
            "WEATHER SYSTEM VIEW",
            "Weather_System");

        AddNav(panel.transform,
            "HUD-217-02B ReturnToLogisticsNetworkMapButton",
            "LOGISTICS NETWORK",
            "Logistics_Network");

        AddNav(panel.transform,
            "HUD-217-02C ReturnToResourceDistributionMapButton",
            "RESOURCE DISTRIBUTION",
            "Resource_Distribution");

        AddNav(panel.transform,
            "HUD-217-02D ReturnToStrategicHeatmapButton",
            "STRATEGIC HEATMAP",
            "Strategic_Heatmap");

        AddNav(panel.transform,
            "HUD-217-02E ReturnToFactionTerritoryOverlayButton",
            "FACTION TERRITORY",
            "Faction_Territory");

        AddNav(panel.transform,
            "HUD-217-02F ReturnToFactionControlMapButton",
            "FACTION CONTROL",
            "Faction_Control");

        AddNav(panel.transform,
            "HUD-217-02G ReturnToWarfareProfileButton",
            "WARFARE PROFILE",
            "Warfare_Profile");

        AddNav(panel.transform,
            "HUD-217-02H ReturnToEnvironmentProfileButton",
            "ENVIRONMENT PROFILE",
            "Environment_Profile");

        AddNav(panel.transform,
            "HUD-217-02I ReturnToIslandMapButton",
            "ISLAND MAP",
            "Island_Map");

        AddNav(panel.transform,
            "HUD-217-02J ReturnToBiomeMapButton",
            "BIOME MAP",
            "Biome_Map");

        AddNav(panel.transform,
            "HUD-217-02K ReturnToRegionMapButton",
            "REGION MAP",
            "Region_Map");

        AddNav(panel.transform,
            "HUD-217-02L ReturnToContinentMapButton",
            "CONTINENT MAP",
            "Continent_Map");

        AddNav(panel.transform,
            "HUD-217-02M ReturnToSurfaceMapButton",
            "SURFACE MAP",
            "Planet_Surface");

        AddNav(panel.transform,
            "HUD-217-02N ReturnToPlanetOverviewButton",
            "PLANET OVERVIEW",
            "Planet_Overview");
    }

    private static void AddNav(
        Transform parent,
        string name,
        string label,
        string keyword)
    {
        Button(
            parent,
            name,
            label,
            () => LoadSceneKeyword(keyword));
    }

    private static void BuildAtmosphericLayers(Transform root)
    {
        GameObject panel = Panel(
            root,
            "HUD-217-03 AtmosphericLayersPanel",
            new Vector2(.18f, .42f),
            new Vector2(.35f, .90f),
            Vector2.zero,
            Vector2.zero);

        ToggleButton(panel.transform,
            "HUD-217-03A LayerToggle_Temperature",
            "TEMPERATURE",
            "TemperatureField");

        ToggleButton(panel.transform,
            "HUD-217-03B LayerToggle_Humidity",
            "HUMIDITY",
            "HumidityField");

        ToggleButton(panel.transform,
            "HUD-217-03C LayerToggle_WindSpeed",
            "WIND SPEED",
            "WindSpeed");

        ToggleButton(panel.transform,
            "HUD-217-03D LayerToggle_Precipitation",
            "PRECIPITATION",
            "Precipitation");

        ToggleButton(panel.transform,
            "HUD-217-03E LayerToggle_StormCells",
            "STORM CELLS",
            "StormCells");

        ToggleButton(panel.transform,
            "HUD-217-03F LayerToggle_Visibility",
            "VISIBILITY",
            "Visibility");

        ToggleButton(panel.transform,
            "HUD-217-03G LayerToggle_AtmosphericPressure",
            "PRESSURE",
            "PressureField");

        ToggleButton(panel.transform,
            "HUD-217-03H LayerToggle_AirDensity",
            "AIR DENSITY",
            "AirDensityField");

        ToggleButton(panel.transform,
            "HUD-217-03I LayerToggle_TurbulenceZones",
            "TURBULENCE",
            "TurbulenceZones");

        ToggleButton(panel.transform,
            "HUD-217-03J LayerToggle_WeatherSystems",
            "WEATHER SYSTEMS",
            "WeatherSystems");

        ToggleButton(panel.transform,
            "HUD-217-03K LayerToggle_FactionControl",
            "FACTION CONTROL",
            "FactionControl");

        ToggleButton(panel.transform,
            "HUD-217-03L LayerToggle_WarfareZones",
            "WARFARE ZONES",
            "StormCells");

        ToggleButton(panel.transform,
            "HUD-217-03M LayerToggle_LogisticsRoutes",
            "LOGISTICS ROUTES",
            "LogisticsRoutes");

        ToggleButton(panel.transform,
            "HUD-217-03N LayerToggle_TerrainModifiers",
            "TERRAIN MODIFIERS",
            "TerrainModifiers");
    }

    private static void BuildSimulationControl(Transform root)
    {
        GameObject panel = Panel(
            root,
            "HUD-217-04 SimulationControlPanel",
            new Vector2(.35f, .78f),
            new Vector2(.58f, .90f),
            Vector2.zero,
            Vector2.zero);

        Button(
            panel.transform,
            "HUD-217-04A SimulationStartButton",
            "START",
            StartSimulation);

        Button(
            panel.transform,
            "HUD-217-04B SimulationPauseButton",
            "PAUSE",
            PauseSimulation);

        Button(
            panel.transform,
            "HUD-217-04C SimulationResetButton",
            "RESET",
            ResetSimulation);

        GameObject sliderObj = new GameObject(
            "HUD-217-04D SimulationSpeedSlider");

        sliderObj.transform.SetParent(panel.transform, false);

        Slider slider = sliderObj.AddComponent<Slider>();
        slider.minValue = .25f;
        slider.maxValue = 4f;
        slider.value = 1f;
        slider.onValueChanged.AddListener(
            value => simulationSpeed = value);

        RectTransform sr =
            sliderObj.GetComponent<RectTransform>();

        sr.anchorMin = new Vector2(.05f, .15f);
        sr.anchorMax = new Vector2(.95f, .28f);
        sr.offsetMin = Vector2.zero;
        sr.offsetMax = Vector2.zero;

        GameObject dropdownObj = new GameObject(
            "HUD-217-04E SimulationModeSelector");

        dropdownObj.transform.SetParent(panel.transform, false);

        TMP_Dropdown dropdown =
            dropdownObj.AddComponent<TMP_Dropdown>();

        dropdown.options = new List<TMP_Dropdown.OptionData>
        {
            new TMP_Dropdown.OptionData("REAL-TIME"),
            new TMP_Dropdown.OptionData("FAST SIMULATION"),
            new TMP_Dropdown.OptionData("HIGH ACCURACY"),
            new TMP_Dropdown.OptionData("SCENARIO TEST")
        };

        RectTransform dr =
            dropdownObj.GetComponent<RectTransform>();

        dr.anchorMin = new Vector2(.05f, .02f);
        dr.anchorMax = new Vector2(.50f, .14f);
        dr.offsetMin = Vector2.zero;
        dr.offsetMax = Vector2.zero;

        Text(
            panel.transform,
            "HUD-217-04F SimulationAccuracyDisplay",
            "MODEL ACCURACY // 97.8%",
            13);
    }

    private static void BuildAtmosphericStats(Transform root)
    {
        GameObject panel = Panel(
            root,
            "HUD-217-05 AtmosphericStatsPanel",
            new Vector2(.72f, .46f),
            new Vector2(.99f, .90f),
            Vector2.zero,
            Vector2.zero);

        Text(panel.transform,
            "HUD-217-05A TemperatureDisplay",
            "TEMPERATURE     18.4 °C",
            15);

        Text(panel.transform,
            "HUD-217-05B HumidityDisplay",
            "HUMIDITY        64%",
            15);

        Text(panel.transform,
            "HUD-217-05C WindSpeedDisplay",
            "WIND SPEED      38 km/h",
            15);

        Text(panel.transform,
            "HUD-217-05D PrecipitationDisplay",
            "PRECIPITATION   12 mm/h",
            15);

        Text(panel.transform,
            "HUD-217-05E StormCellIntensityDisplay",
            "STORM INTENSITY 72%",
            15);

        Text(panel.transform,
            "HUD-217-05F VisibilityDisplay",
            "VISIBILITY      8.7 km",
            15);

        Text(panel.transform,
            "HUD-217-05G AtmosphericPressureDisplay",
            "PRESSURE        1008 hPa",
            15);

        Text(panel.transform,
            "HUD-217-05H AirDensityDisplay",
            "AIR DENSITY     1.21 kg/m³",
            15);

        Text(panel.transform,
            "HUD-217-05I TurbulenceLevelDisplay",
            "TURBULENCE      MODERATE",
            15);
    }

    private static void BuildAtmosphericInfo(Transform root)
    {
        GameObject panel = Panel(
            root,
            "HUD-217-06 AtmosphericInfoPanel",
            new Vector2(.72f, .20f),
            new Vector2(.99f, .46f),
            Vector2.zero,
            Vector2.zero);

        Text(panel.transform,
            "HUD-217-06A SelectedZoneName",
            "SELECTED ZONE // STORM ALPHA",
            14);

        Text(panel.transform,
            "HUD-217-06B SelectedZoneType",
            "TYPE // SUPERCELL",
            14);

        Text(panel.transform,
            "HUD-217-06C SelectedTemperature",
            "TEMP // 14.2 °C",
            13);

        Text(panel.transform,
            "HUD-217-06D SelectedHumidity",
            "HUMIDITY // 81%",
            13);

        Text(panel.transform,
            "HUD-217-06E SelectedWindSpeed",
            "WIND // 74 km/h",
            13);

        Text(panel.transform,
            "HUD-217-06F SelectedPrecipitationLevel",
            "PRECIPITATION // HEAVY",
            13);

        Text(panel.transform,
            "HUD-217-06G SelectedStormCellIntensity",
            "STORM CELL // 86%",
            13);

        Text(panel.transform,
            "HUD-217-06H SelectedVisibility",
            "VISIBILITY // 2.4 km",
            13);

        Text(panel.transform,
            "HUD-217-06I SelectedAtmosphericPressure",
            "PRESSURE // 994 hPa",
            13);

        Text(panel.transform,
            "HUD-217-06J SelectedAirDensity",
            "AIR DENSITY // 1.18 kg/m³",
            13);

        Text(panel.transform,
            "HUD-217-06K SelectedTurbulenceLevel",
            "TURBULENCE // HIGH",
            13);

        Button(
            panel.transform,
            "HUD-217-06L EnterZoneButton",
            "ENTER ZONE",
            () => Debug.Log(
                "[HUD-217] Enter atmospheric zone."));
    }

    private static void BuildImpactAnalysis(Transform root)
    {
        GameObject panel = Panel(
            root,
            "HUD-217-07 ImpactAnalysisPanel",
            new Vector2(.35f, .42f),
            new Vector2(.58f, .78f),
            Vector2.zero,
            Vector2.zero);

        Text(panel.transform,
            "HUD-217-07A MobilityImpactDisplay",
            "MOBILITY IMPACT       -22%",
            14);

        Text(panel.transform,
            "HUD-217-07B ReconVisibilityImpactDisplay",
            "RECON VISIBILITY      -31%",
            14);

        Text(panel.transform,
            "HUD-217-07C SensorInterferenceImpactDisplay",
            "SENSOR INTERFERENCE   +18%",
            14);

        Text(panel.transform,
            "HUD-217-07D LogisticsDifficultyImpactDisplay",
            "LOGISTICS DIFFICULTY  +27%",
            14);

        Text(panel.transform,
            "HUD-217-07E CombatEffectivenessImpactDisplay",
            "COMBAT EFFECTIVENESS  -14%",
            14);

        Text(panel.transform,
            "HUD-217-07F EnvironmentalHazardLevelDisplay",
            "ENVIRONMENTAL HAZARD  HIGH",
            14);
    }

    private static void BuildForecast(Transform root)
    {
        GameObject panel = Panel(
            root,
            "HUD-217-08 ForecastPanel",
            new Vector2(.35f, .20f),
            new Vector2(.58f, .42f),
            Vector2.zero,
            Vector2.zero);

        Text(panel.transform,
            "HUD-217-08A ShortTermForecastDisplay",
            "0-6 HR  // STORM INTENSIFICATION",
            13);

        Text(panel.transform,
            "HUD-217-08B MidTermForecastDisplay",
            "6-24 HR // PRECIPITATION DECREASING",
            13);

        Text(panel.transform,
            "HUD-217-08C LongTermForecastDisplay",
            "24-72 HR // STABLE ATMOSPHERIC CYCLE",
            13);

        Text(panel.transform,
            "HUD-217-08D StormTrajectoryDisplay",
            "STORM TRAJECTORY // EAST-NORTHEAST",
            13);

        Text(panel.transform,
            "HUD-217-08E WeatherShiftProbabilityDisplay",
            "WEATHER SHIFT PROBABILITY // 63%",
            13);

        Text(panel.transform,
            "HUD-217-08F AtmosphericInstabilityDisplay",
            "ATMOSPHERIC INSTABILITY // MODERATE",
            13);
    }

    private static void BuildQuickActions(Transform root)
    {
        GameObject panel = Panel(
            root,
            "HUD-217-09 QuickActionsPanel",
            new Vector2(.59f, .20f),
            new Vector2(.72f, .90f),
            Vector2.zero,
            Vector2.zero);

        Quick(panel.transform,
            "HUD-217-09A DeployReconButton",
            "DEPLOY RECON");

        Quick(panel.transform,
            "HUD-217-09B DeployLogisticsTeamButton",
            "LOGISTICS TEAM");

        Quick(panel.transform,
            "HUD-217-09C DeployWeatherMonitoringTeamButton",
            "WEATHER TEAM");

        Quick(panel.transform,
            "HUD-217-09D DeployAtmosphericDroneButton",
            "ATMOSPHERIC DRONE");

        Quick(panel.transform,
            "HUD-217-09E OpenIntelNetworkButton",
            "INTEL NETWORK");

        Quick(panel.transform,
            "HUD-217-09F OpenDoctrineManagerButton",
            "DOCTRINE MANAGER");

        Quick(panel.transform,
            "HUD-217-09G OpenLogisticsOverviewButton",
            "LOGISTICS OVERVIEW");

        Quick(panel.transform,
            "HUD-217-09H OpenFactionDiplomacyButton",
            "FACTION DIPLOMACY");
    }

    private static void Quick(
        Transform parent,
        string name,
        string label)
    {
        Button(
            parent,
            name,
            label,
            () => Debug.Log(
                "[HUD-217] Quick action: " + label));
    }

    private static void BuildAlerts(Transform root)
    {
        GameObject panel = Panel(
            root,
            "HUD-217-10 AlertsPanel",
            new Vector2(0, .08f),
            new Vector2(.35f, .25f),
            Vector2.zero,
            Vector2.zero);

        Text(panel.transform,
            "HUD-217-10A StrategicAlertEntry",
            "STRATEGIC // ATMOSPHERIC FRONT APPROACHING",
            12);

        Text(panel.transform,
            "HUD-217-10B WeatherAlertEntry",
            "WEATHER // SEVERE STORM CELL",
            12);

        Text(panel.transform,
            "HUD-217-10C EnvironmentalHazardAlertEntry",
            "HAZARD // FLOODING RISK ELEVATED",
            12);

        Text(panel.transform,
            "HUD-217-10D LogisticsAlertEntry",
            "LOGISTICS // TRANSPORT EFFICIENCY REDUCED",
            12);

        Text(panel.transform,
            "HUD-217-10E WarfareAlertEntry",
            "WARFARE // LOW VISIBILITY CONDITIONS",
            12);

        Text(panel.transform,
            "HUD-217-10F StormTrajectoryAlertEntry",
            "STORM // TRAJECTORY UPDATED",
            12);

        Text(panel.transform,
            "HUD-217-10G VisibilityDropAlertEntry",
            "VISIBILITY // RAPID DROP DETECTED",
            12);

        Text(panel.transform,
            "HUD-217-10H AtmosphericInstabilityAlertEntry",
            "ATMOSPHERE // INSTABILITY RISING",
            12);
    }

    private static void BuildBottomBar(Transform root)
    {
        GameObject panel = Panel(
            root,
            "HUD-217-11 BottomBar",
            new Vector2(0, 0),
            new Vector2(1, .07f),
            Vector2.zero,
            Vector2.zero);

        Button(
            panel.transform,
            "HUD-217-11A BackButton",
            "BACK",
            () => LoadSceneKeyword("Logistics_Network"));

        Button(
            panel.transform,
            "HUD-217-11B ExitToCommandCenterButton",
            "COMMAND CENTER",
            () => LoadSceneKeyword("Command_Center"));

        Button(
            panel.transform,
            "HUD-217-11C SettingsButton",
            "SETTINGS",
            () => LoadSceneKeyword("Settings"));

        Button(
            panel.transform,
            "HUD-217-11D HelpButton",
            "HELP",
            () => Debug.Log("[HUD-217] Atmospheric Simulation Help"));
    }

    // ============================================================
    // RUNTIME
    // ============================================================

    private static void AddRuntimeController(GameObject canvas)
    {
        var controller =
            canvas.AddComponent<AtmosphericSimulationRuntime217>();

        controller.Initialize();
    }

    private static void ToggleLayer(string layer)
    {
        if (!Layers.TryGetValue(layer, out GameObject target) ||
            target == null)
        {
            Debug.LogWarning(
                "[HUD-217] Layer not found: " + layer);
            return;
        }

        target.SetActive(!target.activeSelf);

        Debug.Log(
            "[HUD-217] " + layer +
            " = " + target.activeSelf);
    }

    private static void StartSimulation()
    {
        simulationRunning = true;
        Debug.Log("[HUD-217] Atmospheric simulation STARTED.");
    }

    private static void PauseSimulation()
    {
        simulationRunning = false;
        Debug.Log("[HUD-217] Atmospheric simulation PAUSED.");
    }

    private static void ResetSimulation()
    {
        simulationTime = 0f;
        simulationRunning = false;
        Debug.Log("[HUD-217] Atmospheric simulation RESET.");
    }

    private static void LoadSceneKeyword(string keyword)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path =
                SceneUtility.GetScenePathByBuildIndex(i);

            if (string.IsNullOrEmpty(path))
                continue;

            string file =
                System.IO.Path.GetFileNameWithoutExtension(path);

            if (file.IndexOf(
                    keyword,
                    StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Debug.Log(
                    "[HUD-217] Loading scene: " + path);

                SceneManager.LoadScene(i);
                return;
            }
        }

        Debug.LogWarning(
            "[HUD-217] No Build Settings scene matched: " +
            keyword);
    }

    // ============================================================
    // RUNTIME COMPONENT
    // ============================================================

    private sealed class AtmosphericSimulationRuntime217
        : MonoBehaviour
    {
        private TextMeshProUGUI[] displays;

        public void Initialize()
        {
            displays =
                GetComponentsInChildren<TextMeshProUGUI>(
                    true);
        }

        private void Update()
        {
            if (!simulationRunning)
                return;

            simulationTime +=
                Time.deltaTime * simulationSpeed;

            UpdateSimulationDisplay();
        }

        private void UpdateSimulationDisplay()
        {
            foreach (var text in displays)
            {
                if (text == null)
                    continue;

                if (text.name ==
                    "HUD-217-01G TimeControlPanel")
                {
                    text.text =
                        "TIME  " +
                        FormatSimulationTime();
                }
            }
        }

        private string FormatSimulationTime()
        {
            int totalSeconds =
                Mathf.FloorToInt(simulationTime);

            int hours =
                (totalSeconds / 3600) % 24;

            int minutes =
                (totalSeconds / 60) % 60;

            int seconds =
                totalSeconds % 60;

            return string.Format(
                "{0:00}:{1:00}:{2:00}",
                hours,
                minutes,
                seconds);
        }
    }
}
