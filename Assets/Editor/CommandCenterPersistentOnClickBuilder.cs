using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class CommandCenterPersistentOnClickBuilder
{
    [MenuItem("Obsidian Protocol/Command Center/FILL PERSISTENT ONCLICKS")]
    public static void Build()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name != "Command_Center")
        {
            Debug.LogError("[COMMAND CENTER] Open Command_Center first.");
            return;
        }

        GameObject root = GameObject.Find("COMMAND_CENTER_FULL_INTERACTION");

        if (root == null)
        {
            Debug.LogError("[COMMAND CENTER] COMMAND_CENTER_FULL_INTERACTION not found.");
            return;
        }

        CommandCenterFullController controller =
            root.GetComponent<CommandCenterFullController>();

        if (controller == null)
        {
            controller = root.AddComponent<CommandCenterFullController>();
        }

        Connect(root, "COMMAND", controller.OpenCommand);
        Connect(root, "OPERATIONS", controller.OpenOperations);
        Connect(root, "FLEET", controller.OpenFleet);
        Connect(root, "DEPLOY", controller.OpenDeploy);
        Connect(root, "LOGISTICS", controller.OpenLogistics);
        Connect(root, "RESEARCH", controller.OpenResearch);
        Connect(root, "GARAGE", controller.OpenGarage);
        Connect(root, "FINANCE", controller.OpenFinance);
        Connect(root, "INTELLIGENCE", controller.OpenIntelligence);
        Connect(root, "STORE", controller.OpenStore);

        EditorUtility.SetDirty(root);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[COMMAND CENTER] PERSISTENT ONCLICKS FILLED USING CommandCenterFullController.");
    }

    private static void Connect(
        GameObject root,
        string buttonName,
        UnityEngine.Events.UnityAction action)
    {
        Transform t = root.transform.Find("CC_TEST_" + buttonName);

        if (t == null)
        {
            Debug.LogError("[COMMAND CENTER] BUTTON MISSING: CC_TEST_" + buttonName);
            return;
        }

        Button button = t.GetComponent<Button>();

        if (button == null)
        {
            Debug.LogError("[COMMAND CENTER] NO BUTTON COMPONENT: " + t.name);
            return;
        }

        button.onClick.RemoveAllListeners();

        UnityEventTools.AddPersistentListener(
            button.onClick,
            action
        );

        EditorUtility.SetDirty(button);

        Debug.Log(
            "[COMMAND CENTER] " +
            t.name +
            " -> " +
            action.Method.Name
        );
    }
}
