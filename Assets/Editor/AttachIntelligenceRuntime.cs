using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public static class AttachIntelligenceRuntime
{
    [MenuItem("Obsidian Protocol/Fix/Attach Intelligence Runtime")]
    public static void Attach()
    {
        var scene = EditorSceneManager.OpenScene(
            "Assets/Scenes/SCN-18 INTELLIGENCE/Intellegence.unity");

        var obj = GameObject.Find("INTELLIGENCE SYSTEMS");

        if (obj == null)
        {
            Debug.LogError("[INTELLIGENCE] INTELLIGENCE SYSTEMS not found.");
            return;
        }

        if (obj.GetComponent<ObsidianProtocol.Game.Intelligence.IntelligenceRuntime>() == null)
        {
            obj.AddComponent<ObsidianProtocol.Game.Intelligence.IntelligenceRuntime>();
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            Debug.Log("[INTELLIGENCE] Runtime attached and scene saved.");
        }
        else
        {
            Debug.Log("[INTELLIGENCE] Runtime already attached.");
        }
    }
}
