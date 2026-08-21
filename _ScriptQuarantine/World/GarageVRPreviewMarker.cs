using UnityEngine;

namespace ObsidianProtocol.World
{
    public class GarageVRPreviewMarker : MonoBehaviour
    {
        [Header("Preview")]
        [SerializeField]
        private string markerId = "GARAGE_VR_MARKER_01";

        [SerializeField]
        private bool active = true;

        [Header("Display")]
        [SerializeField]
        private float markerRadius = 0.35f;

        public string MarkerId =>
            markerId;

        public bool Active =>
            active;

        public void SetActive(
            bool value)
        {
            active = value;
        }

        private void OnDrawGizmos()
        {
            if (!active)
                return;

            Gizmos.DrawWireSphere(
                transform.position,
                markerRadius);

            Gizmos.DrawLine(
                transform.position,
                transform.position +
                transform.forward);

            Gizmos.DrawLine(
                transform.position,
                transform.position +
                transform.right);

            Gizmos.DrawLine(
                transform.position,
                transform.position +
                transform.up);
        }
    }
}
