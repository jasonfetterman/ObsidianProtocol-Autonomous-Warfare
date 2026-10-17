using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class InspectSCN15CancelState
{
    [MenuItem("Obsidian Protocol/Debug/Inspect SCN15 Cancel State")]
    public static void Inspect()
    {
        Debug.Log("===== SCN15 CANCEL STATE =====");

        Scene scene = SceneManager.GetActiveScene();
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            Debug.Log(
                "ROOT: " + root.name +
                " | activeSelf=" + root.activeSelf +
                " | activeInHierarchy=" + root.activeInHierarchy
            );

            Canvas canvas = root.GetComponent<Canvas>();
            if (canvas != null)
            {
                Debug.Log(
                    "  CANVAS: " + root.name +
                    " | enabled=" + canvas.enabled +
                    " | renderMode=" + canvas.renderMode
                );
            }
        }

        GameObject window = GameObject.Find("EXIT CONFIRMATION WINDOW");

        Debug.Log(
            "WINDOW AFTER FIND: " +
            (window == null ? "NOT FOUND" : window.name)
        );

        GameObject background = GameObject.Find("BACKGROUND");

        if (background != null)
        {
            CanvasRenderer cr =
                background.GetComponent<CanvasRenderer>();

            Debug.Log(
                "BACKGROUND: activeSelf=" + background.activeSelf +
                " | activeInHierarchy=" + background.activeInHierarchy +
                " | renderer=" + (cr != null)
            );
        }

        Debug.Log("===== END SCN15 CANCEL STATE =====");
    }
}
