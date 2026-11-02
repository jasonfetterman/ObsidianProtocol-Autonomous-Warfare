using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class CommandCenterButtonAudit
{
    [MenuItem("Obsidian Protocol/Command Center/LIST BUTTONS")]
    public static void ListButtons()
    {
        Button[] buttons = Object.FindObjectsByType<Button>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        Debug.Log(
            "[COMMAND CENTER AUDIT] Found " +
            buttons.Length + " buttons."
        );

        foreach (Button button in buttons)
        {
            Debug.Log(
                "[COMMAND CENTER BUTTON] " +
                button.gameObject.name +
                " | ActiveInHierarchy: " +
                button.gameObject.activeInHierarchy +
                " | PersistentListeners: " +
                button.onClick.GetPersistentEventCount(),
                button.gameObject
            );

            for (
                int i = 0;
                i < button.onClick.GetPersistentEventCount();
                i++
            )
            {
                Object target =
                    button.onClick.GetPersistentTarget(i);

                string method =
                    button.onClick.GetPersistentMethodName(i);

                Debug.Log(
                    "[BUTTON EVENT] " +
                    button.gameObject.name +
                    " | Target: " +
                    (target != null
                        ? target.name
                        : "MISSING TARGET") +
                    " | Method: " +
                    (!string.IsNullOrEmpty(method)
                        ? method
                        : "MISSING METHOD"),
                    button.gameObject
                );
            }
        }
    }
}
