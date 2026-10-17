using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class AuditDeploymentPlayer
{
    [MenuItem("Obsidian Protocol/Deployment/AUDIT PLAYER")]
    public static void Audit()
    {
        EditorSceneManager.OpenScene(
            "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity",
            OpenSceneMode.Single);

        foreach (GameObject obj in Object.FindObjectsByType<GameObject>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None))
        {
            if (obj.GetComponent<CharacterController>() != null ||
                obj.name.ToLower().Contains("player") ||
                obj.name.ToLower().Contains("character") ||
                obj.name.ToLower().Contains("firstperson"))
            {
                Debug.Log("[DEPLOYMENT PLAYER] " + obj.name);
            }
        }
    }
}
