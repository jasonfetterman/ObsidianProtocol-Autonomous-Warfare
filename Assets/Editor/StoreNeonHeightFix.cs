using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class StoreNeonHeightFix
{
    [MenuItem("Obsidian Protocol/Store/SET SIX NEONS Y TO 9.5")]
    private static void SetNeonHeight()
    {
        var scene = SceneManager.GetActiveScene();

        if (scene.path.Replace("\\", "/") !=
            "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity" ||
            EditorApplication.isPlaying)
        {
            Debug.LogError("Open SCN16_STORE outside Play Mode. Nothing changed.");
            return;
        }

        string[] labels =
        {
            "AIR UNITS wall header text",
            "GROUND UNITS wall header text",
            "SEA UNITS wall header text",
            "COMMANDERS wall header text",
            "EXPERIMENTAL UNITS wall header text",
            "SATELLITES wall header text"
        };

        Transform[] words = new Transform[labels.Length];

        for (int i = 0; i < labels.Length; i++)
        {
            foreach (var go in scene.GetRootGameObjects())
            {
                foreach (var t in go.GetComponentsInChildren<Transform>(true))
                {
                    if (t.name == labels[i])
                    {
                        words[i] = t;
                        break;
                    }
                }
                if (words[i] != null) break;
            }

            if (words[i] == null)
            {
                Debug.LogError("Missing neon: " + labels[i] + ". Nothing changed.");
                return;
            }
        }

        Undo.RecordObjects(words, "Set Store Neon Height to 9.5");

        foreach (var word in words)
        {
            var p = word.localPosition;
            p.y = 9.5f;
            word.localPosition = p;
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("All six neon words set to local Y = 9.5.");
    }
}
