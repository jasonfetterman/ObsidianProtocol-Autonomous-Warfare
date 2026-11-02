using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class AttachNewStoreHUDRuntime
{
    [MenuItem("Obsidian Protocol/Store/Connect Existing New HUD")]
    private static void Connect()
    {
        GameObject root = GameObject.Find("NEW_STORE_HUD_ROOT");

        if (root == null)
        {
            Debug.LogError("NEW_STORE_HUD_ROOT not found in the open scene.");
            return;
        }

        if (root.GetComponent<NewStoreHUDRuntime>() == null)
            Undo.AddComponent<NewStoreHUDRuntime>(root);

        EditorSceneManager.MarkSceneDirty(root.scene);
        Debug.Log("New Store HUD runtime connected. Save the scene.");
    }
}
