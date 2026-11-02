using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class DeploymentBudgetHoloAudit
{
    [MenuItem("Obsidian Protocol/Deployment/Audit Deployment Budget Holos")]
    public static void Audit()
    {
        Scene scene = SceneManager.GetActiveScene();

        AuditObject(scene, "[HOLO] DEPLOYMENT BUDGET - BUDGET");
        AuditObject(scene, "[HOLO] DEPLOYMENT BUDGET - FORCE COST");
    }

    private static void AuditObject(Scene scene, string name)
    {
        GameObject go = GameObject.Find(name);

        if (go == null)
        {
            Debug.LogError("[HOLO AUDIT] NOT FOUND: " + name);
            return;
        }

        Transform t = go.transform;

        Debug.Log(
            "\n[HOLO AUDIT] " + name +
            "\nACTIVE = " + go.activeInHierarchy +
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
