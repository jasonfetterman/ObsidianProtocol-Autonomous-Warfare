using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClimateModelViewHUD219Controller : MonoBehaviour
{
    private bool simulationRunning = true;
    private float simulationSpeed = 1f;
    private float simulationTime;

    private readonly Dictionary<string, GameObject> layers =
        new Dictionary<string, GameObject>();

    private readonly Dictionary<string, TMP_Text> labels =
        new Dictionary<string, TMP_Text>();

    private TMP_Text modeDisplay;
    private TMP_Text accuracyDisplay;
    private TMP_Text alertDisplay;

    private void Awake()
    {
        RegisterSceneObjects();
        RegisterButtons();
        RegisterToggles();
        RegisterSimulationControls();
        UpdateAllDisplays();
    }

    private void Update()
    {
        if (simulationRunning)
        {
            simulationTime += Time.deltaTime * simulationSpeed;
            UpdateSimulationValues();
        }
    }

    private void RegisterSceneObjects()
    {
        foreach (Transform t in GetComponentsInChildren<Transform>(true))
        {
            TMP_Text tmp = t.GetComponent<TMP_Text>();
            if (tmp != null)
                labels[t.name] = tmp;

            if (t.name.StartsWith("CLIMATE_LAYER_"))
                layers[t.name] = t.gameObject;
        }

        modeDisplay = FindText("HUD-219-01D SimulationModeDisplay");
        accuracyDisplay = FindText("HUD-219-04F SimulationAccuracyDisplay");
        alertDisplay = FindText("HUD-219-10A StrategicAlertEntry");
    }

    private void RegisterButtons()
    {
        foreach (Transform t in GetComponentsInChildren<Transform>(true))
        {
            Button button = t.GetComponent<Button>();
            if (button == null)
                continue;

            string n = t.name;

            if (n == "HUD-219-02A ReturnToStormCellAnalysisButton")
                button.onClick.AddListener(() => LoadMatchingScene("STORM_CELL_ANALYSIS"));

            else if (n == "HUD-219-02B ReturnToAtmosphericSimulationButton")
                button.onClick.AddListener(() => LoadMatchingScene("ATMOSPHERIC_SIMULATION"));

            else if (n == "HUD-219-02C ReturnToWeatherSystemViewButton")
                button.onClick.AddListener(() => LoadMatchingScene("WEATHER_SYSTEM"));

            else if (n == "HUD-219-02D ReturnToLogisticsNetworkMapButton")
                button.onClick.AddListener(() => LoadMatchingScene("LOGISTICS_NETWORK"));

            else if (n == "HUD-219-02E ReturnToResourceDistributionMapButton")
                button.onClick.AddListener(() => LoadMatchingScene("RESOURCE_DISTRIBUTION"));

            else if (n == "HUD-219-02F ReturnToStrategicHeatmapButton")
                button.onClick.AddListener(() => LoadMatchingScene("STRATEGIC_HEATMAP"));

            else if (n == "HUD-219-02G ReturnToFactionTerritoryOverlayButton")
                button.onClick.AddListener(() => LoadMatchingScene("FACTION_TERRITORY"));

            else if (n == "HUD-219-02H ReturnToFactionControlMapButton")
                button.onClick.AddListener(() => LoadMatchingScene("FACTION_CONTROL"));

            else if (n == "HUD-219-02I ReturnToWarfareProfileButton")
                button.onClick.AddListener(() => LoadMatchingScene("WARFARE_PROFILE"));

            else if (n == "HUD-219-02J ReturnToEnvironmentProfileButton")
                button.onClick.AddListener(() => LoadMatchingScene("ENVIRONMENT_PROFILE"));

            else if (n == "HUD-219-02K ReturnToIslandMapButton")
                button.onClick.AddListener(() => LoadMatchingScene("ISLAND_MAP"));

            else if (n == "HUD-219-02L ReturnToBiomeMapButton")
                button.onClick.AddListener(() => LoadMatchingScene("BIOME_MAP"));

            else if (n == "HUD-219-02M ReturnToRegionMapButton")
                button.onClick.AddListener(() => LoadMatchingScene("REGION_MAP"));

            else if (n == "HUD-219-02N ReturnToContinentMapButton")
                button.onClick.AddListener(() => LoadMatchingScene("CONTINENT_MAP"));

            else if (n == "HUD-219-02O ReturnToSurfaceMapButton")
                button.onClick.AddListener(() => LoadMatchingScene("SURFACE_MAP"));

            else if (n == "HUD-219-02P ReturnToPlanetOverviewButton")
                button.onClick.AddListener(() => LoadMatchingScene("PLANET_OVERVIEW"));

            else if (n == "HUD-219-06N EnterZoneButton")
                button.onClick.AddListener(EnterSelectedZone);

            else if (n == "HUD-219-04A SimulationStartButton")
                button.onClick.AddListener(StartSimulation);

            else if (n == "HUD-219-04B SimulationPauseButton")
                button.onClick.AddListener(PauseSimulation);

            else if (n == "HUD-219-04C SimulationResetButton")
                button.onClick.AddListener(ResetSimulation);

            else if (n == "HUD-219-09A DeployReconButton")
                button.onClick.AddListener(() => Alert("RECON DEPLOYMENT QUEUED"));

            else if (n == "HUD-219-09B DeployWeatherMonitoringTeamButton")
                button.onClick.AddListener(() => Alert("WEATHER MONITORING TEAM QUEUED"));

            else if (n == "HUD-219-09C DeployAtmosphericDroneButton")
                button.onClick.AddListener(() => Alert("ATMOSPHERIC DRONE DEPLOYMENT QUEUED"));

            else if (n == "HUD-219-09D DeployClimateResearchTeamButton")
                button.onClick.AddListener(() => Alert("CLIMATE RESEARCH TEAM QUEUED"));

            else if (n == "HUD-219-09E OpenIntelNetworkButton")
                button.onClick.AddListener(() => LoadMatchingScene("INTELLIGENCE"));

            else if (n == "HUD-219-09F OpenDoctrineManagerButton")
                button.onClick.AddListener(() => Alert("DOCTRINE MANAGER REQUESTED"));

            else if (n == "HUD-219-09G OpenLogisticsOverviewButton")
                button.onClick.AddListener(() => LoadMatchingScene("LOGISTICS_NETWORK"));

            else if (n == "HUD-219-09H OpenFactionDiplomacyButton")
                button.onClick.AddListener(() => Alert("FACTION DIPLOMACY REQUESTED"));

            else if (n == "HUD-219-11A BackButton")
                button.onClick.AddListener(() => LoadMatchingScene("ATMOSPHERIC_SIMULATION"));

            else if (n == "HUD-219-11B ExitToCommandCenterButton")
                button.onClick.AddListener(() => LoadMatchingScene("COMMAND_CENTER"));

            else if (n == "HUD-219-11C SettingsButton")
                button.onClick.AddListener(() => LoadMatchingScene("SETTINGS"));

            else if (n == "HUD-219-11D HelpButton")
                button.onClick.AddListener(() => Alert("CLIMATE MODEL HELP REQUESTED"));
        }
    }

    private void RegisterToggles()
    {
        foreach (Transform t in GetComponentsInChildren<Transform>(true))
        {
            Toggle toggle = t.GetComponent<Toggle>();
            if (toggle == null)
                continue;

            string layerName = "CLIMATE_LAYER_" +
                               t.name.Replace("HUD-219-03", "")
                                    .Replace(" ", "_");

            if (!layers.ContainsKey(layerName))
            {
                Transform layer = transform.Find(layerName);
                if (layer != null)
                    layers[layerName] = layer.gameObject;
            }

            toggle.onValueChanged.AddListener(value =>
            {
                if (layers.TryGetValue(layerName, out GameObject layer))
                    layer.SetActive(value);

                Alert(t.name + (value ? " ENABLED" : " DISABLED"));
            });
        }
    }

    private void RegisterSimulationControls()
    {
        Transform sliderTransform =
            transform.Find("HUD-219-04 SimulationControlPanel/HUD-219-04D SimulationSpeedSlider");

        if (sliderTransform != null)
        {
            Slider slider = sliderTransform.GetComponent<Slider>();
            if (slider != null)
            {
                slider.minValue = 0.25f;
                slider.maxValue = 4f;
                slider.value = 1f;

                slider.onValueChanged.AddListener(value =>
                {
                    simulationSpeed = value;
                    UpdateSimulationMode();
                });
            }
        }

        Transform selectorTransform =
            transform.Find("HUD-219-04 SimulationControlPanel/HUD-219-04E SimulationModeSelector");

        if (selectorTransform != null)
        {
            TMP_Dropdown dropdown =
                selectorTransform.GetComponent<TMP_Dropdown>();

            if (dropdown != null)
            {
                dropdown.ClearOptions();
                dropdown.AddOptions(new List<string>
                {
                    "BALANCED",
                    "HIGH FIDELITY",
                    "FAST PREDICTION"
                });

                dropdown.onValueChanged.AddListener(value =>
                {
                    if (modeDisplay != null)
                    {
                        modeDisplay.text =
                            "SIMULATION MODE // " +
                            dropdown.options[value].text;
                    }

                    Alert("SIMULATION MODE CHANGED");
                });
            }
        }
    }

    private void UpdateSimulationValues()
    {
        float phase = simulationTime * 0.025f;

        float temperature = 18f + Mathf.Sin(phase) * 8f;
        float humidity = 58f + Mathf.Sin(phase * 0.73f) * 17f;
        float wind = 22f + Mathf.Sin(phase * 1.21f) * 12f;
        float precipitation = 42f + Mathf.Sin(phase * 0.91f) * 28f;
        float storm = 54f + Mathf.Sin(phase * 1.37f) * 24f;
        float visibility = 76f - storm * 0.28f;
        float pressure = 1013f - storm * 1.7f;
        float density = 1.225f - temperature * 0.0018f;
        float turbulence = Mathf.Clamp(storm * 0.8f, 0f, 100f);

        SetText("HUD-219-05A TemperatureDisplay",
            $"TEMPERATURE\n{temperature:0.0} °C");

        SetText("HUD-219-05B HumidityDisplay",
            $"HUMIDITY\n{humidity:0}%");

        SetText("HUD-219-05C WindSpeedDisplay",
            $"WIND SPEED\n{wind:0.0} m/s");

        SetText("HUD-219-05D PrecipitationDisplay",
            $"PRECIPITATION\n{precipitation:0}%");

        SetText("HUD-219-05E StormCellIntensityDisplay",
            $"STORM CELL INTENSITY\n{storm:0}%");

        SetText("HUD-219-05F VisibilityDisplay",
            $"VISIBILITY\n{Mathf.Clamp(visibility, 0f, 100f):0}%");

        SetText("HUD-219-05G AtmosphericPressureDisplay",
            $"PRESSURE\n{pressure:0} hPa");

        SetText("HUD-219-05H AirDensityDisplay",
            $"AIR DENSITY\n{density:0.000} kg/m³");

        SetText("HUD-219-05I TurbulenceLevelDisplay",
            $"TURBULENCE\n{turbulence:0}%");

        string climateBand =
            temperature < 5f ? "COLD" :
            temperature < 15f ? "TEMPERATE" :
            temperature < 25f ? "WARM" : "HOT";

        string weatherPattern =
            precipitation > 65f ? "WET / STORMING" :
            precipitation > 40f ? "UNSTABLE" :
            "DRY / STABLE";

        SetText("HUD-219-05J ClimateBandDisplay",
            $"CLIMATE BAND\n{climateBand}");

        SetText("HUD-219-05K LongTermWeatherPatternDisplay",
            $"LONG-TERM PATTERN\n{weatherPattern}");

        SetText("HUD-219-06C SelectedTemperature",
            $"{temperature:0.0} °C");

        SetText("HUD-219-06D SelectedHumidity",
            $"{humidity:0}%");

        SetText("HUD-219-06E SelectedWindSpeed",
            $"{wind:0.0} m/s");

        SetText("HUD-219-06F SelectedPrecipitationLevel",
            $"{precipitation:0}%");

        SetText("HUD-219-06G SelectedStormCellIntensity",
            $"{storm:0}%");

        SetText("HUD-219-06H SelectedVisibility",
            $"{Mathf.Clamp(visibility, 0f, 100f):0}%");

        SetText("HUD-219-06I SelectedAtmosphericPressure",
            $"{pressure:0} hPa");

        SetText("HUD-219-06J SelectedAirDensity",
            $"{density:0.000} kg/m³");

        SetText("HUD-219-06K SelectedTurbulenceLevel",
            $"{turbulence:0}%");

        SetText("HUD-219-07A MobilityImpactDisplay",
            $"MOBILITY IMPACT\n{Mathf.Clamp(storm * 0.65f, 0f, 100f):0}%");

        SetText("HUD-219-07B ReconVisibilityImpactDisplay",
            $"RECON VISIBILITY\n-{Mathf.Clamp(100f - visibility, 0f, 100f):0}%");

        SetText("HUD-219-07C SensorInterferenceImpactDisplay",
            $"SENSOR INTERFERENCE\n{Mathf.Clamp(storm * 0.72f, 0f, 100f):0}%");

        SetText("HUD-219-07D LogisticsDifficultyImpactDisplay",
            $"LOGISTICS DIFFICULTY\n{Mathf.Clamp(storm * 0.58f, 0f, 100f):0}%");

        SetText("HUD-219-07E CombatEffectivenessImpactDisplay",
            $"COMBAT EFFECTIVENESS\n-{Mathf.Clamp(storm * 0.44f, 0f, 100f):0}%");

        SetText("HUD-219-07F EnvironmentalHazardLevelDisplay",
            $"ENVIRONMENTAL HAZARD\n{Mathf.Clamp(storm * 0.9f, 0f, 100f):0}%");

        SetText("HUD-219-08A ShortTermForecastDisplay",
            $"SHORT TERM\n{temperature + 1.5f:0.0} °C / {precipitation + 4f:0}% rain");

        SetText("HUD-219-08B MidTermForecastDisplay",
            $"MID TERM\n{temperature - 2f:0.0} °C / {humidity + 3f:0}% humidity");

        SetText("HUD-219-08C LongTermForecastDisplay",
            $"LONG TERM\n{climateBand} / {weatherPattern}");

        float shiftProbability =
            Mathf.Clamp(35f + Mathf.Abs(Mathf.Sin(phase * 0.4f)) * 45f, 0f, 100f);

        SetText("HUD-219-08D ClimateShiftProbabilityDisplay",
            $"CLIMATE SHIFT PROBABILITY\n{shiftProbability:0}%");

        SetText("HUD-219-08E AtmosphericInstabilityDisplay",
            $"ATMOSPHERIC INSTABILITY\n{Mathf.Clamp(turbulence, 0f, 100f):0}%");

        SetText("HUD-219-08F SeasonalTrendDisplay",
            $"SEASONAL TREND\n{(Mathf.Sin(phase * 0.18f) >= 0f ? "WARMING" : "COOLING")}");

        if (accuracyDisplay != null)
            accuracyDisplay.text =
                "MODEL ACCURACY // " +
                (simulationSpeed <= 1f ? "98.7%" : "94.2%");
    }

    private void UpdateAllDisplays()
    {
        SetText("HUD-219-01A PlanetNameDisplay", "AURELIA");
        SetText("HUD-219-01B RegionNameDisplay", "NORTHERN CONTINENT");
        SetText("HUD-219-01C ClimateModelDisplay", "CLIMATE MODEL // CM-07");
        SetText("HUD-219-01D SimulationModeDisplay",
            "SIMULATION MODE // BALANCED");
        SetText("HUD-219-01E OwnerFactionEmblem", "A");
        SetText("HUD-219-01F PlayerResourcesPanel",
            "CREDITS  84,250\nENERGY  72%\nDATA  18,440");

        SetText("HUD-219-06A SelectedZoneName", "NORTHERN CLIMATE ZONE");
        SetText("HUD-219-06B SelectedZoneType", "TEMPERATE / MARITIME");
        SetText("HUD-219-06L SelectedClimateBand", "TEMPERATE");
        SetText("HUD-219-06M SelectedLongTermWeatherPattern",
            "UNSTABLE / CYCLICAL");

        SetText("HUD-219-10A StrategicAlertEntry",
            "STRATEGIC // CLIMATE MODEL ACTIVE");
        SetText("HUD-219-10B WeatherAlertEntry",
            "WEATHER // PRECIPITATION PATTERN MONITORED");
        SetText("HUD-219-10C EnvironmentalHazardAlertEntry",
            "ENVIRONMENT // HAZARD LEVEL WITHIN MODEL RANGE");
        SetText("HUD-219-10D LogisticsAlertEntry",
            "LOGISTICS // ROUTE CONDITIONS MONITORED");
        SetText("HUD-219-10E WarfareAlertEntry",
            "WARFARE // CLIMATE IMPACT MODEL AVAILABLE");
        SetText("HUD-219-10F ClimateShiftAlertEntry",
            "CLIMATE SHIFT // PROBABILITY TRACKING ACTIVE");
        SetText("HUD-219-10G AtmosphericInstabilityAlertEntry",
            "ATMOSPHERIC // INSTABILITY MONITORED");
        SetText("HUD-219-10H SeasonalTrendAlertEntry",
            "SEASONAL // LONG-TERM TREND TRACKING ACTIVE");

        UpdateSimulationMode();
    }

    private void StartSimulation()
    {
        simulationRunning = true;
        Alert("CLIMATE SIMULATION STARTED");
    }

    private void PauseSimulation()
    {
        simulationRunning = false;
        Alert("CLIMATE SIMULATION PAUSED");
    }

    private void ResetSimulation()
    {
        simulationTime = 0f;
        simulationRunning = true;
        Alert("CLIMATE SIMULATION RESET");
        UpdateSimulationValues();
    }

    private void EnterSelectedZone()
    {
        Alert("ENTERING SELECTED CLIMATE ZONE");
        SetText("HUD-219-06A SelectedZoneName",
            "ZONE INTERFACE // NORTHERN CLIMATE ZONE");
    }

    private void UpdateSimulationMode()
    {
        if (modeDisplay != null)
        {
            modeDisplay.text =
                "SIMULATION MODE // " +
                (simulationSpeed >= 3f ? "FAST PREDICTION" :
                 simulationSpeed <= 0.5f ? "HIGH FIDELITY" :
                 "BALANCED");
        }
    }

    private void Alert(string message)
    {
        if (alertDisplay != null)
            alertDisplay.text = "ALERT // " + message;

        Debug.Log("[HUD-219] " + message);
    }

    private TMP_Text FindText(string objectName)
    {
        Transform t = FindDeep(transform, objectName);
        return t != null ? t.GetComponent<TMP_Text>() : null;
    }

    private void SetText(string objectName, string value)
    {
        if (labels.TryGetValue(objectName, out TMP_Text tmp))
        {
            if (tmp != null)
                tmp.text = value;

            return;
        }

        TMP_Text found = FindText(objectName);

        if (found != null)
        {
            labels[objectName] = found;
            found.text = value;
        }
    }

    private Transform FindDeep(Transform root, string objectName)
    {
        foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == objectName)
                return child;
        }

        return null;
    }

    private void LoadMatchingScene(string keyword)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);

            if (path.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                SceneManager.LoadScene(i);
                return;
            }
        }

        Alert("TARGET SCENE NOT REGISTERED // " + keyword);
    }
}
