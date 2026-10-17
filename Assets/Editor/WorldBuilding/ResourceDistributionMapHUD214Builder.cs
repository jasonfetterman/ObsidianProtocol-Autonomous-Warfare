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

public static class ResourceDistributionMapHUD214Builder
{
    private const string ScenePath =
        "Assets/Scenes/SCN73_RESOURCE_DISTRIBUTION/Resource_Distribution.unity";

    private const string SceneName = "Resource_Distribution";

    private static Canvas canvas;
    private static TextMeshProUGUI statusDisplay;

    private static readonly Dictionary<string, GameObject> LayerObjects =
        new Dictionary<string, GameObject>();

    [MenuItem("Obsidian Protocol/Worlds/BUILD HUD-214 RESOURCE DISTRIBUTION MAP")]
    public static void Build()
    {
        EnsureFolder();

        var scene = EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene,
            NewSceneMode.Single);

        BuildEnvironment();
        BuildCamera();
        BuildEventSystem();
        BuildHUD();

        EditorSceneManager.SaveScene(scene, ScenePath);
        AddToBuildSettings(ScenePath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Selection.activeObject = canvas.gameObject;

        Debug.Log("[HUD-214] Resource Distribution Map built successfully: " + ScenePath);
    }

    private static void EnsureFolder()
    {
        string folder = "Assets/Scenes/SCN73_RESOURCE_DISTRIBUTION";

        if (!AssetDatabase.IsValidFolder("Assets/Scenes"))
            AssetDatabase.CreateFolder("Assets", "Scenes");

        if (!AssetDatabase.IsValidFolder(folder))
            AssetDatabase.CreateFolder("Assets/Scenes", "SCN73_RESOURCE_DISTRIBUTION");
    }

    private static void BuildEnvironment()
    {
        GameObject root = new GameObject("RESOURCE DISTRIBUTION ENVIRONMENT");

        CreateEnvironmentPlane(root.transform);
        CreateTerrain(root.transform);
        CreateResourceFields(root.transform);
        CreateExtractionZones(root.transform);
        CreateProcessingFacilities(root.transform);
        CreateSupplyDepots(root.transform);
        CreateTradeRoutes(root.transform);
        CreateLogisticsRoutes(root.transform);
        CreateFactionAreas(root.transform);
        CreateWarfareZones(root.transform);
        CreateWeatherZones(root.transform);
        CreateTerrainModifiers(root.transform);
        CreateGrid(root.transform);
    }

    private static void CreateEnvironmentPlane(Transform parent)
    {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.name = "ResourceDistributionSurface";
        plane.transform.SetParent(parent);
        plane.transform.position = Vector3.zero;
        plane.transform.localScale = new Vector3(12f, 1f, 8f);

        SetMaterial(
            plane.GetComponent<Renderer>(),
            new Color(0.035f, 0.055f, 0.065f));
    }

    private static void CreateTerrain(Transform parent)
    {
        GameObject terrainRoot = new GameObject("Terrain");
        terrainRoot.transform.SetParent(parent);

        for (int x = -5; x <= 5; x++)
        {
            for (int z = -3; z <= 3; z++)
            {
                float height =
                    0.15f +
                    Mathf.Abs(Mathf.Sin(x * 0.7f + z * 0.4f)) * 0.35f;

                GameObject block =
                    GameObject.CreatePrimitive(PrimitiveType.Cube);

                block.name = "Terrain_" + x + "_" + z;
                block.transform.SetParent(terrainRoot.transform);
                block.transform.position =
                    new Vector3(x * 8f, height * 0.5f, z * 8f);
                block.transform.localScale =
                    new Vector3(7.5f, height, 7.5f);

                SetMaterial(
                    block.GetComponent<Renderer>(),
                    new Color(0.055f, 0.08f, 0.085f));
            }
        }
    }

    private static void CreateResourceFields(Transform parent)
    {
        GameObject root = new GameObject("RESOURCE NODES");
        root.transform.SetParent(parent);

        CreateResourceNode(root.transform, "Titanium Field", new Vector3(-25, 0.45f, 16), 1.0f);
        CreateResourceNode(root.transform, "Rare Earth Deposit", new Vector3(-8, 0.45f, 22), 0.8f);
        CreateResourceNode(root.transform, "Helium-3 Reserve", new Vector3(15, 0.45f, 18), 0.95f);
        CreateResourceNode(root.transform, "Hydrocarbon Basin", new Vector3(27, 0.45f, 2), 0.7f);
        CreateResourceNode(root.transform, "Uranium Deposit", new Vector3(-30, 0.45f, -8), 0.9f);
        CreateResourceNode(root.transform, "Silicate Belt", new Vector3(-5, 0.45f, -18), 0.75f);
        CreateResourceNode(root.transform, "Water Reserve", new Vector3(20, 0.45f, -15), 0.85f);
        CreateResourceNode(root.transform, "Industrial Metals", new Vector3(30, 0.45f, -2), 0.65f);

        LayerObjects["ResourceNodes"] = root;
    }

    private static void CreateResourceNode(
        Transform parent,
        string name,
        Vector3 position,
        float scale)
    {
        GameObject node =
            GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        node.name = name;
        node.transform.SetParent(parent);
        node.transform.position = position;
        node.transform.localScale =
            new Vector3(2.2f * scale, 0.8f, 2.2f * scale);

        SetMaterial(
            node.GetComponent<Renderer>(),
            new Color(0.16f, 0.34f, 0.30f));

        GameObject marker = CreateMarker(
            parent,
            name + "_Marker",
            position + Vector3.up * 0.8f,
            0.7f);

        SetMaterial(
            marker.GetComponent<Renderer>(),
            new Color(0.22f, 0.65f, 0.52f));
    }

    private static void CreateExtractionZones(Transform parent)
    {
        GameObject root = new GameObject("EXTRACTION ZONES");
        root.transform.SetParent(parent);

        CreateZone(root.transform, "ExtractionZone_A", new Vector3(-18, 0.08f, 13), new Vector3(13, 0.1f, 8));
        CreateZone(root.transform, "ExtractionZone_B", new Vector3(15, 0.08f, 13), new Vector3(12, 0.1f, 7));
        CreateZone(root.transform, "ExtractionZone_C", new Vector3(-18, 0.08f, -12), new Vector3(14, 0.1f, 9));

        LayerObjects["ExtractionZones"] = root;
    }

    private static void CreateZone(
        Transform parent,
        string name,
        Vector3 position,
        Vector3 scale)
    {
        GameObject zone =
            GameObject.CreatePrimitive(PrimitiveType.Cube);

        zone.name = name;
        zone.transform.SetParent(parent);
        zone.transform.position = position;
        zone.transform.localScale = scale;

        SetMaterial(
            zone.GetComponent<Renderer>(),
            new Color(0.20f, 0.30f, 0.12f));
    }

    private static void CreateProcessingFacilities(Transform parent)
    {
        GameObject root = new GameObject("PROCESSING FACILITIES");
        root.transform.SetParent(parent);

        CreateFacility(root.transform, "ProcessingFacility_A", new Vector3(-2, 1.5f, 20));
        CreateFacility(root.transform, "ProcessingFacility_B", new Vector3(22, 1.5f, -10));
        CreateFacility(root.transform, "ProcessingFacility_C", new Vector3(-26, 1.5f, -17));

        LayerObjects["ProcessingFacilities"] = root;
    }

    private static void CreateFacility(
        Transform parent,
        string name,
        Vector3 position)
    {
        GameObject building =
            GameObject.CreatePrimitive(PrimitiveType.Cube);

        building.name = name;
        building.transform.SetParent(parent);
        building.transform.position = position;
        building.transform.localScale =
            new Vector3(4f, 3f, 4f);

        SetMaterial(
            building.GetComponent<Renderer>(),
            new Color(0.20f, 0.22f, 0.25f));
    }

    private static void CreateSupplyDepots(Transform parent)
    {
        GameObject root = new GameObject("SUPPLY DEPOTS");
        root.transform.SetParent(parent);

        CreateDepot(root.transform, "SupplyDepot_North", new Vector3(6, 1f, 26));
        CreateDepot(root.transform, "SupplyDepot_Central", new Vector3(5, 1f, 0));
        CreateDepot(root.transform, "SupplyDepot_South", new Vector3(8, 1f, -24));

        LayerObjects["SupplyDepots"] = root;
    }

    private static void CreateDepot(
        Transform parent,
        string name,
        Vector3 position)
    {
        GameObject depot =
            GameObject.CreatePrimitive(PrimitiveType.Cube);

        depot.name = name;
        depot.transform.SetParent(parent);
        depot.transform.position = position;
        depot.transform.localScale =
            new Vector3(3.5f, 2f, 3.5f);

        SetMaterial(
            depot.GetComponent<Renderer>(),
            new Color(0.18f, 0.24f, 0.30f));
    }

    private static void CreateTradeRoutes(Transform parent)
    {
        GameObject root = new GameObject("TRADE ROUTES");
        root.transform.SetParent(parent);

        CreateRoute(
            root.transform,
            "TradeRoute_North",
            new Vector3(-25, 0.15f, 16),
            new Vector3(15, 0.15f, 23));

        CreateRoute(
            root.transform,
            "TradeRoute_East",
            new Vector3(15, 0.15f, 18),
            new Vector3(27, 0.15f, -2));

        LayerObjects["TradeRoutes"] = root;
    }

    private static void CreateLogisticsRoutes(Transform parent)
    {
        GameObject root = new GameObject("LOGISTICS ROUTES");
        root.transform.SetParent(parent);

        CreateRoute(
            root.transform,
            "LogisticsRoute_A",
            new Vector3(6, 0.2f, 26),
            new Vector3(5, 0.2f, 0));

        CreateRoute(
            root.transform,
            "LogisticsRoute_B",
            new Vector3(5, 0.2f, 0),
            new Vector3(8, 0.2f, -24));

        LayerObjects["LogisticsRoutes"] = root;
    }

    private static void CreateRoute(
        Transform parent,
        string name,
        Vector3 a,
        Vector3 b)
    {
        GameObject route =
            GameObject.CreatePrimitive(PrimitiveType.Cube);

        route.name = name;
        route.transform.SetParent(parent);

        Vector3 midpoint = (a + b) * 0.5f;
        Vector3 delta = b - a;

        route.transform.position = midpoint;
        route.transform.localScale =
            new Vector3(
                0.35f,
                0.18f,
                Mathf.Max(1f, delta.magnitude * 0.5f));

        route.transform.rotation =
            Quaternion.LookRotation(delta.normalized, Vector3.up);

        SetMaterial(
            route.GetComponent<Renderer>(),
            new Color(0.45f, 0.32f, 0.10f));
    }

    private static void CreateFactionAreas(Transform parent)
    {
        GameObject root = new GameObject("FACTION CONTROL");
        root.transform.SetParent(parent);

        CreateZone(
            root.transform,
            "FactionTerritory_A",
            new Vector3(-22, 0.03f, 5),
            new Vector3(24, 0.06f, 30));

        CreateZone(
            root.transform,
            "FactionTerritory_B",
            new Vector3(21, 0.04f, -2),
            new Vector3(18, 0.08f, 28));

        LayerObjects["FactionControl"] = root;
    }

    private static void CreateWarfareZones(Transform parent)
    {
        GameObject root = new GameObject("WARFARE ZONES");
        root.transform.SetParent(parent);

        CreateZone(
            root.transform,
            "WarfareZone_Alpha",
            new Vector3(-4, 0.07f, 4),
            new Vector3(12, 0.1f, 16));

        CreateZone(
            root.transform,
            "WarfareZone_Beta",
            new Vector3(13, 0.09f, -13),
            new Vector3(10, 0.12f, 8));

        LayerObjects["WarfareZones"] = root;
    }

    private static void CreateWeatherZones(Transform parent)
    {
        GameObject root = new GameObject("WEATHER IMPACT");
        root.transform.SetParent(parent);

        CreateZone(
            root.transform,
            "WeatherImpact_A",
            new Vector3(-10, 0.06f, -20),
            new Vector3(22, 0.08f, 7));

        LayerObjects["WeatherImpact"] = root;
    }

    private static void CreateTerrainModifiers(Transform parent)
    {
        GameObject root = new GameObject("TERRAIN MODIFIERS");
        root.transform.SetParent(parent);

        CreateZone(
            root.transform,
            "TerrainModifier_A",
            new Vector3(24, 0.05f, 17),
            new Vector3(10, 0.08f, 10));

        LayerObjects["TerrainModifiers"] = root;
    }

    private static void CreateGrid(Transform parent)
    {
        GameObject root = new GameObject("STRATEGIC GRID");
        root.transform.SetParent(parent);

        for (int i = -5; i <= 5; i++)
        {
            CreateGridLine(
                root.transform,
                new Vector3(i * 8f, 0.12f, -30f),
                new Vector3(i * 8f, 0.12f, 30f));

            CreateGridLine(
                root.transform,
                new Vector3(-40f, 0.12f, i * 6f),
                new Vector3(40f, 0.12f, i * 6f));
        }
    }

    private static void CreateGridLine(
        Transform parent,
        Vector3 a,
        Vector3 b)
    {
        GameObject line =
            GameObject.CreatePrimitive(PrimitiveType.Cube);

        line.name = "GridLine";
        line.transform.SetParent(parent);

        Vector3 delta = b - a;

        line.transform.position = (a + b) * 0.5f;
        line.transform.localScale =
            new Vector3(
                0.04f,
                0.03f,
                Mathf.Max(1f, delta.magnitude * 0.5f));

        line.transform.rotation =
            Quaternion.LookRotation(delta.normalized, Vector3.up);

        SetMaterial(
            line.GetComponent<Renderer>(),
            new Color(0.12f, 0.16f, 0.17f));
    }

    private static GameObject CreateMarker(
        Transform parent,
        string name,
        Vector3 position,
        float size)
    {
        GameObject marker =
            GameObject.CreatePrimitive(PrimitiveType.Sphere);

        marker.name = name;
        marker.transform.SetParent(parent);
        marker.transform.position = position;
        marker.transform.localScale =
            Vector3.one * size;

        return marker;
    }

    private static void BuildCamera()
    {
        GameObject cameraObject = new GameObject("ResourceDistributionCamera");
        Camera cam = cameraObject.AddComponent<Camera>();

        cam.orthographic = true;
        cam.orthographicSize = 42f;
        cam.transform.position = new Vector3(0f, 55f, 0f);
        cam.transform.rotation =
            Quaternion.Euler(90f, 0f, 0f);

        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor =
            new Color(0.008f, 0.012f, 0.016f);

        cam.nearClipPlane = 0.1f;
        cam.farClipPlane = 200f;
    }

    private static void BuildEventSystem()
    {
        GameObject eventSystemObject =
            new GameObject("EventSystem");

        eventSystemObject.AddComponent<EventSystem>();
        eventSystemObject.AddComponent<InputSystemUIInputModule>();
    }

    private static void BuildHUD()
    {
        GameObject canvasObject =
            new GameObject("HUD-214 ResourceDistributionMapHUD");

        canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler =
            canvasObject.AddComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution =
            new Vector2(1920f, 1080f);
        scaler.screenMatchMode =
            CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        canvasObject.AddComponent<GraphicRaycaster>();

        BuildTopBar(canvas.transform);
        BuildNavigation(canvas.transform);
        BuildResourceLayers(canvas.transform);
        BuildResourceLegend(canvas.transform);
        BuildResourceInfo(canvas.transform);
        BuildLogistics(canvas.transform);
        BuildEconomic(canvas.transform);
        BuildQuickActions(canvas.transform);
        BuildAlerts(canvas.transform);
        BuildBottomBar(canvas.transform);
    }

    private static void BuildTopBar(Transform parent)
    {
        GameObject panel =
            CreatePanel(
                "HUD-214-01 TopBar",
                parent,
                new Vector2(0f, 1f),
                new Vector2(1f, 1f),
                new Vector2(0f, -72f),
                new Vector2(0f, 0f));

        CreateDisplay("HUD-214-01A PlanetNameDisplay", panel.transform, "PLANET // OBSIDIAN WORLD", 220f, 34f);
        CreateDisplay("HUD-214-01B ContinentNameDisplay", panel.transform, "CONTINENT // CINDERFALL", 220f, 34f);
        CreateDisplay("HUD-214-01C RegionNameDisplay", panel.transform, "REGION // RESOURCE BASIN", 220f, 34f);
        CreateDisplay("HUD-214-01D ResourceLayerDisplay", panel.transform, "RESOURCE DISTRIBUTION // ACTIVE", 250f, 34f);
        CreateDisplay("HUD-214-01E OwnerFactionEmblem", panel.transform, "[ F ]", 70f, 34f);
        CreateDisplay("HUD-214-01F PlayerResourcesPanel", panel.transform, "CREDITS  84,250   |   MATERIALS  62,480", 330f, 34f);
        CreateDisplay("HUD-214-01G TimeControlPanel", panel.transform, "PAUSED  |  1X  2X  4X", 210f, 34f);
    }

    private static void BuildNavigation(Transform parent)
    {
        GameObject panel =
            CreateSidePanel(
                "HUD-214-02 NavigationPanel",
                parent,
                new Vector2(8f, 110f),
                new Vector2(255f, 420f));

        string[] buttons =
        {
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

        for (int i = 0; i < buttons.Length; i++)
        {
            Button button =
                CreateButton(
                    "HUD-214-02" + (char)('A' + i) + " " + buttons[i],
                    panel.transform,
                    labels[i],
                    0f,
                    32f);

            string sceneName = ResolveNavigationScene(buttons[i]);
            button.onClick.AddListener(() =>
            {
                Navigate(sceneName);
            });
        }
    }

    private static void BuildResourceLayers(Transform parent)
    {
        GameObject panel =
            CreateSidePanel(
                "HUD-214-03 ResourceLayersPanel",
                parent,
                new Vector2(8f, 540f),
                new Vector2(255f, 450f));

        AddToggle(panel.transform, "HUD-214-03A LayerToggle_ResourceNodes", "RESOURCE NODES", "ResourceNodes", true);
        AddToggle(panel.transform, "HUD-214-03B LayerToggle_ResourceDensity", "RESOURCE DENSITY", "ResourceNodes", true);
        AddToggle(panel.transform, "HUD-214-03C LayerToggle_ResourceType", "RESOURCE TYPE", "ResourceNodes", true);
        AddToggle(panel.transform, "HUD-214-03D LayerToggle_LogisticsRoutes", "LOGISTICS ROUTES", "LogisticsRoutes", true);
        AddToggle(panel.transform, "HUD-214-03E LayerToggle_SupplyDepots", "SUPPLY DEPOTS", "SupplyDepots", true);
        AddToggle(panel.transform, "HUD-214-03F LayerToggle_ExtractionZones", "EXTRACTION ZONES", "ExtractionZones", true);
        AddToggle(panel.transform, "HUD-214-03G LayerToggle_ProcessingFacilities", "PROCESSING FACILITIES", "ProcessingFacilities", true);
        AddToggle(panel.transform, "HUD-214-03H LayerToggle_TradeRoutes", "TRADE ROUTES", "TradeRoutes", true);
        AddToggle(panel.transform, "HUD-214-03I LayerToggle_FactionControl", "FACTION CONTROL", "FactionControl", false);
        AddToggle(panel.transform, "HUD-214-03J LayerToggle_WarfareZones", "WARFARE ZONES", "WarfareZones", false);
        AddToggle(panel.transform, "HUD-214-03K LayerToggle_WeatherImpact", "WEATHER IMPACT", "WeatherImpact", false);
        AddToggle(panel.transform, "HUD-214-03L LayerToggle_TerrainModifiers", "TERRAIN MODIFIERS", "TerrainModifiers", false);
    }

    private static void BuildResourceLegend(Transform parent)
    {
        GameObject panel =
            CreateSidePanel(
                "HUD-214-04 ResourceLegendPanel",
                parent,
                new Vector2(1640f, 610f),
                new Vector2(270f, 260f));

        CreateDisplay("HUD-214-04A ResourceTypeLegend", panel.transform, "RESOURCE TYPE\nMETALS  |  FUELS  |  RARE EARTH\nWATER  |  FISSILE  |  INDUSTRIAL", 250f, 62f);
        CreateDisplay("HUD-214-04B ResourceDensityLegend", panel.transform, "DENSITY\nLOW  ░░░   MEDIUM  ▒▒▒   HIGH  ███", 250f, 45f);
        CreateDisplay("HUD-214-04C ExtractionZoneLegend", panel.transform, "EXTRACTION ZONES  ■", 250f, 32f);
        CreateDisplay("HUD-214-04D LogisticsRouteLegend", panel.transform, "LOGISTICS ROUTES  ━━━", 250f, 32f);
        CreateDisplay("HUD-214-04E ProcessingFacilityLegend", panel.transform, "PROCESSING FACILITIES  ■", 250f, 32f);
    }

    private static void BuildResourceInfo(Transform parent)
    {
        GameObject panel =
            CreateSidePanel(
                "HUD-214-05 ResourceInfoPanel",
                parent,
                new Vector2(1640f, 320f),
                new Vector2(270f, 275f));

        CreateDisplay("HUD-214-05A SelectedResourceName", panel.transform, "TITANIUM FIELD", 250f, 32f);
        CreateDisplay("HUD-214-05B SelectedResourceType", panel.transform, "TYPE: STRATEGIC METAL", 250f, 30f);
        CreateDisplay("HUD-214-05C SelectedResourceDensity", panel.transform, "DENSITY: HIGH", 250f, 30f);
        CreateDisplay("HUD-214-05D SelectedResourceOwnerFaction", panel.transform, "OWNER: TERRAN DIRECTORATE", 250f, 42f);
        CreateDisplay("HUD-214-05E SelectedResourceExtractionRate", panel.transform, "EXTRACTION: 8,400 / HR", 250f, 30f);
        CreateDisplay("HUD-214-05F SelectedResourceLogisticsDifficulty", panel.transform, "LOGISTICS DIFFICULTY: 34%", 250f, 42f);
        CreateDisplay("HUD-214-05G SelectedResourceThreatLevel", panel.transform, "THREAT LEVEL: 61%", 250f, 30f);

        Button enter =
            CreateButton(
                "HUD-214-05H EnterResourceZoneButton",
                panel.transform,
                "ENTER RESOURCE ZONE",
                0f,
                34f);

        enter.onClick.AddListener(() =>
        {
            SetStatus("ENTERING RESOURCE ZONE // TITANIUM FIELD");
        });
    }

    private static void BuildLogistics(Transform parent)
    {
        GameObject panel =
            CreateSidePanel(
                "HUD-214-06 LogisticsPanel",
                parent,
                new Vector2(1290f, 110f),
                new Vector2(330f, 240f));

        CreateDisplay("HUD-214-06A SupplyLineIntegrityDisplay", panel.transform, "SUPPLY LINE INTEGRITY  87%", 305f, 35f);
        CreateDisplay("HUD-214-06B LogisticsPressureDisplay", panel.transform, "LOGISTICS PRESSURE  42%", 305f, 35f);
        CreateDisplay("HUD-214-06C ReinforcementRouteDisplay", panel.transform, "REINFORCEMENT ROUTES  6 ACTIVE", 305f, 45f);
        CreateDisplay("HUD-214-06D TransportCapacityDisplay", panel.transform, "TRANSPORT CAPACITY  74%", 305f, 35f);
        CreateDisplay("HUD-214-06E ProcessingEfficiencyDisplay", panel.transform, "PROCESSING EFFICIENCY  81%", 305f, 45f);
    }

    private static void BuildEconomic(Transform parent)
    {
        GameObject panel =
            CreateSidePanel(
                "HUD-214-07 EconomicPanel",
                parent,
                new Vector2(1290f, 365f),
                new Vector2(330f, 215f));

        CreateDisplay("HUD-214-07A ResourceValueDisplay", panel.transform, "RESOURCE VALUE  1.42M", 305f, 34f);
        CreateDisplay("HUD-214-07B ExtractionCostDisplay", panel.transform, "EXTRACTION COST  18.4K", 305f, 34f);
        CreateDisplay("HUD-214-07C ProcessingCostDisplay", panel.transform, "PROCESSING COST  11.2K", 305f, 34f);
        CreateDisplay("HUD-214-07D TradeValueDisplay", panel.transform, "TRADE VALUE  2.08M", 305f, 34f);
        CreateDisplay("HUD-214-07E FactionEconomicImpactDisplay", panel.transform, "FACTION ECONOMIC IMPACT  +13%", 305f, 52f);
    }

    private static void BuildQuickActions(Transform parent)
    {
        GameObject panel =
            CreateSidePanel(
                "HUD-214-08 QuickActionsPanel",
                parent,
                new Vector2(1290f, 610f),
                new Vector2(330f, 330f));

        string[] names =
        {
            "DeployReconButton",
            "DeployLogisticsTeamButton",
            "DeployExtractionTeamButton",
            "OpenIntelNetworkButton",
            "OpenDoctrineManagerButton",
            "OpenLogisticsOverviewButton",
            "OpenFactionDiplomacyButton"
        };

        string[] labels =
        {
            "DEPLOY RECON",
            "DEPLOY LOGISTICS TEAM",
            "DEPLOY EXTRACTION TEAM",
            "OPEN INTEL NETWORK",
            "OPEN DOCTRINE MANAGER",
            "OPEN LOGISTICS OVERVIEW",
            "OPEN FACTION DIPLOMACY"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Button button =
                CreateButton(
                    "HUD-214-08" + (char)('A' + i) + " " + names[i],
                    panel.transform,
                    labels[i],
                    0f,
                    36f);

            string action = labels[i];

            button.onClick.AddListener(() =>
            {
                SetStatus(action + " // COMMAND ACCEPTED");
            });
        }
    }

    private static void BuildAlerts(Transform parent)
    {
        GameObject panel =
            CreateSidePanel(
                "HUD-214-09 AlertsPanel",
                parent,
                new Vector2(1640f, 110f),
                new Vector2(270f, 190f));

        CreateDisplay("HUD-214-09A StrategicAlertEntry", panel.transform, "STRATEGIC  // RESOURCE PRIORITY SHIFT", 250f, 40f);
        CreateDisplay("HUD-214-09B ResourceAlertEntry", panel.transform, "RESOURCE  // TITANIUM OUTPUT RISING", 250f, 40f);
        CreateDisplay("HUD-214-09C LogisticsAlertEntry", panel.transform, "LOGISTICS // ROUTE CAPACITY 74%", 250f, 40f);
        CreateDisplay("HUD-214-09D WarfareAlertEntry", panel.transform, "WARFARE    // EXTRACTION ZONE THREAT", 250f, 40f);
        CreateDisplay("HUD-214-09E WeatherAlertEntry", panel.transform, "WEATHER    // LOW IMPACT", 250f, 40f);
        CreateDisplay("HUD-214-09F EnvironmentalHazardAlertEntry", panel.transform, "HAZARD     // NONE DETECTED", 250f, 40f);
        CreateDisplay("HUD-214-09G SupplyLineBreakAlertEntry", panel.transform, "SUPPLY     // NO BREAKS", 250f, 40f);
        CreateDisplay("HUD-214-09H ExtractionZoneThreatAlertEntry", panel.transform, "EXTRACTION // WATCH LEVEL", 250f, 40f);
    }

    private static void BuildBottomBar(Transform parent)
    {
        GameObject panel =
            CreatePanel(
                "HUD-214-10 BottomBar",
                parent,
                new Vector2(0f, 0f),
                new Vector2(1f, 0f),
                new Vector2(0f, 68f),
                new Vector2(0f, 0f));

        Button back =
            CreateButton(
                "HUD-214-10A BackButton",
                panel.transform,
                "BACK",
                130f,
                42f);

        back.onClick.AddListener(() =>
        {
            Navigate("Strategic_Heatmap");
        });

        Button command =
            CreateButton(
                "HUD-214-10B ExitToCommandCenterButton",
                panel.transform,
                "COMMAND CENTER",
                180f,
                42f);

        command.onClick.AddListener(() =>
        {
            Navigate("Command_Center");
        });

        Button settings =
            CreateButton(
                "HUD-214-10C SettingsButton",
                panel.transform,
                "SETTINGS",
                130f,
                42f);

        settings.onClick.AddListener(() =>
        {
            SetStatus("SETTINGS // COMMAND REQUESTED");
        });

        Button help =
            CreateButton(
                "HUD-214-10D HelpButton",
                panel.transform,
                "HELP",
                110f,
                42f);

        help.onClick.AddListener(() =>
        {
            SetStatus("RESOURCE DISTRIBUTION MAP // HELP ACTIVE");
        });

        GameObject status =
            CreateDisplay(
                "HUD-214_STATUS",
                panel.transform,
                "RESOURCE DISTRIBUTION MAP // READY",
                420f,
                42f);

        statusDisplay =
            status.GetComponent<TextMeshProUGUI>();
    }

    private static GameObject CreatePanel(
        string name,
        Transform parent,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 size,
        Vector2 position)
    {
        GameObject go = new GameObject(name);

        go.transform.SetParent(parent, false);

        RectTransform rect =
            go.AddComponent<RectTransform>();

        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.sizeDelta = size;
        rect.anchoredPosition = position;

        Image image = go.AddComponent<Image>();
        image.color = new Color(0.015f, 0.025f, 0.032f, 0.94f);

        return go;
    }

    private static GameObject CreateSidePanel(
        string name,
        Transform parent,
        Vector2 position,
        Vector2 size)
    {
        GameObject go =
            CreatePanel(
                name,
                parent,
                new Vector2(0f, 0f),
                new Vector2(0f, 0f),
                size,
                position);

        VerticalLayoutGroup layout =
            go.AddComponent<VerticalLayoutGroup>();

        layout.padding =
            new RectOffset(10, 10, 10, 10);

        layout.spacing = 5f;
        layout.childControlWidth = true;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;

        return go;
    }

    private static GameObject CreateDisplay(
        string name,
        Transform parent,
        string value,
        float width,
        float height)
    {
        GameObject go = new GameObject(name);

        go.transform.SetParent(parent, false);

        RectTransform rect =
            go.AddComponent<RectTransform>();

        rect.sizeDelta =
            new Vector2(width, height);

        TextMeshProUGUI text =
            go.AddComponent<TextMeshProUGUI>();

        text.text = value;
        text.fontSize = 16f;
        text.alignment = TextAlignmentOptions.Left;
        text.color = new Color(0.72f, 0.82f, 0.84f);
        text.textWrappingMode = TextWrappingModes.Normal;
        text.raycastTarget = false;

        LayoutElement element =
            go.AddComponent<LayoutElement>();

        element.preferredHeight = height;
        element.minHeight = height;

        return go;
    }

    private static Button CreateButton(
        string name,
        Transform parent,
        string label,
        float width,
        float height)
    {
        GameObject go = new GameObject(name);

        go.transform.SetParent(parent, false);

        RectTransform rect =
            go.AddComponent<RectTransform>();

        rect.sizeDelta =
            new Vector2(width > 0f ? width : 220f, height);

        Image image =
            go.AddComponent<Image>();

        image.color =
            new Color(0.07f, 0.11f, 0.13f, 0.98f);

        Button button =
            go.AddComponent<Button>();

        button.targetGraphic = image;

        ColorBlock colors = button.colors;
        colors.normalColor =
            new Color(0.07f, 0.11f, 0.13f, 0.98f);
        colors.highlightedColor =
            new Color(0.13f, 0.23f, 0.25f, 1f);
        colors.pressedColor =
            new Color(0.20f, 0.34f, 0.35f, 1f);
        colors.selectedColor =
            new Color(0.13f, 0.23f, 0.25f, 1f);
        button.colors = colors;

        GameObject textObject =
            new GameObject("Label");

        textObject.transform.SetParent(go.transform, false);

        RectTransform textRect =
            textObject.AddComponent<RectTransform>();

        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = new Vector2(8f, 2f);
        textRect.offsetMax = new Vector2(-8f, -2f);

        TextMeshProUGUI text =
            textObject.AddComponent<TextMeshProUGUI>();

        text.text = label;
        text.fontSize = 13f;
        text.alignment = TextAlignmentOptions.Center;
        text.color = new Color(0.78f, 0.88f, 0.90f);
        text.textWrappingMode = TextWrappingModes.Normal;
        text.raycastTarget = false;

        LayoutElement element =
            go.AddComponent<LayoutElement>();

        element.preferredHeight = height;
        element.minHeight = height;

        return button;
    }

    private static void AddToggle(
        Transform parent,
        string objectName,
        string label,
        string layerKey,
        bool initialState)
    {
        GameObject go = new GameObject(objectName);

        go.transform.SetParent(parent, false);

        RectTransform rect =
            go.AddComponent<RectTransform>();

        rect.sizeDelta =
            new Vector2(220f, 32f);

        Toggle toggle =
            go.AddComponent<Toggle>();

        GameObject background =
            new GameObject("Background");

        background.transform.SetParent(go.transform, false);

        RectTransform bgRect =
            background.AddComponent<RectTransform>();

        bgRect.anchorMin = new Vector2(0f, 0.5f);
        bgRect.anchorMax = new Vector2(0f, 0.5f);
        bgRect.sizeDelta = new Vector2(24f, 24f);
        bgRect.anchoredPosition = new Vector2(12f, 0f);

        Image bgImage =
            background.AddComponent<Image>();

        bgImage.color =
            new Color(0.08f, 0.12f, 0.13f, 1f);

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
            new Color(0.28f, 0.70f, 0.55f, 1f);

        toggle.graphic = checkImage;
        toggle.targetGraphic = bgImage;
        toggle.isOn = initialState;

        GameObject textObject =
            new GameObject("Label");

        textObject.transform.SetParent(go.transform, false);

        RectTransform textRect =
            textObject.AddComponent<RectTransform>();

        textRect.anchorMin = new Vector2(0f, 0f);
        textRect.anchorMax = new Vector2(1f, 1f);
        textRect.offsetMin = new Vector2(40f, 0f);
        textRect.offsetMax = new Vector2(0f, 0f);

        TextMeshProUGUI text =
            textObject.AddComponent<TextMeshProUGUI>();

        text.text = label;
        text.fontSize = 12f;
        text.alignment = TextAlignmentOptions.Left;
        text.color = new Color(0.68f, 0.78f, 0.80f);
        text.raycastTarget = false;

        toggle.onValueChanged.AddListener(value =>
        {
            if (LayerObjects.TryGetValue(layerKey, out GameObject layer) &&
                layer != null)
            {
                layer.SetActive(value);
            }

            SetStatus(label + (value ? " // ENABLED" : " // DISABLED"));
        });
    }

    private static string ResolveNavigationScene(string buttonName)
    {
        switch (buttonName)
        {
            case "ReturnToStrategicHeatmapButton":
                return "Strategic_Heatmap";

            case "ReturnToFactionTerritoryOverlayButton":
                return "Faction_Territory_Overlay";

            case "ReturnToFactionControlMapButton":
                return "Faction_Control";

            case "ReturnToWarfareProfileButton":
                return "Warfare_Profile";

            case "ReturnToEnvironmentProfileButton":
                return "Environment_Profile";

            case "ReturnToIslandMapButton":
                return "Island_Map";

            case "ReturnToBiomeMapButton":
                return "Biome_Map";

            case "ReturnToRegionMapButton":
                return "Region_Map";

            case "ReturnToContinentMapButton":
                return "Continent_Map";

            case "ReturnToSurfaceMapButton":
                return "Planet_Surface_Map";

            case "ReturnToPlanetOverviewButton":
                return "Planet_Overview";

            default:
                return "";
        }
    }

    private static void Navigate(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            SetStatus("NAVIGATION TARGET NOT DEFINED");
            return;
        }

        if (Application.isPlaying)
        {
            if (Application.CanStreamedLevelBeLoaded(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                SetStatus("SCENE NOT IN BUILD SETTINGS // " + sceneName);
            }

            return;
        }

        SetStatus("NAVIGATION // " + sceneName);
    }

    private static void SetStatus(string message)
    {
        if (statusDisplay != null)
            statusDisplay.text = message;

        Debug.Log("[HUD-214] " + message);
    }

    private static void SetMaterial(
        Renderer renderer,
        Color color)
    {
        if (renderer == null)
            return;

        Shader shader =
            Shader.Find("Universal Render Pipeline/Lit");

        if (shader == null)
            shader = Shader.Find("Standard");

        if (shader == null)
            return;

        Material material =
            new Material(shader);

        material.color = color;

        renderer.sharedMaterial = material;
    }

    private static void AddToBuildSettings(string scenePath)
    {
        List<EditorBuildSettingsScene> scenes =
            new List<EditorBuildSettingsScene>(
                EditorBuildSettings.scenes);

        bool exists = false;

        foreach (EditorBuildSettingsScene scene in scenes)
        {
            if (scene.path == scenePath)
            {
                exists = true;
                break;
            }
        }

        if (!exists)
        {
            scenes.Add(
                new EditorBuildSettingsScene(
                    scenePath,
                    true));

            EditorBuildSettings.scenes =
                scenes.ToArray();
        }
    }
}
