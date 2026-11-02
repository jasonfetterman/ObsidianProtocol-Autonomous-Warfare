using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class BuildBattlefieldWorldAAA
{
    private const string ScenePath =
        "Assets/Scenes/SCN03_BATTLEFIELD_AAA/SCN03_BATTLEFIELD_AAA.unity";

    [MenuItem("OPAW/AAA Battlefield/Build 3D World Preview")]
    public static void Build()
    {
        if (!EditorUtility.DisplayDialog(
            "Build AAA Battlefield World",
            "Add a separate 3D battlefield world, lighting, and camera to the existing AAA scene? The existing HUD will be preserved. A backup is strongly recommended.",
            "Build World", "Cancel"))
            return;

        Scene scene = SceneManager.GetActiveScene();
        if (scene.path != ScenePath)
        {
            EditorUtility.DisplayDialog(
                "Wrong Scene",
                "Open SCN03_BATTLEFIELD_AAA before running this builder.",
                "OK");
            return;
        }

        if (GameObject.Find("[WORLD] AAA BATTLEFIELD") != null)
        {
            EditorUtility.DisplayDialog(
                "World Already Exists",
                "A battlefield world root already exists. Nothing was changed.",
                "OK");
            return;
        }

        GameObject world = new GameObject("[WORLD] AAA BATTLEFIELD");

        Material groundMat = MakeMaterial("MAT AAA Ground", new Color(0.16f, 0.22f, 0.17f));
        Material hillMat = MakeMaterial("MAT AAA High Ground", new Color(0.24f, 0.29f, 0.20f));
        Material roadMat = MakeMaterial("MAT AAA Roads", new Color(0.16f, 0.17f, 0.16f));
        Material coverMat = MakeMaterial("MAT AAA Defensive Cover", new Color(0.34f, 0.32f, 0.25f));
        Material blueMat = MakeMaterial("MAT AAA Blue Deployment", new Color(0.08f, 0.36f, 0.52f, 0.75f));
        Material redMat = MakeMaterial("MAT AAA Red Deployment", new Color(0.55f, 0.12f, 0.10f, 0.75f));

        CreateCube(world.transform, "[WORLD] Ground", new Vector3(0f, -0.6f, 0f),
            new Vector3(160f, 1f, 140f), groundMat);

        CreateCube(world.transform, "[TERRAIN] Northern Ridge", new Vector3(0f, 1.8f, 43f),
            new Vector3(105f, 4f, 18f), hillMat);
        CreateCube(world.transform, "[TERRAIN] Western Rise", new Vector3(-57f, 1.1f, 5f),
            new Vector3(18f, 2.8f, 65f), hillMat);
        CreateCube(world.transform, "[TERRAIN] Eastern Rise", new Vector3(58f, 1.3f, -8f),
            new Vector3(17f, 3.2f, 60f), hillMat);

        CreateCube(world.transform, "[ROAD] Main Supply Route", new Vector3(0f, -0.06f, 0f),
            new Vector3(12f, 0.12f, 122f), roadMat);
        CreateCube(world.transform, "[ROAD] Cross Route", new Vector3(0f, -0.045f, 8f),
            new Vector3(112f, 0.1f, 9f), roadMat);

        for (int i = 0; i < 7; i++)
        {
            float x = -36f + i * 12f;
            CreateCube(world.transform, "[COVER] North Defensive Barrier " + (i + 1),
                new Vector3(x, 0.8f, 23f), new Vector3(7f, 1.6f, 2.2f), coverMat);
            CreateCube(world.transform, "[COVER] South Defensive Barrier " + (i + 1),
                new Vector3(x, 0.8f, -28f), new Vector3(7f, 1.6f, 2.2f), coverMat);
        }

        CreateCube(world.transform, "[DEPLOYMENT] BLUE ZONE",
            new Vector3(0f, 0.03f, -48f), new Vector3(54f, 0.08f, 14f), blueMat);
        CreateCube(world.transform, "[DEPLOYMENT] RED ZONE",
            new Vector3(0f, 0.03f, 51f), new Vector3(54f, 0.08f, 12f), redMat);

        CreateLabel(world.transform, "[LABEL] BLUE FORCES", new Vector3(0f, 1.2f, -48f),
            "BLUE FORCES  /  DEPLOYMENT AREA", new Color(0.15f, 0.8f, 1f));
        CreateLabel(world.transform, "[LABEL] RED FORCES", new Vector3(0f, 1.2f, 51f),
            "RED FORCES  /  OPPOSING SECTOR", new Color(1f, 0.3f, 0.2f));

        GameObject cameraObject = new GameObject("[CAMERA] AAA BATTLEFIELD");
        cameraObject.transform.SetParent(world.transform);
        cameraObject.transform.position = new Vector3(0f, 105f, -118f);
        cameraObject.transform.rotation = Quaternion.Euler(34f, 0f, 0f);

        Camera camera = cameraObject.AddComponent<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.035f, 0.055f, 0.065f);
        camera.orthographic = false;
        camera.fieldOfView = 52f;
        camera.nearClipPlane = 0.3f;
        camera.farClipPlane = 500f;
        camera.depth = -1;
        cameraObject.tag = "MainCamera";
        cameraObject.AddComponent<AudioListener>();

        GameObject lightObject = new GameObject("[LIGHT] AAA Battlefield Sun");
        lightObject.transform.SetParent(world.transform);
        lightObject.transform.rotation = Quaternion.Euler(48f, -32f, 0f);
        Light sun = lightObject.AddComponent<Light>();
        sun.type = LightType.Directional;
        sun.intensity = 1.25f;
        sun.color = new Color(1f, 0.91f, 0.78f);

        RenderSettings.ambientLight = new Color(0.36f, 0.39f, 0.36f);
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.16f, 0.20f, 0.19f);
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogDensity = 0.0025f;

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Selection.activeGameObject = world;

        EditorUtility.DisplayDialog(
            "Battlefield World Added",
            "The 3D battlefield preview was added and the scene saved. Enter Play mode to inspect it. The HUD remains in place; deployment and tactical-map integration are the next steps.",
            "OK");
    }

    private static Material MakeMaterial(string name, Color color)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) shader = Shader.Find("Standard");
        Material material = new Material(shader);
        material.name = name;
        material.color = color;
        return material;
    }

    private static GameObject CreateCube(
        Transform parent, string name, Vector3 position, Vector3 scale, Material material)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.SetParent(parent, false);
        go.transform.position = position;
        go.transform.localScale = scale;
        Renderer renderer = go.GetComponent<Renderer>();
        renderer.sharedMaterial = material;
        return go;
    }

    private static void CreateLabel(
        Transform parent, string name, Vector3 position, string text, Color color)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.transform.position = position;
        go.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        TextMesh label = go.AddComponent<TextMesh>();
        label.text = text;
        label.fontSize = 48;
        label.characterSize = 0.16f;
        label.anchor = TextAnchor.MiddleCenter;
        label.alignment = TextAlignment.Center;
        label.color = color;
    }
}
