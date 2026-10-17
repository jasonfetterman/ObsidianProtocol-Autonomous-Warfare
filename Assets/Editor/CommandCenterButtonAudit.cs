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

        foreach (Button button in buttons)
            Debug.Log("[COMMAND CENTER BUTTON] " + button.gameObject.name);
    }
}
