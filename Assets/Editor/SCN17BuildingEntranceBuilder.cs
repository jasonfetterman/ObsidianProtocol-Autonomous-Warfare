using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public static class SCN17BuildingEntranceBuilder
{
    private const string ScenePath =
        "Assets/Scenes/SCN17_PERSISTENT_MILITARY/SCN17_PERSISTENT_MILITARY.unity";
    private const string RootName = "SCN17 BUILDING ENTRANCE UPGRADE";
    private const string WallName = "SOUTH WALL";

    [MenuItem("Obsidian Protocol/SCN17/Build Building Entrance")]
    public static void Build()
    {
        if (!System.IO.File.Exists(ScenePath))
            throw new System.Exception("SCN17 active scene file was not found.");

        Scene scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

        if (scene.path != ScenePath)
            throw new System.Exception("Safety check failed: wrong scene opened.");

        GameObject architecture = GameObject.Find("FACILITY ARCHITECTURE");
        GameObject wall = GameObject.Find(WallName);

        if (architecture == null || wall == null)
            throw new System.Exception("Required architecture or south wall is missing.");

        Transform rootTransform = architecture.transform.Find(RootName);
        if (rootTransform != null)
            Object.DestroyImmediate(rootTransform.gameObject);

        Renderer wallRenderer = wall.GetComponent<Renderer>();
        if (wallRenderer == null)
            throw new System.Exception("South wall has no Renderer.");

        Material wallMaterial = wallRenderer.sharedMaterial;
        wall.SetActive(false);

        GameObject root = new GameObject(RootName);
        root.transform.SetParent(architecture.transform, false);

        // The original wall spans X=-41..41 and Y=-0.5..16.5.
        // The clear entrance is 6 units wide and 6 units high.
        CreateCube(root.transform, "ENTRANCE WALL LEFT",
            new Vector3(-22f, 8f, -26f), new Vector3(38f, 17f, 1f),
            wallMaterial, true);

        CreateCube(root.transform, "ENTRANCE WALL RIGHT",
            new Vector3(22f, 8f, -26f), new Vector3(38f, 17f, 1f),
            wallMaterial, true);

        CreateCube(root.transform, "ENTRANCE WALL HEADER",
            new Vector3(0f, 11.25f, -26f), new Vector3(6f, 10.5f, 1f),
            wallMaterial, true);

        // Heavy architectural frame, positioned on the interior face.
        CreateCube(root.transform, "DOOR FRAME LEFT",
            new Vector3(-3.25f, 3f, -25.35f), new Vector3(0.5f, 6.2f, 0.45f),
            wallMaterial, false);

        CreateCube(root.transform, "DOOR FRAME RIGHT",
            new Vector3(3.25f, 3f, -25.35f), new Vector3(0.5f, 6.2f, 0.45f),
            wallMaterial, false);

        CreateCube(root.transform, "DOOR FRAME HEADER",
            new Vector3(0f, 6.15f, -25.35f), new Vector3(7f, 0.5f, 0.45f),
            wallMaterial, false);

        // Door leaves are displayed slid open; no collider blocks the passage.
        CreateCube(root.transform, "SLIDING DOOR LEFT OPEN",
            new Vector3(-4.25f, 2.8f, -25.25f), new Vector3(2.1f, 5.6f, 0.18f),
            wallMaterial, false);

        CreateCube(root.transform, "SLIDING DOOR RIGHT OPEN",
            new Vector3(4.25f, 2.8f, -25.25f), new Vector3(2.1f, 5.6f, 0.18f),
            wallMaterial, false);

        // Header lights mark the access point.
        CreateLight(root.transform, "ENTRANCE LIGHT LEFT",
            new Vector3(-2.2f, 6.8f, -24.8f));
        CreateLight(root.transform, "ENTRANCE LIGHT RIGHT",
            new Vector3(2.2f, 6.8f, -24.8f));

        EditorSceneManager.MarkSceneDirty(scene);
        if (!EditorSceneManager.SaveScene(scene))
            throw new System.Exception("SCN17 scene save failed.");

        Debug.Log("SCN17 BUILDING ENTRANCE COMPLETE. Original SOUTH WALL preserved inactive; clear 6x6 passage created.");
    }

    private static GameObject CreateCube(
        Transform parent, string name, Vector3 position,
        Vector3 scale, Material material, bool keepCollider)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.SetParent(parent, false);
        go.transform.localPosition = position;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = scale;

        Renderer renderer = go.GetComponent<Renderer>();
        if (material != null)
            renderer.sharedMaterial = material;

        if (!keepCollider)
        {
            Collider collider = go.GetComponent<Collider>();
            if (collider != null)
                Object.DestroyImmediate(collider);
        }

        return go;
    }

    private static void CreateLight(Transform parent, string name, Vector3 position)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.transform.localPosition = position;

        Light light = go.AddComponent<Light>();
        light.type = LightType.Point;
        light.color = new Color(0.15f, 0.75f, 1f);
        light.intensity = 2f;
        light.range = 5f;
    }
}
