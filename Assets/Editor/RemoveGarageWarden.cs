using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class RemoveGarageWarden
{
    private const string ScenePath =
        "Assets/Scenes/SCN04_GARAGE/SCN04_GARAGE.unity";

    private const string BackupPath =
        "Assets/Scenes/SCN04_GARAGE/SCN04_GARAGE_BEFORE_REMOVE_WARDEN_20261011_160918.unity";

    [MenuItem("Obsidian Protocol/Garage/Remove Placed Warden")]
    private static void RemovePlacedWarden()
    {
        if (!System.IO.File.Exists(BackupPath))
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

        var matches = new List<GameObject>();

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            foreach (Transform item in root.GetComponentsInChildren<Transform>(true))
            {
                if (item.name == "AIR_WARDEN")
                    matches.Add(item.gameObject);
            }
        }

        if (matches.Count != 1)
        {
            Debug.LogError(
                "[GARAGE] Expected exactly one AIR_WARDEN; found " +
                matches.Count + ". Nothing removed.");

            if (openedHere)
                EditorSceneManager.CloseScene(scene, true);

            return;
        }

        GameObject warden = matches[0];
        string parentName = warden.transform.parent != null
            ? warden.transform.parent.name
            : "(scene root)";

        Undo.DestroyObjectImmediate(warden);
        EditorSceneManager.MarkSceneDirty(scene);

        if (!EditorSceneManager.SaveScene(scene))
        {
            Debug.LogError(
                "[GARAGE] Scene save failed. Restore from the verified backup if needed.");
            return;
        }

        Debug.Log(
            "[GARAGE] Removed AIR_WARDEN and its child hierarchy. " +
            "Parent preserved: " + parentName +
            ". Scene saved: " + ScenePath);

        if (openedHere)
            EditorSceneManager.CloseScene(scene, true);
    }
}
