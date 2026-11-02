using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class StoreRaiseAndGlowHeaders
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

    private static readonly Color[] Colors =
    {
        new Color(0.00f, 0.88f, 1.00f),
        new Color(1.00f, 0.48f, 0.08f),
        new Color(0.00f, 1.00f, 0.58f),
        new Color(1.00f, 0.82f, 0.20f),
        new Color(1.00f, 0.16f, 0.43f),
        new Color(0.62f, 0.42f, 1.00f)
    };

    [MenuItem("Obsidian Protocol/Store/Raise and Glow Category Headers")]
    public static void Apply()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogError("Stop Play mode first. No changes made.");
            return;
        }

        Scene scene = SceneManager.GetActiveScene();
        if (scene.path != ScenePath)
        {
            Debug.LogError("Open SCN16_STORE first. No changes made.");
            return;
        }

        GameObject[] labels = new GameObject[Names.Length];

        for (int i = 0; i < Names.Length; i++)
        {
            foreach (GameObject go in
                     Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (go.name == Names[i] && go.scene == scene)
                {
                    labels[i] = go;
                    break;
                }
            }

            if (labels[i] == null ||
                labels[i].GetComponent<TextMesh>() == null)
            {
                Debug.LogError(
                    "Missing label or TextMesh: " + Names[i] +
                    ". No changes made.");
                return;
            }
        }

        string backup = ScenePath + ".bak_header_glow_" +
                        DateTime.Now.ToString("yyyyMMdd_HHmmss");
        File.Copy(ScenePath, backup, false);

        Undo.IncrementCurrentGroup();
        int group = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName("Raise and Glow Store Category Headers");

        for (int i = 0; i < labels.Length; i++)
        {
            GameObject label = labels[i];
            TextMesh main = label.GetComponent<TextMesh>();

            Undo.RecordObject(label.transform, "Raise category header");
            Undo.RecordObject(main, "Color category header");

            // Raise the text only; do not move its screen or header panel.
            Vector3 pos = label.transform.localPosition;
            pos.y = 7.30f;
            label.transform.localPosition = pos;

            main.color = Colors[i];
            EditorUtility.SetDirty(main);
            EditorUtility.SetDirty(label.transform);

            string glowName = Names[i] + " glow";
            Transform existing = label.transform.Find(glowName);
            GameObject glowObject;

            if (existing == null)
            {
                glowObject = new GameObject(glowName);
                Undo.RegisterCreatedObjectUndo(
                    glowObject, "Create category header glow");
                glowObject.transform.SetParent(label.transform, false);
            }
            else
            {
                glowObject = existing.gameObject;
            }

            TextMesh glow = glowObject.GetComponent<TextMesh>();
            if (glow == null)
                glow = Undo.AddComponent<TextMesh>(glowObject);

            Undo.RecordObject(glowObject.transform, "Position header glow");
            Undo.RecordObject(glow, "Set header glow");

            glowObject.transform.localPosition =
                new Vector3(0f, 0f, 0.025f);
            glowObject.transform.localRotation = Quaternion.identity;
            glowObject.transform.localScale = Vector3.one;

            glow.text = main.text;
            glow.anchor = main.anchor;
            glow.alignment = main.alignment;
            glow.characterSize = main.characterSize * 1.10f;
            glow.fontSize = main.fontSize;
            Color glowColor = Colors[i];
            glowColor.a = 0.30f;
            glow.color = glowColor;
            glowObject.transform.SetAsFirstSibling();

            EditorUtility.SetDirty(glow);
            EditorUtility.SetDirty(glowObject);
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Undo.CollapseUndoOperations(group);

        Debug.Log("Raised six category labels and added glow text. " +
                  "Screens untouched. Backup: " + backup);
    }
}
