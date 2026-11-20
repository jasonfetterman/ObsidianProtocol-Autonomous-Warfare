#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class CleanBackupScenesFromBuildSettings
{
    [MenuItem("Obsidian Protocol/Build Settings/Remove Backup Scene Entries")]
    private static void RemoveBackupSceneEntries()
    {
        var scenes = EditorBuildSettings.scenes;
        var backupScenes = scenes
            .Where(s => System.Text.RegularExpressions.Regex.IsMatch(
                System.IO.Path.GetFileNameWithoutExtension(s.path),
                @"(?i)(backup|before|copy|duplicate|old|temp)"))
            .ToArray();

        if (backupScenes.Length == 0)
        {
            Debug.Log("[BUILD SETTINGS CLEANUP] No backup entries found.");
            return;
        }

        string report = string.Join("\n", backupScenes.Select(s =>
            $"{(s.enabled ? "ENABLED" : "DISABLED")}  {s.path}"));

        if (!EditorUtility.DisplayDialog(
            "Remove Backup Scene Entries?",
            $"This removes {backupScenes.Length} matching entries from Build Settings only.\n\n" +
            "It does NOT delete scene files.\n\n" + report,
            "Remove Entries",
            "Cancel"))
        {
            return;
        }

        var cleaned = scenes.Except(backupScenes).ToArray();
        EditorBuildSettings.scenes = cleaned;

        Debug.Log($"[BUILD SETTINGS CLEANUP] Removed {backupScenes.Length} backup entries from Build Settings. Scene files were not deleted.");
    }
}
#endif
