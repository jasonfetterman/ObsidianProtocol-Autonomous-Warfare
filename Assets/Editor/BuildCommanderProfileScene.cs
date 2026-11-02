using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class BuildCommanderProfileScene
{
    private const float W = 1920f;
    private const float H = 1080f;

    private static readonly Color BG =
        new Color(0.006f, 0.010f, 0.016f, 1f);

    private static readonly Color PANEL =
        new Color(0.018f, 0.030f, 0.040f, 0.985f);

    private static readonly Color PANEL2 =
        new Color(0.025f, 0.043f, 0.055f, 1f);

    private static readonly Color LINE =
        new Color(0.08f, 0.30f, 0.36f, 1f);

    private static readonly Color CYAN =
        new Color(0.20f, 0.84f, 0.96f, 1f);

    private static readonly Color CYAN_DARK =
        new Color(0.035f, 0.22f, 0.28f, 1f);

    private static readonly Color WHITE =
        new Color(0.88f, 0.94f, 0.96f, 1f);

    private static readonly Color MUTED =
        new Color(0.43f, 0.55f, 0.60f, 1f);

    private static readonly Color GREEN =
        new Color(0.25f, 0.88f, 0.52f, 1f);

    private static readonly Color AMBER =
        new Color(0.96f, 0.68f, 0.20f, 1f);

    private static readonly Color RED =
        new Color(0.85f, 0.18f, 0.20f, 1f);

    private static Canvas canvas;
    private static Transform canvasRoot;
    private static Camera profileCamera;

    [MenuItem("Obsidian Protocol/Build/SCN-09 Commander Profile")]
    public static void Build()
    {
        string scenePath = FindCommanderProfileScene();

        EnsureFolder(Path.GetDirectoryName(scenePath));

        Scene scene = EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene,
            NewSceneMode.Single
        );

        CreateCamera();
        CreateEventSystem();
        CreateCanvas();

        GameObject controllerHost =
            new GameObject(
                "CommanderProfileController",
                typeof(CommanderProfileController)
            );

        controllerHost.transform.SetParent(
            canvas.transform,
            false
        );

        CreateBackground();
        CreateTopProtocolBar();
        CreateNavigation();
        CreateContent();

        CreateDoctrineWindow();
        CreateFooter();

        Selection.activeGameObject = canvas.gameObject;

        EditorSceneManager.SaveScene(scene, scenePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "OBSIDIAN PROTOCOL COMMANDER PROFILE: FULL BUILD COMPLETE - CAMERA + HUD + NAVIGATION + PROFILE DATA"
        );
    }

    private static string FindCommanderProfileScene()
    {
        string[] guids =
            AssetDatabase.FindAssets(
                "Player_Commander_Profile t:Scene"
            );

        foreach (string guid in guids)
        {
            string path =
                AssetDatabase.GUIDToAssetPath(guid);

            if (
                path.EndsWith(
                    "Player_Commander_Profile.unity",
                    StringComparison.OrdinalIgnoreCase
                )
            )
            {
                return path;
            }
        }

        return "Assets/Scenes/SCN-09  COMMANDER PROFILE/[HUD] COMMANDER PROFILE HUD/Player_Commander_Profile.unity";
    }

    private static void EnsureFolder(string folder)
    {
        if (string.IsNullOrEmpty(folder))
            return;

        folder = folder.Replace("\\", "/");

        string[] parts = folder.Split('/');

        if (parts.Length == 0)
            return;

        string current = parts[0];

        for (int i = 1; i < parts.Length; i++)
        {
            string next = current + "/" + parts[i];

            if (!AssetDatabase.IsValidFolder(next))
                AssetDatabase.CreateFolder(current, parts[i]);

            current = next;
        }
    }

    private static void CreateCamera()
    {
        GameObject oldCamera =
            GameObject.Find("Main Camera");

        if (oldCamera != null)
            UnityEngine.Object.DestroyImmediate(oldCamera);

        oldCamera =
            GameObject.Find("MainCamera");

        if (oldCamera != null)
            UnityEngine.Object.DestroyImmediate(oldCamera);

        GameObject cameraObject =
            new GameObject(
                "MainCamera",
                typeof(Transform),
                typeof(Camera),
                typeof(AudioListener)
            );

        profileCamera =
            cameraObject.GetComponent<Camera>();

        profileCamera.clearFlags =
            CameraClearFlags.SolidColor;

        profileCamera.backgroundColor = BG;

        profileCamera.orthographic = true;
        profileCamera.orthographicSize = H / 2f;

        profileCamera.nearClipPlane = -100f;
        profileCamera.farClipPlane = 100f;

        cameraObject.transform.position =
            new Vector3(0f, 0f, -10f);

        cameraObject.transform.rotation =
            Quaternion.identity;

        cameraObject.tag = "MainCamera";

        profileCamera.enabled = true;
    }

    private static void CreateEventSystem()
    {
        GameObject[] systems =
            UnityEngine.Object.FindObjectsByType<GameObject>(
                FindObjectsSortMode.None
            );

        foreach (GameObject go in systems)
        {
            if (go.GetComponent<EventSystem>() != null)
                UnityEngine.Object.DestroyImmediate(go);
        }

        GameObject eventSystem =
            new GameObject(
                "EventSystem",
                typeof(EventSystem),
                typeof(InputSystemUIInputModule)
            );

        eventSystem.transform.SetParent(
            null,
            false
        );
    }

    private static void CreateCanvas()
    {
        GameObject go =
            new GameObject(
                "Canvas_CommanderProfileHUD",
                typeof(RectTransform),
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster)
            );

        canvas =
            go.GetComponent<Canvas>();

        canvas.renderMode =
            RenderMode.ScreenSpaceCamera;

        canvas.worldCamera =
            profileCamera;

        canvas.planeDistance = 1f;

        CanvasScaler scaler =
            go.GetComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(W, H);

        scaler.screenMatchMode =
            CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        scaler.matchWidthOrHeight = 0.5f;

        canvasRoot =
            canvas.transform;
    }

    private static GameObject Rect(
        string name,
        Transform parent,
        float x,
        float y,
        float w,
        float h)
    {
        GameObject go =
            new GameObject(
                name,
                typeof(RectTransform)
            );

        RectTransform rt =
            go.GetComponent<RectTransform>();

        rt.SetParent(parent, false);

        rt.anchorMin =
            new Vector2(0f, 0f);

        rt.anchorMax =
            new Vector2(0f, 0f);

        rt.pivot =
            new Vector2(0f, 0f);

        rt.anchoredPosition =
            new Vector2(x, y);

        rt.sizeDelta =
            new Vector2(w, h);

        return go;
    }

    private static GameObject Box(
        string name,
        Transform parent,
        float x,
        float y,
        float w,
        float h,
        Color color)
    {
        GameObject go =
            Rect(
                name,
                parent,
                x,
                y,
                w,
                h
            );

        Image image =
            go.AddComponent<Image>();

        image.color = color;

        image.raycastTarget = false;

        return go;
    }

    private static Text Label(
        string name,
        Transform parent,
        string textValue,
        float x,
        float y,
        float w,
        float h,
        int fontSize,
        Color color,
        TextAnchor alignment =
            TextAnchor.MiddleLeft)
    {
        GameObject go =
            Rect(
                name,
                parent,
                x,
                y,
                w,
                h
            );

        Text text =
            go.AddComponent<Text>();

        text.text = textValue;

        text.font =
            Resources.GetBuiltinResource<Font>(
                "LegacyRuntime.ttf"
            );

        text.fontSize = fontSize;
        text.color = color;
        text.alignment = alignment;

        text.horizontalOverflow =
            HorizontalWrapMode.Overflow;

        text.verticalOverflow =
            VerticalWrapMode.Overflow;

        text.raycastTarget = false;

        return text;
    }

    private static GameObject Button(
        string name,
        Transform parent,
        string caption,
        float x,
        float y,
        float w,
        float h,
        int fontSize = 13)
    {
        GameObject go =
            Rect(
                name,
                parent,
                x,
                y,
                w,
                h
            );

        Image image =
            go.AddComponent<Image>();

        image.color =
            new Color(
                0.022f,
                0.043f,
                0.055f,
                1f
            );

        image.raycastTarget = true;

        Button button =
            go.AddComponent<Button>();

        ColorBlock colors =
            button.colors;

        colors.normalColor =
            new Color(
                0.022f,
                0.043f,
                0.055f,
                1f
            );

        colors.highlightedColor =
            new Color(
                0.045f,
                0.19f,
                0.24f,
                1f
            );

        colors.pressedColor =
            new Color(
                0.06f,
                0.30f,
                0.36f,
                1f
            );

        colors.selectedColor =
            new Color(
                0.05f,
                0.22f,
                0.28f,
                1f
            );

        colors.disabledColor =
            new Color(
                0.02f,
                0.03f,
                0.035f,
                1f
            );

        button.colors = colors;

        Label(
            "LABEL",
            go.transform,
            caption,
            0f,
            0f,
            w,
            h,
            fontSize,
            WHITE,
            TextAnchor.MiddleCenter
        );

        return go;
    }

    private static void Border(
        Transform parent,
        float w,
        float h)
    {
        Box(
            "BORDER_TOP",
            parent,
            0f,
            h - 1f,
            w,
            1f,
            LINE
        );

        Box(
            "BORDER_BOTTOM",
            parent,
            0f,
            0f,
            w,
            1f,
            LINE
        );

        Box(
            "BORDER_LEFT",
            parent,
            0f,
            0f,
            1f,
            h,
            LINE
        );

        Box(
            "BORDER_RIGHT",
            parent,
            w - 1f,
            0f,
            1f,
            h,
            LINE
        );
    }

    private static GameObject Panel(
        string name,
        Transform parent,
        float x,
        float y,
        float w,
        float h)
    {
        GameObject panel =
            Box(
                name,
                parent,
                x,
                y,
                w,
                h,
                PANEL
            );

        Border(
            panel.transform,
            w,
            h
        );

        return panel;
    }

    private static void CreateBackground()
    {
        GameObject bg =
            Box(
                "BACKGROUND",
                canvasRoot,
                0f,
                0f,
                W,
                H,
                BG
            );

        bg.transform.SetAsFirstSibling();

        for (int x = 0; x <= 1920; x += 80)
        {
            Box(
                "GRID_V_" + x,
                canvasRoot,
                x,
                0f,
                1f,
                H,
                new Color(
                    0.03f,
                    0.10f,
                    0.12f,
                    0.28f
                )
            );
        }

        for (int y = 0; y <= 1080; y += 80)
        {
            Box(
                "GRID_H_" + y,
                canvasRoot,
                0f,
                y,
                W,
                1f,
                new Color(
                    0.03f,
                    0.10f,
                    0.12f,
                    0.28f
                )
            );
        }

        Box(
            "TOP_GLOW",
            canvasRoot,
            0f,
            1010f,
            W,
            70f,
            new Color(
                0.02f,
                0.18f,
                0.23f,
                0.18f
            )
        );
    }

    private static void CreateTopProtocolBar()
    {
        GameObject bar =
            Panel(
                "TOP_OBSIDIAN_PROTOCOL_BAR",
                canvasRoot,
                0f,
                1000f,
                W,
                80f
            );

        Label(
            "GAME_TITLE",
            bar.transform,
            "OBSIDIAN PROTOCOL",
            28f,
            25f,
            330f,
            35f,
            23,
            WHITE
        );

        Label(
            "GAME_SUBTITLE",
            bar.transform,
            "AUTONOMOUS WARFARE",
            28f,
            7f,
            330f,
            20f,
            10,
            CYAN
        );

        Button(
            "BUTTON_PROTOCOL_DROPDOWN",
            bar.transform,
            "COMMAND ▼",
            1370f,
            18f,
            220f,
            44f,
            12
        );

        Label(
            "PROFILE_STATUS",
            bar.transform,
            "COMMANDER PROFILE  //  ONLINE",
            1610f,
            18f,
            280f,
            44f,
            11,
            GREEN,
            TextAnchor.MiddleRight
        );
    }

    private static void CreateNavigation()
    {
        GameObject nav =
            Panel(
                "MAIN_NAVIGATION",
                canvasRoot,
                0f,
                930f,
                W,
                70f
            );

        string[] names =
        {
            "PROFILE",
            "RANK",
            "PROGRESSION",
            "DOCTRINE",
            "STATISTICS",
            "ACHIEVEMENTS",
            "CAREER"
        };

        string[] objects =
        {
            "BUTTON_NAV_PROFILE",
            "BUTTON_NAV_RANK",
            "BUTTON_NAV_PROGRESSION",
            "BUTTON_NAV_DOCTRINE",
            "BUTTON_NAV_STATISTICS",
            "BUTTON_NAV_ACHIEVEMENTS",
            "BUTTON_NAV_CAREER"
        };

        float x = 20f;

        for (int i = 0; i < names.Length; i++)
        {
            Button(
                objects[i],
                nav.transform,
                names[i],
                x,
                14f,
                245f,
                42f,
                11
            );

            x += 265f;
        }
    }

    private static Transform CreatePage(
        string name,
        string title,
        string status)
    {
        GameObject page =
            Panel(
                name,
                canvasRoot,
                35f,
                95f,
                1850f,
                815f
            );

        Header(
            page.transform,
            title,
            1850f,
            815f,
            status
        );

        return page.transform;
    }

    private static void Header(
        Transform parent,
        string title,
        float w,
        float h,
        string status)
    {
        Label(
            "HEADER_TITLE",
            parent,
            title,
            26f,
            h - 55f,
            w - 360f,
            34f,
            18,
            CYAN
        );

        Label(
            "HEADER_STATUS",
            parent,
            status,
            w - 310f,
            h - 55f,
            280f,
            34f,
            10,
            GREEN,
            TextAnchor.MiddleRight
        );

        Box(
            "HEADER_LINE",
            parent,
            22f,
            h - 68f,
            w - 44f,
            1f,
            CYAN_DARK
        );
    }

    private static void CreateContent()
    {
        CreateProfilePage();
        CreateRankPage();
        CreateProgressionPage();
        CreateDoctrinePage();
        CreateStatisticsPage();
        CreateAchievementsPage();
        CreateCareerPage();
    }

    private static void CreateProfilePage()
    {
        Transform page =
            CreatePage(
                "PAGE_PROFILE",
                "COMMANDER PROFILE",
                "IDENTITY VERIFIED"
            );

        GameObject identity =
            Panel(
                "PANEL_IDENTITY",
                page,
                25f,
                400f,
                570f,
                325f
            );

        Label(
            "COMMANDER_NAME",
            identity.transform,
            "COMMANDER",
            25f,
            245f,
            520f,
            55f,
            30,
            WHITE
        );

        Label(
            "CALLSIGN",
            identity.transform,
            "CALLSIGN  //  OBSIDIAN",
            25f,
            210f,
            520f,
            30f,
            13,
            CYAN
        );

        Label(
            "COMMANDER_ID",
            identity.transform,
            "COMMANDER ID    OP-000001",
            25f,
            165f,
            520f,
            28f,
            12,
            MUTED
        );

        Label(
            "CLEARANCE",
            identity.transform,
            "CLEARANCE       OMEGA",
            25f,
            130f,
            520f,
            28f,
            12,
            GREEN
        );

        Label(
            "FACTION",
            identity.transform,
            "COMMAND AFFILIATION    OBSIDIAN",
            25f,
            95f,
            520f,
            28f,
            12,
            WHITE
        );

        Label(
            "STATUS",
            identity.transform,
            "STATUS           ACTIVE",
            25f,
            60f,
            520f,
            28f,
            12,
            GREEN
        );

        GameObject service =
            Panel(
                "PANEL_SERVICE_RECORD",
                page,
                620f,
                400f,
                580f,
                325f
            );

        Label(
            "SERVICE_HEADER",
            service.transform,
            "SERVICE RECORD",
            24f,
            260f,
            520f,
            30f,
            15,
            CYAN
        );

        string[] serviceLines =
        {
            "OPERATIONS COMPLETED        000",
            "BATTLES PARTICIPATED        000",
            "VICTORIES                    000",
            "DEFEATS                      000",
            "COMMAND HOURS                000",
            "UNITS DEPLOYED               000",
            "UNITS RECOVERED              000"
        };

        float sy = 220f;

        foreach (string line in serviceLines)
        {
            Label(
                "SERVICE_" + sy,
                service.transform,
                line,
                24f,
                sy,
                520f,
                25f,
                11,
                WHITE
            );

            sy -= 30f;
        }

        GameObject security =
            Panel(
                "PANEL_SECURITY",
                page,
                1225f,
                400f,
                600f,
                325f
            );

        Label(
            "SECURITY_HEADER",
            security.transform,
            "COMMAND SECURITY",
            24f,
            260f,
            540f,
            30f,
            15,
            CYAN
        );

        Label(
            "SECURITY_01",
            security.transform,
            "BIOMETRIC AUTHENTICATION       VERIFIED",
            24f,
            215f,
            540f,
            28f,
            11,
            GREEN
        );

        Label(
            "SECURITY_02",
            security.transform,
            "NEURAL COMMAND LINK             READY",
            24f,
            175f,
            540f,
            28f,
            11,
            GREEN
        );

        Label(
            "SECURITY_03",
            security.transform,
            "TACTICAL NETWORK ACCESS         OMEGA",
            24f,
            135f,
            540f,
            28f,
            11,
            WHITE
        );

        Label(
            "SECURITY_04",
            security.transform,
            "AUTONOMY AUTHORIZATION          ENABLED",
            24f,
            95f,
            540f,
            28f,
            11,
            GREEN
        );

        Label(
            "SECURITY_05",
            security.transform,
            "LAST AUTHENTICATION             CURRENT SESSION",
            24f,
            55f,
            540f,
            28f,
            11,
            MUTED
        );

        GameObject stats =
            Panel(
                "PANEL_PROFILE_SUMMARY",
                page,
                25f,
                65f,
                1800f,
                300f
            );

        Label(
            "SUMMARY_HEADER",
            stats.transform,
            "COMMANDER SUMMARY",
            24f,
            250f,
            600f,
            30f,
            15,
            CYAN
        );

        Label(
            "SUMMARY_TEXT",
            stats.transform,
            "Strategic commander authorized for autonomous warfare operations.",
            24f,
            205f,
            850f,
            30f,
            13,
            WHITE
        );

        Label(
            "SUMMARY_TEXT2",
            stats.transform,
            "Current doctrine: adaptive combined-arms command.",
            24f,
            165f,
            850f,
            30f,
            13,
            MUTED
        );

        Label(
            "SUMMARY_TEXT3",
            stats.transform,
            "Deployment authority, fleet command, research access and logistics control active.",
            24f,
            125f,
            850f,
            30f,
            13,
            MUTED
        );

        Label(
            "PROFILE_LEVEL",
            stats.transform,
            "COMMAND LEVEL     01",
            1030f,
            205f,
            650f,
            30f,
            15,
            CYAN,
            TextAnchor.MiddleRight
        );

        Label(
            "PROFILE_XP",
            stats.transform,
            "EXPERIENCE        0 / 1,000",
            1030f,
            165f,
            650f,
            30f,
            12,
            WHITE,
            TextAnchor.MiddleRight
        );

        Label(
            "PROFILE_STATUS2",
            stats.transform,
            "PROFILE STATE     OPERATIONAL",
            1030f,
            125f,
            650f,
            30f,
            12,
            GREEN,
            TextAnchor.MiddleRight
        );
    }

    private static void CreateRankPage()
    {
        Transform page =
            CreatePage(
                "PAGE_RANK",
                "COMMAND RANK",
                "RANK SYSTEM ONLINE"
            );

        GameObject rank =
            Panel(
                "PANEL_CURRENT_RANK",
                page,
                25f,
                475f,
                850f,
                250f
            );

        Label(
            "RANK_TITLE",
            rank.transform,
            "CURRENT RANK",
            25f,
            185f,
            790f,
            30f,
            13,
            MUTED
        );

        Label(
            "RANK_VALUE",
            rank.transform,
            "COMMANDER",
            25f,
            125f,
            790f,
            55f,
            30,
            WHITE
        );

        Label(
            "RANK_PROGRESS",
            rank.transform,
            "RANK PROGRESS       0 / 10,000 XP",
            25f,
            75f,
            790f,
            30f,
            12,
            CYAN
        );

        Box(
            "RANK_BAR",
            rank.transform,
            25f,
            45f,
            790f,
            8f,
            CYAN_DARK
        );

        GameObject next =
            Panel(
                "PANEL_NEXT_RANK",
                page,
                900f,
                475f,
                925f,
                250f
            );

        Label(
            "NEXT_HEADER",
            next.transform,
            "NEXT COMMAND TIER",
            25f,
            185f,
            875f,
            30f,
            13,
            MUTED
        );

        Label(
            "NEXT_VALUE",
            next.transform,
            "FIELD COMMANDER",
            25f,
            125f,
            875f,
            55f,
            30,
            WHITE
        );

        Label(
            "NEXT_UNLOCKS",
            next.transform,
            "UNLOCKS  //  ADVANCED DOCTRINES  //  FLEET COMMAND  //  HIGHER DEPLOYMENT AUTHORITY",
            25f,
            72f,
            875f,
            30f,
            11,
            CYAN
        );

        GameObject ladder =
            Panel(
                "PANEL_RANK_LADDER",
                page,
                25f,
                65f,
                1800f,
                380f
            );

        Label(
            "LADDER_HEADER",
            ladder.transform,
            "COMMAND LADDER",
            25f,
            325f,
            1700f,
            30f,
            15,
            CYAN
        );

        string[] ranks =
        {
            "01  COMMANDER",
            "02  FIELD COMMANDER",
            "03  STRATEGIC COMMANDER",
            "04  OPERATIONS DIRECTOR",
            "05  THEATER COMMANDER",
            "06  FLEET COMMANDER",
            "07  WAR COUNCIL",
            "08  SUPREME COMMAND",
            "09  OBSIDIAN AUTHORITY",
            "10  PROTOCOL ARCHITECT"
        };

        float ry = 280f;

        foreach (string rankName in ranks)
        {
            Label(
                "RANK_" + ry,
                ladder.transform,
                rankName,
                25f,
                ry,
                800f,
                25f,
                11,
                rankName.StartsWith("01")
                    ? CYAN
                    : MUTED
            );

            ry -= 27f;
        }
    }

    private static void CreateProgressionPage()
    {
        Transform page =
            CreatePage(
                "PAGE_PROGRESSION",
                "COMMAND PROGRESSION",
                "PROGRESSION TRACKING ACTIVE"
            );

        GameObject progression =
            Panel(
                "PANEL_PROGRESSION",
                page,
                25f,
                65f,
                1170f,
                660f
            );

        Label(
            "PROGRESSION_HEADER",
            progression.transform,
            "COMMAND DEVELOPMENT",
            25f,
            600f,
            1100f,
            30f,
            15,
            CYAN
        );

        string[] tracks =
        {
            "COMMAND EXPERIENCE",
            "TACTICAL EXPERIENCE",
            "STRATEGIC EXPERIENCE",
            "AUTONOMY EXPERIENCE",
            "LOGISTICS EXPERIENCE",
            "RESEARCH EXPERIENCE",
            "DEPLOYMENT EXPERIENCE",
            "SURVIVAL EXPERIENCE"
        };

        float py = 550f;

        foreach (string track in tracks)
        {
            Label(
                "TRACK_LABEL",
                progression.transform,
                track,
                25f,
                py,
                360f,
                25f,
                11,
                WHITE
            );

            Box(
                "TRACK_BG",
                progression.transform,
                400f,
                py + 5f,
                650f,
                10f,
                CYAN_DARK
            );

            Label(
                "TRACK_VALUE",
                progression.transform,
                "0%",
                1070f,
                py,
                70f,
                25f,
                11,
                MUTED,
                TextAnchor.MiddleRight
            );

            py -= 62f;
        }

        GameObject unlocks =
            Panel(
                "PANEL_UNLOCKS",
                page,
                1220f,
                65f,
                605f,
                660f
            );

        Label(
            "UNLOCK_HEADER",
            unlocks.transform,
            "UPCOMING UNLOCKS",
            24f,
            600f,
            550f,
            30f,
            15,
            CYAN
        );

        string[] unlockList =
        {
            "ADVANCED AUTONOMY",
            "TACTICAL NETWORKING",
            "FLEET MANAGEMENT",
            "AI PERSONALITY LAB",
            "FABRICATION CONTROL",
            "EXPERIMENTAL ACCESS",
            "THEATER DEPLOYMENT",
            "OBSIDIAN CLEARANCE"
        };

        float uy = 550f;

        foreach (string unlock in unlockList)
        {
            Box(
                "UNLOCK_MARKER",
                unlocks.transform,
                24f,
                uy + 7f,
                8f,
                8f,
                CYAN_DARK
            );

            Label(
                "UNLOCK_" + uy,
                unlocks.transform,
                unlock,
                48f,
                uy,
                500f,
                25f,
                11,
                WHITE
            );

            Label(
                "LOCKED_" + uy,
                unlocks.transform,
                "LOCKED",
                440f,
                uy,
                120f,
                25f,
                10,
                MUTED,
                TextAnchor.MiddleRight
            );

            uy -= 62f;
        }
    }

    private static void CreateDoctrinePage()
    {
        Transform page =
            CreatePage(
                "PAGE_DOCTRINE",
                "COMMAND DOCTRINE",
                "DOCTRINE CONTROL READY"
            );

        GameObject doctrine =
            Panel(
                "PANEL_ACTIVE_DOCTRINE",
                page,
                25f,
                400f,
                900f,
                325f
            );

        Label(
            "DOCTRINE_HEADER",
            doctrine.transform,
            "ACTIVE DOCTRINE",
            25f,
            260f,
            820f,
            30f,
            15,
            CYAN
        );

        Label(
            "DOCTRINE_NAME",
            doctrine.transform,
            "ADAPTIVE COMBINED ARMS",
            25f,
            200f,
            820f,
            50f,
            24,
            WHITE
        );

        Label(
            "DOCTRINE_DESCRIPTION",
            doctrine.transform,
            "Balances autonomous maneuver, reconnaissance, logistics and concentrated force.",
            25f,
            150f,
            820f,
            55f,
            12,
            MUTED
        );

        Label(
            "DOCTRINE_STATUS",
            doctrine.transform,
            "STATUS       ACTIVE",
            25f,
            90f,
            820f,
            30f,
            12,
            GREEN
        );

        Button(
            "BUTTON_DOCTRINE_LIBRARY",
            doctrine.transform,
            "OPEN DOCTRINE LIBRARY",
            25f,
            30f,
            330f,
            42f,
            11
        );

        GameObject principles =
            Panel(
                "PANEL_DOCTRINE_PRINCIPLES",
                page,
                950f,
                400f,
                875f,
                325f
            );

        Label(
            "PRINCIPLES_HEADER",
            principles.transform,
            "COMMAND PRINCIPLES",
            25f,
            260f,
            800f,
            30f,
            15,
            CYAN
        );

        string[] principlesList =
        {
            "COMMAND INTENT OVER MICRO-MANAGEMENT",
            "AUTONOMY WITHIN DEFINED OBJECTIVES",
            "RECONNAISSANCE BEFORE COMMITMENT",
            "LOGISTICS PRESERVES OPERATIONAL TEMPO",
            "ADAPTATION OVER STATIC FORMATIONS",
            "FORCE PRESERVATION WHEN OBJECTIVES PERMIT"
        };

        float dy = 215f;

        foreach (string principle in principlesList)
        {
            Label(
                "PRINCIPLE_" + dy,
                principles.transform,
                "• " + principle,
                25f,
                dy,
                800f,
                25f,
                11,
                WHITE
            );

            dy -= 34f;
        }

        GameObject modes =
            Panel(
                "PANEL_DOCTRINE_MODES",
                page,
                25f,
                65f,
                1800f,
                300f
            );

        Label(
            "MODES_HEADER",
            modes.transform,
            "TACTICAL BEHAVIOR PROFILES",
            25f,
            250f,
            1700f,
            30f,
            15,
            CYAN
        );

        string[] modesList =
        {
            "BALANCED",
            "AGGRESSIVE",
            "DEFENSIVE",
            "RECONNAISSANCE",
            "LOGISTICS",
            "AUTONOMOUS",
            "HOLD POSITION",
            "OBJECTIVE FOCUSED"
        };

        float mx = 25f;

        foreach (string mode in modesList)
        {
            Button(
                "DOCTRINE_MODE_" + mode.Replace(" ", "_"),
                modes.transform,
                mode,
                mx,
                175f,
                205f,
                45f,
                10
            );

            mx += 220f;

            if (mx > 1600f)
                mx = 25f;
        }
    }

    private static void CreateStatisticsPage()
    {
        Transform page =
            CreatePage(
                "PAGE_STATISTICS",
                "COMBAT STATISTICS",
                "STATISTICS DATABASE ONLINE"
            );

        GameObject combat =
            Panel(
                "PANEL_COMBAT_STATISTICS",
                page,
                25f,
                390f,
                570f,
                335f
            );

        Label(
            "COMBAT_HEADER",
            combat.transform,
            "COMBAT PERFORMANCE",
            25f,
            275f,
            520f,
            30f,
            15,
            CYAN
        );

        string[] combatStats =
        {
            "VICTORIES                 000",
            "DEFEATS                   000",
            "WIN RATE                  0.0%",
            "KILLS                     000",
            "LOSSES                    000",
            "K/D RATIO                 0.00",
            "OBJECTIVES COMPLETED      000",
            "OBJECTIVES LOST           000"
        };

        float cy = 230f;

        foreach (string line in combatStats)
        {
            Label(
                "COMBAT_" + cy,
                combat.transform,
                line,
                25f,
                cy,
                520f,
                25f,
                11,
                WHITE
            );

            cy -= 30f;
        }

        GameObject autonomous =
            Panel(
                "PANEL_AUTONOMY_STATISTICS",
                page,
                620f,
                390f,
                580f,
                335f
            );

        Label(
            "AUTONOMY_HEADER",
            autonomous.transform,
            "AUTONOMY PERFORMANCE",
            25f,
            275f,
            530f,
            30f,
            15,
            CYAN
        );

        string[] autonomyStats =
        {
            "AUTONOMOUS MISSIONS       000",
            "SUCCESSFUL COMMANDS       000",
            "FAILED COMMANDS           000",
            "AI DECISIONS EXECUTED     000",
            "AUTONOMOUS HOURS           000",
            "COMMAND OVERRIDES         000",
            "FORMATION ADAPTATIONS     000",
            "THREAT RESPONSES           000"
        };

        float ay = 230f;

        foreach (string line in autonomyStats)
        {
            Label(
                "AUTO_" + ay,
                autonomous.transform,
                line,
                25f,
                ay,
                530f,
                25f,
                11,
                WHITE
            );

            ay -= 30f;
        }

        GameObject logistics =
            Panel(
                "PANEL_LOGISTICS_STATISTICS",
                page,
                1225f,
                390f,
                600f,
                335f
            );

        Label(
            "LOGISTICS_HEADER",
            logistics.transform,
            "LOGISTICS PERFORMANCE",
            25f,
            275f,
            550f,
            30f,
            15,
            CYAN
        );

        string[] logisticsStats =
        {
            "SUPPLY MISSIONS            000",
            "RECOVERY MISSIONS          000",
            "REPAIRS COMPLETED          000",
            "UNITS FABRICATED           000",
            "UNITS RECOVERED            000",
            "RESOURCE TRANSFERS         000",
            "DEPLOYMENTS SUPPORTED      000",
            "FLEET READINESS             0%"
        };

        float ly = 230f;

        foreach (string line in logisticsStats)
        {
            Label(
                "LOG_" + ly,
                logistics.transform,
                line,
                25f,
                ly,
                550f,
                25f,
                11,
                WHITE
            );

            ly -= 30f;
        }

        GameObject totals =
            Panel(
                "PANEL_STATISTICS_TOTALS",
                page,
                25f,
                65f,
                1800f,
                285f
            );

        Label(
            "TOTALS_HEADER",
            totals.transform,
            "CAREER TOTALS",
            25f,
            235f,
            1700f,
            30f,
            15,
            CYAN
        );

        Label(
            "TOTAL_01",
            totals.transform,
            "TOTAL DEPLOYMENTS       000",
            25f,
            180f,
            500f,
            30f,
            12,
            WHITE
        );

        Label(
            "TOTAL_02",
            totals.transform,
            "TOTAL COMMAND TIME       000 H",
            25f,
            140f,
            500f,
            30f,
            12,
            WHITE
        );

        Label(
            "TOTAL_03",
            totals.transform,
            "TOTAL EXPERIENCE         000 XP",
            650f,
            180f,
            500f,
            30f,
            12,
            WHITE
        );

        Label(
            "TOTAL_04",
            totals.transform,
            "TOTAL OPERATIONS         000",
            650f,
            140f,
            500f,
            30f,
            12,
            WHITE
        );

        Label(
            "TOTAL_05",
            totals.transform,
            "OVERALL READINESS        100%",
            1270f,
            180f,
            450f,
            30f,
            12,
            GREEN,
            TextAnchor.MiddleRight
        );

        Label(
            "TOTAL_06",
            totals.transform,
            "COMMAND STATUS           OPERATIONAL",
            1270f,
            140f,
            450f,
            30f,
            12,
            GREEN,
            TextAnchor.MiddleRight
        );
    }

    private static void CreateAchievementsPage()
    {
        Transform page =
            CreatePage(
                "PAGE_ACHIEVEMENTS",
                "ACHIEVEMENTS & COMMENDATIONS",
                "AWARD DATABASE ONLINE"
            );

        GameObject medals =
            Panel(
                "PANEL_MEDALS",
                page,
                25f,
                65f,
                875f,
                660f
            );

        Label(
            "MEDALS_HEADER",
            medals.transform,
            "COMMENDATIONS",
            25f,
            600f,
            800f,
            30f,
            15,
            CYAN
        );

        string[] awards =
        {
            "OBSIDIAN SERVICE MEDAL",
            "AUTONOMOUS WARFARE COMMENDATION",
            "TACTICAL EXCELLENCE MEDAL",
            "STRATEGIC COMMAND CITATION",
            "LOGISTICS DISTINCTION",
            "RECONNAISSANCE SERVICE AWARD",
            "FLEET COMMAND CITATION",
            "THEATER OPERATIONS MEDAL",
            "SURVIVAL COMMENDATION",
            "COMMAND INTENT AWARD"
        };

        float ay = 545f;

        foreach (string award in awards)
        {
            Box(
                "AWARD_MARKER",
                medals.transform,
                25f,
                ay + 7f,
                12f,
                12f,
                CYAN_DARK
            );

            Label(
                "AWARD_" + ay,
                medals.transform,
                award,
                55f,
                ay,
                700f,
                27f,
                11,
                WHITE
            );

            Label(
                "AWARD_STATUS_" + ay,
                medals.transform,
                "LOCKED",
                735f,
                ay,
                100f,
                27f,
                10,
                MUTED,
                TextAnchor.MiddleRight
            );

            ay -= 52f;
        }

        GameObject milestones =
            Panel(
                "PANEL_MILESTONES",
                page,
                925f,
                65f,
                900f,
                660f
            );

        Label(
            "MILESTONE_HEADER",
            milestones.transform,
            "COMMAND MILESTONES",
            25f,
            600f,
            820f,
            30f,
            15,
            CYAN
        );

        string[] milestonesList =
        {
            "FIRST DEPLOYMENT",
            "FIRST VICTORY",
            "FIRST AUTONOMOUS COMMAND",
            "FIRST RECOVERY",
            "FIRST FLEET DEPLOYMENT",
            "FIRST THEATER OPERATION",
            "100 OPERATIONS",
            "1,000 UNITS DEPLOYED",
            "MASTER COMMANDER",
            "OBSIDIAN PROTOCOL"
        };

        float my = 545f;

        foreach (string milestone in milestonesList)
        {
            Label(
                "MILESTONE_" + my,
                milestones.transform,
                milestone,
                25f,
                my,
                650f,
                27f,
                11,
                WHITE
            );

            Label(
                "MILESTONE_STATE_" + my,
                milestones.transform,
                "0%",
                735f,
                my,
                100f,
                27f,
                10,
                MUTED,
                TextAnchor.MiddleRight
            );

            my -= 52f;
        }
    }

    private static void CreateCareerPage()
    {
        Transform page =
            CreatePage(
                "PAGE_CAREER",
                "COMMAND CAREER",
                "CAREER RECORD VERIFIED"
            );

        GameObject career =
            Panel(
                "PANEL_CAREER_TIMELINE",
                page,
                25f,
                65f,
                1170f,
                660f
            );

        Label(
            "CAREER_HEADER",
            career.transform,
            "OPERATIONAL CAREER",
            25f,
            600f,
            1080f,
            30f,
            15,
            CYAN
        );

        string[] careerItems =
        {
            "CURRENT ASSIGNMENT        COMMANDER",
            "CURRENT THEATER           UNASSIGNED",
            "CURRENT CAMPAIGN          UNASSIGNED",
            "CURRENT OPERATION         NONE",
            "SERVICE STATUS            ACTIVE",
            "DEPLOYMENT STATUS         READY",
            "FLEET STATUS              READY",
            "LOGISTICS STATUS          READY",
            "RESEARCH STATUS           READY",
            "AUTONOMY STATUS           READY",
            "CLEARANCE LEVEL           OMEGA"
        };

        float ky = 550f;

        foreach (string line in careerItems)
        {
            Label(
                "CAREER_" + ky,
                career.transform,
                line,
                25f,
                ky,
                1000f,
                30f,
                11,
                WHITE
            );

            ky -= 47f;
        }

        GameObject records =
            Panel(
                "PANEL_CAREER_RECORDS",
                page,
                1220f,
                65f,
                605f,
                660f
            );

        Label(
            "RECORDS_HEADER",
            records.transform,
            "RECORD SUMMARY",
            24f,
            600f,
            550f,
            30f,
            15,
            CYAN
        );

        Label(
            "RECORD_01",
            records.transform,
            "CAMPAIGNS        000",
            24f,
            540f,
            550f,
            30f,
            12,
            WHITE
        );

        Label(
            "RECORD_02",
            records.transform,
            "OPERATIONS       000",
            24f,
            495f,
            550f,
            30f,
            12,
            WHITE
        );

        Label(
            "RECORD_03",
            records.transform,
            "DEPLOYMENTS      000",
            24f,
            450f,
            550f,
            30f,
            12,
            WHITE
        );

        Label(
            "RECORD_04",
            records.transform,
            "VICTORIES        000",
            24f,
            405f,
            550f,
            30f,
            12,
            WHITE
        );

        Label(
            "RECORD_05",
            records.transform,
            "COMMENDATIONS    000",
            24f,
            360f,
            550f,
            30f,
            12,
            WHITE
        );

        Label(
            "RECORD_06",
            records.transform,
            "ACHIEVEMENTS     000",
            24f,
            315f,
            550f,
            30f,
            12,
            WHITE
        );

        Label(
            "RECORD_07",
            records.transform,
            "RANK             COMMANDER",
            24f,
            270f,
            550f,
            30f,
            12,
            CYAN
        );

        Label(
            "RECORD_08",
            records.transform,
            "EXPERIENCE       0 XP",
            24f,
            225f,
            550f,
            30f,
            12,
            WHITE
        );

        Label(
            "RECORD_09",
            records.transform,
            "READINESS        100%",
            24f,
            180f,
            550f,
            30f,
            12,
            GREEN
        );

        Label(
            "RECORD_10",
            records.transform,
            "PROFILE STATE    OPERATIONAL",
            24f,
            135f,
            550f,
            30f,
            12,
            GREEN
        );
    }

    private static void CreateDoctrineWindow()
    {
        GameObject window =
            Panel(
                "WINDOW_DOCTRINE_LIBRARY",
                canvasRoot,
                300f,
                145f,
                1320f,
                790f
            );

        Label(
            "WINDOW_TITLE",
            window.transform,
            "DOCTRINE LIBRARY",
            35f,
            710f,
            900f,
            40f,
            22,
            CYAN
        );

        Button(
            "BUTTON_DOCTRINE_CLOSE",
            window.transform,
            "CLOSE",
            1110f,
            700f,
            160f,
            42f,
            11
        );

        string[] doctrines =
        {
            "ADAPTIVE COMBINED ARMS",
            "AGGRESSIVE MANEUVER",
            "DEFENSIVE ENTRENCHMENT",
            "RECONNAISSANCE FIRST",
            "LOGISTICS DOMINANCE",
            "AUTONOMOUS SWARM",
            "OBJECTIVE CONTROL",
            "FORCE PRESERVATION"
        };

        float y = 620f;

        foreach (string doctrine in doctrines)
        {
            Panel(
                "DOCTRINE_CARD",
                window.transform,
                35f,
                y,
                1250f,
                65f
            );

            Label(
                "DOCTRINE_NAME",
                window.transform,
                doctrine,
                55f,
                y + 22f,
                800f,
                25f,
                12,
                WHITE
            );

            Label(
                "DOCTRINE_STATE",
                window.transform,
                doctrine == "ADAPTIVE COMBINED ARMS"
                    ? "ACTIVE"
                    : "AVAILABLE",
                1060f,
                y + 22f,
                190f,
                25f,
                10,
                doctrine == "ADAPTIVE COMBINED ARMS"
                    ? GREEN
                    : MUTED,
                TextAnchor.MiddleRight
            );

            y -= 78f;
        }
    }

    private static void CreateFooter()
    {
        GameObject footer =
            Panel(
                "FOOTER",
                canvasRoot,
                0f,
                0f,
                W,
                70f
            );

        Label(
            "FOOTER_LEFT",
            footer.transform,
            "OBSIDIAN PROTOCOL  //  COMMAND NETWORK",
            28f,
            18f,
            700f,
            30f,
            10,
            MUTED
        );

        Label(
            "FOOTER_CENTER",
            footer.transform,
            "PROFILE DATA SYNCHRONIZED",
            650f,
            18f,
            620f,
            30f,
            10,
            GREEN,
            TextAnchor.MiddleCenter
        );

        Button(
            "BUTTON_BACK",
            footer.transform,
            "BACK TO COMMAND",
            1620f,
            14f,
            250f,
            42f,
            10
        );
    }
}