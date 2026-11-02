using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public static class AuditDeploymentBuildProfile
{
    [MenuItem("Obsidian Protocol/Deployment/AUDIT BUILD PROFILE")]
    public static void Audit()
    {
        const string scenePath =
            "Assets/Scenes/SCN12_DEPLOYMENT/Deployment.unity";

        bool found = false;

        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (scene.path == scenePath)
            {
                found = true;

                Debug.Log(
                    "[DEPLOYMENT BUILD AUDIT] FOUND | " +
                    "ENABLED=" + scene.enabled +
                    " | PATH=" + scene.path
                );

                break;
            }
        }

        if (!found)
        {
            Debug.LogError(
                "[DEPLOYMENT BUILD AUDIT] NOT FOUND IN BUILD PROFILE | " +
                scenePath
            );
        }
    }
}
