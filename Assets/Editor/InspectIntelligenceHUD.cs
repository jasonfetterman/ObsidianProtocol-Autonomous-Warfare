using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public static class InspectIntelligenceHUD
{
    [MenuItem("Obsidian Protocol/Inspect Intelligence HUD")]
    public static void Inspect()
    {
        string scenePath = "Assets/Scenes/SCN-18 INTELLIGENCE/Intellegence.unity";
        string outputPath = Path.GetFullPath("INTELLIGENCE_HUD_INSPECTION.txt");

        Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

        GameObject hud = GameObject.Find("INTELLIGENCE HUD");

        if (hud == null)
        {
            File.WriteAllText(outputPath, "INTELLIGENCE HUD NOT FOUND");
            Debug.LogError("INTELLIGENCE HUD NOT FOUND");
            return;
        }

        using (StreamWriter writer = new StreamWriter(outputPath, false))
        {
            writer.WriteLine("=== INTELLIGENCE HUD COMPONENT INSPECTION ===");
            writer.WriteLine("");

            InspectObject(hud.transform, writer, 0);

            writer.WriteLine("");
            writer.WriteLine("=== END ===");
        }

        Debug.Log("INTELLIGENCE HUD INSPECTION WRITTEN TO: " + outputPath);
        EditorUtility.RevealInFinder(outputPath);
    }

    static void InspectObject(Transform t, StreamWriter writer, int depth)
    {
        string indent = new string(' ', depth * 2);
        GameObject obj = t.gameObject;

        writer.WriteLine(
            indent + "- " + obj.name +
            " [" + (obj.activeSelf ? "ACTIVE" : "INACTIVE") + "]"
        );

        Component[] components = obj.GetComponents<Component>();

        foreach (Component component in components)
        {
            if (component == null)
            {
                writer.WriteLine(indent + "  COMPONENT: MISSING SCRIPT");
            }
            else
            {
                writer.WriteLine(indent + "  COMPONENT: " + component.GetType().FullName);
            }
        }

        for (int i = 0; i < t.childCount; i++)
        {
            InspectObject(t.GetChild(i), writer, depth + 1);
        }
    }
}
