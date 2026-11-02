using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class FixCommanderScale
{
    [MenuItem("Obsidian Protocol/Fix/COMMANDER SCALE")]
    public static void Fix()
    {
        Scene scene = SceneManager.GetActiveScene();

        GameObject commander = FindRecursive(
            scene.GetRootGameObjects(),
            "[PLAYER] COMMANDER"
        );

        if (commander == null)
        {
            Debug.LogError(
                "[COMMANDER SCALE] [PLAYER] COMMANDER NOT FOUND."
            );
            return;
        }

        CharacterController cc =
            commander.GetComponent<CharacterController>();

        if (cc == null)
        {
            Debug.LogError(
                "[COMMANDER SCALE] CHARACTER CONTROLLER NOT FOUND."
            );
            return;
        }

        cc.height = 4.0f;
        cc.radius = 0.6f;
        cc.center = new Vector3(0f, 2.0f, 0f);
        cc.stepOffset = 0.6f;
        cc.slopeLimit = 45f;
        cc.skinWidth = 0.08f;

        Transform cameraTransform =
            FindRecursiveTransform(
                commander.transform,
                "DEPLOYMENT CAMERA"
            );

        if (cameraTransform == null)
        {
            Debug.LogError(
                "[COMMANDER SCALE] DEPLOYMENT CAMERA NOT FOUND."
            );
            return;
        }

        cameraTransform.localPosition =
            new Vector3(0f, 3.2f, 0f);

        cameraTransform.localEulerAngles =
            new Vector3(16.35f, 0f, 0f);

        EditorUtility.SetDirty(commander);
        EditorUtility.SetDirty(cc);
        EditorUtility.SetDirty(cameraTransform.gameObject);

        EditorSceneManager.MarkSceneDirty(scene);

        Debug.Log(
            "[COMMANDER SCALE] CAPSULE HEIGHT = " +
            cc.height
        );

        Debug.Log(
            "[COMMANDER SCALE] CAPSULE RADIUS = " +
            cc.radius
        );

        Debug.Log(
            "[COMMANDER SCALE] CAPSULE CENTER = " +
            cc.center
        );

        Debug.Log(
            "[COMMANDER SCALE] CAMERA LOCAL POSITION = " +
            cameraTransform.localPosition
        );

        Debug.Log(
            "[COMMANDER SCALE] CAMERA LOCAL ROTATION = " +
            cameraTransform.localEulerAngles
        );

        Debug.Log(
            "[COMMANDER SCALE] COMPLETE."
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
                FindRecursiveObject(root.transform, target);

            if (found != null)
                return found;
        }

        return null;
    }

    private static GameObject FindRecursiveObject(
        Transform parent,
        string target
    )
    {
        foreach (Transform child in parent)
        {
            if (child.name == target)
                return child.gameObject;

            GameObject found =
                FindRecursiveObject(child, target);

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
                FindRecursiveTransform(child, target);

            if (found != null)
                return found;
        }

        return null;
    }
}
