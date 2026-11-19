using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public static class BlackspireContinentRebuilder
{
    private const string ScenePath =
        "Assets/Scenes/SCN30_ACHERON/Blackspire_Continent.unity";

    private const string DataFolder =
        "Assets/Scenes/SCN30_ACHERON/GeneratedBlackspire";

    private const string RootName =
        "BLACKSPIRE_REBUILD_ROOT";

    private static Terrain terrain;
    private static Transform root;

    [MenuItem("Obsidian Protocol/Blackspire/Rebuild Continent")]
    public static void Rebuild()
    {
        if (!EditorUtility.DisplayDialog(
            "Rebuild Blackspire Continent",
            "This will back up the scene and replace only the generated " +
            "BLACKSPIRE_REBUILD_ROOT. Other scene objects will be preserved.",
            "REBUILD",
            "CANCEL"))
            return;

        string fullScenePath = Path.GetFullPath(ScenePath);

        if (!File.Exists(fullScenePath))
        {
            Debug.LogError("Blackspire scene not found: " + ScenePath);
            return;
        }

        string backup = fullScenePath + ".before_rebuild_" +
                        DateTime.Now.ToString("yyyyMMdd_HHmmss");

        File.Copy(fullScenePath, backup, false);

        EditorSceneManager.OpenScene(
            ScenePath, OpenSceneMode.Single);

        GameObject oldRoot = GameObject.Find(RootName);

        if (oldRoot != null)
            UnityEngine.Object.DestroyImmediate(oldRoot);

        EnsureFolder(DataFolder);

        GameObject rootObject = new GameObject(RootName);
        root = rootObject.transform;

        root.position = Vector3.zero;

        BuildTerrain();
        BuildMountainFormations();
        BuildIndustrialFacility();
        BuildRoadNetwork();
        BuildLighting();
        PlacePlayer();

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("BLACKSPIRE REBUILD COMPLETE");
        Debug.Log("Backup: " + backup);
        Debug.Log("Terrain: 1000 x 1000");
        Debug.Log("Height range: 600");
        Debug.Log("Terrain Collider: generated with Terrain");
    }

    private static void EnsureFolder(string path)
    {
        string[] parts = path.Split('/');
        string current = parts[0];

        for (int i = 1; i < parts.Length; i++)
        {
            string next = current + "/" + parts[i];

            if (!AssetDatabase.IsValidFolder(next))
                AssetDatabase.CreateFolder(current, parts[i]);

            current = next;
        }
    }

    private static void BuildTerrain()
    {
        TerrainData data = new TerrainData();
        data.heightmapResolution = 1025;
        data.size = new Vector3(1000f, 600f, 1000f);
        data.SetDetailResolution(512, 16);

        int resolution = data.heightmapResolution;
        float[,] heights = new float[resolution, resolution];

        for (int z = 0; z < resolution; z++)
        {
            for (int x = 0; x < resolution; x++)
            {
                float wx = (float)x / (resolution - 1) * 1000f - 500f;
                float wz = (float)z / (resolution - 1) * 1000f - 500f;

                float hills = Mathf.PerlinNoise(
                    (wx + 1200f) * 0.0025f,
                    (wz + 1200f) * 0.0025f);

                float detail = Mathf.PerlinNoise(
                    (wx + 500f) * 0.012f,
                    (wz + 500f) * 0.012f);

                float ridgeNoise = Mathf.PerlinNoise(
                    (wx + 300f) * 0.004f,
                    (wz + 800f) * 0.004f);

                float ridge = 1f - Mathf.Abs(ridgeNoise * 2f - 1f);

                float mountainCenter =
                    Mathf.Exp(-Mathf.Pow((wx + 100f) / 210f, 2f)) *
                    Mathf.Exp(-Mathf.Pow((wz - 60f) / 350f, 2f));

                float mountainHeight =
                    mountainCenter * (0.35f + ridge * 0.65f);

                float baseHeight =
                    0.035f +
                    hills * 0.055f +
                    detail * 0.018f +
                    mountainHeight * 0.50f;

                float edge = Mathf.Clamp01(
                    Mathf.Min(
                        (500f - Mathf.Abs(wx)) / 90f,
                        (500f - Mathf.Abs(wz)) / 90f));

                baseHeight *= edge;

                heights[z, x] = Mathf.Clamp01(baseHeight);
            }
        }

        data.SetHeights(0, 0, heights);

        string assetPath = DataFolder + "/Blackspire_Generated_Terrain.asset";

        if (AssetDatabase.LoadAssetAtPath<TerrainData>(assetPath) != null)
            assetPath = DataFolder + "/Blackspire_Generated_Terrain_" +
                        DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".asset";

        AssetDatabase.CreateAsset(data, assetPath);

        GameObject terrainObject = Terrain.CreateTerrainGameObject(data);
        terrainObject.name = "BLACKSPIRE_MAIN_TERRAIN";
        terrainObject.transform.SetParent(root);
        terrainObject.transform.position = new Vector3(-500f, 0f, -500f);

        terrain = terrainObject.GetComponent<Terrain>();

        TerrainCollider terrainCollider =
            terrainObject.GetComponent<TerrainCollider>();

        terrainCollider.terrainData = data;
        terrainCollider.enabled = true;

        terrain.heightmapPixelError = 5;
        terrain.basemapDistance = 2000f;
        terrain.drawInstanced = true;

        terrainObject.layer = LayerMask.NameToLayer("Terrain") >= 0
            ? LayerMask.NameToLayer("Terrain") : 0;

        Debug.Log("Blackspire terrain and collision created.");
    }

    private static float Ground(float worldX, float worldZ)
    {
        return terrain.SampleHeight(
            new Vector3(worldX, 0f, worldZ));
    }

    private static GameObject MakePrimitive(
        PrimitiveType type,
        string objectName,
        Vector3 position,
        Vector3 scale,
        Material material)
    {
        GameObject obj = GameObject.CreatePrimitive(type);
        obj.name = objectName;
        obj.transform.SetParent(root);
        obj.transform.position = position;
        obj.transform.localScale = scale;

        Renderer renderer = obj.GetComponent<Renderer>();

        if (renderer != null && material != null)
            renderer.sharedMaterial = material;

        return obj;
    }

    private static Material MakeMaterial(
        string name, Color color)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");

        if (shader == null)
            shader = Shader.Find("Standard");

        if (shader == null)
            return null;

        Material material = new Material(shader);
        material.name = name;
        material.color = color;

        return material;
    }

    private static void BuildMountainFormations()
    {
        Material rock = MakeMaterial(
            "MAT_BLACKSPIRE_DARK_ROCK",
            new Color(0.16f, 0.18f, 0.20f));

        Material metal = MakeMaterial(
            "MAT_BLACKSPIRE_METAL",
            new Color(0.10f, 0.13f, 0.16f));

        System.Random random = new System.Random(2187);

        for (int i = 0; i < 38; i++)
        {
            float x = (float)random.NextDouble() * 780f - 390f;
            float z = (float)random.NextDouble() * 780f - 390f;
            float y = Ground(x, z);

            float height = 12f + (float)random.NextDouble() * 55f;
            float width = 12f + (float)random.NextDouble() * 35f;

            GameObject peak = MakePrimitive(
                PrimitiveType.Cube,
                "BLACKSPIRE_ROCK_FORMATION_" + i.ToString("00"),
                new Vector3(x, y + height * 0.35f, z),
                new Vector3(width, height, width),
                rock);

            peak.transform.rotation = Quaternion.Euler(
                random.Next(-12, 12),
                random.Next(0, 180),
                random.Next(-12, 12));
        }

        for (int i = 0; i < 18; i++)
        {
            float x = -360f + i * 40f;
            float z = 160f + (float)random.NextDouble() * 80f;
            float y = Ground(x, z);

            GameObject ridge = MakePrimitive(
                PrimitiveType.Cube,
                "BLACKSPIRE_RIDGE_" + i.ToString("00"),
                new Vector3(x, y + 22f, z),
                new Vector3(55f, 44f, 32f),
                metal);

            ridge.transform.rotation = Quaternion.Euler(
                0f, random.Next(0, 180), 0f);
        }
    }

    private static void BuildIndustrialFacility()
    {
        Material metal = MakeMaterial(
            "MAT_BLACKSPIRE_FACILITY",
            new Color(0.09f, 0.12f, 0.15f));

        Material accent = MakeMaterial(
            "MAT_BLACKSPIRE_CYAN",
            new Color(0.02f, 0.55f, 0.65f));

        Vector2 center = new Vector2(100f, -100f);
        float ground = Ground(center.x, center.y);

        GameObject commandCenter = MakePrimitive(
            PrimitiveType.Cube,
            "BLACKSPIRE_INDUSTRIAL_COMMAND_CENTER",
            new Vector3(center.x, ground + 10f, center.y),
            new Vector3(55f, 20f, 45f),
            metal);

        MakePrimitive(
            PrimitiveType.Cube,
            "COMMAND_CENTER_ROOF",
            new Vector3(center.x, ground + 21f, center.y),
            new Vector3(58f, 2f, 48f),
            accent);

        for (int i = 0; i < 4; i++)
        {
            float x = center.x - 35f + i * 23f;

            MakePrimitive(
                PrimitiveType.Cylinder,
                "INDUSTRIAL_VENT_" + i,
                new Vector3(x, ground + 29f, center.y + 12f),
                new Vector3(3f, 8f, 3f),
                metal);
        }

        for (int i = 0; i < 6; i++)
        {
            float x = center.x - 90f + (i % 3) * 85f;
            float z = center.y + 85f + (i / 3) * 45f;
            float y = Ground(x, z);

            MakePrimitive(
                PrimitiveType.Cube,
                "INDUSTRIAL_HANGAR_" + i,
                new Vector3(x, y + 8f, z),
                new Vector3(45f, 16f, 32f),
                metal);
        }

        for (int i = 0; i < 5; i++)
        {
            float x = center.x + 100f + i * 24f;
            float z = center.y - 40f;
            float y = Ground(x, z);

            MakePrimitive(
                PrimitiveType.Cylinder,
                "INDUSTRIAL_SMOKE_STACK_" + i,
                new Vector3(x, y + 20f, z),
                new Vector3(7f, 20f, 7f),
                metal);
        }
    }

    private static void BuildRoadNetwork()
    {
        Material road = MakeMaterial(
            "MAT_BLACKSPIRE_ROAD",
            new Color(0.07f, 0.075f, 0.08f));

        for (int i = 0; i < 8; i++)
        {
            float z = -220f + i * 18f;
            float y = Ground(0f, z);

            MakePrimitive(
                PrimitiveType.Cube,
                "BLACKSPIRE_ACCESS_ROAD_" + i,
                new Vector3(0f, y + 0.25f, z),
                new Vector3(520f, 0.5f, 12f),
                road);
        }

        for (int i = 0; i < 5; i++)
        {
            float x = 50f + i * 18f;
            float y = Ground(x, -100f);

            MakePrimitive(
                PrimitiveType.Cube,
                "FACILITY_ROAD_" + i,
                new Vector3(x, y + 0.3f, -100f),
                new Vector3(12f, 0.6f, 220f),
                road);
        }
    }

    private static void BuildLighting()
    {
        GameObject sun = new GameObject("BLACKSPIRE_SUN");
        sun.transform.SetParent(root);
        sun.transform.rotation = Quaternion.Euler(45f, -35f, 0f);

        Light directional = sun.AddComponent<Light>();
        directional.type = LightType.Directional;
        directional.intensity = 1.2f;
        directional.color = new Color(0.72f, 0.80f, 0.88f);

        GameObject facilityLight = new GameObject(
            "BLACKSPIRE_FACILITY_LIGHT");

        facilityLight.transform.SetParent(root);
        facilityLight.transform.position = new Vector3(100f, 55f, -100f);

        Light point = facilityLight.AddComponent<Light>();
        point.type = LightType.Point;
        point.range = 100f;
        point.intensity = 5f;
        point.color = new Color(0.05f, 0.65f, 0.75f);
    }

    private static void PlacePlayer()
    {
        GameObject player = GameObject.Find("BLACKSPIRE_WALK_PLAYER");

        if (player == null)
        {
            player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "BLACKSPIRE_WALK_PLAYER";
        }

        float spawnX = 100f;
        float spawnZ = -260f;
        float ground = Ground(spawnX, spawnZ);

        player.transform.position =
            new Vector3(spawnX, ground + 2f, spawnZ);

        player.transform.rotation = Quaternion.identity;

        CapsuleCollider capsule = player.GetComponent<CapsuleCollider>();

        if (capsule != null)
            UnityEngine.Object.DestroyImmediate(capsule);

        CharacterController controller =
            player.GetComponent<CharacterController>();

        if (controller == null)
            controller = player.AddComponent<CharacterController>();

        controller.center = new Vector3(0f, 0.9f, 0f);
        controller.height = 1.8f;
        controller.radius = 0.35f;
        controller.stepOffset = 0.3f;
        controller.skinWidth = 0.08f;

        Type movementType = Type.GetType(
            "BlackspireWalkController, Assembly-CSharp");

        if (movementType != null &&
            player.GetComponent(movementType) == null)
        {
            player.AddComponent(movementType);
        }

        player.tag = "Player";

        Debug.Log(
            "Player placed at " + player.transform.position);
    }
}
