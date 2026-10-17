using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class DoctrineControlFinalTransformFix
{
    private const string ScenePath =
        "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

    [MenuItem("Obsidian Protocol/Deployment/FINAL Fix Doctrine Holo Transforms")]
    public static void Fix()
    {
        var scene = EditorSceneManager.OpenScene(ScenePath);

        FixObject(
            "[HOLO] DOCTRINE CONTROL - DOCTRINE",
            new Vector3(12.16f, 8.00f, 5.44f)
        );

        FixObject(
            "[HOLO] DOCTRINE CONTROL - PARAMETERS",
            new Vector3(5.96f, 8.00f, 5.44f)
        );

        EditorSceneManager.SaveScene(scene);

        Debug.Log("[HOLO] Doctrine Control FINAL transforms fixed.");
    }

    private static void FixObject(string name, Vector3 position)
    {
        GameObject go = GameObject.Find(name);

        if (go == null)
        {
            Debug.LogError("[HOLO] NOT FOUND: " + name);
            return;
        }

        go.transform.SetParent(null, true);
        go.transform.position = position;
        go.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        go.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        Debug.Log(
            "[HOLO] " + name +
            " | WORLD=" + go.transform.position +
            " | ROT=" + go.transform.eulerAngles +
            " | SCALE=" + go.transform.localScale
        );
    }
}
