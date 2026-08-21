using UnityEngine;

namespace ObsidianProtocol.World
{
    public class GarageVRPreviewController : MonoBehaviour
    {
        [Header("Preview Anchor")]
        [SerializeField]
        private GarageVRPreviewAnchor previewAnchor;

        [Header("VR Player")]
        [SerializeField]
        private Transform vrPlayerRoot;

        [Header("Settings")]
        [SerializeField]
        private bool alignPlayerOnStart;

        [SerializeField]
        private bool preservePlayerHeight = true;

        public bool IsReady =>
            previewAnchor != null &&
            vrPlayerRoot != null &&
            previewAnchor.EnabledForPreview;

        private void Start()
        {
            if (alignPlayerOnStart)
                EnterPreview();
        }

        public bool EnterPreview()
        {
            if (!IsReady)
            {
                Debug.LogWarning(
                    "GarageVRPreviewController: Preview is not ready.");

                return false;
            }

            Vector3 targetPosition =
                previewAnchor.Position;

            Quaternion targetRotation =
                previewAnchor.Rotation;

            if (preservePlayerHeight)
            {
                targetPosition.y =
                    vrPlayerRoot.position.y;
            }

            vrPlayerRoot.SetPositionAndRotation(
                targetPosition,
                targetRotation);

            Debug.Log(
                "GARAGE VR PREVIEW ENTRY COMPLETE.");

            return true;
        }

        public void ExitPreview()
        {
            Debug.Log(
                "GARAGE VR PREVIEW EXIT.");
        }

        public Vector3 GetLookTarget()
        {
            if (previewAnchor == null)
                return Vector3.zero;

            return previewAnchor.LookTarget;
        }
    }
}
