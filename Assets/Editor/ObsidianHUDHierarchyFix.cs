#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class ObsidianHUDHierarchyFix
{
    [MenuItem("Obsidian Protocol/UI/FIX HUD HIERARCHY")]
    public static void FixHierarchy()
    {
        GameObject hud = GameObject.Find("HUD");

        if (hud == null)
        {
            Debug.LogError("[HUD FIX] Could not find GameObject named 'HUD'.");
            return;
        }

        Transform top = GetOrCreate("Top", hud.transform);
        Transform battlefield = GetOrCreate("Battlefield", hud.transform);

        Transform selection = GetOrCreate("Selection", battlefield);
        Transform units = GetOrCreate("Units", battlefield);
        Transform objectives = GetOrCreate("Objectives", battlefield);
        Transform tactical = GetOrCreate("Tactical", battlefield);
        Transform unitControl = GetOrCreate("UnitControl", battlefield);
        Transform commandPanel = GetOrCreate("UnitCommandPanel", battlefield);

        // ============================================================
        // TOP HUD
        // ============================================================

        Move("HUDTopResourceBar", top);
        Move("HUDResourceMeat", top);
        Move("HUDResourceWood", top);
        Move("HUDResourceCoal", top);
        Move("HUDResourceIron", top);
        Move("HUDResourceAlloy", top);
        Move("HUDResourceElectronics", top);
        Move("HUDResourceFuel", top);
        Move("HUDResourceEnergy", top);
        Move("HUDPopulationForceIndicator", top);
        Move("HUDCommandIntentAvailability", top);
        Move("HUDMatchTimer", top);
        Move("HUDGameStateIndicator", top);
        Move("HUDConnectionIndicator", top);

        // ============================================================
        // BATTLEFIELD - SELECTION
        // ============================================================

        Move("HUDSelectionIndicator", selection);
        Move("HUDSelectionBox", selection);
        Move("HUDSelectionRing", selection);

        // ============================================================
        // BATTLEFIELD - UNITS
        // ============================================================

        Move("HUDUnitHealthBars", units);
        Move("HUDUnitStatusIndicators", units);
        Move("HUDSquadIndicators", units);
        Move("HUDEnemyIndicators", units);
        Move("HUDFriendlyIndicators", units);

        // ============================================================
        // BATTLEFIELD - OBJECTIVES
        // ============================================================

        Move("HUDObjectiveMarkers", objectives);
        Move("HUDWaypointMarkers", objectives);
        Move("HUDAttackMarkers", objectives);
        Move("HUDDefenseMarkers", objectives);
        Move("HUDMoveMarkers", objectives);
        Move("HUDReconMarkers", objectives);

        // ============================================================
        // BATTLEFIELD - TACTICAL
        // ============================================================

        Move("HUDTacticalIntentMarkers", tactical);
        Move("HUDThreatMarkers", tactical);
        Move("HUDSuppressionIndicators", tactical);
        Move("HUDDamageIndicators", tactical);
        Move("HUDDestructionIndicators", tactical);
        Move("HUDBuildingStructureIndicators", tactical);

        // ============================================================
        // BATTLEFIELD - UNIT CONTROL
        // ============================================================

        Move("HUDSingleUnitSelection", unitControl);
        Move("HUDMultiUnitSelection", unitControl);
        Move("HUDSquadSelection", unitControl);
        Move("HUDUnitIdentification", unitControl);
        Move("HUDUnitStatus", unitControl);
        Move("HUDUnitHealth", unitControl);
        Move("HUDUnitArmor", unitControl);
        Move("HUDUnitEnergy", unitControl);
        Move("HUDUnitAmmunition", unitControl);
        Move("HUDUnitFuel", unitControl);
        Move("HUDUnitCapabilityStatus", unitControl);

        // ============================================================
        // BATTLEFIELD - UNIT COMMAND PANEL
        // ============================================================

        Move("HUDUnitCommandPanel", commandPanel);
        Move("HUDMove", commandPanel);
        Move("HUDAttack", commandPanel);
        Move("HUDStop", commandPanel);
        Move("HUDHold", commandPanel);
        Move("HUDPatrol", commandPanel);
        Move("HUDFollow", commandPanel);
        Move("HUDEscort", commandPanel);
        Move("HUDRetreat", commandPanel);
        Move("HUDRepair", commandPanel);
        Move("HUDResupply", commandPanel);
        Move("HUDRecon", commandPanel);
        Move("HUDAbilityControls", commandPanel);

        // ============================================================
        // FORCE ORDER
        // ============================================================

        top.SetSiblingIndex(0);
        battlefield.SetSiblingIndex(1);

        selection.SetSiblingIndex(0);
        units.SetSiblingIndex(1);
        objectives.SetSiblingIndex(2);
        tactical.SetSiblingIndex(3);
        unitControl.SetSiblingIndex(4);
        commandPanel.SetSiblingIndex(5);

        Selection.activeGameObject = hud;

        EditorUtility.SetDirty(hud);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(hud.scene);

        Debug.Log("[HUD FIX] =======================================");
        Debug.Log("[HUD FIX] HUD hierarchy fixed successfully.");
        Debug.Log("[HUD FIX] Top");
        Debug.Log("[HUD FIX] Battlefield");
        Debug.Log("[HUD FIX]   Selection");
        Debug.Log("[HUD FIX]   Units");
        Debug.Log("[HUD FIX]   Objectives");
        Debug.Log("[HUD FIX]   Tactical");
        Debug.Log("[HUD FIX]   UnitControl");
        Debug.Log("[HUD FIX]   UnitCommandPanel");
        Debug.Log("[HUD FIX] =======================================");
    }

    private static Transform GetOrCreate(string objectName, Transform parent)
    {
        Transform existing = FindDirectChild(objectName, parent);

        if (existing != null)
            return existing;

        GameObject obj = new GameObject(objectName);
        obj.transform.SetParent(parent, false);

        return obj.transform;
    }

    private static Transform FindDirectChild(string objectName, Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);

            if (child.name == objectName)
                return child;
        }

        return null;
    }

    private static GameObject FindAnywhere(string objectName)
    {
        GameObject[] objects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in objects)
        {
            if (obj.name != objectName)
                continue;

            if (EditorUtility.IsPersistent(obj))
                continue;

            if (obj.scene.IsValid())
                return obj;
        }

        return null;
    }

    private static void Move(string objectName, Transform destination)
    {
        GameObject obj = FindAnywhere(objectName);

        if (obj == null)
        {
            Debug.LogWarning("[HUD FIX] Object not found: " + objectName);
            return;
        }

        obj.transform.SetParent(destination, false);
    }
}
#endif
