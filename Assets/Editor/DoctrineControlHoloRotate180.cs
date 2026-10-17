using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class DoctrineControlHoloRotate180
{
    private const string ScenePath =
        "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

    [MenuItem("Obsidian Protocol/Deployment/Rotate Doctrine Holos 180")]
    public static void Fix()
    {
        var scene = EditorSceneManager.OpenScene(ScenePath);

        Rotate("[HOLO] DOCTRINE CONTROL - DOCTRINE");
        Rotate("[HOLO] DOCTRINE CONTROL - PARAMETERS");

        EditorSceneManager.SaveScene(scene);

        Debug.Log("[HOLO] Doctrine Control rotated to 180 degrees.");
    }

    private static void Rotate(string name)
    {
        GameObject go = GameObject.Find(name);

        if (go == null)
        {
            Debug.LogError("[HOLO] NOT FOUND: " + name);
            return;
        }

        go.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }
}
