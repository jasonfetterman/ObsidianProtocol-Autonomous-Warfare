using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public static class BuildDeploymentOperationSelectionController
{
    [MenuItem("Obsidian Protocol/Deployment/WIRE Operation Selection Controller")]
    public static void Build()
    {
        string scenePath =
            "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

        Scene scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        GameObject system = FindSceneObject(
            "[SYSTEM] OPERATION SELECTION"
        );

        GameObject buttonObject = FindSceneObject(
            "[BUTTON] SELECT OPERATION"
        );

        GameObject displayObject = FindSceneObject(
            "[DISPLAY] Operation"
        );

        if (system == null)
        {
            Debug.LogError(
                "[OPERATION SELECTION] SYSTEM NOT FOUND."
            );
            return;
        }

        if (buttonObject == null)
        {
            Debug.LogError(
                "[OPERATION SELECTION] SELECT OPERATION BUTTON NOT FOUND."
            );
            return;
        }

        if (displayObject == null)
        {
            Debug.LogError(
                "[OPERATION SELECTION] OPERATION DISPLAY NOT FOUND."
            );
            return;
        }

        Button button =
            buttonObject.GetComponent<Button>();

        Text display =
            displayObject.GetComponent<Text>();

        if (button == null)
        {
            Debug.LogError(
                "[OPERATION SELECTION] BUTTON COMPONENT NOT FOUND."
            );
            return;
        }

        if (display == null)
        {
            Debug.LogError(
                "[OPERATION SELECTION] TEXT COMPONENT NOT FOUND."
            );
            return;
        }

        DeploymentOperationSelectionController controller =
            system.GetComponent<
                DeploymentOperationSelectionController>();

        if (controller == null)
        {
            controller =
                system.AddComponent<
                    DeploymentOperationSelectionController>();
        }

        SerializedObject serialized =
            new SerializedObject(controller);

        serialized.FindProperty(
            "selectOperationButton"
        ).objectReferenceValue = button;

        serialized.FindProperty(
            "operationDisplay"
        ).objectReferenceValue = display;

        serialized.ApplyModifiedPropertiesWithoutUndo();

        EditorUtility.SetDirty(system);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[OPERATION SELECTION] SYSTEM WIRED."
        );

        Debug.Log(
            "[OPERATION SELECTION] BUTTON = " +
            buttonObject.name
        );

        Debug.Log(
            "[OPERATION SELECTION] DISPLAY = " +
            displayObject.name
        );

        Debug.Log(
            "[OPERATION SELECTION] INITIAL DISPLAY = " +
            display.text
        );

        Debug.Log(
            "[OPERATION SELECTION] WIRING COMPLETE."
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
