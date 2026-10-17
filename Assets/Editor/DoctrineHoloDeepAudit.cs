using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public static class DoctrineHoloDeepAudit
{
    [MenuItem("Obsidian Protocol/Deployment/Deep Audit Doctrine Holos")]
    public static void Audit()
    {
        Debug.Log("[DOCTRINE] ACTIVE SCENE = " +
            EditorSceneManager.GetActiveScene().path);

        string[] names =
        {
            "[HOLO] DOCTRINE CONTROL - DOCTRINE",
            "[HOLO] DOCTRINE CONTROL - PARAMETERS"
        };

        foreach (string name in names)
        {
            GameObject[] matches =
                Resources.FindObjectsOfTypeAll<GameObject>();

            foreach (GameObject go in matches)
            {
                if (go.name != name)
                    continue;

                Debug.Log(
                    "[DOCTRINE] " + name +
                    " | SCENE=" + go.scene.path +
                    " | ACTIVE=" + go.activeSelf +
                    " | WORLD=" + go.transform.position +
                    " | LOCAL=" + go.transform.localPosition +
                    " | ROTATION=" + go.transform.eulerAngles +
                    " | SCALE=" + go.transform.localScale +
                    " | PARENT=" +
                    (go.transform.parent == null
                        ? "NONE"
                        : go.transform.parent.name)
                );
            }
        }
    }
}
