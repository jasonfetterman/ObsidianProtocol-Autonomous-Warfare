using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SixWheelGroundController : MonoBehaviour
{
    [Header("Vehicle")]
    [Tooltip("Vehicle mass in kilograms.")]
    [SerializeField] private float vehicleWeight = 1500f;

    [Tooltip("Maximum vehicle speed in meters per second.")]
    [SerializeField] private float maxSpeed = 25f;

    [Tooltip("Approximate vehicle height from ground to top.")]
    [SerializeField] private float vehicleHeight = 1.8f;

    [Header("Wheel Setup")]
    [Tooltip("Radius of each wheel in meters.")]
    [SerializeField] private float wheelRadius = 0.35f;

    [Tooltip("Maximum suspension travel below each wheel mount.")]
    [SerializeField] private float suspensionDistance = 0.25f;

    [Tooltip("How strongly the vehicle is held against the ground.")]
    [SerializeField] private float groundStickForce = 5000f;

    [Tooltip("How strongly the suspension pushes the vehicle upward.")]
    [SerializeField] private float suspensionStrength = 35000f;

    [Tooltip("Suspension damping.")]
    [SerializeField] private float suspensionDamping = 4500f;

    [Header("Slope Speed")]
    [Tooltip("Slope angle at which the vehicle can still use full speed.")]
    [SerializeField] private float fullSpeedSlope = 15f;

    [Tooltip("Slope angle at which the vehicle reaches its minimum speed.")]
    [SerializeField] private float slowSlope = 35f;

    [Tooltip("Minimum percentage of maximum speed on very steep slopes.")]
    [Range(0f, 1f)]
    [SerializeField] private float minimumSlopeSpeed = 0.15f;

    [Header("Movement")]
    [Tooltip("Acceleration toward the requested speed.")]
    [SerializeField] private float acceleration = 12f;

    [Tooltip("Braking force when slowing down.")]
    [SerializeField] private float braking = 20f;

    [Tooltip("How strongly the vehicle resists sideways sliding.")]
    [SerializeField] private float lateralGrip = 8f;

    [Header("Wheel Transforms")]
    [SerializeField] private Transform frontLeftWheel;
    [SerializeField] private Transform frontRightWheel;

    [SerializeField] private Transform middleLeftWheel;
    [SerializeField] private Transform middleRightWheel;

    [SerializeField] private Transform rearLeftWheel;
    [SerializeField] private Transform rearRightWheel;

    [Header("Ground Detection")]
    [SerializeField] private LayerMask groundLayers = ~0;

    [Tooltip("Maximum distance used for wheel ground detection.")]
    [SerializeField] private float groundRayLength = 1.0f;

    [Header("Debug")]
    [SerializeField] private bool drawDebugRays = true;

    private Rigidbody rb;

    private readonly WheelContact[] wheels = new WheelContact[6];

    private float requestedThrottle;

    private struct WheelContact
    {
        public Transform transform;
        public bool grounded;
        public RaycastHit hit;
        public float compression;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.mass = vehicleWeight;

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        wheels[0].transform = frontLeftWheel;
        wheels[1].transform = frontRightWheel;

        wheels[2].transform = middleLeftWheel;
        wheels[3].transform = middleRightWheel;

        wheels[4].transform = rearLeftWheel;
        wheels[5].transform = rearRightWheel;
    }

    private void FixedUpdate()
    {
        UpdateWheelContacts();
        ApplySuspension();
        ApplyGroundStick();
        ApplyMovement();
        ApplyLateralGrip();
        LimitSpeed();
    }

    /// <summary>
    /// Set throttle from -1 to +1.
    /// +1 = forward
    ///  0 = stop
    /// -1 = reverse
    /// </summary>
    public void SetThrottle(float throttle)
    {
        requestedThrottle = Mathf.Clamp(throttle, -1f, 1f);
    }

    private void UpdateWheelContacts()
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            Transform wheel = wheels[i].transform;

            if (wheel == null)
            {
                wheels[i].grounded = false;
                continue;
            }

            Vector3 rayStart = wheel.position + transform.up * 0.05f;

            float rayDistance =
                wheelRadius +
                suspensionDistance +
                groundRayLength;

            if (Physics.Raycast(
                rayStart,
                -transform.up,
                out RaycastHit hit,
                rayDistance,
                groundLayers,
                QueryTriggerInteraction.Ignore))
            {
                wheels[i].grounded = true;
                wheels[i].hit = hit;

                float distanceToGround =
                    hit.distance - wheelRadius;

                wheels[i].compression =
                    1f - Mathf.Clamp01(
                        distanceToGround / suspensionDistance);

                if (drawDebugRays)
                {
                    Debug.DrawRay(
                        rayStart,
                        -transform.up * hit.distance,
                        Color.green);
                }
            }
            else
            {
                wheels[i].grounded = false;
                wheels[i].compression = 0f;

                if (drawDebugRays)
                {
                    Debug.DrawRay(
                        rayStart,
                        -transform.up * rayDistance,
                        Color.red);
                }
            }
        }
    }

    private void ApplySuspension()
    {
        int groundedCount = 0;

        for (int i = 0; i < wheels.Length; i++)
        {
            if (!wheels[i].grounded)
                continue;

            groundedCount++;

            Transform wheel = wheels[i].transform;

            Vector3 velocityAtWheel =
                rb.GetPointVelocity(wheel.position);

            float verticalVelocity =
                Vector3.Dot(velocityAtWheel, transform.up);

            float springForce =
                wheels[i].compression * suspensionStrength;

            float dampingForce =
                -verticalVelocity * suspensionDamping;

            float totalForce =
                springForce + dampingForce;

            totalForce = Mathf.Max(0f, totalForce);

            rb.AddForceAtPosition(
                transform.up * totalForce,
                wheel.position,
                ForceMode.Force);
        }
    }

    private void ApplyGroundStick()
    {
        int groundedCount = 0;

        for (int i = 0; i < wheels.Length; i++)
        {
            if (wheels[i].grounded)
                groundedCount++;
        }

        if (groundedCount == 0)
            return;

        /*
         * Keeps the vehicle planted when travelling over
         * uneven terrain or small bumps.
         */

        Vector3 velocity = rb.linearVelocity;

        float upwardVelocity =
            Vector3.Dot(velocity, transform.up);

        if (upwardVelocity > 0f)
        {
            rb.AddForce(
                -transform.up *
                upwardVelocity *
                groundStickForce,
                ForceMode.Force);
        }
    }

    private void ApplyMovement()
    {
        if (requestedThrottle == 0f)
            return;

        Vector3 averageNormal = Vector3.zero;
        int groundedCount = 0;

        for (int i = 0; i < wheels.Length; i++)
        {
            if (!wheels[i].grounded)
                continue;

            averageNormal += wheels[i].hit.normal;
            groundedCount++;
        }

        if (groundedCount == 0)
            return;

        averageNormal.Normalize();

        /*
         * Project forward movement onto the ground.
         * This prevents the vehicle from trying to drive
         * directly through the terrain on slopes.
         */

        Vector3 forward =
            Vector3.ProjectOnPlane(
                transform.forward,
                averageNormal).normalized;

        float slopeAngle =
            Vector3.Angle(
                averageNormal,
                Vector3.up);

        float slopeSpeedMultiplier =
            CalculateSlopeSpeedMultiplier(slopeAngle);

        float targetSpeed =
            maxSpeed *
            slopeSpeedMultiplier *
            Mathf.Abs(requestedThrottle);

        float currentForwardSpeed =
            Vector3.Dot(rb.linearVelocity, forward);

        float speedDifference =
            targetSpeed - currentForwardSpeed;

        float forceAmount;

        if (speedDifference >= 0f)
        {
            forceAmount =
                acceleration *
                Mathf.Abs(speedDifference);
        }
        else
        {
            forceAmount =
                braking *
                Mathf.Abs(speedDifference);
        }

        forceAmount =
            Mathf.Clamp(
                forceAmount,
                0f,
                acceleration * maxSpeed);

        Vector3 driveForce =
            forward *
            forceAmount *
            Mathf.Sign(requestedThrottle) *
            rb.mass;

        rb.AddForce(
            driveForce,
            ForceMode.Force);
    }

    private float CalculateSlopeSpeedMultiplier(float slopeAngle)
    {
        if (slopeAngle <= fullSpeedSlope)
            return 1f;

        if (slopeAngle >= slowSlope)
            return minimumSlopeSpeed;

        float t =
            Mathf.InverseLerp(
                fullSpeedSlope,
                slowSlope,
                slopeAngle);

        return Mathf.Lerp(
            1f,
            minimumSlopeSpeed,
            t);
    }

    private void ApplyLateralGrip()
    {
        Vector3 velocity = rb.linearVelocity;

        Vector3 sideways =
            transform.right *
            Vector3.Dot(
                velocity,
                transform.right);

        Vector3 gripForce =
            -sideways *
            lateralGrip *
            rb.mass;

        rb.AddForce(
            gripForce,
            ForceMode.Force);
    }

    private void LimitSpeed()
    {
        Vector3 horizontalVelocity =
            Vector3.ProjectOnPlane(
                rb.linearVelocity,
                transform.up);

        if (horizontalVelocity.magnitude <= maxSpeed)
            return;

        Vector3 limitedVelocity =
            horizontalVelocity.normalized *
            maxSpeed;

        Vector3 verticalVelocity =
            Vector3.Project(
                rb.linearVelocity,
                transform.up);

        rb.linearVelocity =
            limitedVelocity +
            verticalVelocity;
    }

    public float GetCurrentSlope()
    {
        Vector3 averageNormal = Vector3.zero;
        int groundedCount = 0;

        for (int i = 0; i < wheels.Length; i++)
        {
            if (!wheels[i].grounded)
                continue;

            averageNormal += wheels[i].hit.normal;
            groundedCount++;
        }

        if (groundedCount == 0)
            return 0f;

        averageNormal.Normalize();

        return Vector3.Angle(
            averageNormal,
            Vector3.up);
    }

    public float GetSlopeSpeedMultiplier()
    {
        return CalculateSlopeSpeedMultiplier(
            GetCurrentSlope());
    }

    public bool IsGrounded()
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            if (wheels[i].grounded)
                return true;
        }

        return false;
    }

    private void OnValidate()
    {
        vehicleWeight = Mathf.Max(1f, vehicleWeight);
        maxSpeed = Mathf.Max(0f, maxSpeed);
        vehicleHeight = Mathf.Max(0.01f, vehicleHeight);

        wheelRadius = Mathf.Max(0.01f, wheelRadius);
        suspensionDistance = Mathf.Max(0.01f, suspensionDistance);
        groundStickForce = Mathf.Max(0f, groundStickForce);
        suspensionStrength = Mathf.Max(0f, suspensionStrength);
        suspensionDamping = Mathf.Max(0f, suspensionDamping);

        fullSpeedSlope =
            Mathf.Clamp(fullSpeedSlope, 0f, 89f);

        slowSlope =
            Mathf.Clamp(
                slowSlope,
                fullSpeedSlope,
                89f);

        acceleration = Mathf.Max(0f, acceleration);
        braking = Mathf.Max(0f, braking);
        lateralGrip = Mathf.Max(0f, lateralGrip);
        groundRayLength = Mathf.Max(0.01f, groundRayLength);
    }

    private void OnDrawGizmosSelected()
    {
        DrawWheelGizmo(frontLeftWheel);
        DrawWheelGizmo(frontRightWheel);

        DrawWheelGizmo(middleLeftWheel);
        DrawWheelGizmo(middleRightWheel);

        DrawWheelGizmo(rearLeftWheel);
        DrawWheelGizmo(rearRightWheel);
    }

    private void DrawWheelGizmo(Transform wheel)
    {
        if (wheel == null)
            return;

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            wheel.position,
            wheelRadius);

        Gizmos.DrawLine(
            wheel.position,
            wheel.position -
            transform.up *
            (wheelRadius + suspensionDistance));
    }
}