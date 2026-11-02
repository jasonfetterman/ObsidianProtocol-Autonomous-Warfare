using UnityEditor;
using UnityEngine;

public static class RotateBlackspireClownUnits180
{
    [MenuItem("Obsidian Protocol/World Building/Rotate Clown Units Another 90 Degrees")]
    public static void Run()
    {
        var root = GameObject.Find("CLOWN_SYNDICATE_FIELD_UNITS");
        if (!root)
        {
            Debug.LogError("[CLOWN ROTATE] Unit group not found.");
            return;
        }

        foreach (Transform unit in root.transform)
        {
            Undo.RecordObject(unit, "Rotate Clown Unit Another 90 Degrees");
            unit.Rotate(0f, 90f, 0f, Space.World);
            EditorUtility.SetDirty(unit);
            Debug.Log("[CLOWN ROTATE] " + unit.name +
                      " -> " + unit.eulerAngles.ToString("F1"));
        }

        Debug.Log("[CLOWN ROTATE] Another 90-degree turn complete. Review before saving.");
    }
}
