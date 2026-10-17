using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using ObsidianProtocol.Game.UI;

[InitializeOnLoad]
public static class WireMainMenuContinue
{
    private const string SceneName = "SCN-01 MAIN MENU";

    static WireMainMenuContinue()
    {
        EditorApplication.delayCall += Wire;
    }

    private static void Wire()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
            return;

        var scene = EditorSceneManager.GetActiveScene();

        if (!scene.IsValid() || scene.name != SceneName)
            return;

        GameObject continueObject = GameObject.Find("Continue");

        if (continueObject == null)
        {
            Debug.LogError("[Obsidian Protocol] Continue GameObject not found.");
            return;
        }

        Button button = continueObject.GetComponent<Button>();

        if (button == null)
        {
            Debug.LogError("[Obsidian Protocol] Continue has no Button component.");
            return;
        }

        MainMenuContinueAction action =
            continueObject.GetComponent<MainMenuContinueAction>();

        if (action == null)
            action = continueObject.AddComponent<MainMenuContinueAction>();

        button.onClick.RemoveListener(action.ResumeGameplay);
        button.onClick.AddListener(action.ResumeGameplay);

        EditorUtility.SetDirty(continueObject);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[Obsidian Protocol] CONTINUE WIRED: Continue -> MainMenuContinueAction.ResumeGameplay()");
    }
}
