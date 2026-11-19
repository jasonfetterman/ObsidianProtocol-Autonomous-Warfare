using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text;

public static class BattlefieldHierarchyInspector
{
    [MenuItem("Obsidian Protocol/Battlefield/INSPECT HUD HIERARCHY")]
    public static void Inspect()
    {
        StringBuilder report = new StringBuilder();
        report.AppendLine("========== BLACKSPIRE HUD HIERARCHY ==========");

        GameObject[] roots =
            SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (GameObject root in roots)
            PrintTree(root.transform, report, 0);

        Debug.Log(report.ToString());
    }

    private static void PrintTree(
        Transform current,
        StringBuilder report,
        int depth)
    {
        report.Append(' ', depth * 2);
        report.AppendLine(
            "[" + current.GetSiblingIndex() + "] " +
            current.name +
            (current.gameObject.activeSelf ? "" : " [INACTIVE]"));

        for (int i = 0; i < current.childCount; i++)
            PrintTree(current.GetChild(i), report, depth + 1);
    }
}
