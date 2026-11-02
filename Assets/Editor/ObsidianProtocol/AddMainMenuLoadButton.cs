using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using ObsidianProtocol.UI;

public static class AddMainMenuLoadButton
{
    private const string SceneFolder =
        "Assets/Scenes/SCN01_MAIN_MENU/[HUD] MAIN MENU HUD";

    [MenuItem("Tools/Obsidian Protocol/Main Menu/Add Load Button")]
    public static void AddLoadButton()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            Debug.LogWarning("[Obsidian Protocol] Stop Play Mode before editing the main menu.");
            return;
        }

        string scenePath = null;

        foreach (string guid in AssetDatabase.FindAssets("t:Scene", new[] { SceneFolder }))
        {
            string candidate = AssetDatabase.GUIDToAssetPath(guid);

            if (candidate.EndsWith(".unity", StringComparison.OrdinalIgnoreCase) &&
                System.IO.Path.GetFileName(candidate).Contains("MAIN MENU"))
            {
                scenePath = candidate;
                break;
            }
        }

        if (string.IsNullOrEmpty(scenePath))
        {
            Debug.LogError("[Obsidian Protocol] Official main menu scene was not found.");
            return;
        }

        Scene scene = SceneManager.GetSceneByPath(scenePath);
        if (!scene.IsValid() || !scene.isLoaded)
            scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

        GameObject campaign = FindSceneObject(scene, "CAMPAIGN");
        if (campaign == null)
        {
            Debug.LogError("[Obsidian Protocol] CAMPAIGN button was not found.");
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
            Debug.LogError("[Obsidian Protocol] Main menu controller was not found.");
            return;
        }

        GameObject load = FindSceneObject(scene, "LOAD");

        if (load == null)
        {
            load = UnityEngine.Object.Instantiate(campaign, campaign.transform.parent);
            load.name = "LOAD";
        }

        RectTransform rect = load.GetComponent<RectTransform>();
        Button button = load.GetComponent<Button>();

        if (rect == null || button == null)
        {
            Debug.LogError("[Obsidian Protocol] LOAD requires a RectTransform and Button.");
            return;
        }

        // Place LOAD directly below CAMPAIGN, preserving the existing button dimensions.
        rect.anchorMin = new Vector2(0.1f, 0.604f);
        rect.anchorMax = new Vector2(0.9f, 0.664f);
        rect.anchoredPosition = Vector2.zero;

        // Change cloned label text without requiring a specific text-rendering package.
        foreach (Component component in load.GetComponentsInChildren<Component>(true))
        {
            if (component == null)
                continue;

            PropertyInfo textProperty = component.GetType().GetProperty(
                "text",
                BindingFlags.Instance | BindingFlags.Public);

            if (textProperty != null &&
                textProperty.PropertyType == typeof(string) &&
                textProperty.CanWrite)
            {
                try
                {
                    textProperty.SetValue(component, "LOAD");
                }
                catch (Exception)
                {
                    // Ignore components whose text property is not writable at runtime.
                }
            }
        }

        UnityAction callback = controller.LoadCampaign;
        UnityEventTools.RemovePersistentListener(button.onClick, callback);
        UnityEventTools.AddPersistentListener(button.onClick, callback);

        EditorUtility.SetDirty(load);
        EditorSceneManager.MarkSceneDirty(scene);

        if (!EditorSceneManager.SaveScene(scene))
        {
            Debug.LogError("[Obsidian Protocol] Failed to save main menu scene.");
            return;
        }

        Debug.Log("[Obsidian Protocol] LOAD button saved and persistently connected to LoadCampaign().");
    }

    private static GameObject FindSceneObject(Scene scene, string objectName)
    {
        foreach (GameObject candidate in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (candidate != null &&
                candidate.name == objectName &&
                candidate.scene == scene &&
                candidate.hideFlags == HideFlags.None)
            {
                return candidate;
            }
        }

        return null;
    }
}
