using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class DoctrineRestoreTransforms
{
    [MenuItem("Obsidian Protocol/Deployment/Restore Doctrine Screen Positions")]
    public static void Restore()
    {
        var scene = EditorSceneManager.OpenScene(
            "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity",
            OpenSceneMode.Single
        );

        GameObject a = GameObject.Find("[HOLO] DOCTRINE CONTROL - DOCTRINE");
        GameObject b = GameObject.Find("[HOLO] DOCTRINE CONTROL - PARAMETERS");

        if (a != null)
        {
            a.transform.position = new Vector3(12.16f, 8.00f, 5.44f);
            a.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            a.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        }

        if (b != null)
        {
            b.transform.position = new Vector3(5.96f, 8.00f, 5.44f);
            b.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            b.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        }

        EditorSceneManager.SaveScene(scene);

        Debug.Log("[DOCTRINE] ROOT POSITIONS RESTORED ONLY.");
    }
}
