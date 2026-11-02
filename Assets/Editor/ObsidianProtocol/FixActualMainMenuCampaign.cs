using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using ObsidianProtocol.UI;

public static class FixActualMainMenuCampaign
{
    private const string TargetSceneFolder =
        "Assets/Scenes/SCN01_MAIN_MENU/";

    [MenuItem("Tools/Obsidian Protocol/Main Menu/FIX ACTUAL CAMPAIGN BUTTON")]
    public static void FixCampaignButton()
    {
        var scene = EditorSceneManager.GetActiveScene();

        if (!scene.IsValid() ||
            string.IsNullOrEmpty(scene.path) ||
            !scene.path.StartsWith(
                TargetSceneFolder,
                System.StringComparison.OrdinalIgnoreCase))
        {
            EditorUtility.DisplayDialog(
                "Obsidian Protocol",
                "Open the actual SCN01 Main Menu scene first.",
                "OK");
            return;
        }

        var centerMenu = GameObject.Find("CENTER_COMMAND_MENU");
        var campaignObject = GameObject.Find("CAMPAIGN");

        if (centerMenu == null || campaignObject == null)
        {
            Debug.LogError(
                "[Main Menu Fix] Required objects were not found. No changes made.");
            return;
        }

        var button = campaignObject.GetComponent<Button>();

        if (button == null)
        {
            Debug.LogError(
                "[Main Menu Fix] CAMPAIGN has no Button component. No changes made.");
            return;
        }

        var controller =
            centerMenu.GetComponent<ObsidianMainMenuController>();

        if (controller == null)
            controller = Undo.AddComponent<ObsidianMainMenuController>(centerMenu);

        button.onClick.RemoveListener(controller.Campaign);
        button.onClick.AddListener(controller.Campaign);

        EditorUtility.SetDirty(button);
        EditorUtility.SetDirty(centerMenu);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[Main Menu Fix] CAMPAIGN is wired to the controller on CENTER_COMMAND_MENU.");
        EditorUtility.DisplayDialog(
            "Obsidian Protocol",
            "Campaign button wiring saved. Test it in Play mode.",
            "OK");
    }
}
