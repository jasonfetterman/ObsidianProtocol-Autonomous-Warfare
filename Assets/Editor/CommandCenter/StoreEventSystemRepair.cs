using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public static class StoreEventSystemRepair
{
    [MenuItem("Obsidian Protocol/Store/FIX STORE UI CLICKS")]
    private static void Fix()
    {
        var scene = EditorSceneManager.OpenScene(
            "Assets/Scenes/SCN02_COMMAND_CENTER/COMMAND CENTER HUD/Command_Center.unity");

        var existing = Object.FindFirstObjectByType<EventSystem>();
        if (existing == null)
        {
            var go = new GameObject("EVENT_SYSTEM");
            Undo.RegisterCreatedObjectUndo(go, "Create EventSystem");
            go.AddComponent<EventSystem>();
            go.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("Store UI click repair saved.");
    }
}
