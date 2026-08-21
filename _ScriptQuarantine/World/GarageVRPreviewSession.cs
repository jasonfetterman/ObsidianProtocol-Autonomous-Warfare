using UnityEngine;

namespace ObsidianProtocol.World
{
    public class GarageVRPreviewSession : MonoBehaviour
    {
        [Header("Systems")]
        [SerializeField]
        private GarageVRPreviewLocator locator;

        [SerializeField]
        private GarageVRPreviewController controller;

        [SerializeField]
        private WorldControlManager controlManager;

        [Header("Session")]
        [SerializeField]
        private bool previewActive;

        public bool IsActive =>
            previewActive;

        public bool IsReady =>
            locator != null &&
            controller != null &&
            locator.IsReady;

        public bool Enter()
        {
            if (!IsReady)
            {
                Debug.LogWarning(
                    "GarageVRPreviewSession: Preview is not ready.");

                return false;
            }

            if (!locator.SupportsVR())
            {
                Debug.LogWarning(
                    "GarageVRPreviewSession: VR is not enabled for this entry point.");

                return false;
            }

            if (controlManager != null)
            {
                if (!controlManager.SetMode(
                        WorldControlMode.VR))
                    return false;
            }

            if (!controller.EnterPreview())
                return false;

            previewActive = true;

            Debug.Log(
                "GARAGE VR PREVIEW ACTIVE.");

            return true;
        }

        public void Exit()
        {
            if (!previewActive)
                return;

            if (controller != null)
                controller.ExitPreview();

            if (controlManager != null)
            {
                controlManager.SetMode(
                    WorldControlMode.RTS);
            }

            previewActive = false;

            Debug.Log(
                "GARAGE VR PREVIEW CLOSED.");
        }
    }
}
