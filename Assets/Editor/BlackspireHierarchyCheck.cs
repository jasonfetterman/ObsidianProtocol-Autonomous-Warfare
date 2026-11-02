using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class BlackspireHierarchyCheck
{
    [MenuItem("Obsidian Protocol/Diagnostics/List Blackspire Hierarchy")]
    public static void Run()
    {
        var scene = SceneManager.GetActiveScene();
        Debug.Log("[HIERARCHY CHECK] Active scene: " + scene.path +
                  " | Loaded=" + scene.isLoaded +
                  " | Roots=" + scene.rootCount);

        foreach (var go in scene.GetRootGameObjects())
        {
            Debug.Log("[HIERARCHY ROOT] " + go.name +
                      " | Children=" + go.transform.childCount +
                      " | Active=" + go.activeSelf);
            PrintChildren(go.transform, 1);
        }
    }

    static void PrintChildren(Transform parent, int depth)
    {
        foreach (Transform child in parent)
        {
            Debug.Log("[HIERARCHY] " + new string('-', depth) +
                      child.name + " | Children=" + child.childCount);
            PrintChildren(child, depth + 1);
        }
    }
}
