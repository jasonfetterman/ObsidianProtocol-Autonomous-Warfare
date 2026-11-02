using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem.XR;

public class CommandCenterPlayerMode : MonoBehaviour
{
    public GameObject desktopPlayer;
    public GameObject xrOrigin;

    private void Awake()
    {
        bool vrActive = XRSettings.isDeviceActive;

        if (xrOrigin != null)
            xrOrigin.SetActive(true);

        if (desktopPlayer != null)
            desktopPlayer.SetActive(false);

        if (xrOrigin != null)
        {
            var movement = xrOrigin.GetComponent<CommandCenterWalkability>();
            if (movement != null)
                movement.enabled = !vrActive;

            var camera = xrOrigin.GetComponentInChildren<Camera>(true);
            if (camera != null)
            {
                var trackedPose = camera.GetComponent<TrackedPoseDriver>();
                if (trackedPose != null)
                    trackedPose.enabled = vrActive;
            }
        }
    }
}
