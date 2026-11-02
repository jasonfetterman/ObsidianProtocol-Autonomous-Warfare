using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.Linq;

public static class AuditOperationSelectionTerminal
{
    [MenuItem("Obsidian Protocol/Deployment/AUDIT Operation Selection Terminal")]
    public static void Audit()
    {
        string scenePath =
            "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

        var scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        Debug.Log("========================================");
        Debug.Log("[OPERATION TERMINAL AUDIT]");
        Debug.Log("SCENE = " + scene.path);
        Debug.Log("========================================");

        GameObject[] all = Resources.FindObjectsOfTypeAll<GameObject>()
            .Where(g =>
                g.scene == scene &&
                g.hideFlags == HideFlags.None)
            .ToArray();

        var matches = all
            .Where(g =>
                g.name.ToLower().Contains("operation"))
            .OrderBy(g => g.name)
            .ToArray();

        Debug.Log(
            "[OPERATION TERMINAL AUDIT] MATCHES = " +
            matches.Length
        );

        foreach (GameObject go in matches)
        {
            Transform t = go.transform;

            Debug.Log(
                "[OBJECT] " + GetPath(t) +
                " | ACTIVE=" + go.activeSelf +
                " | POS=" + t.position +
                " | COMPONENTS=" +
                string.Join(
                    ", ",
                    go.GetComponents<Component>()
                        .Where(c => c != null)
                        .Select(c => c.GetType().Name)
                )
            );

            Collider[] colliders =
                go.GetComponents<Collider>();

            foreach (Collider c in colliders)
            {
                Debug.Log(
                    "[COLLIDER] " + GetPath(t) +
                    " | TYPE=" + c.GetType().Name +
                    " | TRIGGER=" + c.isTrigger
                );
            }

            ButtonAudit(go);
        }

        Debug.Log("========================================");
        Debug.Log("[OPERATION TERMINAL AUDIT] COMPLETE");
        Debug.Log("========================================");
    }

    private static void ButtonAudit(GameObject go)
    {
        var buttons = go
            .GetComponentsInChildren<UnityEngine.UI.Button>(true);

        foreach (var button in buttons)
        {
            Debug.Log(
                "[BUTTON] " +
                GetPath(button.transform) +
                " | ACTIVE=" +
                button.gameObject.activeSelf +
                " | PERSISTENT LISTENERS=" +
                button.onClick.GetPersistentEventCount()
            );
        }
    }

    private static string GetPath(Transform t)
    {
        string path = t.name;

        while (t.parent != null)
        {
            t = t.parent;
            path = t.name + "/" + path;
        }

        return path;
    }
}
