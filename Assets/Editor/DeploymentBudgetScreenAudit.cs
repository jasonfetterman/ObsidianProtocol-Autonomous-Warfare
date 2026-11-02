using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public static class DeploymentBudgetScreenAudit
{
    [MenuItem("Obsidian Protocol/Deployment/AUDIT Budget Screen Parents")]
    public static void Audit()
    {
        var scene = EditorSceneManager.GetActiveScene();

        foreach (var go in scene.GetRootGameObjects())
        {
            foreach (var t in go.GetComponentsInChildren<Transform>(true))
            {
                if (t.name == "[SCREEN] LEFT OPERATOR" ||
                    t.name == "[SCREEN] RIGHT OPERATOR")
                {
                    Debug.Log(
                        "[SCREEN AUDIT] " +
                        t.name +
                        " | WORLD=" + t.position +
                        " | PARENT=" + (t.parent ? t.parent.name : "ROOT") +
                        " | PATH=" + GetPath(t)
                    );
                }
            }
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
