#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

[InitializeOnLoad]
public static class AddScenesToBuildSettings
{
    static AddScenesToBuildSettings()
    {
        SyncScenes();
    }

    [InitializeOnLoadMethod]
    public static void SyncScenes()
    {
        var currentScenes = EditorBuildSettings.scenes.ToList();

        // Remove backup/pre-change snapshots already listed in Build Settings.
        var filteredScenes = currentScenes
            .Where(s => !Regex.IsMatch(
                Path.GetFileNameWithoutExtension(s.path),
                @"(?i)(backup|before|copy|duplicate|old|temp)"))
            .ToList();

        bool changed = filteredScenes.Count != currentScenes.Count;
        currentScenes = filteredScenes;

        string[] foundSceneGUIDs =
            AssetDatabase.FindAssets("t:Scene", new[] { "Assets/Scenes" });

        foreach (string guid in foundSceneGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string fileName = Path.GetFileNameWithoutExtension(path);

            if (Regex.IsMatch(fileName, @"(?i)(backup|before|copy|duplicate|old|temp)"))
                continue;

            if (!currentScenes.Any(s => s.path == path))
            {
                currentScenes.Add(new EditorBuildSettingsScene(path, true));
                changed = true;
                Debug.Log($"[BUILD SETTINGS] Added eligible scene: {fileName}");
            }
        }

        if (changed)
        {
            EditorBuildSettings.scenes = currentScenes.ToArray();
            AssetDatabase.SaveAssets();
            Debug.Log("[BUILD SETTINGS] Backup entries filtered; eligible scenes synchronized.");
        }
    }
}
#endif
