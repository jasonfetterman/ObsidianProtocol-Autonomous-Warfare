using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public static class StarSystemHUD202Builder
{
    private const string SceneFolder =
        "Assets/Scenes/SCN61_STAR_SYSTEM";

    private const string ScenePath =
        SceneFolder + "/Star_System.unity";

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

    private static readonly string[] Planets =
    {
        "PLN-01 ACHERON",
        "PLN-02 VIRELLIA",
        "PLN-03 KARTHOS",
        "PLN-04 SOLARA PRIME",
        "PLN-05 NOX MERIDIAN",
        "PLN-06 THALASSA IX",
        "PLN-07 DRAVONS REACH",
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
        "PLN-27 EONS CRADLE",
        "PLN-28 VIREX-THETA",
        "PLN-29 SOLARIS ECHO",
        "PLN-30 OBSIDIAN WORLD"
    };

    [MenuItem("Obsidian Protocol/Worlds/BUILD HUD-202 STAR SYSTEM")]
    public static void Build()
    {
        EnsureFolder();

        Scene scene = EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene,
            NewSceneMode.Single);

        BuildEnvironment();
        BuildCanvas();

        EditorSceneManager.SaveScene(scene, ScenePath);

        AddToBuildSettings(ScenePath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "[HUD-202] STAR SYSTEM BUILD COMPLETE: " +
            ScenePath);
    }

    private static void EnsureFolder()
    {
        if (!AssetDatabase.IsValidFolder(
            "Assets/Scenes"))
            AssetDatabase.CreateFolder(
                "Assets",
                "Scenes");

        if (!AssetDatabase.IsValidFolder(SceneFolder))
            AssetDatabase.CreateFolder(
                "Assets/Scenes",
                "SCN61_STAR_SYSTEM");
    }

    private static void BuildEnvironment()
    {
        GameObject root =
            new GameObject("STAR SYSTEM ENVIRONMENT");

        GameObject backdrop =
            GameObject.CreatePrimitive(
                PrimitiveType.Quad);

        backdrop.name =
            "STAR SYSTEM BACKDROP";

        backdrop.transform.SetParent(
            root.transform);

        backdrop.transform.position =
            new Vector3(0,0,8);

        backdrop.transform.rotation =
            Quaternion.Euler(0,180,0);

        backdrop.transform.localScale =
            new Vector3(30,18,1);

        Material mat =
            new Material(
                Shader.Find("Unlit/Color"));

        mat.color =
            new Color(.004f,.008f,.018f,1);

        backdrop.GetComponent<Renderer>().sharedMaterial =
            mat;

        Object.DestroyImmediate(
            backdrop.GetComponent<Collider>());

        for(int i=0;i<80;i++)
        {
            GameObject star =
                GameObject.CreatePrimitive(
                    PrimitiveType.Sphere);

            star.name =
                "STAR_" + i.ToString("00");

            star.transform.SetParent(
                root.transform);

            float x =
                Mathf.Sin(i * 17.31f) * 14f;

            float y =
                Mathf.Cos(i * 9.17f) * 8f;

            float z =
                4f + (i % 6) * .5f;

            star.transform.position =
                new Vector3(x,y,z);

            star.transform.localScale =
                Vector3.one *
                (.025f + (i % 3)*.018f);

            Material starMat =
                new Material(
                    Shader.Find("Unlit/Color"));

            starMat.color =
                Color.Lerp(
                    Color.white,
                    Accent,
                    (i%5)*.08f);

            star.GetComponent<Renderer>().sharedMaterial =
                starMat;

            Object.DestroyImmediate(
                star.GetComponent<Collider>());
        }

        GameObject sun =
            GameObject.CreatePrimitive(
                PrimitiveType.Sphere);

        sun.name = "SYSTEM STAR";

        sun.transform.SetParent(
            root.transform);

        sun.transform.position =
            Vector3.zero;

        sun.transform.localScale =
            Vector3.one * 3.2f;

        Material sunMat =
            new Material(
                Shader.Find("Unlit/Color"));

        sunMat.color =
            new Color(1f,.55f,.12f,1f);

        sun.GetComponent<Renderer>().sharedMaterial =
            sunMat;

        Object.DestroyImmediate(
            sun.GetComponent<Collider>());

        for(int i=0;i<Planets.Length;i++)
        {
            float angle =
                i * Mathf.PI * 2f /
                Planets.Length;

            float radius =
                2.2f + i*.24f;

            GameObject planet =
                GameObject.CreatePrimitive(
                    PrimitiveType.Sphere);

            planet.name =
                "SYSTEM_PLANET_" +
                Planets[i].Replace(" ","_");

            planet.transform.SetParent(
                root.transform);

            planet.transform.position =
                new Vector3(
                    Mathf.Cos(angle)*radius,
                    Mathf.Sin(angle)*radius,
                    1f);

            planet.transform.localScale =
                Vector3.one *
                (.18f + (i%4)*.035f);

            Material planetMat =
                new Material(
                    Shader.Find("Unlit/Color"));

            planetMat.color =
                i == 29
                ? Threat
                : Accent;

            planet.GetComponent<Renderer>().sharedMaterial =
                planetMat;
        }

        GameObject cameraObject =
            new GameObject("STAR SYSTEM CAMERA");

        Camera camera =
            cameraObject.AddComponent<Camera>();

        camera.orthographic = true;
        camera.orthographicSize = 9f;
        camera.transform.position =
            new Vector3(0,0,-10);

        camera.clearFlags =
            CameraClearFlags.SolidColor;

        camera.backgroundColor =
            new Color(.002f,.004f,.012f,1);
    }

    private static void BuildCanvas()
    {
        GameObject canvasObject =
            new GameObject(
                "HUD-202 StarSystemHUD",
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
            new Vector2(1920,1080);

        scaler.screenMatchMode =
            CanvasScaler.ScreenMatchMode.Expand;

        GameObject eventSystem =
            new GameObject(
                "EventSystem",
                typeof(EventSystem),
                typeof(UnityEngine.InputSystem.UI.InputSystemUIInputModule));

        BuildTopBar(canvasObject.transform);
        BuildNavigation(canvasObject.transform);
        BuildMapLayers(canvasObject.transform);
        BuildObjectInfo(canvasObject.transform);
        BuildOrbit(canvasObject.transform);
        BuildQuickActions(canvasObject.transform);
        BuildAlerts(canvasObject.transform);
        BuildBottomBar(canvasObject.transform);
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
            parent,false);

        RectTransform rt =
            go.GetComponent<RectTransform>();

        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.offsetMin = offsetMin;
        rt.offsetMax = offsetMax;

        Image image =
            go.GetComponent<Image>();

        image.color = PanelColor;
        image.raycastTarget = false;

        return go;
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

    private static TextMeshProUGUI Label(
        Transform parent,
        string name,
        string value,
        float size,
        TextAlignmentOptions alignment =
            TextAlignmentOptions.Left)
    {
        GameObject go =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(TextMeshProUGUI));

        go.transform.SetParent(
            parent,false);

        RectTransform rt =
            go.GetComponent<RectTransform>();

        rt.anchorMin =
            new Vector2(.05f,.05f);

        rt.anchorMax =
            new Vector2(.95f,.95f);

        rt.offsetMin =
            Vector2.zero;

        rt.offsetMax =
            Vector2.zero;

        TextMeshProUGUI text =
            go.GetComponent<TextMeshProUGUI>();

        text.text = value;
        text.fontSize = size;
        text.color = Color.white;
        text.alignment = alignment;
        text.raycastTarget = false;
        text.textWrappingMode =
            TextWrappingModes.Normal;

        return text;
    }

    private static Button Button(
        Transform parent,
        string name,
        string label,
        UnityEngine.Events.UnityAction action = null)
    {
        GameObject go =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image),
                typeof(Button));

        go.transform.SetParent(
            parent,false);

        Image image =
            go.GetComponent<Image>();

        image.color = Panel2;
        image.raycastTarget = true;

        Button button =
            go.GetComponent<Button>();

        if(action != null)
            button.onClick.AddListener(action);

        GameObject labelObject =
            new GameObject(
                "LABEL",
                typeof(RectTransform),
                typeof(TextMeshProUGUI));

        labelObject.transform.SetParent(
            go.transform,false);

        RectTransform rt =
            labelObject.GetComponent<RectTransform>();

        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        TextMeshProUGUI text =
            labelObject.GetComponent<TextMeshProUGUI>();

        text.text = label;
        text.fontSize = 13;
        text.color = Color.white;
        text.alignment =
            TextAlignmentOptions.Center;
        text.raycastTarget = false;

        return button;
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

        rt.offsetMin =
            Vector2.zero;

        rt.offsetMax =
            Vector2.zero;
    }

    private static void BuildTopBar(
        Transform canvas)
    {
        GameObject panel = Panel(
            canvas,
            "HUD-202-01 TopBar",
            new Vector2(0,.91f),
            new Vector2(1,1),
            new Vector2(12,8),
            new Vector2(-12,-8));

        Label(
            panel.transform,
            "HUD-202-01A CurrentSystemNameDisplay",
            "SYSTEM // OBSIDIAN THEATER",
            22);

        Label(
            panel.transform,
            "HUD-202-01B CurrentStarNameDisplay",
            "STAR // PRIMARY",
            15);

        GameObject faction = Panel(
            panel.transform,
            "HUD-202-01C PlayerFactionEmblem",
            new Vector2(.64f,0),
            new Vector2(.73f,1),
            new Vector2(4,4),
            new Vector2(-4,-4));

        Label(
            faction.transform,
            "FactionEmblem",
            "OP",
            25,
            TextAlignmentOptions.Center);

        GameObject resources = Panel(
            panel.transform,
            "HUD-202-01D PlayerResourcesPanel",
            new Vector2(.73f,0),
            new Vector2(.88f,1),
            new Vector2(4,4),
            new Vector2(-4,-4));

        Label(
            resources.transform,
            "Resources",
            "CREDITS  125,400\nMATERIAL  84,250\nENERGY  67,800",
            12);

        GameObject time = Panel(
            panel.transform,
            "HUD-202-01E TimeControlPanel",
            new Vector2(.88f,0),
            new Vector2(1,1),
            new Vector2(4,4),
            new Vector2(-4,-4));

        Label(
            time.transform,
            "Time",
            "T+ 184:27:16\nPAUSED",
            12,
            TextAlignmentOptions.Center);
    }

    private static void BuildNavigation(
        Transform canvas)
    {
        GameObject panel = Panel(
            canvas,
            "HUD-202-02 NavigationPanel",
            new Vector2(0,.57f),
            new Vector2(.19f,.90f),
            new Vector2(12,8),
            new Vector2(-6,-8));

        Button zoomIn = Button(
            panel.transform,
            "HUD-202-02A SystemZoomInButton",
            "+  ZOOM IN");

        SetRect(
            zoomIn.gameObject,
            .05f,.80f,.95f,.96f);

        Button zoomOut = Button(
            panel.transform,
            "HUD-202-02B SystemZoomOutButton",
            "-  ZOOM OUT");

        SetRect(
            zoomOut.gameObject,
            .05f,.63f,.95f,.79f);

        GameObject list = Panel(
            panel.transform,
            "HUD-202-02C PlanetListDropdown",
            new Vector2(.05f,.46f),
            new Vector2(.95f,.61f),
            Vector2.zero,
            Vector2.zero);

        Label(
            list.transform,
            "PlanetListValue",
            "PLANET // SELECT",
            12,
            TextAlignmentOptions.Center);

        GameObject search = Panel(
            panel.transform,
            "HUD-202-02D SystemSearchField",
            new Vector2(.05f,.27f),
            new Vector2(.95f,.43f),
            Vector2.zero,
            Vector2.zero);

        Label(
            search.transform,
            "SearchValue",
            "SEARCH SYSTEM...",
            12,
            TextAlignmentOptions.Center);

        Button filter = Button(
            panel.transform,
            "HUD-202-02E FilterPanelButton",
            "FILTERS");

        SetRect(
            filter.gameObject,
            .05f,.06f,.95f,.22f);
    }

    private static void BuildMapLayers(
        Transform canvas)
    {
        GameObject panel = Panel(
            canvas,
            "HUD-202-03 MapLayerPanel",
            new Vector2(.81f,.52f),
            new Vector2(.99f,.90f),
            new Vector2(4,4),
            new Vector2(-12,-8));

        string[] names =
        {
            "HUD-202-03A LayerToggle_Star",
            "HUD-202-03B LayerToggle_Planets",
            "HUD-202-03C LayerToggle_Orbits",
            "HUD-202-03D LayerToggle_Factions",
            "HUD-202-03E LayerToggle_WarfareZones",
            "HUD-202-03F LayerToggle_Resources",
            "HUD-202-03G LayerToggle_LogisticsRoutes"
        };

        string[] labels =
        {
            "STAR",
            "PLANETS",
            "ORBITS",
            "FACTIONS",
            "WARFARE ZONES",
            "RESOURCES",
            "LOGISTICS ROUTES"
        };

        for(int i=0;i<names.Length;i++)
        {
            Button b = Button(
                panel.transform,
                names[i],
                labels[i]);

            float top =
                .94f - i*.13f;

            SetRect(
                b.gameObject,
                .05f,
                top-.10f,
                .95f,
                top);
        }
    }

    private static void BuildObjectInfo(
        Transform canvas)
    {
        GameObject panel = Panel(
            canvas,
            "HUD-202-04 SystemObjectInfoPanel",
            new Vector2(.81f,.14f),
            new Vector2(.99f,.50f),
            new Vector2(4,4),
            new Vector2(-12,-4));

        Label(
            panel.transform,
            "HUD-202-04A ObjectNameDisplay",
            "OBJECT // NO SELECTION",
            17);

        GameObject type =
            ChildPanel(
                panel.transform,
                "HUD-202-04B ObjectTypeDisplay",
                .64f,.86f);

        Label(
            type.transform,
            "Value",
            "TYPE // PLANET",
            11);

        GameObject owner =
            ChildPanel(
                panel.transform,
                "HUD-202-04C ObjectOwnerFactionDisplay",
                .49f,.61f);

        Label(
            owner.transform,
            "Value",
            "OWNER // OBSIDIAN PROTOCOL",
            11);

        GameObject status =
            ChildPanel(
                panel.transform,
                "HUD-202-04D ObjectStatusDisplay",
                .34f,.46f);

        Label(
            status.transform,
            "Value",
            "STATUS // OPERATIONAL",
            11);

        GameObject threat =
            ChildPanel(
                panel.transform,
                "HUD-202-04E ObjectThreatLevelDisplay",
                .19f,.31f);

        Label(
            threat.transform,
            "Value",
            "THREAT // ELEVATED",
            11);

        Button enter = Button(
            panel.transform,
            "HUD-202-04F ObjectEnterButton",
            "ENTER SELECTED WORLD");

        SetRect(
            enter.gameObject,
            .05f,.03f,.95f,.14f);
    }

    private static void BuildOrbit(
        Transform canvas)
    {
        GameObject panel = Panel(
            canvas,
            "HUD-202-05 OrbitPanel",
            new Vector2(.02f,.14f),
            new Vector2(.19f,.54f),
            new Vector2(12,4),
            new Vector2(-6,-4));

        string[] names =
        {
            "HUD-202-05A OrbitPathToggle",
            "HUD-202-05B OrbitalVelocityIndicator",
            "HUD-202-05C OrbitalDistanceDisplay",
            "HUD-202-05D OrbitalStabilityIndicator"
        };

        string[] values =
        {
            "ORBIT PATH // VISIBLE",
            "VELOCITY // 27.4 KM/S",
            "DISTANCE // 2.74 AU",
            "STABILITY // STABLE"
        };

        for(int i=0;i<4;i++)
        {
            GameObject entry =
                ChildPanel(
                    panel.transform,
                    names[i],
                    .75f-i*.18f,
                    .90f-i*.18f);

            Label(
                entry.transform,
                "Value",
                values[i],
                11);
        }
    }

    private static void BuildQuickActions(
        Transform canvas)
    {
        GameObject panel = Panel(
            canvas,
            "HUD-202-06 QuickActionsPanel",
            new Vector2(.02f,.55f),
            new Vector2(.19f,.90f),
            new Vector2(12,4),
            new Vector2(-6,-4));

        string[] names =
        {
            "HUD-202-06A JumpToGalaxyMapButton",
            "HUD-202-06B JumpToPlanetOverviewButton",
            "HUD-202-06C OpenIntelNetworkButton",
            "HUD-202-06D OpenDoctrineManagerButton",
            "HUD-202-06E OpenLogisticsOverviewButton"
        };

        string[] labels =
        {
            "GALAXY MAP",
            "PLANET OVERVIEW",
            "INTEL NETWORK",
            "DOCTRINE MANAGER",
            "LOGISTICS OVERVIEW"
        };

        for(int i=0;i<5;i++)
        {
            Button b = Button(
                panel.transform,
                names[i],
                labels[i]);

            float top =
                .94f-i*.17f;

            SetRect(
                b.gameObject,
                .05f,
                top-.14f,
                .95f,
                top);
        }
    }

    private static void BuildAlerts(
        Transform canvas)
    {
        GameObject panel = Panel(
            canvas,
            "HUD-202-07 AlertsPanel",
            new Vector2(.81f,.02f),
            new Vector2(.99f,.13f),
            new Vector2(4,4),
            new Vector2(-12,-4));

        string[] names =
        {
            "HUD-202-07A StrategicAlertEntry",
            "HUD-202-07B WarfareAlertEntry",
            "HUD-202-07C FactionAlertEntry",
            "HUD-202-07D ResourceAlertEntry"
        };

        string[] values =
        {
            "STRATEGIC // 03",
            "WARFARE // 02",
            "FACTION // 01",
            "RESOURCE // 04"
        };

        for(int i=0;i<4;i++)
        {
            GameObject entry =
                ChildPanel(
                    panel.transform,
                    names[i],
                    .08f,.90f);

            Label(
                entry.transform,
                "Value",
                values[i],
                10);
        }
    }

    private static void BuildBottomBar(
        Transform canvas)
    {
        GameObject bar = Panel(
            canvas,
            "HUD-202-08 BottomBar",
            new Vector2(.20f,.02f),
            new Vector2(.80f,.13f),
            new Vector2(4,4),
            new Vector2(-4,-4));

        string[] names =
        {
            "HUD-202-08A BackButton",
            "HUD-202-08B ExitToCommandCenterButton",
            "HUD-202-08C SettingsButton",
            "HUD-202-08D HelpButton"
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
            Button b = Button(
                bar.transform,
                names[i],
                labels[i]);

            float left =
                .02f + i*.245f;

            SetRect(
                b.gameObject,
                left,
                .12f,
                left+.225f,
                .88f);
        }
    }

    private static void AddToBuildSettings(
        string scenePath)
    {
        List<EditorBuildSettingsScene> scenes =
            new List<EditorBuildSettingsScene>(
                EditorBuildSettings.scenes);

        bool exists = false;

        foreach(
            EditorBuildSettingsScene scene
            in scenes)
        {
            if(scene.path == scenePath)
            {
                exists = true;
                break;
            }
        }

        if(!exists)
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
