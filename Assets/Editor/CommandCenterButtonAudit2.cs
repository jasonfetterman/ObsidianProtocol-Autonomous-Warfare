using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class CommandCenterButtonAudit2
{
    [MenuItem("Obsidian Protocol/Command Center/AUDIT CONTROLLER")]
    public static void Audit()
    {
        CommandCenterFullController controller =
            Object.FindFirstObjectByType<CommandCenterFullController>();

        if (controller == null)
        {
            Debug.LogError("[COMMAND CENTER] CONTROLLER NOT FOUND");
            return;
        }

        Debug.Log("[COMMAND CENTER] CONTROLLER OBJECT: " + controller.gameObject.name);

        Button[] buttons =
            controller.GetComponentsInChildren<Button>(true);

        foreach (Button button in buttons)
            Debug.Log("[COMMAND CENTER OWNED BUTTON] " + button.gameObject.name);

        Debug.Log("[COMMAND CENTER] OWNED BUTTON COUNT: " + buttons.Length);
    }
}
