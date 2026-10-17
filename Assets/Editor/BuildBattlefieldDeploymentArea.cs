using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class BuildBattlefieldDeploymentArea
{
    [MenuItem("Obsidian Protocol/Battlefield/BUILD PLAYER DEPLOYMENT AREA")]
    public static void Build()
    {
        const string scenePath =
            "Assets/Scenes/SCN03 _BATTLEFIELD/[HUD] BATTLEFIELD HUD/Battlefield_HUD.unity";

        var scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single);

        GameObject existing =
            GameObject.Find("[SYSTEM] PLAYER DEPLOYMENT AREA");

        if (existing != null)
        {
            Debug.Log("[BATTLEFIELD DEPLOYMENT] AREA ALREADY EXISTS.");
            return;
        }

        GameObject area =
            new GameObject("[SYSTEM] PLAYER DEPLOYMENT AREA");

        area.transform.position = Vector3.zero;

        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[BATTLEFIELD DEPLOYMENT] PLAYER DEPLOYMENT AREA CREATED.");
    }
}
