using UnityEngine;
using UnityEngine.Serialization;
using ObsidianProtocol.Game.Units;

namespace ObsidianProtocol.Game.AirWarfare
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class ScoutEyeFlightBehaviour : MonoBehaviour, IUnitManualControl
    {
        [Header("Translation")]
        [SerializeField, Min(0.1f)] private float maximumSpeed = 20f;
        [SerializeField, Min(0.1f)] private float verticalSpeed = 8f;
        [SerializeField, Min(0.1f)] private float acceleration = 24f;
        [SerializeField, Min(0.1f)] private float deceleration = 32f;

        [Header("Roll")]
        [FormerlySerializedAs("turnRate")]
        [SerializeField, Min(1f)] private float rollRate = 90f;

        private Rigidbody body;
        private Vector2 moveInput;
        private float verticalInput;
        private float rollInput;
        private Vector3 horizontalVelocity;
        private float currentVerticalSpeed;
        private float pendingYawDegrees;
        private bool manualControlActive;

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
            body.useGravity = false;
            body.isKinematic = true;
            body.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            body.interpolation = RigidbodyInterpolation.Interpolate;
        }

        private void Update()
        {
            if (!manualControlActive)
                return;

            float forward = 0f;
            float strafe = 0f;

            if (Input.GetKey(KeyCode.W)) forward += 1f;
            if (Input.GetKey(KeyCode.S)) forward -= 1f;

            if (Input.GetKey(KeyCode.D)) strafe += 1f;
            if (Input.GetKey(KeyCode.A)) strafe -= 1f;

            moveInput = Vector2.ClampMagnitude(
                new Vector2(strafe, forward), 1f);

            verticalInput = 0f;
            if (Input.GetKey(KeyCode.Space)) verticalInput += 1f;
            if (Input.GetKey(KeyCode.LeftControl)) verticalInput -= 1f;

            // Q = roll left; E = roll right. No yaw input here.
            rollInput = 0f;
            if (Input.GetKey(KeyCode.Q)) rollInput -= 1f;
            if (Input.GetKey(KeyCode.E)) rollInput += 1f;
        }

        private void FixedUpdate()
        {
            if (!manualControlActive || body == null)
                return;

            float dt = Time.fixedDeltaTime;

            // Use the drone's heading projected onto the horizontal plane.
            Vector3 forward = Vector3.ProjectOnPlane(
                body.rotation * Vector3.down, Vector3.up);

            if (forward.sqrMagnitude < 0.001f)
                forward = Vector3.ProjectOnPlane(
                    body.rotation * Vector3.left, Vector3.up);

            if (forward.sqrMagnitude < 0.001f)
                forward = Vector3.forward;

            forward.Normalize();

            Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;

            Vector3 desiredHorizontalVelocity =
                (forward * moveInput.y + right * moveInput.x) * maximumSpeed;

            float horizontalResponse =
                moveInput.sqrMagnitude > 0.001f ? acceleration : deceleration;

            horizontalVelocity = Vector3.MoveTowards(
                horizontalVelocity,
                desiredHorizontalVelocity,
                horizontalResponse * dt);

            float desiredVerticalSpeed = verticalInput * verticalSpeed;
            float verticalResponse =
                Mathf.Abs(verticalInput) > 0.001f ? acceleration : deceleration;

            currentVerticalSpeed = Mathf.MoveTowards(
                currentVerticalSpeed,
                desiredVerticalSpeed,
                verticalResponse * dt);

            Vector3 displacement =
                (horizontalVelocity +
                 Vector3.up * currentVerticalSpeed) * dt;

            MoveWithCollision(displacement);

            // Apply mouse yaw around world-up, then Q/E roll around
            // the drone's local nose-to-tail axis.
            float yawDegrees = pendingYawDegrees;
            pendingYawDegrees = 0f;

            if (Mathf.Abs(yawDegrees) > 0.0001f ||
                Mathf.Abs(rollInput) > 0.001f)
            {
                Quaternion targetRotation = body.rotation;

                if (Mathf.Abs(yawDegrees) > 0.0001f)
                {
                    targetRotation =
                        Quaternion.AngleAxis(yawDegrees, Vector3.up) *
                        targetRotation;
                }

                if (Mathf.Abs(rollInput) > 0.001f)
                {
                    Quaternion localRoll = Quaternion.AngleAxis(
                        rollInput * rollRate * dt,
                        Vector3.up);

                    targetRotation = targetRotation * localRoll;
                }

                body.MoveRotation(targetRotation);
            }
        }

        public void QueueManualYaw(float yawDegrees)
        {
            if (manualControlActive)
                pendingYawDegrees += yawDegrees;
        }
        private void MoveWithCollision(Vector3 displacement)
        {
            if (body == null)
                return;

            float distance = displacement.magnitude;
            if (distance < 0.0001f)
                return;

            const float skinWidth = 0.03f;
            Vector3 direction = displacement / distance;

            // Sweep the drone collider before moving the kinematic Rigidbody.
            if (body.SweepTest(
                direction,
                out RaycastHit hit,
                distance + skinWidth,
                QueryTriggerInteraction.Ignore))
            {
                float safeDistance = Mathf.Clamp(
                    hit.distance - skinWidth,
                    0f,
                    distance);

                displacement = direction * safeDistance;
            }

            body.MovePosition(body.position + displacement);
        }
        public void EnterManualControl(Transform controller)
        {
            moveInput = Vector2.zero;
            verticalInput = 0f;
            rollInput = 0f;
            horizontalVelocity = Vector3.zero;
            currentVerticalSpeed = 0f;
            manualControlActive = true;
            enabled = true;

            Debug.Log("[SCOUT EYE] Manual flight controls enabled.");
        }

        public void ExitManualControl()
        {
            manualControlActive = false;
            moveInput = Vector2.zero;
            verticalInput = 0f;
            rollInput = 0f;
            horizontalVelocity = Vector3.zero;
            currentVerticalSpeed = 0f;
        }
    }
}









