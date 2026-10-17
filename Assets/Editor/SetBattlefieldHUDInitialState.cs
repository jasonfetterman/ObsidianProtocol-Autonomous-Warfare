using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class SetBattlefieldHUDInitialState
{
    [MenuItem("Obsidian Protocol/Battlefield/SET HUD INITIAL STATE")]
    public static void Build()
    {
        const string scenePath =
            "Assets/Scenes/SCN03 _BATTLEFIELD/[HUD] BATTLEFIELD HUD/Battlefield_HUD.unity";

        var scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single);

        GameObject hudRoot = FindInScene(
            "[HUD] BATTLEFIELD HUD");

        if (hudRoot == null)
        {
            Debug.LogError(
                "[BATTLEFIELD HUD] HUD ROOT NOT FOUND.");
            return;
        }

        string[] windows =
        {
            "HUD UNIT TACTICAL DETAILS",
            "WINDOW COMMAND INTENT",
            "WINDOW OBJECTIVE PANEL",
            "WINDOW FORCE OVERVIEW",
            "WINDOW FORMATION MANAGEMENT",
            "WINDOW DOCTRINE",
            "WINDOW OPERATIONAL ORDERS",
            "WINDOW CONTACT DETAILS",
            "WINDOW NETWORK HUD",
            "WINDOW SENSOR COVERAGE",
            "WINDOW BATTLEFIELD LOGISTICS",
            "WINDOW ALERT DETAILS"
        };

        int closed = 0;

        foreach (string windowName in windows)
        {
            Transform target =
                FindChildRecursive(
                    hudRoot.transform,
                    windowName);

            if (target == null)
            {
                Debug.LogWarning(
                    "[BATTLEFIELD HUD] NOT FOUND: " +
                    windowName);
                continue;
            }

            target.gameObject.SetActive(false);
            closed++;

            Debug.Log(
                "[BATTLEFIELD HUD] CLOSED: " +
                target.name);
        }

        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[BATTLEFIELD HUD] INITIAL STATE COMPLETE // " +
            closed +
            " WINDOWS CLOSED.");
    }

    private static GameObject FindInScene(string name)
    {
        GameObject[] roots =
            UnityEngine.SceneManagement.SceneManager
                .GetActiveScene()
                .GetRootGameObjects();

        foreach (GameObject root in roots)
        {
            Transform found =
                FindChildRecursive(root.transform, name);

            if (found != null)
                return found.gameObject;
        }

        return null;
    }

    private static Transform FindChildRecursive(
        Transform parent,
        string name)
    {
        if (parent.name == name)
            return parent;

        foreach (Transform child in parent)
        {
            Transform found =
                FindChildRecursive(child, name);

            if (found != null)
                return found;
        }

        return null;
    }
}
