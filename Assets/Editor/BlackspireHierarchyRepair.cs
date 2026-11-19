using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class BlackspireHierarchyRepair
{
    [MenuItem("Obsidian Protocol/RECOVERY/ORGANIZE BLACKSPIRE HIERARCHY")]
    public static void Organize()
    {
        var scene = SceneManager.GetActiveScene();
        var canvas = GameObject.Find("Canvas");
        var minimap = GameObject.Find("Minimap_Panel");

        if (canvas == null || minimap == null)
        {
            Debug.LogError("[HIERARCHY] Canvas or Minimap_Panel missing. Nothing changed.");
            return;
        }

        Undo.IncrementCurrentGroup();
        int group = Undo.GetCurrentGroup();

        GameObject roster = GameObject.Find("[HUD] UNIT ROSTER");
        if (roster == null)
        {
            roster = new GameObject("[HUD] UNIT ROSTER", typeof(RectTransform));
            Undo.RegisterCreatedObjectUndo(roster, "Create HUD Roster Group");
            roster.transform.SetParent(canvas.transform, false);
            var rt = roster.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        string[] rosterNames =
        {
            "UNIT_ROSTER_PAGING", "Units",
            "Selected_Units (1)", "Selected_Units (2)",
            "Selected_Units (3)", "Selected_Units (4)",
            "Selected_Units (5)"
        };

        foreach (string name in rosterNames)
        {
            var obj = GameObject.Find(name);
            if (obj != null && obj.transform.parent != roster.transform)
                Undo.SetTransformParent(obj.transform, roster.transform,
                    "Organize Unit Roster");
        }

        var marker = GameObject.Find("Minimap_MarkerOverlay");
        if (marker != null && marker.transform.parent != minimap.transform)
            Undo.SetTransformParent(marker.transform, minimap.transform,
                "Organize Minimap Overlay");

        GameObject systems = GameObject.Find("[SYSTEM] COMMAND AND SELECTION");
        if (systems == null)
        {
            systems = new GameObject("[SYSTEM] COMMAND AND SELECTION");
            Undo.RegisterCreatedObjectUndo(systems, "Create Command Systems Group");
        }

        foreach (string name in new[] { "CommandIntentManager", "UnitSelectionManager" })
        {
            var obj = GameObject.Find(name);
            if (obj != null && obj.transform.parent != systems.transform)
                Undo.SetTransformParent(obj.transform, systems.transform,
                    "Organize Command Systems");
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Undo.CollapseUndoOperations(group);

        Debug.Log("[HIERARCHY] ORGANIZED. Existing HUD objects and positions preserved.");
    }
}
