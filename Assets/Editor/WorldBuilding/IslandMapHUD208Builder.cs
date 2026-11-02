using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public static class IslandMapHUD208Builder
{
    private const string ScenePath =
        "Assets/Scenes/SCN67_ISLAND_MAP/Island_Map.unity";

    private static readonly Color Background =
        new Color(0.006f, 0.012f, 0.020f, 1f);

    private static readonly Color PanelColor =
        new Color(0.025f, 0.045f, 0.065f, 0.96f);

    private static readonly Color Accent =
        new Color(0.15f, 0.75f, 0.95f, 1f);

    private static readonly Color Good =
        new Color(0.25f, 0.9f, 0.55f, 1f);

    private static readonly Color Warning =
        new Color(1f, 0.55f, 0.18f, 1f);

    private static readonly Color Danger =
        new Color(0.95f, 0.25f, 0.25f, 1f);

    [MenuItem("Obsidian Protocol/Worlds/BUILD HUD-208 ISLAND MAP")]
    public static void Build()
    {
        EnsureFolders();

        Scene scene =
            EditorSceneManager.NewScene(
                NewSceneSetup.EmptyScene,
                NewSceneMode.Single);

        BuildEnvironment();
        BuildCamera();
        BuildEventSystem();

        Canvas canvas = BuildCanvas();

        Transform root =
            CreateRoot(
                canvas.transform,
                "HUD-208 IslandMapHUD");

        BuildTopBar(root);
        BuildNavigation(root);
        BuildIslandLayers(root);
        BuildObjectInfo(root);
        BuildTerrainAnalysis(root);
        BuildWarfare(root);
        BuildQuickActions(root);
        BuildAlerts(root);
        BuildBottomBar(root);

        EditorSceneManager.SaveScene(
            scene,
            ScenePath);

        AddToBuildSettings(ScenePath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "[HUD-208] Island Map HUD built successfully: " +
            ScenePath);
    }

    private static void EnsureFolders()
    {
        System.IO.Directory.CreateDirectory(
            "Assets/Scenes/SCN67_ISLAND_MAP");

        System.IO.Directory.CreateDirectory(
            "Assets/Editor/WorldBuilding");
    }

    private static void BuildEnvironment()
    {
        GameObject environment =
            new GameObject(
                "ISLAND MAP ENVIRONMENT");

        GameObject backdrop =
            GameObject.CreatePrimitive(
                PrimitiveType.Quad);

        backdrop.name =
            "ISLAND MAP BACKDROP";

        backdrop.transform.SetParent(
            environment.transform);

        backdrop.transform.position =
            new Vector3(0f, 0f, 8f);

        backdrop.transform.localScale =
            new Vector3(28f, 18f, 1f);

        Material backdropMaterial =
            new Material(
                Shader.Find("Unlit/Color"));

        backdropMaterial.color =
            Background;

        Renderer backdropRenderer =
            backdrop.GetComponent<Renderer>();

        if (backdropRenderer != null)
            backdropRenderer.sharedMaterial =
                backdropMaterial;

        GameObject ocean =
            GameObject.CreatePrimitive(
                PrimitiveType.Quad);

        ocean.name =
            "ISLAND OCEAN MAP";

        ocean.transform.SetParent(
            environment.transform);

        ocean.transform.position =
            new Vector3(0f, 0f, 6f);

        ocean.transform.localScale =
            new Vector3(20f, 12f, 1f);

        Material oceanMaterial =
            new Material(
                Shader.Find("Unlit/Color"));

        oceanMaterial.color =
            new Color(
                0.015f,
                0.13f,
                0.19f,
                1f);

        Renderer oceanRenderer =
            ocean.GetComponent<Renderer>();

        if (oceanRenderer != null)
            oceanRenderer.sharedMaterial =
                oceanMaterial;

        BuildIslandMasses(environment);
        BuildWaterways(environment);
        BuildResourceNodes(environment);
        BuildWarfareZones(environment);
        BuildLogisticsRoutes(environment);
        BuildEnvironmentZones(environment);
        BuildThreatZones(environment);
        BuildChokepoints(environment);
    }

    private static void BuildIslandMasses(
        GameObject parent)
    {
        string[] names =
        {
            "ASHEN ISLAND",
            "NORTHWATCH ISLAND",
            "IRONCROWN ISLAND",
            "VERDANT ISLAND",
            "STORMBREAK ISLAND",
            "BLACKSHORE ISLAND",
            "FROSTFALL ISLAND",
            "SUNWARD ISLAND",
            "OUTER REACH"
        };

        Vector3[] positions =
        {
            new Vector3(-6.2f, 3.2f, 5f),
            new Vector3(-1.9f, 4.1f, 5f),
            new Vector3(3.0f, 3.2f, 5f),
            new Vector3(6.5f, 0.6f, 5f),
            new Vector3(2.6f, -0.9f, 5f),
            new Vector3(-2.1f, -1.2f, 5f),
            new Vector3(-6.0f, -0.8f, 5f),
            new Vector3(-3.6f, -4.0f, 5f),
            new Vector3(4.8f, -3.8f, 5f)
        };

        Vector3[] scales =
        {
            new Vector3(3.2f, 2.2f, 0.15f),
            new Vector3(2.8f, 2.0f, 0.15f),
            new Vector3(3.0f, 2.4f, 0.15f),
            new Vector3(2.4f, 1.8f, 0.15f),
            new Vector3(3.0f, 2.1f, 0.15f),
            new Vector3(3.2f, 2.2f, 0.15f),
            new Vector3(2.7f, 2.0f, 0.15f),
            new Vector3(2.6f, 1.8f, 0.15f),
            new Vector3(2.5f, 1.7f, 0.15f)
        };

        for (int i = 0; i < names.Length; i++)
        {
            GameObject island =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cylinder);

            island.name =
                "ISLAND_" +
                (i + 1).ToString("00") +
                "_" +
                names[i].Replace(
                    " ",
                    "_");

            island.transform.SetParent(
                parent.transform);

            island.transform.position =
                positions[i];

            island.transform.localScale =
                scales[i];

            Material material =
                new Material(
                    Shader.Find("Unlit/Color"));

            material.color =
                new Color(
                    0.08f + i * 0.008f,
                    0.28f + i * 0.006f,
                    0.18f + i * 0.005f,
                    1f);

            Renderer renderer =
                island.GetComponent<Renderer>();

            if (renderer != null)
                renderer.sharedMaterial =
                    material;
        }
    }

    private static void BuildWaterways(
        GameObject parent)
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject waterway =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube);

            waterway.name =
                "WATERWAY_" +
                (i + 1).ToString("00");

            waterway.transform.SetParent(
                parent.transform);

            waterway.transform.position =
                new Vector3(
                    -7.5f + i * 1.65f,
                    Mathf.Sin(i * 0.9f) * 3.2f,
                    4.3f);

            waterway.transform.rotation =
                Quaternion.Euler(
                    0f,
                    0f,
                    -15f + i * 3f);

            waterway.transform.localScale =
                new Vector3(
                    2.8f,
                    0.08f,
                    0.035f);

            Material material =
                new Material(
                    Shader.Find("Unlit/Color"));

            material.color =
                new Color(
                    0.04f,
                    0.42f,
                    0.72f,
                    1f);

            Renderer renderer =
                waterway.GetComponent<Renderer>();

            if (renderer != null)
                renderer.sharedMaterial =
                    material;
        }
    }

    private static void BuildResourceNodes(
        GameObject parent)
    {
        for (int i = 0; i < 14; i++)
        {
            float angle =
                i * Mathf.PI * 2f / 14f;

            GameObject node =
                GameObject.CreatePrimitive(
                    PrimitiveType.Sphere);

            node.name =
                "RESOURCE_NODE_" +
                (i + 1).ToString("00");

            node.transform.SetParent(
                parent.transform);

            node.transform.position =
                new Vector3(
                    Mathf.Cos(angle) * 7.4f,
                    Mathf.Sin(angle) * 4.7f,
                    4f);

            node.transform.localScale =
                Vector3.one * 0.17f;

            Material material =
                new Material(
                    Shader.Find("Unlit/Color"));

            material.color =
                new Color(
                    0.95f,
                    0.78f,
                    0.12f,
                    1f);

            Renderer renderer =
                node.GetComponent<Renderer>();

            if (renderer != null)
                renderer.sharedMaterial =
                    material;
        }
    }

    private static void BuildWarfareZones(
        GameObject parent)
    {
        for (int i = 0; i < 6; i++)
        {
            GameObject zone =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cylinder);

            zone.name =
                "WARFARE_ZONE_" +
                (i + 1).ToString("00");

            zone.transform.SetParent(
                parent.transform);

            zone.transform.position =
                new Vector3(
                    -6.5f + i * 2.5f,
                    0.4f +
                    Mathf.Sin(i) * 2f,
                    3.7f);

            zone.transform.localScale =
                new Vector3(
                    0.7f,
                    0.025f,
                    0.7f);

            Material material =
                new Material(
                    Shader.Find("Unlit/Color"));

            material.color =
                new Color(
                    0.85f,
                    0.10f,
                    0.12f,
                    0.75f);

            Renderer renderer =
                zone.GetComponent<Renderer>();

            if (renderer != null)
                renderer.sharedMaterial =
                    material;
        }
    }

    private static void BuildLogisticsRoutes(
        GameObject parent)
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject route =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube);

            route.name =
                "LOGISTICS_ROUTE_" +
                (i + 1).ToString("00");

            route.transform.SetParent(
                parent.transform);

            route.transform.position =
                new Vector3(
                    -7f + i * 2f,
                    -2.5f +
                    Mathf.Cos(i * 1.1f),
                    3.5f);

            route.transform.rotation =
                Quaternion.Euler(
                    0f,
                    0f,
                    20f);

            route.transform.localScale =
                new Vector3(
                    2.1f,
                    0.045f,
                    0.035f);

            Material material =
                new Material(
                    Shader.Find("Unlit/Color"));

            material.color =
                new Color(
                    0.82f,
                    0.55f,
                    0.12f,
                    1f);

            Renderer renderer =
                route.GetComponent<Renderer>();

            if (renderer != null)
                renderer.sharedMaterial =
                    material;
        }
    }

    private static void BuildEnvironmentZones(
        GameObject parent)
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject zone =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube);

            zone.name =
                "ENVIRONMENT_ZONE_" +
                (i + 1).ToString("00");

            zone.transform.SetParent(
                parent.transform);

            zone.transform.position =
                new Vector3(
                    -6f + (i % 4) * 4f,
                    3.8f - (i / 4) * 7f,
                    3.3f);

            zone.transform.localScale =
                new Vector3(
                    1.1f,
                    0.035f,
                    0.025f);

            Material material =
                new Material(
                    Shader.Find("Unlit/Color"));

            material.color =
                new Color(
                    0.2f,
                    0.75f,
                    0.65f,
                    0.8f);

            Renderer renderer =
                zone.GetComponent<Renderer>();

            if (renderer != null)
                renderer.sharedMaterial =
                    material;
        }
    }

    private static void BuildThreatZones(
        GameObject parent)
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject zone =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cylinder);

            zone.name =
                "THREAT_ZONE_" +
                (i + 1).ToString("00");

            zone.transform.SetParent(
                parent.transform);

            zone.transform.position =
                new Vector3(
                    -6f + i * 3f,
                    -3.2f +
                    Mathf.Sin(i) * 1.4f,
                    3.2f);

            zone.transform.localScale =
                new Vector3(
                    0.9f,
                    0.02f,
                    0.9f);

            Material material =
                new Material(
                    Shader.Find("Unlit/Color"));

            material.color =
                new Color(
                    0.9f,
                    0.2f,
                    0.1f,
                    0.55f);

            Renderer renderer =
                zone.GetComponent<Renderer>();

            if (renderer != null)
                renderer.sharedMaterial =
                    material;
        }
    }

    private static void BuildChokepoints(
        GameObject parent)
    {
        for (int i = 0; i < 7; i++)
        {
            GameObject point =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube);

            point.name =
                "CHOKEPOINT_" +
                (i + 1).ToString("00");

            point.transform.SetParent(
                parent.transform);

            point.transform.position =
                new Vector3(
                    -7f + i * 2.2f,
                    0.1f +
                    Mathf.Cos(i) * 2.6f,
                    3f);

            point.transform.localScale =
                new Vector3(
                    0.32f,
                    0.32f,
                    0.05f);

            Material material =
                new Material(
                    Shader.Find("Unlit/Color"));

            material.color =
                Warning;

            Renderer renderer =
                point.GetComponent<Renderer>();

            if (renderer != null)
                renderer.sharedMaterial =
                    material;
        }
    }

    private static void BuildCamera()
    {
        GameObject cameraObject =
            new GameObject(
                "ISLAND MAP CAMERA");

        Camera camera =
            cameraObject.AddComponent<Camera>();

        cameraObject.transform.position =
            new Vector3(
                0f,
                0f,
                -15f);

        cameraObject.transform.rotation =
            Quaternion.identity;

        camera.orthographic = true;
        camera.orthographicSize = 9f;
        camera.clearFlags =
            CameraClearFlags.SolidColor;
        camera.backgroundColor =
            Background;
        camera.depth = -10;

        cameraObject.tag =
            "MainCamera";
    }

    private static void BuildEventSystem()
    {
        GameObject eventSystemObject =
            new GameObject(
                "EventSystem",
                typeof(EventSystem),
                typeof(InputSystemUIInputModule));

        EventSystem eventSystem =
            eventSystemObject.GetComponent<EventSystem>();

        eventSystem.enabled = true;

        InputSystemUIInputModule module =
            eventSystemObject.GetComponent<
                InputSystemUIInputModule>();

        module.enabled = true;
    }

    private static Canvas BuildCanvas()
    {
        GameObject canvasObject =
            new GameObject(
                "ISLAND HUD CANVAS",
                typeof(RectTransform),
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));

        Canvas canvas =
            canvasObject.GetComponent<Canvas>();

        canvas.renderMode =
            RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler =
            canvasObject.GetComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(1920f, 1080f);

        scaler.screenMatchMode =
            CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        scaler.matchWidthOrHeight = 0.5f;

        return canvas;
    }

    private static Transform CreateRoot(
        Transform parent,
        string name)
    {
        GameObject go =
            new GameObject(
                name,
                typeof(RectTransform));

        go.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin =
            Vector2.zero;

        rect.anchorMax =
            Vector2.one;

        rect.offsetMin =
            Vector2.zero;

        rect.offsetMax =
            Vector2.zero;

        return go.transform;
    }

    private static GameObject Panel(
        Transform parent,
        string name,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 offsetMin,
        Vector2 offsetMax)
    {
        GameObject go =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image));

        go.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin =
            anchorMin;

        rect.anchorMax =
            anchorMax;

        rect.offsetMin =
            offsetMin;

        rect.offsetMax =
            offsetMax;

        Image image =
            go.GetComponent<Image>();

        image.color =
            PanelColor;

        return go;
    }

    private static TextMeshProUGUI Label(
        Transform parent,
        string name,
        string value,
        int size = 16,
        TextAlignmentOptions alignment =
            TextAlignmentOptions.Left)
    {
        GameObject go =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(TextMeshProUGUI));

        go.transform.SetParent(
            parent,
            false);

        TextMeshProUGUI text =
            go.GetComponent<TextMeshProUGUI>();

        text.text =
            value;

        text.fontSize =
            size;

        text.color =
            Color.white;

        text.alignment =
            alignment;

        text.raycastTarget =
            false;

        text.textWrappingMode =
            TextWrappingModes.Normal;

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin =
            Vector2.zero;

        rect.anchorMax =
            Vector2.one;

        rect.offsetMin =
            new Vector2(
                10f,
                4f);

        rect.offsetMax =
            new Vector2(
                -10f,
                -4f);

        return text;
    }

    private static Button Button(
        Transform parent,
        string name,
        string label)
    {
        GameObject go =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image),
                typeof(Button));

        go.transform.SetParent(
            parent,
            false);

        Image image =
            go.GetComponent<Image>();

        image.color =
            new Color(
                0.05f,
                0.10f,
                0.14f,
                0.98f);

        Button button =
            go.GetComponent<Button>();

        ColorBlock colors =
            button.colors;

        colors.normalColor =
            new Color(
                0.05f,
                0.10f,
                0.14f,
                0.98f);

        colors.highlightedColor =
            new Color(
                0.08f,
                0.22f,
                0.30f,
                1f);

        colors.pressedColor =
            Accent;

        colors.selectedColor =
            new Color(
                0.08f,
                0.22f,
                0.30f,
                1f);

        button.colors =
            colors;

        Label(
            go.transform,
            "LABEL",
            label,
            13,
            TextAlignmentOptions.Center);

        return button;
    }

    private static void SetRect(
        GameObject go,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 offsetMin,
        Vector2 offsetMax)
    {
        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin =
            anchorMin;

        rect.anchorMax =
            anchorMax;

        rect.offsetMin =
            offsetMin;

        rect.offsetMax =
            offsetMax;
    }

    private static void AddLogAction(
        Button button,
        string action)
    {
        button.onClick.AddListener(
            () => Debug.Log(
                "[HUD-208] " + action));
    }

    private static void BuildTopBar(
        Transform root)
    {
        GameObject panel =
            Panel(
                root,
                "HUD-208-01 TopBar",
                new Vector2(0f, 0.91f),
                new Vector2(1f, 1f),
                Vector2.zero,
                Vector2.zero);

        Label(
            panel.transform,
            "HUD-208-01A PlanetNameDisplay",
            "PLANET: ACHERON",
            19);

        Label(
            panel.transform,
            "HUD-208-01B ContinentNameDisplay",
            "CONTINENT: CINDERFALL",
            14);

        Label(
            panel.transform,
            "HUD-208-01C RegionNameDisplay",
            "REGION: ASHEN FRONTIER",
            14);

        Label(
            panel.transform,
            "HUD-208-01D IslandNameDisplay",
            "ISLAND: ASHEN ISLAND",
            15);

        Label(
            panel.transform,
            "HUD-208-01E OwnerFactionEmblem",
            "FACTION // OBSIDIAN COMMAND",
            13);

        Label(
            panel.transform,
            "HUD-208-01F PlayerResourcesPanel",
            "CREDITS 84,250    ENERGY 72%    SUPPLY 91%",
            13);

        Label(
            panel.transform,
            "HUD-208-01G TimeControlPanel",
            "TIME 14:32:08    PAUSED",
            13);
    }

    private static void BuildNavigation(
        Transform root)
    {
        GameObject panel =
            Panel(
                root,
                "HUD-208-02 NavigationPanel",
                new Vector2(0f, 0.36f),
                new Vector2(0.19f, 0.90f),
                new Vector2(12f, 0f),
                new Vector2(-8f, 0f));

        string[] names =
        {
            "HUD-208-02A ReturnToBiomeMapButton",
            "HUD-208-02B ReturnToRegionMapButton",
            "HUD-208-02C ReturnToContinentMapButton",
            "HUD-208-02D ReturnToSurfaceMapButton",
            "HUD-208-02E ReturnToPlanetOverviewButton"
        };

        string[] labels =
        {
            "RETURN TO BIOME MAP",
            "RETURN TO REGION MAP",
            "RETURN TO CONTINENT MAP",
            "RETURN TO SURFACE MAP",
            "RETURN TO PLANET OVERVIEW"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Button button =
                Button(
                    panel.transform,
                    names[i],
                    labels[i]);

            SetRect(
                button.gameObject,
                new Vector2(
                    0.06f,
                    0.80f - i * 0.16f),
                new Vector2(
                    0.94f,
                    0.93f - i * 0.16f),
                Vector2.zero,
                Vector2.zero);

            AddLogAction(
                button,
                labels[i]);
        }
    }

    private static void BuildIslandLayers(
        Transform root)
    {
        GameObject panel =
            Panel(
                root,
                "HUD-208-03 IslandLayersPanel",
                new Vector2(0.19f, 0.40f),
                new Vector2(0.34f, 0.90f),
                new Vector2(6f, 0f),
                new Vector2(-6f, 0f));

        string[] names =
        {
            "LayerToggle_Terrain",
            "LayerToggle_Elevation",
            "LayerToggle_Heatmap",
            "LayerToggle_Waterways",
            "LayerToggle_FactionControl",
            "LayerToggle_WarfareZones",
            "LayerToggle_ResourceNodes",
            "LayerToggle_LogisticsRoutes",
            "LayerToggle_WeatherSystems",
            "LayerToggle_EnvironmentType",
            "LayerToggle_ThreatZones"
        };

        string[] labels =
        {
            "TERRAIN",
            "ELEVATION",
            "HEATMAP",
            "WATERWAYS",
            "FACTION CONTROL",
            "WARFARE ZONES",
            "RESOURCE NODES",
            "LOGISTICS ROUTES",
            "WEATHER SYSTEMS",
            "ENVIRONMENT TYPE",
            "THREAT ZONES"
        };

        for (int i = 0; i < names.Length; i++)
        {
            int index = i;

            Button button =
                Button(
                    panel.transform,
                    "HUD-208-03" +
                    (char)('A' + i) +
                    " " +
                    names[i],
                    "[ ON ]  " +
                    labels[i]);

            SetRect(
                button.gameObject,
                new Vector2(
                    0.05f,
                    0.92f - i * 0.081f),
                new Vector2(
                    0.95f,
                    0.98f - i * 0.081f),
                Vector2.zero,
                Vector2.zero);

            bool state = true;

            button.onClick.AddListener(
                () =>
                {
                    state = !state;

                    TextMeshProUGUI text =
                        button.GetComponentInChildren<
                            TextMeshProUGUI>();

                    if (text != null)
                    {
                        text.text =
                            (state
                                ? "[ ON ]  "
                                : "[OFF]  ") +
                            labels[index];
                    }
                });
        }
    }

    private static void BuildObjectInfo(
        Transform root)
    {
        GameObject panel =
            Panel(
                root,
                "HUD-208-04 IslandObjectInfoPanel",
                new Vector2(0.68f, 0.38f),
                new Vector2(1f, 0.90f),
                Vector2.zero,
                new Vector2(-12f, 0f));

        string[] names =
        {
            "HUD-208-04A SelectedObjectName",
            "HUD-208-04B SelectedObjectType",
            "HUD-208-04C SelectedObjectOwnerFaction",
            "HUD-208-04D SelectedObjectStatus",
            "HUD-208-04E SelectedObjectThreatLevel",
            "HUD-208-04F SelectedObjectEnvironmentType",
            "HUD-208-04G SelectedObjectWarfareFocus"
        };

        string[] values =
        {
            "SELECTED: NORTHWATCH FORWARD BASE",
            "TYPE: ISLAND INSTALLATION",
            "OWNER: OBSIDIAN COMMAND",
            "STATUS: OPERATIONAL",
            "THREAT: ELEVATED",
            "ENVIRONMENT: COASTAL / TEMPERATE",
            "WARFARE FOCUS: DEFENSIVE CONTROL"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Label(
                panel.transform,
                names[i],
                values[i],
                14);
        }

        Button enter =
            Button(
                panel.transform,
                "HUD-208-04H EnterObjectButton",
                "ENTER SELECTED OBJECT");

        SetRect(
            enter.gameObject,
            new Vector2(0.08f, 0.04f),
            new Vector2(0.92f, 0.12f),
            Vector2.zero,
            Vector2.zero);

        AddLogAction(
            enter,
            "ENTER SELECTED OBJECT");
    }

    private static void BuildTerrainAnalysis(
        Transform root)
    {
        GameObject panel =
            Panel(
                root,
                "HUD-208-05 TerrainAnalysisPanel",
                new Vector2(0.34f, 0.04f),
                new Vector2(0.51f, 0.36f),
                Vector2.zero,
                Vector2.zero);

        string[] names =
        {
            "ElevationDisplay",
            "SlopeDisplay",
            "TraversabilityDisplay",
            "CoverDensityDisplay",
            "HazardZonesDisplay",
            "WaterAccessDisplay",
            "ChokepointDensityDisplay"
        };

        string[] values =
        {
            "ELEVATION          420 m",
            "SLOPE              19%",
            "TRAVERSABILITY     78%",
            "COVER DENSITY      64%",
            "HAZARD ZONES       3 ACTIVE",
            "WATER ACCESS       HIGH",
            "CHOKEPOINT DENSITY  MODERATE"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Label(
                panel.transform,
                "HUD-208-05" +
                (char)('A' + i) +
                " " +
                names[i],
                values[i],
                13);
        }
    }

    private static void BuildWarfare(
        Transform root)
    {
        GameObject panel =
            Panel(
                root,
                "HUD-208-06 WarfarePanel",
                new Vector2(0.51f, 0.04f),
                new Vector2(0.68f, 0.36f),
                Vector2.zero,
                Vector2.zero);

        string[] names =
        {
            "WarfareFocusDisplay",
            "EngagementRangeDisplay",
            "MobilityRatingDisplay",
            "ReconVisibilityDisplay",
            "SensorInterferenceDisplay",
            "AmbushPotentialDisplay",
            "DefensivePositionRatingDisplay"
        };

        string[] values =
        {
            "WARFARE FOCUS       DEFENSIVE",
            "ENGAGEMENT RANGE    3.1 km",
            "MOBILITY RATING     HIGH",
            "RECON VISIBILITY    82%",
            "SENSOR INTERFERENCE 17%",
            "AMBUSH POTENTIAL    HIGH",
            "DEFENSIVE POSITION  91%"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Label(
                panel.transform,
                "HUD-208-06" +
                (char)('A' + i) +
                " " +
                names[i],
                values[i],
                13);
        }
    }

    private static void BuildQuickActions(
        Transform root)
    {
        GameObject panel =
            Panel(
                root,
                "HUD-208-07 QuickActionsPanel",
                new Vector2(0.68f, 0.20f),
                new Vector2(0.84f, 0.38f),
                Vector2.zero,
                Vector2.zero);

        string[] names =
        {
            "HUD-208-07A DeployReconButton",
            "HUD-208-07B DeployLogisticsTeamButton",
            "HUD-208-07C DeployCombatGroupButton",
            "HUD-208-07D OpenIntelNetworkButton",
            "HUD-208-07E OpenDoctrineManagerButton",
            "HUD-208-07F OpenWarfareProfileButton"
        };

        string[] labels =
        {
            "DEPLOY RECON",
            "DEPLOY LOGISTICS",
            "DEPLOY COMBAT GROUP",
            "INTELLIGENCE NETWORK",
            "DOCTRINE MANAGER",
            "WARFARE PROFILE"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Button button =
                Button(
                    panel.transform,
                    names[i],
                    labels[i]);

            SetRect(
                button.gameObject,
                new Vector2(
                    0.04f,
                    0.84f - i * 0.14f),
                new Vector2(
                    0.96f,
                    0.96f - i * 0.14f),
                Vector2.zero,
                Vector2.zero);

            AddLogAction(
                button,
                labels[i]);
        }
    }

    private static void BuildAlerts(
        Transform root)
    {
        GameObject panel =
            Panel(
                root,
                "HUD-208-08 AlertsPanel",
                new Vector2(0.84f, 0.20f),
                new Vector2(1f, 0.90f),
                Vector2.zero,
                new Vector2(-12f, 0f));

        Label(
            panel.transform,
            "HUD-208-08A StrategicAlertEntry",
            "STRATEGIC // ISLAND SUPPLY ROUTE ACTIVE",
            12);

        Label(
            panel.transform,
            "HUD-208-08B WarfareAlertEntry",
            "WARFARE // HOSTILE LANDING ZONE DETECTED",
            12);

        Label(
            panel.transform,
            "HUD-208-08C FactionAlertEntry",
            "FACTION // NORTHERN CONTROL CONTESTED",
            12);

        Label(
            panel.transform,
            "HUD-208-08D ResourceAlertEntry",
            "RESOURCE // OFFSHORE NODE IDENTIFIED",
            12);

        Label(
            panel.transform,
            "HUD-208-08E WeatherAlertEntry",
            "WEATHER // STORM FRONT APPROACHING",
            12);

        Label(
            panel.transform,
            "HUD-208-08F EnvironmentalHazardAlertEntry",
            "ENVIRONMENT // TOXIC SHORELINE ZONE",
            12);
    }

    private static void BuildBottomBar(
        Transform root)
    {
        GameObject panel =
            Panel(
                root,
                "HUD-208-09 BottomBar",
                new Vector2(0f, 0f),
                new Vector2(1f, 0.085f),
                Vector2.zero,
                Vector2.zero);

        string[] names =
        {
            "HUD-208-09A BackButton",
            "HUD-208-09B ExitToCommandCenterButton",
            "HUD-208-09C SettingsButton",
            "HUD-208-09D HelpButton"
        };

        string[] labels =
        {
            "BACK",
            "COMMAND CENTER",
            "SETTINGS",
            "HELP"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Button button =
                Button(
                    panel.transform,
                    names[i],
                    labels[i]);

            float min =
                0.02f +
                i * 0.245f;

            SetRect(
                button.gameObject,
                new Vector2(
                    min,
                    0.12f),
                new Vector2(
                    min + 0.225f,
                    0.88f),
                Vector2.zero,
                Vector2.zero);

            AddLogAction(
                button,
                labels[i]);
        }
    }

    private static void AddToBuildSettings(
        string scenePath)
    {
        EditorBuildSettingsScene[] current =
            EditorBuildSettings.scenes;

        foreach (
            EditorBuildSettingsScene entry
            in current)
        {
            if (entry.path == scenePath)
                return;
        }

        EditorBuildSettingsScene[] updated =
            new EditorBuildSettingsScene[
                current.Length + 1];

        Array.Copy(
            current,
            updated,
            current.Length);

        updated[current.Length] =
            new EditorBuildSettingsScene(
                scenePath,
                true);

        EditorBuildSettings.scenes =
            updated;
    }
}
