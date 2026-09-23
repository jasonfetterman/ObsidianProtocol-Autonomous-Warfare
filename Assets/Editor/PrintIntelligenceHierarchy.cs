using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public static class PrintIntelligenceHierarchy
{
    [MenuItem("Obsidian Protocol/Print Intelligence Hierarchy")]
    public static void Print()
    {
        string scenePath = "Assets/Scenes/SCN-18 INTELLIGENCE/Intellegence.unity";
        string outputPath = Path.GetFullPath("INTELLIGENCE_HIERARCHY.txt");

        Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

        using (StreamWriter writer = new StreamWriter(outputPath, false))
        {
            writer.WriteLine("=== SCN-18 INTELLIGENCE ACTUAL HIERARCHY ===");
            writer.WriteLine("");

            foreach (GameObject root in scene.GetRootGameObjects())
            {
                PrintObject(root.transform, writer, 0);
            }

            writer.WriteLine("");
            writer.WriteLine("=== END ===");
        }

        Debug.Log("INTELLIGENCE HIERARCHY WRITTEN TO: " + outputPath);
        EditorUtility.RevealInFinder(outputPath);
    }

    static void PrintObject(Transform t, StreamWriter writer, int depth)
    {
        string indent = new string(' ', depth * 2);

        writer.WriteLine(
            indent + "- " + t.name +
            " [" + (t.gameObject.activeSelf ? "ACTIVE" : "INACTIVE") + "]"
        );

        for (int i = 0; i < t.childCount; i++)
        {
            PrintObject(t.GetChild(i), writer, depth + 1);
        }
    }
}
