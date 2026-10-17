using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class AcheronWireHUD
{
    [MenuItem("Obsidian Protocol/Acheron/WIRE HUD BUTTONS")]
    public static void Wire()
    {
        GameObject hud = GameObject.Find("ACHERON_FINAL_HUD");

        if (hud == null)
        {
            Debug.LogError("[ACHERON] ACHERON_FINAL_HUD not found.");
            return;
        }

        AcheronFinalHUDController controller =
            hud.GetComponent<AcheronFinalHUDController>();

        if (controller == null)
            hud.AddComponent<AcheronFinalHUDController>();

        EditorSceneManager.MarkSceneDirty(
            hud.scene
        );

        EditorSceneManager.SaveScene(
            hud.scene
        );

        Debug.Log("[ACHERON] ALL HUD BUTTONS WIRED.");
    }
}
