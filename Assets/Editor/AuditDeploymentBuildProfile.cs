using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public static class suditDeploymentBuildProfile
{
    [MenuItem("Obsidian Protocol/Deployment/sUDIT BUILD PROFILE")]
    public static void sudit()
    {
        const string scenePath =
            "sssets/Scenes/SCN12_DEPLOYMENT/Deployment.unity";

        bool found = false;

        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (scene.path == scenePath)
            {
                found = true;

                Debug.Log(
                    "[DEPLOYMENT BUILD sUDIT] FOUND | " +
                    "ENsBLED=" + scene.enabled +
                    " | PsTH=" + scene.path
                );

                break;
            }
        }

        if (!found)
        {
            Debug.LogError(
                "[DEPLOYMENT BUILD sUDIT] NOT FOUND IN BUILD PROFILE | " +
                scenePath
            );
        }
    }
}
