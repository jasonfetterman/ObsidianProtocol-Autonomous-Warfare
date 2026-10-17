using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class AuditIntelligenceTopLevel
{
    [MenuItem("Obsidian Protocol/Intelligence/AUDIT TOP LEVEL HUD")]
    public static void Audit()
    {
        const string scenePath =
            "Assets/Scenes/SCN18_INTELLIGENCE/SCN18_INTELLIGENCE.unity";

        EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        GameObject hud =
            GameObject.Find("INTELLIGENCE HUD");

        if (hud == null)
        {
            Debug.LogError(
                "[INTELLIGENCE TOP LEVEL] " +
                "INTELLIGENCE HUD NOT FOUND."
            );
            return;
        }

        Debug.Log(
            "[INTELLIGENCE TOP LEVEL] ROOT FOUND: " +
            hud.name
        );

        foreach (Transform child in hud.transform)
        {
            Debug.Log(
                "[INTELLIGENCE TOP LEVEL] " +
                child.name +
                " | ACTIVE=" +
                child.gameObject.activeSelf +
                " | COMPONENTS=" +
                GetComponents(child)
            );
        }

        Debug.Log(
            "[INTELLIGENCE TOP LEVEL] COMPLETE"
        );
    }

    private static string GetComponents(Transform target)
    {
        Component[] components =
            target.GetComponents<Component>();

        string result = "";

        foreach (Component component in components)
        {
            if (component == null)
                continue;

            if (result.Length > 0)
                result += ", ";

            result += component.GetType().Name;
        }

        return result;
    }
}
