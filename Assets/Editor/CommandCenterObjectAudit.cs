using UnityEditor;
using UnityEngine;

public static class CommandCenterObjectAudit
{
    [MenuItem("Obsidian Protocol/Command Center/AUDIT RUNTIME OBJECTS")]
    public static void Audit()
    {
        var objects = Object.FindObjectsByType<GameObject>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        foreach (var obj in objects)
        {
            if (obj.name.Contains("COMMAND_CENTER"))
            {
                Debug.Log(
                    "[COMMAND CENTER OBJECT] " +
                    obj.name +
                    " | " +
                    obj.GetComponents<Component>().Length +
                    " components"
                );

                foreach (var component in obj.GetComponents<Component>())
                {
                    if (component != null)
                        Debug.Log(
                            "[COMMAND CENTER COMPONENT] " +
                            obj.name +
                            " -> " +
                            component.GetType().FullName
                        );
                }
            }
        }
    }
}

