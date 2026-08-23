#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class ObsidianWindows1617Builder
{
    [MenuItem("Obsidian Protocol/UI/Build PHASE 16-17 Windows")]
    public static void Build()
    {
        GameObject hud = GameObject.Find("HUD");

        if (hud == null)
        {
            Debug.LogError("[UI] HUD GameObject not found.");
            return;
        }

        Transform windows = GetOrCreate("Windows", hud.transform);

        // =====================================================
        // PHASE 16 — EQUIPMENT / LOADOUT
        // =====================================================

        Transform equipment = GetOrCreate("Equipment", windows);

        Transform equipmentCore =
            GetOrCreate("Core", equipment);

        Transform equipmentLoadout =
            GetOrCreate("Loadout", equipment);

        Transform equipmentSlots =
            GetOrCreate("Slots", equipment);

        Transform equipmentComparison =
            GetOrCreate("Comparison", equipment);

        Transform equipmentActions =
            GetOrCreate("Actions", equipment);

        CreateNode("WIN-089 EquipmentWindow", equipmentCore);

        CreateNode("WIN-090 Loadout", equipmentLoadout);
        CreateNode("WIN-099 ApplyLoadout", equipmentLoadout);

        CreateNode("WIN-091 WeaponSlots", equipmentSlots);
        CreateNode("WIN-092 ArmorSlots", equipmentSlots);
        CreateNode("WIN-093 SensorSlots", equipmentSlots);
        CreateNode("WIN-094 ModuleSlots", equipmentSlots);
        CreateNode("WIN-095 PayloadSlots", equipmentSlots);

        CreateNode("WIN-096 EquipmentComparison", equipmentComparison);
        CreateNode("WIN-097 EquipmentStatistics", equipmentComparison);

        CreateNode("WIN-098 EquipmentRestrictions", equipmentActions);
        CreateNode("WIN-100 SaveLoadout", equipmentActions);

        // =====================================================
        // PHASE 17 — ABILITY
        // =====================================================

        Transform ability = GetOrCreate("Ability", windows);

        Transform abilityCore =
            GetOrCreate("Core", ability);

        Transform abilityInformation =
            GetOrCreate("Information", ability);

        Transform abilityTargeting =
            GetOrCreate("Targeting", ability);

        Transform abilityState =
            GetOrCreate("State", ability);

        CreateNode("WIN-101 AbilityPanel", abilityCore);

        CreateNode("WIN-102 AbilityIcons", abilityInformation);
        CreateNode("WIN-103 AbilityDescription", abilityInformation);
        CreateNode("WIN-104 AbilityCooldown", abilityInformation);
        CreateNode("WIN-105 AbilityResourceCost", abilityInformation);

        CreateNode("WIN-106 AbilityRange", abilityTargeting);
        CreateNode("WIN-107 AbilityTargeting", abilityTargeting);

        CreateNode("WIN-108 AbilityUnavailableState", abilityState);

        // =====================================================
        // SAVE
        // =====================================================

        EditorUtility.SetDirty(hud);

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            hud.scene
        );

        Selection.activeGameObject = windows.gameObject;

        Debug.Log(
            "[UI] PHASE 16-17 hierarchy built successfully. WIN-089 through WIN-108."
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
