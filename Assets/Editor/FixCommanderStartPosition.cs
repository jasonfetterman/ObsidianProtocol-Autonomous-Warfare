using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class FixCommanderStartPosition
{
    private const string ScenePath =
        "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

    [MenuItem("Obsidian Protocol/Deployment/Fix COMMANDER START POSITION")]
    public static void Fix()
    {
        Scene scene =
            EditorSceneManager.OpenScene(
                ScenePath,
                OpenSceneMode.Single);

        GameObject player =
            GameObject.Find("[PLAYER] COMMANDER");

        if (player == null)
        {
            Debug.LogError(
                "[COMMANDER START] PLAYER NOT FOUND.");
            return;
        }

        Vector3 position =
            player.transform.position;

        Debug.Log(
            "[COMMANDER START] OLD POSITION = " +
            position);

        position.y = 3.0f;

        player.transform.position = position;

        EditorUtility.SetDirty(player);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[COMMANDER START] NEW POSITION = " +
            player.transform.position);

        Debug.Log(
            "[COMMANDER START] CAPSULE BOTTOM = Y 3.0");

        Debug.Log(
            "[COMMANDER START] COMPLETE.");
    }
}
