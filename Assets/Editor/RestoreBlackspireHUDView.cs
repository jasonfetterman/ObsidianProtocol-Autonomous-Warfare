using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class RestoreBlackspireHUDView
{
    [MenuItem("Obsidian Protocol/RECOVERY/RESTORE HUD VIEW")]
    public static void Restore()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            Debug.LogError("HUD recovery: Canvas not found.");
            return;
        }

        canvas.SetActive(true);

        GameObject hud = GameObject.Find("BattleHUD");
        if (hud != null) hud.SetActive(true);

        GameObject overlay = GameObject.Find("BattleHUD_Overlay");
        if (overlay != null) overlay.SetActive(true);

        Selection.activeGameObject = canvas;
        SceneView.lastActiveSceneView?.FrameSelected();
        SceneView.lastActiveSceneView?.Repaint();

        EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());

        Debug.Log("HUD RECOVERY: Canvas, BattleHUD and overlay activated. Scene view focused.");
    }
}
