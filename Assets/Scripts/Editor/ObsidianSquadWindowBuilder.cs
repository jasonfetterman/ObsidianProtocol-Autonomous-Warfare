#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class ObsidianSquadWindowBuilder
{
    [MenuItem("Obsidian Protocol/UI/Build WIN-020 to WIN-034")]
    public static void Build()
    {
        GameObject hud = GameObject.Find("HUD");

        if (hud == null)
        {
            Debug.LogError("[SQUAD WINDOW] HUD GameObject not found.");
            return;
        }

        Transform windows = GetOrCreate(
            "Windows",
            hud.transform
        );

        Transform squad = GetOrCreate(
            "Squad",
            windows
        );

        Transform core = GetOrCreate(
            "Core",
            squad
        );

        Transform identity = GetOrCreate(
            "Identity",
            squad
        );

        Transform composition = GetOrCreate(
            "Composition",
            squad
        );

        Transform operations = GetOrCreate(
            "Operations",
            squad
        );

        Transform autonomy = GetOrCreate(
            "Autonomy",
            squad
        );

        // CORE
        CreateNode("WIN-020 SquadDetails", core);

        // IDENTITY
        CreateNode("WIN-021 SquadName", identity);
        CreateNode("WIN-023 SquadLeader", identity);

        // COMPOSITION
        CreateNode("WIN-022 SquadComposition", composition);
        CreateNode("WIN-024 SquadHealth", composition);
        CreateNode("WIN-025 SquadStrength", composition);
        CreateNode("WIN-032 SquadCasualties", composition);

        // OPERATIONS
        CreateNode("WIN-026 SquadFormation", operations);
        CreateNode("WIN-027 SquadObjective", operations);
        CreateNode("WIN-031 SquadOrders", operations);
        CreateNode("WIN-033 SquadReinforcement", operations);

        // AUTONOMY
        CreateNode("WIN-028 SquadIntent", autonomy);
        CreateNode("WIN-029 SquadPriority", autonomy);
        CreateNode("WIN-030 SquadBehavior", autonomy);
        CreateNode("WIN-034 SquadAutonomyState", autonomy);

        EditorUtility.SetDirty(hud);

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            hud.scene
        );

        Selection.activeGameObject = squad.gameObject;

        Debug.Log(
            "[SQUAD WINDOW] WIN-020 through WIN-034 built successfully."
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
