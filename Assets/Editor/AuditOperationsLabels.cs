using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class AuditOperationsLabels
{
    [MenuItem("Obsidian Protocol/Operations/AUDIT NAV LABELS")]
    public static void Audit()
    {
        const string scenePath =
            "Assets/Scenes/SCN05_OPERATIONS/" +
            "[HUD] OPERATIONS HUD/Operations.unity";

        EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        foreach (Transform t in Object.FindObjectsByType<Transform>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None))
        {
            if (!t.name.StartsWith("NAV_LABEL_"))
                continue;

            Text text = t.GetComponent<Text>();

            if (text != null)
            {
                Debug.Log(
                    "[OPERATIONS LABEL] " +
                    t.name +
                    " = " +
                    text.text
                );
            }
        }
    }
}
