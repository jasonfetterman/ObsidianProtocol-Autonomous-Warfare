using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class CopyCommandCloseToOperations
{
    [MenuItem("Obsidian Protocol/Command Center/COPY COMMAND CLOSE TO OPERATIONS")]
    public static void Build()
    {
        GameObject commandPanel = FindObject("COMMAND_PANEL");
        GameObject operationsPanel = FindObject("OPERATIONS_PANEL");

        if (commandPanel == null)
        {
            Debug.LogError("[COMMAND CENTER] COMMAND_PANEL not found.");
            return;
        }

        if (operationsPanel == null)
        {
            Debug.LogError("[COMMAND CENTER] OPERATIONS_PANEL not found.");
            return;
        }

        Transform source = commandPanel.transform.Find("CLOSE");

        if (source == null)
        {
            Debug.LogError("[COMMAND CENTER] COMMAND_PANEL/CLOSE not found.");
            return;
        }

        // Remove existing Operations CLOSE so the copy is exact.
        Transform existing = operationsPanel.transform.Find("CLOSE");

        if (existing != null)
        {
            Object.DestroyImmediate(existing.gameObject);
        }

        // Duplicate the exact Command Panel CLOSE button.
        GameObject copy = Object.Instantiate(
            source.gameObject,
            operationsPanel.transform
        );

        copy.name = "CLOSE";

        // Preserve the exact RectTransform values from the Command Panel.
        RectTransform sourceRect =
            source.GetComponent<RectTransform>();

        RectTransform copyRect =
            copy.GetComponent<RectTransform>();

        if (sourceRect != null && copyRect != null)
        {
            copyRect.anchorMin = sourceRect.anchorMin;
            copyRect.anchorMax = sourceRect.anchorMax;
            copyRect.anchoredPosition = sourceRect.anchoredPosition;
            copyRect.sizeDelta = sourceRect.sizeDelta;
            copyRect.pivot = sourceRect.pivot;
            copyRect.offsetMin = sourceRect.offsetMin;
            copyRect.offsetMax = sourceRect.offsetMax;
        }

        // Remove existing persistent listeners.
        Button button = copy.GetComponent<Button>();

        if (button != null)
        {
            button.onClick.RemoveAllListeners();

            GameObject controllerObject =
                FindObject("COMMAND_CENTER_FULL_INTERACTION");

            if (controllerObject != null)
            {
                CommandCenterFullController controller =
                    controllerObject.GetComponent<CommandCenterFullController>();

                if (controller != null)
                {
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(
                        button.onClick,
                        controller.CloseAll
                    );

                    Debug.Log(
                        "[OPERATIONS] CLOSE copied from COMMAND_PANEL and wired to CloseAll()."
                    );
                }
                else
                {
                    Debug.LogError(
                        "[COMMAND CENTER] CommandCenterFullController missing."
                    );
                }
            }
            else
            {
                Debug.LogError(
                    "[COMMAND CENTER] COMMAND_CENTER_FULL_INTERACTION not found."
                );
            }
        }
        else
        {
            Debug.LogError(
                "[OPERATIONS] Copied CLOSE has no Button component."
            );
        }

        EditorUtility.SetDirty(operationsPanel);

        EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );

        EditorSceneManager.SaveScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );

        Debug.Log(
            "[COMMAND CENTER] OPERATIONS CLOSE NOW MATCHES COMMAND CLOSE."
        );
    }

    private static GameObject FindObject(string name)
    {
        GameObject[] objects =
            Object.FindObjectsByType<GameObject>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

        foreach (GameObject obj in objects)
        {
            if (obj.name == name)
                return obj;
        }

        return null;
    }
}
