using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class AuditDeploymentButtons
{
    [MenuItem("Obsidian Protocol/Deployment/AUDIT BUTTONS")]
    public static void Audit()
    {
        const string scenePath =
            "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

        EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        Button[] buttons =
            Object.FindObjectsByType<Button>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

        Debug.Log(
            "[DEPLOYMENT AUDIT] BUTTON COUNT = " +
            buttons.Length
        );

        foreach (Button button in buttons)
        {
            Debug.Log(
                "[DEPLOYMENT AUDIT] " +
                GetPath(button.transform) +
                " | ACTIVE=" +
                button.gameObject.activeSelf +
                " | LISTENERS=" +
                button.onClick.GetPersistentEventCount()
            );
        }

        Debug.Log("[DEPLOYMENT AUDIT] COMPLETE.");
    }

    private static string GetPath(Transform target)
    {
        string path = target.name;
        Transform parent = target.parent;

        while (parent != null)
        {
            path = parent.name + "/" + path;
            parent = parent.parent;
        }

        return path;
    }
}
