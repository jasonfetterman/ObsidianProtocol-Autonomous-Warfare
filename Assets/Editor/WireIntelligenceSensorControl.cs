using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.Events;

public static class WireIntelligenceSensorControl
{
    [MenuItem("Obsidian Protocol/Fix/Wire Intelligence Sensor Control")]
    public static void Wire()
    {
        GameObject buttonObject = GameObject.Find("BUTTON - SENSOR CONTROL");

        if (buttonObject == null)
        {
            Debug.LogError("[INTELLIGENCE] BUTTON - SENSOR CONTROL not found.");
            return;
        }

        Button button = buttonObject.GetComponent<Button>();

        if (button == null)
        {
            Debug.LogError("[INTELLIGENCE] Button component missing.");
            return;
        }

        ObsidianProtocol.Game.Intelligence.IntelligenceSensorControl control =
            buttonObject.GetComponent<ObsidianProtocol.Game.Intelligence.IntelligenceSensorControl>();

        if (control == null)
            control = buttonObject.AddComponent<ObsidianProtocol.Game.Intelligence.IntelligenceSensorControl>();

        button.onClick.RemoveAllListeners();
        UnityEventTools.AddPersistentListener(button.onClick, control.ToggleSensors);

        EditorUtility.SetDirty(buttonObject);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(buttonObject.scene);
        UnityEditor.SceneManagement.EditorSceneManager.SaveScene(buttonObject.scene);

        Debug.Log("[INTELLIGENCE] SENSOR CONTROL button wired to ToggleSensors().");
    }
}