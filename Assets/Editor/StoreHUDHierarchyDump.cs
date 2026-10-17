using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class StoreHUDHierarchyDump
{
    [MenuItem("Obsidian Protocol/Store/Dump Military Store HUD")]
    public static void Dump()
    {
        var scene =
            EditorSceneManager.GetActiveScene();

        var root =
            GameObject.Find("MILITARY STORE HUD");

        if (root == null)
        {
            Debug.LogError(
                "[STORE] MILITARY STORE HUD not found.");
            return;
        }

        Debug.Log(
            "[STORE] ===== MILITARY STORE HUD =====");

        DumpRecursive(root.transform, 0);
    }

    private static void DumpRecursive(
        Transform t,
        int depth)
    {
        Debug.Log(
            new string(' ', depth * 2) +
            t.name +
            " | active=" +
            t.gameObject.activeSelf);

        for (int i = 0;
             i < t.childCount;
             i++)
        {
            DumpRecursive(
                t.GetChild(i),
                depth + 1);
        }
    }
}
