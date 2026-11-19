using UnityEngine;

public class BattlefieldCameraController : MonoBehaviour
{
    public enum FlightMode { FPV, Stabilized }

    [Header("Flight Mode")]
    [SerializeField] private FlightMode flightMode = FlightMode.FPV;

    [Header("FPV Flight")]
    [SerializeField] private float fpvThrust = 55f;
    [SerializeField] private float fpvTiltAngle = 35f;
    [SerializeField] private float fpvTiltResponse = 100f;
    [SerializeField] private float fpvYawSpeed = 100f;
    [SerializeField] private float fpvDrag = 1.8f;

    [Header("Stabilized Flight")]
    [SerializeField] private float stabilizedSpeed = 30f;
    [SerializeField] private float stabilizedAcceleration = 90f;
    [SerializeField] private float stabilizedBraking = 180f;
    [SerializeField] private float stabilizedLookSpeed = 2f;

    [Header("Shared Flight")]
    [SerializeField] private float verticalThrust = 45f;
    [SerializeField] private float maxSpeed = 90f;
    [SerializeField] private float scrollAdjustment = 5f;

    [Header("Collision")]
    [SerializeField] private float collisionRadius = 0.8f;
    [SerializeField] private float collisionPadding = 0.08f;

    private Vector3 velocity;
    private float pitch;
    private float roll;
    private float yaw;
    private float lookPitch;
    private float lookYaw;

    private void Start()
    {
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        lookYaw = yaw;
        lookPitch = angles.x;
        pitch = 0f;
        roll = 0f;
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Tab))
            SwitchFlightMode();

        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) > 0.01f)
            maxSpeed = Mathf.Clamp(maxSpeed + scroll * scrollAdjustment, 5f, 180f);

        if (flightMode == FlightMode.FPV)
            UpdateFPV(dt);
        else
            UpdateStabilized(dt);

        MoveWithCollision(dt);
        ApplyTerrainClearance();
    }

    private void SwitchFlightMode()
    {
        flightMode = flightMode == FlightMode.FPV
            ? FlightMode.Stabilized
            : FlightMode.FPV;

        pitch = 0f;
        roll = 0f;
        velocity = Vector3.zero;

        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        lookYaw = yaw;
        lookPitch = angles.x;

        Debug.Log("[OPAW] Flight mode: " + flightMode);
    }

    private void UpdateFPV(float dt)
    {
        float pitchInput = 0f;
        float rollInput = 0f;
        float yawInput = 0f;

        if (Input.GetKey(KeyCode.W)) pitchInput = 1f;
        if (Input.GetKey(KeyCode.S)) pitchInput = -1f;
        if (Input.GetKey(KeyCode.A)) rollInput = 1f;
        if (Input.GetKey(KeyCode.D)) rollInput = -1f;
        if (Input.GetKey(KeyCode.Q)) yawInput = -1f;
        if (Input.GetKey(KeyCode.E)) yawInput = 1f;

        float targetPitch = pitchInput * fpvTiltAngle;
        float targetRoll = rollInput * fpvTiltAngle;

        pitch = Mathf.MoveTowards(pitch, targetPitch, fpvTiltResponse * dt);
        roll = Mathf.MoveTowards(roll, targetRoll, fpvTiltResponse * dt);

        yaw += yawInput * fpvYawSpeed * dt;

        if (Input.GetMouseButton(1))
        {
            yaw += Input.GetAxis("Mouse X") * stabilizedLookSpeed * 50f;
            lookPitch = Mathf.Clamp(
                lookPitch - Input.GetAxis("Mouse Y") * stabilizedLookSpeed,
                -85f, 85f);
        }
        else
        {
            lookPitch = pitch;
        }

        Quaternion bodyRotation = Quaternion.Euler(pitch, yaw, roll);
        transform.rotation = bodyRotation;

        Vector3 thrustDirection = bodyRotation * Vector3.up;
        velocity += thrustDirection * fpvThrust * dt;

        float verticalInput = 0f;
        if (Input.GetKey(KeyCode.Space)) verticalInput += 1f;
        if (Input.GetKey(KeyCode.C)) verticalInput -= 1f;

        velocity += Vector3.up * verticalInput * verticalThrust * dt;
        velocity = Vector3.MoveTowards(
            velocity, Vector3.zero, fpvDrag * dt);

        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
    }

    private void UpdateStabilized(float dt)
    {
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;

        if (Input.GetMouseButton(1))
        {
            yaw += Input.GetAxis("Mouse X") * stabilizedLookSpeed;
            lookPitch = Mathf.Clamp(
                lookPitch - Input.GetAxis("Mouse Y") * stabilizedLookSpeed,
                -85f, 85f);
        }

        transform.rotation = Quaternion.Euler(lookPitch, yaw, 0f);

        Vector3 input = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) input += transform.forward;
        if (Input.GetKey(KeyCode.S)) input -= transform.forward;
        if (Input.GetKey(KeyCode.D)) input += transform.right;
        if (Input.GetKey(KeyCode.A)) input -= transform.right;

        if (Input.GetKey(KeyCode.Space)) input += Vector3.up;
        if (Input.GetKey(KeyCode.C)) input -= Vector3.up;

        if (input.sqrMagnitude > 1f)
            input.Normalize();

        Vector3 targetVelocity = input * stabilizedSpeed;
        float response = input.sqrMagnitude > 0f
            ? stabilizedAcceleration
            : stabilizedBraking;

        velocity = Vector3.MoveTowards(
            velocity, targetVelocity, response * dt);

        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
    }

    private void MoveWithCollision(float dt)
    {
        Vector3 displacement = velocity * dt;
        float distance = displacement.magnitude;

        if (distance < 0.0001f)
            return;

        Vector3 direction = displacement / distance;

        if (Physics.SphereCast(
            transform.position,
            collisionRadius,
            direction,
            out RaycastHit hit,
            distance + collisionPadding,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore))
        {
            float safeDistance = Mathf.Max(
                0f, hit.distance - collisionPadding);

            transform.position += direction * Mathf.Min(
                distance, safeDistance);

            velocity = Vector3.ProjectOnPlane(velocity, hit.normal);
        }
        else
        {
            transform.position += displacement;
        }
    }

    private void ApplyTerrainClearance()
    {
        const int terrainLayerMask = 1 << 6;
        Vector3 rayOrigin = transform.position + Vector3.up * 2000f;

        if (Physics.Raycast(
            rayOrigin,
            Vector3.down,
            out RaycastHit hit,
            4000f,
            terrainLayerMask,
            QueryTriggerInteraction.Ignore))
        {
            float minimumHeight = hit.point.y + collisionRadius + collisionPadding;

            if (transform.position.y < minimumHeight)
            {
                Vector3 correctedPosition = transform.position;
                correctedPosition.y = minimumHeight;
                transform.position = correctedPosition;

                if (velocity.y < 0f)
                    velocity.y = 0f;
            }
        }
    }
}

