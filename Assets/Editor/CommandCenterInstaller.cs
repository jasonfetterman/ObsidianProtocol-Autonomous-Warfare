using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class CommandCenterInstaller
{
    [MenuItem("Obsidian Protocol/Command Center/REMOVE DUPLICATE CONTROLLER")]
    public static void Remove()
    {
        GameObject obj = GameObject.Find("COMMAND_CENTER_CONTROLLER");

        if (obj != null)
        {
            Object.DestroyImmediate(obj);
            Debug.Log("[COMMAND CENTER] DUPLICATE CONTROLLER REMOVED");
        }
        else
        {
            Debug.Log("[COMMAND CENTER] NO DUPLICATE FOUND");
        }

        EditorSceneManager.SaveScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );
    }
}
