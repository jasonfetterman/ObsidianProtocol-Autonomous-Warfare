using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class AuditCommanderScale
{
    [MenuItem("Obsidian Protocol/Audit/COMMANDER SCALE")]
    public static void Audit()
    {
        Scene scene = SceneManager.GetActiveScene();

        GameObject commander = FindRecursive(
            scene.GetRootGameObjects(),
            "[PLAYER] COMMANDER"
        );

        if (commander == null)
        {
            Debug.LogError("[COMMANDER SCALE] COMMANDER NOT FOUND.");
            return;
        }

        Vector3 playerPos = commander.transform.position;

        CharacterController cc =
            commander.GetComponent<CharacterController>();

        Camera cam =
            commander.GetComponentInChildren<Camera>(true);

        Debug.Log("==================================================");
        Debug.Log("[COMMANDER SCALE AUDIT]");
        Debug.Log("PLAYER POSITION = " + playerPos);

        if (cc != null)
        {
            Debug.Log("CC HEIGHT = " + cc.height);
            Debug.Log("CC RADIUS = " + cc.radius);
            Debug.Log("CC CENTER = " + cc.center);
            Debug.Log("CC WORLD TOP = " +
                (commander.transform.position.y +
                 cc.center.y +
                 cc.height * 0.5f));

            Debug.Log("CC WORLD BOTTOM = " +
                (commander.transform.position.y +
                 cc.center.y -
                 cc.height * 0.5f));
        }

        if (cam != null)
        {
            Debug.Log("CAMERA WORLD POSITION = " +
                cam.transform.position);

            Debug.Log("CAMERA LOCAL POSITION = " +
                cam.transform.localPosition);
        }

        Debug.Log("--------------------------------------------------");
        Debug.Log("[COMMANDER SCALE] NEARBY COLLIDERS");

        Collider[] colliders =
            Object.FindObjectsByType<Collider>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

        int found = 0;

        foreach (Collider col in colliders)
        {
            if (col.transform.IsChildOf(commander.transform))
                continue;

            Bounds b = col.bounds;

            float distance =
                Vector3.Distance(
                    playerPos,
                    b.ClosestPoint(playerPos)
                );

            if (distance > 20f)
                continue;

            found++;

            Debug.Log(
                "[GEOMETRY] " +
                col.gameObject.name +
                " | DIST=" + distance.ToString("F2") +
                " | BOTTOM Y=" + b.min.y.ToString("F2") +
                " | TOP Y=" + b.max.y.ToString("F2") +
                " | HEIGHT=" + b.size.y.ToString("F2") +
                " | CENTER=" + b.center
            );
        }

        Debug.Log(
            "[COMMANDER SCALE] COLLIDERS WITHIN 20M = " +
            found
        );

        Debug.Log("==================================================");
        Debug.Log("[COMMANDER SCALE AUDIT] COMPLETE");
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
}
