using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class RulesOfEngagementScreenAudit
{
    [MenuItem("Obsidian Protocol/Deployment/Audit Rules Of Engagement Screens")]
    public static void Audit()
    {
        Scene scene = SceneManager.GetActiveScene();

        string path =
            "09. DEPLOYMENT/Console_Rules_Of_EngagementTerrminals/[DISPLAY] DUAL HOLOGRAPHIC SCREEN (4)";

        Transform display = FindPath(scene, path);

        if (display == null)
        {
            Debug.LogError("[ROE AUDIT] DISPLAY NOT FOUND");
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
                Debug.LogError("[ROE AUDIT] NOT FOUND: " + name);
                continue;
            }

            Debug.Log(
                "[ROE AUDIT] " + name +
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
