using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class CommandCenterAttachController
{
    [MenuItem("Obsidian Protocol/Command Center/ATTACH FULL CONTROLLER")]
    public static void Attach()
    {
        GameObject[] objects =
            Object.FindObjectsByType<GameObject>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

        foreach (GameObject obj in objects)
        {
            if (obj.name != "COMMAND_CENTER_FULL_INTERACTION")
                continue;

            CommandCenterFullController controller =
                obj.GetComponent<CommandCenterFullController>();

            if (controller == null)
            {
                controller =
                    obj.AddComponent<CommandCenterFullController>();

                Debug.Log(
                    "[COMMAND CENTER] CommandCenterFullController ATTACHED"
                );
            }
            else
            {
                Debug.Log(
                    "[COMMAND CENTER] CommandCenterFullController ALREADY EXISTS"
                );
            }

            EditorUtility.SetDirty(obj);

            EditorSceneManager.MarkSceneDirty(
                obj.scene
            );

            EditorSceneManager.SaveScene(
                obj.scene
            );

            Selection.activeGameObject = obj;

            return;
        }

        Debug.LogError(
            "[COMMAND CENTER] COMMAND_CENTER_FULL_INTERACTION NOT FOUND"
        );
    }
}
