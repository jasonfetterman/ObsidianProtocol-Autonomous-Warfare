#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class ObsidianMinimapHUDBuilder
{
    [MenuItem("Obsidian Protocol/UI/Build MAP-001 to MAP-015")]
    public static void Build()
    {
        GameObject hud = GameObject.Find("HUD");

        if (hud == null)
        {
            Debug.LogError("[MINIMAP] HUD GameObject not found.");
            return;
        }

        Transform battlefield = GetOrCreate(
            "Battlefield",
            hud.transform
        );

        Transform minimap = GetOrCreate(
            "Minimap",
            battlefield
        );

        Transform core = GetOrCreate(
            "Core",
            minimap
        );

        Transform entities = GetOrCreate(
            "Entities",
            minimap
        );

        Transform markers = GetOrCreate(
            "Markers",
            minimap
        );

        Transform controls = GetOrCreate(
            "Controls",
            minimap
        );

        // CORE
        CreateNode("MAP-001 MinimapFrame", core);
        CreateNode("MAP-002 Terrain", core);
        CreateNode("MAP-012 CameraPosition", core);

        // ENTITIES
        CreateNode("MAP-003 FriendlyUnits", entities);
        CreateNode("MAP-004 EnemyUnits", entities);
        CreateNode("MAP-005 UnknownContacts", entities);
        CreateNode("MAP-006 SquadMarkers", entities);
        CreateNode("MAP-008 ResourceLocations", entities);
        CreateNode("MAP-009 Structures", entities);

        // MARKERS
        CreateNode("MAP-007 ObjectiveMarkers", markers);
        CreateNode("MAP-010 TacticalMarkers", markers);
        CreateNode("MAP-011 Waypoints", markers);

        // CONTROLS
        CreateNode("MAP-013 CameraDirection", controls);
        CreateNode("MAP-014 Zoom", controls);
        CreateNode("MAP-015 MinimapFilters", controls);

        EditorUtility.SetDirty(hud);

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            hud.scene
        );

        Selection.activeGameObject = minimap.gameObject;

        Debug.Log(
            "[MINIMAP] MAP-001 through MAP-015 built successfully."
        );
    }

    private static Transform GetOrCreate(
        string objectName,
        Transform parent
    )
    {
        Transform existing = parent.Find(objectName);

        if (existing != null)
            return existing;

        GameObject obj = new GameObject(objectName);
        obj.transform.SetParent(parent, false);

        return obj.transform;
    }

    private static void CreateNode(
        string objectName,
        Transform parent
    )
    {
        if (parent.Find(objectName) != null)
            return;

        GameObject obj = new GameObject(objectName);
        obj.transform.SetParent(parent, false);
    }
}
#endif
