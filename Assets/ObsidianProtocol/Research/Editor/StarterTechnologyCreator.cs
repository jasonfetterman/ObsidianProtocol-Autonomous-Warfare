using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Technology;

public static class StarterTechnologyCreator
{
    [MenuItem("Obsidian Protocol/Research/Create Starter Technology")]
    public static void Create()
    {
        const string folder =
            "Assets/Data/Technology";

        const string path =
            folder + "/TECH-001_Basic_Autonomy.asset";

        TechnologyDefinition existing =
            AssetDatabase.LoadAssetAtPath<TechnologyDefinition>(path);

        if (existing != null)
        {
            Selection.activeObject = existing;
            Debug.Log("[RESEARCH] TECH-001 already exists.");
            return;
        }

        TechnologyDefinition technology =
            ScriptableObject.CreateInstance<TechnologyDefinition>();

        AssetDatabase.CreateAsset(
            technology,
            path);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Selection.activeObject = technology;

        Debug.Log(
            "[RESEARCH] Created TECH-001 Basic Autonomy.");
    }
}
