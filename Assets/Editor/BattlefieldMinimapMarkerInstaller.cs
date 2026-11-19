using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using ObsidianProtocol.Game.Deployment;

public static class BattlefieldMinimapMarkerInstaller
{
    [MenuItem("Obsidian Protocol/Battlefield/INSTALL FRIENDLY AND ENEMY MINIMAP MARKERS")]
    public static void Install()
    {
        GameObject overlay = GameObject.Find("Minimap_MarkerOverlay");

        if (overlay == null)
        {
            Debug.LogError("[MINIMAP MARKERS] Minimap_MarkerOverlay not found.");
            return;
        }

        var controller =
            overlay.GetComponent<BattlefieldMinimapMarkerController>();

        if (controller == null)
            Undo.AddComponent<BattlefieldMinimapMarkerController>(overlay);

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());

        Debug.Log("[MINIMAP MARKERS] INSTALLED ON EXISTING OVERLAY // BLUE=FRIENDLY // RED=ENEMY");
    }
}
