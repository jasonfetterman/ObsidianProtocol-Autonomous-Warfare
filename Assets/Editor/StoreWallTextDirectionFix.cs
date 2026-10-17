using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class StoreWallTextDirectionFix
{
    private const string ScenePath = "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity";

    [MenuItem("Obsidian Protocol/Store/Fix Side Wall Text Direction")]
    public static void Apply()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (EditorApplication.isPlaying || scene.path != ScenePath)
        {
            Debug.LogError("Stop Play mode and open SCN16_STORE first. No changes made.");
            return;
        }

        string[] names = {
            "AIR UNITS wall header text",
            "GROUND UNITS wall header text",
            "SEA UNITS wall header text",
            "COMMANDERS wall header text",
            "EXPERIMENTAL UNITS wall header text",
            "SATELLITES wall header text"
        };

        GameObject[] labels = new GameObject[names.Length];

        for (int i = 0; i < names.Length; i++)
        {
            foreach (GameObject go in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (go.name == names[i] && go.scene == scene)
                {
                    labels[i] = go;
                    break;
                }
            }

            if (labels[i] == null)
            {
                Debug.LogError("Missing label: " + names[i] + ". No changes made.");
                return;
            }
        }

        string backup = ScenePath + ".bak_text_direction_" +
                        DateTime.Now.ToString("yyyyMMdd_HHmmss");
        File.Copy(ScenePath, backup, false);

        Undo.IncrementCurrentGroup();
        int group = Undo.GetCurrentGroup();

        foreach (GameObject label in labels)
        {
            Undo.RecordObject(label.transform, "Fix Store header text direction");
            label.transform.localRotation =
                label.transform.localRotation * Quaternion.Euler(0f, 180f, 0f);
            EditorUtility.SetDirty(label.transform);
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Undo.CollapseUndoOperations(group);

        Debug.Log("Flipped all six Store header labels. Backup: " + backup);
    }
}
