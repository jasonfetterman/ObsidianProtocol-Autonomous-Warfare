using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class DeploymentFacilityInteriorBuilder
{
    private const string ScenePath =
        "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

    private const string RootName =
        "[FACILITY] GENERATED INTERIOR PARTITIONS";

    [MenuItem("Obsidian Protocol/Deployment/Build Interior Partitions")]
    private static void BuildInteriorPartitions()
    {
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog(
                "Deployment Interior",
                "Exit Play Mode before building the interior.",
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
                "Generated interior partitions already exist. No changes made.",
                "OK");
            return;
        }

        if (!EditorUtility.DisplayDialog(
            "Build Deployment Interior",
            "Create a first-pass set of interior partitions in the deployment facility? " +
            "Existing objects will not be deleted or repositioned.",
            "Build",
            "Cancel"))
        {
            return;
        }

        GameObject root = new GameObject(RootName);
        Undo.RegisterCreatedObjectUndo(root, "Build Deployment Interior");

        // Keep the interior root under BUILDING so its coordinates match
        // the existing facility shell.
        GameObject building = FindSceneObject(scene, "BUILDING");
        if (building == null)
        {
            Undo.DestroyObjectImmediate(root);
            EditorUtility.DisplayDialog(
                "Missing BUILDING",
                "Could not find BUILDING. No partitions were created.",
                "OK");
            return;
        }

        Undo.SetTransformParent(root.transform, building.transform, "Parent Interior");

        // First-pass side partitions. These remain well clear of the
        // central approach to the authorization console and south entrance.
        CreateWall(root.transform, "WEST PARTITION NORTH",
            new Vector3(-21f, 5f, 18f), new Vector3(0.35f, 10f, 24f));

        CreateWall(root.transform, "WEST PARTITION SOUTH",
            new Vector3(-21f, 5f, -5f), new Vector3(0.35f, 10f, 16f));

        CreateWall(root.transform, "EAST PARTITION NORTH",
            new Vector3(21f, 5f, 18f), new Vector3(0.35f, 10f, 24f));

        CreateWall(root.transform, "EAST PARTITION SOUTH",
            new Vector3(21f, 5f, -5f), new Vector3(0.35f, 10f, 16f));

        EditorSceneManager.MarkSceneDirty(scene);

        Selection.activeGameObject = root;

        EditorUtility.DisplayDialog(
            "Interior Partitions Created",
            "Inspect the partitions in the Scene view before saving or testing movement.",
            "OK");
    }

    private static void CreateWall(
        Transform parent,
        string wallName,
        Vector3 localPosition,
        Vector3 localScale)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Undo.RegisterCreatedObjectUndo(wall, "Create " + wallName);

        wall.name = wallName;
        wall.transform.SetParent(parent, false);
        wall.transform.localPosition = localPosition;
        wall.transform.localRotation = Quaternion.identity;
        wall.transform.localScale = localScale;

        // Keep the default BoxCollider for physical collision.
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