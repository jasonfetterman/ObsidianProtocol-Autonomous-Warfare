using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlanetaryBiosphereMapHUD220Controller : MonoBehaviour
{
    private readonly Dictionary<string, GameObject> objects =
        new Dictionary<string, GameObject>();

    private readonly Dictionary<string, bool> layers =
        new Dictionary<string, bool>();

    private TextMeshProUGUI planetName;
    private TextMeshProUGUI hemisphere;
    private TextMeshProUGUI biosphereLayer;
    private TextMeshProUGUI faction;
    private TextMeshProUGUI timeDisplay;

    private TextMeshProUGUI selectedZoneName;
    private TextMeshProUGUI selectedBiome;
    private TextMeshProUGUI selectedVegetation;
    private TextMeshProUGUI selectedFauna;
    private TextMeshProUGUI selectedWater;
    private TextMeshProUGUI selectedSoil;
    private TextMeshProUGUI selectedClimate;
    private TextMeshProUGUI selectedHazard;

    private TextMeshProUGUI biodiversity;
    private TextMeshProUGUI biomass;
    private TextMeshProUGUI predatorPrey;
    private TextMeshProUGUI stability;
    private TextMeshProUGUI stress;
    private TextMeshProUGUI renewal;

    private TextMeshProUGUI warfareImpact;
    private TextMeshProUGUI logisticsImpact;
    private TextMeshProUGUI climateImpact;
    private TextMeshProUGUI hazardPropagation;
    private TextMeshProUGUI factionActivity;

    private TextMeshProUGUI alertStrategic;
    private TextMeshProUGUI alertEcological;
    private TextMeshProUGUI alertHazard;
    private TextMeshProUGUI alertLogistics;
    private TextMeshProUGUI alertWeather;
    private TextMeshProUGUI alertWarfare;
    private TextMeshProUGUI alertCollapse;
    private TextMeshProUGUI alertPropagation;

    private bool running = true;
    private float simulationTime;
    private int selectedZoneIndex;

    private readonly string[] zoneNames =
    {
        "AURELIA NORTHERN FOREST",
        "AURELIA EQUATORIAL RAINFOREST",
        "AURELIA WESTERN GRASSLAND",
        "AURELIA SOUTHERN WETLAND",
        "AURELIA EASTERN DESERT"
    };

    private readonly string[] biomeTypes =
    {
        "TEMPERATE FOREST",
        "TROPICAL RAINFOREST",
        "OPEN GRASSLAND",
        "FRESHWATER WETLAND",
        "ARID DESERT"
    };

    private readonly string[] waterTypes =
    {
        "RIVER / LAKE NETWORK",
        "DENSE RIVER NETWORK",
        "SEASONAL STREAMS",
        "RIVER / MARSH SYSTEM",
        "LIMITED SUBSURFACE WATER"
    };

    private readonly string[] soilTypes =
    {
        "RICH LOAM",
        "DEEP ORGANIC SOIL",
        "FERTILE PRAIRIE SOIL",
        "WATER-SATURATED ALLUVIUM",
        "MINERAL / SAND SUBSTRATE"
    };

    private readonly string[] climateTypes =
    {
        "TEMPERATE",
        "TROPICAL",
        "SEMI-ARID",
        "HUMID",
        "ARID"
    };

    private readonly string[] hazardLevels =
    {
        "LOW",
        "MODERATE",
        "LOW",
        "ELEVATED",
        "HIGH"
    };

    private void Awake()
    {
        CacheObjects();
        InitializeLayers();
        InitializeZones();
        BindAllControls();
        UpdateAll();
    }

    private void Update()
    {
        if (!running)
            return;

        simulationTime += Time.deltaTime;

        float wave = Mathf.Sin(simulationTime * 0.18f);
        float wave2 = Mathf.Sin(simulationTime * 0.31f);

        UpdateSimulation(wave, wave2);
    }

    private void CacheObjects()
    {
        objects.Clear();

        foreach (Transform t in GetComponentsInChildren<Transform>(true))
        {
            if (!objects.ContainsKey(t.name))
                objects.Add(t.name, t.gameObject);
        }

        planetName = Text("HUD-220-01A PlanetNameDisplay");
        hemisphere = Text("HUD-220-01B HemisphereDisplay");
        biosphereLayer = Text("HUD-220-01C BiosphereLayerDisplay");
        faction = Text("HUD-220-01D OwnerFactionEmblem");
        timeDisplay = Text("HUD-220-01F TimeControlPanel");

        selectedZoneName = Text("HUD-220-05A SelectedZoneName");
        selectedBiome = Text("HUD-220-05B SelectedZoneBiomeType");
        selectedVegetation = Text("HUD-220-05C SelectedVegetationDensity");
        selectedFauna = Text("HUD-220-05D SelectedFaunaDensity");
        selectedWater = Text("HUD-220-05E SelectedWaterSystemType");
        selectedSoil = Text("HUD-220-05F SelectedSoilComposition");
        selectedClimate = Text("HUD-220-05G SelectedClimateBand");
        selectedHazard = Text("HUD-220-05H SelectedEnvironmentalHazardLevel");

        biodiversity = Text("HUD-220-06A BiodiversityIndexDisplay");
        biomass = Text("HUD-220-06B BiomassDensityDisplay");
        predatorPrey = Text("HUD-220-06C PredatorPreyBalanceDisplay");
        stability = Text("HUD-220-06D EcosystemStabilityDisplay");
        stress = Text("HUD-220-06E EnvironmentalStressDisplay");
        renewal = Text("HUD-220-06F ResourceRenewalRateDisplay");

        warfareImpact = Text("HUD-220-07A WarfareImpactDisplay");
        logisticsImpact = Text("HUD-220-07B LogisticsImpactDisplay");
        climateImpact = Text("HUD-220-07C ClimateImpactDisplay");
        hazardPropagation = Text("HUD-220-07D HazardPropagationDisplay");
        factionActivity = Text("HUD-220-07E FactionActivityImpactDisplay");

        alertStrategic = Text("HUD-220-09A StrategicAlertEntry");
        alertEcological = Text("HUD-220-09B EcologicalAlertEntry");
        alertHazard = Text("HUD-220-09C EnvironmentalHazardAlertEntry");
        alertLogistics = Text("HUD-220-09D LogisticsAlertEntry");
        alertWeather = Text("HUD-220-09E WeatherAlertEntry");
        alertWarfare = Text("HUD-220-09F WarfareAlertEntry");
        alertCollapse = Text("HUD-220-09G BiodiversityCollapseAlertEntry");
        alertPropagation = Text("HUD-220-09H HazardPropagationAlertEntry");
    }

    private TextMeshProUGUI Text(string objectName)
    {
        GameObject target;

        if (!objects.TryGetValue(objectName, out target))
        {
            Debug.LogWarning("[HUD-220] Missing TMP object: " + objectName);
            return null;
        }

        TextMeshProUGUI text = target.GetComponent<TextMeshProUGUI>();

        if (text == null)
            Debug.LogWarning("[HUD-220] Object has no TMP component: " + objectName);

        return text;
    }

    private void InitializeLayers()
    {
        layers.Clear();

        layers["BiomeTypes"] = true;
        layers["VegetationDensity"] = true;
        layers["FaunaDistribution"] = true;
        layers["WaterSystems"] = true;
        layers["SoilComposition"] = true;
        layers["ClimateBands"] = true;
        layers["LongTermWeatherPatterns"] = true;
        layers["EnvironmentalHazards"] = true;
        layers["ResourceNodes"] = true;
        layers["FactionControl"] = true;
        layers["WarfareZones"] = true;
        layers["LogisticsRoutes"] = true;
        layers["TerrainModifiers"] = true;
    }

    private void InitializeZones()
    {
        selectedZoneIndex = 0;
    }

    private void BindAllControls()
    {
        BindToggle(
            "HUD-220-03A LayerToggle_BiomeTypes",
            "BiomeTypes",
            "BIOSPHERE_LAYER_BIOME_TYPES");

        BindToggle(
            "HUD-220-03B LayerToggle_VegetationDensity",
            "VegetationDensity",
            "BIOSPHERE_LAYER_VEGETATION_DENSITY");

        BindToggle(
            "HUD-220-03C LayerToggle_FaunaDistribution",
            "FaunaDistribution",
            "BIOSPHERE_LAYER_FAUNA_DISTRIBUTION");

        BindToggle(
            "HUD-220-03D LayerToggle_WaterSystems",
            "WaterSystems",
            "BIOSPHERE_LAYER_WATER_SYSTEMS");

        BindToggle(
            "HUD-220-03E LayerToggle_SoilComposition",
            "SoilComposition",
            "BIOSPHERE_LAYER_SOIL_COMPOSITION");

        BindToggle(
            "HUD-220-03F LayerToggle_ClimateBands",
            "ClimateBands",
            "BIOSPHERE_LAYER_CLIMATE_BANDS");

        BindToggle(
            "HUD-220-03G LayerToggle_LongTermWeatherPatterns",
            "LongTermWeatherPatterns",
            "BIOSPHERE_LAYER_LONG_TERM_WEATHER_PATTERNS");

        BindToggle(
            "HUD-220-03H LayerToggle_EnvironmentalHazards",
            "EnvironmentalHazards",
            "BIOSPHERE_LAYER_ENVIRONMENTAL_HAZARDS");

        BindToggle(
            "HUD-220-03I LayerToggle_ResourceNodes",
            "ResourceNodes",
            "BIOSPHERE_LAYER_RESOURCE_NODES");

        BindToggle(
            "HUD-220-03J LayerToggle_FactionControl",
            "FactionControl",
            "BIOSPHERE_LAYER_FACTION_CONTROL");

        BindToggle(
            "HUD-220-03K LayerToggle_WarfareZones",
            "WarfareZones",
            "BIOSPHERE_LAYER_WARFARE_ZONES");

        BindToggle(
            "HUD-220-03L LayerToggle_LogisticsRoutes",
            "LogisticsRoutes",
            "BIOSPHERE_LAYER_LOGISTICS_ROUTES");

        BindToggle(
            "HUD-220-03M LayerToggle_TerrainModifiers",
            "TerrainModifiers",
            "BIOSPHERE_LAYER_TERRAIN_MODIFIERS");

        Bind(
            "HUD-220-05I EnterZoneButton",
            EnterZone);

        Bind(
            "HUD-220-08A DeployReconButton",
            DeployRecon);

        Bind(
            "HUD-220-08B DeployEcologySurveyTeamButton",
            DeployEcologySurveyTeam);

        Bind(
            "HUD-220-08C DeployAtmosphericDroneButton",
            DeployAtmosphericDrone);

        Bind(
            "HUD-220-08D OpenIntelNetworkButton",
            () => Navigate("Intelligence"));

        Bind(
            "HUD-220-08E OpenDoctrineManagerButton",
            OpenDoctrineManager);

        Bind(
            "HUD-220-08F OpenLogisticsOverviewButton",
            () => Navigate("Logistics"));

        Bind(
            "HUD-220-08G OpenFactionDiplomacyButton",
            OpenFactionDiplomacy);

        Bind(
            "HUD-220-10A BackButton",
            () => Navigate("Climate_Model"));

        Bind(
            "HUD-220-10B ExitToCommandCenterButton",
            () => Navigate("Command_Center"));

        Bind(
            "HUD-220-10C SettingsButton",
            () => Navigate("Settings"));

        Bind(
            "HUD-220-10D HelpButton",
            ShowHelp);

        BindNavigation("HUD-220-02A ReturnToClimateModelViewButton", "Climate_Model");
        BindNavigation("HUD-220-02B ReturnToStormCellAnalysisButton", "Storm_Cell_Analysis");
        BindNavigation("HUD-220-02C ReturnToAtmosphericSimulationButton", "Atmospheric_Simulation");
        BindNavigation("HUD-220-02D ReturnToWeatherSystemViewButton", "Weather");
        BindNavigation("HUD-220-02E ReturnToLogisticsNetworkMapButton", "Logistics_Network");
        BindNavigation("HUD-220-02F ReturnToResourceDistributionMapButton", "Resource_Distribution");
        BindNavigation("HUD-220-02G ReturnToStrategicHeatmapButton", "Strategic_Heatmap");
        BindNavigation("HUD-220-02H ReturnToFactionTerritoryOverlayButton", "Faction_Territory");
        BindNavigation("HUD-220-02I ReturnToFactionControlMapButton", "Faction_Control");
        BindNavigation("HUD-220-02J ReturnToWarfareProfileButton", "Warfare_Profile");
        BindNavigation("HUD-220-02K ReturnToEnvironmentProfileButton", "Environment_Profile");
        BindNavigation("HUD-220-02L ReturnToIslandMapButton", "Island_Map");
        BindNavigation("HUD-220-02M ReturnToBiomeMapButton", "Biome_Map");
        BindNavigation("HUD-220-02N ReturnToRegionMapButton", "Region_Map");
        BindNavigation("HUD-220-02O ReturnToContinentMapButton", "Continent_Map");
        BindNavigation("HUD-220-02P ReturnToSurfaceMapButton", "Planet_Surface");
        BindNavigation("HUD-220-02Q ReturnToPlanetOverviewButton", "Planet_Overview");
    }

    private void Bind(string objectName, Action action)
    {
        GameObject target;

        if (!objects.TryGetValue(objectName, out target))
        {
            Debug.LogWarning("[HUD-220] Missing button: " + objectName);
            return;
        }

        Button button = target.GetComponent<Button>();

        if (button == null)
        {
            Debug.LogWarning("[HUD-220] Object is not a Button: " + objectName);
            return;
        }

        button.onClick.AddListener(() => action());
    }

    private void BindToggle(
        string objectName,
        string layer,
        string worldObjectName)
    {
        GameObject target;

        if (!objects.TryGetValue(objectName, out target))
        {
            Debug.LogWarning("[HUD-220] Missing toggle: " + objectName);
            return;
        }

        Toggle toggle = target.GetComponent<Toggle>();

        if (toggle == null)
        {
            Debug.LogWarning("[HUD-220] Object is not a Toggle: " + objectName);
            return;
        }

        toggle.isOn = true;

        toggle.onValueChanged.AddListener(value =>
        {
            layers[layer] = value;

            GameObject worldLayer;

            if (objects.TryGetValue(worldObjectName, out worldLayer))
                worldLayer.SetActive(value);

            Alert(
                layer.ToUpperInvariant() +
                " LAYER // " +
                (value ? "VISIBLE" : "HIDDEN"));
        });

        GameObject existingLayer;

        if (objects.TryGetValue(worldObjectName, out existingLayer))
            existingLayer.SetActive(true);
        else
            Debug.LogWarning(
                "[HUD-220] Missing world layer: " +
                worldObjectName);
    }

    private void BindNavigation(string objectName, string scene)
    {
        Bind(objectName, () => Navigate(scene));
    }

    private void Navigate(string keyword)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);

            if (path.IndexOf(
                    keyword,
                    StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Debug.Log(
                    "[HUD-220] Loading scene: " +
                    path);

                SceneManager.LoadScene(path);
                return;
            }
        }

        Alert("TARGET SCENE NOT REGISTERED: " + keyword);
    }

    private void EnterZone()
    {
        selectedZoneIndex++;

        if (selectedZoneIndex >= zoneNames.Length)
            selectedZoneIndex = 0;

        UpdateSelectedZone();

        Alert(
            "ENTERED BIOSPHERE ZONE: " +
            zoneNames[selectedZoneIndex]);
    }

    private void DeployRecon()
    {
        Alert(
            "RECON TEAM DEPLOYED TO SELECTED BIOSPHERE ZONE.");
    }

    private void DeployEcologySurveyTeam()
    {
        Alert("ECOLOGY SURVEY TEAM DEPLOYED.");
    }

    private void DeployAtmosphericDrone()
    {
        Alert("ATMOSPHERIC DRONE DEPLOYED.");
    }

    private void OpenDoctrineManager()
    {
        Alert("DOCTRINE MANAGER REQUEST QUEUED.");
    }

    private void OpenFactionDiplomacy()
    {
        Alert("FACTION DIPLOMACY INTERFACE REQUESTED.");
    }

    private void ShowHelp()
    {
        Alert(
            "BIOSPHERE MAP: SELECT LAYERS AND ENTER A ZONE FOR DETAILED ECOLOGICAL ANALYSIS.");
    }

    private void UpdateSimulation(float wave, float wave2)
    {
        UpdateAll(wave, wave2);
    }

    private void UpdateAll(float wave = 0f, float wave2 = 0f)
    {
        SetText(planetName, "AURELIA");
        SetText(hemisphere, "NORTHERN HEMISPHERE");
        SetText(biosphereLayer, "BIOSPHERE // ECOLOGICAL MODEL");
        SetText(faction, "A");

        SetText(
            timeDisplay,
            "SIM TIME  " +
            simulationTime.ToString("0.0") +
            " h");

        UpdateSelectedZone();
        UpdateStats(wave, wave2);
        UpdateImpacts(wave, wave2);
        UpdateAlerts(wave, wave2);
    }

    private void UpdateSelectedZone()
    {
        int i = Mathf.Clamp(
            selectedZoneIndex,
            0,
            zoneNames.Length - 1);

        SetText(selectedZoneName, zoneNames[i]);

        SetText(
            selectedBiome,
            "BIOME // " + biomeTypes[i]);

        SetText(
            selectedVegetation,
            "VEGETATION DENSITY // " +
            (42 + i * 11) +
            "%");

        SetText(
            selectedFauna,
            "FAUNA DENSITY // " +
            (35 + i * 9) +
            "%");

        SetText(
            selectedWater,
            "WATER // " +
            waterTypes[i]);

        SetText(
            selectedSoil,
            "SOIL // " +
            soilTypes[i]);

        SetText(
            selectedClimate,
            "CLIMATE // " +
            climateTypes[i]);

        SetText(
            selectedHazard,
            "HAZARD LEVEL // " +
            hazardLevels[i]);
    }

    private void UpdateStats(float wave, float wave2)
    {
        float biodiversityValue = 74f + wave * 4f;
        float biomassValue = 68f + wave2 * 6f;
        float balanceValue = 82f + wave * 3f;
        float stabilityValue = 88f + wave2 * 2f;
        float stressValue = 19f - wave * 3f;
        float renewalValue = 71f + wave * 5f;

        SetText(
            biodiversity,
            "BIODIVERSITY INDEX // " +
            biodiversityValue.ToString("0.0"));

        SetText(
            biomass,
            "BIOMASS DENSITY // " +
            biomassValue.ToString("0.0") +
            "%");

        SetText(
            predatorPrey,
            "PREDATOR / PREY BALANCE // " +
            balanceValue.ToString("0.0") +
            "%");

        SetText(
            stability,
            "ECOSYSTEM STABILITY // " +
            stabilityValue.ToString("0.0") +
            "%");

        SetText(
            stress,
            "ENVIRONMENTAL STRESS // " +
            stressValue.ToString("0.0") +
            "%");

        SetText(
            renewal,
            "RESOURCE RENEWAL RATE // " +
            renewalValue.ToString("0.0") +
            "%");
    }

    private void UpdateImpacts(float wave, float wave2)
    {
        SetText(
            warfareImpact,
            "WARFARE IMPACT // " +
            (12f + wave * 2f).ToString("0.0") +
            "%");

        SetText(
            logisticsImpact,
            "LOGISTICS IMPACT // " +
            (18f + wave2 * 3f).ToString("0.0") +
            "%");

        SetText(
            climateImpact,
            "CLIMATE IMPACT // " +
            (24f + wave * 3f).ToString("0.0") +
            "%");

        SetText(
            hazardPropagation,
            "HAZARD PROPAGATION // " +
            (15f + wave2 * 4f).ToString("0.0") +
            "%");

        SetText(
            factionActivity,
            "FACTION ACTIVITY IMPACT // " +
            (21f + wave * 2f).ToString("0.0") +
            "%");
    }

    private void UpdateAlerts(float wave, float wave2)
    {
        SetText(
            alertStrategic,
            "STRATEGIC // BIOSPHERE MONITORING NOMINAL");

        SetText(
            alertEcological,
            "ECOLOGICAL // BIODIVERSITY WITHIN PROJECTED RANGE");

        SetText(
            alertHazard,
            "HAZARD // " +
            (wave2 > 0.5f
                ? "PROPAGATION WATCH"
                : "NO CRITICAL PROPAGATION"));

        SetText(
            alertLogistics,
            "LOGISTICS // ECOLOGICAL ROUTES OPERATIONAL");

        SetText(
            alertWeather,
            "WEATHER // SEASONAL CONDITIONS STABLE");

        SetText(
            alertWarfare,
            "WARFARE // NO SIGNIFICANT ECOLOGICAL DISRUPTION");

        SetText(
            alertCollapse,
            "BIODIVERSITY // COLLAPSE RISK 4.2%");

        SetText(
            alertPropagation,
            "HAZARD PROPAGATION // MONITORING ACTIVE");
    }

    private void SetText(
        TextMeshProUGUI target,
        string value)
    {
        if (target != null)
            target.text = value;
    }

    private void Alert(string message)
    {
        Debug.Log("[HUD-220] " + message);

        SetText(
            alertStrategic,
            "STRATEGIC // " + message);
    }
}
