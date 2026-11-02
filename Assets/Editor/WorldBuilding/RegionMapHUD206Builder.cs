using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using TMPro;

public static class RegionMapHUD206Builder
{
    private const string ScenePath =
        "Assets/Scenes/SCN65_REGION_MAP/Region_Map.unity";

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

    [MenuItem("Obsidian Protocol/Worlds/BUILD HUD-206 REGION MAP")]
    public static void Build()
    {
        EnsureFolder("Assets/Scenes");
        EnsureFolder("Assets/Scenes/SCN65_REGION_MAP");

        Scene scene =
            EditorSceneManager.NewScene(
                NewSceneSetup.EmptyScene,
                NewSceneMode.Single);

        BuildEnvironment();
        BuildEventSystem();
        BuildHUD();

        AddToBuildSettings();

        EditorSceneManager.SaveScene(
            scene,
            ScenePath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "[HUD-206] REGION MAP BUILD COMPLETE: " +
            ScenePath);
    }

    private static void EnsureFolder(string path)
    {
        if (AssetDatabase.IsValidFolder(path))
            return;

        string parent =
            System.IO.Path.GetDirectoryName(path)
            .Replace("\\","/");

        string folder =
            System.IO.Path.GetFileName(path);

        if (!string.IsNullOrEmpty(parent) &&
            !AssetDatabase.IsValidFolder(parent))
        {
            EnsureFolder(parent);
        }

        AssetDatabase.CreateFolder(parent,folder);
    }

    // ============================================================
    // ENVIRONMENT
    // ============================================================

    private static void BuildEnvironment()
    {
        GameObject root =
            new GameObject("REGION MAP ENVIRONMENT");

        CreateMapQuad(
            root.transform,
            "REGION MAP BACKDROP",
            new Vector3(0f,0f,10f),
            new Vector3(28f,18f,1f),
            new Color(.008f,.018f,.032f,1f));

        CreateMapQuad(
            root.transform,
            "REGION TERRAIN MAP",
            new Vector3(0f,0f,8f),
            new Vector3(20f,12f,1f),
            new Color(.055f,.115f,.105f,1f));

        BuildTerrain(root.transform);
        BuildWaterways(root.transform);
        BuildResourceNodes(root.transform);
        BuildWarfareZones(root.transform);
        BuildLogisticsRoutes(root.transform);
        BuildWeather(root.transform);
        BuildBiomeBoundaries(root.transform);

        GameObject cameraObject =
            new GameObject("REGION MAP CAMERA");

        Camera camera =
            cameraObject.AddComponent<Camera>();

        camera.orthographic = true;
        camera.orthographicSize = 9f;
        camera.transform.position =
            new Vector3(0f,0f,-10f);

        camera.clearFlags =
            CameraClearFlags.SolidColor;

        camera.backgroundColor =
            new Color(.005f,.012f,.022f,1f);

        cameraObject.tag = "MainCamera";
    }

    private static void CreateMapQuad(
        Transform parent,
        string name,
        Vector3 position,
        Vector3 scale,
        Color color)
    {
        GameObject go =
            GameObject.CreatePrimitive(
                PrimitiveType.Quad);

        go.name = name;
        go.transform.SetParent(parent);
        go.transform.position = position;
        go.transform.localScale = scale;

        Renderer renderer =
            go.GetComponent<Renderer>();

        if (renderer != null)
        {
            Material material =
                new Material(
                    Shader.Find("Unlit/Color"));

            material.color = color;
            renderer.sharedMaterial = material;
        }
    }

    private static void BuildTerrain(Transform parent)
    {
        for (int i=0;i<30;i++)
        {
            GameObject region =
                GameObject.CreatePrimitive(
                    PrimitiveType.Quad);

            region.name =
                "REGION_TERRAIN_" +
                (i+1).ToString("00");

            region.transform.SetParent(parent);

            float angle =
                (Mathf.PI*2f/30f)*i;

            float radius =
                1.5f+(i%5)*1.15f;

            region.transform.position =
                new Vector3(
                    Mathf.Cos(angle)*radius,
                    Mathf.Sin(angle)*radius*.58f,
                    6.5f);

            region.transform.localScale =
                new Vector3(
                    .9f+(i%3)*.25f,
                    .65f+(i%4)*.15f,
                    1f);

            Renderer renderer =
                region.GetComponent<Renderer>();

            if (renderer != null)
            {
                Material material =
                    new Material(
                        Shader.Find("Unlit/Color"));

                float shade =
                    .07f+(i%5)*.018f;

                material.color =
                    new Color(
                        shade,
                        .16f+(i%4)*.025f,
                        .10f,
                        1f);

                renderer.sharedMaterial =
                    material;
            }
        }
    }

    private static void BuildWaterways(Transform parent)
    {
        for (int i=0;i<12;i++)
        {
            GameObject water =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube);

            water.name =
                "REGION_WATERWAY_" +
                (i+1).ToString("00");

            water.transform.SetParent(parent);

            water.transform.position =
                new Vector3(
                    -6.5f+i*1.2f,
                    Mathf.Sin(i*.9f)*2.3f,
                    5.5f);

            water.transform.localScale =
                new Vector3(.10f,3.2f,.035f);

            water.transform.rotation =
                Quaternion.Euler(
                    0f,0f,
                    -25f+(i%4)*15f);

            Renderer renderer =
                water.GetComponent<Renderer>();

            if (renderer != null)
            {
                Material material =
                    new Material(
                        Shader.Find("Unlit/Color"));

                material.color =
                    new Color(.04f,.42f,.85f,1f);

                renderer.sharedMaterial =
                    material;
            }
        }
    }

    private static void BuildResourceNodes(Transform parent)
    {
        for (int i=0;i<18;i++)
        {
            GameObject node =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cylinder);

            node.name =
                "REGION_RESOURCE_NODE_" +
                (i+1).ToString("00");

            node.transform.SetParent(parent);

            float angle =
                Mathf.PI*2f/18f*i;

            node.transform.position =
                new Vector3(
                    Mathf.Cos(angle)*6f,
                    Mathf.Sin(angle)*3.5f,
                    4.5f);

            node.transform.localScale =
                new Vector3(.13f,.08f,.13f);

            Renderer renderer =
                node.GetComponent<Renderer>();

            if (renderer != null)
            {
                Material material =
                    new Material(
                        Shader.Find("Unlit/Color"));

                material.color = Good;
                renderer.sharedMaterial =
                    material;
            }
        }
    }

    private static void BuildWarfareZones(Transform parent)
    {
        for (int i=0;i<8;i++)
        {
            GameObject zone =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube);

            zone.name =
                "REGION_WARFARE_ZONE_" +
                (i+1).ToString("00");

            zone.transform.SetParent(parent);

            zone.transform.position =
                new Vector3(
                    -6f+(i%4)*4f,
                    i<4 ? 3.2f : -3.2f,
                    4f);

            zone.transform.localScale =
                new Vector3(
                    1.45f,.045f,1.1f);

            Renderer renderer =
                zone.GetComponent<Renderer>();

            if (renderer != null)
            {
                Material material =
                    new Material(
                        Shader.Find("Unlit/Color"));

                material.color =
                    new Color(1f,.12f,.12f,.45f);

                renderer.sharedMaterial =
                    material;
            }
        }
    }

    private static void BuildLogisticsRoutes(Transform parent)
    {
        for (int i=0;i<9;i++)
        {
            GameObject route =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube);

            route.name =
                "REGION_LOGISTICS_ROUTE_" +
                (i+1).ToString("00");

            route.transform.SetParent(parent);

            route.transform.position =
                new Vector3(
                    -6f+(i%5)*3f,
                    -1.5f+(i%3)*1.3f,
                    3.5f);

            route.transform.localScale =
                new Vector3(
                    1.5f,.035f,.035f);

            route.transform.rotation =
                Quaternion.Euler(
                    0f,0f,
                    -15f+(i%4)*10f);

            Renderer renderer =
                route.GetComponent<Renderer>();

            if (renderer != null)
            {
                Material material =
                    new Material(
                        Shader.Find("Unlit/Color"));

                material.color = Accent;
                renderer.sharedMaterial =
                    material;
            }
        }
    }

    private static void BuildWeather(Transform parent)
    {
        for (int i=0;i<6;i++)
        {
            GameObject weather =
                GameObject.CreatePrimitive(
                    PrimitiveType.Sphere);

            weather.name =
                "REGION_WEATHER_SYSTEM_" +
                (i+1).ToString("00");

            weather.transform.SetParent(parent);

            weather.transform.position =
                new Vector3(
                    -6f+i*2.4f,
                    4.2f-(i%3)*1.2f,
                    3f);

            weather.transform.localScale =
                Vector3.one*.42f;

            Renderer renderer =
                weather.GetComponent<Renderer>();

            if (renderer != null)
            {
                Material material =
                    new Material(
                        Shader.Find("Unlit/Color"));

                material.color =
                    new Color(.45f,.65f,1f,.35f);

                renderer.sharedMaterial =
                    material;
            }
        }
    }

    private static void BuildBiomeBoundaries(
        Transform parent)
    {
        for (int i=0;i<7;i++)
        {
            GameObject boundary =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube);

            boundary.name =
                "BIOME_BOUNDARY_" +
                (i+1).ToString("00");

            boundary.transform.SetParent(parent);

            boundary.transform.position =
                new Vector3(
                    -5f+i*1.7f,
                    Mathf.Sin(i)*2f,
                    3.2f);

            boundary.transform.localScale =
                new Vector3(
                    1.2f,.025f,.025f);

            boundary.transform.rotation =
                Quaternion.Euler(
                    0f,0f,
                    20f);

            Renderer renderer =
                boundary.GetComponent<Renderer>();

            if (renderer != null)
            {
                Material material =
                    new Material(
                        Shader.Find("Unlit/Color"));

                material.color =
                    new Color(.75f,.35f,1f,.7f);

                renderer.sharedMaterial =
                    material;
            }
        }
    }

    // ============================================================
    // EVENT SYSTEM
    // ============================================================

    private static void BuildEventSystem()
    {
        new GameObject(
            "EventSystem",
            typeof(EventSystem),
            typeof(InputSystemUIInputModule));

        // Do not manually assign EventSystem.current.
        // Unity establishes it automatically.
    }

    // ============================================================
    // HUD ROOT
    // ============================================================

    private static void BuildHUD()
    {
        GameObject canvasObject =
            new GameObject(
                "HUD-206 RegionMapHUD",
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
            new Vector2(1920f,1080f);

        scaler.matchWidthOrHeight = .5f;

        BuildTopBar(canvasObject.transform);
        BuildNavigation(canvasObject.transform);
        BuildLayers(canvasObject.transform);
        BuildObjectInfo(canvasObject.transform);
        BuildTerrain(canvasObject.transform);
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
                "HUD-206-01 TopBar",
                parent,
                new Vector2(0f,.88f),
                new Vector2(1f,1f));

        Label(
            "HUD-206-01A PlanetNameDisplay",
            panel.transform,
            "PLANET: ACHERON",
            new Vector2(.01f,.20f),
            new Vector2(.18f,.85f),
            21,
            Accent);

        Label(
            "HUD-206-01B ContinentNameDisplay",
            panel.transform,
            "CONTINENT: CINDERFALL",
            new Vector2(.19f,.20f),
            new Vector2(.38f,.85f),
            17,
            Color.white);

        Label(
            "HUD-206-01C RegionNameDisplay",
            panel.transform,
            "REGION: ASHEN FRONTIER",
            new Vector2(.39f,.20f),
            new Vector2(.59f,.85f),
            17,
            Good);

        Label(
            "HUD-206-01D OwnerFactionEmblem",
            panel.transform,
            "OWNER // OBSIDIAN",
            new Vector2(.60f,.20f),
            new Vector2(.73f,.85f),
            13,
            Good);

        Label(
            "HUD-206-01E PlayerResourcesPanel",
            panel.transform,
            "CREDITS 84,250 | MAT 12,480 | ENERGY 8,920",
            new Vector2(.73f,.20f),
            new Vector2(.91f,.85f),
            12,
            Color.white);

        Label(
            "HUD-206-01F TimeControlPanel",
            panel.transform,
            "PAUSED | 1X 2X 4X",
            new Vector2(.91f,.20f),
            new Vector2(.995f,.85f),
            11,
            Warning);
    }

    // ============================================================
    // NAVIGATION
    // ============================================================

    private static void BuildNavigation(
        Transform parent)
    {
        GameObject panel =
            Panel(
                "HUD-206-02 NavigationPanel",
                parent,
                new Vector2(.01f,.42f),
                new Vector2(.19f,.86f));

        string[] names =
        {
            "HUD-206-02A ReturnToContinentMapButton",
            "HUD-206-02B ReturnToSurfaceMapButton",
            "HUD-206-02C ReturnToPlanetOverviewButton",
            "HUD-206-02D BiomeMapButton",
            "HUD-206-02E IslandMapButton"
        };

        string[] labels =
        {
            "CONTINENT MAP",
            "SURFACE MAP",
            "PLANET OVERVIEW",
            "BIOME MAP",
            "ISLAND MAP"
        };

        for(int i=0;i<names.Length;i++)
        {
            Button(
                names[i],
                panel.transform,
                labels[i],
                new Vector2(.07f,.79f-i*.15f),
                new Vector2(.93f,.91f-i*.15f));
        }
    }

    // ============================================================
    // LAYERS
    // ============================================================

    private static void BuildLayers(
        Transform parent)
    {
        GameObject panel =
            Panel(
                "HUD-206-03 RegionLayersPanel",
                parent,
                new Vector2(.01f,.04f),
                new Vector2(.19f,.40f));

        string[] names =
        {
            "HUD-206-03A LayerToggle_Terrain",
            "HUD-206-03B LayerToggle_Elevation",
            "HUD-206-03C LayerToggle_Heatmap",
            "HUD-206-03D LayerToggle_Waterways",
            "HUD-206-03E LayerToggle_FactionControl",
            "HUD-206-03F LayerToggle_WarfareZones",
            "HUD-206-03G LayerToggle_ResourceNodes",
            "HUD-206-03H LayerToggle_LogisticsRoutes",
            "HUD-206-03I LayerToggle_WeatherSystems",
            "HUD-206-03J LayerToggle_BiomeBoundaries"
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
            "BIOME BOUNDARIES"
        };

        for(int i=0;i<names.Length;i++)
        {
            int col = i/5;
            int row = i%5;

            float x0 =
                .04f+col*.47f;

            float x1 =
                x0+.43f;

            float y1 =
                .94f-row*.18f;

            float y0 =
                y1-.13f;

            Button(
                names[i],
                panel.transform,
                labels[i],
                new Vector2(x0,y0),
                new Vector2(x1,y1));
        }
    }

    // ============================================================
    // OBJECT INFO
    // ============================================================

    private static void BuildObjectInfo(
        Transform parent)
    {
        GameObject panel =
            Panel(
                "HUD-206-04 RegionObjectInfoPanel",
                parent,
                new Vector2(.80f,.44f),
                new Vector2(.99f,.86f));

        string[] names =
        {
            "HUD-206-04A SelectedObjectName",
            "HUD-206-04B SelectedObjectType",
            "HUD-206-04C SelectedObjectOwnerFaction",
            "HUD-206-04D SelectedObjectStatus",
            "HUD-206-04E SelectedObjectThreatLevel",
            "HUD-206-04F SelectedObjectEnvironmentType"
        };

        string[] values =
        {
            "SELECTED: ASHEN COMMAND OUTPOST",
            "TYPE: MILITARY INSTALLATION",
            "OWNER: OBSIDIAN PROTOCOL",
            "STATUS: OPERATIONAL",
            "THREAT: HIGH",
            "ENVIRONMENT: VOLCANIC HIGHLANDS"
        };

        for(int i=0;i<names.Length;i++)
        {
            Label(
                names[i],
                panel.transform,
                values[i],
                new Vector2(.06f,.84f-i*.105f),
                new Vector2(.94f,.95f-i*.105f),
                11,
                i==4 ? Threat : Color.white);
        }

        Button(
            "HUD-206-04G EnterObjectButton",
            panel.transform,
            "ENTER OBJECT",
            new Vector2(.20f,.04f),
            new Vector2(.80f,.14f));
    }

    // ============================================================
    // TERRAIN ANALYSIS
    // ============================================================

    

    // ============================================================
    // WARFARE
    // ============================================================

    private static void BuildWarfare(
        Transform parent)
    {
        GameObject panel =
            Panel(
                "HUD-206-06 WarfarePanel",
                parent,
                new Vector2(.80f,.03f),
                new Vector2(.99f,.19f));

        string[] names =
        {
            "HUD-206-06A WarfareFocusDisplay",
            "HUD-206-06B EngagementRangeDisplay",
            "HUD-206-06C MobilityRatingDisplay",
            "HUD-206-06D ReconVisibilityDisplay",
            "HUD-206-06E SensorInterferenceDisplay"
        };

        string[] values =
        {
            "FOCUS: GROUND CONTROL",
            "ENGAGEMENT RANGE: 3.8 km",
            "MOBILITY: MEDIUM",
            "RECON VISIBILITY: 61%",
            "SENSOR INTERFERENCE: 29%"
        };

        for(int i=0;i<5;i++)
        {
            Label(
                names[i],
                panel.transform,
                values[i],
                new Vector2(.04f,.76f-i*.18f),
                new Vector2(.96f,.92f-i*.18f),
                10,
                Color.white);
        }
    }

    // ============================================================
    // QUICK ACTIONS
    // ============================================================

    private static void BuildQuickActions(
        Transform parent)
    {
        GameObject panel =
            Panel(
                "HUD-206-07 QuickActionsPanel",
                parent,
                new Vector2(.20f,.03f),
                new Vector2(.44f,.20f));

        string[] names =
        {
            "HUD-206-07A DeployReconButton",
            "HUD-206-07B DeployLogisticsTeamButton",
            "HUD-206-07C OpenIntelNetworkButton",
            "HUD-206-07D OpenDoctrineManagerButton",
            "HUD-206-07E OpenWarfareProfileButton"
        };

        string[] labels =
        {
            "DEPLOY RECON",
            "DEPLOY LOGISTICS",
            "INTEL NETWORK",
            "DOCTRINE MANAGER",
            "WARFARE PROFILE"
        };

        for(int i=0;i<5;i++)
        {
            int row=i/3;
            int col=i%3;

            float x0=.04f+col*.32f;
            float x1=x0+.29f;

            float y1=.91f-row*.42f;
            float y0=y1-.32f;

            Button(
                names[i],
                panel.transform,
                labels[i],
                new Vector2(x0,y0),
                new Vector2(x1,y1));
        }
    }

    // ============================================================
    // ALERTS
    // ============================================================

    private static void BuildAlerts(
        Transform parent)
    {
        GameObject panel =
            Panel(
                "HUD-206-08 AlertsPanel",
                parent,
                new Vector2(.45f,.03f),
                new Vector2(.79f,.20f));

        string[] names =
        {
            "HUD-206-08A StrategicAlertEntry",
            "HUD-206-08B WarfareAlertEntry",
            "HUD-206-08C FactionAlertEntry",
            "HUD-206-08D ResourceAlertEntry",
            "HUD-206-08E WeatherAlertEntry"
        };

        string[] values =
        {
            "STRATEGIC // REGIONAL ACTIVITY ELEVATED",
            "WARFARE // HOSTILE PATROLS DETECTED",
            "FACTION // CONTROL LINE SHIFT DETECTED",
            "RESOURCE // EXTRACTION OUTPUT +11%",
            "WEATHER // ASH STORM APPROACHING"
        };

        for(int i=0;i<5;i++)
        {
            Label(
                names[i],
                panel.transform,
                values[i],
                new Vector2(.05f,.80f-i*.17f),
                new Vector2(.95f,.94f-i*.17f),
                10,
                i==1 ? Threat :
                i==4 ? Warning :
                Color.white);
        }
    }

    // ============================================================
    // BOTTOM BAR
    // ============================================================

    private static void BuildBottomBar(
        Transform parent)
    {
        GameObject panel =
            Panel(
                "HUD-206-09 BottomBar",
                parent,
                new Vector2(.20f,.22f),
                new Vector2(.79f,.30f));

        Button(
            "HUD-206-09A BackButton",
            panel.transform,
            "BACK",
            new Vector2(.02f,.15f),
            new Vector2(.22f,.85f));

        Button(
            "HUD-206-09B ExitToCommandCenterButton",
            panel.transform,
            "COMMAND CENTER",
            new Vector2(.27f,.15f),
            new Vector2(.52f,.85f));

        Button(
            "HUD-206-09C SettingsButton",
            panel.transform,
            "SETTINGS",
            new Vector2(.57f,.15f),
            new Vector2(.76f,.85f));

        Button(
            "HUD-206-09D HelpButton",
            panel.transform,
            "HELP",
            new Vector2(.81f,.15f),
            new Vector2(.98f,.85f));
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

        go.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin=anchorMin;
        rect.anchorMax=anchorMax;
        rect.offsetMin=Vector2.zero;
        rect.offsetMax=Vector2.zero;

        go.GetComponent<Image>().color =
            PanelColor;

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

        go.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin=anchorMin;
        rect.anchorMax=anchorMax;
        rect.offsetMin=Vector2.zero;
        rect.offsetMax=Vector2.zero;

        TextMeshProUGUI text =
            go.GetComponent<TextMeshProUGUI>();

        text.text=value;
        text.fontSize=fontSize;
        text.color=color;
        text.alignment=
            TextAlignmentOptions.Left;
        text.raycastTarget=false;
        text.textWrappingMode=
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

        go.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin=anchorMin;
        rect.anchorMax=anchorMax;
        rect.offsetMin=Vector2.zero;
        rect.offsetMax=Vector2.zero;

        Image image =
            go.GetComponent<Image>();

        image.color=Panel2;

        Button button =
            go.GetComponent<Button>();

        button.targetGraphic=image;

        GameObject labelObject =
            new GameObject(
                "LABEL",
                typeof(RectTransform),
                typeof(TextMeshProUGUI));

        labelObject.transform.SetParent(
            go.transform,
            false);

        TextMeshProUGUI text =
            labelObject.GetComponent<TextMeshProUGUI>();

        if(text==null)
            throw new Exception(
                "Failed to create button label: "+
                name);

        text.text=label;
        text.alignment=
            TextAlignmentOptions.Center;
        text.fontSize=13;
        text.color=Color.white;
        text.raycastTarget=false;
        text.textWrappingMode=
            TextWrappingModes.Normal;

        RectTransform labelRect =
            labelObject.GetComponent<RectTransform>();

        labelRect.anchorMin=Vector2.zero;
        labelRect.anchorMax=Vector2.one;
        labelRect.offsetMin=Vector2.zero;
        labelRect.offsetMax=Vector2.zero;

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

        bool found=false;

        for(int i=0;i<scenes.Count;i++)
        {
            if(scenes[i].path==ScenePath)
            {
                scenes[i].enabled=true;
                found=true;
                break;
            }
        }

        if(!found)
        {
            scenes.Add(
                new EditorBuildSettingsScene(
                    ScenePath,
                    true));
        }

        EditorBuildSettings.scenes=
            scenes.ToArray();
    }
}



