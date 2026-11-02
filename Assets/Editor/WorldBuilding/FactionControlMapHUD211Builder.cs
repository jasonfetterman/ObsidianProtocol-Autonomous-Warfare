using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public static class FactionControlMapHUD211Builder
{
    private const string ScenePath =
        "Assets/Scenes/SCN70_FACTION_CONTROL/Faction_Control.unity";

    private static readonly Color PanelColor =
        new Color(0.025f, 0.035f, 0.05f, 0.92f);

    private static readonly Color PanelHeaderColor =
        new Color(0.055f, 0.075f, 0.10f, 0.96f);

    private static readonly Color ButtonColor =
        new Color(0.08f, 0.11f, 0.15f, 0.98f);

    private static readonly Color AccentColor =
        new Color(0.20f, 0.65f, 0.95f, 1f);

    private static readonly Color FactionA =
        new Color(0.15f, 0.65f, 0.95f, 0.35f);

    private static readonly Color FactionB =
        new Color(0.85f, 0.30f, 0.25f, 0.35f);

    private static readonly Color FactionC =
        new Color(0.75f, 0.65f, 0.20f, 0.35f);

    [MenuItem("Obsidian Protocol/Worlds/BUILD HUD-211 FACTION CONTROL")]
    public static void Build()
    {
        EnsureFolder("Assets/Scenes");
        EnsureFolder("Assets/Scenes/SCN70_FACTION_CONTROL");

        EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene,
            NewSceneMode.Single);

        BuildFactionEnvironment();
        BuildFactionCamera();
        BuildFactionEventSystem();
        BuildFactionHUD();

        Scene scene = SceneManager.GetActiveScene();
        EditorSceneManager.SaveScene(scene, ScenePath);

        AddToBuildSettings(ScenePath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "[HUD-211] Faction Control Map built successfully: " +
            ScenePath);
    }

    // =========================================================
    // ENVIRONMENT
    // =========================================================

    private static void BuildFactionEnvironment()
    {
        GameObject root = new GameObject("FACTION CONTROL ENVIRONMENT");

        CreateEnvironmentBlock(
            root.transform,
            "FactionControlTerrain",
            new Vector3(0f, -0.6f, 0f),
            new Vector3(26f, 1f, 20f),
            new Color(0.075f, 0.095f, 0.11f));

        CreateEnvironmentBlock(
            root.transform,
            "NorthernTerritory",
            new Vector3(-7f, 0.15f, 5f),
            new Vector3(10f, 0.45f, 6f),
            FactionA);

        CreateEnvironmentBlock(
            root.transform,
            "SouthernTerritory",
            new Vector3(7f, 0.18f, -5f),
            new Vector3(9f, 0.5f, 6f),
            FactionB);

        CreateEnvironmentBlock(
            root.transform,
            "EasternTerritory",
            new Vector3(8f, 0.2f, 5f),
            new Vector3(5f, 0.5f, 5f),
            FactionC);

        CreateEnvironmentBlock(
            root.transform,
            "CentralContestedZone",
            new Vector3(0f, 0.25f, 0f),
            new Vector3(7f, 0.35f, 6f),
            new Color(0.45f, 0.25f, 0.18f, 0.5f));

        CreateEnvironmentBlock(
            root.transform,
            "WesternNeutralZone",
            new Vector3(-9f, 0.15f, -5f),
            new Vector3(5f, 0.35f, 5f),
            new Color(0.22f, 0.25f, 0.28f, 0.6f));

        BuildFactionBorder(
            root.transform,
            "NorthernBorder",
            new Vector3(-12f, 0.45f, 2f),
            new Vector3(2f, 0.2f, 15f),
            AccentColor);

        BuildFactionBorder(
            root.transform,
            "SouthernBorder",
            new Vector3(2f, 0.5f, -2f),
            new Vector3(15f, 0.2f, 2f),
            new Color(0.9f, 0.35f, 0.25f));

        BuildFactionMarker(
            root.transform,
            "FactionCapital",
            new Vector3(-7f, 0.7f, 5f),
            AccentColor);

        BuildFactionMarker(
            root.transform,
            "ForwardOperatingBase",
            new Vector3(5f, 0.7f, -3f),
            new Color(0.9f, 0.35f, 0.25f));

        BuildFactionMarker(
            root.transform,
            "ResourceNode",
            new Vector3(8f, 0.7f, 5f),
            new Color(0.9f, 0.75f, 0.25f));

        BuildFactionRoute(
            root.transform,
            "SupplyLineRoute",
            new Vector3[]
            {
                new Vector3(-7f, 0.7f, 5f),
                new Vector3(-3f, 0.7f, 3f),
                new Vector3(1f, 0.7f, 0f),
                new Vector3(5f, 0.7f, -3f)
            });

        BuildFactionRoute(
            root.transform,
            "ReinforcementRoute",
            new Vector3[]
            {
                new Vector3(8f, 0.72f, 5f),
                new Vector3(5f, 0.72f, 3f),
                new Vector3(2f, 0.72f, 0f),
                new Vector3(5f, 0.72f, -3f)
            });

        BuildInfluenceZone(
            root.transform,
            "FactionInfluenceZone_A",
            new Vector3(-7f, 0.8f, 5f),
            5f,
            AccentColor);

        BuildInfluenceZone(
            root.transform,
            "FactionInfluenceZone_B",
            new Vector3(7f, 0.82f, -5f),
            4.5f,
            new Color(0.9f, 0.25f, 0.2f));

        BuildThreatZone(
            root.transform,
            "ThreatZone",
            new Vector3(0f, 0.85f, 0f),
            3.2f);

        BuildSensorNetwork(
            root.transform,
            "SensorNetwork",
            new Vector3[]
            {
                new Vector3(-8f, 1f, 6f),
                new Vector3(-2f, 1f, 7f),
                new Vector3(5f, 1f, 5f),
                new Vector3(8f, 1f, -2f),
                new Vector3(2f, 1f, -6f)
            });
    }

    private static void BuildFactionCamera()
    {
        GameObject cameraObject = new GameObject("FactionControlCamera");

        Camera camera = cameraObject.AddComponent<Camera>();
        camera.orthographic = true;
        camera.orthographicSize = 14f;
        camera.nearClipPlane = 0.1f;
        camera.farClipPlane = 100f;
        camera.backgroundColor = new Color(0.008f, 0.012f, 0.018f);

        cameraObject.transform.position =
            new Vector3(0f, 18f, 0f);

        cameraObject.transform.rotation =
            Quaternion.Euler(90f, 0f, 0f);
    }

    private static void BuildFactionEventSystem()
    {
        GameObject eventSystem =
            new GameObject("EventSystem");

        eventSystem.AddComponent<EventSystem>();
        eventSystem.AddComponent<InputSystemUIInputModule>();
    }

    // =========================================================
    // HUD
    // =========================================================

    private static void BuildFactionHUD()
    {
        GameObject canvasObject =
            new GameObject("HUD-211 FactionControlMapHUD");

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

        BuildFactionTopBar(canvasObject.transform);
        BuildFactionNavigation(canvasObject.transform);
        BuildFactionLayers(canvasObject.transform);
        BuildFactionInfo(canvasObject.transform);
        BuildFactionTerritoryStats(canvasObject.transform);
        BuildFactionWarfare(canvasObject.transform);
        BuildFactionQuickActions(canvasObject.transform);
        BuildFactionAlerts(canvasObject.transform);
        BuildFactionBottomBar(canvasObject.transform);
    }

    // =========================================================
    // TOP BAR
    // =========================================================

    private static void BuildFactionTopBar(Transform parent)
    {
        GameObject panel = CreatePanel(
            parent,
            "HUD-211-01 TopBar",
            "FACTION CONTROL",
            new Vector2(0f, 0.91f),
            new Vector2(1f, 1f));

        CreateDisplay(panel.transform,
            "HUD-211-01A PlanetNameDisplay",
            "PLANET // OBSIDIAN WORLD");

        CreateDisplay(panel.transform,
            "HUD-211-01B ContinentNameDisplay",
            "CONTINENT // ASHEN CONTINENT");

        CreateDisplay(panel.transform,
            "HUD-211-01C RegionNameDisplay",
            "REGION // NORTHERN FRONT");

        CreateDisplay(panel.transform,
            "HUD-211-01D FactionControlLayerDisplay",
            "LAYER // FACTION CONTROL");

        CreateEmblem(
            panel.transform,
            "HUD-211-01E PlayerFactionEmblem");

        CreateDisplay(
            panel.transform,
            "HUD-211-01F PlayerResourcesPanel",
            "RESOURCES  84,250  |  SUPPLY  72%");

        CreateDisplay(
            panel.transform,
            "HUD-211-01G TimeControlPanel",
            "TIME  14:32:08  |  PAUSED");
    }

    // =========================================================
    // NAVIGATION
    // =========================================================

    private static void BuildFactionNavigation(Transform parent)
    {
        GameObject panel = CreatePanel(
            parent,
            "HUD-211-02 NavigationPanel",
            "NAVIGATION",
            new Vector2(0.01f, 0.19f),
            new Vector2(0.145f, 0.90f));

        string[] buttons =
        {
            "HUD-211-02A ReturnToWarfareProfileButton",
            "HUD-211-02B ReturnToEnvironmentProfileButton",
            "HUD-211-02C ReturnToIslandMapButton",
            "HUD-211-02D ReturnToBiomeMapButton",
            "HUD-211-02E ReturnToRegionMapButton",
            "HUD-211-02F ReturnToContinentMapButton",
            "HUD-211-02G ReturnToSurfaceMapButton",
            "HUD-211-02H ReturnToPlanetOverviewButton"
        };

        string[] labels =
        {
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
            buttons,
            labels);
    }

    // =========================================================
    // FACTION LAYERS
    // =========================================================

    private static void BuildFactionLayers(Transform parent)
    {
        GameObject panel = CreatePanel(
            parent,
            "HUD-211-03 FactionLayersPanel",
            "FACTION LAYERS",
            new Vector2(0.155f, 0.19f),
            new Vector2(0.30f, 0.90f));

        string[] names =
        {
            "HUD-211-03A LayerToggle_FactionControl",
            "HUD-211-03B LayerToggle_FactionBorders",
            "HUD-211-03C LayerToggle_FactionInfluence",
            "HUD-211-03D LayerToggle_WarfareZones",
            "HUD-211-03E LayerToggle_ThreatZones",
            "HUD-211-03F LayerToggle_LogisticsRoutes",
            "HUD-211-03G LayerToggle_ResourceNodes",
            "HUD-211-03H LayerToggle_ReconCoverage",
            "HUD-211-03I LayerToggle_SensorNetworks",
            "HUD-211-03J LayerToggle_WeatherImpact",
            "HUD-211-03K LayerToggle_TerrainModifiers"
        };

        string[] labels =
        {
            "FACTION CONTROL",
            "FACTION BORDERS",
            "FACTION INFLUENCE",
            "WARFARE ZONES",
            "THREAT ZONES",
            "LOGISTICS ROUTES",
            "RESOURCE NODES",
            "RECON COVERAGE",
            "SENSOR NETWORKS",
            "WEATHER IMPACT",
            "TERRAIN MODIFIERS"
        };

        BuildToggleList(
            panel.transform,
            names,
            labels);
    }

    // =========================================================
    // FACTION INFO
    // =========================================================

    private static void BuildFactionInfo(Transform parent)
    {
        GameObject panel = CreatePanel(
            parent,
            "HUD-211-04 FactionInfoPanel",
            "SELECTED FACTION",
            new Vector2(0.31f, 0.57f),
            new Vector2(0.54f, 0.90f));

        CreateDisplay(panel.transform,
            "HUD-211-04A SelectedFactionName",
            "IRON COVENANT");

        CreateEmblem(panel.transform,
            "HUD-211-04B SelectedFactionEmblem");

        CreateDisplay(panel.transform,
            "HUD-211-04C SelectedFactionTerritorySize",
            "TERRITORY SIZE // 31.4%");

        CreateDisplay(panel.transform,
            "HUD-211-04D SelectedFactionStrengthRating",
            "STRENGTH // 82 / 100");

        CreateDisplay(panel.transform,
            "HUD-211-04E SelectedFactionThreatLevel",
            "THREAT LEVEL // HIGH");

        CreateDisplay(panel.transform,
            "HUD-211-04F SelectedFactionDoctrineType",
            "DOCTRINE // EXPANSIONIST");

        CreateDisplay(panel.transform,
            "HUD-211-04G SelectedFactionLogisticsCapacity",
            "LOGISTICS CAPACITY // 74%");

        CreateDisplay(panel.transform,
            "HUD-211-04H SelectedFactionWarfareFocus",
            "WARFARE FOCUS // ARMORED ASSAULT");

        CreateButton(
            panel.transform,
            "HUD-211-04I EnterFactionTerritoryButton",
            "ENTER FACTION TERRITORY");
    }

    // =========================================================
    // TERRITORY STATS
    // =========================================================

    private static void BuildFactionTerritoryStats(Transform parent)
    {
        GameObject panel = CreatePanel(
            parent,
            "HUD-211-05 TerritoryStatsPanel",
            "TERRITORY STATUS",
            new Vector2(0.55f, 0.57f),
            new Vector2(0.77f, 0.90f));

        CreateDisplay(panel.transform,
            "HUD-211-05A ControlPercentageDisplay",
            "CONTROL // 68%");

        CreateDisplay(panel.transform,
            "HUD-211-05B ConflictIntensityDisplay",
            "CONFLICT INTENSITY // 74%");

        CreateDisplay(panel.transform,
            "HUD-211-05C OccupationStatusDisplay",
            "OCCUPATION // CONTESTED");

        CreateDisplay(panel.transform,
            "HUD-211-05D ResistanceActivityDisplay",
            "RESISTANCE // MODERATE");

        CreateDisplay(panel.transform,
            "HUD-211-05E StabilityIndexDisplay",
            "STABILITY // 61 / 100");

        CreateDisplay(panel.transform,
            "HUD-211-05F SupplyLineIntegrityDisplay",
            "SUPPLY LINE // 83%");

        CreateDisplay(panel.transform,
            "HUD-211-05G ReinforcementRoutesDisplay",
            "REINFORCEMENT ROUTES // 6 ACTIVE");
    }

    // =========================================================
    // WARFARE
    // =========================================================

    private static void BuildFactionWarfare(Transform parent)
    {
        GameObject panel = CreatePanel(
            parent,
            "HUD-211-06 WarfarePanel",
            "WARFARE PROFILE",
            new Vector2(0.78f, 0.57f),
            new Vector2(0.99f, 0.90f));

        CreateDisplay(panel.transform,
            "HUD-211-06A WarfareFocusDisplay",
            "FOCUS // TERRITORIAL CONTROL");

        CreateDisplay(panel.transform,
            "HUD-211-06B EngagementRangeDisplay",
            "ENGAGEMENT RANGE // 4.8 KM");

        CreateDisplay(panel.transform,
            "HUD-211-06C MobilityRatingDisplay",
            "MOBILITY // 71 / 100");

        CreateDisplay(panel.transform,
            "HUD-211-06D ReconVisibilityDisplay",
            "RECON VISIBILITY // 63%");

        CreateDisplay(panel.transform,
            "HUD-211-06E SensorInterferenceDisplay",
            "SENSOR INTERFERENCE // 28%");

        CreateDisplay(panel.transform,
            "HUD-211-06F AmbushPotentialDisplay",
            "AMBUSH POTENTIAL // HIGH");

        CreateDisplay(panel.transform,
            "HUD-211-06G DefensivePositionRatingDisplay",
            "DEFENSE // 79 / 100");
    }

    // =========================================================
    // QUICK ACTIONS
    // =========================================================

    private static void BuildFactionQuickActions(Transform parent)
    {
        GameObject panel = CreatePanel(
            parent,
            "HUD-211-07 QuickActionsPanel",
            "QUICK ACTIONS",
            new Vector2(0.31f, 0.19f),
            new Vector2(0.54f, 0.54f));

        string[] names =
        {
            "HUD-211-07A DeployReconButton",
            "HUD-211-07B DeployCombatGroupButton",
            "HUD-211-07C DeployEW_TeamButton",
            "HUD-211-07D OpenIntelNetworkButton",
            "HUD-211-07E OpenDoctrineManagerButton",
            "HUD-211-07F OpenLogisticsOverviewButton",
            "HUD-211-07G OpenFactionDiplomacyButton"
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

    // =========================================================
    // ALERTS
    // =========================================================

    private static void BuildFactionAlerts(Transform parent)
    {
        GameObject panel = CreatePanel(
            parent,
            "HUD-211-08 AlertsPanel",
            "FACTION CONTROL ALERTS",
            new Vector2(0.55f, 0.19f),
            new Vector2(0.99f, 0.54f));

        CreateDisplay(panel.transform,
            "HUD-211-08A StrategicAlertEntry",
            "STRATEGIC // Territory expansion detected");

        CreateDisplay(panel.transform,
            "HUD-211-08B WarfareAlertEntry",
            "WARFARE // Enemy force concentration detected");

        CreateDisplay(panel.transform,
            "HUD-211-08C FactionAlertEntry",
            "FACTION // Diplomatic status changed");

        CreateDisplay(panel.transform,
            "HUD-211-08D ResourceAlertEntry",
            "RESOURCE // Strategic node contested");

        CreateDisplay(panel.transform,
            "HUD-211-08E WeatherAlertEntry",
            "WEATHER // Visibility degradation");

        CreateDisplay(panel.transform,
            "HUD-211-08F EnvironmentalHazardAlertEntry",
            "ENVIRONMENT // Hazard zone expanding");

        CreateDisplay(panel.transform,
            "HUD-211-08G TerritoryShiftAlertEntry",
            "TERRITORY // Control boundary shifting");

        CreateDisplay(panel.transform,
            "HUD-211-08H ConflictEscalationAlertEntry",
            "CONFLICT // Escalation detected");
    }

    // =========================================================
    // BOTTOM BAR
    // =========================================================

    private static void BuildFactionBottomBar(Transform parent)
    {
        GameObject panel = CreatePanel(
            parent,
            "HUD-211-09 BottomBar",
            "",
            new Vector2(0.01f, 0.02f),
            new Vector2(0.99f, 0.105f));

        CreateButton(
            panel.transform,
            "HUD-211-09A BackButton",
            "BACK");

        CreateButton(
            panel.transform,
            "HUD-211-09B ExitToCommandCenterButton",
            "COMMAND CENTER");

        CreateButton(
            panel.transform,
            "HUD-211-09C SettingsButton",
            "SETTINGS");

        CreateButton(
            panel.transform,
            "HUD-211-09D HelpButton",
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

        panel.transform.SetParent(parent, false);

        RectTransform rect =
            panel.GetComponent<RectTransform>();

        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = new Vector2(4f, 4f);
        rect.offsetMax = new Vector2(-4f, -4f);

        Image image =
            panel.GetComponent<Image>();

        image.color = PanelColor;

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
                PanelHeaderColor;

            CreateText(
                header.transform,
                "TITLE",
                title,
                18,
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
        GameObject display =
            new GameObject(
                name,
                typeof(RectTransform));

        display.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            display.GetComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0.03f, 0f);

        rect.anchorMax =
            new Vector2(0.97f, 0f);

        rect.sizeDelta =
            new Vector2(0f, 31f);

        rect.anchoredPosition =
            new Vector2(0f, -30f - ParentChildIndex(display.transform) * 34f);

        CreateText(
            display.transform,
            "VALUE",
            value,
            14,
            false,
            TextAlignmentOptions.Left);

        return display;
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
            new Vector2(0.02f, 0f);

        rect.anchorMax =
            new Vector2(0.02f, 1f);

        rect.pivot =
            new Vector2(0f, 0.5f);

        rect.sizeDelta =
            new Vector2(42f, 42f);

        rect.anchoredPosition =
            new Vector2(8f, 0f);

        emblem.GetComponent<Image>().color =
            AccentColor;

        return emblem;
    }

    private static Button CreateButton(
        Transform parent,
        string name,
        string label)
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

        Button button =
            buttonObject.GetComponent<Button>();

        Image image =
            buttonObject.GetComponent<Image>();

        image.color = ButtonColor;

        RectTransform rect =
            buttonObject.GetComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0.05f, 0f);

        rect.anchorMax =
            new Vector2(0.95f, 0f);

        rect.sizeDelta =
            new Vector2(0f, 34f);

        int index =
            ParentChildIndex(buttonObject.transform);

        rect.anchoredPosition =
            new Vector2(0f, -42f - index * 38f);

        CreateText(
            buttonObject.transform,
            "LABEL",
            label,
            13,
            true,
            TextAlignmentOptions.Center);

        return button;
    }

    private static Toggle CreateToggle(
        Transform parent,
        string name,
        string label)
    {
        GameObject toggleObject =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image),
                typeof(Toggle));

        toggleObject.transform.SetParent(
            parent,
            false);

        Toggle toggle =
            toggleObject.GetComponent<Toggle>();

        toggle.isOn = true;

        Image image =
            toggleObject.GetComponent<Image>();

        image.color = ButtonColor;

        RectTransform rect =
            toggleObject.GetComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0.05f, 0f);

        rect.anchorMax =
            new Vector2(0.95f, 0f);

        rect.sizeDelta =
            new Vector2(0f, 31f);

        int index =
            ParentChildIndex(toggleObject.transform);

        rect.anchoredPosition =
            new Vector2(0f, -40f - index * 34f);

        CreateText(
            toggleObject.transform,
            "LABEL",
            "●  " + label,
            12,
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
        {
            CreateButton(
                parent,
                names[i],
                labels[i]);
        }
    }

    private static void BuildToggleList(
        Transform parent,
        string[] names,
        string[] labels)
    {
        for (int i = 0; i < names.Length; i++)
        {
            CreateToggle(
                parent,
                names[i],
                labels[i]);
        }
    }

    private static TMP_Text CreateText(
        Transform parent,
        string name,
        string value,
        float fontSize,
        bool bold,
        TextAlignmentOptions alignment)
    {
        GameObject textObject =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(TextMeshProUGUI));

        textObject.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            textObject.GetComponent<RectTransform>();

        rect.anchorMin =
            Vector2.zero;

        rect.anchorMax =
            Vector2.one;

        rect.offsetMin =
            new Vector2(8f, 3f);

        rect.offsetMax =
            new Vector2(-8f, -3f);

        TextMeshProUGUI text =
            textObject.GetComponent<TextMeshProUGUI>();

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

    private static int ParentChildIndex(
        Transform child)
    {
        if (child.parent == null)
            return 0;

        return child.GetSiblingIndex();
    }

    // =========================================================
    // WORLD VISUALS
    // =========================================================

    private static void CreateEnvironmentBlock(
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

    private static void BuildFactionBorder(
        Transform parent,
        string name,
        Vector3 position,
        Vector3 scale,
        Color color)
    {
        CreateEnvironmentBlock(
            parent,
            name,
            position,
            scale,
            color);
    }

    private static void BuildFactionMarker(
        Transform parent,
        string name,
        Vector3 position,
        Color color)
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
            new Vector3(0.65f, 0.45f, 0.65f);

        ApplyMaterial(
            marker,
            color);
    }

    private static void BuildInfluenceZone(
        Transform parent,
        string name,
        Vector3 position,
        float radius,
        Color color)
    {
        GameObject zone =
            GameObject.CreatePrimitive(
                PrimitiveType.Cylinder);

        zone.name = name;
        zone.transform.SetParent(
            parent,
            false);

        zone.transform.position =
            position;

        zone.transform.localScale =
            new Vector3(
                radius,
                0.06f,
                radius);

        ApplyMaterial(
            zone,
            color);
    }

    private static void BuildThreatZone(
        Transform parent,
        string name,
        Vector3 position,
        float radius)
    {
        BuildInfluenceZone(
            parent,
            name,
            position,
            radius,
            new Color(0.9f, 0.15f, 0.12f, 0.38f));
    }

    private static void BuildFactionRoute(
        Transform parent,
        string name,
        Vector3[] points)
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
        line.widthMultiplier = 0.09f;

        for (int i = 0; i < points.Length; i++)
            line.SetPosition(i, points[i]);

        Material material =
            CreateWorldMaterial(
                new Color(0.15f, 0.7f, 0.95f));

        if (material != null)
            line.material = material;
    }

    private static void BuildSensorNetwork(
        Transform parent,
        string name,
        Vector3[] points)
    {
        GameObject network =
            new GameObject(name);

        network.transform.SetParent(
            parent,
            false);

        for (int i = 0; i < points.Length; i++)
        {
            GameObject node =
                GameObject.CreatePrimitive(
                    PrimitiveType.Sphere);

            node.name =
                name + "_NODE_" + (i + 1);

            node.transform.SetParent(
                network.transform,
                false);

            node.transform.position =
                points[i];

            node.transform.localScale =
                Vector3.one * 0.35f;

            ApplyMaterial(
                node,
                new Color(0.15f, 0.85f, 0.95f));
        }

        for (int i = 0; i < points.Length - 1; i++)
        {
            BuildFactionRoute(
                network.transform,
                name + "_LINK_" + (i + 1),
                new Vector3[]
                {
                    points[i],
                    points[i + 1]
                });
        }
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
            CreateWorldMaterial(color);

        if (material != null)
            renderer.sharedMaterial = material;
    }

    private static Material CreateWorldMaterial(
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

    private static void EnsureFolder(string path)
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
