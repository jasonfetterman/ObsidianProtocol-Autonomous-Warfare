using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class InspectSCN15AfterCancel
{
    [MenuItem("Obsidian Protocol/Debug/Inspect SCN15 Missing Scripts")]
    public static void Inspect()
    {
        Debug.Log("===== SCN15 MISSING SCRIPT INSPECTION =====");

        Scene scene = SceneManager.GetActiveScene();

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            InspectObject(root);
        }

        Camera cam = Object.FindFirstObjectByType<Camera>();

        if (cam != null)
        {
            Debug.Log(
                "CAMERA: " + cam.gameObject.name +
                " | enabled=" + cam.enabled +
                " | clearFlags=" + cam.clearFlags +
                " | background=" + cam.backgroundColor
            );
        }

        Debug.Log("===== END SCN15 MISSING SCRIPT INSPECTION =====");
    }

    private static void InspectObject(GameObject obj)
    {
        Component[] components = obj.GetComponents<Component>();

        foreach (Component component in components)
        {
            if (component == null)
            {
                Debug.Log(
                    "MISSING SCRIPT: " +
                    GetPath(obj.transform)
                );
            }
        }

        for (int i = 0; i < obj.transform.childCount; i++)
        {
            InspectObject(obj.transform.GetChild(i).gameObject);
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
