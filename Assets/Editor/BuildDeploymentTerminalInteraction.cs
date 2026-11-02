using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class BuildDeploymentTerminalInteraction
{
    [MenuItem("Obsidian Protocol/Deployment/WIRE AUTHORIZATION CONSOLE")]
    public static void Build()
    {
        var scene = EditorSceneManager.GetActiveScene();

        if (!scene.isLoaded)
        {
            Debug.LogError("[DEPLOYMENT TERMINAL] Active scene is not loaded.");
            return;
        }

        GameObject console = FindInScene(scene, "AUTHORIZATION CONSOLE");
        GameObject hud = FindInScene(scene, "[HUD] DEPLOYMENT HUD");

        if (console == null)
        {
            Debug.LogError("[DEPLOYMENT TERMINAL] AUTHORIZATION CONSOLE NOT FOUND.");
            return;
        }

        if (hud == null)
        {
            Debug.LogError("[DEPLOYMENT TERMINAL] [HUD] DEPLOYMENT HUD NOT FOUND.");
            return;
        }

        DeploymentTerminalInteraction interaction =
            console.GetComponent<DeploymentTerminalInteraction>();

        if (interaction == null)
            interaction = console.AddComponent<DeploymentTerminalInteraction>();

        SerializedObject serializedInteraction =
            new SerializedObject(interaction);

        SerializedProperty hudProperty =
            serializedInteraction.FindProperty("deploymentHUD");

        hudProperty.objectReferenceValue = hud;

        serializedInteraction.ApplyModifiedPropertiesWithoutUndo();

        hud.SetActive(false);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[DEPLOYMENT TERMINAL] AUTHORIZATION CONSOLE WIRED.");
        Debug.Log("[DEPLOYMENT TERMINAL] HUD ASSIGNED = " + hud.name);
        Debug.Log("[DEPLOYMENT TERMINAL] HUD START STATE = INACTIVE");
        Debug.Log("[DEPLOYMENT TERMINAL] WIRING COMPLETE.");
    }

    private static GameObject FindInScene(
        UnityEngine.SceneManagement.Scene scene,
        string targetName)
    {
        GameObject[] roots = scene.GetRootGameObjects();

        foreach (GameObject root in roots)
        {
            GameObject found = FindRecursive(root.transform, targetName);

            if (found != null)
                return found;
        }

        return null;
    }

    private static GameObject FindRecursive(
        Transform parent,
        string targetName)
    {
        if (parent.name == targetName)
            return parent.gameObject;

        for (int i = 0; i < parent.childCount; i++)
        {
            GameObject found =
                FindRecursive(parent.GetChild(i), targetName);

            if (found != null)
                return found;
        }

        return null;
    }
}
