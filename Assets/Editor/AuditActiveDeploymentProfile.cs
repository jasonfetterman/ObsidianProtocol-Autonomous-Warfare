using UnityEditor;
using UnityEditor.Build.Profile;
using UnityEngine;

public static class AuditActiveDeploymentProfile
{
    [MenuItem("Obsidian Protocol/Deployment/AUDIT ACTIVE PROFILE")]
    public static void Audit()
    {
        BuildProfile profile =
            BuildProfile.GetActiveBuildProfile();

        if (profile == null)
        {
            Debug.Log(
                "[DEPLOYMENT PROFILE AUDIT] " +
                "NO CUSTOM BUILD PROFILE ACTIVE."
            );

            return;
        }

        Debug.Log(
            "[DEPLOYMENT PROFILE AUDIT] " +
            "ACTIVE PROFILE=" + profile.name
        );

        Debug.Log(
            "[DEPLOYMENT PROFILE AUDIT] " +
            "ASSET PATH=" +
            AssetDatabase.GetAssetPath(profile)
        );

        Debug.Log(
            "[DEPLOYMENT PROFILE AUDIT] " +
            "OVERRIDE GLOBAL SCENES=" +
            profile.overrideGlobalScenes
        );

        var scenes =
            profile.GetScenesForBuild();

        bool found = false;

        foreach (var scene in scenes)
        {
            Debug.Log(
                "[DEPLOYMENT PROFILE AUDIT] SCENE | " +
                "ENABLED=" + scene.enabled +
                " | " +
                scene.path
            );

            if (scene.path ==
                "Assets/Scenes/SCN12_DEPLOYMENT/Deployment.unity")
            {
                found = true;
            }
        }

        Debug.Log(
            "[DEPLOYMENT PROFILE AUDIT] " +
            "SCN12 IN EFFECTIVE BUILD LIST=" +
            found
        );
    }
}
