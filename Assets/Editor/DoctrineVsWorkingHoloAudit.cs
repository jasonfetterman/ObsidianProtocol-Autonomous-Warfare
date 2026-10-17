using UnityEditor;
using UnityEngine;

public static class DoctrineVsWorkingHoloAudit
{
    [MenuItem("Obsidian Protocol/Deployment/Audit Doctrine Vs Working Holo")]
    public static void Audit()
    {
        Check("[HOLO] DOCTRINE CONTROL - DOCTRINE");
        Check("[HOLO] OPERATION SELECTION - OPERATION");
    }

    private static void Check(string name)
    {
        GameObject go = GameObject.Find(name);

        if (go == null)
        {
            Debug.LogError("[COMPARE] NOT FOUND: " + name);
            return;
        }

        Debug.Log(
            "[COMPARE] " + name +
            " | WORLD=" + go.transform.position +
            " | ROT=" + go.transform.eulerAngles +
            " | SCALE=" + go.transform.localScale +
            " | PARENT=" +
            (go.transform.parent == null ? "NONE" : go.transform.parent.name)
        );
    }
}
