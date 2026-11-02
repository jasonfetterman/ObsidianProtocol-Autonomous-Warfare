using System;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public static class PlanetOverviewHUD203Builder
{
    private const string SceneFolder = "Assets/Scenes/SCN62_PLANET_OVERVIEW";
    private const string ScenePath = SceneFolder + "/Planet_Overview.unity";

    private static readonly Color PanelColor = new Color(.025f,.040f,.070f,.97f);
    private static readonly Color Panel2 = new Color(.045f,.065f,.100f,.98f);
    private static readonly Color Accent = new Color(.15f,.75f,1f,1f);
    private static readonly Color Warning = new Color(1f,.65f,.15f,1f);
    private static readonly Color Threat = new Color(1f,.22f,.22f,1f);
    private static readonly Color Good = new Color(.20f,1f,.45f,1f);

    private static readonly string[] Planets =
    {
        "PLN-01 ACHERON",
        "PLN-02 VIRELLIA",
        "PLN-03 KARTHOS",
        "PLN-04 SOLARA PRIME",
        "PLN-05 NOX MERIDIAN",
        "PLN-06 THALASSA IX",
        "PLN-07 DRAVON'S REACH",
        "PLN-08 EREBOS STATION",
        "PLN-09 HALCYON VERGE",
        "PLN-10 MYRIDIAN CORE",
        "PLN-11 VANTOR HELIX",
        "PLN-12 ORPHEON DELTA",
        "PLN-13 CRYOSTRIA",
        "PLN-14 ZEPHYRIA",
        "PLN-15 UMBRA-7",
        "PLN-16 CAELUS RIFT",
        "PLN-17 PYRRHOS GATE",
        "PLN-18 LUMINARA",
        "PLN-19 VEXILON",
        "PLN-20 ASTRYX",
        "PLN-21 KAIROTH",
        "PLN-22 NEMERIS",
        "PLN-23 ODYSSAR",
        "PLN-24 VALTOR PRIME",
        "PLN-25 SERAPHIS",
        "PLN-26 KRONIS-4",
        "PLN-27 EON'S CRADLE",
        "PLN-28 VIREX-THETA",
        "PLN-29 SOLARIS ECHO",
        "PLN-30 OBSIDIAN WORLD"
    };

    [MenuItem("Obsidian Protocol/Worlds/BUILD HUD-203 PLANET OVERVIEW")]
    public static void Build()
    {
        EnsureFolder();

        Scene scene = EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene,
            NewSceneMode.Single);

        BuildEnvironment(scene);
        BuildCanvas();

        EditorSceneManager.SaveScene(scene, ScenePath);
        AddToBuildSettings(ScenePath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[HUD-203] PLANET OVERVIEW BUILD COMPLETE: " + ScenePath);
    }

    private static void EnsureFolder()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Scenes"))
            AssetDatabase.CreateFolder("Assets", "Scenes");

        if (!AssetDatabase.IsValidFolder(SceneFolder))
            AssetDatabase.CreateFolder("Assets/Scenes", "SCN62_PLANET_OVERVIEW");
    }

    private static void BuildEnvironment(Scene scene)
    {
        GameObject root = new GameObject("PLANET OVERVIEW ENVIRONMENT");

        GameObject backdrop = GameObject.CreatePrimitive(
            PrimitiveType.Quad);

        backdrop.name = "PLANET SURFACE BACKDROP";
        backdrop.transform.SetParent(root.transform);
        backdrop.transform.position = new Vector3(0,0,8);
        backdrop.transform.localScale = new Vector3(40,22,1);

        Material backdropMat = new Material(
            Shader.Find("Universal Render Pipeline/Unlit"));

        if (backdropMat.shader == null)
            backdropMat = new Material(Shader.Find("Unlit/Color"));

        backdropMat.color = new Color(.008f,.018f,.032f,1f);

        Renderer backdropRenderer =
            backdrop.GetComponent<Renderer>();

        backdropRenderer.sharedMaterial = backdropMat;

        UnityEngine.Object.DestroyImmediate(
            backdrop.GetComponent<Collider>());

        BuildPlanetSurface(root);

        GameObject cameraGO = new GameObject("Planet Overview Camera");
        Camera cam = cameraGO.AddComponent<Camera>();

        cameraGO.transform.position =
            new Vector3(0,0,-20);

        cameraGO.transform.rotation =
            Quaternion.identity;

        cam.orthographic = true;
        cam.orthographicSize = 10f;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(.005f,.012f,.025f,1f);
    }

    private static void BuildPlanetSurface(GameObject root)
    {
        GameObject planet = GameObject.CreatePrimitive(
            PrimitiveType.Sphere);

        planet.name = "SELECTED PLANET - PLANET OVERVIEW";
        planet.transform.SetParent(root.transform);
        planet.transform.position = new Vector3(0,-.8f,4);
        planet.transform.localScale = new Vector3(10f,10f,10f);

        Material mat = new Material(
            Shader.Find("Universal Render Pipeline/Lit"));

        if (mat.shader == null)
            mat = new Material(Shader.Find("Standard"));

        mat.color = new Color(.08f,.24f,.30f,1f);

        Renderer r = planet.GetComponent<Renderer>();
        r.sharedMaterial = mat;

        UnityEngine.Object.DestroyImmediate(
            planet.GetComponent<Collider>());

        for (int i = 0; i < 18; i++)
        {
            GameObject marker = GameObject.CreatePrimitive(
                PrimitiveType.Cube);

            marker.name = "SURFACE_REGION_" +
                          (i + 1).ToString("00");

            marker.transform.SetParent(root.transform);

            float angle =
                (Mathf.PI * 2f * i) / 18f;

            float radius =
                3.0f + ((i % 4) * .65f);

            marker.transform.position =
                new Vector3(
                    Mathf.Cos(angle) * radius,
                    Mathf.Sin(angle) * radius - .8f,
                    1.5f);

            marker.transform.localScale =
                new Vector3(.18f,.18f,.18f);

            Material mm = new Material(
                Shader.Find("Universal Render Pipeline/Unlit"));

            if (mm.shader == null)
                mm = new Material(Shader.Find("Unlit/Color"));

            mm.color =
                (i % 3 == 0) ? Accent :
                (i % 3 == 1) ? Good :
                Warning;

            marker.GetComponent<Renderer>().sharedMaterial = mm;

            UnityEngine.Object.DestroyImmediate(
                marker.GetComponent<Collider>());
        }
    }

    private static Canvas BuildCanvas()
    {
        GameObject canvasGO =
            new GameObject(
                "HUD-203 PlanetOverviewHUD",
                typeof(RectTransform),
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));

        Canvas canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler =
            canvasGO.GetComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(1920,1080);

        scaler.matchWidthOrHeight = .5f;

        GameObject eventSystem =
            new GameObject(
                "EventSystem",
                typeof(EventSystem),
                typeof(InputSystemUIInputModule));

        BuildTopBar(canvasGO.transform);
        BuildNavigation(canvasGO.transform);
        BuildStats(canvasGO.transform);
        BuildLayers(canvasGO.transform);
        BuildObjectInfo(canvasGO.transform);
        BuildQuickActions(canvasGO.transform);
        BuildAlerts(canvasGO.transform);
        BuildBottomBar(canvasGO.transform);

        return canvas;
    }

    private static void BuildTopBar(Transform parent)
    {
        GameObject panel =
            Panel(
                parent,
                "HUD-203-01 TopBar",
                new Vector2(0.02f,.90f),
                new Vector2(.98f,.985f),
                PanelColor);

        Label(panel.transform,
            "HUD-203-01A PlanetNameDisplay",
            "PLN-01 ACHERON",
            new Vector2(.01f,.30f),
            new Vector2(.22f,.88f),
            24,
            Accent);

        Label(panel.transform,
            "HUD-203-01B PlanetTypeDisplay",
            "TERRESTRIAL WORLD // CONTROLLED",
            new Vector2(.23f,.35f),
            new Vector2(.48f,.82f),
            14,
            Color.white);

        GameObject emblem =
            Panel(
                panel.transform,
                "HUD-203-01C PlanetOwnerFactionEmblem",
                new Vector2(.55f,.20f),
                new Vector2(.61f,.82f),
                Panel2);

        Label(emblem.transform,
            "FACTION",
            "OP",
            new Vector2(.05f,.05f),
            new Vector2(.95f,.95f),
            20,
            Accent);

        Panel(
            panel.transform,
            "HUD-203-01D PlayerResourcesPanel",
            new Vector2(.63f,.15f),
            new Vector2(.82f,.85f),
            Panel2);

        Label(panel.transform,
            "RESOURCES",
            "CR  84,250   |   MAT  12,480   |   EN  92%",
            new Vector2(.635f,.28f),
            new Vector2(.815f,.78f),
            12,
            Good);

        Panel(
            panel.transform,
            "HUD-203-01E TimeControlPanel",
            new Vector2(.84f,.15f),
            new Vector2(.985f,.85f),
            Panel2);

        Label(panel.transform,
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
                "HUD-203-02 NavigationPanel",
                new Vector2(.02f,.34f),
                new Vector2(.20f,.88f),
                PanelColor);

        string[] names =
        {
            "ReturnToStarSystemButton",
            "ReturnToGalaxyMapButton",
            "SurfaceMapButton",
            "ContinentMapButton",
            "RegionMapButton",
            "IslandMapButton",
            "BiomeMapButton"
        };

        string[] labels =
        {
            "STAR SYSTEM",
            "GALAXY MAP",
            "SURFACE MAP",
            "CONTINENTS",
            "REGIONS",
            "ISLANDS",
            "BIOMES"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Button b =
                Button(
                    panel.transform,
                    "HUD-203-02" +
                    (char)('A' + i) +
                    " " + names[i],
                    labels[i]);

            SetRect(
                b.gameObject,
                new Vector2(.08f,.90f - i * .115f),
                new Vector2(.92f,.985f - i * .115f));
        }
    }

    private static void BuildStats(Transform parent)
    {
        GameObject panel =
            Panel(
                parent,
                "HUD-203-03 PlanetStatsPanel",
                new Vector2(.77f,.52f),
                new Vector2(.98f,.88f),
                PanelColor);

        string[] names =
        {
            "PopulationDisplay",
            "StabilityDisplay",
            "ResourceOutputDisplay",
            "LogisticsCapacityDisplay",
            "ThreatLevelDisplay",
            "EnvironmentTypeDisplay"
        };

        string[] values =
        {
            "POPULATION       8.42 BILLION",
            "STABILITY        87%",
            "RESOURCE OUTPUT  12,480 / DAY",
            "LOGISTICS CAP.   74,000 DP",
            "THREAT LEVEL     31% // MODERATE",
            "ENVIRONMENT      TEMPERATE"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Color c =
                names[i] == "ThreatLevelDisplay"
                    ? Warning
                    : Color.white;

            Label(
                panel.transform,
                "HUD-203-03" +
                (char)('A' + i) +
                " " + names[i],
                values[i],
                new Vector2(.07f,.84f - i * .14f),
                new Vector2(.94f,.97f - i * .14f),
                12,
                c);
        }
    }

    private static void BuildLayers(Transform parent)
    {
        GameObject panel =
            Panel(
                parent,
                "HUD-203-04 PlanetLayersPanel",
                new Vector2(.77f,.29f),
                new Vector2(.98f,.50f),
                PanelColor);

        string[] names =
        {
            "LayerToggle_Continents",
            "LayerToggle_Regions",
            "LayerToggle_Biomes",
            "LayerToggle_Islands",
            "LayerToggle_FactionControl",
            "LayerToggle_WarfareZones",
            "LayerToggle_ResourceNodes",
            "LayerToggle_LogisticsRoutes"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Button b =
                Button(
                    panel.transform,
                    "HUD-203-04" +
                    (char)('A' + i) +
                    " " + names[i],
                    names[i]
                        .Replace("LayerToggle_","")
                        .Replace("_"," "));

            int col = i / 4;
            int row = i % 4;

            SetRect(
                b.gameObject,
                new Vector2(.05f + col * .48f,
                            .76f - row * .22f),
                new Vector2(.47f + col * .48f,
                            .94f - row * .22f));
        }
    }

    private static void BuildObjectInfo(Transform parent)
    {
        GameObject panel =
            Panel(
                parent,
                "HUD-203-05 PlanetObjectInfoPanel",
                new Vector2(.52f,.29f),
                new Vector2(.75f,.50f),
                PanelColor);

        Label(
            panel.transform,
            "HUD-203-05A SelectedObjectName",
            "ACHERON // CENTRAL COMMAND",
            new Vector2(.06f,.72f),
            new Vector2(.94f,.92f),
            15,
            Accent);

        Label(
            panel.transform,
            "HUD-203-05B SelectedObjectType",
            "COMMAND FACILITY",
            new Vector2(.06f,.54f),
            new Vector2(.94f,.70f),
            11,
            Color.white);

        Label(
            panel.transform,
            "HUD-203-05C SelectedObjectOwnerFaction",
            "OWNER // OBSIDIAN PROTOCOL",
            new Vector2(.06f,.40f),
            new Vector2(.94f,.53f),
            11,
            Color.white);

        Label(
            panel.transform,
            "HUD-203-05D SelectedObjectStatus",
            "STATUS // OPERATIONAL",
            new Vector2(.06f,.26f),
            new Vector2(.94f,.39f),
            11,
            Good);

        Label(
            panel.transform,
            "HUD-203-05E SelectedObjectThreatLevel",
            "THREAT // LOW",
            new Vector2(.06f,.12f),
            new Vector2(.50f,.25f),
            11,
            Warning);

        Button enter =
            Button(
                panel.transform,
                "HUD-203-05F EnterObjectButton",
                "ENTER");

        SetRect(
            enter.gameObject,
            new Vector2(.57f,.08f),
            new Vector2(.94f,.27f));
    }

    private static void BuildQuickActions(Transform parent)
    {
        GameObject panel =
            Panel(
                parent,
                "HUD-203-06 QuickActionsPanel",
                new Vector2(.22f,.54f),
                new Vector2(.50f,.88f),
                PanelColor);

        string[] names =
        {
            "DeployReconButton",
            "OpenIntelNetworkButton",
            "OpenDoctrineManagerButton",
            "OpenLogisticsOverviewButton",
            "OpenWarfareProfileButton"
        };

        string[] labels =
        {
            "DEPLOY RECON",
            "INTELLIGENCE NETWORK",
            "DOCTRINE MANAGER",
            "LOGISTICS OVERVIEW",
            "WARFARE PROFILE"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Button b =
                Button(
                    panel.transform,
                    "HUD-203-06" +
                    (char)('A' + i) +
                    " " + names[i],
                    labels[i]);

            SetRect(
                b.gameObject,
                new Vector2(.08f,.82f - i * .16f),
                new Vector2(.92f,.95f - i * .16f));
        }
    }

    private static void BuildAlerts(Transform parent)
    {
        GameObject panel =
            Panel(
                parent,
                "HUD-203-07 AlertsPanel",
                new Vector2(.52f,.07f),
                new Vector2(.98f,.27f),
                PanelColor);

        Label(
            panel.transform,
            "HUD-203-07A StrategicAlertEntry",
            "STRATEGIC // PLANETARY CONTROL STABLE",
            new Vector2(.05f,.72f),
            new Vector2(.95f,.94f),
            11,
            Accent);

        Label(
            panel.transform,
            "HUD-203-07B WarfareAlertEntry",
            "WARFARE // NO ACTIVE FRONTLINE ENGAGEMENTS",
            new Vector2(.05f,.51f),
            new Vector2(.95f,.70f),
            11,
            Warning);

        Label(
            panel.transform,
            "HUD-203-07C FactionAlertEntry",
            "FACTION // FRIENDLY CONTROL CONFIRMED",
            new Vector2(.05f,.30f),
            new Vector2(.95f,.49f),
            11,
            Good);

        Label(
            panel.transform,
            "HUD-203-07D ResourceAlertEntry",
            "RESOURCE // EXTRACTION WITHIN CAPACITY",
            new Vector2(.05f,.09f),
            new Vector2(.95f,.28f),
            11,
            Color.white);
    }

    private static void BuildBottomBar(Transform parent)
    {
        GameObject panel =
            Panel(
                parent,
                "HUD-203-08 BottomBar",
                new Vector2(.22f,.02f),
                new Vector2(.50f,.09f),
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
            Button b =
                Button(
                    panel.transform,
                    "HUD-203-08" +
                    (char)('A' + i) +
                    " " + names[i],
                    labels[i]);

            float x0 = .02f + i * .245f;
            float x1 = x0 + .225f;

            SetRect(
                b.gameObject,
                new Vector2(x0,.12f),
                new Vector2(x1,.88f));
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

        go.transform.SetParent(parent,false);

        SetRect(go,min,max);

        Image image =
            go.GetComponent<Image>();

        image.color = color;

        return go;
    }

    private static GameObject ChildPanel(
        Transform parent,
        string name)
    {
        return Panel(
            parent,
            name,
            new Vector2(.02f,.02f),
            new Vector2(.98f,.98f),
            Panel2);
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

        go.transform.SetParent(parent,false);

        Image image =
            go.GetComponent<Image>();

        image.color =
            new Color(.06f,.10f,.15f,.98f);

        Button button =
            go.GetComponent<Button>();

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
                "Failed to create TextMeshProUGUI: " + name);

        text.text = label;
        text.alignment = TextAlignmentOptions.Center;
        text.fontSize = 12;
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

        go.transform.SetParent(parent,false);

        SetRect(go,min,max);

        TextMeshProUGUI text =
            go.GetComponent<TextMeshProUGUI>();

        text.text = value;
        text.alignment = TextAlignmentOptions.Left;
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
