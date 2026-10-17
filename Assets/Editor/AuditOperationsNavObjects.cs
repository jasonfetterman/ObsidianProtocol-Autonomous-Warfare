using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class AuditOperationsNavObjects
{
    [MenuItem("Obsidian Protocol/Operations/AUDIT NAV OBJECTS")]
    public static void Audit()
    {
        const string scenePath =
            "Assets/Scenes/SCN05_OPERATIONS/" +
            "[HUD] OPERATIONS HUD/Operations.unity";

        EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        foreach (Transform t in Object.FindObjectsByType<Transform>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None))
        {
            if (!t.name.StartsWith("NAV_"))
                continue;

            Debug.Log(
                "[OPERATIONS NAV] " +
                t.name +
                " | ACTIVE=" +
                t.gameObject.activeInHierarchy
            );

            Component[] components =
                t.GetComponents<Component>();

            foreach (Component component in components)
            {
                if (component != null)
                {
                    Debug.Log(
                        "    COMPONENT: " +
                        component.GetType().FullName
                    );
                }
            }
        }
    }
}
