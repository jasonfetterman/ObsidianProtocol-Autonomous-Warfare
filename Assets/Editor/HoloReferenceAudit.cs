using UnityEditor;
using UnityEngine;

public static class HoloReferenceAudit
{
    [MenuItem("Obsidian Protocol/Deployment/Audit Correct Holo Reference")]
    public static void Audit()
    {
        AuditObject("[HOLO] OPERATION SELECTION - OPERATION");
        AuditObject("[HOLO] OPERATION SELECTION - THEATER");
    }

    private static void AuditObject(string name)
    {
        GameObject go = GameObject.Find(name);

        if (go == null)
        {
            Debug.LogError("[REFERENCE AUDIT] NOT FOUND: " + name);
            return;
        }

        Transform t = go.transform;

        Debug.Log(
            "\n[REFERENCE AUDIT] " + name +
            "\nPARENT = " + (t.parent != null ? t.parent.name : "NONE") +
            "\nWORLD POSITION = " + t.position +
            "\nWORLD ROTATION = " + t.rotation.eulerAngles +
            "\nLOCAL POSITION = " + t.localPosition +
            "\nLOCAL ROTATION = " + t.localRotation.eulerAngles +
            "\nLOCAL SCALE = " + t.localScale +
            "\nLOSSY SCALE = " + t.lossyScale +
            "\nPATH = " + GetPath(t)
        );
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
