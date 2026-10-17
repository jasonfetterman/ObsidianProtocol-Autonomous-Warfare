using UnityEditor;
using UnityEngine;

public static class DeploymentCameraHeightFix
{
    [MenuItem("Obsidian Protocol/Deployment/Fix Camera Height")]
    public static void Fix()
    {
        GameObject player = GameObject.Find("[PLAYER] COMMANDER");

        if (player == null)
        {
            Debug.LogError("[CAMERA FIX] [PLAYER] COMMANDER not found.");
            return;
        }

        Transform camera = player.transform.Find("DEPLOYMENT CAMERA");

        if (camera == null)
        {
            Debug.LogError("[CAMERA FIX] DEPLOYMENT CAMERA not found.");
            return;
        }

        Vector3 local = camera.localPosition;
        local.y = 1.80f;
        camera.localPosition = local;

        EditorUtility.SetDirty(camera);

        Debug.Log("[CAMERA FIX] DEPLOYMENT CAMERA local position set to " + camera.localPosition);
    }
}

