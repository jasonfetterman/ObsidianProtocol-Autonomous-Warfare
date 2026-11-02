using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Research;

public static class ResearchManagerSceneInstaller
{
    [MenuItem("Obsidian Protocol/Research/Install Research Manager")]
    public static void Install()
    {
        ResearchManager existing =
            Object.FindFirstObjectByType<ResearchManager>();

        if (existing != null)
        {
            Debug.Log("[RESEARCH] Research Manager already exists in scene.");
            Selection.activeGameObject = existing.gameObject;
            return;
        }

        GameObject manager =
            new GameObject("[SYSTEM] RESEARCH MANAGER");

        manager.AddComponent<ResearchManager>();

        Undo.RegisterCreatedObjectUndo(
            manager,
            "Create Research Manager");

        Selection.activeGameObject = manager;

        Debug.Log(
            "[RESEARCH] Research Manager installed.");
    }
}
