using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using ObsidianProtocol.Game.PersistentMilitary;

public static class BuildSCN17PersistentMilitaryUI
{
    private const string ScenePath =
        "Assets/Scenes/SCN17_PERSISTENT_MILITARY/SCN17_PERSISTENT_MILITARY.unity";

    private const string HudName = "PERSISTENT MILITARY HUD";

    [MenuItem("Obsidian Protocol/SCN17/BUILD PERSISTENT MILITARY UI")]
    public static void Build()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.path != ScenePath)
        {
            Debug.LogError(
                "[SCN17] ABORTED: Active scene is not SCN17 Persistent Military.\n" +
                "Open: " + ScenePath);
            return;
        }

        GameObject hud = GameObject.Find(HudName);

        if (hud == null)
        {
            Debug.LogError(
                "[SCN17] ABORTED: Could not find exact HUD object: " +
                HudName);
            return;
        }

        SCN17PersistentMilitaryUI existing =
            hud.GetComponent<SCN17PersistentMilitaryUI>();

        if (existing != null)
        {
            Debug.Log(
                "[SCN17] UI controller already exists on " +
                HudName + ". No changes made.");
            return;
        }

        hud.AddComponent<SCN17PersistentMilitaryUI>();

        EditorUtility.SetDirty(hud);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[SCN17] BUILD COMPLETE: SCN17PersistentMilitaryUI added ONLY to " +
            HudName + " and scene saved.");
    }
}
