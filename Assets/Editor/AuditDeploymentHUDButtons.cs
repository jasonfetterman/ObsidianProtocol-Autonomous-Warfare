using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class AuditDeploymentHUDButtons
{
    [MenuItem("Obsidian Protocol/Deployment/AUDIT HUD BUTTONS")]
    public static void Audit()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (!scene.isLoaded)
        {
            Debug.LogError("[DEPLOYMENT HUD AUDIT] Scene not loaded.");
            return;
        }

        GameObject hud = FindRecursive(
            scene.GetRootGameObjects(),
            "[HUD] DEPLOYMENT HUD"
        );

        if (hud == null)
        {
            Debug.LogError(
                "[DEPLOYMENT HUD AUDIT] HUD NOT FOUND."
            );
            return;
        }

        Button[] buttons = hud.GetComponentsInChildren<Button>(true);

        Debug.Log(
            "[DEPLOYMENT HUD AUDIT] BUTTON COUNT = " +
            buttons.Length
        );

        foreach (Button button in buttons)
        {
            int persistent = button.onClick.GetPersistentEventCount();

            Debug.Log(
                "[DEPLOYMENT HUD AUDIT] " +
                button.gameObject.name +
                " | ACTIVE=" +
                button.gameObject.activeInHierarchy +
                " | PERSISTENT LISTENERS=" +
                persistent
            );
        }

        Debug.Log("[DEPLOYMENT HUD AUDIT] COMPLETE.");
    }

    private static GameObject FindRecursive(
        GameObject[] roots,
        string target)
    {
        foreach (GameObject root in roots)
        {
            GameObject found =
                FindRecursive(root.transform, target);

            if (found != null)
                return found;
        }

        return null;
    }

    private static GameObject FindRecursive(
        Transform parent,
        string target)
    {
        if (parent.name == target)
            return parent.gameObject;

        for (int i = 0; i < parent.childCount; i++)
        {
            GameObject found =
                FindRecursive(parent.GetChild(i), target);

            if (found != null)
                return found;
        }

        return null;
    }
}
