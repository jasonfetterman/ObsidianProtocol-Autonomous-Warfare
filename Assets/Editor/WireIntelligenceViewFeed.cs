using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;

public static class WireIntelligenceViewFeed
{
    [MenuItem("Obsidian Protocol/Fix/Wire Intelligence View Feed")]
    public static void Wire()
    {
        GameObject buttonObject = GameObject.Find("BUTTON - VIEW FEED");

        if (buttonObject == null)
        {
            Debug.LogError("[INTELLIGENCE] BUTTON - VIEW FEED not found.");
            return;
        }

        Button button = buttonObject.GetComponent<Button>();

        if (button == null)
        {
            Debug.LogError("[INTELLIGENCE] VIEW FEED Button component missing.");
            return;
        }

        var control =
            buttonObject.GetComponent<ObsidianProtocol.Game.Intelligence.IntelligenceFeedControl>();

        if (control == null)
            control = buttonObject.AddComponent<ObsidianProtocol.Game.Intelligence.IntelligenceFeedControl>();

        button.onClick.RemoveAllListeners();
        UnityEventTools.AddPersistentListener(button.onClick, control.ToggleFeed);

        EditorUtility.SetDirty(buttonObject);
        EditorSceneManager.MarkSceneDirty(buttonObject.scene);
        EditorSceneManager.SaveScene(buttonObject.scene);

        Debug.Log("[INTELLIGENCE] VIEW FEED button wired to ToggleFeed().");
    }
}