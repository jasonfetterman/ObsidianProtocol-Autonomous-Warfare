using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class RemoveGarageUnitPlaceholders
{
    private const string ScenePath =
        "Assets/Scenes/SCN04_GARAGE/SCN04_GARAGE.unity";

    private const string BackupPath =
        "Assets/Scenes/SCN04_GARAGE/SCN04_GARAGE_BEFORE_REMOVE_UNIT_PLACEHOLDERS_20261011_161836.unity";

    private const int ExpectedChildCount = 18;

    [MenuItem("Obsidian Protocol/Garage/Remove Unit Placeholders")]
    private static void RemoveUnitPlaceholders()
    {
        if (!File.Exists(ScenePath))
        {
            Debug.LogError("[GARAGE] Scene file is missing. Nothing removed.");
            return;
        }

        if (!File.Exists(BackupPath))
        {
            Debug.LogError("[GARAGE] Verified backup is missing. Nothing removed.");
            return;
        }

        Scene scene = SceneManager.GetSceneByPath(ScenePath);
        bool openedHere = !scene.IsValid() || !scene.isLoaded;

        if (openedHere)
            scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Additive);

        if (!scene.IsValid() || !scene.isLoaded)
        {
            Debug.LogError("[GARAGE] Could not load the Garage scene.");
            return;
        }

        var containers = new List<GameObject>();

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            foreach (Transform item in root.GetComponentsInChildren<Transform>(true))
            {
                if (item.name == "GARAGE_UNITS")
                    containers.Add(item.gameObject);
            }
        }

        if (containers.Count != 1)
        {
            Debug.LogError(
                "[GARAGE] Expected exactly one GARAGE_UNITS container; found " +
                containers.Count + ". Nothing removed.");

            if (openedHere)
                EditorSceneManager.CloseScene(scene, true);

            return;
        }

        Transform container = containers[0].transform;
        int childCount = container.childCount;

        if (childCount != ExpectedChildCount)
        {
            Debug.LogError(
                "[GARAGE] Expected exactly " + ExpectedChildCount +
                " direct children under GARAGE_UNITS; found " + childCount +
                ". Nothing removed.");

            if (openedHere)
                EditorSceneManager.CloseScene(scene, true);

            return;
        }

        var children = new List<GameObject>();

        for (int i = 0; i < container.childCount; i++)
            children.Add(container.GetChild(i).gameObject);

        foreach (GameObject child in children)
            Undo.DestroyObjectImmediate(child);

        if (container.childCount != 0)
        {
            Debug.LogError(
                "[GARAGE] Container is not empty after removal. Check the scene before proceeding.");
            return;
        }

        EditorSceneManager.MarkSceneDirty(scene);

        if (!EditorSceneManager.SaveScene(scene))
        {
            Debug.LogError(
                "[GARAGE] Scene save failed. Restore from the verified backup if needed.");
            return;
        }

        Debug.Log(
            "[GARAGE] Removed " + children.Count +
            " unit placeholder hierarchies. GARAGE_UNITS is now empty. Scene saved: " +
            ScenePath);

        if (openedHere)
            EditorSceneManager.CloseScene(scene, true);
    }
}
