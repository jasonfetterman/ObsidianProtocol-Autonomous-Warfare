using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public static class BuildDeploymentOperationTerminal
{
    [MenuItem("Obsidian Protocol/Deployment/WIRE Operation Selection Terminal")]
    public static void Build()
    {
        string scenePath =
            "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

        Scene scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        GameObject terminal = FindSceneObject(
            "[STATION] OPERATION SELECTION TERMINAL"
        );

        if (terminal == null)
        {
            Debug.LogError(
                "[OPERATION TERMINAL] STATION NOT FOUND."
            );
            return;
        }

        GameObject hud = FindSceneObject(
            "[HUD] DEPLOYMENT HUD"
        );

        if (hud == null)
        {
            Debug.LogError(
                "[OPERATION TERMINAL] DEPLOYMENT HUD NOT FOUND."
            );
            return;
        }

        GameObject window = FindSceneObject(
            "[WINDOW] SELECT OPERATION"
        );

        if (window == null)
        {
            Debug.LogError(
                "[OPERATION TERMINAL] SELECT OPERATION WINDOW NOT FOUND."
            );
            return;
        }

        BoxCollider collider =
            terminal.GetComponent<BoxCollider>();

        if (collider == null)
        {
            collider =
                terminal.AddComponent<BoxCollider>();
        }

        collider.isTrigger = false;

        DeploymentOperationTerminalInteraction interaction =
            terminal.GetComponent<
                DeploymentOperationTerminalInteraction>();

        if (interaction == null)
        {
            interaction =
                terminal.AddComponent<
                    DeploymentOperationTerminalInteraction>();
        }

        SerializedObject serialized =
            new SerializedObject(interaction);

        serialized.FindProperty("deploymentHUD").objectReferenceValue =
            hud;

        serialized.FindProperty("selectOperationWindow")
            .objectReferenceValue = window;

        serialized.FindProperty("interactionDistance").floatValue =
            3.0f;

        serialized.ApplyModifiedPropertiesWithoutUndo();

        EditorUtility.SetDirty(terminal);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[OPERATION TERMINAL] STATION WIRED."
        );

        Debug.Log(
            "[OPERATION TERMINAL] HUD ASSIGNED = " +
            hud.name
        );

        Debug.Log(
            "[OPERATION TERMINAL] WINDOW ASSIGNED = " +
            window.name
        );

        Debug.Log(
            "[OPERATION TERMINAL] COLLIDER = " +
            collider.GetType().Name +
            " | TRIGGER=" +
            collider.isTrigger
        );

        Debug.Log(
            "[OPERATION TERMINAL] WIRING COMPLETE."
        );
    }

    private static GameObject FindSceneObject(string objectName)
    {
        return Resources.FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(go =>
                go.scene.IsValid() &&
                go.scene.isLoaded &&
                go.name == objectName &&
                go.hideFlags == HideFlags.None
            );
    }
}
