using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class AuditIntelligenceNavigationAreas
{
    [MenuItem("Obsidian Protocol/Intelligence/AUDIT NAVIGATION AREAS")]
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
                "[INTELLIGENCE NAV AUDIT] HUD NOT FOUND."
            );
            return;
        }

        Debug.Log(
            "[INTELLIGENCE NAV AUDIT] " +
            "SEARCHING FOR NAVIGATION-LIKE OBJECTS..."
        );

        foreach (Transform t in
            hud.GetComponentsInChildren<Transform>(true))
        {
            string name = t.name.ToUpperInvariant();

            if (name.Contains("NAV") ||
                name.Contains("MENU") ||
                name.Contains("BACK") ||
                name.Contains("RETURN") ||
                name.Contains("COMMAND") ||
                name.Contains("OPERATIONS") ||
                name.Contains("CLOSE"))
            {
                Debug.Log(
                    "[INTELLIGENCE NAV AUDIT] " +
                    GetPath(t, hud.transform) +
                    " | ACTIVE=" +
                    t.gameObject.activeSelf
                );
            }
        }

        Debug.Log(
            "[INTELLIGENCE NAV AUDIT] COMPLETE"
        );
    }

    private static string GetPath(
        Transform target,
        Transform root)
    {
        string path = target.name;
        Transform parent = target.parent;

        while (parent != null && parent != root)
        {
            path = parent.name + "/" + path;
            parent = parent.parent;
        }

        return path;
    }
}
