using UnityEngine;

namespace Obsidian.Vehicles
{
    public class VehicleWheelVisuals : MonoBehaviour
    {
        [Header("Wheel Meshes")]
        [SerializeField] private Transform frontLeft;
        [SerializeField] private Transform frontRight;
        [SerializeField] private Transform middleLeft;
        [SerializeField] private Transform middleRight;
        [SerializeField] private Transform rearLeft;
        [SerializeField] private Transform rearRight;

        [Header("Wheel Probes")]
        [SerializeField] private Transform probeFL;
        [SerializeField] private Transform probeFR;
        [SerializeField] private Transform probeML;
        [SerializeField] private Transform probeMR;
        [SerializeField] private Transform probeRL;
        [SerializeField] private Transform probeRR;

        [Header("Ground")]
        [SerializeField] private float rayHeight = 1f;
        [SerializeField] private float rayDistance = 2f;

        private Vector3 flStart;
        private Vector3 frStart;
        private Vector3 mlStart;
        private Vector3 mrStart;
        private Vector3 rlStart;
        private Vector3 rrStart;

        private void Awake()
        {
            flStart = frontLeft.position;
            frStart = frontRight.position;

            mlStart = middleLeft.position;
            mrStart = middleRight.position;

            rlStart = rearLeft.position;
            rrStart = rearRight.position;
        }

        private void LateUpdate()
        {
            UpdateWheel(frontLeft, probeFL, flStart);
            UpdateWheel(frontRight, probeFR, frStart);

            UpdateWheel(middleLeft, probeML, mlStart);
            UpdateWheel(middleRight, probeMR, mrStart);

            UpdateWheel(rearLeft, probeRL, rlStart);
            UpdateWheel(rearRight, probeRR, rrStart);
        }

        private void UpdateWheel(
            Transform wheel,
            Transform probe,
            Vector3 startPosition)
        {
            if (wheel == null || probe == null)
                return;

            Vector3 rayStart =
                probe.position +
                Vector3.up * rayHeight;

            if (!Physics.Raycast(
                    rayStart,
                    Vector3.down,
                    out RaycastHit hit,
                    rayDistance,
                    ~0,
                    QueryTriggerInteraction.Ignore))
            {
                return;
            }

            if (hit.transform.IsChildOf(transform))
                return;

            float groundDelta =
                hit.point.y - probe.position.y;

            Vector3 targetPosition =
                startPosition;

            targetPosition.y += groundDelta;

            wheel.position = targetPosition;
        }
    }
}