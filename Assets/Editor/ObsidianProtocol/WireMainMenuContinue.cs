using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using ObsidianProtocol.UI;
using UnityEngine.SceneManagement;

public static class WireMainMenuContinue
{
    private const string SceneFolder =
        "Assets/Scenes/SCN01_MAIN_MENU/[HUD] MAIN MENU HUD";

    [MenuItem("Tools/Obsidian Protocol/Main Menu/Wire Continue Persistently")]
    public static void Wire()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            Debug.LogWarning("[Obsidian Protocol] Stop Play Mode before wiring Continue.");
            return;
        }

        string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", new[] { SceneFolder });
        string scenePath = null;

        foreach (string guid in sceneGuids)
        {
            string candidate = AssetDatabase.GUIDToAssetPath(guid);
            if (candidate.EndsWith(".unity", System.StringComparison.OrdinalIgnoreCase) &&
                System.IO.Path.GetFileName(candidate).Contains("MAIN MENU"))
            {
                scenePath = candidate;
                break;
            }
        }

        if (string.IsNullOrEmpty(scenePath))
        {
            Debug.LogError("[Obsidian Protocol] Could not locate the official MAIN MENU scene in: " + SceneFolder);
            return;
        }

        var scene = SceneManager.GetSceneByPath(scenePath);
        if (!scene.IsValid() || !scene.isLoaded)
            scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

        GameObject continueObject = null;

        foreach (GameObject candidate in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (candidate.name == "CONTINUE" &&
                candidate.scene == scene &&
                candidate.hideFlags == HideFlags.None)
            {
                continueObject = candidate;
                break;
            }
        }
        if (continueObject == null)
        {
            Debug.LogError("[Obsidian Protocol] CONTINUE object not found in official main menu.");
            return;
        }

        Button button = continueObject.GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("[Obsidian Protocol] CONTINUE has no Button component.");
            return;
        }

        ObsidianMainMenuController controller = null;

        foreach (ObsidianMainMenuController candidate in
            Resources.FindObjectsOfTypeAll<ObsidianMainMenuController>())
        {
            if (candidate != null &&
                candidate.gameObject.scene == scene &&
                candidate.hideFlags == HideFlags.None)
            {
                controller = candidate;
                break;
            }
        }

        if (controller == null)
        {
            Debug.LogError("[Obsidian Protocol] ObsidianMainMenuController not found.");
            return;
        }

        UnityAction callback = controller.Continue;
        UnityEventTools.RemovePersistentListener(button.onClick, callback);
        UnityEventTools.AddPersistentListener(button.onClick, callback);

        EditorUtility.SetDirty(continueObject);
        EditorSceneManager.MarkSceneDirty(scene);

        if (!EditorSceneManager.SaveScene(scene))
        {
            Debug.LogError("[Obsidian Protocol] Failed to save official main menu scene.");
            return;
        }

        Debug.Log("[Obsidian Protocol] CONTINUE PERSISTENTLY WIRED to ObsidianMainMenuController.Continue().");
    }
}



