using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ObsidianProtocol.UI;

public static class ObsidianMainMenuFunctionalPatch
{
    private const string SceneName = "SCN-01_MainMenu";

    [MenuItem("Tools/Obsidian Protocol/Main Menu/MAKE MAIN MENU FUNCTIONAL")]
    public static void Patch()
    {
        var scene = EditorSceneManager.GetActiveScene();

        if (!scene.IsValid() || string.IsNullOrEmpty(scene.path))
        {
            EditorUtility.DisplayDialog(
                "Obsidian Protocol",
                "Open SCN-01_MainMenu first.",
                "OK");
            return;
        }

        CreateBackup(scene.path);

        AddAllScenesToBuildSettings();

        var eventSystem = UnityEngine.Object.FindFirstObjectByType<EventSystem>();

        if (eventSystem == null)
        {
            var go = new GameObject("EventSystem");
            Undo.RegisterCreatedObjectUndo(go, "Create EventSystem");
            go.AddComponent<EventSystem>();

            Type inputModule = Type.GetType(
                "UnityEngine.InputSystem.UI.InputSystemUIInputModule, Unity.InputSystem");

            if (inputModule != null)
                go.AddComponent(inputModule);
            else
                go.AddComponent<StandaloneInputModule>();
        }

        var controllerObject = GameObject.Find("MainMenuController");

        if (controllerObject == null)
            controllerObject = new GameObject("MainMenuController");

        var controller =
            controllerObject.GetComponent<ObsidianMainMenuController>();

        if (controller == null)
            controllerObject.AddComponent<ObsidianMainMenuController>();

        Wire("BUTTON_Continue", "Continue");
        Wire("BUTTON_Campaign", "Campaign");
        Wire("BUTTON_Multiplayer", "Multiplayer");
        Wire("BUTTON_Garage", "Garage");
        Wire("BUTTON_Store", "Store");
        Wire("BUTTON_VROperator", "VROperator");
        Wire("BUTTON_Settings", "Settings");
        Wire("BUTTON_Credits", "Credits");
        Wire("BUTTON_Exit", "Exit");

        WireChild("POPUP_ExitConfirmation",
            "BUTTON_ConfirmExit",
            "ConfirmExit");

        WireChild("POPUP_ExitConfirmation",
            "BUTTON_CancelExit",
            "CancelExit");

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        EditorUtility.DisplayDialog(
            "Obsidian Protocol",
            "MAIN MENU FUNCTIONAL PATCH COMPLETE.\n\n" +
            "Scene routing has been configured.\n" +
            "Build Settings were updated.\n" +
            "Existing Main Menu was preserved.\n\n" +
            "Now press Play and test every button.",
            "OK");
    }

    private static void Wire(string objectName, string method)
    {
        var go = GameObject.Find(objectName);

        if (go == null)
        {
            Debug.LogWarning(
                "[Obsidian Protocol] Missing button: " + objectName);
            return;
        }

        var button = go.GetComponent<Button>();

        if (button == null)
        {
            Debug.LogWarning(
                "[Obsidian Protocol] No Button component: " + objectName);
            return;
        }

        button.onClick.RemoveAllListeners();

        var controller = GameObject.Find("MainMenuController")
            .GetComponent<ObsidianMainMenuController>();

        var info = typeof(ObsidianMainMenuController)
            .GetMethod(method);

        if (info == null)
        {
            Debug.LogError(
                "[Obsidian Protocol] Missing controller method: " + method);
            return;
        }

        button.onClick.AddListener(
            () => info.Invoke(controller, null));

        EditorUtility.SetDirty(button);
    }

    private static void WireChild(
        string parentName,
        string childName,
        string method)
    {
        var parent = GameObject.Find(parentName);

        if (parent == null)
            return;

        var child = parent.transform.Find("PANEL/" + childName);

        if (child == null)
            child = parent.transform.Find(childName);

        if (child == null)
            return;

        var button = child.GetComponent<Button>();

        if (button == null)
            return;

        button.onClick.RemoveAllListeners();

        var controller = GameObject.Find("MainMenuController")
            .GetComponent<ObsidianMainMenuController>();

        var info = typeof(ObsidianMainMenuController)
            .GetMethod(method);

        if (info == null)
            return;

        button.onClick.AddListener(
            () => info.Invoke(controller, null));

        EditorUtility.SetDirty(button);
    }

    private static void AddAllScenesToBuildSettings()
    {
        var sceneGuids = AssetDatabase.FindAssets("t:Scene");

        var paths = new System.Collections.Generic.List<string>();

        foreach (var guid in sceneGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            if (path.StartsWith("Assets/Scenes/",
                StringComparison.OrdinalIgnoreCase))
            {
                paths.Add(path);
            }
        }

        paths.Sort(StringComparer.OrdinalIgnoreCase);

        var existing = new System.Collections.Generic.HashSet<string>(
            EditorBuildSettings.scenes.Length > 0
                ? Array.ConvertAll(
                    EditorBuildSettings.scenes,
                    s => s.path)
                : new string[0],
            StringComparer.OrdinalIgnoreCase);

        var result =
            new System.Collections.Generic.List<EditorBuildSettingsScene>(
                EditorBuildSettings.scenes);

        foreach (var path in paths)
        {
            if (!existing.Contains(path))
            {
                result.Add(
                    new EditorBuildSettingsScene(path, true));

                existing.Add(path);

                Debug.Log(
                    "[Obsidian Protocol] Added Build Scene: " + path);
            }
        }

        EditorBuildSettings.scenes = result.ToArray();
    }

    private static void CreateBackup(string scenePath)
    {
        string directory = Path.GetDirectoryName(scenePath);
        string file = Path.GetFileNameWithoutExtension(scenePath);

        string backup = Path.Combine(
            directory,
            file + "_BACKUP_" +
            DateTime.Now.ToString("yyyyMMdd_HHmmss") +
            ".unity");

        File.Copy(scenePath, backup, false);
        AssetDatabase.Refresh();

        Debug.Log(
            "[Obsidian Protocol] Main Menu backup: " + backup);
    }
}
