using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class StoreCategoryHeaderRepair
{
    private const string ScenePath =
        "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity";

    private static readonly string[] Names =
    {
        "AIR UNITS wall header text",
        "GROUND UNITS wall header text",
        "SEA UNITS wall header text",
        "COMMANDERS wall header text",
        "EXPERIMENTAL UNITS wall header text",
        "SATELLITES wall header text"
    };

    [MenuItem("Obsidian Protocol/Store/Repair Raised Category Headers")]
    public static void Apply()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (EditorApplication.isPlaying || scene.path != ScenePath)
        {
            Debug.LogError("Stop Play mode and open SCN16_STORE. No changes made.");
            return;
        }

        GameObject[] labels = new GameObject[Names.Length];

        for (int i = 0; i < Names.Length; i++)
        {
            foreach (GameObject go in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (go.name == Names[i] && go.scene == scene)
                {
                    labels[i] = go;
                    break;
                }
            }

            if (labels[i] == null || labels[i].GetComponent<TextMesh>() == null)
            {
                Debug.LogError("Missing label: " + Names[i] + ". No changes made.");
                return;
            }
        }

        string backup = ScenePath + ".bak_header_repair_" +
                        DateTime.Now.ToString("yyyyMMdd_HHmmss");
        File.Copy(ScenePath, backup, false);

        Undo.IncrementCurrentGroup();
        int group = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName("Repair Store Category Headers");

        for (int i = 0; i < labels.Length; i++)
        {
            GameObject label = labels[i];
            Undo.RecordObject(label.transform, "Raise original category label");

            Vector3 position = label.transform.localPosition;
            position.y = 7.7f;
            label.transform.localPosition = position;

            Transform oldGlow = label.transform.Find(Names[i] + " glow");
            if (oldGlow != null)
                Undo.DestroyObjectImmediate(oldGlow.gameObject);

            EditorUtility.SetDirty(label.transform);
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Undo.CollapseUndoOperations(group);

        Debug.Log("Removed duplicate glow text and raised six original labels. " +
                  "Screens untouched. Backup: " + backup);
    }
}
