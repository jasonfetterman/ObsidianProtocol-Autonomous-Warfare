using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class DeploymentFacilityWindowBuilder
{
    private const string ScenePath =
        "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

    private const string RootName = "[FACILITY] GENERATED WINDOWS";

    [MenuItem("Obsidian Protocol/Deployment/Build Front and Rear Windows")]
    private static void BuildWindows()
    {
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog(
                "Facility Windows",
                "Exit Play Mode before building windows.",
                "OK");
            return;
        }

        Scene scene = SceneManager.GetActiveScene();

        if (scene.path != ScenePath)
        {
            EditorUtility.DisplayDialog(
                "Wrong Scene",
                "Open SCN12_DEPLOYMENT before running this builder.",
                "OK");
            return;
        }

        if (FindSceneObject(scene, RootName) != null)
        {
            EditorUtility.DisplayDialog(
                "Already Built",
                "Generated windows already exist. No changes made.",
                "OK");
            return;
        }

        GameObject north = FindSceneObject(scene, "[FACILITY] NORTH WALL");
        GameObject southLeft = FindSceneObject(scene, "[WALL] SOUTH LEFT");
        GameObject southRight = FindSceneObject(scene, "[WALL] SOUTH RIGHT");

        if (north == null || southLeft == null || southRight == null)
        {
            EditorUtility.DisplayDialog(
                "Missing Wall",
                "Could not find the expected north wall and south entrance wall sections. No changes made.",
                "OK");
            return;
        }

        if (!EditorUtility.DisplayDialog(
            "Build Facility Windows",
            "Add reinforced industrial window panels to the front and rear walls? Existing walls will not be cut or deleted.",
            "Build",
            "Cancel"))
            return;

        GameObject building = FindSceneObject(scene, "BUILDING");
        if (building == null)
        {
            EditorUtility.DisplayDialog(
                "Missing BUILDING",
                "Could not find BUILDING. No changes made.",
                "OK");
            return;
        }

        GameObject root = new GameObject(RootName);
        Undo.RegisterCreatedObjectUndo(root, "Build Facility Windows");
        Undo.SetTransformParent(root.transform, building.transform, "Parent Facility Windows");

        // Rear wall: three visual window panels along the north wall.
        for (int i = -1; i <= 1; i++)
        {
            CreateWindow(root.transform, "REAR WINDOW " + (i + 2),
                new Vector3(i * 17f, 7.5f, 36.35f),
                new Vector3(10f, 4f, 0.12f));
        }

        // Front wall: two panels, one on each side of the entrance.
        CreateWindow(root.transform, "FRONT WINDOW LEFT",
            new Vector3(-22.5f, 7.5f, -26.35f),
            new Vector3(12f, 4f, 0.12f));

        CreateWindow(root.transform, "FRONT WINDOW RIGHT",
            new Vector3(22.5f, 7.5f, -26.35f),
            new Vector3(12f, 4f, 0.12f));

        EditorSceneManager.MarkSceneDirty(scene);
        Selection.activeGameObject = root;

        EditorUtility.DisplayDialog(
            "Window Panels Created",
            "Inspect the front and rear window panels. The original walls remain intact.",
            "OK");
    }

    private static void CreateWindow(
        Transform parent,
        string objectName,
        Vector3 localPosition,
        Vector3 localScale)
    {
        GameObject pane = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Undo.RegisterCreatedObjectUndo(pane, "Create " + objectName);
        pane.name = objectName;
        pane.transform.SetParent(parent, false);
        pane.transform.localPosition = localPosition;
        pane.transform.localRotation = Quaternion.identity;
        pane.transform.localScale = localScale;

        Renderer renderer = pane.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material glass = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            glass.name = objectName + " Material";
            glass.SetFloat("_Metallic", 0.35f);
            glass.SetFloat("_Smoothness", 0.85f);
            glass.color = new Color(0.08f, 0.22f, 0.28f, 1f);
            renderer.sharedMaterial = glass;
        }

        // The pane remains non-traversable; wall openings are not cut by this script.
    }

    private static GameObject FindSceneObject(Scene scene, string objectName)
    {
        foreach (GameObject sceneRoot in scene.GetRootGameObjects())
        {
            foreach (Transform child in sceneRoot.GetComponentsInChildren<Transform>(true))
            {
                if (child.name == objectName)
                    return child.gameObject;
            }
        }

        return null;
    }
}