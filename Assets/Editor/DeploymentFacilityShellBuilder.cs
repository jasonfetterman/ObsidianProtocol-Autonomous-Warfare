using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public static class DeploymentFacilityShellBuilder
{
    private const string ScenePath =
        "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

    private const string RootName =
        "[FACILITY] GENERATED ENTRANCE";

    [MenuItem("Obsidian Protocol/Deployment/Build Facility Shell Entrance")]
    public static void Build()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogError("[FACILITY] Stop Play Mode before building.");
            return;
        }

        Scene scene = SceneManager.GetActiveScene();
        if (scene.path != ScenePath)
        {
            Debug.LogError(
                "[FACILITY] Open the exact scene first: " + ScenePath);
            return;
        }

        GameObject building = FindSceneObjectIncludingInactive(scene, "BUILDING");
        GameObject southWall = FindSceneObjectIncludingInactive(scene, "[FACILITY] SOUTH WALL");
        GameObject floor = FindSceneObjectIncludingInactive(scene, "[FACILITY] FLOOR");

        if (building == null || southWall == null || floor == null)
        {
            Debug.LogError(
                "[FACILITY] Required BUILDING, SOUTH WALL, or FLOOR missing. Nothing changed.");
            return;
        }

        if (GameObject.Find(RootName) != null)
        {
            Debug.LogWarning(
                "[FACILITY] Generated entrance already exists. Nothing changed.");
            return;
        }

        if (!EditorUtility.DisplayDialog(
            "Build Deployment Facility Entrance",
            "Create a 6-unit-wide entrance in the south wall and activate the existing facility shell? The scene will not be cleared.",
            "Build Entrance",
            "Cancel"))
            return;

        Undo.IncrementCurrentGroup();
        int undoGroup = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName("Build Deployment Facility Entrance");

        GameObject root = new GameObject(RootName);
        Undo.RegisterCreatedObjectUndo(root, "Create facility entrance");
        Undo.SetTransformParent(root.transform, building.transform, "Parent entrance");
        root.transform.localPosition = Vector3.zero;
        root.transform.localRotation = Quaternion.identity;
        root.transform.localScale = Vector3.one;

        // The original south wall is 84 wide, 15 high, 1 deep,
        // centered at local (0, 7, -27). Leave a 6-wide, 9-high opening.
        CreateWallSection(root.transform, "[WALL] SOUTH LEFT",
            new Vector3(-22.5f, 7f, -27f), new Vector3(39f, 15f, 1f));
        CreateWallSection(root.transform, "[WALL] SOUTH RIGHT",
            new Vector3(22.5f, 7f, -27f), new Vector3(39f, 15f, 1f));
        CreateWallSection(root.transform, "[WALL] SOUTH LINTEL",
            new Vector3(0f, 11.5f, -27f), new Vector3(6f, 6f, 1f));

        Undo.RecordObject(southWall, "Disable original south wall");
        southWall.SetActive(false);

        ActivateExisting("[FACILITY] EAST WALL");
        ActivateExisting("[FACILITY] NORTH WALL");
        ActivateExisting("[FACILITY] WEST WALL");
        ActivateExisting("[FACILITY] CEILING");

        EditorSceneManager.MarkSceneDirty(scene);
        Undo.CollapseUndoOperations(undoGroup);

        Debug.Log(
            "[FACILITY] Entrance built. Inspect the scene and test player clearance before saving.");
    }

    private static GameObject FindSceneObjectIncludingInactive(Scene scene, string objectName)
    {
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            foreach (Transform item in root.GetComponentsInChildren<Transform>(true))
            {
                if (item.name == objectName)
                    return item.gameObject;
            }
        }

        return null;
    }
    private static void ActivateExisting(string objectName)
    {
        GameObject go = GameObject.Find(objectName);
        if (go == null)
        {
            Debug.LogWarning("[FACILITY] Not found: " + objectName);
            return;
        }

        Undo.RecordObject(go, "Activate facility shell");
        go.SetActive(true);
    }

    private static void CreateWallSection(
        Transform parent, string objectName, Vector3 position, Vector3 size)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Undo.RegisterCreatedObjectUndo(go, "Create " + objectName);
        go.name = objectName;
        Undo.SetTransformParent(go.transform, parent, "Parent " + objectName);
        go.transform.localPosition = position;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = size;
        go.isStatic = true;
    }
}

