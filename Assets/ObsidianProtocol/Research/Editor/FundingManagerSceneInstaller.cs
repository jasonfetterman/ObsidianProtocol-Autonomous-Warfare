#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Economy;

public static class FundingManagerSceneInstaller
{
    [MenuItem("Obsidian Protocol/Economy/Install Funding Manager")]
    public static void Install()
    {
        FundingManager existing =
            Object.FindFirstObjectByType<FundingManager>();

        if (existing != null)
        {
            Selection.activeGameObject = existing.gameObject;
            Debug.Log("[FUNDING] Funding Manager already exists.");
            return;
        }

        GameObject manager =
            new GameObject("[SYSTEM] FUNDING MANAGER");

        manager.AddComponent<FundingManager>();

        Undo.RegisterCreatedObjectUndo(
            manager,
            "Install Funding Manager");

        Selection.activeGameObject = manager;

        Debug.Log("[FUNDING] Funding Manager installed.");
    }
}
#endif
