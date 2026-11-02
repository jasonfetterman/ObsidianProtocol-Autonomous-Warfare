using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class DoctrineControlHoloRestore
{
    private const string ScenePath =
        "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

    [MenuItem("Obsidian Protocol/Deployment/Restore Doctrine Holo Position")]
    public static void Fix()
    {
        var scene = EditorSceneManager.OpenScene(ScenePath);

        Restore(
            "[HOLO] DOCTRINE CONTROL - DOCTRINE",
            new Vector3(12.16f, 8.00f, 5.44f)
        );

        Restore(
            "[HOLO] DOCTRINE CONTROL - PARAMETERS",
            new Vector3(5.96f, 8.00f, 5.44f)
        );

        EditorSceneManager.SaveScene(scene);

        Debug.Log("[HOLO] Doctrine Control positions RESTORED.");
    }

    private static void Restore(string name, Vector3 position)
    {
        GameObject go = GameObject.Find(name);

        if (go == null)
        {
            Debug.LogError("[HOLO] NOT FOUND: " + name);
            return;
        }

        go.transform.SetParent(null, false);
        go.transform.position = position;
        go.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        go.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        Debug.Log(
            "[HOLO] RESTORED " + name +
            " | WORLD=" + go.transform.position +
            " | ROT=" + go.transform.eulerAngles +
            " | SCALE=" + go.transform.localScale
        );
    }
}
