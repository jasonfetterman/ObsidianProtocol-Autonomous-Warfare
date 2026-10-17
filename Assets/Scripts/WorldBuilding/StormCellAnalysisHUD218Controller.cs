using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class StormCellAnalysisHUD218Controller : MonoBehaviour
{
    private const string RootName = "HUD\u2011218 StormCellAnalysisHUD";

    private readonly string[] layerButtons =
    {
        "HUD\u2011218\u201103A LayerToggle_StormCells",
        "HUD\u2011218\u201103B LayerToggle_Precipitation",
        "HUD\u2011218\u201103C LayerToggle_WindSpeed",
        "HUD\u2011218\u201103D LayerToggle_TurbulenceZones",
        "HUD\u2011218\u201103E LayerToggle_Visibility",
        "HUD\u2011218\u201103F LayerToggle_AtmosphericPressure",
        "HUD\u2011218\u201103G LayerToggle_AirDensity",
        "HUD\u2011218\u201103H LayerToggle_WeatherSystems",
        "HUD\u2011218\u201103I LayerToggle_FactionControl",
        "HUD\u2011218\u201103J LayerToggle_WarfareZones",
        "HUD\u2011218\u201103K LayerToggle_LogisticsRoutes",
        "HUD\u2011218\u201103L LayerToggle_TerrainModifiers"
    };

    private readonly string[] layerObjects =
    {
        "STORM_LAYER_STORM_CELLS",
        "STORM_LAYER_PRECIPITATION",
        "STORM_LAYER_WIND_SPEED",
        "STORM_LAYER_TURBULENCE",
        "STORM_LAYER_VISIBILITY",
        "STORM_LAYER_PRESSURE",
        "STORM_LAYER_AIR_DENSITY",
        "STORM_LAYER_WEATHER",
        "STORM_LAYER_FACTION",
        "STORM_LAYER_WARFARE",
        "STORM_LAYER_LOGISTICS",
        "STORM_LAYER_TERRAIN"
    };

    private bool simulationRunning = true;
    private float simulationTime;
    private float simulationSpeed = 1f;
    private float nextUpdate;
    private int selectedStormIndex;

    private readonly string[] stormNames =
    {
        "STORM CELL ALPHA",
        "STORM CELL BRAVO",
        "STORM CELL CHARLIE"
    };

    private readonly string[] stormCategories =
    {
        "SUPERCELL",
        "SEVERE CONVECTIVE",
        "TROPICAL CYCLONE"
    };

    private void Start()
    {
        BindNavigation("HUD\u2011218\u201102A ReturnToAtmosphericSimulationButton",
            "Atmospheric_Simulation", "Atmospheric Simulation");

        BindNavigation("HUD\u2011218\u201102B ReturnToWeatherSystemViewButton",
            "Weather_System", "WeatherSystem", "Weather");

        BindNavigation("HUD\u2011218\u201102C ReturnToLogisticsNetworkMapButton",
            "Logistics_Network");

        BindNavigation("HUD\u2011218\u201102D ReturnToResourceDistributionMapButton",
            "Resource_Distribution");

        BindNavigation("HUD\u2011218\u201102E ReturnToStrategicHeatmapButton",
            "Strategic_Heatmap");

        BindNavigation("HUD\u2011218\u201102F ReturnToFactionTerritoryOverlayButton",
            "Faction_Territory");

        BindNavigation("HUD\u2011218\u201102G ReturnToFactionControlMapButton",
            "Faction_Control");

        BindNavigation("HUD\u2011218\u201102H ReturnToWarfareProfileButton",
            "Warfare_Profile");

        BindNavigation("HUD\u2011218\u201102I ReturnToEnvironmentProfileButton",
            "Environment_Profile");

        BindNavigation("HUD\u2011218\u201102J ReturnToIslandMapButton",
            "Island_Map");

        BindNavigation("HUD\u2011218\u201102K ReturnToBiomeMapButton",
            "Biome_Map");

        BindNavigation("HUD\u2011218\u201102L ReturnToRegionMapButton",
            "Region_Map");

        BindNavigation("HUD\u2011218\u201102M ReturnToContinentMapButton",
            "Continent_Map");

        BindNavigation("HUD\u2011218\u201102N ReturnToSurfaceMapButton",
            "Planet_Surface", "Surface_Map");

        BindNavigation("HUD\u2011218\u201102O ReturnToPlanetOverviewButton",
            "Planet_Overview");

        for (int i = 0; i < layerButtons.Length; i++)
        {
            int index = i;
            Button button = FindButton(layerButtons[index]);

            if (button != null)
            {
                button.onClick.AddListener(() => ToggleLayer(index));
            }
        }

        BindButton("HUD\u2011218\u201105J EnterStormCellButton", EnterStormCell);

        BindButton("HUD\u2011218\u201109A DeployReconButton",
            () => ActionMessage("RECON TEAM DEPLOYED TO STORM PERIMETER"));

        BindButton("HUD\u2011218\u201109B DeployWeatherMonitoringTeamButton",
            () => ActionMessage("WEATHER MONITORING TEAM DEPLOYED"));

        BindButton("HUD\u2011218\u201109C DeployAtmosphericDroneButton",
            () => ActionMessage("ATMOSPHERIC DRONE DEPLOYED"));

        BindButton("HUD\u2011218\u201109D OpenIntelNetworkButton",
            () => ActionMessage("INTELLIGENCE NETWORK LINK ESTABLISHED"));

        BindButton("HUD\u2011218\u201109E OpenDoctrineManagerButton",
            () => ActionMessage("DOCTRINE MANAGER REQUESTED"));

        BindButton("HUD\u2011218\u201109F OpenLogisticsOverviewButton",
            () => ActionMessage("LOGISTICS OVERVIEW REQUESTED"));

        BindButton("HUD\u2011218\u201109G OpenFactionDiplomacyButton",
            () => ActionMessage("FACTION DIPLOMACY CHANNEL REQUESTED"));

        BindButton("HUD\u2011218\u201111A BackButton",
            () => LoadMatchingScene("Atmospheric_Simulation", "Logistics_Network"));

        BindButton("HUD\u2011218\u201111B ExitToCommandCenterButton",
            () => LoadMatchingScene("Command_Center", "Command Center"));

        BindButton("HUD\u2011218\u201111C SettingsButton",
            () => LoadMatchingScene("Settings"));

        BindButton("HUD\u2011218\u201111D HelpButton",
            () => ActionMessage("STORM CELL ANALYSIS: SELECT A CELL, REVIEW TRAJECTORY, THEN DEPLOY RECON OR MONITORING AS REQUIRED"));

        Button selectCell = FindButton("HUD\u2011218\u201101C StormCellIDDisplay");
        if (selectCell != null)
            selectCell.onClick.AddListener(CycleStormCell);

        Slider speed = FindSlider("HUD\u2011218\u201106D SimulationSpeedSlider");
        if (speed != null)
        {
            speed.minValue = 0.25f;
            speed.maxValue = 4f;
            speed.value = 1f;
            speed.onValueChanged.AddListener(value =>
            {
                simulationSpeed = value;
                UpdateDisplay();
            });
        }

        UpdateDisplay();
    }

    private void Update()
    {
        if (!simulationRunning)
            return;

        simulationTime += Time.deltaTime * simulationSpeed;

        if (Time.time >= nextUpdate)
        {
            nextUpdate = Time.time + 0.15f;
            UpdateDisplay();
            AnimateStorm();
        }
    }

    private void ToggleLayer(int index)
    {
        if (index < 0 || index >= layerObjects.Length)
            return;

        GameObject layer = GameObject.Find(layerObjects[index]);

        if (layer == null)
            return;

        layer.SetActive(!layer.activeSelf);

        Button button = FindButton(layerButtons[index]);

        if (button != null)
        {
            TextMeshProUGUI text = button.GetComponent<TextMeshProUGUI>();

            if (text != null)
                text.text = LayerLabel(index) + (layer.activeSelf ? "  [ON]" : "  [OFF]");
        }
    }

    private string LayerLabel(int index)
    {
        switch (index)
        {
            case 0: return "STORM CELLS";
            case 1: return "PRECIPITATION";
            case 2: return "WIND SPEED";
            case 3: return "TURBULENCE";
            case 4: return "VISIBILITY";
            case 5: return "PRESSURE";
            case 6: return "AIR DENSITY";
            case 7: return "WEATHER SYSTEMS";
            case 8: return "FACTION CONTROL";
            case 9: return "WARFARE ZONES";
            case 10: return "LOGISTICS ROUTES";
            case 11: return "TERRAIN MODIFIERS";
        }

        return "LAYER";
    }

    private void CycleStormCell()
    {
        selectedStormIndex++;

        if (selectedStormIndex >= stormNames.Length)
            selectedStormIndex = 0;

        UpdateDisplay();
        ActionMessage("SELECTED " + stormNames[selectedStormIndex]);
    }

    private void EnterStormCell()
    {
        ActionMessage(
            "ENTERING " +
            stormNames[selectedStormIndex] +
            " // ANALYSIS MODE ACTIVE"
        );
    }

    private void ActionMessage(string message)
    {
        SetText("HUD\u2011218\u201110A StrategicAlertEntry", "STRATEGIC // " + message);
        SetText("HUD\u2011218\u201110B WeatherAlertEntry", "WEATHER // STORM CELL ANALYSIS UPDATED");
        SetText("HUD\u2011218\u201110F StormTrajectoryAlertEntry", "TRAJECTORY // PREDICTION MODEL ACTIVE");
    }

    private void UpdateDisplay()
    {
        float phase = simulationTime * 0.035f;

        float intensity =
            68f +
            Mathf.Sin(phase) * 9f +
            selectedStormIndex * 4f;

        float wind =
            92f +
            Mathf.Sin(phase * 1.7f) * 14f;

        float precipitation =
            74f +
            Mathf.Cos(phase * 1.2f) * 12f;

        float pressure =
            952f +
            Mathf.Sin(phase * 0.8f) * 8f;

        float density =
            1.08f +
            Mathf.Cos(phase) * 0.04f;

        float turbulence =
            71f +
            Mathf.Sin(phase * 1.5f) * 13f;

        float visibility =
            Mathf.Clamp(
                34f +
                Mathf.Cos(phase * 1.1f) * 9f,
                10f,
                80f
            );

        SetText("HUD\u2011218\u201101A PlanetNameDisplay", "PLANET // AURELIA");
        SetText("HUD\u2011218\u201101B RegionNameDisplay", "REGION // NORTHERN CONTINENT");
        SetText("HUD\u2011218\u201101C StormCellIDDisplay",
            "CELL // SC-" + (218 + selectedStormIndex).ToString("000"));

        SetText("HUD\u2011218\u201101D StormCellCategoryDisplay",
            "CATEGORY // " + stormCategories[selectedStormIndex]);

        SetText("HUD\u2011218\u201101E OwnerFactionEmblem", "A");
        SetText("HUD\u2011218\u201101F PlayerResourcesPanel",
            "RESOURCES // 84,250 C // 6,420 MAT");
        SetText("HUD\u2011218\u201101G TimeControlPanel",
            simulationRunning
                ? "SIMULATION // RUNNING x" + simulationSpeed.ToString("0.00")
                : "SIMULATION // PAUSED");

        SetText("HUD\u2011218\u201105A StormCellNameDisplay",
            "NAME // " + stormNames[selectedStormIndex]);

        SetText("HUD\u2011218\u201105B StormCellCategoryDisplay",
            "CATEGORY // " + stormCategories[selectedStormIndex]);

        SetText("HUD\u2011218\u201105C StormCellIntensityDisplay",
            "INTENSITY // " + intensity.ToString("0") + "%");

        SetText("HUD\u2011218\u201105D StormCellWindSpeedDisplay",
            "WIND // " + wind.ToString("0") + " KM/H");

        SetText("HUD\u2011218\u201105E StormCellPrecipitationDisplay",
            "PRECIPITATION // " + precipitation.ToString("0") + "%");

        SetText("HUD\u2011218\u201105F StormCellPressureDisplay",
            "PRESSURE // " + pressure.ToString("0") + " HPA");

        SetText("HUD\u2011218\u201105G StormCellAirDensityDisplay",
            "AIR DENSITY // " + density.ToString("0.00") + " KG/M³");

        SetText("HUD\u2011218\u201105H StormCellTurbulenceDisplay",
            "TURBULENCE // " + turbulence.ToString("0") + "%");

        SetText("HUD\u2011218\u201105I StormCellVisibilityDisplay",
            "VISIBILITY // " + visibility.ToString("0") + "%");

        SetText("HUD\u2011218\u201106A CurrentTrajectoryDisplay",
            "CURRENT // 032° NE");

        SetText("HUD\u2011218\u201106B PredictedTrajectoryDisplay",
            "PREDICTED // 047° NE");

        SetText("HUD\u2011218\u201106C TrajectoryConfidenceDisplay",
            "CONFIDENCE // " + Mathf.Clamp(91f + Mathf.Sin(phase) * 3f, 80f, 98f).ToString("0") + "%");

        SetText("HUD\u2011218\u201106D StormSpeedDisplay",
            "SPEED // " + (wind * 0.21f).ToString("0.0") + " KM/H");

        SetText("HUD\u2011218\u201106E StormDirectionDisplay",
            "DIRECTION // NORTHEAST");

        SetText("HUD\u2011218\u201106F LandfallPredictionDisplay",
            "LANDFALL // 18H 42M");

        SetText("HUD\u2011218\u201107A MobilityImpactDisplay",
            "MOBILITY IMPACT // -42%");

        SetText("HUD\u2011218\u201107B ReconVisibilityImpactDisplay",
            "RECON VISIBILITY // -58%");

        SetText("HUD\u2011218\u201107C SensorInterferenceImpactDisplay",
            "SENSOR INTERFERENCE // +31%");

        SetText("HUD\u2011218\u201107D LogisticsDifficultyImpactDisplay",
            "LOGISTICS DIFFICULTY // +47%");

        SetText("HUD\u2011218\u201107E CombatEffectivenessImpactDisplay",
            "COMBAT EFFECTIVENESS // -19%");

        SetText("HUD\u2011218\u201107F EnvironmentalHazardLevelDisplay",
            "ENVIRONMENTAL HAZARD // HIGH");

        SetText("HUD\u2011218\u201108A ShortTermForecastDisplay",
            "0-6H // INTENSIFICATION EXPECTED");

        SetText("HUD\u2011218\u201108B MidTermForecastDisplay",
            "6-18H // EASTWARD PROPAGATION");

        SetText("HUD\u2011218\u201108C LongTermForecastDisplay",
            "18-48H // GRADUAL DECAY");

        SetText("HUD\u2011218\u201108D StormGrowthProbabilityDisplay",
            "GROWTH PROBABILITY // 64%");

        SetText("HUD\u2011218\u201108E StormDecayProbabilityDisplay",
            "DECAY PROBABILITY // 36%");

        SetText("HUD\u2011218\u201108F AtmosphericInstabilityDisplay",
            "INSTABILITY // SEVERE");

        SetText("HUD\u2011218\u201110C EnvironmentalHazardAlertEntry",
            "HAZARD // SEVERE CONVECTIVE CONDITIONS");

        SetText("HUD\u2011218\u201110D LogisticsAlertEntry",
            "LOGISTICS // ROUTE DISRUPTION RISK HIGH");

        SetText("HUD\u2011218\u201110E WarfareAlertEntry",
            "WARFARE // AIR OPERATIONS DEGRADED");

        SetText("HUD\u2011218\u201110G VisibilityDropAlertEntry",
            "VISIBILITY // RAPID DROP POSSIBLE");

        SetText("HUD\u2011218\u201110H AtmosphericInstabilityAlertEntry",
            "ATMOSPHERE // INSTABILITY ELEVATED");
    }

    private void AnimateStorm()
    {
        GameObject stormLayer = GameObject.Find("STORM_LAYER_STORM_CELLS");

        if (stormLayer == null)
            return;

        Transform[] cells = stormLayer.GetComponentsInChildren<Transform>();

        float pulse = 1f + Mathf.Sin(simulationTime * 1.5f) * 0.035f;

        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i] == stormLayer.transform)
                continue;

            float scale = pulse + i * 0.002f;
            cells[i].localScale = Vector3.one * scale;
        }
    }

    private void BindButton(string objectName, Action action)
    {
        Button button = FindButton(objectName);

        if (button != null)
            button.onClick.AddListener(() => action());
    }

    private void BindNavigation(string objectName, params string[] keys)
    {
        BindButton(objectName, () => LoadMatchingScene(keys));
    }

    private void LoadMatchingScene(params string[] keys)
    {
        string currentPath = SceneManager.GetActiveScene().path;

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);

            if (string.IsNullOrEmpty(path) ||
                string.Equals(path, currentPath, StringComparison.OrdinalIgnoreCase))
                continue;

            string normalized = path.Replace("\\", "/").ToLowerInvariant();

            for (int k = 0; k < keys.Length; k++)
            {
                if (normalized.Contains(keys[k].ToLowerInvariant()))
                {
                    SceneManager.LoadScene(path);
                    return;
                }
            }
        }

        ActionMessage("TARGET SCENE NOT REGISTERED // NO SCENE WAS CHANGED");
    }

    private Button FindButton(string objectName)
    {
        Transform t = FindTransform(objectName);

        return t == null ? null : t.GetComponent<Button>();
    }

    private Slider FindSlider(string objectName)
    {
        Transform t = FindTransform(objectName);

        return t == null ? null : t.GetComponent<Slider>();
    }

    private void SetText(string objectName, string value)
    {
        Transform t = FindTransform(objectName);

        if (t == null)
            return;

        TextMeshProUGUI text = t.GetComponent<TextMeshProUGUI>();

        if (text != null)
            text.text = value;
    }

    private Transform FindTransform(string objectName)
    {
        Transform[] all = GetComponentsInChildren<Transform>(true);

        for (int i = 0; i < all.Length; i++)
        {
            if (all[i].name == objectName)
                return all[i];
        }

        return null;
    }
}
