using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using ObsidianProtocol.UI.Store;

public static class AuditStoreSceneController
{
    [MenuItem("Obsidian Protocol/Store/Audit Source Store Controller")]
    private static void Run()
    {
        const string path = "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity";
        var original = SceneManager.GetActiveScene();
        var source = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);

        int count = 0;

        foreach (var root in source.GetRootGameObjects())
        {
            foreach (var controller in root.GetComponentsInChildren<StoreMasterController>(true))
            {
                count++;
                Debug.Log($"[STORE AUDIT] Controller: {controller.name} | " +
                          $"Object path: {GetPath(controller.transform)} | " +
                          $"Active: {controller.gameObject.activeInHierarchy} | " +
                          $"Scene: {source.path}");
            }

            foreach (var canvas in root.GetComponentsInChildren<Canvas>(true))
                Debug.Log($"[STORE AUDIT] Canvas: {GetPath(canvas.transform)} | " +
                          $"Render mode: {canvas.renderMode} | Scene: {source.path}");
        }

        Debug.Log($"[STORE AUDIT] Source controllers found: {count}");

        EditorSceneManager.CloseScene(source, true);
        if (original.IsValid())
            SceneManager.SetActiveScene(original);
    }

    private static string GetPath(Transform t)
    {
        string path = t.name;
        while (t.parent != null)
        {
            t = t.parent;
            path = t.name + "/" + path;
        }
        return path;
    }
}
