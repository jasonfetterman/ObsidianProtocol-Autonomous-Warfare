using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class MinimapVisibilityTest
{
    [MenuItem("Obsidian Protocol/Battlefield/TEST HUD WITHOUT MINIMAP IMAGE")]
    public static void Test()
    {
        var display = GameObject.Find("MINIMAP_RENDER_DISPLAY");

        if (!display)
        {
            Debug.LogError("[HUD TEST] Minimap display not found.");
            return;
        }

        Undo.RecordObject(display, "Temporarily Hide Minimap Display");
        display.SetActive(false);

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());

        Debug.Log("[HUD TEST] Minimap display disabled. Other HUD elements were not changed.");
    }
}
