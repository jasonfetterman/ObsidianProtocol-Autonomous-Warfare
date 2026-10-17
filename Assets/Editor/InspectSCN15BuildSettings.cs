using UnityEditor;
using UnityEngine;

public static class InspectSCN15BuildSettings
{
    [MenuItem("Obsidian Protocol/Debug/Inspect SCN15 Build Settings")]
    public static void Inspect()
    {
        Debug.Log("===== SCN15 BUILD SETTINGS =====");

        string current =
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;

        Debug.Log("CURRENT: " + current);

        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            var scene = EditorBuildSettings.scenes[i];

            if (!scene.enabled)
                continue;

            Debug.Log(
                "BUILD[" + i + "]: " + scene.path
            );
        }

        Debug.Log("===== END SCN15 BUILD SETTINGS =====");
    }
}
