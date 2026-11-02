using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class AuditIntelligenceRoot
{
    [MenuItem("Obsidian Protocol/Intelligence/AUDIT ROOT CHILDREN")]
    public static void Audit()
    {
        const string scenePath =
            "Assets/Scenes/SCN18_INTELLIGENCE/SCN18_INTELLIGENCE.unity";

        EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        GameObject hud = GameObject.Find("INTELLIGENCE HUD");

        if (hud == null)
        {
            Debug.LogError("[INTELLIGENCE ROOT AUDIT] HUD NOT FOUND.");
            return;
        }

        Debug.Log("[INTELLIGENCE ROOT AUDIT] DIRECT CHILDREN:");

        foreach (Transform child in hud.transform)
        {
            Debug.Log(
                "[INTELLIGENCE ROOT AUDIT] " +
                child.name +
                " | ACTIVE=" +
                child.gameObject.activeSelf
            );
        }

        Debug.Log("[INTELLIGENCE ROOT AUDIT] COMPLETE.");
    }
}
