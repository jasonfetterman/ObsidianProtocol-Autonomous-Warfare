using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class DoctrineControlScreenAudit
{
    [MenuItem("Obsidian Protocol/Deployment/Audit Doctrine Control Screens")]
    public static void Audit()
    {
        Scene scene = SceneManager.GetActiveScene();

        string path =
            "09. DEPLOYMENT/Console_Doctrine_Control_Terrminals/[DISPLAY] DUAL HOLOGRAPHIC SCREEN (5)";

        Transform display = FindPath(scene, path);

        if (display == null)
        {
            Debug.LogError("[DOCTRINE AUDIT] DISPLAY NOT FOUND");
            return;
        }

        foreach (string name in new string[]
        {
            "[SCREEN] LEFT OPERATOR",
            "[SCREEN] RIGHT OPERATOR"
        })
        {
            Transform screen = display.Find(name);

            if (screen == null)
            {
                Debug.LogError("[DOCTRINE AUDIT] NOT FOUND: " + name);
                continue;
            }

            Debug.Log(
                "[DOCTRINE AUDIT] " + name +
                " | WORLD=" + screen.position +
                " | ROTATION=" + screen.rotation.eulerAngles +
                " | LOSSY SCALE=" + screen.lossyScale
            );
        }
    }

    private static Transform FindPath(Scene scene, string path)
    {
        string[] parts = path.Split('/');
        Transform current = null;

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            if (root.name == parts[0])
            {
                current = root.transform;
                break;
            }
        }

        if (current == null)
            return null;

        for (int i = 1; i < parts.Length; i++)
        {
            current = current.Find(parts[i]);

            if (current == null)
                return null;
        }

        return current;
    }
}
