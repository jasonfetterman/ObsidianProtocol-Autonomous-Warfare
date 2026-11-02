using UnityEditor;
using UnityEngine;

public static class DeploymentOperationHoloLayoutAudit
{
    [MenuItem("Obsidian Protocol/Deployment/Audit Operation Holo Layout")]
    public static void Audit()
    {
        string[] names =
        {
            "[HOLO] OPERATION SELECTION - OPERATION",
            "[HOLO] OPERATION SELECTION - THEATER"
        };

        foreach (string name in names)
        {
            GameObject root = GameObject.Find(name);

            if (root == null)
            {
                Debug.LogError("[LAYOUT AUDIT] NOT FOUND: " + name);
                continue;
            }

            RectTransform rootRect = root.GetComponent<RectTransform>();

            Debug.Log(
                "[LAYOUT AUDIT] " + name +
                " | Root Y=" + root.transform.position.y +
                " | Root Z=" + root.transform.position.z +
                " | Size=" + rootRect.rect.size
            );

            RectTransform[] children = root.GetComponentsInChildren<RectTransform>(true);

            foreach (RectTransform child in children)
            {
                if (child == root)
                    continue;

                if (child.name == "HOLO GLASS" ||
                    child.name == "HOLO BORDER TOP" ||
                    child.name == "HOLO BORDER BOTTOM" ||
                    child.name == "TITLE" ||
                    child.name == "SYSTEM")
                {
                    Debug.Log(
                        "[LAYOUT AUDIT]   " +
                        child.name +
                        " | Anchored=" + child.anchoredPosition +
                        " | Size=" + child.sizeDelta
                    );
                }
            }
        }
    }
}
