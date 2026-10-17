using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class DoctrineTransformAudit
{
    [MenuItem("Obsidian Protocol/Deployment/Audit Doctrine Transforms")]
    public static void Audit()
    {
        const string scenePath =
            "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

        var scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        string[] names =
        {
            "[HOLO] DOCTRINE CONTROL - DOCTRINE",
            "[HOLO] DOCTRINE CONTROL - PARAMETERS",
            "[HOLO] OPERATION SELECTION - OPERATION",
            "[HOLO] OPERATION SELECTION - THEATER"
        };

        foreach (string name in names)
        {
            GameObject go = GameObject.Find(name);

            if (go == null)
            {
                Debug.LogError("[AUDIT] MISSING: " + name);
                continue;
            }

            Transform t = go.transform;

            Debug.Log(
                "[AUDIT] " + name +
                " | WORLD=" + t.position +
                " | ROT=" + t.eulerAngles +
                " | LOCAL SCALE=" + t.localScale +
                " | LOSSY SCALE=" + t.lossyScale +
                " | PARENT=" +
                (t.parent == null ? "NONE" : t.parent.name)
            );
        }
    }
}
