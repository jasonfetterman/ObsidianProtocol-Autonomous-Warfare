using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class StoreCategoryNeonColors
{
    private const string ScenePath = "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity";

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
        new Color(0.00f, 0.88f, 1.00f), // Electric cyan
        new Color(1.00f, 0.48f, 0.08f), // Amber orange
        new Color(0.00f, 1.00f, 0.58f), // Reactor green
        new Color(1.00f, 0.82f, 0.20f), // Gold
        new Color(1.00f, 0.16f, 0.43f), // Hot magenta
        new Color(0.62f, 0.42f, 1.00f)  // Violet
    };

    [MenuItem("Obsidian Protocol/Store/Apply Category Neon Colors")]
    public static void Apply()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (EditorApplication.isPlaying || scene.path != ScenePath)
        {
            Debug.LogError("Stop Play mode and open SCN16_STORE first. No changes made.");
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
                Debug.LogError("Required category label missing: " + Names[i] +
                               ". No changes made.");
                return;
            }
        }

        string backup = ScenePath + ".bak_category_colors_" +
                        DateTime.Now.ToString("yyyyMMdd_HHmmss");
        File.Copy(ScenePath, backup, false);

        Undo.IncrementCurrentGroup();
        int group = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName("Apply Store Category Neon Colors");

        for (int i = 0; i < labels.Length; i++)
        {
            GameObject label = labels[i];
            TextMesh mainText = label.GetComponent<TextMesh>();

            Undo.RecordObject(mainText, "Color Store category text");
            mainText.color = Colors[i];
            EditorUtility.SetDirty(mainText);

            Transform glowTransform = label.transform.Find(Names[i] + " glow");
            if (glowTransform != null)
            {
                TextMesh glowText = glowTransform.GetComponent<TextMesh>();
                if (glowText != null)
                {
                    Undo.RecordObject(glowText, "Color Store category glow");
                    Color glowColor = Colors[i];
                    glowColor.r = Mathf.Min(1f, glowColor.r * 0.55f + 0.20f);
                    glowColor.g = Mathf.Min(1f, glowColor.g * 0.55f + 0.20f);
                    glowColor.b = Mathf.Min(1f, glowColor.b * 0.55f + 0.20f);
                    glowColor.a = 0.32f;
                    glowText.color = glowColor;
                    EditorUtility.SetDirty(glowText);
                }
            }

            EditorUtility.SetDirty(label);
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Undo.CollapseUndoOperations(group);

        Debug.Log("Applied six category-specific neon colors. Scene backup: " + backup);
    }
}
