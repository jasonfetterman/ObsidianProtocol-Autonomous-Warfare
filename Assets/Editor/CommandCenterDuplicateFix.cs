using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CommandCenterDuplicateFix
{
    [MenuItem("Obsidian Protocol/Command Center/REMOVE DUPLICATE FULL CONTROLLER")]
    public static void Remove()
    {
        GameObject obj = GameObject.Find("COMMAND_CENTER_FULL_INTERACTION");

        if (obj == null)
        {
            Debug.LogError("[COMMAND CENTER] FULL INTERACTION OBJECT NOT FOUND");
            return;
        }

        CommandCenterFullController[] controllers =
            obj.GetComponents<CommandCenterFullController>();

        Debug.Log("[COMMAND CENTER] CONTROLLERS FOUND: " + controllers.Length);

        for (int i = 1; i < controllers.Length; i++)
        {
            Object.DestroyImmediate(controllers[i]);
            Debug.Log("[COMMAND CENTER] DUPLICATE CONTROLLER REMOVED");
        }

        EditorUtility.SetDirty(obj);

        Scene scene =
            UnityEngine.SceneManagement.SceneManager.GetActiveScene();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[COMMAND CENTER] CONTROLLER CLEANUP COMPLETE");
    }
}

