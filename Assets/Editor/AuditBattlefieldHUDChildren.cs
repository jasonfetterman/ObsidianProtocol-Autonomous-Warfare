using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class AuditBattlefieldHUDChildren
{
    [MenuItem("Obsidian Protocol/Battlefield/AUDIT HUD CHILDREN")]
    public static void Audit()
    {
        const string scenePath =
            "Assets/Scenes/SCN03 _BATTLEFIELD/[HUD] BATTLEFIELD HUD/Battlefield_HUD.unity";

        var scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single);

        GameObject[] roots =
            scene.GetRootGameObjects();

        foreach (GameObject root in roots)
        {
            Print(root.transform, 0);
        }

        Debug.Log(
            "[BATTLEFIELD HUD AUDIT] COMPLETE.");
    }

    private static void Print(
        Transform current,
        int depth)
    {
        string indent = new string(' ', depth * 2);

        if (current.name.Contains("HUD") ||
            current.name.Contains("WINDOW") ||
            current.name.Contains("PANEL") ||
            current.name.Contains("DETAIL"))
        {
            Debug.Log(
                "[BATTLEFIELD HUD AUDIT] " +
                indent +
                current.name +
                " | ACTIVE=" +
                current.gameObject.activeSelf);
        }

        foreach (Transform child in current)
        {
            Print(child, depth + 1);
        }
    }
}
