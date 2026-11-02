using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class AuditIntelligenceOperationsLink
{
    [MenuItem("Obsidian Protocol/Intelligence/AUDIT OPERATIONS LINK")]
    public static void Audit()
    {
        const string scenePath =
            "Assets/Scenes/SCN18_INTELLIGENCE/SCN18_INTELLIGENCE.unity";

        EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        GameObject link =
            GameObject.Find("OPERATIONS LINK");

        if (link == null)
        {
            Debug.LogError(
                "[OPERATIONS LINK AUDIT] NOT FOUND."
            );
            return;
        }

        Debug.Log(
            "[OPERATIONS LINK AUDIT] ROOT FOUND"
        );

        foreach (Transform child in link.GetComponentsInChildren<Transform>(true))
        {
            Button button =
                child.GetComponent<Button>();

            Component[] components =
                child.GetComponents<Component>();

            string componentList = "";

            foreach (Component component in components)
            {
                if (component == null)
                    continue;

                if (componentList.Length > 0)
                    componentList += ", ";

                componentList += component.GetType().Name;
            }

            int listeners =
                button != null
                    ? button.onClick.GetPersistentEventCount()
                    : -1;

            Debug.Log(
                "[OPERATIONS LINK AUDIT] " +
                GetPath(child, link.transform) +
                " | ACTIVE=" +
                child.gameObject.activeSelf +
                " | COMPONENTS=" +
                componentList +
                " | BUTTON_LISTENERS=" +
                listeners
            );
        }

        Debug.Log(
            "[OPERATIONS LINK AUDIT] COMPLETE"
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
