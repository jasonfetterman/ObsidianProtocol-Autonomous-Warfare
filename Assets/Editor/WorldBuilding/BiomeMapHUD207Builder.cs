using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public static class BiomeMapHUD207Builder
{
    private const string ScenePath =
        "Assets/Scenes/SCN66_BIOME_MAP/Biome_Map.unity";

    private static readonly Color Background =
        new Color(0.008f, 0.014f, 0.022f, 1f);

    private static readonly Color PanelColor =
        new Color(0.025f, 0.045f, 0.065f, 0.96f);

    private static readonly Color PanelAlt =
        new Color(0.035f, 0.060f, 0.085f, 0.96f);

    private static readonly Color Accent =
        new Color(0.15f, 0.75f, 0.95f, 1f);

    private static readonly Color Warning =
        new Color(1f, 0.55f, 0.18f, 1f);

    private static readonly Color Danger =
        new Color(0.95f, 0.25f, 0.25f, 1f);

    private static readonly Color Good =
        new Color(0.25f, 0.9f, 0.55f, 1f);

    [MenuItem("Obsidian Protocol/Worlds/BUILD HUD-207 BIOME MAP")]
    public static void Build()
    {
        EnsureFolders();

        var scene = EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene,
            NewSceneMode.Single);

        BuildEnvironment();
        BuildCamera();
        BuildEventSystem();

        Canvas canvas = BuildCanvas();
        Transform root = CreateRoot(
            canvas.transform,
            "HUD-207 BiomeMapHUD");

        BuildTopBar(root);
        BuildNavigation(root);
        BuildBiomeLayers(root);
        BuildObjectInfo(root);
        BuildTerrainAnalysis(root);
        BuildWarfare(root);
        BuildQuickActions(root);
        BuildAlerts(root);
        BuildBottomBar(root);

        EditorSceneManager.SaveScene(scene, ScenePath);
        AddToBuildSettings(ScenePath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "[HUD-207] Biome Map HUD built successfully: " +
            ScenePath);
    }

    private static void EnsureFolders()
    {
        System.IO.Directory.CreateDirectory(
            "Assets/Scenes/SCN66_BIOME_MAP");
        System.IO.Directory.CreateDirectory(
            "Assets/Editor/WorldBuilding");
    }

    private static void BuildEnvironment()
    {
        GameObject environment =
            new GameObject("BIOME MAP ENVIRONMENT");

        GameObject backdrop =
            GameObject.CreatePrimitive(
                PrimitiveType.Quad);

        backdrop.name = "BIOME MAP BACKDROP";
        backdrop.transform.SetParent(
            environment.transform);

        backdrop.transform.position =
            new Vector3(0f, 0f, 8f);

        backdrop.transform.rotation =
            Quaternion.Euler(0f, 0f, 0f);

        backdrop.transform.localScale =
            new Vector3(28f, 18f, 1f);

        Material backdropMaterial =
            new Material(Shader.Find("Unlit/Color"));

        backdropMaterial.color = Background;

        Renderer backdropRenderer =
            backdrop.GetComponent<Renderer>();

        if (backdropRenderer != null)
            backdropRenderer.sharedMaterial =
                backdropMaterial;

        GameObject terrain =
            GameObject.CreatePrimitive(
                PrimitiveType.Quad);

        terrain.name = "BIOME RELIEF MAP";
        terrain.transform.SetParent(
            environment.transform);

        terrain.transform.position =
            new Vector3(0f, 0f, 6f);

        terrain.transform.localScale =
            new Vector3(19f, 11f, 1f);

        Material terrainMaterial =
            new Material(Shader.Find("Unlit/Color"));

        terrainMaterial.color =
            new Color(0.025f, 0.12f, 0.10f, 1f);

        Renderer terrainRenderer =
            terrain.GetComponent<Renderer>();

        if (terrainRenderer != null)
            terrainRenderer.sharedMaterial =
                terrainMaterial;

        BuildBiomeRegions(environment);
        BuildWaterways(environment);
        BuildResourceNodes(environment);
        BuildWarfareZones(environment);
        BuildLogisticsRoutes(environment);
        BuildBiomeBoundaries(environment);
        BuildVegetation(environment);
        BuildTraversability(environment);
    }

    private static void BuildBiomeRegions(GameObject parent)
    {
        string[] names =
        {
            "TEMPERATE FOREST",
            "ARID HIGHLANDS",
            "TROPICAL BASIN",
            "FROZEN TUNDRA",
            "COASTAL MARSH",
            "VOLCANIC WASTE",
            "GRASSLAND",
            "DENSE JUNGLE",
            "ROCKY PLATEAU"
        };

        Vector3[] positions =
        {
            new Vector3(-6f, 3f, 5f),
            new Vector3(0f, 4f, 5f),
            new Vector3(6f, 3f, 5f),
            new Vector3(-5f, -1f, 5f),
            new Vector3(0f, -1f, 5f),
            new Vector3(5f, -1f, 5f),
            new Vector3(-6f, -4f, 5f),
            new Vector3(0f, -4f, 5f),
            new Vector3(6f, -4f, 5f)
        };

        for (int i = 0; i < names.Length; i++)
        {
            GameObject biome =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube);

            biome.name =
                "BIOME_REGION_" +
                (i + 1).ToString("00") +
                "_" +
                names[i].Replace(" ", "_");

            biome.transform.SetParent(
                parent.transform);

            biome.transform.position =
                positions[i];

            biome.transform.localScale =
                new Vector3(4.4f, 2.3f, 0.15f);

            Material material =
                new Material(Shader.Find("Unlit/Color"));

            material.color =
                new Color(
                    0.035f + i * 0.006f,
                    0.16f + i * 0.006f,
                    0.12f + i * 0.008f,
                    1f);

            Renderer renderer =
                biome.GetComponent<Renderer>();

            if (renderer != null)
                renderer.sharedMaterial = material;
        }
    }

    private static void BuildWaterways(GameObject parent)
    {
        for (int i = 0; i < 7; i++)
        {
            GameObject water =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube);

            water.name =
                "WATERWAY_" +
                (i + 1).ToString("00");

            water.transform.SetParent(
                parent.transform);

            water.transform.position =
                new Vector3(
                    -7f + i * 2.1f,
                    -0.2f + Mathf.Sin(i) * 2.1f,
                    4.6f);

            water.transform.rotation =
                Quaternion.Euler(
                    0f,
                    0f,
                    -12f + i * 4f);

            water.transform.localScale =
                new Vector3(
                    3.4f,
                    0.12f,
                    0.04f);

            Material material =
                new Material(Shader.Find("Unlit/Color"));

            material.color =
                new Color(0.04f, 0.35f, 0.65f, 1f);

            Renderer renderer =
                water.GetComponent<Renderer>();

            if (renderer != null)
                renderer.sharedMaterial = material;
        }
    }

    private static void BuildResourceNodes(GameObject parent)
    {
        for (int i = 0; i < 12; i++)
        {
            GameObject node =
                GameObject.CreatePrimitive(
                    PrimitiveType.Sphere);

            node.name =
                "RESOURCE_NODE_" +
                (i + 1).ToString("00");

            node.transform.SetParent(
                parent.transform);

            float angle =
                i * Mathf.PI * 2f / 12f;

            node.transform.position =
                new Vector3(
                    Mathf.Cos(angle) * 7f,
                    Mathf.Sin(angle) * 4.5f,
                    4.2f);

            node.transform.localScale =
                Vector3.one * 0.16f;

            Material material =
                new Material(Shader.Find("Unlit/Color"));

            material.color =
                new Color(0.9f, 0.75f, 0.15f, 1f);

            Renderer renderer =
                node.GetComponent<Renderer>();

            if (renderer != null)
                renderer.sharedMaterial = material;
        }
    }

    private static void BuildWarfareZones(GameObject parent)
    {
        for (int i = 0; i < 5; i++)
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
                    -6f + i * 3f,
                    0.5f - (i % 2) * 2.2f,
                    4f);

            zone.transform.localScale =
                new Vector3(
                    0.75f,
                    0.02f,
                    0.75f);

            Material material =
                new Material(Shader.Find("Unlit/Color"));

            material.color =
                new Color(
                    0.8f,
                    0.12f,
                    0.12f,
                    0.7f);

            Renderer renderer =
                zone.GetComponent<Renderer>();

            if (renderer != null)
                renderer.sharedMaterial = material;
        }
    }

    private static void BuildLogisticsRoutes(GameObject parent)
    {
        for (int i = 0; i < 6; i++)
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
                    -6f + i * 2.4f,
                    -2.8f + Mathf.Sin(i * 1.3f),
                    4.1f);

            route.transform.rotation =
                Quaternion.Euler(
                    0f,
                    0f,
                    25f);

            route.transform.localScale =
                new Vector3(
                    2.2f,
                    0.045f,
                    0.035f);

            Material material =
                new Material(Shader.Find("Unlit/Color"));

            material.color =
                new Color(0.8f, 0.55f, 0.1f, 1f);

            Renderer renderer =
                route.GetComponent<Renderer>();

            if (renderer != null)
                renderer.sharedMaterial = material;
        }
    }

    private static void BuildBiomeBoundaries(GameObject parent)
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject boundary =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube);

            boundary.name =
                "BIOME_BOUNDARY_" +
                (i + 1).ToString("00");

            boundary.transform.SetParent(
                parent.transform);

            boundary.transform.position =
                new Vector3(
                    -7f + i * 2f,
                    Mathf.Cos(i * 0.8f) * 3f,
                    3.9f);

            boundary.transform.rotation =
                Quaternion.Euler(
                    0f,
                    0f,
                    90f);

            boundary.transform.localScale =
                new Vector3(
                    0.035f,
                    1.6f,
                    0.03f);

            Material material =
                new Material(Shader.Find("Unlit/Color"));

            material.color =
                new Color(0.25f, 0.8f, 0.65f, 1f);

            Renderer renderer =
                boundary.GetComponent<Renderer>();

            if (renderer != null)
                renderer.sharedMaterial = material;
        }
    }

    private static void BuildVegetation(GameObject parent)
    {
        for (int i = 0; i < 16; i++)
        {
            GameObject vegetation =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube);

            vegetation.name =
                "VEGETATION_DENSITY_" +
                (i + 1).ToString("00");

            vegetation.transform.SetParent(
                parent.transform);

            vegetation.transform.position =
                new Vector3(
                    -7f + (i % 8) * 2f,
                    3.7f - (i / 8) * 7f,
                    3.8f);

            vegetation.transform.localScale =
                new Vector3(
                    0.5f,
                    0.5f + (i % 4) * 0.15f,
                    0.05f);

            Material material =
                new Material(Shader.Find("Unlit/Color"));

            material.color =
                new Color(0.1f, 0.55f, 0.2f, 1f);

            Renderer renderer =
                vegetation.GetComponent<Renderer>();

            if (renderer != null)
                renderer.sharedMaterial = material;
        }
    }

    private static void BuildTraversability(GameObject parent)
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject marker =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube);

            marker.name =
                "TRAVERSABILITY_" +
                (i + 1).ToString("00");

            marker.transform.SetParent(
                parent.transform);

            marker.transform.position =
                new Vector3(
                    -8f + i * 1.7f,
                    -5f,
                    3.6f);

            marker.transform.localScale =
                new Vector3(
                    1.2f,
                    0.06f,
                    0.03f);

            Material material =
                new Material(Shader.Find("Unlit/Color"));

            material.color =
                i < 6 ? Good : Warning;

            Renderer renderer =
                marker.GetComponent<Renderer>();

            if (renderer != null)
                renderer.sharedMaterial = material;
        }
    }

    private static void BuildCamera()
    {
        GameObject cameraObject =
            new GameObject("BIOME MAP CAMERA");

        Camera camera =
            cameraObject.AddComponent<Camera>();

        cameraObject.transform.position =
            new Vector3(0f, 0f, -15f);

        cameraObject.transform.rotation =
            Quaternion.identity;

        camera.orthographic = true;
        camera.orthographicSize = 9f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = Background;
        camera.depth = -10;

        cameraObject.tag = "MainCamera";
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
            eventSystemObject.GetComponent<InputSystemUIInputModule>();

        module.enabled = true;
    }

    private static Canvas BuildCanvas()
    {
        GameObject canvasObject =
            new GameObject(
                "BIOME HUD CANVAS",
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

        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

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

        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = offsetMin;
        rect.offsetMax = offsetMax;

        Image image =
            go.GetComponent<Image>();

        image.color = PanelColor;

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

        text.text = value;
        text.fontSize = size;
        text.color = Color.white;
        text.alignment = alignment;
        text.raycastTarget = false;
        text.textWrappingMode =
            TextWrappingModes.Normal;

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin =
            new Vector2(10f, 4f);
        rect.offsetMax =
            new Vector2(-10f, -4f);

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
            new Color(0.05f, 0.10f, 0.14f, 0.98f);

        Button button =
            go.GetComponent<Button>();

        ColorBlock colors =
            button.colors;

        colors.normalColor =
            new Color(0.05f, 0.10f, 0.14f, 0.98f);

        colors.highlightedColor =
            new Color(0.08f, 0.22f, 0.30f, 1f);

        colors.pressedColor =
            Accent;

        colors.selectedColor =
            new Color(0.08f, 0.22f, 0.30f, 1f);

        button.colors = colors;

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

        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = offsetMin;
        rect.offsetMax = offsetMax;
    }

    private static void AddLogAction(
        Button button,
        string action)
    {
        button.onClick.AddListener(
            () => Debug.Log(
                "[HUD-207] " + action));
    }

    private static void BuildTopBar(Transform root)
    {
        GameObject panel =
            Panel(
                root,
                "HUD-207-01 TopBar",
                new Vector2(0f, 0.91f),
                new Vector2(1f, 1f),
                Vector2.zero,
                Vector2.zero);

        Label(panel.transform,
            "HUD-207-01A PlanetNameDisplay",
            "PLANET: ACHERON",
            20);

        Label(panel.transform,
            "HUD-207-01B ContinentNameDisplay",
            "CONTINENT: CINDERFALL",
            15);

        Label(panel.transform,
            "HUD-207-01C RegionNameDisplay",
            "REGION: ASHEN FRONTIER",
            15);

        Label(panel.transform,
            "HUD-207-01D BiomeNameDisplay",
            "BIOME: VOLCANIC WASTELAND",
            15);

        Label(panel.transform,
            "HUD-207-01E OwnerFactionEmblem",
            "FACTION // OBSIDIAN COMMAND",
            14);

        Label(panel.transform,
            "HUD-207-01F PlayerResourcesPanel",
            "CREDITS  84,250    ENERGY  72%    SUPPLY  91%",
            14);

        Label(panel.transform,
            "HUD-207-01G TimeControlPanel",
            "TIME  14:32:08    PAUSED",
            14);
    }

    private static void BuildNavigation(Transform root)
    {
        GameObject panel =
            Panel(
                root,
                "HUD-207-02 NavigationPanel",
                new Vector2(0f, 0.36f),
                new Vector2(0.19f, 0.90f),
                new Vector2(12f, 0f),
                new Vector2(-8f, 0f));

        string[] names =
        {
            "HUD-207-02A ReturnToRegionMapButton",
            "HUD-207-02B ReturnToContinentMapButton",
            "HUD-207-02C ReturnToSurfaceMapButton",
            "HUD-207-02D ReturnToPlanetOverviewButton",
            "HUD-207-02E IslandMapButton"
        };

        string[] labels =
        {
            "RETURN TO REGION MAP",
            "RETURN TO CONTINENT MAP",
            "RETURN TO SURFACE MAP",
            "RETURN TO PLANET OVERVIEW",
            "OPEN ISLAND MAP"
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
                new Vector2(0.06f, 0.80f - i * 0.16f),
                new Vector2(0.94f, 0.93f - i * 0.16f),
                Vector2.zero,
                Vector2.zero);

            AddLogAction(
                button,
                labels[i]);
        }
    }

    private static void BuildBiomeLayers(Transform root)
    {
        GameObject panel =
            Panel(
                root,
                "HUD-207-03 BiomeLayersPanel",
                new Vector2(0.19f, 0.42f),
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
            "LayerToggle_BiomeBoundaries",
            "LayerToggle_VegetationDensity",
            "LayerToggle_Traversability"
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
            "BIOME BOUNDARIES",
            "VEGETATION DENSITY",
            "TRAVERSABILITY"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Button button =
                Button(
                    panel.transform,
                    "HUD-207-03" +
                    (char)('A' + i) +
                    " " +
                    names[i],
                    "[ ON ]  " + labels[i]);

            SetRect(
                button.gameObject,
                new Vector2(0.05f, 0.91f - i * 0.075f),
                new Vector2(0.95f, 0.97f - i * 0.075f),
                Vector2.zero,
                Vector2.zero);

            bool state = true;

            button.onClick.AddListener(
                () =>
                {
                    state = !state;
                    button.GetComponentInChildren<TextMeshProUGUI>()
                        .text =
                        (state ? "[ ON ]  " : "[OFF]  ") +
                        labels[i];
                });
        }
    }

    private static void BuildObjectInfo(Transform root)
    {
        GameObject panel =
            Panel(
                root,
                "HUD-207-04 BiomeObjectInfoPanel",
                new Vector2(0.68f, 0.36f),
                new Vector2(1f, 0.90f),
                new Vector2(0f, 0f),
                new Vector2(-12f, 0f));

        string[] fields =
        {
            "HUD-207-04A SelectedObjectName",
            "HUD-207-04B SelectedObjectType",
            "HUD-207-04C SelectedObjectOwnerFaction",
            "HUD-207-04D SelectedObjectStatus",
            "HUD-207-04E SelectedObjectThreatLevel",
            "HUD-207-04F SelectedObjectEnvironmentType",
            "HUD-207-04G SelectedObjectBiomeType"
        };

        string[] values =
        {
            "SELECTED: ASHEN OUTPOST 07",
            "TYPE: FORWARD OPERATING SITE",
            "OWNER: OBSIDIAN COMMAND",
            "STATUS: OPERATIONAL",
            "THREAT: HIGH",
            "ENVIRONMENT: VOLCANIC",
            "BIOME: VOLCANIC WASTELAND"
        };

        for (int i = 0; i < fields.Length; i++)
        {
            Label(
                panel.transform,
                fields[i],
                values[i],
                14);
        }

        Button enter =
            Button(
                panel.transform,
                "HUD-207-04H EnterObjectButton",
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

    private static void BuildTerrainAnalysis(Transform root)
    {
        GameObject panel =
            Panel(
                root,
                "HUD-207-05 TerrainAnalysisPanel",
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
            "VegetationDensityDisplay",
            "WaterSaturationDisplay"
        };

        string[] values =
        {
            "ELEVATION       1,840 m",
            "SLOPE           27%",
            "TRAVERSABILITY  62%",
            "COVER DENSITY   74%",
            "HAZARD ZONES    4 ACTIVE",
            "VEGETATION      81%",
            "WATER SATURATION 18%"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Label(
                panel.transform,
                "HUD-207-05" +
                (char)('A' + i) +
                " " +
                names[i],
                values[i],
                13);
        }
    }

    private static void BuildWarfare(Transform root)
    {
        GameObject panel =
            Panel(
                root,
                "HUD-207-06 WarfarePanel",
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
            "AmbushPotentialDisplay"
        };

        string[] values =
        {
            "WARFARE FOCUS      DEFENSIVE",
            "ENGAGEMENT RANGE   4.2 km",
            "MOBILITY RATING    MODERATE",
            "RECON VISIBILITY   68%",
            "SENSOR INTERFERENCE 31%",
            "AMBUSH POTENTIAL   HIGH"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Label(
                panel.transform,
                "HUD-207-06" +
                (char)('A' + i) +
                " " +
                names[i],
                values[i],
                13);
        }
    }

    private static void BuildQuickActions(Transform root)
    {
        GameObject panel =
            Panel(
                root,
                "HUD-207-07 QuickActionsPanel",
                new Vector2(0.68f, 0.20f),
                new Vector2(0.84f, 0.36f),
                Vector2.zero,
                Vector2.zero);

        string[] names =
        {
            "HUD-207-07A DeployReconButton",
            "HUD-207-07B DeployLogisticsTeamButton",
            "HUD-207-07C OpenIntelNetworkButton",
            "HUD-207-07D OpenDoctrineManagerButton",
            "HUD-207-07E OpenWarfareProfileButton"
        };

        string[] labels =
        {
            "DEPLOY RECON",
            "DEPLOY LOGISTICS",
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
                new Vector2(0.04f, 0.80f - i * 0.17f),
                new Vector2(0.96f, 0.94f - i * 0.17f),
                Vector2.zero,
                Vector2.zero);

            AddLogAction(
                button,
                labels[i]);
        }
    }

    private static void BuildAlerts(Transform root)
    {
        GameObject panel =
            Panel(
                root,
                "HUD-207-08 AlertsPanel",
                new Vector2(0.84f, 0.20f),
                new Vector2(1f, 0.90f),
                Vector2.zero,
                new Vector2(-12f, 0f));

        Label(
            panel.transform,
            "HUD-207-08A StrategicAlertEntry",
            "STRATEGIC // SUPPLY CORRIDOR UNDER REVIEW",
            12);

        Label(
            panel.transform,
            "HUD-207-08B WarfareAlertEntry",
            "WARFARE // HOSTILE PATROL DETECTED",
            12);

        Label(
            panel.transform,
            "HUD-207-08C FactionAlertEntry",
            "FACTION // CONTROL CONTESTED",
            12);

        Label(
            panel.transform,
            "HUD-207-08D ResourceAlertEntry",
            "RESOURCE // RARE MINERAL NODE ACTIVE",
            12);

        Label(
            panel.transform,
            "HUD-207-08E WeatherAlertEntry",
            "WEATHER // ASH STORM APPROACHING",
            12);

        Label(
            panel.transform,
            "HUD-207-08F EnvironmentalHazardAlertEntry",
            "ENVIRONMENT // TOXIC ZONE DETECTED",
            12);
    }

    private static void BuildBottomBar(Transform root)
    {
        GameObject panel =
            Panel(
                root,
                "HUD-207-09 BottomBar",
                new Vector2(0f, 0f),
                new Vector2(1f, 0.085f),
                Vector2.zero,
                Vector2.zero);

        string[] names =
        {
            "HUD-207-09A BackButton",
            "HUD-207-09B ExitToCommandCenterButton",
            "HUD-207-09C SettingsButton",
            "HUD-207-09D HelpButton"
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
                0.02f + i * 0.245f;

            SetRect(
                button.gameObject,
                new Vector2(min, 0.12f),
                new Vector2(min + 0.225f, 0.88f),
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

        foreach (EditorBuildSettingsScene entry in current)
        {
            if (entry.path == scenePath)
                return;
        }

        EditorBuildSettingsScene[] updated =
            new EditorBuildSettingsScene[current.Length + 1];

        Array.Copy(
            current,
            updated,
            current.Length);

        updated[current.Length] =
            new EditorBuildSettingsScene(
                scenePath,
                true);

        EditorBuildSettings.scenes = updated;
    }
}
