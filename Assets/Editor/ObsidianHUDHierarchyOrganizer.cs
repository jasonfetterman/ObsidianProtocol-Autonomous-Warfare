#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class ObsidianHUDHierarchyOrganizer
{
    [MenuItem("Obsidian Protocol/UI/Organize HUD Hierarchy")]
    public static void OrganizeHUD()
    {
        GameObject hud = GameObject.Find("HUD");

        if (hud == null)
        {
            Debug.LogError("[HUD Organizer] Could not find a GameObject named 'HUD'.");
            return;
        }

        Transform top = GetOrCreate("Top", hud.transform);
        Transform battlefield = GetOrCreate("Battlefield", hud.transform);

        Transform selection = GetOrCreate("Selection", battlefield);
        Transform units = GetOrCreate("Units", battlefield);
        Transform objectives = GetOrCreate("Objectives", battlefield);
        Transform tactical = GetOrCreate("Tactical", battlefield);
        Transform unitControl = GetOrCreate("UnitControl", battlefield);

        MoveIfExists("HUDTopResourceBar", top);

        MoveIfExists("HUDResourceMeat", top);
        MoveIfExists("HUDResourceWood", top);
        MoveIfExists("HUDResourceCoal", top);
        MoveIfExists("HUDResourceIron", top);
        MoveIfExists("HUDResourceAlloy", top);
        MoveIfExists("HUDResourceElectronics", top);
        MoveIfExists("HUDResourceFuel", top);
        MoveIfExists("HUDResourceEnergy", top);
        MoveIfExists("HUDPopulationForceIndicator", top);
        MoveIfExists("HUDCommandIntentAvailability", top);
        MoveIfExists("HUDMatchTimer", top);
        MoveIfExists("HUDGameStateIndicator", top);
        MoveIfExists("HUDConnectionIndicator", top);

        MoveIfExists("HUDSelectionIndicator", selection);
        MoveIfExists("HUDSelectionBox", selection);
        MoveIfExists("HUDSelectionRing", selection);

        MoveIfExists("HUDUnitHealthBars", units);
        MoveIfExists("HUDUnitStatusIndicators", units);
        MoveIfExists("HUDSquadIndicators", units);
        MoveIfExists("HUDEnemyIndicators", units);
        MoveIfExists("HUDFriendlyIndicators", units);

        MoveIfExists("HUDObjectiveMarkers", objectives);
        MoveIfExists("HUDWaypointMarkers", objectives);
        MoveIfExists("HUDAttackMarkers", objectives);
        MoveIfExists("HUDDefenseMarkers", objectives);
        MoveIfExists("HUDMoveMarkers", objectives);
        MoveIfExists("HUDReconMarkers", objectives);

        MoveIfExists("HUDTacticalIntentMarkers", tactical);
        MoveIfExists("HUDThreatMarkers", tactical);
        MoveIfExists("HUDSuppressionIndicators", tactical);
        MoveIfExists("HUDDamageIndicators", tactical);
        MoveIfExists("HUDDestructionIndicators", tactical);
        MoveIfExists("HUDBuildingStructureIndicators", tactical);

        MoveIfExists("HUDSingleUnitSelection", unitControl);
        MoveIfExists("HUDMultiUnitSelection", unitControl);
        MoveIfExists("HUDSquadSelection", unitControl);
        MoveIfExists("HUDUnitIdentification", unitControl);
        MoveIfExists("HUDUnitStatus", unitControl);
        MoveIfExists("HUDUnitHealth", unitControl);
        MoveIfExists("HUDUnitArmor", unitControl);
        MoveIfExists("HUDUnitEnergy", unitControl);
        MoveIfExists("HUDUnitAmmunition", unitControl);
        MoveIfExists("HUDUnitFuel", unitControl);
        MoveIfExists("HUDUnitCapabilityStatus", unitControl);

        Selection.activeGameObject = hud;

        EditorUtility.SetDirty(hud);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(hud.scene);

        Debug.Log("[HUD Organizer] HUD hierarchy organized successfully.");
    }

    private static Transform GetOrCreate(string objectName, Transform parent)
    {
        Transform existing = parent.Find(objectName);

        if (existing != null)
            return existing;

        GameObject obj = new GameObject(objectName);
        obj.transform.SetParent(parent, false);

        return obj.transform;
    }

    private static void MoveIfExists(string objectName, Transform destination)
    {
        GameObject obj = GameObject.Find(objectName);

        if (obj == null)
            return;

        if (obj.transform == destination)
            return;

        obj.transform.SetParent(destination, false);
    }
}
#endif
