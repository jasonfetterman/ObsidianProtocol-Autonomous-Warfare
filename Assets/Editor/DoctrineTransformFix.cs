using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class DoctrineTransformFix
{
    [MenuItem("Obsidian Protocol/Deployment/Fix Doctrine Root Transforms")]
    public static void Fix()
    {
        const string scenePath =
            "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

        var scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        SetTransform(
            "[HOLO] DOCTRINE CONTROL - DOCTRINE",
            new Vector3(12.16f, 8.00f, 5.44f),
            new Vector3(0f, 0f, 0f)
        );

        SetTransform(
            "[HOLO] DOCTRINE CONTROL - PARAMETERS",
            new Vector3(5.96f, 8.00f, 5.44f),
            new Vector3(0f, 0f, 0f)
        );

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[DOCTRINE TRANSFORM FIX] COMPLETE.");
    }

    static void SetTransform(
        string objectName,
        Vector3 position,
        Vector3 rotation)
    {
        GameObject go = GameObject.Find(objectName);

        if (go == null)
        {
            Debug.LogError(
                "[DOCTRINE TRANSFORM FIX] MISSING: " +
                objectName
            );
            return;
        }

        Transform t = go.transform;

        t.SetParent(null, true);
        t.position = position;
        t.rotation = Quaternion.Euler(rotation);
        t.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        Debug.Log(
            "[DOCTRINE TRANSFORM FIX] " +
            objectName +
            " -> WORLD=" + t.position +
            " ROT=" + t.eulerAngles +
            " SCALE=" + t.localScale
        );
    }
}
