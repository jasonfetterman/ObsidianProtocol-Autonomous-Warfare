using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Deployment;

public static class CreateDeploymentRulesConfiguration
{
    [MenuItem("Obsidian Protocol/Deployment/Create Rules Configuration")]
    public static void Create()
    {
        const string path = "Assets/Resources/DeploymentRulesConfiguration.asset";

        DeploymentRulesConfiguration existing =
            AssetDatabase.LoadAssetAtPath<DeploymentRulesConfiguration>(path);

        if (existing != null)
        {
            Debug.Log("[DEPLOYMENT] RULES CONFIGURATION ALREADY EXISTS // " + path);
            Selection.activeObject = existing;
            EditorGUIUtility.PingObject(existing);
            return;
        }

        DeploymentRulesConfiguration configuration =
            ScriptableObject.CreateInstance<DeploymentRulesConfiguration>();

        AssetDatabase.CreateAsset(configuration, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Selection.activeObject = configuration;
        EditorGUIUtility.PingObject(configuration);

        Debug.Log("[DEPLOYMENT] RULES CONFIGURATION CREATED // " + path);
    }
}
