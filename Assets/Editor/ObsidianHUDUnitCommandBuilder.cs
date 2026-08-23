#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class ObsidianHUDUnitCommandBuilder
{
    [MenuItem("Obsidian Protocol/UI/Build HUD-046 to HUD-058")]
    public static void Build()
    {
        GameObject hud = GameObject.Find("HUD");

        if (hud == null)
        {
            Debug.LogError("[Obsidian HUD] GameObject 'HUD' was not found.");
            return;
        }

        Transform battlefield = GetOrCreate("Battlefield", hud.transform);
        Transform commandPanel = GetOrCreate(
            "UnitCommandPanel",
            battlefield
        );

        AddComponent<HUDUnitCommandPanel>(commandPanel.gameObject);
        AddComponent<HUDMove>(commandPanel.gameObject);
        AddComponent<HUDAttack>(commandPanel.gameObject);
        AddComponent<HUDStop>(commandPanel.gameObject);
        AddComponent<HUDHold>(commandPanel.gameObject);
        AddComponent<HUDPatrol>(commandPanel.gameObject);
        AddComponent<HUDFollow>(commandPanel.gameObject);
        AddComponent<HUDEscort>(commandPanel.gameObject);
        AddComponent<HUDRetreat>(commandPanel.gameObject);
        AddComponent<HUDRepair>(commandPanel.gameObject);
        AddComponent<HUDResupply>(commandPanel.gameObject);
        AddComponent<HUDRecon>(commandPanel.gameObject);
        AddComponent<HUDAbilityControls>(commandPanel.gameObject);

        Selection.activeGameObject = commandPanel.gameObject;

        EditorUtility.SetDirty(hud);
        EditorUtility.SetDirty(commandPanel.gameObject);

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            hud.scene
        );

        Debug.Log(
            "[Obsidian HUD] HUD-046 through HUD-058 built successfully."
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

    private static void AddComponent<T>(GameObject target)
        where T : Component
    {
        if (target.GetComponent<T>() == null)
            target.AddComponent<T>();
    }
}
#endif
