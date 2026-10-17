using UnityEditor;
using UnityEngine;
using TMPro;

public static class DoctrineVsOperationComponentAudit
{
    [MenuItem("Obsidian Protocol/Deployment/Audit Doctrine Vs Operation Components")]
    public static void Audit()
    {
        AuditRoot("[HOLO] DOCTRINE CONTROL - DOCTRINE");
        AuditRoot("[HOLO] OPERATION SELECTION - OPERATION");
    }

    private static void AuditRoot(string rootName)
    {
        GameObject root = GameObject.Find(rootName);

        if (root == null)
        {
            Debug.LogError("[COMPONENT] NOT FOUND: " + rootName);
            return;
        }

        Debug.Log("[COMPONENT] ===== " + rootName + " =====");

        foreach (Component c in root.GetComponentsInChildren<Component>(true))
        {
            if (c == null)
                continue;

            Debug.Log(
                "[COMPONENT] " +
                c.gameObject.name +
                " | TYPE=" +
                c.GetType().FullName
            );
        }
    }
}
