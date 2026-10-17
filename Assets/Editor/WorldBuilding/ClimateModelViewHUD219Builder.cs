using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class ClimateModelViewHUD219Builder
{
    private const string ScenePath =
        "Assets/Scenes/SCN77_CLIMATE_MODEL/Climate_Model.unity";

    [MenuItem("Obsidian Protocol/Worlds/BUILD HUD-219 CLIMATE MODEL")]
    public static void Build()
    {
        BuildHUD();
    }

    private static void BuildHUD()
    {
        EnsureFolders();

        Scene scene = EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene,
            NewSceneMode.Single);

        BuildWorld();

        GameObject canvasObject =
            new GameObject("HUD-219 ClimateModelViewHUD");

        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler =
            canvasObject.AddComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(1920f, 1080f);

        canvasObject.AddComponent<GraphicRaycaster>();

        BuildEventSystem();

        BuildTopBar(canvasObject.transform);
        BuildNavigation(canvasObject.transform);
        BuildClimateLayers(canvasObject.transform);
        BuildSimulationControl(canvasObject.transform);
        BuildClimateStats(canvasObject.transform);
        BuildClimateInfo(canvasObject.transform);
        BuildImpactAnalysis(canvasObject.transform);
        BuildForecast(canvasObject.transform);
        BuildQuickActions(canvasObject.transform);
        BuildAlerts(canvasObject.transform);
        BuildBottomBar(canvasObject.transform);

        canvasObject.AddComponent<ClimateModelViewHUD219Controller>();

        SaveAndRegister(scene);

        Debug.Log("[HUD-219] Climate Model HUD built successfully.");
    }

    private static void EnsureFolders()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Scenes/SCN77_CLIMATE_MODEL"))
            AssetDatabase.CreateFolder("Assets/Scenes", "SCN77_CLIMATE_MODEL");
    }

    private static void BuildWorld()
    {
        GameObject world =
            new GameObject("CLIMATE_MODEL_WORLD");

        GameObject surface =
            GameObject.CreatePrimitive(PrimitiveType.Sphere);

        surface.name = "PLANET_SURFACE_AURELIA";
        surface.transform.SetParent(world.transform, false);
        surface.transform.localScale =
            new Vector3(12f, 12f, 12f);

        Material surfaceMat =
            new Material(Shader.Find("Universal Render Pipeline/Lit"));

        surfaceMat.color =
            new Color(0.08f, 0.22f, 0.16f, 1f);

        surface.GetComponent<Renderer>().sharedMaterial =
            surfaceMat;

        GameObject atmosphere =
            GameObject.CreatePrimitive(PrimitiveType.Sphere);

        atmosphere.name = "ATMOSPHERIC_CLIMATE_SHELL";
        atmosphere.transform.SetParent(world.transform, false);
        atmosphere.transform.localScale =
            new Vector3(12.5f, 12.5f, 12.5f);

        Material atmosphereMat =
            new Material(Shader.Find("Universal Render Pipeline/Lit"));

        atmosphereMat.color =
            new Color(0.12f, 0.28f, 0.38f, 0.12f);

        atmosphere.GetComponent<Renderer>().sharedMaterial =
            atmosphereMat;

        Camera cameraObject =
            new GameObject("ClimateModelCamera")
                .AddComponent<Camera>();

        cameraObject.transform.position =
            new Vector3(0f, 0f, -24f);

        cameraObject.transform.LookAt(Vector3.zero);
    }

    private static void BuildEventSystem()
    {
        GameObject eventSystem =
            new GameObject("EventSystem");

        eventSystem.AddComponent<EventSystem>();
        eventSystem.AddComponent<InputSystemUIInputModule>();
    }

    private static void BuildTopBar(Transform root)
    {
        Transform panel =
            Panel(root, "HUD-219-01 TopBar",
                new Vector2(0f, 475f),
                new Vector2(1880f, 90f));

        Label(panel, "HUD-219-01A PlanetNameDisplay",
            "AURELIA", new Vector2(-760f, 20f),
            new Vector2(240f, 30f), 22f);

        Label(panel, "HUD-219-01B RegionNameDisplay",
            "NORTHERN CONTINENT", new Vector2(-490f, 20f),
            new Vector2(300f, 30f), 16f);

        Label(panel, "HUD-219-01C ClimateModelDisplay",
            "CLIMATE MODEL // CM-07", new Vector2(-140f, 20f),
            new Vector2(300f, 30f), 16f);

        Label(panel, "HUD-219-01D SimulationModeDisplay",
            "SIMULATION MODE // BALANCED", new Vector2(190f, 20f),
            new Vector2(300f, 30f), 15f);

        Label(panel, "HUD-219-01E OwnerFactionEmblem",
            "A", new Vector2(515f, 20f),
            new Vector2(50f, 50f), 24f);

        Label(panel, "HUD-219-01F PlayerResourcesPanel",
            "CREDITS 84,250 | ENERGY 72% | DATA 18,440",
            new Vector2(760f, 20f),
            new Vector2(430f, 40f), 13f);

        Label(panel, "HUD-219-01G TimeControlPanel",
            "TIME // 1.0x", new Vector2(770f, -20f),
            new Vector2(200f, 25f), 12f);
    }

    private static void BuildNavigation(Transform root)
    {
        Transform panel =
            Panel(root, "HUD-219-02 NavigationPanel",
                new Vector2(-820f, 70f),
                new Vector2(270f, 760f));

        string[] names =
        {
            "HUD-219-02A ReturnToStormCellAnalysisButton",
            "HUD-219-02B ReturnToAtmosphericSimulationButton",
            "HUD-219-02C ReturnToWeatherSystemViewButton",
            "HUD-219-02D ReturnToLogisticsNetworkMapButton",
            "HUD-219-02E ReturnToResourceDistributionMapButton",
            "HUD-219-02F ReturnToStrategicHeatmapButton",
            "HUD-219-02G ReturnToFactionTerritoryOverlayButton",
            "HUD-219-02H ReturnToFactionControlMapButton",
            "HUD-219-02I ReturnToWarfareProfileButton",
            "HUD-219-02J ReturnToEnvironmentProfileButton",
            "HUD-219-02K ReturnToIslandMapButton",
            "HUD-219-02L ReturnToBiomeMapButton",
            "HUD-219-02M ReturnToRegionMapButton",
            "HUD-219-02N ReturnToContinentMapButton",
            "HUD-219-02O ReturnToSurfaceMapButton",
            "HUD-219-02P ReturnToPlanetOverviewButton"
        };

        string[] labels =
        {
            "STORM CELL ANALYSIS",
            "ATMOSPHERIC SIMULATION",
            "WEATHER SYSTEM VIEW",
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
            float y = 330f - i * 43f;

            Button(
                panel,
                names[i],
                labels[i],
                new Vector2(0f, y),
                new Vector2(235f, 34f));
        }
    }

    private static void BuildClimateLayers(Transform root)
    {
        Transform panel =
            Panel(root, "HUD-219-03 ClimateLayersPanel",
                new Vector2(-570f, 155f),
                new Vector2(250f, 450f));

        string[] names =
        {
            "HUD-219-03A LayerToggle_Temperature",
            "HUD-219-03B LayerToggle_Humidity",
            "HUD-219-03C LayerToggle_WindSpeed",
            "HUD-219-03D LayerToggle_Precipitation",
            "HUD-219-03E LayerToggle_StormCells",
            "HUD-219-03F LayerToggle_Visibility",
            "HUD-219-03G LayerToggle_AtmosphericPressure",
            "HUD-219-03H LayerToggle_AirDensity",
            "HUD-219-03I LayerToggle_TurbulenceZones",
            "HUD-219-03J LayerToggle_ClimateBands",
            "HUD-219-03K LayerToggle_LongTermWeatherPatterns",
            "HUD-219-03L LayerToggle_FactionControl",
            "HUD-219-03M LayerToggle_WarfareZones",
            "HUD-219-03N LayerToggle_LogisticsRoutes",
            "HUD-219-03O LayerToggle_TerrainModifiers"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Toggle(
                panel,
                names[i],
                LayerLabel(names[i]),
                new Vector2(0f, 195f - i * 26f));
        }
    }

    private static void BuildSimulationControl(Transform root)
    {
        Transform panel =
            Panel(root, "HUD-219-04 SimulationControlPanel",
                new Vector2(-570f, -220f),
                new Vector2(250f, 230f));

        Button(panel, "HUD-219-04A SimulationStartButton",
            "START", new Vector2(-75f, 80f),
            new Vector2(105f, 35f));

        Button(panel, "HUD-219-04B SimulationPauseButton",
            "PAUSE", new Vector2(75f, 80f),
            new Vector2(105f, 35f));

        Button(panel, "HUD-219-04C SimulationResetButton",
            "RESET", new Vector2(0f, 35f),
            new Vector2(210f, 32f));

        Slider slider =
            SliderControl(panel,
                "HUD-219-04D SimulationSpeedSlider",
                new Vector2(0f, -10f));

        slider.minValue = 0.25f;
        slider.maxValue = 4f;
        slider.value = 1f;

        TMP_Dropdown dropdown =
            Dropdown(panel,
                "HUD-219-04E SimulationModeSelector",
                new Vector2(0f, -55f));

        dropdown.AddOptions(new List<string>
        {
            "BALANCED",
            "HIGH FIDELITY",
            "FAST PREDICTION"
        });

        Label(panel, "HUD-219-04F SimulationAccuracyDisplay",
            "MODEL ACCURACY // 98.7%",
            new Vector2(0f, -95f),
            new Vector2(220f, 30f), 12f);
    }

    private static void BuildClimateStats(Transform root)
    {
        Transform panel =
            Panel(root, "HUD-219-05 ClimateStatsPanel",
                new Vector2(250f, 230f),
                new Vector2(390f, 330f));

        string[] names =
        {
            "HUD-219-05A TemperatureDisplay",
            "HUD-219-05B HumidityDisplay",
            "HUD-219-05C WindSpeedDisplay",
            "HUD-219-05D PrecipitationDisplay",
            "HUD-219-05E StormCellIntensityDisplay",
            "HUD-219-05F VisibilityDisplay",
            "HUD-219-05G AtmosphericPressureDisplay",
            "HUD-219-05H AirDensityDisplay",
            "HUD-219-05I TurbulenceLevelDisplay",
            "HUD-219-05J ClimateBandDisplay",
            "HUD-219-05K LongTermWeatherPatternDisplay"
        };

        for (int i = 0; i < names.Length; i++)
        {
            int column = i % 2;
            int row = i / 2;

            Label(
                panel,
                names[i],
                names[i].Replace("HUD-219-05", "")
                         .Replace("Display", "")
                         .Replace("_", " "),
                new Vector2(-92f + column * 185f,
                    125f - row * 45f),
                new Vector2(175f, 38f),
                11f);
        }
    }

    private static void BuildClimateInfo(Transform root)
    {
        Transform panel =
            Panel(root, "HUD-219-06 ClimateInfoPanel",
                new Vector2(680f, 210f),
                new Vector2(390f, 390f));

        string[] names =
        {
            "HUD-219-06A SelectedZoneName",
            "HUD-219-06B SelectedZoneType",
            "HUD-219-06C SelectedTemperature",
            "HUD-219-06D SelectedHumidity",
            "HUD-219-06E SelectedWindSpeed",
            "HUD-219-06F SelectedPrecipitationLevel",
            "HUD-219-06G SelectedStormCellIntensity",
            "HUD-219-06H SelectedVisibility",
            "HUD-219-06I SelectedAtmosphericPressure",
            "HUD-219-06J SelectedAirDensity",
            "HUD-219-06K SelectedTurbulenceLevel",
            "HUD-219-06L SelectedClimateBand",
            "HUD-219-06M SelectedLongTermWeatherPattern"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Label(
                panel,
                names[i],
                names[i].Replace("HUD-219-06", "")
                         .Replace("Selected", ""),
                new Vector2(0f, 160f - i * 25f),
                new Vector2(340f, 22f),
                11f);
        }

        Button(
            panel,
            "HUD-219-06N EnterZoneButton",
            "ENTER SELECTED ZONE",
            new Vector2(0f, -170f),
            new Vector2(300f, 36f));
    }

    private static void BuildImpactAnalysis(Transform root)
    {
        Transform panel =
            Panel(root, "HUD-219-07 ImpactAnalysisPanel",
                new Vector2(250f, -150f),
                new Vector2(390f, 260f));

        string[] names =
        {
            "HUD-219-07A MobilityImpactDisplay",
            "HUD-219-07B ReconVisibilityImpactDisplay",
            "HUD-219-07C SensorInterferenceImpactDisplay",
            "HUD-219-07D LogisticsDifficultyImpactDisplay",
            "HUD-219-07E CombatEffectivenessImpactDisplay",
            "HUD-219-07F EnvironmentalHazardLevelDisplay"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Label(
                panel,
                names[i],
                names[i].Replace("HUD-219-07", "")
                         .Replace("ImpactDisplay", "")
                         .Replace("_", " "),
                new Vector2(0f, 100f - i * 35f),
                new Vector2(340f, 28f),
                11f);
        }
    }

    private static void BuildForecast(Transform root)
    {
        Transform panel =
            Panel(root, "HUD-219-08 ForecastPanel",
                new Vector2(680f, -110f),
                new Vector2(390f, 260f));

        string[] names =
        {
            "HUD-219-08A ShortTermForecastDisplay",
            "HUD-219-08B MidTermForecastDisplay",
            "HUD-219-08C LongTermForecastDisplay",
            "HUD-219-08D ClimateShiftProbabilityDisplay",
            "HUD-219-08E AtmosphericInstabilityDisplay",
            "HUD-219-08F SeasonalTrendDisplay"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Label(
                panel,
                names[i],
                names[i].Replace("HUD-219-08", "")
                         .Replace("Display", ""),
                new Vector2(0f, 100f - i * 35f),
                new Vector2(340f, 28f),
                11f);
        }
    }

    private static void BuildQuickActions(Transform root)
    {
        Transform panel =
            Panel(root, "HUD-219-09 QuickActionsPanel",
                new Vector2(25f, -365f),
                new Vector2(600f, 110f));

        string[] names =
        {
            "HUD-219-09A DeployReconButton",
            "HUD-219-09B DeployWeatherMonitoringTeamButton",
            "HUD-219-09C DeployAtmosphericDroneButton",
            "HUD-219-09D DeployClimateResearchTeamButton",
            "HUD-219-09E OpenIntelNetworkButton",
            "HUD-219-09F OpenDoctrineManagerButton",
            "HUD-219-09G OpenLogisticsOverviewButton",
            "HUD-219-09H OpenFactionDiplomacyButton"
        };

        for (int i = 0; i < names.Length; i++)
        {
            int column = i % 4;
            int row = i / 4;

            Button(
                panel,
                names[i],
                QuickLabel(names[i]),
                new Vector2(-210f + column * 140f,
                    25f - row * 45f),
                new Vector2(125f, 34f));
        }
    }

    private static void BuildAlerts(Transform root)
    {
        Transform panel =
            Panel(root, "HUD-219-10 AlertsPanel",
                new Vector2(-190f, -365f),
                new Vector2(330f, 110f));

        string[] names =
        {
            "HUD-219-10A StrategicAlertEntry",
            "HUD-219-10B WeatherAlertEntry",
            "HUD-219-10C EnvironmentalHazardAlertEntry",
            "HUD-219-10D LogisticsAlertEntry",
            "HUD-219-10E WarfareAlertEntry",
            "HUD-219-10F ClimateShiftAlertEntry",
            "HUD-219-10G AtmosphericInstabilityAlertEntry",
            "HUD-219-10H SeasonalTrendAlertEntry"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Label(
                panel,
                names[i],
                "ALERT // MONITORING ACTIVE",
                new Vector2(0f, 38f - i * 14f),
                new Vector2(300f, 15f),
                8f);
        }
    }

    private static void BuildBottomBar(Transform root)
    {
        Transform panel =
            Panel(root, "HUD-219-11 BottomBar",
                new Vector2(0f, -505f),
                new Vector2(1880f, 55f));

        Button(panel, "HUD-219-11A BackButton",
            "BACK", new Vector2(-720f, 0f),
            new Vector2(180f, 34f));

        Button(panel, "HUD-219-11B ExitToCommandCenterButton",
            "COMMAND CENTER", new Vector2(-300f, 0f),
            new Vector2(220f, 34f));

        Button(panel, "HUD-219-11C SettingsButton",
            "SETTINGS", new Vector2(300f, 0f),
            new Vector2(180f, 34f));

        Button(panel, "HUD-219-11D HelpButton",
            "HELP", new Vector2(720f, 0f),
            new Vector2(180f, 34f));
    }

    private static Transform Panel(
        Transform parent,
        string name,
        Vector2 position,
        Vector2 size)
    {
        GameObject go = new GameObject(name);

        go.transform.SetParent(parent, false);

        RectTransform rect = go.AddComponent<RectTransform>();

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        Image image = go.AddComponent<Image>();

        image.color =
            new Color(0.025f, 0.06f, 0.09f, 0.94f);

        return go.transform;
    }

    private static void Label(
        Transform parent,
        string name,
        string text,
        Vector2 position,
        Vector2 size,
        float fontSize)
    {
        GameObject go = new GameObject(name);

        go.transform.SetParent(parent, false);

        RectTransform rect = go.AddComponent<RectTransform>();

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        TextMeshProUGUI tmp =
            go.AddComponent<TextMeshProUGUI>();

        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        tmp.textWrappingMode = TextWrappingModes.Normal;
    }

    private static void Button(
        Transform parent,
        string name,
        string text,
        Vector2 position,
        Vector2 size)
    {
        GameObject go = new GameObject(name);

        go.transform.SetParent(parent, false);

        RectTransform rect = go.AddComponent<RectTransform>();

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        Image image = go.AddComponent<Image>();

        image.color =
            new Color(0.06f, 0.12f, 0.17f, 1f);

        Button button = go.AddComponent<Button>();

        button.targetGraphic = image;

        ColorBlock colors = button.colors;

        colors.normalColor =
            new Color(0.06f, 0.12f, 0.17f, 1f);

        colors.highlightedColor =
            new Color(0.12f, 0.26f, 0.34f, 1f);

        colors.pressedColor =
            new Color(0.18f, 0.36f, 0.42f, 1f);

        colors.selectedColor =
            new Color(0.12f, 0.26f, 0.34f, 1f);

        button.colors = colors;

        GameObject textObject = new GameObject("Text");

        textObject.transform.SetParent(go.transform, false);

        RectTransform textRect =
            textObject.AddComponent<RectTransform>();

        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        TextMeshProUGUI tmp =
            textObject.AddComponent<TextMeshProUGUI>();

        tmp.text = text;
        tmp.fontSize = 11f;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        tmp.textWrappingMode = TextWrappingModes.Normal;
    }

    private static Toggle Toggle(
        Transform parent,
        string name,
        string text,
        Vector2 position)
    {
        GameObject go = new GameObject(name);

        go.transform.SetParent(parent, false);

        RectTransform rect = go.AddComponent<RectTransform>();

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(225f, 24f);

        Toggle toggle = go.AddComponent<Toggle>();

        GameObject background =
            new GameObject("Background");

        background.transform.SetParent(go.transform, false);

        RectTransform bgRect =
            background.AddComponent<RectTransform>();

        bgRect.anchorMin = new Vector2(0f, 0.5f);
        bgRect.anchorMax = new Vector2(0f, 0.5f);
        bgRect.anchoredPosition =
            new Vector2(10f, 0f);
        bgRect.sizeDelta =
            new Vector2(18f, 18f);

        Image bgImage =
            background.AddComponent<Image>();

        bgImage.color =
            new Color(0.08f, 0.16f, 0.20f, 1f);

        GameObject check =
            new GameObject("Checkmark");

        check.transform.SetParent(background.transform, false);

        RectTransform checkRect =
            check.AddComponent<RectTransform>();

        checkRect.anchorMin = Vector2.zero;
        checkRect.anchorMax = Vector2.one;
        checkRect.offsetMin = Vector2.zero;
        checkRect.offsetMax = Vector2.zero;

        Image checkImage =
            check.AddComponent<Image>();

        checkImage.color =
            new Color(0.18f, 0.65f, 0.75f, 1f);

        toggle.graphic = checkImage;
        toggle.targetGraphic = bgImage;
        toggle.isOn = true;

        Label(
            go.transform,
            name + "_Label",
            text,
            new Vector2(70f, 0f),
            new Vector2(150f, 22f),
            10f);

        return toggle;
    }

    private static Slider SliderControl(
        Transform parent,
        string name,
        Vector2 position)
    {
        GameObject go = new GameObject(name);

        go.transform.SetParent(parent, false);

        RectTransform rect = go.AddComponent<RectTransform>();

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(210f, 25f);

        Slider slider = go.AddComponent<Slider>();

        GameObject background =
            new GameObject("Background");

        background.transform.SetParent(go.transform, false);

        RectTransform bgRect =
            background.AddComponent<RectTransform>();

        bgRect.anchorMin = new Vector2(0f, 0.5f);
        bgRect.anchorMax = new Vector2(1f, 0.5f);
        bgRect.offsetMin = new Vector2(0f, -4f);
        bgRect.offsetMax = new Vector2(0f, 4f);

        Image bgImage =
            background.AddComponent<Image>();

        bgImage.color =
            new Color(0.08f, 0.15f, 0.18f, 1f);

        GameObject fill =
            new GameObject("Fill");

        fill.transform.SetParent(go.transform, false);

        RectTransform fillRect =
            fill.AddComponent<RectTransform>();

        fillRect.anchorMin = new Vector2(0f, 0.5f);
        fillRect.anchorMax = new Vector2(1f, 0.5f);
        fillRect.offsetMin = new Vector2(0f, -4f);
        fillRect.offsetMax = new Vector2(0f, 4f);

        Image fillImage =
            fill.AddComponent<Image>();

        fillImage.color =
            new Color(0.12f, 0.42f, 0.55f, 1f);

        GameObject handle =
            new GameObject("Handle");

        handle.transform.SetParent(go.transform, false);

        RectTransform handleRect =
            handle.AddComponent<RectTransform>();

        handleRect.sizeDelta =
            new Vector2(16f, 25f);

        Image handleImage =
            handle.AddComponent<Image>();

        handleImage.color = Color.white;

        slider.targetGraphic = handleImage;
        slider.handleRect = handleRect;

        return slider;
    }

    private static TMP_Dropdown Dropdown(
        Transform parent,
        string name,
        Vector2 position)
    {
        GameObject go = new GameObject(name);

        go.transform.SetParent(parent, false);

        RectTransform rect =
            go.AddComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0.5f, 0.5f);

        rect.anchorMax =
            new Vector2(0.5f, 0.5f);

        rect.anchoredPosition = position;
        rect.sizeDelta =
            new Vector2(210f, 30f);

        Image image =
            go.AddComponent<Image>();

        image.color =
            new Color(0.06f, 0.12f, 0.17f, 1f);

        TMP_Dropdown dropdown =
            go.AddComponent<TMP_Dropdown>();

        GameObject labelObject =
            new GameObject("Label");

        labelObject.transform.SetParent(go.transform, false);

        RectTransform labelRect =
            labelObject.AddComponent<RectTransform>();

        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.offsetMin =
            new Vector2(10f, 0f);
        labelRect.offsetMax =
            new Vector2(-10f, 0f);

        TextMeshProUGUI label =
            labelObject.AddComponent<TextMeshProUGUI>();

        label.text = "BALANCED";
        label.fontSize = 11f;
        label.alignment =
            TextAlignmentOptions.Center;
        label.color = Color.white;

        dropdown.captionText = label;

        return dropdown;
    }

    private static string LayerLabel(string name)
    {
        int index = name.IndexOf("LayerToggle_");

        if (index >= 0)
            return name.Substring(index + "LayerToggle_".Length)
                .Replace("_", " ");

        return name;
    }

    private static string QuickLabel(string name)
    {
        string value = name;

        int index = value.IndexOf("Deploy");

        if (index >= 0)
            value = value.Substring(index);

        value = value
            .Replace("Button", "")
            .Replace("Open", "OPEN ")
            .Replace("Deploy", "DEPLOY ");

        return value.ToUpperInvariant();
    }

    private static void SaveAndRegister(Scene scene)
    {
        EditorSceneManager.SaveScene(scene, ScenePath);

        string[] currentScenes =
            EditorBuildSettings.scenes != null
                ? System.Array.ConvertAll(
                    EditorBuildSettings.scenes,
                    x => x.path)
                : new string[0];

        bool exists = false;

        foreach (string path in currentScenes)
        {
            if (path == ScenePath)
            {
                exists = true;
                break;
            }
        }

        if (!exists)
        {
            List<EditorBuildSettingsScene> scenes =
                new List<EditorBuildSettingsScene>(
                    EditorBuildSettings.scenes);

            scenes.Add(
                new EditorBuildSettingsScene(
                    ScenePath,
                    true));

            EditorBuildSettings.scenes =
                scenes.ToArray();
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}

