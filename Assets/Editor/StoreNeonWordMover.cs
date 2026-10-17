using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class StoreNeonWordMover
{
    [MenuItem("Obsidian Protocol/Store/MOVE SIX NEON WORDS ABOVE SCREENS")]
    private static void MoveWords()
    {
        var scene = SceneManager.GetActiveScene();
        if (scene.path.Replace("\\", "/") !=
            "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity" ||
            EditorApplication.isPlaying)
        {
            Debug.LogError("Open SCN16_STORE outside Play Mode. Nothing changed.");
            return;
        }

        var root = GameObject.Find("STORE_AAA_REBUILD");
        if (root == null)
        {
            Debug.LogError("STORE_AAA_REBUILD missing. Nothing changed.");
            return;
        }

        string[] labels = {
            "AIR UNITS wall header text",
            "GROUND UNITS wall header text",
            "SEA UNITS wall header text",
            "COMMANDERS wall header text",
            "EXPERIMENTAL UNITS wall header text",
            "SATELLITES wall header text"
        };

        string[] screens = {
            "LEFT_SCREEN_1_DISPLAY", "LEFT_SCREEN_2_DISPLAY",
            "LEFT_SCREEN_3_DISPLAY", "RIGHT_SCREEN_4_DISPLAY",
            "RIGHT_SCREEN_5_DISPLAY", "RIGHT_SCREEN_6_DISPLAY"
        };

        Transform[] words = new Transform[6];
        for (int i = 0; i < 6; i++)
        {
            words[i] = FindInScene(labels[i]);
            if (words[i] == null ||
                root.transform.Find(screens[i]) == null)
            {
                Debug.LogError("Missing label or target screen: " +
                    labels[i] + " / " + screens[i] + ". Nothing changed.");
                return;
            }
        }

        if (!EditorSceneManager.SaveScene(scene))
        {
            Debug.LogError("Could not save scene. Nothing changed.");
            return;
        }

        string backup = scene.path + ".bak_neon_words_" +
            DateTime.Now.ToString("yyyyMMdd_HHmmss");
        if (File.Exists(backup))
        {
            Debug.LogError("Backup already exists. Nothing changed.");
            return;
        }
        File.Copy(scene.path, backup);

        float[] z = { -7.5f, 1.5f, 10.5f, -7.5f, 1.5f, 10.5f };

        for (int i = 0; i < 6; i++)
        {
            Undo.SetTransformParent(words[i], root.transform,
                "Move Store neon word");
            words[i].localPosition = new Vector3(
                i < 3 ? -18.22f : 18.22f, 7.45f, z[i]);
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("Moved six existing neon labels above their screens. Backup: " + backup);
    }

    private static Transform FindInScene(string objectName)
    {
        foreach (var go in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            foreach (var t in go.GetComponentsInChildren<Transform>(true))
                if (t.name == objectName) return t;
        }
        return null;
    }
}
