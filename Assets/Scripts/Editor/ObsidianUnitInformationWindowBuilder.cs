#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class ObsidianUnitInformationWindowBuilder
{
    [MenuItem("Obsidian Protocol/UI/Build WIN-001 to WIN-019")]
    public static void Build()
    {
        GameObject hud = GameObject.Find("HUD");

        if (hud == null)
        {
            Debug.LogError("[UNIT INFORMATION] HUD GameObject not found.");
            return;
        }

        Transform windows = GetOrCreate(
            "Windows",
            hud.transform
        );

        Transform unitInformation = GetOrCreate(
            "UnitInformation",
            windows
        );

        Transform core = GetOrCreate(
            "Core",
            unitInformation
        );

        Transform identity = GetOrCreate(
            "Identity",
            unitInformation
        );

        Transform systems = GetOrCreate(
            "Systems",
            unitInformation
        );

        Transform status = GetOrCreate(
            "Status",
            unitInformation
        );

        // CORE
        CreateNode("WIN-001 UnitDetailsWindow", core);
        CreateNode("WIN-002 UnitPortraitModel", core);

        // IDENTITY
        CreateNode("WIN-003 UnitName", identity);
        CreateNode("WIN-004 UnitType", identity);
        CreateNode("WIN-005 UnitFaction", identity);

        // SYSTEMS
        CreateNode("WIN-006 Health", systems);
        CreateNode("WIN-007 Armor", systems);
        CreateNode("WIN-008 Speed", systems);
        CreateNode("WIN-009 Fuel", systems);
        CreateNode("WIN-010 Energy", systems);
        CreateNode("WIN-011 Sensors", systems);
        CreateNode("WIN-012 Weapons", systems);
        CreateNode("WIN-013 Equipment", systems);
        CreateNode("WIN-014 Abilities", systems);

        // STATUS
        CreateNode("WIN-015 Status", status);
        CreateNode("WIN-016 CurrentOrders", status);
        CreateNode("WIN-017 CurrentIntent", status);
        CreateNode("WIN-018 AIState", status);
        CreateNode("WIN-019 SquadAssignment", status);

        EditorUtility.SetDirty(hud);

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            hud.scene
        );

        Selection.activeGameObject = unitInformation.gameObject;

        Debug.Log(
            "[UNIT INFORMATION] WIN-001 through WIN-019 built successfully."
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
