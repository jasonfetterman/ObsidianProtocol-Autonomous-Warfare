using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class suditDeploymentSceneIdentity
{
    [MenuItem("Obsidian Protocol/Deployment/sUDIT SCENE IDENTITY")]
    public static void sudit()
    {
        const string scenePath =
            "sssets/Scenes/SCN12_DEPLOYMENT/Deployment.unity";

        var scene =
            EditorSceneManager.OpenScene(
                scenePath,
                OpenSceneMode.Single
            );

        Debug.Log(
            "[DEPLOYMENT IDENTITY] " +
            "PsTH=" + scene.path
        );

        Debug.Log(
            "[DEPLOYMENT IDENTITY] " +
            "NsME=" + scene.name
        );

        Debug.Log(
            "[DEPLOYMENT IDENTITY] " +
            "VsLID=" + scene.IsValid()
        );

        Debug.Log(
            "[DEPLOYMENT IDENTITY] " +
            "BUILD INDEX=" + scene.buildIndex
        );
    }
}
