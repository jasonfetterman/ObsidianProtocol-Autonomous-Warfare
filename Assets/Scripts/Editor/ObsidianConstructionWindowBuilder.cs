#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class ObsidianConstructionWindowBuilder
{
    [MenuItem("Obsidian Protocol/UI/Build WIN-047 to WIN-057")]
    public static void Build()
    {
        GameObject hud = GameObject.Find("HUD");

        if (hud == null)
        {
            Debug.LogError("[CONSTRUCTION WINDOW] HUD GameObject not found.");
            return;
        }

        Transform windows = GetOrCreate(
            "Windows",
            hud.transform
        );

        Transform construction = GetOrCreate(
            "Construction",
            windows
        );

        Transform core = GetOrCreate(
            "Core",
            construction
        );

        Transform catalog = GetOrCreate(
            "Catalog",
            construction
        );

        Transform placement = GetOrCreate(
            "Placement",
            construction
        );

        Transform progress = GetOrCreate(
            "Progress",
            construction
        );

        Transform state = GetOrCreate(
            "State",
            construction
        );

        // CORE
        CreateNode("WIN-047 ConstructionMenu", core);

        // CATALOG
        CreateNode("WIN-048 StructureCategories", catalog);
        CreateNode("WIN-049 StructurePreview", catalog);
        CreateNode("WIN-050 StructureCost", catalog);

        // PLACEMENT
        CreateNode("WIN-051 PlacementPreview", placement);
        CreateNode("WIN-052 ValidPlacement", placement);
        CreateNode("WIN-053 InvalidPlacement", placement);
        CreateNode("WIN-054 Rotation", placement);

        // PROGRESS
        CreateNode("WIN-055 ConstructionProgress", progress);
        CreateNode("WIN-056 CancelConstruction", progress);

        // STATE
        CreateNode("WIN-057 ConstructionComplete", state);

        EditorUtility.SetDirty(hud);

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            hud.scene
        );

        Selection.activeGameObject = construction.gameObject;

        Debug.Log(
            "[CONSTRUCTION WINDOW] WIN-047 through WIN-057 built successfully."
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
