using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class BuildCommandCenterStore
{
    [MenuItem("Obsidian Protocol/Store/Install Existing Runtime Store")]
    private static void Install()
    {
        Scene scene = SceneManager.GetActiveScene();
        GameObject host = null;

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
                if (child.name == "STORE_REAL_PANEL")
                    host = child.gameObject;
        }

        if (host == null)
        {
            Debug.LogError("STORE_REAL_PANEL was not found in the active scene.");
            return;
        }

        Type runtimeType = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => {
                try { return a.GetTypes(); }
                catch { return Type.EmptyTypes; }
            })
            .FirstOrDefault(t => t.Name == "StoreRuntimeController");

        if (runtimeType == null)
        {
            Debug.LogError("StoreRuntimeController type was not found.");
            return;
        }

        Undo.RegisterFullObjectHierarchyUndo(host, "Install Command Center Store");

        foreach (Component component in host.GetComponents<Component>())
            if (component != null && component.GetType().Name == "StoreMasterController")
                Undo.DestroyObjectImmediate(component);

        if (host.GetComponent(runtimeType) == null)
            Undo.AddComponent(host, runtimeType);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("Existing runtime Store installed on STORE_REAL_PANEL. Enter Play mode to test.");
    }
}