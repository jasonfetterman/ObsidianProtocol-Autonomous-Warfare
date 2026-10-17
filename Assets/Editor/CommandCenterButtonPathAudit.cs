using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class CommandCenterButtonPathAudit
{
    [MenuItem("Obsidian Protocol/Command Center/AUDIT BUTTON PATHS")]
    public static void Audit()
    {
        CommandCenterFullController controller =
            Object.FindFirstObjectByType<CommandCenterFullController>();

        if (controller == null)
        {
            Debug.LogError("[COMMAND CENTER] CONTROLLER NOT FOUND");
            return;
        }

        Button[] buttons =
            controller.GetComponentsInChildren<Button>(true);

        foreach (Button button in buttons)
        {
            string path = button.transform.name;
            Transform parent = button.transform.parent;

            while (parent != null && parent != controller.transform)
            {
                path = parent.name + "/" + path;
                parent = parent.parent;
            }

            Debug.Log("[COMMAND CENTER BUTTON PATH] " + path);
        }

        Debug.Log("[COMMAND CENTER] BUTTON COUNT: " + buttons.Length);
    }
}
