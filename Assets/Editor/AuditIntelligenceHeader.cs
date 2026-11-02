using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class AuditIntelligenceHeader
{
    [MenuItem("Obsidian Protocol/Intelligence/AUDIT HEADER")]
    public static void Audit()
    {
        const string scenePath =
            "Assets/Scenes/SCN18_INTELLIGENCE/SCN18_INTELLIGENCE.unity";

        EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        GameObject hud = GameObject.Find("INTELLIGENCE HUD");

        if (hud == null)
        {
            Debug.LogError("[INTELLIGENCE HEADER AUDIT] HUD NOT FOUND.");
            return;
        }

        Transform header = hud.transform.Find("HEADER");

        if (header == null)
        {
            Debug.LogError("[INTELLIGENCE HEADER AUDIT] HEADER NOT FOUND.");
            return;
        }

        Debug.Log("[INTELLIGENCE HEADER AUDIT] CHILDREN:");

        foreach (Transform child in
            header.GetComponentsInChildren<Transform>(true))
        {
            if (child == header)
                continue;

            RectTransform rect =
                child.GetComponent<RectTransform>();

            string rectInfo = rect != null
                ? $"POS={rect.anchoredPosition} SIZE={rect.sizeDelta}"
                : "NO RECT";

            Debug.Log(
                "[INTELLIGENCE HEADER AUDIT] " +
                GetPath(child, header) +
                " | ACTIVE=" +
                child.gameObject.activeSelf +
                " | " +
                rectInfo
            );
        }

        Debug.Log("[INTELLIGENCE HEADER AUDIT] COMPLETE.");
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
