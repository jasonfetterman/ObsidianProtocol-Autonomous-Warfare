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

public static class PlanetSurfaceMapHUD204Builder
{
    private const string SceneFolder =
        "Assets/Scenes/SCN63_PLANET_SURFACE_MAP";

    private const string ScenePath =
        SceneFolder + "/Planet_Surface_Map.unity";

    private static readonly Color PanelColor =
        new Color(.025f,.040f,.070f,.97f);

    private static readonly Color Panel2 =
        new Color(.045f,.065f,.100f,.98f);

    private static readonly Color Accent =
        new Color(.15f,.75f,1f,1f);

    private static readonly Color Warning =
        new Color(1f,.65f,.15f,1f);

    private static readonly Color Threat =
        new Color(1f,.22f,.22f,1f);

    private static readonly Color Good =
        new Color(.20f,1f,.45f,1f);

    [MenuItem(
        "Obsidian Protocol/Worlds/BUILD HUD-204 PLANET SURFACE MAP")]
    public static void Build()
    {
        EnsureFolder();

        Scene scene =
            EditorSceneManager.NewScene(
                NewSceneSetup.EmptyScene,
                NewSceneMode.Single);

        BuildEnvironment();
        BuildCanvas();

        EditorSceneManager.SaveScene(
            scene,
            ScenePath);

        AddToBuildSettings(ScenePath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "[HUD-204] PLANET SURFACE MAP BUILD COMPLETE: " +
            ScenePath);
    }

    private static void EnsureFolder()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Scenes"))
            AssetDatabase.CreateFolder("Assets","Scenes");

        if (!AssetDatabase.IsValidFolder(SceneFolder))
            AssetDatabase.CreateFolder(
                "Assets/Scenes",
                "SCN63_PLANET_SURFACE_MAP");
    }

    private static void BuildEnvironment()
    {
        GameObject root =
            new GameObject("PLANET SURFACE MAP ENVIRONMENT");

        GameObject backdrop =
            GameObject.CreatePrimitive(
                PrimitiveType.Quad);

        backdrop.name =
            "SURFACE MAP BACKDROP";

        backdrop.transform.SetParent(
            root.transform);

        backdrop.transform.position =
            new Vector3(0,0,8);

        backdrop.transform.localScale =
            new Vector3(40,22,1);

        Material backdropMat =
            new Material(
                Shader.Find(
                    "Universal Render Pipeline/Unlit"));

        if (backdropMat.shader == null)
            backdropMat =
                new Material(
                    Shader.Find("Unlit/Color"));

        backdropMat.color =
            new Color(.008f,.018f,.032f,1f);

        backdrop.GetComponent<Renderer>()
            .sharedMaterial = backdropMat;

        UnityEngine.Object.DestroyImmediate(
            backdrop.GetComponent<Collider>());

        BuildTerrain(root);
        BuildWaterways(root);
        BuildResourceNodes(root);
        BuildWarfareZones(root);
        BuildLogisticsRoutes(root);

        GameObject cameraGO =
            new GameObject("Planet Surface Map Camera");

        Camera camera =
            cameraGO.AddComponent<Camera>();

        cameraGO.transform.position =
            new Vector3(0,0,-20);

        cameraGO.transform.rotation =
            Quaternion.identity;

        camera.orthographic = true;
        camera.orthographicSize = 10f;
        camera.clearFlags =
            CameraClearFlags.SolidColor;

        camera.backgroundColor =
            new Color(.005f,.012f,.025f,1f);
    }

    private static void BuildTerrain(GameObject root)
    {
        GameObject terrain =
            GameObject.CreatePrimitive(
                PrimitiveType.Quad);

        terrain.name =
            "TERRAIN RELIEF MAP";

        terrain.transform.SetParent(
            root.transform);

        terrain.transform.position =
            new Vector3(0,-.5f,4);

        terrain.transform.localScale =
            new Vector3(15f,8.5f,1f);

        Material material =
            new Material(
                Shader.Find(
                    "Universal Render Pipeline/Unlit"));

        if (material.shader == null)
            material =
                new Material(
                    Shader.Find("Unlit/Color"));

        material.color =
            new Color(.045f,.18f,.16f,1f);

        terrain.GetComponent<Renderer>()
            .sharedMaterial = material;

        UnityEngine.Object.DestroyImmediate(
            terrain.GetComponent<Collider>());

        for (int i = 0; i < 30; i++)
        {
            float x =
                -6.8f + ((i % 10) * 1.48f);

            float y =
                -3.4f + ((i / 10) * 2.2f);

            GameObject region =
                GameObject.CreatePrimitive(
                    PrimitiveType.Quad);

            region.name =
                "TERRAIN_REGION_" +
                (i + 1).ToString("00");

            region.transform.SetParent(
                root.transform);

            region.transform.position =
                new Vector3(x,y,2.8f);

            region.transform.localScale =
                new Vector3(
                    .95f + ((i % 3) * .12f),
                    .62f + ((i % 2) * .10f),
                    1f);

            Material regionMat =
                new Material(
                    Shader.Find(
                        "Universal Render Pipeline/Unlit"));

            if (regionMat.shader == null)
                regionMat =
                    new Material(
                        Shader.Find("Unlit/Color"));

            regionMat.color =
                (i % 5 == 0)
                    ? new Color(.12f,.30f,.22f,1f)
                    : new Color(.07f,.24f,.18f,1f);

            region.GetComponent<Renderer>()
                .sharedMaterial = regionMat;

            UnityEngine.Object.DestroyImmediate(
                region.GetComponent<Collider>());
        }
    }

    private static void BuildWaterways(GameObject root)
    {
        for (int i = 0; i < 9; i++)
        {
            GameObject water =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube);

            water.name =
                "WATERWAY_" +
                (i + 1).ToString("00");

            water.transform.SetParent(
                root.transform);

            water.transform.position =
                new Vector3(
                    -5.5f + i * 1.3f,
                    -2.8f + (i % 4) * 1.65f,
                    1.8f);

            water.transform.rotation =
                Quaternion.Euler(
                    0,0,
                    -18f + i * 7f);

            water.transform.localScale =
                new Vector3(1.35f,.08f,.02f);

            Material mat =
                new Material(
                    Shader.Find(
                        "Universal Render Pipeline/Unlit"));

            if (mat.shader == null)
                mat =
                    new Material(
                        Shader.Find("Unlit/Color"));

            mat.color =
                new Color(.05f,.45f,.85f,1f);

            water.GetComponent<Renderer>()
                .sharedMaterial = mat;

            UnityEngine.Object.DestroyImmediate(
                water.GetComponent<Collider>());
        }
    }

    private static void BuildResourceNodes(GameObject root)
    {
        for (int i = 0; i < 18; i++)
        {
            GameObject node =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cylinder);

            node.name =
                "RESOURCE_NODE_" +
                (i + 1).ToString("00");

            node.transform.SetParent(
                root.transform);

            float angle =
                (Mathf.PI * 2f * i) / 18f;

            float radius =
                2.0f + ((i % 4) * .7f);

            node.transform.position =
                new Vector3(
                    Mathf.Cos(angle) * radius,
                    Mathf.Sin(angle) * radius - .5f,
                    1.2f);

            node.transform.localScale =
                new Vector3(.08f,.08f,.08f);

            Material mat =
                new Material(
                    Shader.Find(
                        "Universal Render Pipeline/Unlit"));

            if (mat.shader == null)
                mat =
                    new Material(
                        Shader.Find("Unlit/Color"));

            mat.color = Accent;

            node.GetComponent<Renderer>()
                .sharedMaterial = mat;

            UnityEngine.Object.DestroyImmediate(
                node.GetComponent<Collider>());
        }
    }

    private static void BuildWarfareZones(GameObject root)
    {
        for (int i = 0; i < 6; i++)
        {
            GameObject zone =
                GameObject.CreatePrimitive(
                    PrimitiveType.Quad);

            zone.name =
                "WARFARE_ZONE_" +
                (i + 1).ToString("00");

            zone.transform.SetParent(
                root.transform);

            zone.transform.position =
                new Vector3(
                    -5.5f + i * 2.2f,
                    2.7f - (i % 2) * 1.1f,
                    1f);

            zone.transform.localScale =
                new Vector3(1.25f,.65f,1f);

            Material mat =
                new Material(
                    Shader.Find(
                        "Universal Render Pipeline/Unlit"));

            if (mat.shader == null)
                mat =
                    new Material(
                        Shader.Find("Unlit/Color"));

            mat.color =
                new Color(1f,.12f,.10f,.22f);

            zone.GetComponent<Renderer>()
                .sharedMaterial = mat;

            UnityEngine.Object.DestroyImmediate(
                zone.GetComponent<Collider>());
        }
    }

    private static void BuildLogisticsRoutes(GameObject root)
    {
        for (int i = 0; i < 7; i++)
        {
            GameObject route =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube);

            route.name =
                "LOGISTICS_ROUTE_" +
                (i + 1).ToString("00");

            route.transform.SetParent(
                root.transform);

            route.transform.position =
                new Vector3(
                    -5f + i * 1.65f,
                    -1.7f + (i % 3) * 1.4f,
                    .9f);

            route.transform.rotation =
                Quaternion.Euler(
                    0,0,
                    8f + i * 4f);

            route.transform.localScale =
                new Vector3(1.05f,.025f,.025f);

            Material mat =
                new Material(
                    Shader.Find(
                        "Universal Render Pipeline/Unlit"));

            if (mat.shader == null)
                mat =
                    new Material(
                        Shader.Find("Unlit/Color"));

            mat.color =
                new Color(.85f,.70f,.18f,1f);

            route.GetComponent<Renderer>()
                .sharedMaterial = mat;

            UnityEngine.Object.DestroyImmediate(
                route.GetComponent<Collider>());
        }
    }

    private static void BuildCanvas()
    {
        GameObject canvasGO =
            new GameObject(
                "HUD-204 PlanetSurfaceMapHUD",
                typeof(RectTransform),
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));

        Canvas canvas =
            canvasGO.GetComponent<Canvas>();

        canvas.renderMode =
            RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler =
            canvasGO.GetComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(1920,1080);

        scaler.matchWidthOrHeight = .5f;

        new GameObject(
            "EventSystem",
            typeof(EventSystem),
            typeof(InputSystemUIInputModule));

        BuildTopBar(canvasGO.transform);
        BuildNavigation(canvasGO.transform);
        BuildSurfaceLayers(canvasGO.transform);
        BuildObjectInfo(canvasGO.transform);
        BuildTerrainAnalysis(canvasGO.transform);
        BuildWarfare(canvasGO.transform);
        BuildQuickActions(canvasGO.transform);
        BuildAlerts(canvasGO.transform);
        BuildBottomBar(canvasGO.transform);
    }

    private static void BuildTopBar(Transform parent)
    {
        GameObject panel =
            Panel(
                parent,
                "HUD-204-01 TopBar",
                new Vector2(.02f,.90f),
                new Vector2(.98f,.985f),
                PanelColor);

        Label(
            panel.transform,
            "HUD-204-01A PlanetNameDisplay",
            "PLN-01 ACHERON",
            new Vector2(.01f,.30f),
            new Vector2(.22f,.88f),
            24,
            Accent);

        Label(
            panel.transform,
            "HUD-204-01B SurfaceTypeDisplay",
            "SURFACE // TEMPERATE CONTINENTAL",
            new Vector2(.23f,.35f),
            new Vector2(.49f,.82f),
            13,
            Color.white);

        GameObject emblem =
            Panel(
                panel.transform,
                "HUD-204-01C OwnerFactionEmblem",
                new Vector2(.55f,.20f),
                new Vector2(.61f,.82f),
                Panel2);

        Label(
            emblem.transform,
            "FACTION",
            "OP",
            new Vector2(.05f,.05f),
            new Vector2(.95f,.95f),
            20,
            Accent);

        Panel(
            panel.transform,
            "HUD-204-01D PlayerResourcesPanel",
            new Vector2(.63f,.15f),
            new Vector2(.82f,.85f),
            Panel2);

        Label(
            panel.transform,
            "RESOURCES",
            "CR 84,250  |  MAT 12,480  |  EN 92%",
            new Vector2(.635f,.28f),
            new Vector2(.815f,.78f),
            12,
            Good);

        Panel(
            panel.transform,
            "HUD-204-01E TimeControlPanel",
            new Vector2(.84f,.15f),
            new Vector2(.985f,.85f),
            Panel2);

        Label(
            panel.transform,
            "TIME",
            "PAUSED   ▶   1X",
            new Vector2(.845f,.28f),
            new Vector2(.98f,.78f),
            12,
            Warning);
    }

    private static void BuildNavigation(Transform parent)
    {
        GameObject panel =
            Panel(
                parent,
                "HUD-204-02 NavigationPanel",
                new Vector2(.02f,.38f),
                new Vector2(.20f,.88f),
                PanelColor);

        string[] names =
        {
            "ReturnToPlanetOverviewButton",
            "ReturnToStarSystemButton",
            "ReturnToGalaxyMapButton",
            "ContinentMapButton",
            "RegionMapButton",
            "BiomeMapButton",
            "IslandMapButton"
        };

        string[] labels =
        {
            "PLANET OVERVIEW",
            "STAR SYSTEM",
            "GALAXY MAP",
            "CONTINENTS",
            "REGIONS",
            "BIOMES",
            "ISLANDS"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Button button =
                Button(
                    panel.transform,
                    "HUD-204-02" +
                    (char)('A' + i) +
                    " " + names[i],
                    labels[i]);

            SetRect(
                button.gameObject,
                new Vector2(
                    .08f,
                    .90f - i * .12f),
                new Vector2(
                    .92f,
                    .985f - i * .12f));
        }
    }

    private static void BuildSurfaceLayers(Transform parent)
    {
        GameObject panel =
            Panel(
                parent,
                "HUD-204-03 SurfaceLayersPanel",
                new Vector2(.77f,.48f),
                new Vector2(.98f,.88f),
                PanelColor);

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
            "LayerToggle_WeatherSystems"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Button button =
                Button(
                    panel.transform,
                    "HUD-204-03" +
                    (char)('A' + i) +
                    " " + names[i],
                    names[i]
                        .Replace(
                            "LayerToggle_",
                            "")
                        .Replace(
                            "_",
                            " "));

            int row = i % 5;
            int col = i / 5;

            SetRect(
                button.gameObject,
                new Vector2(
                    .06f + col * .47f,
                    .77f - row * .15f),
                new Vector2(
                    .45f + col * .47f,
                    .92f - row * .15f));
        }
    }

    private static void BuildObjectInfo(Transform parent)
    {
        GameObject panel =
            Panel(
                parent,
                "HUD-204-04 SurfaceObjectInfoPanel",
                new Vector2(.52f,.29f),
                new Vector2(.75f,.47f),
                PanelColor);

        string[] names =
        {
            "SelectedObjectName",
            "SelectedObjectType",
            "SelectedObjectOwnerFaction",
            "SelectedObjectStatus",
            "SelectedObjectThreatLevel",
            "SelectedObjectEnvironmentType"
        };

        string[] values =
        {
            "ACHERON // NORTHWESTERN REGION",
            "TERRAIN REGION",
            "OWNER // OBSIDIAN PROTOCOL",
            "STATUS // CONTROLLED",
            "THREAT // LOW",
            "ENVIRONMENT // TEMPERATE"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Label(
                panel.transform,
                "HUD-204-04" +
                (char)('A' + i) +
                " " + names[i],
                values[i],
                new Vector2(
                    .06f,
                    .82f - i * .125f),
                new Vector2(
                    .94f,
                    .94f - i * .125f),
                10.5f,
                i == 4
                    ? Warning
                    : Color.white);
        }

        Button enter =
            Button(
                panel.transform,
                "HUD-204-04G EnterObjectButton",
                "ENTER");

        SetRect(
            enter.gameObject,
            new Vector2(.68f,.03f),
            new Vector2(.94f,.19f));
    }

    private static void BuildTerrainAnalysis(Transform parent)
    {
        GameObject panel =
            Panel(
                parent,
                "HUD-204-05 TerrainAnalysisPanel",
                new Vector2(.22f,.53f),
                new Vector2(.50f,.88f),
                PanelColor);

        string[] names =
        {
            "ElevationDisplay",
            "SlopeDisplay",
            "TraversabilityDisplay",
            "CoverDensityDisplay",
            "HazardZonesDisplay"
        };

        string[] values =
        {
            "ELEVATION          420 m",
            "SLOPE              8° AVG",
            "TRAVERSABILITY     81%",
            "COVER DENSITY      64%",
            "HAZARD ZONES       03 ACTIVE"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Label(
                panel.transform,
                "HUD-204-05" +
                (char)('A' + i) +
                " " + names[i],
                values[i],
                new Vector2(
                    .07f,
                    .76f - i * .15f),
                new Vector2(
                    .94f,
                    .92f - i * .15f),
                12,
                i == 4
                    ? Warning
                    : Color.white);
        }
    }

    private static void BuildWarfare(Transform parent)
    {
        GameObject panel =
            Panel(
                parent,
                "HUD-204-06 WarfarePanel",
                new Vector2(.52f,.07f),
                new Vector2(.75f,.27f),
                PanelColor);

        string[] names =
        {
            "WarfareFocusDisplay",
            "EngagementRangeDisplay",
            "MobilityRatingDisplay",
            "ReconVisibilityDisplay",
            "SensorInterferenceDisplay"
        };

        string[] values =
        {
            "WARFARE FOCUS      DEFENSIVE",
            "ENGAGEMENT RANGE   2.8 KM",
            "MOBILITY RATING    HIGH",
            "RECON VISIBILITY   74%",
            "SENSOR INTERFERENCE 12%"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Label(
                panel.transform,
                "HUD-204-06" +
                (char)('A' + i) +
                " " + names[i],
                values[i],
                new Vector2(
                    .05f + (i % 2) * .48f,
                    .68f - (i / 2) * .28f),
                new Vector2(
                    .47f + (i % 2) * .48f,
                    .90f - (i / 2) * .28f),
                10,
                Color.white);
        }
    }

    private static void BuildQuickActions(Transform parent)
    {
        GameObject panel =
            Panel(
                parent,
                "HUD-204-07 QuickActionsPanel",
                new Vector2(.22f,.07f),
                new Vector2(.50f,.50f),
                PanelColor);

        string[] names =
        {
            "DeployReconButton",
            "DeployLogisticsTeamButton",
            "OpenIntelNetworkButton",
            "OpenDoctrineManagerButton",
            "OpenWarfareProfileButton"
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
                    "HUD-204-07" +
                    (char)('A' + i) +
                    " " + names[i],
                    labels[i]);

            SetRect(
                button.gameObject,
                new Vector2(
                    .08f,
                    .78f - i * .15f),
                new Vector2(
                    .92f,
                    .91f - i * .15f));
        }
    }

    private static void BuildAlerts(Transform parent)
    {
        GameObject panel =
            Panel(
                parent,
                "HUD-204-08 AlertsPanel",
                new Vector2(.77f,.07f),
                new Vector2(.98f,.44f),
                PanelColor);

        Label(
            panel.transform,
            "HUD-204-08A StrategicAlertEntry",
            "STRATEGIC // SURFACE CONTROL STABLE",
            new Vector2(.06f,.79f),
            new Vector2(.94f,.94f),
            10.5f,
            Accent);

        Label(
            panel.transform,
            "HUD-204-08B WarfareAlertEntry",
            "WARFARE // 02 ACTIVE ZONES",
            new Vector2(.06f,.61f),
            new Vector2(.94f,.76f),
            10.5f,
            Warning);

        Label(
            panel.transform,
            "HUD-204-08C FactionAlertEntry",
            "FACTION // FRIENDLY CONTROL 87%",
            new Vector2(.06f,.43f),
            new Vector2(.94f,.58f),
            10.5f,
            Good);

        Label(
            panel.transform,
            "HUD-204-08D ResourceAlertEntry",
            "RESOURCE // 18 NODES ACTIVE",
            new Vector2(.06f,.25f),
            new Vector2(.94f,.40f),
            10.5f,
            Color.white);

        Label(
            panel.transform,
            "HUD-204-08E WeatherAlertEntry",
            "WEATHER // CLEAR // 18°C",
            new Vector2(.06f,.07f),
            new Vector2(.94f,.22f),
            10.5f,
            Color.white);
    }

    private static void BuildBottomBar(Transform parent)
    {
        GameObject panel =
            Panel(
                parent,
                "HUD-204-09 BottomBar",
                new Vector2(.22f,.01f),
                new Vector2(.50f,.06f),
                PanelColor);

        string[] names =
        {
            "BackButton",
            "ExitToCommandCenterButton",
            "SettingsButton",
            "HelpButton"
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
                    "HUD-204-09" +
                    (char)('A' + i) +
                    " " + names[i],
                    labels[i]);

            float x0 =
                .02f + i * .245f;

            SetRect(
                button.gameObject,
                new Vector2(x0,.10f),
                new Vector2(x0 + .225f,.90f));
        }
    }

    private static GameObject Panel(
        Transform parent,
        string name,
        Vector2 min,
        Vector2 max,
        Color color)
    {
        GameObject go =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image));

        go.transform.SetParent(
            parent,
            false);

        SetRect(go,min,max);

        go.GetComponent<Image>().color =
            color;

        return go;
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

        go.GetComponent<Image>().color =
            new Color(.06f,.10f,.15f,.98f);

        Button button =
            go.GetComponent<Button>();

        GameObject labelGO =
            new GameObject(
                "LABEL",
                typeof(RectTransform),
                typeof(TextMeshProUGUI));

        labelGO.transform.SetParent(
            go.transform,
            false);

        TextMeshProUGUI text =
            labelGO.GetComponent<TextMeshProUGUI>();

        text.text = label;
        text.alignment =
            TextAlignmentOptions.Center;

        text.fontSize = 11;
        text.color = Color.white;
        text.raycastTarget = false;
        text.textWrappingMode =
            TextWrappingModes.Normal;

        RectTransform rect =
            labelGO.GetComponent<RectTransform>();

        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        return button;
    }

    private static TextMeshProUGUI Label(
        Transform parent,
        string name,
        string value,
        Vector2 min,
        Vector2 max,
        float size,
        Color color)
    {
        GameObject go =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(TextMeshProUGUI));

        go.transform.SetParent(
            parent,
            false);

        SetRect(go,min,max);

        TextMeshProUGUI text =
            go.GetComponent<TextMeshProUGUI>();

        text.text = value;
        text.alignment =
            TextAlignmentOptions.Left;

        text.fontSize = size;
        text.color = color;
        text.raycastTarget = false;
        text.textWrappingMode =
            TextWrappingModes.Normal;

        return text;
    }

    private static void SetRect(
        GameObject go,
        Vector2 min,
        Vector2 max)
    {
        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    private static void AddToBuildSettings(
        string scenePath)
    {
        List<EditorBuildSettingsScene> scenes =
            new List<EditorBuildSettingsScene>(
                EditorBuildSettings.scenes);

        foreach (
            EditorBuildSettingsScene scene
            in scenes)
        {
            if (scene.path == scenePath)
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
