using UnityEngine;

public class VehicleDropToGround : MonoBehaviour
{
    [Header("Drop Settings")]
    [SerializeField] private float dropHeight = 10f;
    [SerializeField] private float startingDownwardVelocity = 2f;

    [Header("Vehicle Physics")]
    [SerializeField] private float mass = 25000f;
    [SerializeField] private float linearDamping = 2f;
    [SerializeField] private float angularDamping = 5f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.useGravity = true;
        rb.isKinematic = false;

        rb.mass = mass;
        rb.linearDamping = linearDamping;
        rb.angularDamping = angularDamping;

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    private void Start()
    {
        transform.position += Vector3.up * dropHeight;

        rb.linearVelocity = Vector3.down * startingDownwardVelocity;
    }
}