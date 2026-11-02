using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using ObsidianProtocol.Game.Intelligence;

public static class WireUnknownPersistent
{
    [MenuItem("Obsidian Protocol/Fix/Wire UNKNOWN Persistent")]
    public static void Wire()
    {
        GameObject hud = GameObject.Find("INTELLIGENCE HUD");
        GameObject buttonObject = GameObject.Find("BUTTON - UNKNOWN");

        if (hud == null || buttonObject == null)
        {
            Debug.LogError("[INTELLIGENCE] UNKNOWN wiring target not found.");
            return;
        }

        IntelligenceContactFunctionController controller =
            hud.GetComponent<IntelligenceContactFunctionController>();

        if (controller == null)
            controller = hud.AddComponent<IntelligenceContactFunctionController>();

        Button button = buttonObject.GetComponent<Button>();

        if (button == null)
        {
            Debug.LogError("[INTELLIGENCE] BUTTON - UNKNOWN has no Button.");
            return;
        }

        button.onClick.RemoveAllListeners();
        UnityEventTools.AddPersistentListener(
            button.onClick,
            controller.ShowUnknown);

        EditorUtility.SetDirty(button);
        EditorUtility.SetDirty(controller);
        EditorSceneManager.MarkSceneDirty(button.gameObject.scene);
        EditorSceneManager.SaveScene(button.gameObject.scene);

        Debug.Log("[INTELLIGENCE] UNKNOWN persistent OnClick saved.");
    }
}
