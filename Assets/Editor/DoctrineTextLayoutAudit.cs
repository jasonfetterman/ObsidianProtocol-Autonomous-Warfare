using UnityEditor;
using UnityEngine;
using TMPro;

public static class DoctrineTextLayoutAudit
{
    [MenuItem("Obsidian Protocol/Deployment/Audit Doctrine Text Layout")]
    public static void Audit()
    {
        string[] roots =
        {
            "[HOLO] DOCTRINE CONTROL - DOCTRINE",
            "[HOLO] DOCTRINE CONTROL - PARAMETERS"
        };

        foreach (string rootName in roots)
        {
            GameObject root = GameObject.Find(rootName);

            if (root == null)
            {
                Debug.LogError("[TEXT AUDIT] ROOT NOT FOUND: " + rootName);
                continue;
            }

            Debug.Log("[TEXT AUDIT] ===== " + rootName + " =====");

            RectTransform[] rects =
                root.GetComponentsInChildren<RectTransform>(true);

            foreach (RectTransform rect in rects)
            {
                if (rect.gameObject == root)
                    continue;

                TMP_Text tmp = rect.GetComponent<TMP_Text>();

                if (tmp != null)
                {
                    Debug.Log(
                        "[TEXT AUDIT] " + rect.name +
                        " | TEXT=\"" + tmp.text + "\"" +
                        " | LOCAL=" + rect.localPosition +
                        " | ANCHORED=" + rect.anchoredPosition +
                        " | SIZE=" + rect.rect.size +
                        " | ACTIVE=" + rect.gameObject.activeSelf
                    );
                }
            }
        }
    }
}
