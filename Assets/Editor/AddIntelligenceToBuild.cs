using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public static class AddIntelligenceToBuild
{
    [MenuItem("Obsidian Protocol/Build/ADD INTELLIGENCE SCENE")]
    public static void Add()
    {
        const string scenePath =
            "Assets/Scenes/SCN18_INTELLIGENCE/SCN18_INTELLIGENCE.unity";

        Object sceneAsset =
            AssetDatabase.LoadAssetAtPath<Object>(scenePath);

        if (sceneAsset == null)
        {
            Debug.LogError(
                "[BUILD] Intelligence scene NOT FOUND: " +
                scenePath
            );
            return;
        }

        List<EditorBuildSettingsScene> scenes =
            new List<EditorBuildSettingsScene>(
                EditorBuildSettings.scenes
            );

        foreach (EditorBuildSettingsScene scene in scenes)
        {
            if (scene.path == scenePath)
            {
                scene.enabled = true;
                EditorBuildSettings.scenes = scenes.ToArray();

                Debug.Log(
                    "[BUILD] Intelligence scene already existed. " +
                    "ENABLED: " + scenePath
                );

                return;
            }
        }

        scenes.Add(
            new EditorBuildSettingsScene(
                scenePath,
                true
            )
        );

        EditorBuildSettings.scenes = scenes.ToArray();

        Debug.Log(
            "[BUILD] Intelligence scene ADDED: " +
            scenePath
        );
    }
}
