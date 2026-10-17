using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text;

public static class DumpCurrentHierarchy
{
    [MenuItem("Obsidian Protocol/Debug/Dump Current Hierarchy")]
    public static void Dump()
    {
        var scene = SceneManager.GetActiveScene();
        var sb = new StringBuilder();

        sb.AppendLine("===== CURRENT SCENE =====");
        sb.AppendLine(scene.path);
        sb.AppendLine("=========================");

        foreach (var root in scene.GetRootGameObjects())
            DumpObject(root.transform, sb, 0);

        Debug.Log(sb.ToString());
    }

    static void DumpObject(Transform t, StringBuilder sb, int depth)
    {
        sb.AppendLine(
            new string(' ', depth * 2) +
            "- " + t.name +
            " [active=" + t.gameObject.activeSelf + "]"
        );

        foreach (var c in t.GetComponents<Component>())
            if (c != null)
                sb.AppendLine(
                    new string(' ', depth * 2 + 2) +
                    "COMP: " + c.GetType().FullName
                );

        for (int i = 0; i < t.childCount; i++)
            DumpObject(t.GetChild(i), sb, depth + 1);
    }
}
