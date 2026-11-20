using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class AuditDeploymentSceneIdentity
{
    [MenuItem("Obsidian Protocol/Deployment/AUDIT SCENE IDENTITY")]
    public static void Audit()
    {
        const string scenePath =
            "Assets/Scenes/SCN12_DEPLOYMENT/Deployment.unity";

        var scene =
            EditorSceneManager.OpenScene(
                scenePath,
                OpenSceneMode.Single
            );

        Debug.Log(
            "[DEPLOYMENT IDENTITY] " +
            "PATH=" + scene.path
        );

        Debug.Log(
            "[DEPLOYMENT IDENTITY] " +
            "NAME=" + scene.name
        );

        Debug.Log(
            "[DEPLOYMENT IDENTITY] " +
            "VALID=" + scene.IsValid()
        );

        Debug.Log(
            "[DEPLOYMENT IDENTITY] " +
            "BUILD INDEX=" + scene.buildIndex
        );
    }
}
