using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;

public static class CommanderProfileHierarchyDump
{
    [MenuItem("Obsidian Protocol/Inspection/Dump Commander Profile Hierarchy")]
    public static void Dump()
    {
        string scenePath =
            "Assets/Scenes/SCN09_COMMANDER_PROFILE/[HUD] COMMANDER PROFILE HUD/Player_Commander_Profile.unity";

        Scene scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        StringBuilder sb = new StringBuilder();

        sb.AppendLine("============================================================");
        sb.AppendLine("OBSIDIAN PROTOCOL — SCN09 COMMANDER PROFILE");
        sb.AppendLine("UNITY-AUTHORITATIVE FULL HIERARCHY");
        sb.AppendLine("============================================================");
        sb.AppendLine();
        sb.AppendLine("Scene: " + scene.path);
        sb.AppendLine();

        GameObject[] roots = scene.GetRootGameObjects();

        sb.AppendLine("Root objects: " + roots.Length);
        sb.AppendLine();

        foreach (GameObject root in roots)
        {
            DumpTransform(root.transform, sb, 0);
        }

        string output =
            "C:/ObsidianProtocol-Autonomous-Warfare/OPAW/" +
            "_HierarchyInspection/09_COMMANDER_PROFILE_UNITY_HIERARCHY.txt";

        File.WriteAllText(output, sb.ToString());

        Debug.Log(
            "[COMMANDER PROFILE] Hierarchy dump complete: " + output
        );

        AssetDatabase.Refresh();
    }

    private static void DumpTransform(
        Transform transform,
        StringBuilder sb,
        int depth
    )
    {
        string indent = new string(' ', depth * 4);

        sb.AppendLine(
            indent + "├── " + transform.name
        );

        for (int i = 0; i < transform.childCount; i++)
        {
            DumpTransform(
                transform.GetChild(i),
                sb,
                depth + 1
            );
        }
    }
}
