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

public static class LogisticsNetworkMapHUD215Builder
{
    private const string ScenePath =
        "Assets/Scenes/SCN74_LOGISTICS_NETWORK/Logistics_Network.unity";

    private const int Width = 1920;
    private const int Height = 1080;

    private static readonly Color Bg = new Color(0.015f, 0.025f, 0.035f, 1f);
    private static readonly Color PanelColor = new Color(0.025f, 0.055f, 0.075f, 0.96f);
    private static readonly Color Panel2 = new Color(0.035f, 0.075f, 0.095f, 0.96f);
    private static readonly Color Border = new Color(0.15f, 0.55f, 0.68f, 0.9f);
    private static readonly Color TextColor = new Color(0.78f, 0.92f, 0.96f, 1f);
    private static readonly Color Accent = new Color(0.1f, 0.78f, 0.9f, 1f);
    private static readonly Color Warning = new Color(1f, 0.62f, 0.15f, 1f);
    private static readonly Color Danger = new Color(0.95f, 0.2f, 0.2f, 1f);
    private static readonly Color Good = new Color(0.25f, 0.9f, 0.55f, 1f);

    private static readonly Dictionary<string, GameObject> Layers =
        new Dictionary<string, GameObject>();

    private static TextMeshProUGUI selectedName;
    private static TextMeshProUGUI selectedType;
    private static TextMeshProUGUI selectedFaction;
    private static TextMeshProUGUI selectedStatus;
    private static TextMeshProUGUI selectedCapacity;
    private static TextMeshProUGUI selectedEfficiency;
    private static TextMeshProUGUI selectedThreat;

    [MenuItem("Obsidian Protocol/Worlds/BUILD HUD-215 LOGISTICS NETWORK MAP")]
    public static void Build()
    {
        Layers.Clear();

        EnsureFolder("Assets/Scenes");
        EnsureFolder("Assets/Scenes/SCN74_LOGISTICS_NETWORK");

        var scene = EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene,
            NewSceneMode.Single);

        BuildEnvironment();
        BuildCamera();
        BuildEventSystem();
        BuildHUD();

        EditorSceneManager.SaveScene(scene, ScenePath);
        AddBuildSettings(ScenePath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[HUD-215] Logistics Network Map build complete: " + ScenePath);
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

    private static void AddBuildSettings(string scenePath)
    {
        List<EditorBuildSettingsScene> scenes =
            new List<EditorBuildSettingsScene>(
                EditorBuildSettings.scenes);

        bool exists = false;

        foreach (var s in scenes)
        {
            if (s.path == scenePath)
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
    // ENVIRONMENT
    // ============================================================

    private static void BuildEnvironment()
    {
        GameObject world = new GameObject("SCN74_LOGISTICS_NETWORK_ENVIRONMENT");

        GameObject terrain = CreatePrimitive(
            PrimitiveType.Plane,
            "LogisticsTerrain",
            world.transform,
            new Vector3(0, 0, 0),
            new Vector3(8, 1, 5));

        SetMaterial(terrain, new Color(0.045f, 0.09f, 0.085f));

        GameObject strategicGrid = new GameObject(
            "StrategicGrid",
            typeof(LineRenderer));

        strategicGrid.transform.SetParent(world.transform);

        CreateGrid(strategicGrid, 40, 25);

        CreateLayer("LogisticsRoutes", world.transform);
        CreateLayer("SupplyDepots", world.transform);
        CreateLayer("TransportNodes", world.transform);
        CreateLayer("ReinforcementPaths", world.transform);
        CreateLayer("TradeRoutes", world.transform);
        CreateLayer("ResourceNodes", world.transform);
        CreateLayer("ProcessingFacilities", world.transform);
        CreateLayer("SupplyLineIntegrity", world.transform);
        CreateLayer("LogisticsPressure", world.transform);
        CreateLayer("FactionControl", world.transform);
        CreateLayer("WarfareZones", world.transform);
        CreateLayer("WeatherImpact", world.transform);
        CreateLayer("TerrainModifiers", world.transform);

        BuildLogisticsRoutes();
        BuildSupplyDepots();
        BuildTransportNodes();
        BuildReinforcementPaths();
        BuildTradeRoutes();
        BuildResourceNodes();
        BuildProcessingFacilities();
        BuildIntegrityLayer();
        BuildPressureLayer();
        BuildFactionControl();
        BuildWarfareZones();
        BuildWeatherImpact();
        BuildTerrainModifiers();
    }

    private static GameObject CreateLayer(string name, Transform parent)
    {
        GameObject layer = new GameObject(name);
        layer.transform.SetParent(parent);
        Layers[name] = layer;
        return layer;
    }

    private static GameObject CreatePrimitive(
        PrimitiveType type,
        string name,
        Transform parent,
        Vector3 position,
        Vector3 scale)
    {
        GameObject go = GameObject.CreatePrimitive(type);
        go.name = name;
        go.transform.SetParent(parent);
        go.transform.position = position;
        go.transform.localScale = scale;
        return go;
    }

    private static void SetMaterial(GameObject go, Color color)
    {
        Renderer r = go.GetComponent<Renderer>();
        if (r == null)
            return;

        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null)
            shader = Shader.Find("Standard");

        if (shader == null)
            return;

        Material m = new Material(shader);
        m.color = color;
        r.sharedMaterial = m;
    }

    private static void CreateGrid(GameObject go, int halfX, int halfZ)
    {
        LineRenderer lr = go.GetComponent<LineRenderer>();
        lr.useWorldSpace = true;
        lr.loop = false;
        lr.widthMultiplier = 0.015f;
        lr.positionCount = (halfX * 2 + 1) * 2 + (halfZ * 2 + 1) * 2;

        List<Vector3> points = new List<Vector3>();

        for (int x = -halfX; x <= halfX; x++)
        {
            points.Add(new Vector3(x * 1.6f, 0.025f, -halfZ * 1.6f));
            points.Add(new Vector3(x * 1.6f, 0.025f, halfZ * 1.6f));
        }

        for (int z = -halfZ; z <= halfZ; z++)
        {
            points.Add(new Vector3(-halfX * 1.6f, 0.03f, z * 1.6f));
            points.Add(new Vector3(halfX * 1.6f, 0.03f, z * 1.6f));
        }

        lr.positionCount = points.Count;
        lr.SetPositions(points.ToArray());
    }

    private static void CreateRoute(
        string layerName,
        string name,
        Vector3[] points,
        float width)
    {
        GameObject go = new GameObject(
            name,
            typeof(LineRenderer));

        go.transform.SetParent(Layers[layerName].transform);

        LineRenderer lr = go.GetComponent<LineRenderer>();
        lr.positionCount = points.Length;
        lr.SetPositions(points);
        lr.useWorldSpace = true;
        lr.widthMultiplier = width;

        Shader shader = Shader.Find("Universal Render Pipeline/Unlit");
        if (shader == null)
            shader = Shader.Find("Sprites/Default");

        if (shader != null)
        {
            Material m = new Material(shader);

            if (layerName == "TradeRoutes")
                m.color = new Color(0.95f, 0.75f, 0.2f);
            else if (layerName == "ReinforcementPaths")
                m.color = new Color(0.95f, 0.25f, 0.25f);
            else
                m.color = new Color(0.1f, 0.8f, 0.95f);

            lr.sharedMaterial = m;
        }
    }

    private static void BuildLogisticsRoutes()
    {
        CreateRoute(
            "LogisticsRoutes",
            "LR-ALPHA",
            new[]
            {
                new Vector3(-28, .15f, -15),
                new Vector3(-14, .18f, -7),
                new Vector3(0, .2f, 0),
                new Vector3(15, .18f, 8),
                new Vector3(29, .15f, 14)
            },
            .18f);

        CreateRoute(
            "LogisticsRoutes",
            "LR-BRAVO",
            new[]
            {
                new Vector3(-30, .15f, 12),
                new Vector3(-17, .18f, 7),
                new Vector3(-3, .2f, 4),
                new Vector3(12, .18f, -4),
                new Vector3(28, .15f, -13)
            },
            .14f);

        CreateRoute(
            "LogisticsRoutes",
            "LR-CHARLIE",
            new[]
            {
                new Vector3(-22, .15f, 18),
                new Vector3(-8, .2f, 12),
                new Vector3(8, .2f, 11),
                new Vector3(23, .15f, 18)
            },
            .12f);
    }

    private static void BuildSupplyDepots()
    {
        Vector3[] positions =
        {
            new Vector3(-22, .5f, -12),
            new Vector3(0, .5f, 0),
            new Vector3(23, .5f, 14),
            new Vector3(20, .5f, -13)
        };

        for (int i = 0; i < positions.Length; i++)
        {
            GameObject go = CreatePrimitive(
                PrimitiveType.Cube,
                "SupplyDepot_" + (i + 1).ToString("00"),
                Layers["SupplyDepots"].transform,
                positions[i],
                new Vector3(2.2f, 1f, 2.2f));

            SetMaterial(go, new Color(.12f, .4f, .5f));
        }
    }

    private static void BuildTransportNodes()
    {
        Vector3[] positions =
        {
            new Vector3(-14, .8f, -7),
            new Vector3(-3, .8f, 4),
            new Vector3(15, .8f, 8),
            new Vector3(12, .8f, -4),
            new Vector3(-8, .8f, 12)
        };

        for (int i = 0; i < positions.Length; i++)
        {
            GameObject go = CreatePrimitive(
                PrimitiveType.Cylinder,
                "TransportNode_" + (i + 1).ToString("00"),
                Layers["TransportNodes"].transform,
                positions[i],
                new Vector3(1.2f, .7f, 1.2f));

            SetMaterial(go, Accent);
        }
    }

    private static void BuildReinforcementPaths()
    {
        CreateRoute(
            "ReinforcementPaths",
            "REINFORCEMENT-A",
            new[]
            {
                new Vector3(-26, .25f, 15),
                new Vector3(-10, .3f, 7),
                new Vector3(2, .3f, 0)
            },
            .22f);

        CreateRoute(
            "ReinforcementPaths",
            "REINFORCEMENT-B",
            new[]
            {
                new Vector3(2, .3f, 0),
                new Vector3(13, .3f, -5),
                new Vector3(27, .25f, -14)
            },
            .22f);
    }

    private static void BuildTradeRoutes()
    {
        CreateRoute(
            "TradeRoutes",
            "TRADE-EAST",
            new[]
            {
                new Vector3(-21, .2f, -12),
                new Vector3(-4, .25f, -3),
                new Vector3(17, .2f, 10),
                new Vector3(27, .2f, 14)
            },
            .13f);

        CreateRoute(
            "TradeRoutes",
            "TRADE-WEST",
            new[]
            {
                new Vector3(20, .2f, -13),
                new Vector3(5, .25f, -4),
                new Vector3(-20, .2f, -12)
            },
            .13f);
    }

    private static void BuildResourceNodes()
    {
        Vector3[] positions =
        {
            new Vector3(-17, .35f, 13),
            new Vector3(-8, .35f, -14),
            new Vector3(5, .35f, 15),
            new Vector3(18, .35f, -2),
            new Vector3(26, .35f, 5),
            new Vector3(-26, .35f, 2)
        };

        for (int i = 0; i < positions.Length; i++)
        {
            GameObject go = CreatePrimitive(
                PrimitiveType.Sphere,
                "ResourceNode_" + (i + 1).ToString("00"),
                Layers["ResourceNodes"].transform,
                positions[i],
                Vector3.one * 1.1f);

            SetMaterial(go, new Color(.75f, .55f, .15f));
        }
    }

    private static void BuildProcessingFacilities()
    {
        Vector3[] positions =
        {
            new Vector3(-7, 1f, -3),
            new Vector3(9, 1f, 13),
            new Vector3(16, 1f, -11)
        };

        for (int i = 0; i < positions.Length; i++)
        {
            GameObject go = CreatePrimitive(
                PrimitiveType.Cube,
                "ProcessingFacility_" + (i + 1).ToString("00"),
                Layers["ProcessingFacilities"].transform,
                positions[i],
                new Vector3(3f, 2f, 3f));

            SetMaterial(go, new Color(.55f, .2f, .65f));
        }
    }

    private static void BuildIntegrityLayer()
    {
        CreateMarker(
            "SupplyLineIntegrity",
            "Integrity_Critical",
            new Vector3(12, .15f, -4),
            new Color(1f, .15f, .15f),
            2.4f);

        CreateMarker(
            "SupplyLineIntegrity",
            "Integrity_Degraded",
            new Vector3(-14, .15f, -7),
            Warning,
            2f);

        CreateMarker(
            "SupplyLineIntegrity",
            "Integrity_Stable",
            new Vector3(15, .15f, 8),
            Good,
            2f);
    }

    private static void BuildPressureLayer()
    {
        CreateMarker(
            "LogisticsPressure",
            "Pressure_High",
            new Vector3(18, .18f, -2),
            Danger,
            3f);

        CreateMarker(
            "LogisticsPressure",
            "Pressure_Medium",
            new Vector3(-7, .18f, -3),
            Warning,
            2.5f);
    }

    private static void BuildFactionControl()
    {
        CreateZone(
            "FactionControl",
            "Faction_Aegis",
            new Vector3(-16, .1f, 6),
            new Vector3(20, .15f, 18),
            new Color(.08f, .3f, .75f, .22f));

        CreateZone(
            "FactionControl",
            "Faction_Vanguard",
            new Vector3(15, .1f, -6),
            new Vector3(22, .15f, 18),
            new Color(.75f, .12f, .12f, .22f));
    }

    private static void BuildWarfareZones()
    {
        CreateZone(
            "WarfareZones",
            "ActiveConflictZone",
            new Vector3(8, .16f, -1),
            new Vector3(16, .2f, 13),
            new Color(1f, .2f, .1f, .25f));
    }

    private static void BuildWeatherImpact()
    {
        CreateZone(
            "WeatherImpact",
            "StormFront",
            new Vector3(-18, .12f, -8),
            new Vector3(18, .1f, 12),
            new Color(.25f, .45f, .8f, .2f));

        CreateZone(
            "WeatherImpact",
            "DustField",
            new Vector3(17, .12f, 11),
            new Vector3(13, .1f, 10),
            new Color(.75f, .55f, .25f, .18f));
    }

    private static void BuildTerrainModifiers()
    {
        CreateZone(
            "TerrainModifiers",
            "MountainPass",
            new Vector3(-3, .14f, 14),
            new Vector3(12, .1f, 7),
            new Color(.3f, .3f, .35f, .3f));

        CreateZone(
            "TerrainModifiers",
            "SwampBasin",
            new Vector3(3, .14f, -12),
            new Vector3(17, .1f, 7),
            new Color(.15f, .5f, .35f, .28f));
    }

    private static void CreateMarker(
        string layerName,
        string name,
        Vector3 position,
        Color color,
        float size)
    {
        GameObject go = CreatePrimitive(
            PrimitiveType.Sphere,
            name,
            Layers[layerName].transform,
            position,
            Vector3.one * size);

        SetMaterial(go, color);
    }

    private static void CreateZone(
        string layerName,
        string name,
        Vector3 position,
        Vector3 scale,
        Color color)
    {
        GameObject go = CreatePrimitive(
            PrimitiveType.Cube,
            name,
            Layers[layerName].transform,
            position,
            scale);

        SetMaterial(go, color);
    }

    // ============================================================
    // CAMERA / EVENT SYSTEM
    // ============================================================

    private static void BuildCamera()
    {
        GameObject cameraObject = new GameObject("SCN74_LogisticsNetworkCamera");
        Camera camera = cameraObject.AddComponent<Camera>();

        cameraObject.transform.position = new Vector3(0, 55, 0);
        cameraObject.transform.rotation =
            Quaternion.Euler(90f, 0f, 0f);

        camera.orthographic = true;
        camera.orthographicSize = 30f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = Bg;
        camera.nearClipPlane = .1f;
        camera.farClipPlane = 200f;
    }

    private static void BuildEventSystem()
    {
        GameObject eventSystem = new GameObject("EventSystem");

        eventSystem.AddComponent<EventSystem>();
        eventSystem.AddComponent<InputSystemUIInputModule>();
    }

    // ============================================================
    // HUD
    // ============================================================

    private static void BuildHUD()
    {
        GameObject root = new GameObject(
            "HUD-215 LogisticsNetworkMapHUD");

        Canvas canvas = root.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = root.AddComponent<CanvasScaler>();
        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution =
            new Vector2(Width, Height);
        scaler.screenMatchMode =
            CanvasScaler.ScreenMatchMode.Expand;

        root.AddComponent<GraphicRaycaster>();

        BuildTopBar(root.transform);
        BuildNavigationPanel(root.transform);
        BuildLogisticsLayersPanel(root.transform);
        BuildLogisticsLegendPanel(root.transform);
        BuildLogisticsInfoPanel(root.transform);
        BuildSupplyLinePanel(root.transform);
        BuildEconomicPanel(root.transform);
        BuildQuickActionsPanel(root.transform);
        BuildAlertsPanel(root.transform);
        BuildBottomBar(root.transform);
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

        Image image = go.GetComponent<Image>();
        image.color = PanelColor;

        return go;
    }

    private static TextMeshProUGUI Text(
        Transform parent,
        string name,
        string value,
        Vector2 position,
        Vector2 size,
        int fontSize = 20)
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
        tmp.alignment = TextAlignmentOptions.Left;
        tmp.textWrappingMode = TextWrappingModes.Normal;

        return tmp;
    }

    private static Button Button(
        Transform parent,
        string name,
        string label,
        Vector2 position,
        Vector2 size,
        UnityEngine.Events.UnityAction action)
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
        image.color = Panel2;

        Button button = go.GetComponent<Button>();
        button.targetGraphic = image;

        if (action != null)
            button.onClick.AddListener(action);

        Text(
            go.transform,
            "Label",
            label,
            Vector2.zero,
            size - new Vector2(10, 4),
            15).alignment = TextAlignmentOptions.Center;

        return button;
    }

    private static void BuildTopBar(Transform root)
    {
        GameObject p = Panel(
            root,
            "HUD-215-01 TopBar",
            new Vector2(0, 485),
            new Vector2(1880, 70));

        Text(p.transform, "HUD-215-01A PlanetNameDisplay",
            "PLANET // OBSIDIAN WORLD",
            new Vector2(-760, 0),
            new Vector2(260, 45), 21);

        Text(p.transform, "HUD-215-01B ContinentNameDisplay",
            "CONTINENT // NORTHERN REACH",
            new Vector2(-470, 0),
            new Vector2(280, 45), 18);

        Text(p.transform, "HUD-215-01C RegionNameDisplay",
            "REGION // LOGISTICS CORRIDOR 07",
            new Vector2(-150, 0),
            new Vector2(320, 45), 18);

        Text(p.transform, "HUD-215-01D LogisticsLayerDisplay",
            "LAYER // LOGISTICS NETWORK",
            new Vector2(190, 0),
            new Vector2(290, 45), 19);

        Text(p.transform, "HUD-215-01E OwnerFactionEmblem",
            "[ AEGIS ]",
            new Vector2(430, 0),
            new Vector2(150, 45), 20);

        Text(p.transform, "HUD-215-01F PlayerResourcesPanel",
            "CREDITS  284,500   |   SUPPLY  72,840",
            new Vector2(640, 0),
            new Vector2(330, 45), 17);

        Text(p.transform, "HUD-215-01G TimeControlPanel",
            "T+184:22:09",
            new Vector2(835, 0),
            new Vector2(150, 45), 18);
    }

    private static void BuildNavigationPanel(Transform root)
    {
        GameObject p = Panel(
            root,
            "HUD-215-02 NavigationPanel",
            new Vector2(-790, 80),
            new Vector2(250, 760));

        string[] names =
        {
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
            string scene = labels[i];
            Button(
                p.transform,
                "HUD-215-02" +
                (char)('A' + i) + " " + names[i],
                labels[i],
                new Vector2(0, 315 - i * 54),
                new Vector2(220, 42),
                () => Navigation(scene));
        }
    }

    private static void BuildLogisticsLayersPanel(Transform root)
    {
        GameObject p = Panel(
            root,
            "HUD-215-03 LogisticsLayersPanel",
            new Vector2(-790, -325),
            new Vector2(250, 240));

        string[] names =
        {
            "LogisticsRoutes",
            "SupplyDepots",
            "TransportNodes",
            "ReinforcementPaths",
            "TradeRoutes",
            "ResourceNodes",
            "ProcessingFacilities",
            "SupplyLineIntegrity",
            "LogisticsPressure",
            "FactionControl",
            "WarfareZones",
            "WeatherImpact",
            "TerrainModifiers"
        };

        for (int i = 0; i < names.Length; i++)
        {
            int index = i;
            Button(
                p.transform,
                "HUD-215-03" +
                (char)('A' + i) +
                " LayerToggle_" + names[i],
                names[i],
                new Vector2(
                    i % 2 == 0 ? -57 : 57,
                    92 - (i / 2) * 30),
                new Vector2(105, 23),
                () => ToggleLayer(names[index]));
        }
    }

    private static void BuildLogisticsLegendPanel(Transform root)
    {
        GameObject p = Panel(
            root,
            "HUD-215-04 LogisticsLegendPanel",
            new Vector2(780, 295),
            new Vector2(260, 210));

        Text(p.transform, "HUD-215-04A LogisticsRouteLegend",
            "?? LOGISTICS ROUTE",
            new Vector2(0, 75),
            new Vector2(220, 30), 16);

        Text(p.transform, "HUD-215-04B SupplyDepotLegend",
            "¦ SUPPLY DEPOT",
            new Vector2(0, 45),
            new Vector2(220, 30), 16);

        Text(p.transform, "HUD-215-04C TransportNodeLegend",
            "? TRANSPORT NODE",
            new Vector2(0, 15),
            new Vector2(220, 30), 16);

        Text(p.transform, "HUD-215-04D ReinforcementPathLegend",
            "?? REINFORCEMENT",
            new Vector2(0, -15),
            new Vector2(220, 30), 16);

        Text(p.transform, "HUD-215-04E SupplyLineIntegrityLegend",
            "? INTEGRITY",
            new Vector2(0, -45),
            new Vector2(220, 30), 16);

        Text(p.transform, "HUD-215-04F LogisticsPressureLegend",
            "? PRESSURE",
            new Vector2(0, -75),
            new Vector2(220, 30), 16);
    }

    private static void BuildLogisticsInfoPanel(Transform root)
    {
        GameObject p = Panel(
            root,
            "HUD-215-05 LogisticsInfoPanel",
            new Vector2(780, 50),
            new Vector2(260, 330));

        selectedName = Text(
            p.transform,
            "HUD-215-05A SelectedNodeName",
            "NODE // TRANSPORT-03",
            new Vector2(0, 135),
            new Vector2(230, 32), 19);

        selectedType = Text(
            p.transform,
            "HUD-215-05B SelectedNodeType",
            "TYPE // TRANSFER HUB",
            new Vector2(0, 102),
            new Vector2(230, 28), 15);

        selectedFaction = Text(
            p.transform,
            "HUD-215-05C SelectedNodeOwnerFaction",
            "OWNER // AEGIS",
            new Vector2(0, 72),
            new Vector2(230, 28), 15);

        selectedStatus = Text(
            p.transform,
            "HUD-215-05D SelectedNodeStatus",
            "STATUS // OPERATIONAL",
            new Vector2(0, 42),
            new Vector2(230, 28), 15);

        selectedCapacity = Text(
            p.transform,
            "HUD-215-05E SelectedNodeSupplyCapacity",
            "CAPACITY // 84,000",
            new Vector2(0, 12),
            new Vector2(230, 28), 15);

        selectedEfficiency = Text(
            p.transform,
            "HUD-215-05F SelectedNodeTransportEfficiency",
            "EFFICIENCY // 91%",
            new Vector2(0, -18),
            new Vector2(230, 28), 15);

        selectedThreat = Text(
            p.transform,
            "HUD-215-05G SelectedNodeThreatLevel",
            "THREAT // MODERATE",
            new Vector2(0, -48),
            new Vector2(230, 28), 15);

        Button(
            p.transform,
            "HUD-215-05H EnterNodeButton",
            "ENTER NODE",
            new Vector2(0, -112),
            new Vector2(220, 42),
            EnterSelectedNode);
    }

    private static void BuildSupplyLinePanel(Transform root)
    {
        GameObject p = Panel(
            root,
            "HUD-215-06 SupplyLinePanel",
            new Vector2(520, -280),
            new Vector2(330, 205));

        Text(p.transform, "HUD-215-06A SupplyLineIntegrityDisplay",
            "SUPPLY LINE INTEGRITY     87%",
            new Vector2(0, 70),
            new Vector2(300, 30), 17);

        Text(p.transform, "HUD-215-06B LogisticsPressureDisplay",
            "LOGISTICS PRESSURE        42%",
            new Vector2(0, 40),
            new Vector2(300, 30), 17);

        Text(p.transform, "HUD-215-06C ReinforcementRouteDisplay",
            "REINFORCEMENT ROUTE       R-07",
            new Vector2(0, 10),
            new Vector2(300, 30), 16);

        Text(p.transform, "HUD-215-06D TransportCapacityDisplay",
            "TRANSPORT CAPACITY       68%",
            new Vector2(0, -20),
            new Vector2(300, 30), 16);

        Text(p.transform, "HUD-215-06E ProcessingEfficiencyDisplay",
            "PROCESSING EFFICIENCY     81%",
            new Vector2(0, -50),
            new Vector2(300, 30), 16);

        Text(p.transform, "HUD-215-06F VulnerabilityRatingDisplay",
            "VULNERABILITY             LOW",
            new Vector2(0, -80),
            new Vector2(300, 30), 16);
    }

    private static void BuildEconomicPanel(Transform root)
    {
        GameObject p = Panel(
            root,
            "HUD-215-07 EconomicPanel",
            new Vector2(160, -280),
            new Vector2(330, 205));

        Text(p.transform, "HUD-215-07A ResourceFlowRateDisplay",
            "RESOURCE FLOW             12,840/hr",
            new Vector2(0, 70),
            new Vector2(300, 30), 16);

        Text(p.transform, "HUD-215-07B TransportCostDisplay",
            "TRANSPORT COST            184/ton",
            new Vector2(0, 40),
            new Vector2(300, 30), 16);

        Text(p.transform, "HUD-215-07C ProcessingCostDisplay",
            "PROCESSING COST           96/ton",
            new Vector2(0, 10),
            new Vector2(300, 30), 16);

        Text(p.transform, "HUD-215-07D TradeValueDisplay",
            "TRADE VALUE                2.84M",
            new Vector2(0, -20),
            new Vector2(300, 30), 16);

        Text(p.transform, "HUD-215-07E FactionEconomicImpactDisplay",
            "FACTION IMPACT             +8.4%",
            new Vector2(0, -50),
            new Vector2(300, 30), 16);
    }

    private static void BuildQuickActionsPanel(Transform root)
    {
        GameObject p = Panel(
            root,
            "HUD-215-08 QuickActionsPanel",
            new Vector2(-20, -280),
            new Vector2(330, 205));

        string[] names =
        {
            "DeployReconButton",
            "DeployLogisticsTeamButton",
            "DeployTransportFleetButton",
            "OpenIntelNetworkButton",
            "OpenDoctrineManagerButton",
            "OpenLogisticsOverviewButton",
            "OpenFactionDiplomacyButton"
        };

        string[] labels =
        {
            "DEPLOY RECON",
            "LOGISTICS TEAM",
            "TRANSPORT FLEET",
            "INTEL NETWORK",
            "DOCTRINE MANAGER",
            "LOGISTICS OVERVIEW",
            "FACTION DIPLOMACY"
        };

        for (int i = 0; i < names.Length; i++)
        {
            int index = i;

            Button(
                p.transform,
                "HUD-215-08" +
                (char)('A' + i) + " " + names[i],
                labels[i],
                new Vector2(
                    i % 2 == 0 ? -78 : 78,
                    68 - (i / 2) * 35),
                new Vector2(145, 28),
                () => QuickAction(labels[index]));
        }
    }

    private static void BuildAlertsPanel(Transform root)
    {
        GameObject p = Panel(
            root,
            "HUD-215-09 AlertsPanel",
            new Vector2(780, -255),
            new Vector2(260, 270));

        Text(p.transform, "HUD-215-09A StrategicAlertEntry",
            "[STRATEGIC] CORRIDOR PRIORITY HIGH",
            new Vector2(0, 100),
            new Vector2(235, 32), 13);

        Text(p.transform, "HUD-215-09B LogisticsAlertEntry",
            "[LOGISTICS] ROUTE R-07 DEGRADED",
            new Vector2(0, 65),
            new Vector2(235, 32), 13);

        Text(p.transform, "HUD-215-09C ResourceAlertEntry",
            "[RESOURCE] EXTRACTION RATE DROP",
            new Vector2(0, 30),
            new Vector2(235, 32), 13);

        Text(p.transform, "HUD-215-09D WarfareAlertEntry",
            "[WARFARE] THREAT ACTIVITY DETECTED",
            new Vector2(0, -5),
            new Vector2(235, 32), 13);

        Text(p.transform, "HUD-215-09E WeatherAlertEntry",
            "[WEATHER] STORM FRONT APPROACHING",
            new Vector2(0, -40),
            new Vector2(235, 32), 13);

        Text(p.transform, "HUD-215-09F EnvironmentalHazardAlertEntry",
            "[ENV] TERRAIN HAZARD // PASS 04",
            new Vector2(0, -75),
            new Vector2(235, 32), 13);

        Text(p.transform, "HUD-215-09G SupplyLineBreakAlertEntry",
            "[SUPPLY] LINE BREAK RISK 17%",
            new Vector2(0, -110),
            new Vector2(235, 32), 13);

        Text(p.transform, "HUD-215-09H TransportNodeThreatAlertEntry",
            "[NODE] THREAT LEVEL ELEVATED",
            new Vector2(0, -145),
            new Vector2(235, 32), 13);
    }

    private static void BuildBottomBar(Transform root)
    {
        GameObject p = Panel(
            root,
            "HUD-215-10 BottomBar",
            new Vector2(0, -485),
            new Vector2(1880, 65));

        Button(
            p.transform,
            "HUD-215-10A BackButton",
            "BACK",
            new Vector2(-790, 0),
            new Vector2(150, 42),
            Back);

        Button(
            p.transform,
            "HUD-215-10B ExitToCommandCenterButton",
            "COMMAND CENTER",
            new Vector2(610, 0),
            new Vector2(180, 42),
            () => Navigation("COMMAND CENTER"));

        Button(
            p.transform,
            "HUD-215-10C SettingsButton",
            "SETTINGS",
            new Vector2(775, 0),
            new Vector2(150, 42),
            () => Navigation("SETTINGS"));

        Button(
            p.transform,
            "HUD-215-10D HelpButton",
            "HELP",
            new Vector2(900, 0),
            new Vector2(120, 42),
            Help);
    }

    // ============================================================
    // INTERACTION
    // ============================================================

    private static void ToggleLayer(string layerName)
    {
        if (!Layers.ContainsKey(layerName))
            return;

        GameObject layer = Layers[layerName];
        layer.SetActive(!layer.activeSelf);

        Debug.Log(
            "[HUD-215] Layer " +
            layerName +
            " = " +
            layer.activeSelf);
    }

    private static void EnterSelectedNode()
    {
        if (selectedName != null)
            selectedName.text = "NODE // ACCESSING";

        if (selectedStatus != null)
            selectedStatus.text = "STATUS // NODE INTERFACE OPEN";

        Debug.Log("[HUD-215] Enter selected logistics node.");
    }

    private static void QuickAction(string action)
    {
        Debug.Log("[HUD-215] Quick Action: " + action);

        if (selectedStatus != null)
            selectedStatus.text =
                "ACTION // " + action.ToUpperInvariant();
    }

    private static void Help()
    {
        Debug.Log(
            "[HUD-215] HELP: Select logistics nodes, toggle map layers, " +
            "inspect supply lines, and execute logistics quick actions.");
    }

    private static void Back()
    {
        Navigation("RESOURCE DISTRIBUTION");
    }

    private static void Navigation(string destination)
    {
        string sceneName = ResolveSceneName(destination);

        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.Log(
                "[HUD-215] Navigation target not available in Build Settings: " +
                destination);
            return;
        }

        Debug.Log("[HUD-215] Loading: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    private static string ResolveSceneName(string destination)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path =
                SceneUtility.GetScenePathByBuildIndex(i);

            string name =
                System.IO.Path.GetFileNameWithoutExtension(path);

            string normalized =
                name.Replace("_", " ")
                    .Replace("-", " ")
                    .ToUpperInvariant();

            if (normalized.Contains(
                    destination.Replace("_", " ")
                        .Replace("-", " ")
                        .ToUpperInvariant()))
            {
                return name;
            }
        }

        return string.Empty;
    }
}

