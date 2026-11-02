using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class SCN15WirePersistentButtons
{
    [MenuItem("Obsidian Protocol/SCN-15/WIRE EXIT BUTTONS")]
    public static void Wire()
    {
        string scenePath =
            "Assets/Scenes/SCN15  EXIT CONFIRMATION/[HUD] EXIT CONFIRMATION HUD/Exit_Confirmation.unity";

        var scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        GameObject popupObject =
            GameObject.Find("POP-024 EXIT CONFIRMATION");

        GameObject cancelObject =
            GameObject.Find("CANCEL");

        GameObject confirmObject =
            GameObject.Find("CONFIRM EXIT");

        if (popupObject == null)
        {
            Debug.LogError("[SCN-15 WIRE] POP-024 EXIT CONFIRMATION NOT FOUND.");
            return;
        }

        if (cancelObject == null)
        {
            Debug.LogError("[SCN-15 WIRE] CANCEL NOT FOUND.");
            return;
        }

        if (confirmObject == null)
        {
            Debug.LogError("[SCN-15 WIRE] CONFIRM EXIT NOT FOUND.");
            return;
        }

        var popup =
            popupObject.GetComponent<
                ObsidianProtocol.UI.Popups.System.POP024_ExitConfirmation
            >();

        if (popup == null)
        {
            Debug.LogError(
                "[SCN-15 WIRE] POP024_ExitConfirmation COMPONENT NOT FOUND."
            );
            return;
        }

        Button cancelButton = cancelObject.GetComponent<Button>();
        Button confirmButton = confirmObject.GetComponent<Button>();

        if (cancelButton == null || confirmButton == null)
        {
            Debug.LogError("[SCN-15 WIRE] BUTTON COMPONENT MISSING.");
            return;
        }

        Undo.RecordObject(cancelButton, "Wire SCN-15 Cancel");
        Undo.RecordObject(confirmButton, "Wire SCN-15 Confirm");

        cancelButton.onClick.RemoveAllListeners();
        confirmButton.onClick.RemoveAllListeners();

        UnityEventTools.AddPersistentListener(
            cancelButton.onClick,
            popup.CancelExit
        );

        UnityEventTools.AddPersistentListener(
            confirmButton.onClick,
            popup.ConfirmExit
        );

        EditorUtility.SetDirty(cancelButton);
        EditorUtility.SetDirty(confirmButton);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[SCN-15 WIRE] CANCEL -> POP024_ExitConfirmation.CancelExit()"
        );

        Debug.Log(
            "[SCN-15 WIRE] CONFIRM EXIT -> POP024_ExitConfirmation.ConfirmExit()"
        );

        Debug.Log("[SCN-15 WIRE] SCENE SAVED.");
    }
}
