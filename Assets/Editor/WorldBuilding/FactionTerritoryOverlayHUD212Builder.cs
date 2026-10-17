using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public static class FactionTerritoryOverlayHUD212Builder
{
    private const string ScenePath =
        "Assets/Scenes/SCN71_FACTION_TERRITORY_OVERLAY/Faction_Territory_Overlay.unity";

    private static readonly Color PanelColor =
        new Color(0.025f, 0.035f, 0.055f, 0.94f);

    private static readonly Color HeaderColor =
        new Color(0.055f, 0.075f, 0.105f, 0.98f);

    private static readonly Color ButtonColor =
        new Color(0.075f, 0.105f, 0.145f, 0.98f);

    private static readonly Color Accent =
        new Color(0.18f, 0.68f, 0.95f, 1f);

    private static readonly Color FactionBlue =
        new Color(0.12f, 0.50f, 0.95f, 0.55f);

    private static readonly Color FactionRed =
        new Color(0.90f, 0.20f, 0.18f, 0.55f);

    private static readonly Color FactionGold =
        new Color(0.90f, 0.70f, 0.18f, 0.55f);

    private static readonly Color Contested =
        new Color(0.85f, 0.28f, 0.16f, 0.48f);

    [MenuItem("Obsidian Protocol/Worlds/BUILD HUD-212 FACTION TERRITORY OVERLAY")]
    public static void Build()
    {
        EnsureFolder("Assets/Scenes");
        EnsureFolder("Assets/Scenes/SCN71_FACTION_TERRITORY_OVERLAY");

        EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene,
            NewSceneMode.Single);

        BuildTerritoryEnvironment();
        BuildTerritoryCamera();
        BuildTerritoryEventSystem();
        BuildTerritoryHUD();

        Scene scene = SceneManager.GetActiveScene();

        EditorSceneManager.SaveScene(
            scene,
            ScenePath);

        AddToBuildSettings(ScenePath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "[HUD-212] Faction Territory Overlay built: " +
            ScenePath);
    }

    // =========================================================
    // ENVIRONMENT
    // =========================================================

    private static void BuildTerritoryEnvironment()
    {
        GameObject root =
            new GameObject(
                "FACTION TERRITORY OVERLAY ENVIRONMENT");

        CreateBlock(
            root.transform,
            "OverlayTerrain",
            new Vector3(0f, -0.65f, 0f),
            new Vector3(28f, 1f, 21f),
            new Color(0.065f, 0.085f, 0.105f));

        CreateBlock(
            root.transform,
            "FactionTerritory_A",
            new Vector3(-7f, 0.05f, 4.5f),
            new Vector3(10f, 0.35f, 7f),
            FactionBlue);

        CreateBlock(
            root.transform,
            "FactionTerritory_B",
            new Vector3(7f, 0.06f, -4.5f),
            new Vector3(10f, 0.36f, 7f),
            FactionRed);

        CreateBlock(
            root.transform,
            "FactionTerritory_C",
            new Vector3(8f, 0.07f, 5f),
            new Vector3(6f, 0.38f, 5f),
            FactionGold);

        CreateBlock(
            root.transform,
            "ContestedTerritory",
            new Vector3(0f, 0.10f, 0f),
            new Vector3(7f, 0.40f, 5.5f),
            Contested);

        CreateBlock(
            root.transform,
            "NeutralTerritory",
            new Vector3(-9f, 0.05f, -5.5f),
            new Vector3(5f, 0.32f, 5f),
            new Color(0.20f, 0.24f, 0.28f, 0.65f));

        BuildTerritoryBorder(
            root.transform,
            "FactionBorder_North",
            new Vector3(-7f, 0.55f, 1f),
            new Vector3(0.18f, 0.12f, 13f),
            Accent);

        BuildTerritoryBorder(
            root.transform,
            "FactionBorder_Center",
            new Vector3(0f, 0.58f, 0f),
            new Vector3(13f, 0.12f, 0.18f),
            new Color(0.95f, 0.35f, 0.25f));

        BuildTerritoryBorder(
            root.transform,
            "FactionBorder_East",
            new Vector3(5f, 0.60f, 4f),
            new Vector3(0.18f, 0.12f, 9f),
            FactionGold);

        BuildTerritoryMarker(
            root.transform,
            "FactionCapital",
            new Vector3(-7f, 0.75f, 4.5f),
            Accent,
            0.7f);

        BuildTerritoryMarker(
            root.transform,
            "ForwardBase",
            new Vector3(5f, 0.75f, -3.5f),
            FactionRed,
            0.55f);

        BuildTerritoryMarker(
            root.transform,
            "StrategicResourceNode",
            new Vector3(8f, 0.75f, 5f),
            FactionGold,
            0.5f);

        BuildOverlayRing(
            root.transform,
            "InfluenceRing_A",
            new Vector3(-7f, 0.68f, 4.5f),
            4.5f,
            FactionBlue);

        BuildOverlayRing(
            root.transform,
            "InfluenceRing_B",
            new Vector3(7f, 0.69f, -4.5f),
            4.2f,
            FactionRed);

        BuildOverlayRing(
            root.transform,
            "ConflictIntensityRing",
            new Vector3(0f, 0.72f, 0f),
            3.2f,
            Contested);

        BuildRoute(
            root.transform,
            "LogisticsRoute",
            new Vector3[]
            {
                new Vector3(-7f, 0.82f, 4.5f),
                new Vector3(-3f, 0.82f, 3f),
                new Vector3(0f, 0.82f, 0f),
                new Vector3(5f, 0.82f, -3.5f)
            },
            new Color(0.15f, 0.75f, 0.95f));

        BuildRoute(
            root.transform,
            "ReinforcementPath",
            new Vector3[]
            {
                new Vector3(8f, 0.84f, 5f),
                new Vector3(5f, 0.84f, 3f),
                new Vector3(2f, 0.84f, 1f),
                new Vector3(5f, 0.84f, -3.5f)
            },
            new Color(0.95f, 0.70f, 0.20f));

        BuildResistanceMarkers(root.transform);
        BuildSupplyNodes(root.transform);
    }

    private static void BuildResistanceMarkers(
        Transform parent)
    {
        Vector3[] points =
        {
            new Vector3(-2f, 0.8f, 5f),
            new Vector3(1f, 0.8f, 4f),
            new Vector3(3f, 0.8f, -1f),
            new Vector3(-4f, 0.8f, -3f),
            new Vector3(7f, 0.8f, 0f)
        };

        for (int i = 0; i < points.Length; i++)
        {
            BuildTerritoryMarker(
                parent,
                "ResistanceActivity_" + (i + 1),
                points[i],
                new Color(0.95f, 0.30f, 0.20f),
                0.28f);
        }
    }

    private static void BuildSupplyNodes(
        Transform parent)
    {
        Vector3[] points =
        {
            new Vector3(-6f, 0.82f, 2f),
            new Vector3(-2f, 0.82f, 2.5f),
            new Vector3(2f, 0.82f, -1f),
            new Vector3(6f, 0.82f, -2f),
            new Vector3(8f, 0.82f, 3f)
        };

        for (int i = 0; i < points.Length; i++)
        {
            BuildTerritoryMarker(
                parent,
                "SupplyNode_" + (i + 1),
                points[i],
                new Color(0.25f, 0.85f, 0.55f),
                0.22f);
        }
    }

    private static void BuildTerritoryCamera()
    {
        GameObject cameraObject =
            new GameObject(
                "FactionTerritoryOverlayCamera");

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

    private static void BuildTerritoryEventSystem()
    {
        GameObject eventSystem =
            new GameObject("EventSystem");

        eventSystem.AddComponent<EventSystem>();
        eventSystem.AddComponent<InputSystemUIInputModule>();
    }

    // =========================================================
    // HUD ROOT
    // =========================================================

    private static void BuildTerritoryHUD()
    {
        GameObject canvasObject =
            new GameObject(
                "HUD-212 FactionTerritoryOverlayHUD");

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
        BuildOverlayLayers(canvasObject.transform);
        BuildFactionOverlayInfo(canvasObject.transform);
        BuildTerritoryStats(canvasObject.transform);
        BuildConflictDynamics(canvasObject.transform);
        BuildQuickActions(canvasObject.transform);
        BuildAlerts(canvasObject.transform);
        BuildBottomBar(canvasObject.transform);
    }

    // =========================================================
    // TOP BAR
    // =========================================================

    private static void BuildTopBar(
        Transform parent)
    {
        GameObject panel =
            CreatePanel(
                parent,
                "HUD-212-01 TopBar",
                "FACTION TERRITORY OVERLAY",
                new Vector2(0.01f, 0.91f),
                new Vector2(0.99f, 0.99f));

        CreateDisplay(
            panel.transform,
            "HUD-212-01A PlanetNameDisplay",
            "PLANET // OBSIDIAN WORLD");

        CreateDisplay(
            panel.transform,
            "HUD-212-01B ContinentNameDisplay",
            "CONTINENT // ASHEN CONTINENT");

        CreateDisplay(
            panel.transform,
            "HUD-212-01C RegionNameDisplay",
            "REGION // NORTHERN FRONT");

        CreateDisplay(
            panel.transform,
            "HUD-212-01D OverlayModeDisplay",
            "OVERLAY // FACTION TERRITORY");

        CreateEmblem(
            panel.transform,
            "HUD-212-01E PlayerFactionEmblem");

        CreateDisplay(
            panel.transform,
            "HUD-212-01F PlayerResourcesPanel",
            "RESOURCES 84,250  |  SUPPLY 72%");

        CreateDisplay(
            panel.transform,
            "HUD-212-01G TimeControlPanel",
            "TIME 14:32:08  |  PAUSED");
    }

    // =========================================================
    // NAVIGATION
    // =========================================================

    private static void BuildNavigation(
        Transform parent)
    {
        GameObject panel =
            CreatePanel(
                parent,
                "HUD-212-02 NavigationPanel",
                "NAVIGATION",
                new Vector2(0.01f, 0.20f),
                new Vector2(0.145f, 0.90f));

        string[] names =
        {
            "HUD-212-02A ReturnToFactionControlMapButton",
            "HUD-212-02B ReturnToWarfareProfileButton",
            "HUD-212-02C ReturnToEnvironmentProfileButton",
            "HUD-212-02D ReturnToIslandMapButton",
            "HUD-212-02E ReturnToBiomeMapButton",
            "HUD-212-02F ReturnToRegionMapButton",
            "HUD-212-02G ReturnToContinentMapButton",
            "HUD-212-02H ReturnToSurfaceMapButton",
            "HUD-212-02I ReturnToPlanetOverviewButton"
        };

        string[] labels =
        {
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

    // =========================================================
    // OVERLAY LAYERS
    // =========================================================

    private static void BuildOverlayLayers(
        Transform parent)
    {
        GameObject panel =
            CreatePanel(
                parent,
                "HUD-212-03 OverlayLayersPanel",
                "OVERLAY LAYERS",
                new Vector2(0.155f, 0.20f),
                new Vector2(0.30f, 0.90f));

        string[] names =
        {
            "HUD-212-03A LayerToggle_FactionControl",
            "HUD-212-03B LayerToggle_FactionBorders",
            "HUD-212-03C LayerToggle_FactionInfluence",
            "HUD-212-03D LayerToggle_ConflictIntensity",
            "HUD-212-03E LayerToggle_ThreatZones",
            "HUD-212-03F LayerToggle_LogisticsRoutes",
            "HUD-212-03G LayerToggle_ReinforcementPaths",
            "HUD-212-03H LayerToggle_ResistanceActivity",
            "HUD-212-03I LayerToggle_SupplyLineIntegrity",
            "HUD-212-03J LayerToggle_WeatherImpact",
            "HUD-212-03K LayerToggle_TerrainModifiers"
        };

        string[] labels =
        {
            "FACTION CONTROL",
            "FACTION BORDERS",
            "FACTION INFLUENCE",
            "CONFLICT INTENSITY",
            "THREAT ZONES",
            "LOGISTICS ROUTES",
            "REINFORCEMENT PATHS",
            "RESISTANCE ACTIVITY",
            "SUPPLY LINE INTEGRITY",
            "WEATHER IMPACT",
            "TERRAIN MODIFIERS"
        };

        BuildToggleList(
            panel.transform,
            names,
            labels);
    }

    // =========================================================
    // FACTION OVERLAY INFO
    // =========================================================

    private static void BuildFactionOverlayInfo(
        Transform parent)
    {
        GameObject panel =
            CreatePanel(
                parent,
                "HUD-212-04 FactionOverlayInfoPanel",
                "SELECTED FACTION OVERLAY",
                new Vector2(0.31f, 0.57f),
                new Vector2(0.54f, 0.90f));

        CreateDisplay(
            panel.transform,
            "HUD-212-04A SelectedFactionName",
            "IRON COVENANT");

        CreateEmblem(
            panel.transform,
            "HUD-212-04B SelectedFactionEmblem");

        CreateDisplay(
            panel.transform,
            "HUD-212-04C SelectedFactionTerritorySize",
            "TERRITORY // 31.4%");

        CreateDisplay(
            panel.transform,
            "HUD-212-04D SelectedFactionInfluenceLevel",
            "INFLUENCE // 78 / 100");

        CreateDisplay(
            panel.transform,
            "HUD-212-04E SelectedFactionConflictIntensity",
            "CONFLICT // 74%");

        CreateDisplay(
            panel.transform,
            "HUD-212-04F SelectedFactionThreatLevel",
            "THREAT // HIGH");

        CreateDisplay(
            panel.transform,
            "HUD-212-04G SelectedFactionLogisticsCapacity",
            "LOGISTICS // 74%");

        CreateDisplay(
            panel.transform,
            "HUD-212-04H SelectedFactionWarfareFocus",
            "WARFARE // TERRITORIAL CONTROL");

        CreateButton(
            panel.transform,
            "HUD-212-04I EnterFactionTerritoryButton",
            "ENTER FACTION TERRITORY");
    }

    // =========================================================
    // TERRITORY STATS
    // =========================================================

    private static void BuildTerritoryStats(
        Transform parent)
    {
        GameObject panel =
            CreatePanel(
                parent,
                "HUD-212-05 TerritoryStatsPanel",
                "TERRITORY STATUS",
                new Vector2(0.55f, 0.57f),
                new Vector2(0.77f, 0.90f));

        CreateDisplay(
            panel.transform,
            "HUD-212-05A ControlPercentageDisplay",
            "CONTROL // 68%");

        CreateDisplay(
            panel.transform,
            "HUD-212-05B ConflictIntensityDisplay",
            "CONFLICT INTENSITY // 74%");

        CreateDisplay(
            panel.transform,
            "HUD-212-05C OccupationStatusDisplay",
            "OCCUPATION // CONTESTED");

        CreateDisplay(
            panel.transform,
            "HUD-212-05D ResistanceActivityDisplay",
            "RESISTANCE // MODERATE");

        CreateDisplay(
            panel.transform,
            "HUD-212-05E StabilityIndexDisplay",
            "STABILITY // 61 / 100");

        CreateDisplay(
            panel.transform,
            "HUD-212-05F SupplyLineIntegrityDisplay",
            "SUPPLY LINE // 83%");

        CreateDisplay(
            panel.transform,
            "HUD-212-05G ReinforcementRoutesDisplay",
            "REINFORCEMENT // 6 ACTIVE");
    }

    // =========================================================
    // CONFLICT DYNAMICS
    // =========================================================

    private static void BuildConflictDynamics(
        Transform parent)
    {
        GameObject panel =
            CreatePanel(
                parent,
                "HUD-212-06 ConflictDynamicsPanel",
                "CONFLICT DYNAMICS",
                new Vector2(0.78f, 0.57f),
                new Vector2(0.99f, 0.90f));

        CreateDisplay(
            panel.transform,
            "HUD-212-06A FrontlineShiftDisplay",
            "FRONTLINE SHIFT // +1.8 KM");

        CreateDisplay(
            panel.transform,
            "HUD-212-06B EncirclementRiskDisplay",
            "ENCIRCLEMENT RISK // 42%");

        CreateDisplay(
            panel.transform,
            "HUD-212-06C BreakthroughPotentialDisplay",
            "BREAKTHROUGH POTENTIAL // 67%");

        CreateDisplay(
            panel.transform,
            "HUD-212-06D AttritionRateDisplay",
            "ATTRITION // 3.8% / DAY");

        CreateDisplay(
            panel.transform,
            "HUD-212-06E FactionMoraleDisplay",
            "FACTION MORALE // 72 / 100");
    }

    // =========================================================
    // QUICK ACTIONS
    // =========================================================

    private static void BuildQuickActions(
        Transform parent)
    {
        GameObject panel =
            CreatePanel(
                parent,
                "HUD-212-07 QuickActionsPanel",
                "QUICK ACTIONS",
                new Vector2(0.31f, 0.20f),
                new Vector2(0.54f, 0.54f));

        string[] names =
        {
            "HUD-212-07A DeployReconButton",
            "HUD-212-07B DeployCombatGroupButton",
            "HUD-212-07C DeployEW_TeamButton",
            "HUD-212-07D OpenIntelNetworkButton",
            "HUD-212-07E OpenDoctrineManagerButton",
            "HUD-212-07F OpenLogisticsOverviewButton",
            "HUD-212-07G OpenFactionDiplomacyButton"
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

    private static void BuildAlerts(
        Transform parent)
    {
        GameObject panel =
            CreatePanel(
                parent,
                "HUD-212-08 AlertsPanel",
                "TERRITORY OVERLAY ALERTS",
                new Vector2(0.55f, 0.20f),
                new Vector2(0.99f, 0.54f));

        CreateDisplay(
            panel.transform,
            "HUD-212-08A StrategicAlertEntry",
            "STRATEGIC // Territory control shifting");

        CreateDisplay(
            panel.transform,
            "HUD-212-08B WarfareAlertEntry",
            "WARFARE // Enemy concentration detected");

        CreateDisplay(
            panel.transform,
            "HUD-212-08C FactionAlertEntry",
            "FACTION // Influence boundary changed");

        CreateDisplay(
            panel.transform,
            "HUD-212-08D ResourceAlertEntry",
            "RESOURCE // Strategic node contested");

        CreateDisplay(
            panel.transform,
            "HUD-212-08E WeatherAlertEntry",
            "WEATHER // Regional visibility reduced");

        CreateDisplay(
            panel.transform,
            "HUD-212-08F EnvironmentalHazardAlertEntry",
            "ENVIRONMENT // Hazard zone expanding");

        CreateDisplay(
            panel.transform,
            "HUD-212-08G TerritoryShiftAlertEntry",
            "TERRITORY // Frontline shifted");

        CreateDisplay(
            panel.transform,
            "HUD-212-08H ConflictEscalationAlertEntry",
            "CONFLICT // Escalation detected");

        CreateDisplay(
            panel.transform,
            "HUD-212-08I SupplyLineBreakAlertEntry",
            "SUPPLY // Route integrity compromised");
    }

    // =========================================================
    // BOTTOM BAR
    // =========================================================

    private static void BuildBottomBar(
        Transform parent)
    {
        GameObject panel =
            CreatePanel(
                parent,
                "HUD-212-09 BottomBar",
                "",
                new Vector2(0.01f, 0.02f),
                new Vector2(0.99f, 0.105f));

        CreateButton(
            panel.transform,
            "HUD-212-09A BackButton",
            "BACK");

        CreateButton(
            panel.transform,
            "HUD-212-09B ExitToCommandCenterButton",
            "COMMAND CENTER");

        CreateButton(
            panel.transform,
            "HUD-212-09C SettingsButton",
            "SETTINGS");

        CreateButton(
            panel.transform,
            "HUD-212-09D HelpButton",
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
            PanelColor;

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
                HeaderColor;

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

        Image image =
            objectRoot.GetComponent<Image>();

        image.color = ButtonColor;

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
            ButtonColor;

        RectTransform rect =
            objectRoot.GetComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0.05f, 0f);

        rect.anchorMax =
            new Vector2(0.95f, 0f);

        rect.sizeDelta =
            new Vector2(0f, 29f);

        int index =
            objectRoot.transform.GetSiblingIndex();

        rect.anchoredPosition =
            new Vector2(
                0f,
                -40f - index * 32f);

        CreateText(
            objectRoot.transform,
            "LABEL",
            "●  " + label,
            11.5f,
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
    // WORLD VISUAL HELPERS
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

    private static void BuildTerritoryBorder(
        Transform parent,
        string name,
        Vector3 position,
        Vector3 scale,
        Color color)
    {
        CreateBlock(
            parent,
            name,
            position,
            scale,
            color);
    }

    private static void BuildTerritoryMarker(
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
                0.45f,
                scale);

        ApplyMaterial(
            marker,
            color);
    }

    private static void BuildOverlayRing(
        Transform parent,
        string name,
        Vector3 position,
        float radius,
        Color color)
    {
        GameObject ring =
            GameObject.CreatePrimitive(
                PrimitiveType.Cylinder);

        ring.name = name;

        ring.transform.SetParent(
            parent,
            false);

        ring.transform.position =
            position;

        ring.transform.localScale =
            new Vector3(
                radius,
                0.035f,
                radius);

        ApplyMaterial(
            ring,
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
        line.widthMultiplier = 0.085f;

        for (int i = 0; i < points.Length; i++)
            line.SetPosition(i, points[i]);

        Material material =
            CreateMaterial(color);

        if (material != null)
            line.material = material;
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
