using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public static class DeploymentButtonAudit
{
    [MenuItem("Obsidian Protocol/Deployment/AUDIT BUTTONS")]
    public static void Audit()
    {
        const string scenePath =
            "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

        var scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        var buttons = Object.FindObjectsByType<Button>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        var sb = new StringBuilder();

        sb.AppendLine("=== SCN12 DEPLOYMENT BUTTON AUDIT ===");
        sb.AppendLine("Scene: " + scene.path);
        sb.AppendLine("BUTTON COUNT: " + buttons.Length);
        sb.AppendLine();

        foreach (var button in buttons)
        {
            var go = button.gameObject;

            sb.AppendLine("----------------------------------------");
            sb.AppendLine("BUTTON: " + go.name);
            sb.AppendLine("ACTIVE: " + go.activeInHierarchy);
            sb.AppendLine("INTERACTABLE: " + button.interactable);
            sb.AppendLine("ONCLICK COUNT: " + button.onClick.GetPersistentEventCount());

            for (int i = 0; i < button.onClick.GetPersistentEventCount(); i++)
            {
                sb.AppendLine(
                    "  ONCLICK[" + i + "] TARGET=" +
                    button.onClick.GetPersistentTarget(i) +
                    " METHOD=" +
                    button.onClick.GetPersistentMethodName(i)
                );
            }

            var components = go.GetComponents<MonoBehaviour>();

            foreach (var component in components)
            {
                if (component != null)
                    sb.AppendLine(
                        "COMPONENT: " +
                        component.GetType().FullName
                    );
            }

            sb.AppendLine();
        }

        Debug.Log(sb.ToString());

        System.IO.File.WriteAllText(
            "SCN12_DEPLOYMENT_BUTTON_AUDIT.txt",
            sb.ToString()
        );

        Debug.Log(
            "[DEPLOYMENT BUTTON AUDIT] COMPLETE — " +
            buttons.Length +
            " buttons found."
        );
    }
}
