using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class DeploymentForceCompositionAudit
{
    [MenuItem("Obsidian Protocol/Deployment/Audit Force Composition Terminal")]
    public static void Audit()
    {
        var scene = EditorSceneManager.GetActiveScene();

        foreach (var root in scene.GetRootGameObjects())
        {
            foreach (var t in root.GetComponentsInChildren<Transform>(true))
            {
                if (!t.name.ToLower().Contains("force") ||
                    !t.name.ToLower().Contains("composition"))
                    continue;

                Debug.Log(
                    "[FORCE COMPOSITION AUDIT] OBJECT = " + t.name +
                    " | POSITION = " + t.position +
                    " | ROTATION = " + t.eulerAngles +
                    " | SCALE = " + t.lossyScale);

                foreach (var r in t.GetComponentsInChildren<Renderer>(true))
                {
                    Debug.Log(
                        "[FORCE COMPOSITION AUDIT] RENDERER = " + r.name +
                        " | CENTER = " + r.bounds.center +
                        " | SIZE = " + r.bounds.size +
                        " | MIN = " + r.bounds.min +
                        " | MAX = " + r.bounds.max);
                }
            }
        }
    }
}
