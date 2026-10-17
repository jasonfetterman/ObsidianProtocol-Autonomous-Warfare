using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class AuditOperationsButtons
{
    [MenuItem("Obsidian Protocol/Operations/AUDIT ALL BUTTONS")]
    public static void Audit()
    {
        const string scenePath =
            "Assets/Scenes/SCN05_OPERATIONS/" +
            "[HUD] OPERATIONS HUD/Operations.unity";

        EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        Button[] buttons =
            Object.FindObjectsByType<Button>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

        Debug.Log(
            "[OPERATIONS AUDIT] BUTTON COUNT = " +
            buttons.Length
        );

        foreach (Button button in buttons)
        {
            string label = "";

            Text legacyText =
                button.GetComponentInChildren<Text>(
                    true
                );

            if (legacyText != null)
                label = legacyText.text;

            Debug.Log(
                "[OPERATIONS BUTTON] " +
                "NAME=" + button.gameObject.name +
                " | LABEL=" + label +
                " | ACTIVE=" + button.gameObject.activeInHierarchy
            );
        }
    }
}
