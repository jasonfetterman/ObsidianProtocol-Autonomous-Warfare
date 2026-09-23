#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class LogisticsHUDAutoAttach
{
    [MenuItem("Obsidian Protocol/Logistics/Attach HUD Controller")]
    public static void Attach()
    {
        var scene = EditorSceneManager.OpenScene(
            "Assets/Scenes/SCN-08  LOGISTICS BAY/[HUD] LOGISTICS HUD/Logistics_Bay.unity");

        var hud = GameObject.Find("LOGISTICS HUD");

        if (hud == null)
        {
            Debug.LogError("[LOGISTICS] LOGISTICS HUD not found.");
            return;
        }

        if (hud.GetComponent<LogisticsHUDController>() == null)
            hud.AddComponent<LogisticsHUDController>();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[LOGISTICS] HUD CONTROLLER ATTACHED.");
    }
}
#endif
