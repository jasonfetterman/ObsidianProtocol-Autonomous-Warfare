#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class ObsidianHUDUnitControlBuilder
{
    [MenuItem("Obsidian Protocol/UI/Build HUD-033 to HUD-045")]
    public static void Build()
    {
        GameObject hud = GameObject.Find("HUD");

        if (hud == null)
        {
            Debug.LogError("[Obsidian HUD] GameObject 'HUD' was not found.");
            return;
        }

        Transform battlefield = GetOrCreate("Battlefield", hud.transform);
        Transform unitControl = GetOrCreate("UnitControl", battlefield);

        AddComponent<HUDSingleUnitSelection>(unitControl.gameObject);
        AddComponent<HUDMultiUnitSelection>(unitControl.gameObject);
        AddComponent<HUDSquadSelection>(unitControl.gameObject);
        AddComponent<HUDSelectionBox>(unitControl.gameObject);
        AddComponent<HUDSelectionRing>(unitControl.gameObject);
        AddComponent<HUDUnitIdentification>(unitControl.gameObject);
        AddComponent<HUDUnitStatus>(unitControl.gameObject);
        AddComponent<HUDUnitHealth>(unitControl.gameObject);
        AddComponent<HUDUnitArmor>(unitControl.gameObject);
        AddComponent<HUDUnitEnergy>(unitControl.gameObject);
        AddComponent<HUDUnitAmmunition>(unitControl.gameObject);
        AddComponent<HUDUnitFuel>(unitControl.gameObject);
        AddComponent<HUDUnitCapabilityStatus>(unitControl.gameObject);

        Selection.activeGameObject = unitControl.gameObject;

        EditorUtility.SetDirty(hud);
        EditorUtility.SetDirty(unitControl.gameObject);

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            hud.scene
        );

        Debug.Log("[Obsidian HUD] HUD-033 through HUD-045 built successfully.");
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

    private static void AddComponent<T>(GameObject target)
        where T : Component
    {
        if (target.GetComponent<T>() == null)
            target.AddComponent<T>();
    }
}
#endif
