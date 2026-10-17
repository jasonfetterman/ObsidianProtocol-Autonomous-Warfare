using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class GalaxyMapHUD201Builder
{
    private const string ROOT = "Assets/Scenes/SCN60_GALAXY_MAP";
    private const string SCENE = ROOT + "/Galaxy_Map.unity";

    private static readonly string[] PlanetIDs =
    {
        "PLN-01","PLN-02","PLN-03","PLN-04","PLN-05",
        "PLN-06","PLN-07","PLN-08","PLN-09","PLN-10",
        "PLN-11","PLN-12","PLN-13","PLN-14","PLN-15",
        "PLN-16","PLN-17","PLN-18","PLN-19","PLN-20",
        "PLN-21","PLN-22","PLN-23","PLN-24","PLN-25",
        "PLN-26","PLN-27","PLN-28","PLN-29","PLN-30"
    };

    private static readonly string[] PlanetNames =
    {
        "Acheron",
        "Virellia",
        "Karthos",
        "Solara Prime",
        "Nox Meridian",
        "Thalassa IX",
        "Dravon's Reach",
        "Erebos Station",
        "Halcyon Verge",
        "Myridian Core",
        "Vantor Helix",
        "Orpheon Delta",
        "Cryostria",
        "Zephyria",
        "Umbra-7",
        "Caelus Rift",
        "Pyrrhos Gate",
        "Luminara",
        "Vexilon",
        "Astryx",
        "Kairoth",
        "Nemeris",
        "Odyssar",
        "Valtor Prime",
        "Seraphis",
        "Kronis-4",
        "Eon's Cradle",
        "Virex-Theta",
        "Solaris Echo",
        "Obsidian World"
    };

    private static readonly string[] SceneNames =
    {
        "Acheron",
        "Virellia",
        "Karthos",
        "Solara_Prime",
        "Nox_Meridian",
        "Thalassa_IX",
        "Dravons_Reach",
        "Erebos_Station",
        "Halcyon_Verge",
        "Myridian_Core",
        "Vantor_Helix",
        "Orpheon_Delta",
        "Cryostria",
        "Zephyria",
        "Umbra_7",
        "Caelus_Rift",
        "Pyrrhos_Gate",
        "Luminara",
        "Vexilon",
        "Astryx",
        "Kairoth",
        "Nemeris",
        "Odyssar",
        "Valtor_Prime",
        "Seraphis",
        "Kronis_4",
        "Eons_Cradle",
        "Virex_Theta",
        "Solaris_Echo",
        "Obsidian_World"
    };

    private static readonly Color Background = new Color(.008f,.012f,.025f,1f);
    private static readonly Color PanelColor = new Color(.025f,.040f,.070f,.97f);
    private static readonly Color Panel2 = new Color(.045f,.065f,.105f,.98f);
    private static readonly Color Line = new Color(.16f,.32f,.50f,1f);
    private static readonly Color Accent = new Color(.15f,.70f,1f,1f);
    private static readonly Color Accent2 = new Color(.45f,.85f,1f,1f);
    private static readonly Color Warning = new Color(1f,.58f,.15f,1f);
    private static readonly Color Threat = new Color(1f,.22f,.28f,1f);

    [MenuItem("Obsidian Protocol/Worlds/BUILD HUD-201 GALAXY MAP")]
    public static void Build()
    {
        EnsureFolders();

        Scene scene = EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene,
            NewSceneMode.Single);

        BuildEnvironment();
        BuildEventSystem();

        Canvas canvas = CreateCanvas();

        BuildTopBar(canvas.transform);
        BuildNavigation(canvas.transform);
        BuildMap(canvas.transform);
        BuildMapLayers(canvas.transform);
        BuildObjectInfo(canvas.transform);
        BuildLegend(canvas.transform);
        BuildQuickActions(canvas.transform);
        BuildAlerts(canvas.transform);
        BuildBottomBar(canvas.transform);

        EditorSceneManager.SaveScene(scene, SCENE);

        AddSceneToBuildSettings(SCENE);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[OPAW] HUD-201 GALAXY MAP COMPLETE: " + SCENE);
    }

    private static void EnsureFolders()
    {
        Directory.CreateDirectory(
            Path.Combine(Application.dataPath, "Scenes/SCN60_GALAXY_MAP"));
        AssetDatabase.Refresh();
    }

    private static void BuildEnvironment()
    {
        GameObject environment = new GameObject("GALAXY MAP ENVIRONMENT");

        GameObject backdrop = GameObject.CreatePrimitive(
            PrimitiveType.Quad);

        backdrop.name = "GalaxyBackdrop";
        backdrop.transform.SetParent(environment.transform);
        backdrop.transform.position = new Vector3(0,0,8);
        backdrop.transform.rotation = Quaternion.Euler(0,0,0);
        backdrop.transform.localScale = new Vector3(30,18,1);

        Renderer r = backdrop.GetComponent<Renderer>();
        Material m = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        if (m.shader == null)
            m = new Material(Shader.Find("Unlit/Color"));

        m.color = Background;
        r.sharedMaterial = m;

        for (int i = 0; i < 90; i++)
        {
            GameObject star = GameObject.CreatePrimitive(
                PrimitiveType.Sphere);

            star.name = "STAR_" + i.ToString("00");
            star.transform.SetParent(environment.transform);

            float x = UnityEngine.Random.Range(-14f,14f);
            float y = UnityEngine.Random.Range(-8f,8f);

            star.transform.position =
                new Vector3(x,y,7.5f);

            float size =
                UnityEngine.Random.Range(.015f,.06f);

            star.transform.localScale =
                Vector3.one * size;

            Renderer sr = star.GetComponent<Renderer>();

            Material sm =
                new Material(Shader.Find("Universal Render Pipeline/Unlit"));

            if (sm.shader == null)
                sm = new Material(Shader.Find("Unlit/Color"));

            sm.color =
                new Color(
                    UnityEngine.Random.Range(.35f,.9f),
                    UnityEngine.Random.Range(.55f,1f),
                    1f,
                    1f);

            sr.sharedMaterial = sm;

            UnityEngine.Object.DestroyImmediate(
                star.GetComponent<Collider>());
        }

        Camera cam = new GameObject(
            "GalaxyMapCamera",
            typeof(Camera)).GetComponent<Camera>();

        cam.transform.position = new Vector3(0,0,-10);
        cam.transform.rotation = Quaternion.identity;
        cam.orthographic = true;
        cam.orthographicSize = 10;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Background;
        cam.depth = -10;
    }

    private static void BuildEventSystem()
    {
        GameObject es = new GameObject(
            "EventSystem",
            typeof(EventSystem),
            typeof(StandaloneInputModule));

        es.transform.SetParent(null);
    }

    private static Canvas CreateCanvas()
    {
        GameObject go = new GameObject(
            "HUD-201 GalaxyMapHUD",
            typeof(RectTransform),
            typeof(Canvas),
            typeof(CanvasScaler),
            typeof(GraphicRaycaster));

        Canvas canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        CanvasScaler scaler = go.GetComponent<CanvasScaler>();
        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution =
            new Vector2(1920,1080);
        scaler.screenMatchMode =
            CanvasScaler.ScreenMatchMode.Expand;

        return canvas;
    }

    private static GameObject Panel(
        Transform parent,
        string name,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 offsetMin,
        Vector2 offsetMax)
    {
        GameObject go = new GameObject(
            name,
            typeof(RectTransform),
            typeof(Image));

        go.transform.SetParent(parent,false);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.offsetMin = offsetMin;
        rt.offsetMax = offsetMax;

        Image image = go.GetComponent<Image>();
        image.color = PanelColor;
        image.raycastTarget = true;

        return go;
    }

    private static TMP_Text Label(
        Transform parent,
        string name,
        string value,
        int size,
        TextAlignmentOptions alignment = TextAlignmentOptions.Left)
    {
        GameObject go = new GameObject(
            name,
            typeof(RectTransform),
            typeof(TextMeshProUGUI));

        go.transform.SetParent(parent,false);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = new Vector2(10,4);
        rt.offsetMax = new Vector2(-10,-4);

        TextMeshProUGUI text =
            go.GetComponent<TextMeshProUGUI>();

        text.text = value;
        text.fontSize = size;
        text.color = Color.white;
        text.alignment = alignment;
        text.raycastTarget = false;
        text.textWrappingMode = TextWrappingModes.Normal;

        return text;
    }

    private static Button Button(
        Transform parent,
        string name,
        string label,
        UnityEngine.Events.UnityAction action = null)
    {
        GameObject go = new GameObject(
            name,
            typeof(RectTransform),
            typeof(Image),
            typeof(Button));

        go.transform.SetParent(parent,false);

        Image image = go.GetComponent<Image>();
        image.color = Panel2;
        image.raycastTarget = true;

        Button button = go.GetComponent<Button>();

        if (action != null)
            button.onClick.AddListener(action);

        GameObject labelObject = new GameObject(
            "LABEL",
            typeof(RectTransform),
            typeof(TextMeshProUGUI));

        labelObject.transform.SetParent(go.transform,false);

        RectTransform rt =
            labelObject.GetComponent<RectTransform>();

        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        TextMeshProUGUI text =
            labelObject.GetComponent<TextMeshProUGUI>();

        text.text = label;
        text.fontSize = 14;
        text.color = Color.white;
        text.alignment = TextAlignmentOptions.Center;
        text.raycastTarget = false;

        return button;
    }

    private static void BuildTopBar(Transform canvas)
    {
        GameObject top = Panel(
            canvas,
            "HUD-201-01 TopBar",
            new Vector2(0, .91f),
            new Vector2(1,1),
            new Vector2(12,8),
            new Vector2(-12,-8));

        Label(top.transform,
            "HUD-201-01A CurrentGalaxyNameDisplay",
            "GALAXY // OBSIDIAN THEATER",
            22);

        Label(top.transform,
            "HUD-201-01B CurrentSectorNameDisplay",
            "SECTOR // CENTRAL FRONT",
            15);

        GameObject faction = Panel(
            top.transform,
            "HUD-201-01C PlayerFactionEmblem",
            new Vector2(.63f,0),
            new Vector2(.72f,1),
            new Vector2(4,4),
            new Vector2(-4,-4));

        Label(faction.transform,
            "FactionEmblem",
            "OP",
            26,
            TextAlignmentOptions.Center);

        GameObject resources = Panel(
            top.transform,
            "HUD-201-01D PlayerResourcesPanel",
            new Vector2(.72f,0),
            new Vector2(.88f,1),
            new Vector2(4,4),
            new Vector2(-4,-4));

        Label(resources.transform,
            "Resources",
            "CREDITS  125,400\nMATERIAL  84,250\nENERGY  67,800",
            13);

        GameObject time = Panel(
            top.transform,
            "HUD-201-01E TimeControlPanel",
            new Vector2(.88f,0),
            new Vector2(1,1),
            new Vector2(4,4),
            new Vector2(-4,-4));

        Label(time.transform,
            "Time",
            "T+ 184:27:16\nPAUSED",
            13,
            TextAlignmentOptions.Center);
    }

    private static void BuildNavigation(Transform canvas)
    {
        GameObject nav = Panel(
            canvas,
            "HUD-201-02 NavigationPanel",
            new Vector2(0,.57f),
            new Vector2(.19f,.90f),
            new Vector2(12,8),
            new Vector2(-6,-8));

        Button zoomIn = Button(
            nav.transform,
            "HUD-201-02A GalaxyZoomInButton",
            "+  ZOOM IN");

        SetRect(zoomIn.gameObject,.05f,.80f,.95f,.96f);

        Button zoomOut = Button(
            nav.transform,
            "HUD-201-02B GalaxyZoomOutButton",
            "-  ZOOM OUT");

        SetRect(zoomOut.gameObject,.05f,.63f,.95f,.79f);

        GameObject dropdown = Panel(
            nav.transform,
            "HUD-201-02C SectorListDropdown",
            new Vector2(.05f,.46f),
            new Vector2(.95f,.61f),
            Vector2.zero,
            Vector2.zero);

        Label(dropdown.transform,
            "DropdownValue",
            "SECTOR 01  CENTRAL",
            13,
            TextAlignmentOptions.Center);

        GameObject search = Panel(
            nav.transform,
            "HUD-201-02D PlanetSearchField",
            new Vector2(.05f,.27f),
            new Vector2(.95f,.43f),
            Vector2.zero,
            Vector2.zero);

        Label(search.transform,
            "SearchValue",
            "SEARCH PLANET...",
            12,
            TextAlignmentOptions.Center);

        Button filter = Button(
            nav.transform,
            "HUD-201-02E FilterPanelButton",
            "FILTERS");

        SetRect(filter.gameObject,.05f,.06f,.95f,.22f);
    }

    private static void BuildMap(Transform canvas)
    {
        GameObject map = Panel(
            canvas,
            "GALAXY MAP DISPLAY",
            new Vector2(.20f,.14f),
            new Vector2(.80f,.90f),
            new Vector2(4,4),
            new Vector2(-4,-4));

        GameObject title = new GameObject(
            "MAP_TITLE",
            typeof(RectTransform),
            typeof(TextMeshProUGUI));

        title.transform.SetParent(map.transform,false);

        RectTransform titleRt =
            title.GetComponent<RectTransform>();

        titleRt.anchorMin = new Vector2(0, .93f);
        titleRt.anchorMax = new Vector2(1,1);
        titleRt.offsetMin = Vector2.zero;
        titleRt.offsetMax = Vector2.zero;

        TextMeshProUGUI titleText =
            title.GetComponent<TextMeshProUGUI>();

        titleText.text =
            "GALACTIC STRATEGIC MAP  //  30 KNOWN WORLDS";
        titleText.fontSize = 16;
        titleText.color = Accent;
        titleText.alignment =
            TextAlignmentOptions.Center;

        for (int i = 0; i < 30; i++)
        {
            float angle =
                (Mathf.PI * 2f / 30f) * i;

            float radius =
                0.18f + ((i % 5) * .07f);

            float x =
                .5f + Mathf.Cos(angle) * radius;

            float y =
                .50f + Mathf.Sin(angle) * radius * .82f;

            Button marker = Button(
                map.transform,
                "PLANET_" + PlanetIDs[i],
                PlanetIDs[i]);

            SetRect(
                marker.gameObject,
                x-.035f,
                y-.035f,
                x+.035f,
                y+.035f);

            Image image =
                marker.GetComponent<Image>();

            image.color =
                i == 29 ? Threat : Accent;

            int index = i;

            marker.onClick.AddListener(
                () => SelectPlanet(index));
        }

        GameObject center = Panel(
            map.transform,
            "GALAXY_CORE",
            new Vector2(.44f,.44f),
            new Vector2(.56f,.56f),
            Vector2.zero,
            Vector2.zero);

        Image coreImage =
            center.GetComponent<Image>();

        coreImage.color =
            new Color(.20f,.45f,.75f,.35f);

        Label(
            center.transform,
            "CoreLabel",
            "OP\nCORE",
            15,
            TextAlignmentOptions.Center);
    }

    private static void BuildMapLayers(Transform canvas)
    {
        GameObject panel = Panel(
            canvas,
            "HUD-201-03 MapLayerPanel",
            new Vector2(.81f,.52f),
            new Vector2(.99f,.90f),
            new Vector2(4,4),
            new Vector2(-12,-8));

        string[] names =
        {
            "HUD-201-03A LayerToggle_Galaxy",
            "HUD-201-03B LayerToggle_Sectors",
            "HUD-201-03C LayerToggle_Planets",
            "HUD-201-03D LayerToggle_Factions",
            "HUD-201-03E LayerToggle_WarfareZones",
            "HUD-201-03F LayerToggle_Resources",
            "HUD-201-03G LayerToggle_TradeRoutes"
        };

        string[] labels =
        {
            "GALAXY",
            "SECTORS",
            "PLANETS",
            "FACTIONS",
            "WARFARE ZONES",
            "RESOURCES",
            "TRADE ROUTES"
        };

        for (int i=0;i<names.Length;i++)
        {
            Button b =
                Button(panel.transform,names[i],labels[i]);

            float top =
                .94f - (i * .125f);

            SetRect(
                b.gameObject,
                .06f,
                top-.10f,
                .94f,
                top);
        }
    }

    private static void BuildObjectInfo(Transform canvas)
    {
        GameObject panel = Panel(
            canvas,
            "HUD-201-04 GalaxyObjectInfoPanel",
            new Vector2(.81f,.14f),
            new Vector2(.99f,.50f),
            new Vector2(4,4),
            new Vector2(-12,-4));

        Label(
            panel.transform,
            "HUD-201-04A ObjectNameDisplay",
            "OBJECT // NO SELECTION",
            17);

        GameObject type = ChildPanel(
            panel.transform,
            "HUD-201-04B ObjectTypeDisplay",
            .64f,.86f);

        Label(type.transform,
            "Value",
            "TYPE // PLANET",
            12);

        GameObject owner = ChildPanel(
            panel.transform,
            "HUD-201-04C ObjectOwnerFactionDisplay",
            .48f,.62f);

        Label(owner.transform,
            "Value",
            "OWNER // UNKNOWN",
            12);

        GameObject status = ChildPanel(
            panel.transform,
            "HUD-201-04D ObjectStatusDisplay",
            .32f,.46f);

        Label(status.transform,
            "Value",
            "STATUS // NOMINAL",
            12);

        GameObject threat = ChildPanel(
            panel.transform,
            "HUD-201-04E ObjectThreatLevelDisplay",
            .16f,.30f);

        Label(threat.transform,
            "Value",
            "THREAT // UNASSESSED",
            12);

        Button enter =
            Button(
                panel.transform,
                "HUD-201-04F ObjectEnterButton",
                "ENTER SELECTED WORLD");

        SetRect(
            enter.gameObject,
            .05f,.03f,
            .95f,.13f);
    }

    private static void BuildLegend(Transform canvas)
    {
        GameObject panel = Panel(
            canvas,
            "HUD-201-05 GalaxyLegendPanel",
            new Vector2(.02f,.14f),
            new Vector2(.19f,.54f),
            new Vector2(12,4),
            new Vector2(-6,-4));

        string[] labels =
        {
            "FACTION COLOR LEGEND",
            "WARFARE ZONE LEGEND",
            "RESOURCE LEGEND",
            "ENVIRONMENT LEGEND"
        };

        for(int i=0;i<4;i++)
        {
            GameObject entry =
                ChildPanel(
                    panel.transform,
                    "HUD-201-05" +
                    (char)('A'+i) +
                    " " +
                    labels[i].Replace(" ",""),
                    .73f-(i*.17f),
                    .88f-(i*.17f));

            Label(
                entry.transform,
                "Value",
                labels[i] + "\n────────────────",
                11);
        }
    }

    private static void BuildQuickActions(Transform canvas)
    {
        GameObject panel = Panel(
            canvas,
            "HUD-201-06 QuickActionsPanel",
            new Vector2(.02f,.55f),
            new Vector2(.19f,.90f),
            new Vector2(12,4),
            new Vector2(-6,-4));

        string[] names =
        {
            "HUD-201-06A JumpToHomeworldButton",
            "HUD-201-06B JumpToLastViewedPlanetButton",
            "HUD-201-06C OpenIntelNetworkButton",
            "HUD-201-06D OpenDoctrineManagerButton",
            "HUD-201-06E OpenLogisticsOverviewButton"
        };

        string[] labels =
        {
            "HOMEWORLD",
            "LAST VIEWED PLANET",
            "INTEL NETWORK",
            "DOCTRINE MANAGER",
            "LOGISTICS OVERVIEW"
        };

        for(int i=0;i<5;i++)
        {
            Button b =
                Button(panel.transform,names[i],labels[i]);

            float top =
                .94f - (i*.17f);

            SetRect(
                b.gameObject,
                .05f,
                top-.14f,
                .95f,
                top);
        }
    }

    private static void BuildAlerts(Transform canvas)
    {
        GameObject panel = Panel(
            canvas,
            "HUD-201-07 AlertsPanel",
            new Vector2(.81f,.02f),
            new Vector2(.99f,.13f),
            new Vector2(4,4),
            new Vector2(-12,-4));

        string[] names =
        {
            "HUD-201-07A StrategicAlertEntry",
            "HUD-201-07B WarfareAlertEntry",
            "HUD-201-07C FactionAlertEntry",
            "HUD-201-07D ResourceAlertEntry"
        };

        string[] values =
        {
            "STRATEGIC  //  03",
            "WARFARE    //  07",
            "FACTION    //  02",
            "RESOURCE   //  04"
        };

        for(int i=0;i<4;i++)
        {
            GameObject entry =
                ChildPanel(
                    panel.transform,
                    names[i],
                    .04f,
                    .96f);

            RectTransform rt =
                entry.GetComponent<RectTransform>();

            float left =
                .01f + i*.2475f;

            rt.anchorMin =
                new Vector2(left,.12f);

            rt.anchorMax =
                new Vector2(left+.235f,.88f);

            Label(
                entry.transform,
                "Value",
                values[i],
                10,
                TextAlignmentOptions.Center);
        }
    }

    private static void BuildBottomBar(Transform canvas)
    {
        GameObject bar = Panel(
            canvas,
            "HUD-201-08 BottomBar",
            new Vector2(.20f,.02f),
            new Vector2(.80f,.11f),
            new Vector2(4,4),
            new Vector2(-4,-4));

        string[] names =
        {
            "HUD-201-08A BackButton",
            "HUD-201-08B ExitToCommandCenterButton",
            "HUD-201-08C SettingsButton",
            "HUD-201-08D HelpButton"
        };

        string[] labels =
        {
            "BACK",
            "COMMAND CENTER",
            "SETTINGS",
            "HELP"
        };

        for(int i=0;i<4;i++)
        {
            Button b =
                Button(bar.transform,names[i],labels[i]);

            float left =
                .02f + i*.245f;

            SetRect(
                b.gameObject,
                left,.15f,
                left+.225f,.85f);
        }
    }

    private static GameObject ChildPanel(
        Transform parent,
        string name,
        float yMin,
        float yMax)
    {
        return Panel(
            parent,
            name,
            new Vector2(.05f,yMin),
            new Vector2(.95f,yMax),
            Vector2.zero,
            Vector2.zero);
    }

    private static void SetRect(
        GameObject go,
        float xMin,
        float yMin,
        float xMax,
        float yMax)
    {
        RectTransform rt =
            go.GetComponent<RectTransform>();

        rt.anchorMin =
            new Vector2(xMin,yMin);

        rt.anchorMax =
            new Vector2(xMax,yMax);

        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    private static void SelectPlanet(int index)
    {
        if (index < 0 || index >= PlanetNames.Length)
            return;

        Debug.Log(
            "[OPAW] GALAXY MAP SELECTED: " +
            PlanetIDs[index] +
            " // " +
            PlanetNames[index] +
            " // " +
            SceneNames[index]);
    }

    private static void AddSceneToBuildSettings(string scenePath)
    {
        List<EditorBuildSettingsScene> scenes =
            new List<EditorBuildSettingsScene>(
                EditorBuildSettings.scenes);

        for(int i=0;i<scenes.Count;i++)
        {
            if(scenes[i].path == scenePath)
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






