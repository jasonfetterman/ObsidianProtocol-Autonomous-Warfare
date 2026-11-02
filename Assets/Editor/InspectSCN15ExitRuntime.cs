using UnityEditor;
using UnityEngine;
using ObsidianProtocol.UI.Popups.System;

public static class InspectSCN15ExitRuntime
{
    [MenuItem("Obsidian Protocol/Debug/Inspect SCN15 Exit Runtime")]
    public static void Inspect()
    {
        var obj = Object.FindAnyObjectByType<POP024_ExitConfirmation>();

        if (obj == null)
        {
            Debug.LogError("SCN15: POP024_ExitConfirmation NOT FOUND.");
            return;
        }

        Debug.Log("===== SCN15 EXIT RUNTIME =====");
        Debug.Log("Object: " + obj.gameObject.name);
        Debug.Log("Active: " + obj.gameObject.activeSelf);
        Debug.Log("Type: " + obj.GetType().FullName);

        var fields = obj.GetType().GetFields(
            System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.NonPublic
        );

        foreach (var f in fields)
        {
            bool serialized =
                f.IsPublic ||
                System.Attribute.IsDefined(
                    f,
                    typeof(UnityEngine.SerializeField),
                    true
                );

            if (!serialized)
                continue;

            object value = null;

            try
            {
                value = f.GetValue(obj);
            }
            catch
            {
                value = "<READ ERROR>";
            }

            Debug.Log($"FIELD: {f.Name} = {value}");
        }

        Debug.Log("===== END SCN15 EXIT RUNTIME =====");
    }
}
