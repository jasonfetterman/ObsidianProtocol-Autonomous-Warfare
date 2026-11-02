using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.Linq;

public static class OPAWFinanceBuildProfileFix
{
    private const string ScenePath =
        "Assets/Scenes/SCN-02  COMMAND CENTER/[HUD] COMMAND CENTER HUD/Command_Center.unity";

    [MenuItem("Obsidian Protocol/Build/ADD COMMAND CENTER TO BUILD PROFILE")]
    public static void AddCommandCenter()
    {
        string[] current = EditorBuildSettings.scenes
            .Select(s => s.path)
            .ToArray();

        if (!current.Contains(ScenePath))
        {
            var list = EditorBuildSettings.scenes.ToList();

            list.Add(new EditorBuildSettingsScene(ScenePath, true));

            EditorBuildSettings.scenes = list.ToArray();

            Debug.Log("[FINANCE] COMMAND CENTER added to active Build Profile.");
        }
        else
        {
            Debug.Log("[FINANCE] COMMAND CENTER is already in the active Build Profile.");
        }
    }
}
