using UnityEngine;

namespace ObsidianProtocol.World
{
    public class GarageVRPreviewAnchor : MonoBehaviour
    {
        [Header("Preview")]
        [SerializeField]
        private string previewId = "GARAGE_VR_PREVIEW_01";

        [SerializeField]
        private bool enabledForPreview = true;

        [Header("Spawn")]
        [SerializeField]
        private Transform playerAnchor;

        [Header("View Direction")]
        [SerializeField]
        private Transform lookTarget;

        public string PreviewId =>
            previewId;

        public bool EnabledForPreview =>
            enabledForPreview;

        public Vector3 Position =>
            playerAnchor != null
                ? playerAnchor.position
                : transform.position;

        public Quaternion Rotation =>
            playerAnchor != null
                ? playerAnchor.rotation
                : transform.rotation;

        public Vector3 LookTarget =>
            lookTarget != null
                ? lookTarget.position
                : transform.position +
                  transform.forward * 5f;

        public void SetPreviewEnabled(
            bool value)
        {
            enabledForPreview = value;
        }

        private void OnDrawGizmos()
        {
            Vector3 position =
                playerAnchor != null
                    ? playerAnchor.position
                    : transform.position;

            Vector3 forward =
                playerAnchor != null
                    ? playerAnchor.forward
                    : transform.forward;

            Gizmos.DrawWireSphere(
                position,
                0.5f);

            Gizmos.DrawLine(
                position,
                position + forward * 3f);

            if (lookTarget != null)
            {
                Gizmos.DrawLine(
                    position,
                    lookTarget.position);
            }
        }
    }
}
