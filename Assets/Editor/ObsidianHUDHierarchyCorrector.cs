#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class ObsidianHUDHierarchyCorrector
{
    [MenuItem("Obsidian Protocol/UI/FIX HUD SCENE HIERARCHY")]
    public static void FixHierarchy()
    {
        GameObject hud = GameObject.Find("HUD");

        if (hud == null)
        {
            Debug.LogError("[HUD HIERARCHY] No GameObject named 'HUD' found in the open scene.");
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
        // HUD-015 through HUD-037 selection-related objects
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
        // FORCE TOP-LEVEL ORDER
        // ============================================================

        top.SetSiblingIndex(0);
        battlefield.SetSiblingIndex(1);

        // ============================================================
        // FORCE BATTLEFIELD ORDER
        // ============================================================

        selection.SetSiblingIndex(0);
        units.SetSiblingIndex(1);
        objectives.SetSiblingIndex(2);
        tactical.SetSiblingIndex(3);
        unitControl.SetSiblingIndex(4);
        commandPanel.SetSiblingIndex(5);

        // ============================================================
        // SAVE
        // ============================================================

        EditorUtility.SetDirty(hud);

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            hud.scene
        );

        Selection.activeGameObject = hud;

        Debug.Log("");
        Debug.Log("================================================");
        Debug.Log("[HUD HIERARCHY] HUD-001 through HUD-058 ORGANIZED");
        Debug.Log("================================================");
        Debug.Log("HUD");
        Debug.Log("+-- Top");
        Debug.Log("+-- Battlefield");
        Debug.Log("    +-- Selection");
        Debug.Log("    +-- Units");
        Debug.Log("    +-- Objectives");
        Debug.Log("    +-- Tactical");
        Debug.Log("    +-- UnitControl");
        Debug.Log("    +-- UnitCommandPanel");
        Debug.Log("================================================");
    }

    private static Transform GetOrCreate(
        string objectName,
        Transform parent
    )
    {
        Transform existing = FindDirectChild(objectName, parent);

        if (existing != null)
            return existing;

        GameObject obj = new GameObject(objectName);
        obj.transform.SetParent(parent, false);

        Undo.RegisterCreatedObjectUndo(
            obj,
            "Create HUD Hierarchy Object"
        );

        return obj.transform;
    }

    private static Transform FindDirectChild(
        string objectName,
        Transform parent
    )
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);

            if (child.name == objectName)
                return child;
        }

        return null;
    }

    private static GameObject FindSceneObject(string objectName)
    {
        GameObject[] objects =
            Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in objects)
        {
            if (obj.name != objectName)
                continue;

            if (EditorUtility.IsPersistent(obj))
                continue;

            if (!obj.scene.IsValid())
                continue;

            return obj;
        }

        return null;
    }

    private static void Move(
        string objectName,
        Transform destination
    )
    {
        GameObject obj = FindSceneObject(objectName);

        if (obj == null)
        {
            Debug.LogWarning(
                "[HUD HIERARCHY] Object not found: " + objectName
            );

            return;
        }

        if (obj.transform == destination)
            return;

        Undo.SetTransformParent(
            obj.transform,
            destination,
            "Organize HUD Hierarchy"
        );
    }
}
#endif
