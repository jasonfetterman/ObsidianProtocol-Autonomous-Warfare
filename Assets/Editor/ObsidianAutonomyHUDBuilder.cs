#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class ObsidianAutonomyHUDBuilder
{
    [MenuItem("Obsidian Protocol/UI/Build AUTO-001 to AUTO-020")]
    public static void Build()
    {
        GameObject hud = GameObject.Find("HUD");

        if (hud == null)
        {
            Debug.LogError("[AUTONOMY HUD] HUD GameObject not found.");
            return;
        }

        Transform battlefield = GetOrCreate(
            "Battlefield",
            hud.transform
        );

        Transform autonomy = GetOrCreate(
            "Autonomy",
            battlefield
        );

        Transform interfaceGroup = GetOrCreate(
            "Interface",
            autonomy
        );

        Transform configuration = GetOrCreate(
            "Configuration",
            autonomy
        );

        Transform behaviors = GetOrCreate(
            "Behaviors",
            autonomy
        );

        Transform status = GetOrCreate(
            "Status",
            autonomy
        );

        CreateNode("AUTO-001 IntentInterface", interfaceGroup);
        CreateNode("AUTO-002 IntentSelection", interfaceGroup);
        CreateNode("AUTO-003 ObjectiveSelection", interfaceGroup);

        CreateNode("AUTO-004 PrioritySelection", configuration);
        CreateNode("AUTO-005 RulesOfEngagement", configuration);
        CreateNode("AUTO-006 EngagementRange", configuration);
        CreateNode("AUTO-007 AggressionSetting", configuration);
        CreateNode("AUTO-008 FormationSetting", configuration);

        CreateNode("AUTO-009 MovementBehavior", behaviors);
        CreateNode("AUTO-010 FlankingIntent", behaviors);
        CreateNode("AUTO-011 SuppressionIntent", behaviors);
        CreateNode("AUTO-012 BreachIntent", behaviors);
        CreateNode("AUTO-013 DefensiveIntent", behaviors);
        CreateNode("AUTO-014 ReconIntent", behaviors);
        CreateNode("AUTO-015 PursuitIntent", behaviors);
        CreateNode("AUTO-016 RetreatIntent", behaviors);
        CreateNode("AUTO-017 ReinforcementIntent", behaviors);
        CreateNode("AUTO-018 SupportIntent", behaviors);

        CreateNode("AUTO-019 AutonomyStateIndicator", status);
        CreateNode("AUTO-020 AIDecisionStatusDisplay", status);

        autonomy.SetSiblingIndex(
            battlefield.childCount - 1
        );

        EditorUtility.SetDirty(hud);

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            hud.scene
        );

        Selection.activeGameObject = autonomy.gameObject;

        Debug.Log(
            "[AUTONOMY HUD] AUTO-001 through AUTO-020 built successfully."
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
