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

public static class ContinentMapHUD205Builder
{
    private const string ScenePath =
        "Assets/Scenes/SCN64_CONTINENT_MAP/Continent_Map.unity";

    private static readonly Color PanelColor =
        new Color(.025f, .040f, .070f, .97f);

    private static readonly Color Panel2 =
        new Color(.045f, .065f, .100f, .98f);

    private static readonly Color Accent =
        new Color(.15f, .75f, 1f, 1f);

    private static readonly Color Warning =
        new Color(1f, .65f, .15f, 1f);

    private static readonly Color Threat =
        new Color(1f, .22f, .22f, 1f);

    private static readonly Color Good =
        new Color(.20f, 1f, .45f, 1f);

    [MenuItem("Obsidian Protocol/Worlds/BUILD HUD-205 CONTINENT MAP")]
    public static void Build()
    {
        EnsureFolder("Assets/Scenes");
        EnsureFolder("Assets/Scenes/SCN64_CONTINENT_MAP");

        Scene scene = EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene,
            NewSceneMode.Single);

        BuildEnvironment();
        BuildEventSystem();
        BuildHUD();

        AddToBuildSettings();

        EditorSceneManager.SaveScene(scene, ScenePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Selection.activeObject = GameObject.Find("HUD-205 ContinentMapHUD");

        Debug.Log(
            "[HUD-205] CONTINENT MAP BUILD COMPLETE: " + ScenePath);
    }

    private static void EnsureFolder(string path)
    {
        if (AssetDatabase.IsValidFolder(path))
            return;

        string parent = System.IO.Path.GetDirectoryName(path)
            .Replace("\\", "/");

        string folder = System.IO.Path.GetFileName(path);

        if (!string.IsNullOrEmpty(parent) &&
            !AssetDatabase.IsValidFolder(parent))
        {
            EnsureFolder(parent);
        }

        AssetDatabase.CreateFolder(parent, folder);
    }

    // ============================================================
    // ENVIRONMENT
    // ============================================================

    private static void BuildEnvironment()
    {
        GameObject root =
            new GameObject("CONTINENT MAP ENVIRONMENT");

        GameObject backdrop =
            GameObject.CreatePrimitive(PrimitiveType.Quad);

        backdrop.name = "CONTINENT MAP BACKDROP";
        backdrop.transform.SetParent(root.transform);

        backdrop.transform.position =
            new Vector3(0f, 0f, 10f);

        backdrop.transform.localScale =
            new Vector3(28f, 18f, 1f);

        Renderer backdropRenderer =
            backdrop.GetComponent<Renderer>();

        if (backdropRenderer != null)
        {
            Material mat =
                new Material(Shader.Find("Unlit/Color"));

            mat.color =
                new Color(.008f, .018f, .032f, 1f);

            backdropRenderer.sharedMaterial = mat;
        }

        GameObject terrain =
            GameObject.CreatePrimitive(PrimitiveType.Quad);

        terrain.name = "CONTINENT TERRAIN MAP";
        terrain.transform.SetParent(root.transform);

        terrain.transform.position =
            new Vector3(0f, 0f, 8f);

        terrain.transform.localScale =
            new Vector3(20f, 12f, 1f);

        Renderer terrainRenderer =
            terrain.GetComponent<Renderer>();

        if (terrainRenderer != null)
        {
            Material mat =
                new Material(Shader.Find("Unlit/Color"));

            mat.color =
                new Color(.055f, .115f, .105f, 1f);

            terrainRenderer.sharedMaterial = mat;
        }

        BuildTerrainRegions(root.transform);
        BuildWaterways(root.transform);
        BuildResourceNodes(root.transform);
        BuildWarfareZones(root.transform);
        BuildLogisticsRoutes(root.transform);
        BuildWeatherSystems(root.transform);

        GameObject cameraObject =
            new GameObject("CONTINENT MAP CAMERA");

        Camera cam =
            cameraObject.AddComponent<Camera>();

        cam.orthographic = true;
        cam.orthographicSize = 9f;
        cam.transform.position =
            new Vector3(0f, 0f, -10f);

        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor =
            new Color(.005f, .012f, .022f, 1f);

        cameraObject.tag = "MainCamera";
    }

    private static void BuildTerrainRegions(Transform parent)
    {
        for (int i = 0; i < 24; i++)
        {
            GameObject region =
                GameObject.CreatePrimitive(PrimitiveType.Quad);

            region.name =
                "CONTINENT_TERRAIN_REGION_" +
                (i + 1).ToString("00");

            region.transform.SetParent(parent);

            float angle =
                (Mathf.PI * 2f / 24f) * i;

            float radius =
                2.5f + ((i % 4) * 1.1f);

            region.transform.position =
                new Vector3(
                    Mathf.Cos(angle) * radius,
                    Mathf.Sin(angle) * radius * .55f,
                    6.5f);

            region.transform.localScale =
                new Vector3(
                    1.2f + ((i % 3) * .35f),
                    .8f + ((i % 2) * .25f),
                    1f);

            Renderer r =
                region.GetComponent<Renderer>();

            if (r != null)
            {
                Material m =
                    new Material(Shader.Find("Unlit/Color"));

                float shade =
                    .08f + ((i % 5) * .025f);

                m.color =
                    new Color(
                        shade,
                        .18f + ((i % 4) * .025f),
                        .12f,
                        1f);

                r.sharedMaterial = m;
            }
        }
    }

    private static void BuildWaterways(Transform parent)
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject water =
                GameObject.CreatePrimitive(PrimitiveType.Cube);

            water.name =
                "WATERWAY_" +
                (i + 1).ToString("00");

            water.transform.SetParent(parent);

            float x =
                -7f + (i * 1.45f);

            water.transform.position =
                new Vector3(
                    x,
                    Mathf.Sin(i * .8f) * 1.8f,
                    5.5f);

            water.transform.localScale =
                new Vector3(.12f, 3.5f, .04f);

            water.transform.rotation =
                Quaternion.Euler(
                    0f,
                    0f,
                    -20f + (i % 3) * 20f);

            Renderer r =
                water.GetComponent<Renderer>();

            if (r != null)
            {
                Material m =
                    new Material(Shader.Find("Unlit/Color"));

                m.color =
                    new Color(.05f, .45f, .85f, 1f);

                r.sharedMaterial = m;
            }
        }
    }

    private static void BuildResourceNodes(Transform parent)
    {
        for (int i = 0; i < 14; i++)
        {
            GameObject node =
                GameObject.CreatePrimitive(PrimitiveType.Cylinder);

            node.name =
                "RESOURCE_NODE_" +
                (i + 1).ToString("00");

            node.transform.SetParent(parent);

            float angle =
                (Mathf.PI * 2f / 14f) * i;

            node.transform.position =
                new Vector3(
                    Mathf.Cos(angle) * 6f,
                    Mathf.Sin(angle) * 3.2f,
                    4.5f);

            node.transform.localScale =
                new Vector3(.12f, .08f, .12f);

            Renderer r =
                node.GetComponent<Renderer>();

            if (r != null)
            {
                Material m =
                    new Material(Shader.Find("Unlit/Color"));

                m.color = Good;
                r.sharedMaterial = m;
            }
        }
    }

    private static void BuildWarfareZones(Transform parent)
    {
        for (int i = 0; i < 6; i++)
        {
            GameObject zone =
                GameObject.CreatePrimitive(PrimitiveType.Cube);

            zone.name =
                "WARFARE_ZONE_" +
                (i + 1).ToString("00");

            zone.transform.SetParent(parent);

            zone.transform.position =
                new Vector3(
                    -5f + i * 2f,
                    2.5f - (i % 2) * 4.5f,
                    4f);

            zone.transform.localScale =
                new Vector3(
                    1.5f,
                    .05f,
                    1.2f);

            Renderer r =
                zone.GetComponent<Renderer>();

            if (r != null)
            {
                Material m =
                    new Material(Shader.Find("Unlit/Color"));

                m.color =
                    new Color(1f, .12f, .12f, .45f);

                r.sharedMaterial = m;
            }
        }
    }

    private static void BuildLogisticsRoutes(Transform parent)
    {
        for (int i = 0; i < 7; i++)
        {
            GameObject route =
                GameObject.CreatePrimitive(PrimitiveType.Cube);

            route.name =
                "LOGISTICS_ROUTE_" +
                (i + 1).ToString("00");

            route.transform.SetParent(parent);

            route.transform.position =
                new Vector3(
                    -6f + i * 2f,
                    Mathf.Cos(i) * 2f,
                    3.5f);

            route.transform.localScale =
                new Vector3(
                    1.7f,
                    .035f,
                    .035f);

            route.transform.rotation =
                Quaternion.Euler(
                    0f,
                    0f,
                    15f);

            Renderer r =
                route.GetComponent<Renderer>();

            if (r != null)
            {
                Material m =
                    new Material(Shader.Find("Unlit/Color"));

                m.color = Accent;
                r.sharedMaterial = m;
            }
        }
    }

    private static void BuildWeatherSystems(Transform parent)
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject weather =
                GameObject.CreatePrimitive(PrimitiveType.Sphere);

            weather.name =
                "WEATHER_SYSTEM_" +
                (i + 1).ToString("00");

            weather.transform.SetParent(parent);

            weather.transform.position =
                new Vector3(
                    -6f + i * 3f,
                    4.5f - (i % 2) * 1.5f,
                    3f);

            weather.transform.localScale =
                Vector3.one * .45f;

            Renderer r =
                weather.GetComponent<Renderer>();

            if (r != null)
            {
                Material m =
                    new Material(Shader.Find("Unlit/Color"));

                m.color =
                    new Color(.45f, .65f, 1f, .35f);

                r.sharedMaterial = m;
            }
        }
    }

    // ============================================================
    // EVENT SYSTEM
    // ============================================================

    private static void BuildEventSystem()
    {
        GameObject es =
            new GameObject(
                "EventSystem",
                typeof(EventSystem),
                typeof(InputSystemUIInputModule));

        // Unity manages EventSystem.current automatically.
    }

    // ============================================================
    // HUD
    // ============================================================

    private static void BuildHUD()
    {
        GameObject canvasObject =
            new GameObject(
                "HUD-205 ContinentMapHUD",
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

        scaler.matchWidthOrHeight = .5f;

        BuildTopBar(canvasObject.transform);
        BuildNavigation(canvasObject.transform);
        BuildLayers(canvasObject.transform);
        BuildObjectInfo(canvasObject.transform);
        BuildTerrainAnalysis(canvasObject.transform);
        BuildWarfare(canvasObject.transform);
        BuildQuickActions(canvasObject.transform);
        BuildAlerts(canvasObject.transform);
        BuildBottomBar(canvasObject.transform);
    }

    // ============================================================
    // TOP BAR
    // ============================================================

    private static void BuildTopBar(Transform parent)
    {
        GameObject panel =
            Panel(
                "HUD-205-01 TopBar",
                parent,
                new Vector2(0f, .88f),
                new Vector2(1f, 1f));

        Label(
            "HUD-205-01A PlanetNameDisplay",
            panel.transform,
            "PLANET: ACHERON",
            new Vector2(.01f, .25f),
            new Vector2(.25f, .85f),
            22,
            Accent);

        Label(
            "HUD-205-01B ContinentNameDisplay",
            panel.transform,
            "CONTINENT: CINDERFALL",
            new Vector2(.26f, .25f),
            new Vector2(.52f, .85f),
            20,
            Color.white);

        Label(
            "HUD-205-01C OwnerFactionEmblem",
            panel.transform,
            "FACTION // OBSIDIAN",
            new Vector2(.53f, .25f),
            new Vector2(.68f, .85f),
            16,
            Good);

        Label(
            "HUD-205-01D PlayerResourcesPanel",
            panel.transform,
            "CREDITS  84,250   |   MAT  12,480   |   ENERGY  8,920",
            new Vector2(.68f, .25f),
            new Vector2(.90f, .85f),
            13,
            Color.white);

        Label(
            "HUD-205-01E TimeControlPanel",
            panel.transform,
            "PAUSED  |  1X  2X  4X",
            new Vector2(.90f, .25f),
            new Vector2(.995f, .85f),
            12,
            Warning);
    }

    // ============================================================
    // NAVIGATION
    // ============================================================

    private static void BuildNavigation(Transform parent)
    {
        GameObject panel =
            Panel(
                "HUD-205-02 NavigationPanel",
                parent,
                new Vector2(.01f, .38f),
                new Vector2(.19f, .86f));

        string[] labels =
        {
            "PLANET OVERVIEW",
            "SURFACE MAP",
            "REGION MAP",
            "BIOME MAP",
            "ISLAND MAP"
        };

        string[] names =
        {
            "HUD-205-02A ReturnToPlanetOverviewButton",
            "HUD-205-02B ReturnToSurfaceMapButton",
            "HUD-205-02C RegionMapButton",
            "HUD-205-02D BiomeMapButton",
            "HUD-205-02E IslandMapButton"
        };

        for (int i = 0; i < labels.Length; i++)
        {
            Button b =
                Button(
                    names[i],
                    panel.transform,
                    labels[i],
                    new Vector2(.08f, .80f - i * .15f),
                    new Vector2(.92f, .92f - i * .15f));

            b.gameObject.name = names[i];
        }
    }

    // ============================================================
    // LAYERS
    // ============================================================

    private static void BuildLayers(Transform parent)
    {
        GameObject panel =
            Panel(
                "HUD-205-03 ContinentLayersPanel",
                parent,
                new Vector2(.01f, .04f),
                new Vector2(.19f, .36f));

        string[] names =
        {
            "HUD-205-03A LayerToggle_Terrain",
            "HUD-205-03B LayerToggle_Elevation",
            "HUD-205-03C LayerToggle_Heatmap",
            "HUD-205-03D LayerToggle_Waterways",
            "HUD-205-03E LayerToggle_FactionControl",
            "HUD-205-03F LayerToggle_WarfareZones",
            "HUD-205-03G LayerToggle_ResourceNodes",
            "HUD-205-03H LayerToggle_LogisticsRoutes",
            "HUD-205-03I LayerToggle_WeatherSystems"
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
            "WEATHER SYSTEMS"
        };

        for (int i = 0; i < names.Length; i++)
        {
            int col = i / 5;
            int row = i % 5;

            float x0 = .05f + col * .47f;
            float x1 = x0 + .42f;

            float y1 = .93f - row * .18f;
            float y0 = y1 - .13f;

            Button(
                names[i],
                panel.transform,
                labels[i],
                new Vector2(x0, y0),
                new Vector2(x1, y1));
        }
    }

    // ============================================================
    // OBJECT INFO
    // ============================================================

    private static void BuildObjectInfo(Transform parent)
    {
        GameObject panel =
            Panel(
                "HUD-205-04 ContinentObjectInfoPanel",
                parent,
                new Vector2(.80f, .42f),
                new Vector2(.99f, .86f));

        string[] names =
        {
            "HUD-205-04A SelectedObjectName",
            "HUD-205-04B SelectedObjectType",
            "HUD-205-04C SelectedObjectOwnerFaction",
            "HUD-205-04D SelectedObjectStatus",
            "HUD-205-04E SelectedObjectThreatLevel",
            "HUD-205-04F SelectedObjectEnvironmentType"
        };

        string[] values =
        {
            "SELECTED: CINDERFALL OUTPOST",
            "TYPE: MILITARY INSTALLATION",
            "OWNER: OBSIDIAN PROTOCOL",
            "STATUS: OPERATIONAL",
            "THREAT: HIGH",
            "ENVIRONMENT: TEMPERATE HIGHLANDS"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Label(
                names[i],
                panel.transform,
                values[i],
                new Vector2(.06f, .84f - i * .105f),
                new Vector2(.94f, .95f - i * .105f),
                12,
                i == 4 ? Threat : Color.white);
        }

        Button(
            "HUD-205-04G EnterObjectButton",
            panel.transform,
            "ENTER OBJECT",
            new Vector2(.20f, .04f),
            new Vector2(.80f, .14f));
    }

    // ============================================================
    // TERRAIN ANALYSIS
    // ============================================================

    private static void BuildTerrainAnalysis(Transform parent)
    {
        GameObject panel =
            Panel(
                "HUD-205-05 TerrainAnalysisPanel",
                parent,
                new Vector2(.80f, .20f),
                new Vector2(.99f, .40f));

        string[] names =
        {
            "HUD-205-05A ElevationDisplay",
            "HUD-205-05B SlopeDisplay",
            "HUD-205-05C TraversabilityDisplay",
            "HUD-205-05D CoverDensityDisplay",
            "HUD-205-05E HazardZonesDisplay"
        };

        string[] values =
        {
            "ELEVATION: 1,840 m",
            "SLOPE: 18%",
            "TRAVERSABILITY: 72%",
            "COVER DENSITY: 64%",
            "HAZARD ZONES: 5 ACTIVE"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Label(
                names[i],
                panel.transform,
                values[i],
                new Vector2(.06f, .80f - i * .16f),
                new Vector2(.94f, .94f - i * .16f),
                12,
                i == 4 ? Warning : Color.white);
        }
    }

    // ============================================================
    // WARFARE
    // ============================================================

    private static void BuildWarfare(Transform parent)
    {
        GameObject panel =
            Panel(
                "HUD-205-06 WarfarePanel",
                parent,
                new Vector2(.80f, .03f),
                new Vector2(.99f, .18f));

        string[] names =
        {
            "HUD-205-06A WarfareFocusDisplay",
            "HUD-205-06B EngagementRangeDisplay",
            "HUD-205-06C MobilityRatingDisplay",
            "HUD-205-06D ReconVisibilityDisplay",
            "HUD-205-06E SensorInterferenceDisplay"
        };

        string[] values =
        {
            "FOCUS: GROUND CONTROL",
            "ENGAGEMENT RANGE: 4.2 km",
            "MOBILITY: MEDIUM",
            "RECON VISIBILITY: 68%",
            "SENSOR INTERFERENCE: 21%"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Label(
                names[i],
                panel.transform,
                values[i],
                new Vector2(.04f, .76f - i * .18f),
                new Vector2(.96f, .92f - i * .18f),
                11,
                Color.white);
        }
    }

    // ============================================================
    // QUICK ACTIONS
    // ============================================================

    private static void BuildQuickActions(Transform parent)
    {
        GameObject panel =
            Panel(
                "HUD-205-07 QuickActionsPanel",
                parent,
                new Vector2(.20f, .03f),
                new Vector2(.44f, .20f));

        string[] names =
        {
            "HUD-205-07A DeployReconButton",
            "HUD-205-07B DeployLogisticsTeamButton",
            "HUD-205-07C OpenIntelNetworkButton",
            "HUD-205-07D OpenDoctrineManagerButton",
            "HUD-205-07E OpenWarfareProfileButton"
        };

        string[] labels =
        {
            "DEPLOY RECON",
            "DEPLOY LOGISTICS",
            "INTEL NETWORK",
            "DOCTRINE MANAGER",
            "WARFARE PROFILE"
        };

        for (int i = 0; i < names.Length; i++)
        {
            int row = i / 3;
            int col = i % 3;

            float x0 = .04f + col * .32f;
            float x1 = x0 + .29f;

            float y1 = .91f - row * .42f;
            float y0 = y1 - .32f;

            Button(
                names[i],
                panel.transform,
                labels[i],
                new Vector2(x0, y0),
                new Vector2(x1, y1));
        }
    }

    // ============================================================
    // ALERTS
    // ============================================================

    private static void BuildAlerts(Transform parent)
    {
        GameObject panel =
            Panel(
                "HUD-205-08 AlertsPanel",
                parent,
                new Vector2(.45f, .03f),
                new Vector2(.79f, .20f));

        string[] names =
        {
            "HUD-205-08A StrategicAlertEntry",
            "HUD-205-08B WarfareAlertEntry",
            "HUD-205-08C FactionAlertEntry",
            "HUD-205-08D ResourceAlertEntry",
            "HUD-205-08E WeatherAlertEntry"
        };

        string[] values =
        {
            "STRATEGIC // CONTINENTAL ACTIVITY ELEVATED",
            "WARFARE // HOSTILE PATROLS DETECTED",
            "FACTION // CONTROL LINE SHIFT DETECTED",
            "RESOURCE // EXTRACTION OUTPUT +8%",
            "WEATHER // STORM FRONT APPROACHING"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Label(
                names[i],
                panel.transform,
                values[i],
                new Vector2(.05f, .80f - i * .17f),
                new Vector2(.95f, .94f - i * .17f),
                10,
                i == 1 ? Threat :
                i == 4 ? Warning :
                Color.white);
        }
    }

    // ============================================================
    // BOTTOM BAR
    // ============================================================

    private static void BuildBottomBar(Transform parent)
    {
        GameObject panel =
            Panel(
                "HUD-205-09 BottomBar",
                parent,
                new Vector2(.20f, .22f),
                new Vector2(.79f, .30f));

        Button(
            "HUD-205-09A BackButton",
            panel.transform,
            "BACK",
            new Vector2(.02f, .15f),
            new Vector2(.22f, .85f));

        Button(
            "HUD-205-09B ExitToCommandCenterButton",
            panel.transform,
            "COMMAND CENTER",
            new Vector2(.27f, .15f),
            new Vector2(.52f, .85f));

        Button(
            "HUD-205-09C SettingsButton",
            panel.transform,
            "SETTINGS",
            new Vector2(.57f, .15f),
            new Vector2(.76f, .85f));

        Button(
            "HUD-205-09D HelpButton",
            panel.transform,
            "HELP",
            new Vector2(.81f, .15f),
            new Vector2(.98f, .85f));
    }

    // ============================================================
    // UI HELPERS
    // ============================================================

    private static GameObject Panel(
        string name,
        Transform parent,
        Vector2 anchorMin,
        Vector2 anchorMax)
    {
        GameObject go =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image));

        go.transform.SetParent(parent, false);

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image image =
            go.GetComponent<Image>();

        image.color = PanelColor;

        return go;
    }

    private static void Label(
        string name,
        Transform parent,
        string value,
        Vector2 anchorMin,
        Vector2 anchorMax,
        float fontSize,
        Color color)
    {
        GameObject go =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(TextMeshProUGUI));

        go.transform.SetParent(parent, false);

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        TextMeshProUGUI text =
            go.GetComponent<TextMeshProUGUI>();

        text.text = value;
        text.fontSize = fontSize;
        text.color = color;
        text.alignment = TextAlignmentOptions.Left;
        text.raycastTarget = false;
        text.textWrappingMode =
            TextWrappingModes.Normal;
    }

    private static Button Button(
        string name,
        Transform parent,
        string label,
        Vector2 anchorMin,
        Vector2 anchorMax)
    {
        GameObject go =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image),
                typeof(Button));

        go.transform.SetParent(parent, false);

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image image =
            go.GetComponent<Image>();

        image.color = Panel2;

        Button button =
            go.GetComponent<Button>();

        button.targetGraphic = image;

        GameObject textObject =
            new GameObject(
                "LABEL",
                typeof(RectTransform),
                typeof(TextMeshProUGUI));

        textObject.transform.SetParent(
            go.transform,
            false);

        TextMeshProUGUI text =
            textObject.GetComponent<TextMeshProUGUI>();

        if (text == null)
            throw new Exception(
                "Failed to create TextMeshProUGUI for button: " +
                name);

        text.text = label;
        text.alignment =
            TextAlignmentOptions.Center;

        text.fontSize = 13;
        text.color = Color.white;
        text.raycastTarget = false;
        text.textWrappingMode =
            TextWrappingModes.Normal;

        RectTransform tr =
            textObject.GetComponent<RectTransform>();

        tr.anchorMin = Vector2.zero;
        tr.anchorMax = Vector2.one;
        tr.offsetMin = Vector2.zero;
        tr.offsetMax = Vector2.zero;

        return button;
    }

    // ============================================================
    // BUILD SETTINGS
    // ============================================================

    private static void AddToBuildSettings()
    {
        List<EditorBuildSettingsScene> scenes =
            new List<EditorBuildSettingsScene>(
                EditorBuildSettings.scenes);

        bool found = false;

        for (int i = 0; i < scenes.Count; i++)
        {
            if (scenes[i].path == ScenePath)
            {
                scenes[i].enabled = true;
                found = true;
                break;
            }
        }

        if (!found)
        {
            scenes.Add(
                new EditorBuildSettingsScene(
                    ScenePath,
                    true));
        }

        EditorBuildSettings.scenes =
            scenes.ToArray();
    }
}

