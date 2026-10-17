using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Reflection;

public static class WireMainMenuCampaign
{
    private const string MainMenuManagerGuid =
        "6e99cb7ba6915454a9b23bb3e9355dc3";

    [MenuItem("Obsidian Protocol/Build/01 - Wire Main Menu Campaign")]
    public static void Build()
    {
        string[] guids = AssetDatabase.FindAssets(
            "t:Scene",
            new[] { "Assets/Scenes/SCN01_MAIN_MENU" }
        );

        if (guids.Length == 0)
        {
            Debug.LogError(
                "[WireMainMenuCampaign] Main Menu scene not found."
            );
            return;
        }

        string scenePath = AssetDatabase.GUIDToAssetPath(guids[0]);

        Scene scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        GameObject campaignButtonObject =
            FindObject(scene, "CAMPAIGN");

        if (campaignButtonObject == null)
        {
            Debug.LogError(
                "[WireMainMenuCampaign] CAMPAIGN GameObject not found."
            );
            return;
        }

        Button campaignButton =
            campaignButtonObject.GetComponent<Button>();

        if (campaignButton == null)
        {
            Debug.LogError(
                "[WireMainMenuCampaign] CAMPAIGN has no Button component."
            );
            return;
        }

        MonoScript managerScript =
            AssetDatabase.LoadAssetAtPath<MonoScript>(
                AssetDatabase.GUIDToAssetPath(MainMenuManagerGuid)
            );

        if (managerScript == null)
        {
            Debug.LogError(
                "[WireMainMenuCampaign] MainMenuManager script not found."
            );
            return;
        }

        System.Type managerType = managerScript.GetClass();

        if (managerType == null)
        {
            Debug.LogError(
                "[WireMainMenuCampaign] Could not resolve MainMenuManager type."
            );
            return;
        }

        GameObject systemsObject =
            GameObject.Find("GAME_SYSTEMS");

        if (systemsObject == null)
        {
            systemsObject = new GameObject("GAME_SYSTEMS");

            GameObject world =
                GameObject.Find("MAIN_MENU_WORLD");

            if (world != null)
            {
                systemsObject.transform.SetParent(
                    world.transform,
                    false
                );
            }
        }

        Component manager =
            systemsObject.GetComponent(managerType);

        if (manager == null)
            manager = systemsObject.AddComponent(managerType);

        /*
         * Remove EVERY serialized/persistent CAMPAIGN listener.
         * This specifically removes the old:
         *
         * MissingGameObject.OpenVR
         */
        SerializedObject serializedButton =
            new SerializedObject(campaignButton);

        SerializedProperty onClick =
            serializedButton.FindProperty("m_OnClick");

        SerializedProperty calls =
            onClick.FindPropertyRelative(
                "m_PersistentCalls.m_Calls"
            );

        calls.arraySize = 0;

        serializedButton.ApplyModifiedPropertiesWithoutUndo();

        /*
         * Add exactly one persistent listener:
         *
         * MainMenuManager.Campaign()
         */
        MethodInfo campaignMethod =
            managerType.GetMethod(
                "Campaign",
                BindingFlags.Instance |
                BindingFlags.Public
            );

        if (campaignMethod == null)
        {
            Debug.LogError(
                "[WireMainMenuCampaign] MainMenuManager.Campaign() not found."
            );
            return;
        }

        UnityEventTools.AddPersistentListener(
            campaignButton.onClick,
            ((MainMenuManager)manager).Campaign
        );

        EditorUtility.SetDirty(campaignButtonObject);
        EditorUtility.SetDirty(manager);

        EditorSceneManager.MarkSceneDirty(scene);

        if (!EditorSceneManager.SaveScene(scene))
        {
            Debug.LogError(
                "[WireMainMenuCampaign] Failed to save Main Menu scene."
            );
            return;
        }

        Debug.Log(
            "[WireMainMenuCampaign] SUCCESS: " +
            "CAMPAIGN now has exactly one persistent listener: " +
            "MainMenuManager.Campaign()"
        );
    }

    private static GameObject FindObject(
        Scene scene,
        string objectName
    )
    {
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            GameObject result =
                FindRecursive(root, objectName);

            if (result != null)
                return result;
        }

        return null;
    }

    private static GameObject FindRecursive(
        GameObject current,
        string objectName
    )
    {
        if (current.name == objectName)
            return current;

        foreach (Transform child in current.transform)
        {
            GameObject result =
                FindRecursive(
                    child.gameObject,
                    objectName
                );

            if (result != null)
                return result;
        }

        return null;
    }
}
