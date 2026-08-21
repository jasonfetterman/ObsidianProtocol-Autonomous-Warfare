using UnityEngine;

namespace Obsidian.Vehicles
{
    [RequireComponent(typeof(Rigidbody))]
    public class VehicleGrounding : MonoBehaviour
    {
        [Header("Wheel Probes")]
        [SerializeField] private Transform wheelProbeFL;
        [SerializeField] private Transform wheelProbeFR;

        [SerializeField] private Transform wheelProbeML;
        [SerializeField] private Transform wheelProbeMR;

        [SerializeField] private Transform wheelProbeRL;
        [SerializeField] private Transform wheelProbeRR;

        [Header("Suspension")]
        [SerializeField] private float suspensionTravel = 0.35f;
        [SerializeField] private float suspensionSpring = 180000f;
        [SerializeField] private float suspensionDamping = 30000f;

        [Header("Ground Detection")]
        [SerializeField] private float rayStartHeight = 0.4f;
        [SerializeField] private float rayDistance = 1.0f;

        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            ApplySuspension(wheelProbeFL);
            ApplySuspension(wheelProbeFR);

            ApplySuspension(wheelProbeML);
            ApplySuspension(wheelProbeMR);

            ApplySuspension(wheelProbeRL);
            ApplySuspension(wheelProbeRR);
        }

        private void ApplySuspension(Transform probe)
        {
            if (probe == null)
                return;

            Vector3 origin =
                probe.position +
                Vector3.up * rayStartHeight;

            if (!Physics.Raycast(
                    origin,
                    Vector3.down,
                    out RaycastHit hit,
                    rayDistance,
                    ~0,
                    QueryTriggerInteraction.Ignore))
            {
                return;
            }

            // Never suspend against our own vehicle.
            if (hit.rigidbody == rb)
                return;

            float wheelHeight =
                hit.point.y;

            float probeHeight =
                probe.position.y;

            float distance =
                probeHeight - wheelHeight;

            float compression =
                Mathf.Clamp01(
                    (suspensionTravel - distance) /
                    suspensionTravel
                );

            if (compression <= 0f)
                return;

            Vector3 pointVelocity =
                rb.GetPointVelocity(
                    probe.position
                );

            float verticalVelocity =
                Vector3.Dot(
                    pointVelocity,
                    Vector3.up
                );

            float springForce =
                compression *
                suspensionSpring;

            float dampingForce =
                -verticalVelocity *
                suspensionDamping;

            float totalForce =
                springForce +
                dampingForce;

            if (totalForce <= 0f)
                return;

            rb.AddForceAtPosition(
                Vector3.up * totalForce,
                probe.position,
                ForceMode.Force
            );
        }
    }
}