using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class StoreScreenInterfaceCorrection
{
    private const string ScenePath = "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity";

    private static readonly string[] ScreenNames =
    {
        "AIR UNITS wall screen",
        "GROUND UNITS wall screen",
        "SEA UNITS wall screen",
        "COMMANDERS wall screen",
        "EXPERIMENTAL UNITS wall screen",
        "SATELLITES wall screen"
    };

    [MenuItem("Obsidian Protocol/Store/Correct Interface Size and Text")]
    public static void Correct()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (EditorApplication.isPlaying || scene.path != ScenePath)
        {
            Debug.LogError("Open SCN16_STORE and stop Play mode. No changes made.");
            return;
        }

        Transform[] roots = new Transform[ScreenNames.Length];

        for (int i = 0; i < ScreenNames.Length; i++)
        {
            GameObject screen = null;

            foreach (GameObject go in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (go.scene == scene && go.name == ScreenNames[i])
                {
                    screen = go;
                    break;
                }
            }

            if (screen == null)
            {
                Debug.LogError("Screen not found: " + ScreenNames[i] + ". No changes made.");
                return;
            }

            roots[i] = screen.transform.Find("PROCUREMENT UI");

            if (roots[i] == null)
            {
                Debug.LogError("PROCUREMENT UI missing from " + ScreenNames[i] + ". No changes made.");
                return;
            }

            Vector3 scale = screen.transform.localScale;
            if (Mathf.Approximately(scale.x, 0f) ||
                Mathf.Approximately(scale.y, 0f))
            {
                Debug.LogError("Invalid screen scale on " + ScreenNames[i] + ". No changes made.");
                return;
            }
        }

        string backup = ScenePath + ".bak_interface_correction_" +
                        DateTime.Now.ToString("yyyyMMdd_HHmmss");
        File.Copy(ScenePath, backup, false);

        Undo.IncrementCurrentGroup();
        int group = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName("Correct Store Interface Size and Text");

        for (int i = 0; i < roots.Length; i++)
        {
            Transform root = roots[i];
            Transform screen = root.parent;

            Undo.RecordObject(root, "Fit interface to screen");
            root.localScale = new Vector3(
                1f / screen.localScale.x,
                1f / screen.localScale.y,
                1f);
            root.localPosition = new Vector3(0f, 0f, 0.7f);
            root.localRotation = Quaternion.identity;

            foreach (TextMesh mesh in root.GetComponentsInChildren<TextMesh>(true))
            {
                Undo.RecordObject(mesh.transform, "Correct screen text direction");
                mesh.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            }

            EditorUtility.SetDirty(root.gameObject);
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Undo.CollapseUndoOperations(group);

        Debug.Log("Corrected sizing and text orientation on all six existing interfaces. Backup: " + backup);
    }
}
