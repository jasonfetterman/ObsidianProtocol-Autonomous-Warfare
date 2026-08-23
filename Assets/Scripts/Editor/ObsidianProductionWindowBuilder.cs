#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class ObsidianProductionWindowBuilder
{
    [MenuItem("Obsidian Protocol/UI/Build WIN-035 to WIN-046")]
    public static void Build()
    {
        GameObject hud = GameObject.Find("HUD");

        if (hud == null)
        {
            Debug.LogError("[PRODUCTION WINDOW] HUD GameObject not found.");
            return;
        }

        Transform windows = GetOrCreate(
            "Windows",
            hud.transform
        );

        Transform production = GetOrCreate(
            "Production",
            windows
        );

        Transform core = GetOrCreate(
            "Core",
            production
        );

        Transform catalog = GetOrCreate(
            "Catalog",
            production
        );

        Transform costs = GetOrCreate(
            "Costs",
            production
        );

        Transform queue = GetOrCreate(
            "Queue",
            production
        );

        Transform state = GetOrCreate(
            "State",
            production
        );

        // CORE
        CreateNode("WIN-035 ProductionWindow", core);

        // CATALOG
        CreateNode("WIN-036 AvailableUnits", catalog);
        CreateNode("WIN-037 UnitCategories", catalog);
        CreateNode("WIN-038 UnitPreview", catalog);

        // COSTS
        CreateNode("WIN-039 UnitCost", costs);
        CreateNode("WIN-040 RequiredResources", costs);
        CreateNode("WIN-041 ProductionTime", costs);

        // QUEUE
        CreateNode("WIN-042 Queue", queue);
        CreateNode("WIN-043 QueueControls", queue);
        CreateNode("WIN-044 CancelProduction", queue);

        // STATE
        CreateNode("WIN-045 ProductionComplete", state);
        CreateNode("WIN-046 ProductionUnavailableState", state);

        EditorUtility.SetDirty(hud);

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            hud.scene
        );

        Selection.activeGameObject = production.gameObject;

        Debug.Log(
            "[PRODUCTION WINDOW] WIN-035 through WIN-046 built successfully."
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
