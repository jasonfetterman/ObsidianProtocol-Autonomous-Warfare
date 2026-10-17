using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class RestoreCommanderStartPosition
{
    private const string ScenePath =
        "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

    [MenuItem("Obsidian Protocol/Deployment/RESTORE COMMANDER START")]
    public static void Restore()
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
                "[COMMANDER RESTORE] PLAYER NOT FOUND.");
            return;
        }

        player.transform.position =
            new Vector3(
                0f,
                10.20f,
                -17.40f);

        EditorUtility.SetDirty(player);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[COMMANDER RESTORE] POSITION = " +
            player.transform.position);

        Debug.Log(
            "[COMMANDER RESTORE] COMPLETE.");
    }
}
