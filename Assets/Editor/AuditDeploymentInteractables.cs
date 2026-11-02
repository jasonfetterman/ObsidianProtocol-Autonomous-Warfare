using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class AuditDeploymentInteractables
{
    private const string ScenePath =
        "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

    [MenuItem("Obsidian Protocol/Deployment/AUDIT PHYSICAL INTERACTABLES")]
    public static void Audit()
    {
        EditorSceneManager.OpenScene(
            ScenePath,
            OpenSceneMode.Single);

        Debug.Log(
            "===== DEPLOYMENT PHYSICAL INTERACTABLE AUDIT =====");

        foreach (GameObject obj in
            Object.FindObjectsByType<GameObject>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None))
        {
            if (obj == null)
                continue;

            if (obj.GetComponent<Canvas>() != null)
                continue;

            if (obj.GetComponent<RectTransform>() != null)
                continue;

            string name =
                obj.name.ToLowerInvariant();

            bool keyword =
                name.Contains("terminal") ||
                name.Contains("console") ||
                name.Contains("computer") ||
                name.Contains("desk") ||
                name.Contains("screen") ||
                name.Contains("station") ||
                name.Contains("deployment") ||
                name.Contains("command") ||
                name.Contains("authorization") ||
                name.Contains("control");

            bool collider =
                obj.GetComponent<Collider>() != null ||
                obj.GetComponent<Collider2D>() != null;

            if (keyword || collider)
            {
                Debug.Log(
                    "[PHYSICAL] " +
                    obj.name +
                    " | POS=" +
                    obj.transform.position +
                    " | COLLIDER=" +
                    collider);
            }
        }

        Debug.Log(
            "===== END DEPLOYMENT PHYSICAL INTERACTABLE AUDIT =====");
    }
}
