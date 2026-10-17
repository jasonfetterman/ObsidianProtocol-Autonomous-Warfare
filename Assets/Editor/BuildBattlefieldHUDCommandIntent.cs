using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public static class BuildBattlefieldHUDCommandIntent
{
    private const string ScenePath =
        "Assets/Scenes/SCN03 _BATTLEFIELD/[HUD] BATTLEFIELD HUD/Battlefield_HUD.unity";

    [MenuItem("Obsidian Protocol/Battlefield/BUILD COMMAND INTENT")]
    public static void Build()
    {
        EditorSceneManager.OpenScene(ScenePath);

        GameObject hud = FindExact("[HUD] BATTLEFIELD HUD");
        if (hud == null)
        {
            Debug.LogError("[BATTLEFIELD HUD] [HUD] BATTLEFIELD HUD NOT FOUND.");
            return;
        }

        Transform leftCommandPanel =
            FindChildRecursive(
                hud.transform,
                "[PANEL] LEFT COMMAND PANEL");

        if (leftCommandPanel == null)
        {
            Debug.LogError(
                "[BATTLEFIELD HUD] LEFT COMMAND PANEL NOT FOUND.");
            return;
        }

        Transform commandTransform =
            leftCommandPanel
                .Cast<Transform>()
                .FirstOrDefault(
                    child => child.name == "[BUTTON] COMMAND");

        if (commandTransform == null)
        {
            Debug.LogError(
                "[BATTLEFIELD HUD] LEFT PANEL COMMAND BUTTON NOT FOUND.");
            return;
        }

        Button commandButton =
            commandTransform.GetComponent<Button>();

        if (commandButton == null)
        {
            Debug.LogError(
                "[BATTLEFIELD HUD] LEFT PANEL COMMAND BUTTON HAS NO BUTTON COMPONENT.");
            return;
        }

        Transform commandIntentTransform =
            FindChildRecursive(
                hud.transform,
                "[WINDOW] COMMAND INTENT");

        if (commandIntentTransform == null)
        {
            Debug.LogError(
                "[BATTLEFIELD HUD] COMMAND INTENT WINDOW NOT FOUND.");
            return;
        }

        GameObject commandIntentWindow =
            commandIntentTransform.gameObject;

        BattlefieldHUDController controller =
            hud.GetComponent<BattlefieldHUDController>();

        if (controller == null)
            controller = hud.AddComponent<BattlefieldHUDController>();

        SerializedObject serializedController =
            new SerializedObject(controller);

        serializedController.FindProperty("commandButton").objectReferenceValue =
            commandButton;

        serializedController.FindProperty("commandIntentWindow").objectReferenceValue =
            commandIntentWindow;

        serializedController.ApplyModifiedPropertiesWithoutUndo();

        commandIntentWindow.SetActive(false);

        EditorSceneManager.MarkSceneDirty(
            EditorSceneManager.GetActiveScene());

        EditorSceneManager.SaveScene(
            EditorSceneManager.GetActiveScene());

        Debug.Log(
            "[BATTLEFIELD HUD] COMMAND INTENT BUILD COMPLETE.");

        Debug.Log(
            "[BATTLEFIELD HUD] COMMAND BUTTON = " +
            commandTransform.name);

        Debug.Log(
            "[BATTLEFIELD HUD] COMMAND BUTTON PARENT = " +
            leftCommandPanel.name);

        Debug.Log(
            "[BATTLEFIELD HUD] WINDOW = " +
            commandIntentWindow.name);

        Debug.Log(
            "[BATTLEFIELD HUD] WINDOW START STATE = INACTIVE.");
    }

    private static GameObject FindExact(string objectName)
    {
        GameObject[] objects =
            Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in objects)
        {
            if (obj.scene.IsValid() &&
                obj.name == objectName)
            {
                return obj;
            }
        }

        return null;
    }

    private static Transform FindChildRecursive(
        Transform parent,
        string objectName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == objectName)
                return child;

            Transform result =
                FindChildRecursive(
                    child,
                    objectName);

            if (result != null)
                return result;
        }

        return null;
    }
}
