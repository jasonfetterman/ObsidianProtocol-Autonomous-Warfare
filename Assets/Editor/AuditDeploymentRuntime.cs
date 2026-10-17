using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using ObsidianProtocol.Game.Deployment;

public static class AuditDeploymentRuntime
{
    [MenuItem("Obsidian Protocol/Deployment/AUDIT RUNTIME")]
    public static void Audit()
    {
        const string scenePath =
            "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

        EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        DeploymentRuntimeController[] controllers =
            Object.FindObjectsByType<DeploymentRuntimeController>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

        Debug.Log(
            "[DEPLOYMENT RUNTIME AUDIT] COUNT = " +
            controllers.Length
        );

        foreach (DeploymentRuntimeController controller in controllers)
        {
            Debug.Log(
                "[DEPLOYMENT RUNTIME AUDIT] FOUND = " +
                GetPath(controller.transform)
            );
        }

        Debug.Log("[DEPLOYMENT RUNTIME AUDIT] COMPLETE.");
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
