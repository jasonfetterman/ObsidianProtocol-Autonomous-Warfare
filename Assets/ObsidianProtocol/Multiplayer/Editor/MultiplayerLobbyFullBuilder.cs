using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public static class MultiplayerLobbyFullBuilder
{
    private static readonly Color Background =
        new Color(0.006f, 0.010f, 0.016f, 1f);

    private static readonly Color PanelColorColor =
        new Color(0.025f, 0.045f, 0.060f, 0.97f);

    private static readonly Color PanelColorDark =
        new Color(0.012f, 0.022f, 0.032f, 0.98f);

    private static readonly Color Accent =
        new Color(0.08f, 0.72f, 0.84f, 1f);

    private static readonly Color Green =
        new Color(0.18f, 0.82f, 0.46f, 1f);

    private static readonly Color Yellow =
        new Color(0.90f, 0.68f, 0.18f, 1f);

    private static readonly Color Red =
        new Color(0.88f, 0.22f, 0.24f, 1f);

    private static readonly Color Text =
        new Color(0.82f, 0.91f, 0.94f, 1f);

    private static readonly Color Muted =
        new Color(0.42f, 0.54f, 0.59f, 1f);

    private static Font Font =>
        Resources.GetBuiltinResource<Font>(
            "LegacyRuntime.ttf");

    [MenuItem(
        "Obsidian Protocol/Multiplayer/BUILD FULL WALKABLE LOBBY")]
    public static void Build()
    {
        string path =
            FindScene();

        if (string.IsNullOrEmpty(path))
        {
            EditorUtility.DisplayDialog(
                "SCN-13",
                "MultiplayerHUD.unity was not found.",
                "OK");

            return;
        }

        Scene scene =
            EditorSceneManager.OpenScene(
                path,
                OpenSceneMode.Single);

        ClearScene();

        BuildWorld();
        BuildPlayer();
        BuildNPCs();
        BuildHUD();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[MultiplayerLobbyFullBuilder] " +
            "FULL WALKABLE MULTIPLAYER LOBBY BUILT.");

        EditorUtility.DisplayDialog(
            "SCN-13 MULTIPLAYER",
            "Full walkable multiplayer lobby built.\n\n" +
            "WASD = Move\n" +
            "Mouse = Look\n" +
            "E = Interact\n" +
            "ESC = Cursor\n\n" +
            "Lobby systems are active.",
            "OK");
    }

    private static string FindScene()
    {
        string[] guids =
            AssetDatabase.FindAssets(
                "MultiplayerHUD t:Scene");

        foreach (string guid in guids)
        {
            string path =
                AssetDatabase.GUIDToAssetPath(guid);

            if (path.EndsWith(
                "MultiplayerHUD.unity",
                System.StringComparison.OrdinalIgnoreCase))
                return path;
        }

        return null;
    }

    private static void ClearScene()
    {
        List<GameObject> roots =
            new List<GameObject>(
                SceneManager.GetActiveScene()
                    .GetRootGameObjects());

        foreach (GameObject go in roots)
        {
            Object.DestroyImmediate(go);
        }
    }

    private static void BuildWorld()
    {
        GameObject world =
            new GameObject("MULTIPLAYER_LOBBY_WORLD");

        CreateCube(
            "LobbyFloor",
            world.transform,
            new Vector3(0f, -0.25f, 0f),
            new Vector3(42f, 0.5f, 32f),
            new Color(0.025f, 0.035f, 0.042f));

        CreateCube(
            "NorthWall",
            world.transform,
            new Vector3(0f, 4f, 16f),
            new Vector3(42f, 8f, 0.5f),
            new Color(0.025f, 0.040f, 0.050f));

        CreateCube(
            "SouthWall",
            world.transform,
            new Vector3(0f, 4f, -16f),
            new Vector3(42f, 8f, 0.5f),
            new Color(0.025f, 0.040f, 0.050f));

        CreateCube(
            "WestWall",
            world.transform,
            new Vector3(-21f, 4f, 0f),
            new Vector3(0.5f, 8f, 32f),
            new Color(0.025f, 0.040f, 0.050f));

        CreateCube(
            "EastWall",
            world.transform,
            new Vector3(21f, 4f, 0f),
            new Vector3(0.5f, 8f, 32f),
            new Color(0.025f, 0.040f, 0.050f));

        CreateCube(
            "Ceiling",
            world.transform,
            new Vector3(0f, 8f, 0f),
            new Vector3(42f, 0.5f, 32f),
            new Color(0.012f, 0.018f, 0.025f));

        // CENTRAL COMMAND ATRIUM

        CreateCube(
            "CommandPlatform",
            world.transform,
            new Vector3(0f, 0.2f, 2f),
            new Vector3(12f, 0.4f, 7f),
            new Color(0.035f, 0.055f, 0.065f));

        CreateCube(
            "CommandCore",
            world.transform,
            new Vector3(0f, 1.2f, 2f),
            new Vector3(3f, 2f, 2f),
            new Color(0.035f, 0.11f, 0.13f));

        CreateLight(
            "CommandCoreLight",
            new Vector3(0f, 3f, 2f),
            Accent,
            10f,
            6f);

        // LEFT WING

        CreateRoom(
            world.transform,
            "MATCHMAKING_WING",
            new Vector3(-12f, 0f, 7f),
            new Vector3(12f, 6f, 8f));

        CreateTerminal(
            world.transform,
            "MATCHMAKING_TERMINAL",
            "MATCHMAKING TERMINAL",
            new Vector3(-12f, 1.1f, 8f),
            Accent,
            LobbyInteractable.InteractionType.Queue);

        CreateRoom(
            world.transform,
            "TEAM_WING",
            new Vector3(-12f, 0f, -7f),
            new Vector3(12f, 6f, 8f));

        CreateTerminal(
            world.transform,
            "TEAM_TERMINAL",
            "TEAM MANAGEMENT",
            new Vector3(-12f, 1.1f, -8f),
            Green,
            LobbyInteractable.InteractionType.Team);

        // RIGHT WING

        CreateRoom(
            world.transform,
            "COMMUNICATION_WING",
            new Vector3(12f, 0f, 7f),
            new Vector3(12f, 6f, 8f));

        CreateTerminal(
            world.transform,
            "COMMUNICATION_TERMINAL",
            "COMMUNICATIONS",
            new Vector3(12f, 1.1f, 8f),
            Accent,
            LobbyInteractable.InteractionType.Chat);

        CreateRoom(
            world.transform,
            "DEPLOYMENT_WING",
            new Vector3(12f, 0f, -7f),
            new Vector3(12f, 6f, 8f));

        CreateTerminal(
            world.transform,
            "DEPLOYMENT_TERMINAL",
            "DEPLOYMENT STAGING",
            new Vector3(12f, 1.1f, -8f),
            Yellow,
            LobbyInteractable.InteractionType.Deployment);

        // PRIVATE LOBBY / HOST

        CreateTerminal(
            world.transform,
            "PRIVATE_LOBBY_TERMINAL",
            "CREATE PRIVATE LOBBY",
            new Vector3(-5f, 1.1f, 11f),
            Accent,
            LobbyInteractable.InteractionType.CreateLobby);

        // READY

        CreateTerminal(
            world.transform,
            "READY_TERMINAL",
            "READY / NOT READY",
            new Vector3(5f, 1.1f, 11f),
            Green,
            LobbyInteractable.InteractionType.Ready);

        // PROFILE

        CreateTerminal(
            world.transform,
            "PROFILE_TERMINAL",
            "COMMANDER PROFILE",
            new Vector3(-5f, 1.1f, -11f),
            Accent,
            LobbyInteractable.InteractionType.Profile);

        // MATCH SETTINGS

        CreateTerminal(
            world.transform,
            "MATCH_SETTINGS_TERMINAL",
            "MATCH SETTINGS",
            new Vector3(5f, 1.1f, -11f),
            Yellow,
            LobbyInteractable.InteractionType.MatchSettings);

        // LIGHTING

        CreateLight(
            "LobbyLightNorth",
            new Vector3(0f, 7f, 12f),
            Accent,
            28f,
            12f);

        CreateLight(
            "LobbyLightSouth",
            new Vector3(0f, 7f, -12f),
            Accent,
            28f,
            12f);

        CreateLight(
            "LobbyLightWest",
            new Vector3(-16f, 6f, 0f),
            Green,
            24f,
            10f);

        CreateLight(
            "LobbyLightEast",
            new Vector3(16f, 6f, 0f),
            Yellow,
            24f,
            10f);
    }

    private static void BuildPlayer()
    {
        GameObject player =
            new GameObject(
                "LOCAL_COMMANDER",
                typeof(CharacterController),
                typeof(MultiplayerLobbyPlayerController));

        player.transform.position =
            new Vector3(0f, 1.1f, -4f);

        CharacterController controller =
            player.GetComponent<CharacterController>();

        controller.height = 1.8f;
        controller.radius = 0.35f;
        controller.center =
            new Vector3(0f, 0.9f, 0f);

        GameObject camera =
            new GameObject(
                "CommanderCamera",
                typeof(Camera),
                typeof(AudioListener));

        camera.transform.SetParent(
            player.transform,
            false);

        camera.transform.localPosition =
            new Vector3(0f, 1.6f, 0f);

        camera.transform.localRotation =
            Quaternion.identity;

        Camera cam =
            camera.GetComponent<Camera>();

        cam.fieldOfView = 75f;
        cam.nearClipPlane = 0.05f;
        cam.farClipPlane = 300f;

        camera.tag = "MainCamera";
    }

    private static void BuildNPCs()
    {
        CreateNPC(
            "COMMANDER_VANGUARD",
            "VANGUARD",
            new Vector3(-3f, 0f, 2f),
            Accent);

        CreateNPC(
            "COMMANDER_SENTINEL",
            "SENTINEL",
            new Vector3(3f, 0f, 2f),
            Green);

        CreateNPC(
            "COMMANDER_NIGHTWATCH",
            "NIGHTWATCH",
            new Vector3(-3f, 0f, 6f),
            Yellow);

        CreateNPC(
            "COMMANDER_HORIZON",
            "HORIZON",
            new Vector3(3f, 0f, 6f),
            Accent);
    }

    private static void CreateNPC(
        string objectName,
        string commanderName,
        Vector3 position,
        Color color)
    {
        GameObject root =
            new GameObject(objectName);

        root.transform.position =
            position;

        CreateCube(
            "Body",
            root.transform,
            new Vector3(0f, 1f, 0f),
            new Vector3(0.8f, 1.6f, 0.55f),
            new Color(0.06f, 0.08f, 0.09f));

        GameObject head =
            GameObject.CreatePrimitive(
                PrimitiveType.Sphere);

        head.name = "Head";
        head.transform.SetParent(
            root.transform,
            false);

        head.transform.localPosition =
            new Vector3(0f, 2f, 0f);

        head.transform.localScale =
            Vector3.one * 0.45f;

        Renderer renderer =
            head.GetComponent<Renderer>();

        renderer.sharedMaterial =
            MakeMaterial(color);

        GameObject label =
            new GameObject(
                "NAME_" + commanderName,
                typeof(TextMesh));

        label.transform.SetParent(
            root.transform,
            false);

        label.transform.localPosition =
            new Vector3(0f, 2.6f, 0f);

        label.transform.localRotation =
            Quaternion.Euler(
                0f,
                180f,
                0f);

        TextMesh text =
            label.GetComponent<TextMesh>();

        text.text =
            commanderName +
            "\nONLINE";

        text.font =
            Font;

        text.fontSize = 42;
        text.characterSize = 0.045f;
        text.anchor =
            TextAnchor.MiddleCenter;
        text.alignment =
            TextAlignment.Center;
        text.color = color;
    }

    private static void BuildHUD()
    {
        GameObject canvasGO =
            new GameObject(
                "MULTIPLAYER_CANVAS",
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster),
                typeof(MultiplayerLobbyRuntime));

        Canvas canvas =
            canvasGO.GetComponent<Canvas>();

        canvas.renderMode =
            RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler =
            canvasGO.GetComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(1920f, 1080f);

        GameObject hud =
            Panel(
                "MULTIPLAYER_HUD",
                canvasGO.transform,
                new Color(
                    0f,
                    0f,
                    0f,
                    0f),
                Vector2.zero,
                Vector2.one);

        Panel(
            "HUD_TOP",
            hud.transform,
            PanelColorDark,
            new Vector2(0f, 0.91f),
            new Vector2(1f, 1f));

        Label(
            hud.transform,
            "OBSIDIAN PROTOCOL // MULTIPLAYER COMMAND NETWORK",
            24,
            Text,
            TextAnchor.MiddleLeft,
            new Vector2(0.02f, 0.92f),
            new Vector2(0.70f, 0.99f));

        Label(
            hud.transform,
            "● NETWORK ONLINE",
            16,
            Green,
            TextAnchor.MiddleRight,
            new Vector2(0.78f, 0.92f),
            new Vector2(0.98f, 0.99f));

        Label(
            hud.transform,
            "LOBBY STATUS",
            11,
            Muted,
            TextAnchor.UpperLeft,
            new Vector2(0.02f, 0.80f),
            new Vector2(0.25f, 0.84f));

        Label(
            hud.transform,
            "LOBBY_STATE_TEXT",
            16,
            Accent,
            TextAnchor.UpperLeft,
            new Vector2(0.02f, 0.75f),
            new Vector2(0.35f, 0.80f));

        Label(
            hud.transform,
            "PLAYER_COUNT_TEXT",
            14,
            Text,
            TextAnchor.UpperLeft,
            new Vector2(0.02f, 0.70f),
            new Vector2(0.35f, 0.75f));

        Label(
            hud.transform,
            "QUEUE_STATUS_TEXT",
            14,
            Green,
            TextAnchor.UpperLeft,
            new Vector2(0.02f, 0.65f),
            new Vector2(0.35f, 0.70f));

        // BUDGET

        Panel(
            "BUDGET_PANEL",
            hud.transform,
            PanelColorDark,
            new Vector2(0.35f, 0.91f),
            new Vector2(0.65f, 0.98f));

        Label(
            hud.transform,
            "10,000 / 10,000 DEPLOYMENT POINTS",
            16,
            Text,
            TextAnchor.MiddleCenter,
            new Vector2(0.35f, 0.92f),
            new Vector2(0.65f, 0.97f));

        // INTERACTION

        GameObject interaction =
            Panel(
                "INTERACTION_PANEL",
                hud.transform,
                new Color(
                    0.01f,
                    0.025f,
                    0.035f,
                    0.94f),
                new Vector2(0.35f, 0.10f),
                new Vector2(0.65f, 0.17f));

        Label(
            interaction.transform,
            "INTERACTION_TEXT",
            17,
            WhiteColor(),
            TextAnchor.MiddleCenter,
            Vector2.zero,
            Vector2.one);

        // CHAT

        Panel(
            "CHAT_PANEL",
            hud.transform,
            PanelColorDark,
            new Vector2(0.02f, 0.05f),
            new Vector2(0.30f, 0.28f));

        Label(
            hud.transform,
            "CHAT_LOG_TEXT",
            12,
            Text,
            TextAnchor.LowerLeft,
            new Vector2(0.035f, 0.07f),
            new Vector2(0.285f, 0.25f));

        Label(
            hud.transform,
            "SECURE LOBBY CHANNEL",
            11,
            Accent,
            TextAnchor.UpperLeft,
            new Vector2(0.035f, 0.255f),
            new Vector2(0.285f, 0.28f));

        // CONTROLS

        Panel(
            "CONTROL_PANEL",
            hud.transform,
            PanelColorDark,
            new Vector2(0.70f, 0.05f),
            new Vector2(0.98f, 0.28f));

        Label(
            hud.transform,
            "COMMANDER CONTROLS",
            12,
            Accent,
            TextAnchor.UpperLeft,
            new Vector2(0.72f, 0.255f),
            new Vector2(0.96f, 0.28f));

        Label(
            hud.transform,
            "WASD   MOVE\nMOUSE  LOOK\nE      INTERACT\nESC    CURSOR",
            13,
            Text,
            TextAnchor.UpperLeft,
            new Vector2(0.72f, 0.09f),
            new Vector2(0.96f, 0.245f));

        // RETICLE

        Label(
            hud.transform,
            "+",
            24,
            Accent,
            TextAnchor.MiddleCenter,
            new Vector2(0.49f, 0.48f),
            new Vector2(0.51f, 0.52f));
    }

    private static GameObject CreateRoom(
        Transform parent,
        string name,
        Vector3 center,
        Vector3 size)
    {
        GameObject room =
            new GameObject(name);

        room.transform.SetParent(
            parent,
            false);

        Vector3 half =
            size / 2f;

        CreateCube(
            "Floor",
            room.transform,
            center,
            new Vector3(
                size.x,
                0.2f,
                size.z),
            new Color(
                0.035f,
                0.050f,
                0.060f));

        CreateCube(
            "Back",
            room.transform,
            center +
            new Vector3(
                0f,
                size.y / 2f,
                half.z),
            new Vector3(
                size.x,
                size.y,
                0.2f),
            new Color(
                0.025f,
                0.040f,
                0.050f));

        return room;
    }

    private static void CreateTerminal(
        Transform parent,
        string name,
        string title,
        Vector3 position,
        Color color,
        LobbyInteractable.InteractionType type)
    {
        GameObject terminal =
            new GameObject(
                name);

        terminal.transform.SetParent(
            parent,
            false);

        terminal.transform.position =
            position;

        GameObject baseObj =
            CreateCube(
                "Terminal",
                terminal.transform,
                Vector3.zero,
                new Vector3(
                    1.4f,
                    1.8f,
                    0.7f),
                new Color(
                    0.025f,
                    0.035f,
                    0.042f));

        GameObject screen =
            CreateCube(
                "Screen",
                terminal.transform,
                new Vector3(
                    0f,
                    0.35f,
                    -0.37f),
                new Vector3(
                    1.05f,
                    0.65f,
                    0.05f),
                color);

        LobbyInteractable interactable =
            terminal.AddComponent<LobbyInteractable>();

        interactable.displayName =
            title;

        interactable.interactionType =
            type;

        BoxCollider collider =
            terminal.GetComponent<BoxCollider>();

        if (collider == null)
            collider =
                terminal.AddComponent<BoxCollider>();

        collider.center =
            Vector3.zero;

        collider.size =
            new Vector3(
                1.8f,
                2.2f,
                1.4f);

        GameObject label =
            new GameObject(
                "TerminalLabel",
                typeof(TextMesh));

        label.transform.SetParent(
            terminal.transform,
            false);

        label.transform.localPosition =
            new Vector3(
                0f,
                1.45f,
                0f);

        TextMesh text =
            label.GetComponent<TextMesh>();

        text.text =
            title;

        text.font =
            Font;

        text.fontSize = 40;
        text.characterSize = 0.035f;
        text.anchor =
            TextAnchor.MiddleCenter;
        text.alignment =
            TextAlignment.Center;
        text.color = color;
    }

    private static GameObject CreateCube(
        string name,
        Transform parent,
        Vector3 localPosition,
        Vector3 scale,
        Color color)
    {
        GameObject cube =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube);

        cube.name = name;

        cube.transform.SetParent(
            parent,
            false);

        cube.transform.localPosition =
            localPosition;

        cube.transform.localScale =
            scale;

        Renderer renderer =
            cube.GetComponent<Renderer>();

        renderer.sharedMaterial =
            MakeMaterial(color);

        return cube;
    }

    private static void CreateLight(
        string name,
        Vector3 position,
        Color color,
        float range,
        float intensity)
    {
        GameObject go =
            new GameObject(name);

        go.transform.position =
            position;

        Light light =
            go.AddComponent<Light>();

        light.type =
            LightType.Point;

        light.color =
            color;

        light.range =
            range;

        light.intensity =
            intensity;
    }

    private static Material MakeMaterial(
        Color color)
    {
        Shader shader =
            Shader.Find(
                "Universal Render Pipeline/Lit");

        if (shader == null)
            shader =
                Shader.Find(
                    "Standard");

        Material material =
            new Material(shader);

        material.color =
            color;

        return material;
    }

    private static GameObject Panel(
        string name,
        Transform parent,
        Color color,
        Vector2 min,
        Vector2 max)
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

        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        go.GetComponent<Image>().color =
            color;

        return go;
    }

    private static GameObject Label(
        Transform parent,
        string value,
        int size,
        Color color,
        TextAnchor alignment,
        Vector2 min,
        Vector2 max)
    {
        GameObject go =
            new GameObject(
                value.Replace(
                    " ",
                    "_"),
                typeof(RectTransform),
                typeof(Text));

        go.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Text text =
            go.GetComponent<Text>();

        text.text = value;
        text.font = Font;
        text.fontSize = size;
        text.color = color;
        text.alignment = alignment;
        text.horizontalOverflow =
            HorizontalWrapMode.Overflow;
        text.verticalOverflow =
            VerticalWrapMode.Overflow;

        text.raycastTarget = false;

        return go;
    }

    private static Color WhiteColor()
    {
        return new Color(
            0.95f,
            0.98f,
            1f,
            1f);
    }
}

