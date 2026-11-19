using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using ObsidianProtocol.Game.Deployment;

public static class BuildBattlefieldDeploymentMaterializer
{
    [MenuItem("Obsidian Protocol/Battlefield/BUILD DEPLOYMENT MATERIALIZER")]
    public static void Build()
    {
        const string scenePath =
            "Assets/Scenes/SCN03_BATTLEFIELD_AAA/SCN03_BATTLEFIELD_AAA.unity";

        var scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single);

        GameObject root =
            GameObject.Find("[SYSTEM] BATTLEFIELD DEPLOYMENT");

        if (root == null)
        {
            root = new GameObject(
                "[SYSTEM] BATTLEFIELD DEPLOYMENT");
        }

        BattlefieldDeploymentMaterializer materializer =
            root.GetComponent<BattlefieldDeploymentMaterializer>();

        if (materializer == null)
        {
            materializer =
                root.AddComponent<BattlefieldDeploymentMaterializer>();
        }

        BattlefieldReinforcementService reinforcementService =
            root.GetComponent<BattlefieldReinforcementService>();

        if (reinforcementService == null)
        {
            root.AddComponent<BattlefieldReinforcementService>();
        }

        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[BATTLEFIELD DEPLOYMENT] MATERIALIZER AND REINFORCEMENT SERVICE WIRED.");
    }
}
