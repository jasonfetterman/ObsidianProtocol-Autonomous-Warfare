using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class FixBattlefieldCommandIntentClose
{
    private const string ScenePath =
        "Assets/Scenes/SCN03 _BATTLEFIELD/[HUD] BATTLEFIELD HUD/Battlefield_HUD.unity";

    [MenuItem("Obsidian Protocol/Battlefield/FIX COMMAND INTENT CLOSE")]
    public static void Fix()
    {
        EditorSceneManager.OpenScene(ScenePath);

        GameObject hud =
            GameObject.Find("[HUD] BATTLEFIELD HUD");

        if (hud == null)
        {
            Debug.LogError(
                "[COMMAND INTENT CLOSE] BATTLEFIELD HUD NOT FOUND.");
            return;
        }

        BattlefieldHUDController controller =
            hud.GetComponent<BattlefieldHUDController>();

        if (controller == null)
        {
            Debug.LogError(
                "[COMMAND INTENT CLOSE] BATTLEFIELD HUD CONTROLLER NOT FOUND.");
            return;
        }

        Transform window =
            FindChildRecursive(
                hud.transform,
                "[WINDOW] COMMAND INTENT");

        if (window == null)
        {
            Debug.LogError(
                "[COMMAND INTENT CLOSE] COMMAND INTENT WINDOW NOT FOUND.");
            return;
        }

        Transform closeTransform =
            FindChildRecursive(
                window,
                "[BUTTON] CLOSE COMMAND INTENT");

        if (closeTransform == null)
        {
            Debug.LogError(
                "[COMMAND INTENT CLOSE] CLOSE BUTTON NOT FOUND.");
            return;
        }

        Button closeButton =
            closeTransform.GetComponent<Button>();

        if (closeButton == null)
        {
            Debug.LogError(
                "[COMMAND INTENT CLOSE] CLOSE BUTTON HAS NO BUTTON COMPONENT.");
            return;
        }

        closeButton.onClick.RemoveAllListeners();

        UnityEventTools.AddPersistentListener(
            closeButton.onClick,
            controller.CloseCommandIntent);

        EditorUtility.SetDirty(closeButton);
        EditorUtility.SetDirty(controller);

        EditorSceneManager.MarkSceneDirty(
            EditorSceneManager.GetActiveScene());

        EditorSceneManager.SaveScene(
            EditorSceneManager.GetActiveScene());

        Debug.Log(
            "[COMMAND INTENT CLOSE] CLOSE BUTTON WIRED.");

        Debug.Log(
            "[COMMAND INTENT CLOSE] TARGET = " +
            controller.gameObject.name +
            " // METHOD = CloseCommandIntent");

        Debug.Log(
            "[COMMAND INTENT CLOSE] PERSISTENT LISTENERS = " +
            closeButton.onClick.GetPersistentEventCount());
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
