using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using ObsidianProtocol.Game.World.Terrain;

public static class BlackspireTerrainBlockoutBuilder
{
    private const string Folder = "Assets/Scenes/SCN30_ACHERON";
    private const string ScenePath = Folder + "/Blackspire_Continent.unity";
    private const string DataPath = Folder + "/BlackspireTerrainData.asset";
    private const string DefinitionPath = Folder + "/BlackspireTerrainDefinition.asset";

    [MenuItem("Obsidian Protocol/World Building/Build Blackspire Terrain Blockout")]
    public static void Build()
    {
        if (File.Exists(ScenePath) || File.Exists(DataPath) || File.Exists(DefinitionPath))
        {
            Debug.LogError("[BLACKSPIRE] Target scene or asset already exists. Nothing was overwritten.");
            return;
        }

        EnsureFolder(Folder);

        const int resolution = 513;
        const float width = 1000f;
        const float height = 350f;

        TerrainData data = new TerrainData
        {
            heightmapResolution = resolution,
            size = new Vector3(width, height, width)
        };

        float[,] heights = new float[resolution, resolution];

        for (int z = 0; z < resolution; z++)
        {
            float nz = z / (float)(resolution - 1);

            for (int x = 0; x < resolution; x++)
            {
                float nx = x / (float)(resolution - 1);

                float elevation = 4f;

                // Peak A: (600,100), approximately +300 m.
                elevation += 300f * Gaussian(nx, .60f, .075f, nz, .10f, .075f);

                // Peak B: (100,600), approximately +250 m.
                elevation += 250f * Gaussian(nx, .10f, .085f, nz, .60f, .085f);

                // Eastern mountain spur supporting the Red HQ.
                elevation += 150f * Gaussian(nx, .93f, .12f, nz, .92f, .12f);

                // Broken northern ridge, leaving a lower central pass.
                float ridge = 85f * Gaussian(nz, .82f, .09f, nx, .50f, .48f);
                float passGap = Gaussian(nx, .50f, .105f, nz, .79f, .22f);
                elevation += ridge * (1f - .94f * passGap);

                // Rolling industrial uplands and the southern tunnel ridge.
                elevation += 32f * Gaussian(nx, .36f, .17f, nz, .32f, .20f);
                elevation += 48f * Gaussian(nx, .35f, .12f, nz, .22f, .25f);

                // Carve a shallow riverbed along the designed river route.
                float riverDistance = DistanceToPolyline(
                    nx * width, nz * width,
                    new Vector2(0f, 500f),
                    new Vector2(150f, 600f),
                    new Vector2(400f, 900f));

                float riverCut = 9f * Mathf.Exp(
                    -(riverDistance * riverDistance) / (2f * 16f * 16f));

                elevation = Mathf.Max(1f, elevation - riverCut);
                heights[z, x] = Mathf.Clamp01(elevation / height);
            }
        }

        data.SetHeights(0, 0, heights);
        AssetDatabase.CreateAsset(data, DataPath);

        TerrainDefinition definition = ScriptableObject.CreateInstance<TerrainDefinition>();
        SerializedObject serializedDefinition = new SerializedObject(definition);
        serializedDefinition.FindProperty("terrainId").stringValue = "BLACKSPIRE_EXPANSE";
        serializedDefinition.FindProperty("displayName").stringValue = "Blackspire Expanse - Black Mesa";
        serializedDefinition.ApplyModifiedPropertiesWithoutUndo();
        AssetDatabase.CreateAsset(definition, DefinitionPath);

        Scene scene = EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene, NewSceneMode.Single);

        GameObject root = new GameObject("BLACKSPIRE CONTINENT");
        GameObject terrainObject = Terrain.CreateTerrainGameObject(data);
        terrainObject.name = "BLACKSPIRE SCULPTED TERRAIN";
        terrainObject.transform.SetParent(root.transform, true);

        TerrainController controller = terrainObject.AddComponent<TerrainController>();
        SerializedObject serializedController = new SerializedObject(controller);
        serializedController.FindProperty("definition").objectReferenceValue = definition;
        serializedController.FindProperty("terrain").objectReferenceValue =
            terrainObject.GetComponent<UnityEngine.Terrain>();
        serializedController.ApplyModifiedPropertiesWithoutUndo();

        AddLandmark(root.transform, data, "BLUE HQ - WEST BASE", 50f, 50f, new Color(.15f, .45f, 1f));
        AddLandmark(root.transform, data, "RED HQ - EAST BASE", 950f, 950f, new Color(1f, .18f, .15f));
        AddLandmark(root.transform, data, "INDUSTRIAL COMPLEX", 500f, 300f, new Color(1f, .65f, .12f));
        AddLandmark(root.transform, data, "POWER PLANT / REFINERY", 800f, 200f, new Color(1f, .65f, .12f));
        AddLandmark(root.transform, data, "SUPPLY DEPOT", 200f, 800f, new Color(.2f, .85f, .35f));
        AddLandmark(root.transform, data, "NORTH PASS", 500f, 800f, Color.gray);
        AddLandmark(root.transform, data, "SOUTH TUNNEL RIDGE", 350f, 200f, Color.gray);
        AddLandmark(root.transform, data, "FORWARD AIRFIELD", 500f, 950f, Color.cyan);
        AddLandmark(root.transform, data, "PEAK A - SENSOR HIGH GROUND", 600f, 100f, Color.yellow);
        AddLandmark(root.transform, data, "PEAK B", 100f, 600f, Color.yellow);
        AddLandmark(root.transform, data, "BRIDGE 1", 150f, 600f, Color.white);
        AddLandmark(root.transform, data, "BRIDGE 2", 350f, 500f, Color.white);

        GameObject lightObject = new GameObject("BLACKSPIRE DIRECTIONAL LIGHT");
        Light sun = lightObject.AddComponent<Light>();
        sun.type = LightType.Directional;
        sun.intensity = 1.2f;
        lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        GameObject cameraObject = new GameObject("BLACKSPIRE BLOCKOUT CAMERA");
        Camera camera = cameraObject.AddComponent<Camera>();
        cameraObject.tag = "MainCamera";
        camera.fieldOfView = 60f;
        camera.farClipPlane = 3000f;
        cameraObject.transform.position = new Vector3(1350f, 1050f, -500f);
        cameraObject.transform.rotation = Quaternion.LookRotation(
            new Vector3(500f, 50f, 500f) - cameraObject.transform.position);

        scene.name = "Blackspire_Continent";
        EditorSceneManager.SaveScene(scene, ScenePath);

        EditorBuildSettingsScene[] existing = EditorBuildSettings.scenes;
        bool alreadyListed = false;
        foreach (EditorBuildSettingsScene entry in existing)
            if (entry.path == ScenePath) alreadyListed = true;

        if (!alreadyListed)
        {
            Array.Resize(ref existing, existing.Length + 1);
            existing[existing.Length - 1] =
                new EditorBuildSettingsScene(ScenePath, true);
            EditorBuildSettings.scenes = existing;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Selection.activeGameObject = terrainObject;

        Debug.Log("[BLACKSPIRE] BLOCKOUT CREATED: " + ScenePath +
                  " | Terrain 1000 x 1000 m | Heightmap 513 | Landmark markers placed.");
    }

    private static float Gaussian(float x, float cx, float sx, float z, float cz, float sz)
    {
        float dx = (x - cx) / sx;
        float dz = (z - cz) / sz;
        return Mathf.Exp(-.5f * (dx * dx + dz * dz));
    }

    private static float DistanceToPolyline(float x, float z, params Vector2[] points)
    {
        float best = float.MaxValue;

        for (int i = 0; i < points.Length - 1; i++)
        {
            Vector2 a = points[i];
            Vector2 b = points[i + 1];
            Vector2 p = new Vector2(x, z);
            Vector2 ab = b - a;
            float denominator = Vector2.Dot(ab, ab);
            float t = denominator > 0f
                ? Mathf.Clamp01(Vector2.Dot(p - a, ab) / denominator)
                : 0f;
            best = Mathf.Min(best, Vector2.Distance(p, a + t * ab));
        }

        return best;
    }

    private static void AddLandmark(
        Transform parent, TerrainData data, string label,
        float x, float z, Color color)
    {
        float ground = data.GetInterpolatedHeight(x / 1000f, z / 1000f);

        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
        marker.name = label;
        marker.transform.SetParent(parent, true);
        marker.transform.position = new Vector3(x, ground + 5f, z);
        marker.transform.localScale = new Vector3(10f, 10f, 10f);

        Renderer renderer = marker.GetComponent<Renderer>();
        Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        if (material.shader == null)
            material.shader = Shader.Find("Standard");
        material.color = color;
        renderer.sharedMaterial = material;

        GameObject textObject = new GameObject(label + " LABEL");
        textObject.transform.SetParent(parent, true);
        textObject.transform.position = new Vector3(x, ground + 17f, z);
        TextMesh text = textObject.AddComponent<TextMesh>();
        text.text = label;
        text.fontSize = 48;
        text.characterSize = 0.25f;
        text.anchor = TextAnchor.MiddleCenter;
        text.alignment = TextAlignment.Center;
        text.color = Color.white;
    }

    private static void EnsureFolder(string path)
    {
        if (AssetDatabase.IsValidFolder(path)) return;

        string parent = Path.GetDirectoryName(path).Replace("\\", "/");
        string folder = Path.GetFileName(path);

        if (!AssetDatabase.IsValidFolder(parent))
            EnsureFolder(parent);

        AssetDatabase.CreateFolder(parent, folder);
    }
}
