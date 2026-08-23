#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class ObsidianHUDUnitControlHierarchy
{
    [MenuItem("Obsidian Protocol/UI/Build HUD Unit Control Hierarchy")]
    public static void Build()
    {
        GameObject hud = GameObject.Find("HUD");

        if (hud == null)
        {
            hud = new GameObject("HUD");
        }

        Transform unitControlTransform = hud.transform.Find("UnitControl");

        GameObject unitControl;

        if (unitControlTransform == null)
        {
            unitControl = new GameObject("UnitControl");
            unitControl.transform.SetParent(hud.transform, false);
        }
        else
        {
            unitControl = unitControlTransform.gameObject;
        }

        CreateComponent<HUDSingleUnitSelection>(unitControl);
        CreateComponent<HUDMultiUnitSelection>(unitControl);
        CreateComponent<HUDSquadSelection>(unitControl);
        CreateComponent<HUDSelectionBox>(unitControl);
        CreateComponent<HUDSelectionRing>(unitControl);
        CreateComponent<HUDUnitIdentification>(unitControl);
        CreateComponent<HUDUnitStatus>(unitControl);
        CreateComponent<HUDUnitHealth>(unitControl);
        CreateComponent<HUDUnitArmor>(unitControl);
        CreateComponent<HUDUnitEnergy>(unitControl);
        CreateComponent<HUDUnitAmmunition>(unitControl);
        CreateComponent<HUDUnitFuel>(unitControl);
        CreateComponent<HUDUnitCapabilityStatus>(unitControl);

        Selection.activeGameObject = unitControl;

        EditorUtility.SetDirty(unitControl);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            unitControl.scene
        );

        Debug.Log("[Obsidian Protocol] HUD Unit Control hierarchy created successfully.");
    }

    private static void CreateComponent<T>(GameObject parent) where T : Component
    {
        if (parent.GetComponent<T>() == null)
            parent.AddComponent<T>();
    }
}
#endif
