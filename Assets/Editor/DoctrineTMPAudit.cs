using UnityEditor;
using UnityEngine;
using TMPro;

public static class DoctrineTMPAudit
{
    [MenuItem("Obsidian Protocol/Deployment/Audit Doctrine TMP")]
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
                continue;

            foreach (TMP_Text text in root.GetComponentsInChildren<TMP_Text>(true))
            {
                Debug.Log(
                    "[TMP] " + text.gameObject.name +
                    " | TEXT=" + text.text +
                    " | FONT=" +
                    (text.font == null ? "NULL" : text.font.name) +
                    " | ENABLED=" + text.enabled +
                    " | ACTIVE=" + text.gameObject.activeSelf +
                    " | COLOR=" + text.color
                );
            }
        }
    }
}
