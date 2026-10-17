using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;

public static class AuditBattlefieldHUD
{
    [MenuItem("Obsidian Protocol/Audit/Battlefield HUD")]
    public static void Run()
    {
        string scenePath =
            "Assets/Scenes/SCN03 _BATTLEFIELD/[HUD] BATTLEFIELD HUD/Battlefield_HUD.unity";

        var scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        string outputPath =
            "Assets/_HierarchyInspection/SCN03_BATTLEFIELD/01_BATTLEFIELD_HUD_AUDIT.txt";

        Directory.CreateDirectory(
            "Assets/_HierarchyInspection/SCN03_BATTLEFIELD"
        );

        using (StreamWriter writer = new StreamWriter(outputPath, false))
        {
            writer.WriteLine("=== BATTLEFIELD HUD AUDIT ===");
            writer.WriteLine("SCENE: " + scenePath);
            writer.WriteLine();

            foreach (GameObject root in scene.GetRootGameObjects())
            {
                WriteObject(root, writer, 0);
            }
        }

        AssetDatabase.Refresh();

        Debug.Log(
            "[BATTLEFIELD HUD AUDIT] COMPLETE | " +
            outputPath
        );
    }

    private static void WriteObject(
        GameObject obj,
        StreamWriter writer,
        int depth
    )
    {
        writer.WriteLine(
            new string(' ', depth * 2) +
            obj.name +
            " | ACTIVE=" +
            obj.activeSelf
        );

        Component[] components = obj.GetComponents<Component>();

        foreach (Component component in components)
        {
            if (component == null)
                continue;

            writer.WriteLine(
                new string(' ', depth * 2 + 2) +
                "COMPONENT: " +
                component.GetType().Name
            );
        }

        foreach (Transform child in obj.transform)
        {
            WriteObject(child.gameObject, writer, depth + 1);
        }
    }
}
