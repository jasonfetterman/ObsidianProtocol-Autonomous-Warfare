using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class MultiplayerLobbyExpansionBuilder
{
    private const string SceneName = "MultiplayerHUD";

    private static readonly Color FloorColor = new Color(0.055f, 0.065f, 0.075f);
    private static readonly Color WallColor = new Color(0.09f, 0.10f, 0.115f);
    private static readonly Color TrimColor = new Color(0.16f, 0.18f, 0.20f);
    private static readonly Color AccentColor = new Color(0.15f, 0.55f, 0.85f);

    [MenuItem("Obsidian Protocol/Multiplayer/EXPAND LARGE-SCALE LOBBY")]
    public static void Build()
    {
        Scene scene = FindLobbyScene();

        if (scene == null)
        {
            EditorUtility.DisplayDialog(
                "Multiplayer Lobby",
                "Could not find MultiplayerHUD.unity.",
                "OK");
            return;
        }

        EditorSceneManager.OpenScene(scene.path, OpenSceneMode.Single);

        GameObject root = GameObject.Find("MULTIPLAYER_WORLD_EXPANSION");

        if (root != null)
            Object.DestroyImmediate(root);

        root = new GameObject("MULTIPLAYER_WORLD_EXPANSION");

        BuildMainAtrium(root.transform);
        BuildUpperGallery(root.transform);
        BuildMatchmakingDistrict(root.transform);
        BuildCommandDistrict(root.transform);
        BuildSocialDistrict(root.transform);
        BuildTeamDistrict(root.transform);
        BuildDeploymentDistrict(root.transform);
        BuildTransitCorridors(root.transform);
        BuildLighting(root.transform);

        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());

        EditorUtility.DisplayDialog(
            "MULTIPLAYER LOBBY EXPANDED",
            "Large-scale physical lobby expansion complete.\n\n" +
            "Main Atrium\n" +
            "Upper Commander Gallery\n" +
            "Matchmaking District\n" +
            "Command District\n" +
            "Social District\n" +
            "Team District\n" +
            "Deployment District\n" +
            "Expanded transit corridors",
            "OK");
    }

    private static Scene FindLobbyScene()
    {
        string[] guids = AssetDatabase.FindAssets(
            "MultiplayerHUD t:Scene",
            new[] { "Assets/Scenes" });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            if (path.EndsWith("MultiplayerHUD.unity"))
                return EditorSceneManager.GetSceneManagerSetup().Length >= 0
                    ? SceneManager.GetSceneByPath(path)
                    : default;
        }

        string[] all = AssetDatabase.FindAssets("t:Scene");

        foreach (string guid in all)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            if (path.EndsWith("MultiplayerHUD.unity"))
            {
                SceneAsset asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);

                if (asset != null)
                    return SceneManager.GetSceneByPath(path);
            }
        }

        return default;
    }

    private static void BuildMainAtrium(Transform parent)
    {
        CreateRoom(
            parent,
            "MAIN_COMMAND_ATRIUM",
            new Vector3(0f, 0f, 0f),
            new Vector3(70f, 8f, 70f));

        CreateFloorMark(parent, "ATRIUM_CENTER", new Vector3(0f, 0.08f, 0f), new Vector3(24f, 0.12f, 24f));
        CreatePlatform(parent, "COMMANDER_GATHERING_PLATFORM", new Vector3(0f, 0.35f, 0f), new Vector3(18f, 0.7f, 18f));

        CreateWall(parent, "NORTH_COMMANDER_GATE", new Vector3(0f, 4f, 32f), new Vector3(40f, 8f, 2f));
        CreateWall(parent, "SOUTH_COMMANDER_GATE", new Vector3(0f, 4f, -32f), new Vector3(40f, 8f, 2f));
        CreateWall(parent, "EAST_COMMANDER_GATE", new Vector3(32f, 4f, 0f), new Vector3(2f, 8f, 40f));
        CreateWall(parent, "WEST_COMMANDER_GATE", new Vector3(-32f, 4f, 0f), new Vector3(2f, 8f, 40f));
    }

    private static void BuildUpperGallery(Transform parent)
    {
        CreateRoom(
            parent,
            "UPPER_COMMANDER_GALLERY",
            new Vector3(0f, 10f, 0f),
            new Vector3(64f, 2f, 64f));

        CreatePlatform(
            parent,
            "UPPER_NORTH_GALLERY",
            new Vector3(0f, 10f, 24f),
            new Vector3(50f, 2f, 8f));

        CreatePlatform(
            parent,
            "UPPER_SOUTH_GALLERY",
            new Vector3(0f, 10f, -24f),
            new Vector3(50f, 2f, 8f));

        CreatePlatform(
            parent,
            "UPPER_EAST_GALLERY",
            new Vector3(24f, 10f, 0f),
            new Vector3(8f, 2f, 50f));

        CreatePlatform(
            parent,
            "UPPER_WEST_GALLERY",
            new Vector3(-24f, 10f, 0f),
            new Vector3(8f, 2f, 50f));
    }

    private static void BuildMatchmakingDistrict(Transform parent)
    {
        CreateRoom(
            parent,
            "MATCHMAKING_DISTRICT",
            new Vector3(105f, 0f, 0f),
            new Vector3(55f, 8f, 70f));

        CreatePlatform(
            parent,
            "RANKED_OPERATIONS_HALL",
            new Vector3(105f, 0.4f, 20f),
            new Vector3(42f, 0.8f, 18f));

        CreatePlatform(
            parent,
            "UNRANKED_OPERATIONS_HALL",
            new Vector3(105f, 0.4f, 0f),
            new Vector3(42f, 0.8f, 18f));

        CreatePlatform(
            parent,
            "COOP_OPERATIONS_HALL",
            new Vector3(105f, 0.4f, -20f),
            new Vector3(42f, 0.8f, 18f));
    }

    private static void BuildCommandDistrict(Transform parent)
    {
        CreateRoom(
            parent,
            "COMMAND_DISTRICT",
            new Vector3(-105f, 0f, 0f),
            new Vector3(55f, 8f, 70f));

        string[] sections =
        {
            "FLEET_COMMAND",
            "GARAGE_COMMAND",
            "RESEARCH_COMMAND",
            "INTELLIGENCE_COMMAND",
            "FINANCE_COMMAND",
            "OPERATIONS_COMMAND"
        };

        for (int i = 0; i < sections.Length; i++)
        {
            float z = 25f - (i * 10f);

            CreatePlatform(
                parent,
                sections[i],
                new Vector3(-105f, 0.4f, z),
                new Vector3(42f, 0.8f, 7f));
        }
    }

    private static void BuildSocialDistrict(Transform parent)
    {
        CreateRoom(
            parent,
            "SOCIAL_DISTRICT",
            new Vector3(0f, 0f, 105f),
            new Vector3(70f, 55f, 55f));

        CreatePlatform(
            parent,
            "COMMANDER_PROFILES",
            new Vector3(-20f, 0.4f, 105f),
            new Vector3(20f, 0.8f, 32f));

        CreatePlatform(
            parent,
            "RANKINGS_HALL",
            new Vector3(0f, 0.4f, 105f),
            new Vector3(20f, 0.8f, 32f));

        CreatePlatform(
            parent,
            "COMMUNITY_HALL",
            new Vector3(20f, 0.4f, 105f),
            new Vector3(20f, 0.8f, 32f));
    }

    private static void BuildTeamDistrict(Transform parent)
    {
        CreateRoom(
            parent,
            "TEAM_DISTRICT",
            new Vector3(0f, 0f, -105f),
            new Vector3(70f, 55f, 55f));

        CreatePlatform(
            parent,
            "TEAM_FORMATION",
            new Vector3(-20f, 0.4f, -105f),
            new Vector3(20f, 0.8f, 32f));

        CreatePlatform(
            parent,
            "PARTY_MANAGEMENT",
            new Vector3(0f, 0.4f, -105f),
            new Vector3(20f, 0.8f, 32f));

        CreatePlatform(
            parent,
            "VOICE_COMMUNICATION",
            new Vector3(20f, 0.4f, -105f),
            new Vector3(20f, 0.8f, 32f));
    }

    private static void BuildDeploymentDistrict(Transform parent)
    {
        CreateRoom(
            parent,
            "DEPLOYMENT_STAGING_DISTRICT",
            new Vector3(180f, 0f, 0f),
            new Vector3(70f, 8f, 90f));

        CreatePlatform(
            parent,
            "MATCH_CONFIGURATION",
            new Vector3(180f, 0.4f, 28f),
            new Vector3(50f, 0.8f, 18f));

        CreatePlatform(
            parent,
            "FORCE_SELECTION",
            new Vector3(180f, 0.4f, 6f),
            new Vector3(50f, 0.8f, 18f));

        CreatePlatform(
            parent,
            "COMMAND_CONFIGURATION",
            new Vector3(180f, 0.4f, -16f),
            new Vector3(50f, 0.8f, 18f));

        CreatePlatform(
            parent,
            "DEPLOYMENT_GATE",
            new Vector3(180f, 0.4f, -35f),
            new Vector3(50f, 0.8f, 12f));
    }

    private static void BuildTransitCorridors(Transform parent)
    {
        CreateCorridor(parent, "WEST_TRANSIT", new Vector3(-67f, 0f, 0f), new Vector3(40f, 8f, 14f));
        CreateCorridor(parent, "EAST_TRANSIT", new Vector3(67f, 0f, 0f), new Vector3(40f, 8f, 14f));
        CreateCorridor(parent, "NORTH_TRANSIT", new Vector3(0f, 0f, 67f), new Vector3(14f, 8f, 40f));
        CreateCorridor(parent, "SOUTH_TRANSIT", new Vector3(0f, 0f, -67f), new Vector3(14f, 8f, 40f));
        CreateCorridor(parent, "DEPLOYMENT_TRANSIT", new Vector3(140f, 0f, 0f), new Vector3(45f, 8f, 14f));
    }

    private static void BuildLighting(Transform parent)
    {
        CreateLight(parent, "ATRIUM_LIGHT", new Vector3(0f, 14f, 0f), 120f, 3f);
        CreateLight(parent, "MATCHMAKING_LIGHT", new Vector3(105f, 12f, 0f), 90f, 2f);
        CreateLight(parent, "COMMAND_LIGHT", new Vector3(-105f, 12f, 0f), 90f, 2f);
        CreateLight(parent, "SOCIAL_LIGHT", new Vector3(0f, 12f, 105f), 90f, 2f);
        CreateLight(parent, "TEAM_LIGHT", new Vector3(0f, 12f, -105f), 90f, 2f);
        CreateLight(parent, "DEPLOYMENT_LIGHT", new Vector3(180f, 12f, 0f), 100f, 2f);
    }

    private static void CreateRoom(Transform parent, string name, Vector3 center, Vector3 size)
    {
        GameObject floor = Cube(name + "_FLOOR", center + Vector3.down * 0.5f, new Vector3(size.x, 1f, size.z), FloorColor, parent);

        float wallHeight = size.y;
        float halfX = size.x * 0.5f;
        float halfZ = size.z * 0.5f;

        Cube(name + "_NORTH_WALL", center + new Vector3(0f, wallHeight * 0.5f, halfZ), new Vector3(size.x, wallHeight, 1f), WallColor, parent);
        Cube(name + "_SOUTH_WALL", center + new Vector3(0f, wallHeight * 0.5f, -halfZ), new Vector3(size.x, wallHeight, 1f), WallColor, parent);
        Cube(name + "_EAST_WALL", center + new Vector3(halfX, wallHeight * 0.5f, 0f), new Vector3(1f, wallHeight, size.z), WallColor, parent);
        Cube(name + "_WEST_WALL", center + new Vector3(-halfX, wallHeight * 0.5f, 0f), new Vector3(1f, wallHeight, size.z), WallColor, parent);

        CreateTrim(parent, name + "_TRIM", center, size);
    }

    private static void CreateCorridor(Transform parent, string name, Vector3 center, Vector3 size)
    {
        Cube(name + "_FLOOR", center + Vector3.down * 0.5f, new Vector3(size.x, 1f, size.z), FloorColor, parent);

        float h = size.y;
        float x = size.x * 0.5f;
        float z = size.z * 0.5f;

        Cube(name + "_WALL_A", center + new Vector3(0f, h * 0.5f, z), new Vector3(size.x, h, 1f), WallColor, parent);
        Cube(name + "_WALL_B", center + new Vector3(0f, h * 0.5f, -z), new Vector3(size.x, h, 1f), WallColor, parent);
    }

    private static void CreateTrim(Transform parent, string name, Vector3 center, Vector3 size)
    {
        Cube(
            name + "_NORTH",
            center + new Vector3(0f, 7.6f, size.z * 0.5f - 0.7f),
            new Vector3(size.x, 0.35f, 0.35f),
            TrimColor,
            parent);

        Cube(
            name + "_SOUTH",
            center + new Vector3(0f, 7.6f, -size.z * 0.5f + 0.7f),
            new Vector3(size.x, 0.35f, 0.35f),
            TrimColor,
            parent);
    }

    private static void CreatePlatform(Transform parent, string name, Vector3 position, Vector3 size)
    {
        Cube(name, position, size, TrimColor, parent);

        Cube(
            name + "_ACCENT",
            position + Vector3.up * (size.y * 0.5f + 0.08f),
            new Vector3(size.x * 0.82f, 0.08f, size.z * 0.08f),
            AccentColor,
            parent);
    }

    private static void CreateFloorMark(Transform parent, string name, Vector3 position, Vector3 size)
    {
        Cube(name, position, size, AccentColor, parent);
    }

    private static void CreateWall(Transform parent, string name, Vector3 position, Vector3 size)
    {
        Cube(name, position, size, WallColor, parent);
    }

    private static void CreateLight(Transform parent, string name, Vector3 position, float range, float intensity)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent);
        go.transform.position = position;

        Light light = go.AddComponent<Light>();
        light.type = LightType.Point;
        light.range = range;
        light.intensity = intensity;
    }

    private static GameObject Cube(
        string name,
        Vector3 position,
        Vector3 scale,
        Color color,
        Transform parent)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);

        go.name = name;
        go.transform.SetParent(parent);
        go.transform.position = position;
        go.transform.localScale = scale;

        Renderer renderer = go.GetComponent<Renderer>();

        if (renderer != null)
        {
            Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));

            if (material.shader == null)
                material = new Material(Shader.Find("Standard"));

            material.color = color;
            renderer.sharedMaterial = material;
        }

        return go;
    }
}
