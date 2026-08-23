#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class ObsidianTacticalMapHUDBuilder
{
    [MenuItem("Obsidian Protocol/UI/Build MAP-016 to MAP-030")]
    public static void Build()
    {
        GameObject hud = GameObject.Find("HUD");

        if (hud == null)
        {
            Debug.LogError("[TACTICAL MAP] HUD GameObject not found.");
            return;
        }

        Transform battlefield = GetOrCreate(
            "Battlefield",
            hud.transform
        );

        Transform tacticalMap = GetOrCreate(
            "TacticalMap",
            battlefield
        );

        Transform core = GetOrCreate(
            "Core",
            tacticalMap
        );

        Transform filters = GetOrCreate(
            "Filters",
            tacticalMap
        );

        Transform intelligence = GetOrCreate(
            "Intelligence",
            tacticalMap
        );

        Transform overlays = GetOrCreate(
            "Overlays",
            tacticalMap
        );

        // CORE
        CreateNode("MAP-016 TacticalMapWindow", core);
        CreateNode("MAP-017 FullBattlefieldView", core);

        // FILTERS
        CreateNode("MAP-018 UnitFiltering", filters);
        CreateNode("MAP-019 SquadFiltering", filters);
        CreateNode("MAP-020 EnemyFiltering", filters);
        CreateNode("MAP-021 IntelligenceFiltering", filters);
        CreateNode("MAP-022 LogisticsFiltering", filters);
        CreateNode("MAP-023 ResourceFiltering", filters);
        CreateNode("MAP-024 ObjectiveFiltering", filters);

        // INTELLIGENCE
        CreateNode("MAP-025 ThreatOverlay", intelligence);
        CreateNode("MAP-026 SensorCoverage", intelligence);

        // OVERLAYS
        CreateNode("MAP-027 MovementRoutes", overlays);
        CreateNode("MAP-028 AttackRoutes", overlays);
        CreateNode("MAP-029 DefensiveZones", overlays);
        CreateNode("MAP-030 ReconZones", overlays);

        EditorUtility.SetDirty(hud);

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            hud.scene
        );

        Selection.activeGameObject = tacticalMap.gameObject;

        Debug.Log(
            "[TACTICAL MAP] MAP-016 through MAP-030 built successfully."
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
