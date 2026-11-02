using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public static class StoreSixScreenCleanup
{
    [MenuItem("Obsidian Protocol/Store/CLEAN UP SIX OLD SCREENS")]
    private static void Run()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.path.Replace("\\", "/") !=
            "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity")
        {
            Debug.LogError("Open SCN16_STORE.unity first. Nothing changed.");
            return;
        }

        if (EditorApplication.isPlaying)
        {
            Debug.LogError("Stop Play Mode first. Nothing changed.");
            return;
        }

        string backup = scene.path + ".bak_cleanup_" +
            DateTime.Now.ToString("yyyyMMdd_HHmmss");
        if (!EditorSceneManager.SaveScene(scene) ||
            File.Exists(backup))
        {
            Debug.LogError("Could not safely create scene backup. Nothing changed.");
            return;
        }
        File.Copy(scene.path, backup);

        string[] oldNames = {
            "AIR UNITS wall screen",
            "GROUND UNITS wall screen",
            "SEA UNITS wall screen",
            "COMMANDERS wall screen",
            "EXPERIMENTAL UNITS wall screen",
            "SATELLITES wall screen"
        };

        Transform facility = FindRoot("STORE_FACILITY");
        Transform rebuild = FindRoot("STORE_AAA_REBUILD");

        if (facility == null || rebuild == null)
        {
            Debug.LogError("Required Store groups missing. Restore backup if needed.");
            return;
        }

        foreach (string name in oldNames)
        {
            Transform oldScreen = FindDescendant(facility, name);
            if (oldScreen != null)
                Undo.DestroyObjectImmediate(oldScreen.gameObject);
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("Removed old category screens. Windows and new displays untouched. Backup: " + backup);
    }

    private static Transform FindRoot(string name)
    {
        foreach (GameObject go in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (go.name == name) return go.transform;
            Transform found = FindDescendant(go.transform, name);
            if (found != null) return found;
        }
        return null;
    }

    private static Transform FindDescendant(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name) return child;
            Transform found = FindDescendant(child, name);
            if (found != null) return found;
        }
        return null;
    }
}
