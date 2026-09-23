using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using System.IO;

public static class InspectIntelligenceButtons
{
    [MenuItem("Obsidian Protocol/Inspect Intelligence Buttons")]
    public static void Inspect()
    {
        string scenePath = "Assets/Scenes/SCN-18 INTELLIGENCE/Intellegence.unity";
        string outputPath = Path.GetFullPath("INTELLIGENCE_BUTTON_INSPECTION.txt");

        Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

        using (StreamWriter writer = new StreamWriter(outputPath, false))
        {
            writer.WriteLine("=== INTELLIGENCE BUTTON INSPECTION ===");
            writer.WriteLine("");

            foreach (GameObject root in scene.GetRootGameObjects())
            {
                InspectObject(root.transform, writer);
            }

            writer.WriteLine("");
            writer.WriteLine("=== END ===");
        }

        Debug.Log("INTELLIGENCE BUTTON INSPECTION WRITTEN TO: " + outputPath);
        EditorUtility.RevealInFinder(outputPath);
    }

    static void InspectObject(Transform t, StreamWriter writer)
    {
        GameObject obj = t.gameObject;
        Button button = obj.GetComponent<Button>();

        if (button != null)
        {
            writer.WriteLine("BUTTON: " + GetPath(t));
            writer.WriteLine("  ACTIVE: " + obj.activeSelf);
            writer.WriteLine("  INTERACTABLE: " + button.interactable);
            writer.WriteLine("  ONCLICK COUNT: " + button.onClick.GetPersistentEventCount());

            for (int i = 0; i < button.onClick.GetPersistentEventCount(); i++)
            {
                Object target = button.onClick.GetPersistentTarget(i);
                string method = button.onClick.GetPersistentMethodName(i);

                writer.WriteLine(
                    "  EVENT " + i +
                    ": TARGET=" + (target != null ? target.name : "NULL") +
                    " METHOD=" + (string.IsNullOrEmpty(method) ? "NONE" : method)
                );
            }

            writer.WriteLine("");
        }

        for (int i = 0; i < t.childCount; i++)
        {
            InspectObject(t.GetChild(i), writer);
        }
    }

    static string GetPath(Transform t)
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
