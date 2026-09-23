using UnityEditor;
using UnityEngine;

public static class FindMissingIntelligenceScripts
{
    [MenuItem("Obsidian Protocol/Fix/Find Missing Intelligence Scripts")]
    public static void Find()
    {
        int count = 0;

        foreach (GameObject root in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            foreach (Transform t in root.GetComponentsInChildren<Transform>(true))
            {
                Component[] components = t.GetComponents<Component>();

                foreach (Component component in components)
                {
                    if (component == null)
                    {
                        Debug.LogError("[INTELLIGENCE] MISSING SCRIPT ON: " + t.name + " | PATH: " + GetPath(t), t.gameObject);
                        count++;
                    }
                }
            }
        }

        Debug.Log("[INTELLIGENCE] Missing script count: " + count);
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
