using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Research;
using ObsidianProtocol.Game.Technology;

public static class ResearchSystemTest
{
    [MenuItem("Obsidian Protocol/Research/Test TECH-001")]
    public static void Test()
    {
        ResearchManager manager =
            Object.FindFirstObjectByType<ResearchManager>();

        TechnologyDefinition technology =
            AssetDatabase.LoadAssetAtPath<TechnologyDefinition>(
                "Assets/Data/Technology/TECH-001_Basic_Autonomy.asset");

        if (manager == null)
        {
            Debug.LogError("[RESEARCH TEST] Research Manager not found.");
            return;
        }

        if (technology == null)
        {
            Debug.LogError("[RESEARCH TEST] TECH-001 not found.");
            return;
        }

        bool started =
            manager.StartResearch(technology);

        Debug.Log(
            $"[RESEARCH TEST] StartResearch result: {started}");
    }
}
