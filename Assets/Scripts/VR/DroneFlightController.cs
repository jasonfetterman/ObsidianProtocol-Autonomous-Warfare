using UnityEngine;

public class DroneFlightController : MonoBehaviour
{
    public float thrust = 40f;
    public float turnSpeed = 90f;
    public float pitchSpeed = 70f;
    public float rollSpeed = 80f;

    public float maxSpeed = 120f;
    public float acceleration = 30f;
    public float deceleration = 20f;

    public float CurrentSpeed { get; private set; }
    public float Altitude => transform.position.y;
    public bool IsStalling => CurrentSpeed < 8f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearDamping = 0.1f;
        rb.angularDamping = 0.1f;
    }

    void Update()
    {
        HandleThrottle();
        HandleRotation();
    }

    private void HandleThrottle()
    {
        float input = Input.GetAxis("Vertical");

        if (input > 0f)
            CurrentSpeed += acceleration * Time.deltaTime;
        else if (input < 0f)
            CurrentSpeed -= deceleration * Time.deltaTime;

        CurrentSpeed = Mathf.Clamp(CurrentSpeed, 0f, maxSpeed);

        rb.linearVelocity = transform.forward * CurrentSpeed;
    }

    private void HandleRotation()
    {
        float yaw = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
        float pitch = -Input.GetAxis("Mouse Y") * pitchSpeed * Time.deltaTime;
        float roll = Input.GetAxis("Mouse X") * rollSpeed * Time.deltaTime;

        transform.Rotate(pitch, yaw, roll, Space.Self);
    }
}
