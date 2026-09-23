using UnityEditor;
using UnityEngine;

public static class AttachIntelligenceHUDController
{
    [MenuItem("Obsidian Protocol/Fix/Attach Intelligence HUD Controller")]
    public static void Attach()
    {
        var scene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene(
            "Assets/Scenes/SCN-18 INTELLIGENCE/Intellegence.unity");

        var hud = GameObject.Find("INTELLIGENCE HUD");

        if (hud == null)
        {
            Debug.LogError("[INTELLIGENCE] INTELLIGENCE HUD not found.");
            return;
        }

        if (hud.GetComponent<IntelligenceHUDController>() == null)
        {
            hud.AddComponent<IntelligenceHUDController>();
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
            UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
            Debug.Log("[INTELLIGENCE] IntelligenceHUDController attached and scene saved.");
        }
        else
        {
            Debug.Log("[INTELLIGENCE] IntelligenceHUDController already attached.");
        }
    }
}
