using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class MinimapVisibilityRestore
{
    [MenuItem("Obsidian Protocol/Battlefield/RESTORE MINIMAP DISPLAY")]
    public static void Restore()
    {
        var display = GameObject.Find("MINIMAP_RENDER_DISPLAY");
        if (!display) return;

        display.SetActive(true);
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());

        Debug.Log("[MINIMAP] Display restored.");
    }
}
