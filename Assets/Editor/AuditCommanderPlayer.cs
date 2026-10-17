using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class AuditCommanderPlayer
{
    [MenuItem("Obsidian Protocol/Audit/COMMANDER PLAYER")]
    public static void Audit()
    {
        Scene scene = SceneManager.GetActiveScene();

        Debug.Log("==================================================");
        Debug.Log("[COMMANDER PLAYER AUDIT]");
        Debug.Log("SCENE = " + scene.path);
        Debug.Log("==================================================");

        GameObject commander = FindRecursive(
            scene.GetRootGameObjects(),
            "[PLAYER] COMMANDER"
        );

        if (commander == null)
        {
            Debug.LogError(
                "[COMMANDER PLAYER AUDIT] [PLAYER] COMMANDER NOT FOUND."
            );
            return;
        }

        Debug.Log(
            "[COMMANDER PLAYER AUDIT] FOUND = " +
            commander.name
        );

        Debug.Log(
            "[COMMANDER PLAYER AUDIT] ACTIVE = " +
            commander.activeSelf
        );

        Transform t = commander.transform;

        Debug.Log(
            "[COMMANDER PLAYER AUDIT] POSITION = " +
            t.position
        );

        Debug.Log(
            "[COMMANDER PLAYER AUDIT] ROTATION = " +
            t.eulerAngles
        );

        Debug.Log(
            "[COMMANDER PLAYER AUDIT] SCALE = " +
            t.lossyScale
        );

        CharacterController cc =
            commander.GetComponent<CharacterController>();

        if (cc == null)
        {
            Debug.LogWarning(
                "[COMMANDER PLAYER AUDIT] CHARACTER CONTROLLER = MISSING"
            );
        }
        else
        {
            Debug.Log(
                "[COMMANDER PLAYER AUDIT] CHARACTER CONTROLLER = FOUND"
            );

            Debug.Log(
                "[COMMANDER PLAYER AUDIT] CC CENTER = " +
                cc.center
            );

            Debug.Log(
                "[COMMANDER PLAYER AUDIT] CC HEIGHT = " +
                cc.height
            );

            Debug.Log(
                "[COMMANDER PLAYER AUDIT] CC RADIUS = " +
                cc.radius
            );

            Debug.Log(
                "[COMMANDER PLAYER AUDIT] CC SKIN WIDTH = " +
                cc.skinWidth
            );

            Debug.Log(
                "[COMMANDER PLAYER AUDIT] CC STEP OFFSET = " +
                cc.stepOffset
            );

            Debug.Log(
                "[COMMANDER PLAYER AUDIT] CC SLOPE LIMIT = " +
                cc.slopeLimit
            );

            Debug.Log(
                "[COMMANDER PLAYER AUDIT] CC MIN MOVE DISTANCE = " +
                cc.minMoveDistance
            );

            Debug.Log(
                "[COMMANDER PLAYER AUDIT] CC ENABLED = " +
                cc.enabled
            );

            Debug.Log(
                "[COMMANDER PLAYER AUDIT] CC IS GROUNDED = " +
                cc.isGrounded
            );

            Debug.Log(
                "[COMMANDER PLAYER AUDIT] CC BOUNDS = " +
                cc.bounds
            );
        }

        CommanderPlayerMovement movement =
            commander.GetComponent<CommanderPlayerMovement>();

        if (movement == null)
        {
            Debug.LogWarning(
                "[COMMANDER PLAYER AUDIT] COMMANDER PLAYER MOVEMENT = MISSING"
            );
        }
        else
        {
            Debug.Log(
                "[COMMANDER PLAYER AUDIT] COMMANDER PLAYER MOVEMENT = FOUND"
            );

            Debug.Log(
                "[COMMANDER PLAYER AUDIT] MOVEMENT ENABLED = " +
                movement.enabled
            );
        }

        Debug.Log(
            "[COMMANDER PLAYER AUDIT] CHILDREN = " +
            commander.transform.childCount
        );

        for (int i = 0; i < commander.transform.childCount; i++)
        {
            Transform child = commander.transform.GetChild(i);

            Debug.Log(
                "[COMMANDER PLAYER AUDIT] CHILD " +
                i +
                " = " +
                GetPath(child)
            );

            Debug.Log(
                "[COMMANDER PLAYER AUDIT] CHILD POSITION = " +
                child.position
            );

            Debug.Log(
                "[COMMANDER PLAYER AUDIT] CHILD LOCAL POSITION = " +
                child.localPosition
            );

            Camera camera =
                child.GetComponentInChildren<Camera>(true);

            if (camera != null)
            {
                Debug.Log(
                    "[COMMANDER PLAYER AUDIT] CAMERA FOUND = " +
                    camera.name
                );

                Debug.Log(
                    "[COMMANDER PLAYER AUDIT] CAMERA LOCAL POSITION = " +
                    camera.transform.localPosition
                );

                Debug.Log(
                    "[COMMANDER PLAYER AUDIT] CAMERA LOCAL ROTATION = " +
                    camera.transform.localEulerAngles
                );

                Debug.Log(
                    "[COMMANDER PLAYER AUDIT] CAMERA NEAR CLIP = " +
                    camera.nearClipPlane
                );

                Debug.Log(
                    "[COMMANDER PLAYER AUDIT] CAMERA HEIGHT FROM PLAYER = " +
                    (
                        camera.transform.position.y -
                        commander.transform.position.y
                    )
                );
            }
        }

        Debug.Log("==================================================");
        Debug.Log("[COMMANDER PLAYER AUDIT] COMPLETE");
        Debug.Log("==================================================");
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
                FindRecursiveTransform(root.transform, target);

            if (found != null)
                return found;
        }

        return null;
    }

    private static GameObject FindRecursiveTransform(
        Transform parent,
        string target
    )
    {
        foreach (Transform child in parent)
        {
            if (child.name == target)
                return child.gameObject;

            GameObject found =
                FindRecursiveTransform(child, target);

            if (found != null)
                return found;
        }

        return null;
    }

    private static string GetPath(Transform t)
    {
        string path = t.name;

        while (t.parent != null)
        {
            t = t.parent;
            path = t.name + "/" + path;
        }

        return path;
    }
}
