using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class StoreScreenTextCorrection
{
    private const string ScenePath = "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity";

    private static readonly string[] Screens =
    {
        "AIR UNITS wall screen",
        "GROUND UNITS wall screen",
        "SEA UNITS wall screen",
        "COMMANDERS wall screen",
        "EXPERIMENTAL UNITS wall screen",
        "SATELLITES wall screen"
    };

    [MenuItem("Obsidian Protocol/Store/Fix Screen Text Direction and Size")]
    public static void Fix()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (EditorApplication.isPlaying || scene.path != ScenePath)
        {
            Debug.LogError("Open SCN16_STORE and stop Play mode. No changes made.");
            return;
        }

        Transform[] roots = new Transform[Screens.Length];

        for (int i = 0; i < Screens.Length; i++)
        {
            GameObject screen = null;

            foreach (GameObject go in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (go.scene == scene && go.name == Screens[i])
                {
                    screen = go;
                    break;
                }
            }

            if (screen == null ||
                (roots[i] = screen.transform.Find("PROCUREMENT UI")) == null)
            {
                Debug.LogError("Missing screen or PROCUREMENT UI: " + Screens[i] +
                               ". No changes made.");
                return;
            }
        }

        string backup = ScenePath + ".bak_text_fix_" +
                        DateTime.Now.ToString("yyyyMMdd_HHmmss");
        File.Copy(ScenePath, backup, false);

        Undo.IncrementCurrentGroup();
        int group = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName("Fix Store Screen Text");

        foreach (Transform root in roots)
        {
            foreach (TextMesh mesh in root.GetComponentsInChildren<TextMesh>(true))
            {
                Undo.RecordObject(mesh, "Resize screen text");
                Undo.RecordObject(mesh.transform, "Orient screen text");

                // The screen's own rotation already points text toward the room.
                mesh.transform.localRotation = Quaternion.identity;

                // Set fixed sizes so running this tool again does not shrink text again.
                switch (mesh.gameObject.name)
                {
                    case "System title":       mesh.characterSize = 0.057f;  break;
                    case "System subtitle":    mesh.characterSize = 0.0315f; break;
                    case "System status":      mesh.characterSize = 0.0225f; break;
                    case "Slot code 1":
                    case "Slot code 2":
                    case "Slot code 3":
                    case "Selection heading":  mesh.characterSize = 0.027f;  break;
                    case "Slot state 1":
                    case "Slot state 2":
                    case "Slot state 3":       mesh.characterSize = 0.021f;  break;
                    case "Selection status":   mesh.characterSize = 0.0255f; break;
                    case "Footer":
                    case "Page indicator":     mesh.characterSize = 0.0225f; break;
                }

                EditorUtility.SetDirty(mesh);
                EditorUtility.SetDirty(mesh.transform);
            }
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Undo.CollapseUndoOperations(group);

        Debug.Log("Fixed text direction and size on six store screens. Backup: " + backup);
    }
}
