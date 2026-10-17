using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public static class StrategicHeatmapHUD213Builder
{
    private const string ScenePath =
        "Assets/Scenes/SCN72_STRATEGIC_HEATMAP/Strategic_Heatmap.unity";

    private static readonly Color Panel =
        new Color(0.025f, 0.035f, 0.055f, 0.95f);

    private static readonly Color Header =
        new Color(0.055f, 0.075f, 0.105f, 0.98f);

    private static readonly Color Button =
        new Color(0.075f, 0.105f, 0.145f, 0.98f);

    private static readonly Color Accent =
        new Color(0.18f, 0.68f, 0.95f, 1f);

    [MenuItem("Obsidian Protocol/Worlds/BUILD HUD-213 STRATEGIC HEATMAP")]
    public static void Build()
    {
        EnsureFolder("Assets/Scenes");
        EnsureFolder("Assets/Scenes/SCN72_STRATEGIC_HEATMAP");

        EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene,
            NewSceneMode.Single);

        BuildHeatmapEnvironment();
        BuildCamera();
        BuildEventSystem();
        BuildHUD();

        Scene scene = SceneManager.GetActiveScene();

        EditorSceneManager.SaveScene(
            scene,
            ScenePath);

        AddToBuildSettings(ScenePath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "[HUD-213] Strategic Heatmap built: " +
            ScenePath);
    }

    // =========================================================
    // ENVIRONMENT
    // =========================================================

    private static void BuildHeatmapEnvironment()
    {
        GameObject root =
            new GameObject(
                "STRATEGIC HEATMAP ENVIRONMENT");

        CreateBlock(
            root.transform,
            "StrategicTerrain",
            new Vector3(0f, -0.65f, 0f),
            new Vector3(30f, 1f, 22f),
            new Color(0.055f, 0.075f, 0.09f));

        // Low-intensity background sectors.
        CreateHeatCell(root.transform, -10f, 7f, 3.5f, 0.12f);
        CreateHeatCell(root.transform, -3f, 7f, 3.2f, 0.25f);
        CreateHeatCell(root.transform, 5f, 7f, 3.8f, 0.42f);
        CreateHeatCell(root.transform, 11f, 6f, 3.0f, 0.18f);

        CreateHeatCell(root.transform, -11f, 0f, 3.2f, 0.28f);
        CreateHeatCell(root.transform, -4f, 0f, 4.0f, 0.48f);
        CreateHeatCell(root.transform, 4f, 0f, 4.3f, 0.86f);
        CreateHeatCell(root.transform, 11f, 0f, 3.2f, 0.55f);

        CreateHeatCell(root.transform, -9f, -7f, 3.5f, 0.16f);
        CreateHeatCell(root.transform, -2f, -7f, 3.7f, 0.39f);
        CreateHeatCell(root.transform, 5f, -7f, 4.0f, 0.72f);
        CreateHeatCell(root.transform, 11f, -6f, 3.0f, 0.91f);

        // Strategic hotspots.
        CreateHotspot(
            root.transform,
            "CriticalConflictHotspot",
            new Vector3(4f, 0.85f, 0f),
            1.25f,
            new Color(0.95f, 0.12f, 0.08f, 0.95f));

        CreateHotspot(
            root.transform,
            "HighThreatHotspot",
            new Vector3(10f, 0.84f, -6f),
            1.05f,
            new Color(0.95f, 0.28f, 0.08f, 0.90f));

        CreateHotspot(
            root.transform,
            "LogisticsPressureHotspot",
            new Vector3(-3f, 0.83f, 2f),
            0.9f,
            new Color(0.95f, 0.62f, 0.08f, 0.88f));

        CreateHotspot(
            root.transform,
            "ResistanceHotspot",
            new Vector3(-9f, 0.82f, -4f),
            0.8f,
            new Color(0.72f, 0.18f, 0.80f, 0.85f));

        // Strategic routes.
        BuildRoute(
            root.transform,
            "PrimaryLogisticsPressureRoute",
            new[]
            {
                new Vector3(-10f, 0.80f, 5f),
                new Vector3(-5f, 0.80f, 3f),
                new Vector3(-1f, 0.80f, 2f),
                new Vector3(4f, 0.80f, 0f),
                new Vector3(10f, 0.80f, -5f)
            },
            new Color(0.20f, 0.80f, 0.95f));

        BuildRoute(
            root.transform,
            "ReinforcementDensityRoute",
            new[]
            {
                new Vector3(-8f, 0.82f, -6f),
                new Vector3(-3f, 0.82f, -3f),
                new Vector3(2f, 0.82f, -1f),
                new Vector3(7f, 0.82f, 2f),
                new Vector3(11f, 0.82f, 6f)
            },
            new Color(0.95f, 0.70f, 0.18f));

        // Grid.
        BuildGrid(root.transform);

        // Strategic markers.
        CreateMarker(
            root.transform,
            "CommandNode",
            new Vector3(-7f, 0.8f, 6f),
            Accent,
            0.55f);

        CreateMarker(
            root.transform,
            "EnemyConcentration",
            new Vector3(7f, 0.8f, -4f),
            new Color(0.95f, 0.16f, 0.12f),
            0.6f);

        CreateMarker(
            root.transform,
            "SupplyHub",
            new Vector3(-2f, 0.8f, 3f),
            new Color(0.20f, 0.85f, 0.55f),
            0.5f);

        CreateMarker(
            root.transform,
            "StrategicResource",
            new Vector3(8f, 0.8f, 6f),
            new Color(0.95f, 0.75f, 0.18f),
            0.5f);
    }

    private static void BuildGrid(
        Transform parent)
    {
        for (int x = -14; x <= 14; x += 2)
        {
            CreateBlock(
                parent,
                "Grid_X_" + x,
                new Vector3(x, 0.52f, 0f),
                new Vector3(0.025f, 0.025f, 20f),
                new Color(0.15f, 0.22f, 0.28f, 0.32f));
        }

        for (int z = -10; z <= 10; z += 2)
        {
            CreateBlock(
                parent,
                "Grid_Z_" + z,
                new Vector3(0f, 0.53f, z),
                new Vector3(28f, 0.025f, 0.025f),
                new Color(0.15f, 0.22f, 0.28f, 0.32f));
        }
    }

    private static void CreateHeatCell(
        Transform parent,
        float x,
        float z,
        float size,
        float intensity)
    {
        Color color = HeatColor(intensity);

        CreateBlock(
            parent,
            "HeatCell_" + x + "_" + z,
            new Vector3(x, 0.08f, z),
            new Vector3(size, 0.30f, size),
            color);
    }

    private static Color HeatColor(float value)
    {
        value = Mathf.Clamp01(value);

        if (value < 0.25f)
        {
            return Color.Lerp(
                new Color(0.04f, 0.20f, 0.70f, 0.38f),
                new Color(0.10f, 0.75f, 0.75f, 0.48f),
                value / 0.25f);
        }

        if (value < 0.50f)
        {
            return Color.Lerp(
                new Color(0.10f, 0.75f, 0.75f, 0.48f),
                new Color(0.90f, 0.78f, 0.12f, 0.60f),
                (value - 0.25f) / 0.25f);
        }

        if (value < 0.75f)
        {
            return Color.Lerp(
                new Color(0.90f, 0.78f, 0.12f, 0.60f),
                new Color(0.95f, 0.35f, 0.08f, 0.72f),
                (value - 0.50f) / 0.25f);
        }

        return Color.Lerp(
            new Color(0.95f, 0.35f, 0.08f, 0.72f),
            new Color(0.95f, 0.06f, 0.05f, 0.90f),
            (value - 0.75f) / 0.25f);
    }

    private static void CreateHotspot(
        Transform parent,
        string name,
        Vector3 position,
        float radius,
        Color color)
    {
        GameObject hotspot =
            GameObject.CreatePrimitive(
                PrimitiveType.Cylinder);

        hotspot.name = name;

        hotspot.transform.SetParent(
            parent,
            false);

        hotspot.transform.position =
            position;

        hotspot.transform.localScale =
            new Vector3(
                radius,
                0.16f,
                radius);

        ApplyMaterial(
            hotspot,
            color);
    }

    private static void CreateMarker(
        Transform parent,
        string name,
        Vector3 position,
        Color color,
        float scale)
    {
        GameObject marker =
            GameObject.CreatePrimitive(
                PrimitiveType.Cylinder);

        marker.name = name;

        marker.transform.SetParent(
            parent,
            false);

        marker.transform.position =
            position;

        marker.transform.localScale =
            new Vector3(
                scale,
                0.40f,
                scale);

        ApplyMaterial(
            marker,
            color);
    }

    private static void BuildRoute(
        Transform parent,
        string name,
        Vector3[] points,
        Color color)
    {
        GameObject route =
            new GameObject(name);

        route.transform.SetParent(
            parent,
            false);

        LineRenderer line =
            route.AddComponent<LineRenderer>();

        line.positionCount =
            points.Length;

        line.useWorldSpace = true;
        line.widthMultiplier = 0.08f;

        for (int i = 0; i < points.Length; i++)
            line.SetPosition(i, points[i]);

        Material material =
            CreateMaterial(color);

        if (material != null)
            line.material = material;
    }

    private static void BuildCamera()
    {
        GameObject cameraObject =
            new GameObject(
                "StrategicHeatmapCamera");

        Camera camera =
            cameraObject.AddComponent<Camera>();

        camera.orthographic = true;
        camera.orthographicSize = 14f;
        camera.nearClipPlane = 0.1f;
        camera.farClipPlane = 100f;
        camera.backgroundColor =
            new Color(0.008f, 0.012f, 0.018f);

        cameraObject.transform.position =
            new Vector3(0f, 18f, 0f);

        cameraObject.transform.rotation =
            Quaternion.Euler(90f, 0f, 0f);
    }

    private static void BuildEventSystem()
    {
        GameObject eventSystem =
            new GameObject("EventSystem");

        eventSystem.AddComponent<EventSystem>();
        eventSystem.AddComponent<InputSystemUIInputModule>();
    }

    // =========================================================
    // HUD
    // =========================================================

    private static void BuildHUD()
    {
        GameObject canvasObject =
            new GameObject(
                "HUD-213 StrategicHeatmapHUD");

        Canvas canvas =
            canvasObject.AddComponent<Canvas>();

        canvas.renderMode =
            RenderMode.ScreenSpaceOverlay;

        canvasObject.AddComponent<GraphicRaycaster>();

        CanvasScaler scaler =
            canvasObject.AddComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(1920f, 1080f);

        scaler.screenMatchMode =
            CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        scaler.matchWidthOrHeight = 0.5f;

        BuildTopBar(canvasObject.transform);
        BuildNavigation(canvasObject.transform);
        BuildHeatmapLayers(canvasObject.transform);
        BuildLegend(canvasObject.transform);
        BuildHeatmapInfo(canvasObject.transform);
        BuildConflictDynamics(canvasObject.transform);
        BuildQuickActions(canvasObject.transform);
        BuildAlerts(canvasObject.transform);
        BuildBottomBar(canvasObject.transform);
    }

    private static void BuildTopBar(Transform parent)
    {
        GameObject panel =
            CreatePanel(
                parent,
                "HUD-213-01 TopBar",
                "STRATEGIC HEATMAP",
                new Vector2(0.01f, 0.91f),
                new Vector2(0.99f, 0.99f));

        CreateDisplay(
            panel.transform,
            "HUD-213-01A PlanetNameDisplay",
            "PLANET // OBSIDIAN WORLD");

        CreateDisplay(
            panel.transform,
            "HUD-213-01B ContinentNameDisplay",
            "CONTINENT // ASHEN CONTINENT");

        CreateDisplay(
            panel.transform,
            "HUD-213-01C RegionNameDisplay",
            "REGION // NORTHERN FRONT");

        CreateDisplay(
            panel.transform,
            "HUD-213-01D HeatmapModeDisplay",
            "HEATMAP // STRATEGIC THREAT");

        CreateEmblem(
            panel.transform,
            "HUD-213-01E OwnerFactionEmblem");

        CreateDisplay(
            panel.transform,
            "HUD-213-01F PlayerResourcesPanel",
            "RESOURCES 84,250  |  SUPPLY 72%");

        CreateDisplay(
            panel.transform,
            "HUD-213-01G TimeControlPanel",
            "TIME 14:32:08  |  PAUSED");
    }

    private static void BuildNavigation(Transform parent)
    {
        GameObject panel =
            CreatePanel(
                parent,
                "HUD-213-02 NavigationPanel",
                "NAVIGATION",
                new Vector2(0.01f, 0.20f),
                new Vector2(0.145f, 0.90f));

        string[] names =
        {
            "HUD-213-02A ReturnToFactionTerritoryOverlayButton",
            "HUD-213-02B ReturnToFactionControlMapButton",
            "HUD-213-02C ReturnToWarfareProfileButton",
            "HUD-213-02D ReturnToEnvironmentProfileButton",
            "HUD-213-02E ReturnToIslandMapButton",
            "HUD-213-02F ReturnToBiomeMapButton",
            "HUD-213-02G ReturnToRegionMapButton",
            "HUD-213-02H ReturnToContinentMapButton",
            "HUD-213-02I ReturnToSurfaceMapButton",
            "HUD-213-02J ReturnToPlanetOverviewButton"
        };

        string[] labels =
        {
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

        BuildButtonList(
            panel.transform,
            names,
            labels);
    }

    private static void BuildHeatmapLayers(Transform parent)
    {
        GameObject panel =
            CreatePanel(
                parent,
                "HUD-213-03 HeatmapLayersPanel",
                "HEATMAP LAYERS",
                new Vector2(0.155f, 0.20f),
                new Vector2(0.30f, 0.90f));

        string[] names =
        {
            "HUD-213-03A LayerToggle_ConflictIntensity",
            "HUD-213-03B LayerToggle_FactionInfluence",
            "HUD-213-03C LayerToggle_ThreatZones",
            "HUD-213-03D LayerToggle_LogisticsPressure",
            "HUD-213-03E LayerToggle_ReinforcementDensity",
            "HUD-213-03F LayerToggle_ResistanceActivity",
            "HUD-213-03G LayerToggle_StabilityIndex",
            "HUD-213-03H LayerToggle_SupplyLineIntegrity",
            "HUD-213-03I LayerToggle_WeatherImpact",
            "HUD-213-03J LayerToggle_TerrainModifiers",
            "HUD-213-03K LayerToggle_WarfareZones",
            "HUD-213-03L LayerToggle_FactionControl"
        };

        string[] labels =
        {
            "CONFLICT INTENSITY",
            "FACTION INFLUENCE",
            "THREAT ZONES",
            "LOGISTICS PRESSURE",
            "REINFORCEMENT DENSITY",
            "RESISTANCE ACTIVITY",
            "STABILITY INDEX",
            "SUPPLY LINE INTEGRITY",
            "WEATHER IMPACT",
            "TERRAIN MODIFIERS",
            "WARFARE ZONES",
            "FACTION CONTROL"
        };

        BuildToggleList(
            panel.transform,
            names,
            labels);
    }

    private static void BuildLegend(Transform parent)
    {
        GameObject panel =
            CreatePanel(
                parent,
                "HUD-213-04 HeatmapLegendPanel",
                "HEATMAP LEGEND",
                new Vector2(0.31f, 0.57f),
                new Vector2(0.54f, 0.90f));

        CreateDisplay(
            panel.transform,
            "HUD-213-04A ConflictIntensityLegend",
            "CONFLICT INTENSITY // LOW  →  EXTREME");

        CreateDisplay(
            panel.transform,
            "HUD-213-04B ThreatLevelLegend",
            "THREAT LEVEL // SAFE  →  CRITICAL");

        CreateDisplay(
            panel.transform,
            "HUD-213-04C LogisticsPressureLegend",
            "LOGISTICS PRESSURE // LOW  →  SEVERE");

        CreateDisplay(
            panel.transform,
            "HUD-213-04D ResistanceActivityLegend",
            "RESISTANCE ACTIVITY // LOW  →  ACTIVE");

        CreateDisplay(
            panel.transform,
            "HUD-213-04E StabilityIndexLegend",
            "STABILITY INDEX // STABLE  →  COLLAPSE");

        CreateDisplay(
            panel.transform,
            "HUD-213-04F SupplyLineIntegrityLegend",
            "SUPPLY INTEGRITY // 100%  →  BROKEN");
    }

    private static void BuildHeatmapInfo(Transform parent)
    {
        GameObject panel =
            CreatePanel(
                parent,
                "HUD-213-05 HeatmapInfoPanel",
                "SELECTED HEAT ZONE",
                new Vector2(0.55f, 0.57f),
                new Vector2(0.77f, 0.90f));

        CreateDisplay(
            panel.transform,
            "HUD-213-05A SelectedZoneName",
            "NORTHERN FRONT // ZONE 07");

        CreateDisplay(
            panel.transform,
            "HUD-213-05B SelectedZoneType",
            "ZONE TYPE // HIGH CONFLICT");

        CreateDisplay(
            panel.transform,
            "HUD-213-05C SelectedZoneConflictIntensity",
            "CONFLICT // 86%");

        CreateDisplay(
            panel.transform,
            "HUD-213-05D SelectedZoneThreatLevel",
            "THREAT // CRITICAL");

        CreateDisplay(
            panel.transform,
            "HUD-213-05E SelectedZoneLogisticsPressure",
            "LOGISTICS PRESSURE // 79%");

        CreateDisplay(
            panel.transform,
            "HUD-213-05F SelectedZoneResistanceActivity",
            "RESISTANCE // HIGH");

        CreateDisplay(
            panel.transform,
            "HUD-213-05G SelectedZoneStabilityIndex",
            "STABILITY // 43 / 100");

        CreateDisplay(
            panel.transform,
            "HUD-213-05H SelectedZoneSupplyLineIntegrity",
            "SUPPLY LINE // 58%");

        CreateButton(
            panel.transform,
            "HUD-213-05I EnterZoneButton",
            "ENTER SELECTED ZONE");
    }

    private static void BuildConflictDynamics(Transform parent)
    {
        GameObject panel =
            CreatePanel(
                parent,
                "HUD-213-06 ConflictDynamicsPanel",
                "CONFLICT DYNAMICS",
                new Vector2(0.78f, 0.57f),
                new Vector2(0.99f, 0.90f));

        CreateDisplay(
            panel.transform,
            "HUD-213-06A FrontlineShiftDisplay",
            "FRONTLINE SHIFT // +1.8 KM");

        CreateDisplay(
            panel.transform,
            "HUD-213-06B EncirclementRiskDisplay",
            "ENCIRCLEMENT RISK // 42%");

        CreateDisplay(
            panel.transform,
            "HUD-213-06C BreakthroughPotentialDisplay",
            "BREAKTHROUGH POTENTIAL // 67%");

        CreateDisplay(
            panel.transform,
            "HUD-213-06D AttritionRateDisplay",
            "ATTRITION // 3.8% / DAY");

        CreateDisplay(
            panel.transform,
            "HUD-213-06E FactionMoraleDisplay",
            "FACTION MORALE // 72 / 100");
    }

    private static void BuildQuickActions(Transform parent)
    {
        GameObject panel =
            CreatePanel(
                parent,
                "HUD-213-07 QuickActionsPanel",
                "QUICK ACTIONS",
                new Vector2(0.31f, 0.20f),
                new Vector2(0.54f, 0.54f));

        string[] names =
        {
            "HUD-213-07A DeployReconButton",
            "HUD-213-07B DeployCombatGroupButton",
            "HUD-213-07C DeployEW_TeamButton",
            "HUD-213-07D OpenIntelNetworkButton",
            "HUD-213-07E OpenDoctrineManagerButton",
            "HUD-213-07F OpenLogisticsOverviewButton",
            "HUD-213-07G OpenFactionDiplomacyButton"
        };

        string[] labels =
        {
            "DEPLOY RECON",
            "DEPLOY COMBAT GROUP",
            "DEPLOY EW TEAM",
            "INTEL NETWORK",
            "DOCTRINE MANAGER",
            "LOGISTICS OVERVIEW",
            "FACTION DIPLOMACY"
        };

        BuildButtonList(
            panel.transform,
            names,
            labels);
    }

    private static void BuildAlerts(Transform parent)
    {
        GameObject panel =
            CreatePanel(
                parent,
                "HUD-213-08 AlertsPanel",
                "STRATEGIC HEATMAP ALERTS",
                new Vector2(0.55f, 0.20f),
                new Vector2(0.99f, 0.54f));

        CreateDisplay(
            panel.transform,
            "HUD-213-08A StrategicAlertEntry",
            "STRATEGIC // Critical heat concentration detected");

        CreateDisplay(
            panel.transform,
            "HUD-213-08B WarfareAlertEntry",
            "WARFARE // Enemy concentration increasing");

        CreateDisplay(
            panel.transform,
            "HUD-213-08C FactionAlertEntry",
            "FACTION // Influence boundary changing");

        CreateDisplay(
            panel.transform,
            "HUD-213-08D ResourceAlertEntry",
            "RESOURCE // Strategic node under pressure");

        CreateDisplay(
            panel.transform,
            "HUD-213-08E WeatherAlertEntry",
            "WEATHER // Visibility degradation");

        CreateDisplay(
            panel.transform,
            "HUD-213-08F EnvironmentalHazardAlertEntry",
            "ENVIRONMENT // Hazard intensity rising");

        CreateDisplay(
            panel.transform,
            "HUD-213-08G TerritoryShiftAlertEntry",
            "TERRITORY // Control zone shifting");

        CreateDisplay(
            panel.transform,
            "HUD-213-08H ConflictEscalationAlertEntry",
            "CONFLICT // Escalation threshold exceeded");

        CreateDisplay(
            panel.transform,
            "HUD-213-08I SupplyLineBreakAlertEntry",
            "SUPPLY // Line integrity compromised");
    }

    private static void BuildBottomBar(Transform parent)
    {
        GameObject panel =
            CreatePanel(
                parent,
                "HUD-213-09 BottomBar",
                "",
                new Vector2(0.01f, 0.02f),
                new Vector2(0.99f, 0.105f));

        CreateButton(
            panel.transform,
            "HUD-213-09A BackButton",
            "BACK");

        CreateButton(
            panel.transform,
            "HUD-213-09B ExitToCommandCenterButton",
            "COMMAND CENTER");

        CreateButton(
            panel.transform,
            "HUD-213-09C SettingsButton",
            "SETTINGS");

        CreateButton(
            panel.transform,
            "HUD-213-09D HelpButton",
            "HELP");
    }

    // =========================================================
    // UI HELPERS
    // =========================================================

    private static GameObject CreatePanel(
        Transform parent,
        string name,
        string title,
        Vector2 anchorMin,
        Vector2 anchorMax)
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
        rect.offsetMin = new Vector2(4f, 4f);
        rect.offsetMax = new Vector2(-4f, -4f);

        panel.GetComponent<Image>().color =
            Panel;

        if (!string.IsNullOrEmpty(title))
        {
            GameObject header =
                new GameObject(
                    name + "_HEADER",
                    typeof(RectTransform),
                    typeof(Image));

            header.transform.SetParent(
                panel.transform,
                false);

            RectTransform headerRect =
                header.GetComponent<RectTransform>();

            headerRect.anchorMin =
                new Vector2(0f, 0.88f);

            headerRect.anchorMax =
                new Vector2(1f, 1f);

            headerRect.offsetMin =
                new Vector2(2f, 0f);

            headerRect.offsetMax =
                new Vector2(-2f, -2f);

            header.GetComponent<Image>().color =
                Header;

            CreateText(
                header.transform,
                "TITLE",
                title,
                17f,
                true,
                TextAlignmentOptions.Center);
        }

        return panel;
    }

    private static GameObject CreateDisplay(
        Transform parent,
        string name,
        string value)
    {
        GameObject objectRoot =
            new GameObject(
                name,
                typeof(RectTransform));

        objectRoot.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            objectRoot.GetComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0.03f, 0f);

        rect.anchorMax =
            new Vector2(0.97f, 0f);

        rect.sizeDelta =
            new Vector2(0f, 29f);

        int index =
            objectRoot.transform.GetSiblingIndex();

        rect.anchoredPosition =
            new Vector2(
                0f,
                -38f - index * 31f);

        CreateText(
            objectRoot.transform,
            "VALUE",
            value,
            13f,
            false,
            TextAlignmentOptions.Left);

        return objectRoot;
    }

    private static GameObject CreateEmblem(
        Transform parent,
        string name)
    {
        GameObject emblem =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image));

        emblem.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            emblem.GetComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0f, 0.5f);

        rect.anchorMax =
            new Vector2(0f, 0.5f);

        rect.pivot =
            new Vector2(0f, 0.5f);

        rect.sizeDelta =
            new Vector2(42f, 42f);

        rect.anchoredPosition =
            new Vector2(8f, 0f);

        emblem.GetComponent<Image>().color =
            Accent;

        return emblem;
    }

    private static Button CreateButton(
        Transform parent,
        string name,
        string label)
    {
        GameObject objectRoot =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image),
                typeof(Button));

        objectRoot.transform.SetParent(
            parent,
            false);

        Button button =
            objectRoot.GetComponent<Button>();

        objectRoot.GetComponent<Image>().color =
            Button;

        RectTransform rect =
            objectRoot.GetComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0.05f, 0f);

        rect.anchorMax =
            new Vector2(0.95f, 0f);

        rect.sizeDelta =
            new Vector2(0f, 32f);

        int index =
            objectRoot.transform.GetSiblingIndex();

        rect.anchoredPosition =
            new Vector2(
                0f,
                -40f - index * 36f);

        CreateText(
            objectRoot.transform,
            "LABEL",
            label,
            12f,
            true,
            TextAlignmentOptions.Center);

        return button;
    }

    private static Toggle CreateToggle(
        Transform parent,
        string name,
        string label)
    {
        GameObject objectRoot =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image),
                typeof(Toggle));

        objectRoot.transform.SetParent(
            parent,
            false);

        Toggle toggle =
            objectRoot.GetComponent<Toggle>();

        toggle.isOn = true;

        objectRoot.GetComponent<Image>().color =
            Button;

        RectTransform rect =
            objectRoot.GetComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0.05f, 0f);

        rect.anchorMax =
            new Vector2(0.95f, 0f);

        rect.sizeDelta =
            new Vector2(0f, 28f);

        int index =
            objectRoot.transform.GetSiblingIndex();

        rect.anchoredPosition =
            new Vector2(
                0f,
                -40f - index * 30f);

        CreateText(
            objectRoot.transform,
            "LABEL",
            "●  " + label,
            11f,
            false,
            TextAlignmentOptions.Left);

        return toggle;
    }

    private static void BuildButtonList(
        Transform parent,
        string[] names,
        string[] labels)
    {
        for (int i = 0; i < names.Length; i++)
            CreateButton(
                parent,
                names[i],
                labels[i]);
    }

    private static void BuildToggleList(
        Transform parent,
        string[] names,
        string[] labels)
    {
        for (int i = 0; i < names.Length; i++)
            CreateToggle(
                parent,
                names[i],
                labels[i]);
    }

    private static TMP_Text CreateText(
        Transform parent,
        string name,
        string value,
        float fontSize,
        bool bold,
        TextAlignmentOptions alignment)
    {
        GameObject objectRoot =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(TextMeshProUGUI));

        objectRoot.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            objectRoot.GetComponent<RectTransform>();

        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = new Vector2(7f, 2f);
        rect.offsetMax = new Vector2(-7f, -2f);

        TextMeshProUGUI text =
            objectRoot.GetComponent<TextMeshProUGUI>();

        text.text = value;
        text.fontSize = fontSize;
        text.alignment = alignment;
        text.color = Color.white;
        text.enableAutoSizing = false;
        text.textWrappingMode =
            TextWrappingModes.Normal;

        if (bold)
            text.fontStyle = FontStyles.Bold;

        return text;
    }

    // =========================================================
    // WORLD HELPERS
    // =========================================================

    private static void CreateBlock(
        Transform parent,
        string name,
        Vector3 position,
        Vector3 scale,
        Color color)
    {
        GameObject objectRoot =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube);

        objectRoot.name = name;

        objectRoot.transform.SetParent(
            parent,
            false);

        objectRoot.transform.position =
            position;

        objectRoot.transform.localScale =
            scale;

        ApplyMaterial(
            objectRoot,
            color);
    }

    private static void ApplyMaterial(
        GameObject objectRoot,
        Color color)
    {
        Renderer renderer =
            objectRoot.GetComponent<Renderer>();

        if (renderer == null)
            return;

        Material material =
            CreateMaterial(color);

        if (material != null)
            renderer.sharedMaterial = material;
    }

    private static Material CreateMaterial(
        Color color)
    {
        Shader shader =
            Shader.Find(
                "Universal Render Pipeline/Lit");

        if (shader == null)
            shader = Shader.Find("Standard");

        if (shader == null)
            return null;

        Material material =
            new Material(shader);

        material.color = color;

        return material;
    }

    // =========================================================
    // PROJECT HELPERS
    // =========================================================

    private static void EnsureFolder(
        string path)
    {
        if (AssetDatabase.IsValidFolder(path))
            return;

        string parent =
            System.IO.Path.GetDirectoryName(path)
                .Replace("\\", "/");

        string folder =
            System.IO.Path.GetFileName(path);

        if (!AssetDatabase.IsValidFolder(parent))
            EnsureFolder(parent);

        AssetDatabase.CreateFolder(
            parent,
            folder);
    }

    private static void AddToBuildSettings(
        string scenePath)
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
            new EditorBuildSettingsScene(
                scenePath,
                true));

        EditorBuildSettings.scenes =
            scenes.ToArray();
    }
}
