using UnityEngine;

namespace Obsidian.Vehicles
{
    [RequireComponent(typeof(Rigidbody))]
    public class VehicleMoveToPoint : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float maxSpeed = 6.94f;
        [SerializeField] private float turnSpeed = 5f;
        [SerializeField] private float stoppingDistance = 1.5f;

        [Header("Terrain")]
        [SerializeField] private float fullSpeedAngle = 20f;
        [SerializeField] private float maximumAngle = 45f;
        [SerializeField] private float minimumClimbSpeed = 1.5f;

        [Header("Ground Detection")]
        [SerializeField] private float groundRayStartHeight = 5f;
        [SerializeField] private float groundRayDistance = 15f;

        private Rigidbody rb;

        private Vector3 destination;
        private bool moving;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();

            rb.mass = 25000f;
            rb.useGravity = true;
            rb.isKinematic = false;

            rb.interpolation =
                RigidbodyInterpolation.Interpolate;

            rb.collisionDetectionMode =
                CollisionDetectionMode.Continuous;

            rb.constraints =
                RigidbodyConstraints.FreezeRotationX |
                RigidbodyConstraints.FreezeRotationZ;
        }

        public void MoveTo(Vector3 point)
        {
            destination = point;
            moving = true;
        }

        private void FixedUpdate()
        {
            if (!moving)
                return;

            Vector3 direction =
                destination - rb.position;

            direction.y = 0f;

            float distance =
                direction.magnitude;

            if (distance <= stoppingDistance)
            {
                StopVehicle();
                return;
            }

            direction.Normalize();

            RotateTowards(direction);

            float slope =
                GetGroundSlope();

            if (slope > maximumAngle)
            {
                StopVehicle();
                return;
            }

            float speed =
                GetSpeedForSlope(slope);

            Vector3 velocity =
                rb.linearVelocity;

            velocity.x =
                direction.x * speed;

            velocity.z =
                direction.z * speed;

            rb.linearVelocity =
                velocity;
        }

        private void RotateTowards(Vector3 direction)
        {
            if (direction.sqrMagnitude <= 0.001f)
                return;

            Quaternion targetRotation =
                Quaternion.LookRotation(
                    direction,
                    Vector3.up
                );

            Quaternion newRotation =
                Quaternion.RotateTowards(
                    rb.rotation,
                    targetRotation,
                    turnSpeed * Time.fixedDeltaTime * 60f
                );

            rb.MoveRotation(newRotation);
        }

        private float GetSpeedForSlope(float slope)
        {
            if (slope <= fullSpeedAngle)
                return maxSpeed;

            float amount =
                Mathf.InverseLerp(
                    fullSpeedAngle,
                    maximumAngle,
                    slope
                );

            return Mathf.Lerp(
                maxSpeed,
                minimumClimbSpeed,
                amount
            );
        }

        private float GetGroundSlope()
        {
            Vector3 origin =
                rb.position +
                Vector3.up *
                groundRayStartHeight;

            if (Physics.Raycast(
                    origin,
                    Vector3.down,
                    out RaycastHit hit,
                    groundRayDistance,
                    ~0,
                    QueryTriggerInteraction.Ignore))
            {
                if (hit.rigidbody == rb)
                    return 0f;

                return Vector3.Angle(
                    hit.normal,
                    Vector3.up
                );
            }

            return 0f;
        }

        private void StopVehicle()
        {
            moving = false;

            Vector3 velocity =
                rb.linearVelocity;

            velocity.x = 0f;
            velocity.z = 0f;

            rb.linearVelocity =
                velocity;
        }
    }
}