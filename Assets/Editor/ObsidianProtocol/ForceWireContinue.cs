using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public static class ForceWireContinue
{
    public static void Execute()
    {
        string[] scenes = AssetDatabase.FindAssets("t:Scene")
            .Select(AssetDatabase.GUIDToAssetPath)
            .Where(p => p.EndsWith("SCN-01 MAIN MENU.unity"))
            .ToArray();

        if (scenes.Length == 0)
        {
            Debug.LogError("SCN-01 MAIN MENU.unity NOT FOUND.");
            return;
        }

        string scenePath = scenes[0];

        var scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        GameObject continueObject = scene.GetRootGameObjects()
            .SelectMany(GetAllChildren)
            .FirstOrDefault(go => go.name == "Continue");

        if (continueObject == null)
        {
            Debug.LogError("Continue GameObject NOT FOUND.");
            return;
        }

        Button button = continueObject.GetComponent<Button>();

        if (button == null)
        {
            Debug.LogError("Continue has no Button component.");
            return;
        }

        var action = continueObject.GetComponent<
            ObsidianProtocol.Game.UI.MainMenuContinueAction>();

        if (action == null)
        {
            action = continueObject.AddComponent<
                ObsidianProtocol.Game.UI.MainMenuContinueAction>();
        }

        button.onClick.RemoveAllListeners();

        UnityEditor.Events.UnityEventTools.AddPersistentListener(
            button.onClick,
            action.ResumeGameplay
        );

        EditorUtility.SetDirty(continueObject);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("==============================================");
        Debug.Log("CONTINUE BUTTON SUCCESSFULLY WIRED");
        Debug.Log("Continue -> MainMenuContinueAction.ResumeGameplay()");
        Debug.Log("==============================================");

        EditorApplication.Exit(0);
    }

    private static System.Collections.Generic.IEnumerable<GameObject>
        GetAllChildren(GameObject root)
    {
        yield return root;

        foreach (Transform child in root.transform)
        {
            foreach (var go in GetAllChildren(child.gameObject))
                yield return go;
        }
    }
}
