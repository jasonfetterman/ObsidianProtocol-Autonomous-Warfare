using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class FixCommanderCameraHeight
{
    [MenuItem("Obsidian Protocol/Fix/COMMANDER CAMERA HEIGHT")]
    public static void Fix()
    {
        Scene scene = SceneManager.GetActiveScene();

        GameObject commander =
            FindRecursive(
                scene.GetRootGameObjects(),
                "[PLAYER] COMMANDER"
            );

        if (commander == null)
        {
            Debug.LogError(
                "[COMMANDER CAMERA] [PLAYER] COMMANDER NOT FOUND."
            );
            return;
        }

        Transform cameraTransform =
            FindRecursiveTransform(
                commander.transform,
                "DEPLOYMENT CAMERA"
            );

        if (cameraTransform == null)
        {
            Debug.LogError(
                "[COMMANDER CAMERA] DEPLOYMENT CAMERA NOT FOUND."
            );
            return;
        }

        cameraTransform.localPosition =
            new Vector3(0f, 1.6f, 0f);

        cameraTransform.localEulerAngles =
            new Vector3(16.35f, 0f, 0f);

        EditorUtility.SetDirty(cameraTransform.gameObject);
        EditorSceneManager.MarkSceneDirty(scene);

        Debug.Log(
            "[COMMANDER CAMERA] HEIGHT FIXED."
        );

        Debug.Log(
            "[COMMANDER CAMERA] LOCAL POSITION = " +
            cameraTransform.localPosition
        );

        Debug.Log(
            "[COMMANDER CAMERA] LOCAL ROTATION = " +
            cameraTransform.localEulerAngles
        );

        Debug.Log(
            "[COMMANDER CAMERA] COMMANDER CAPSULE = EXISTING CHARACTER CONTROLLER"
        );

        Debug.Log(
            "[COMMANDER CAMERA] COMPLETE."
        );
    }

    private static GameObject FindRecursive(
        GameObject[] roots,
        string target
    )
    {
        foreach (GameObject root in roots)
        {
            if (root.name == target)
                return root;

            GameObject found =
                FindRecursiveTransformObject(
                    root.transform,
                    target
                );

            if (found != null)
                return found;
        }

        return null;
    }

    private static GameObject FindRecursiveTransformObject(
        Transform parent,
        string target
    )
    {
        foreach (Transform child in parent)
        {
            if (child.name == target)
                return child.gameObject;

            GameObject found =
                FindRecursiveTransformObject(
                    child,
                    target
                );

            if (found != null)
                return found;
        }

        return null;
    }

    private static Transform FindRecursiveTransform(
        Transform parent,
        string target
    )
    {
        foreach (Transform child in parent)
        {
            if (child.name == target)
                return child;

            Transform found =
                FindRecursiveTransform(
                    child,
                    target
                );

            if (found != null)
                return found;
        }

        return null;
    }
}
