using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class BlackspireWalkController : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float sprintSpeed = 12f;
    public float gravity = -25f;
    public float jumpHeight = 1.5f;
    public float mouseSensitivity = 2f;

    private CharacterController controller;
    private Camera playerCamera;
    private float verticalVelocity;
    private float cameraPitch;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();

        if (playerCamera == null)
        {
            GameObject cameraObject = new GameObject("BLACKSPIRE_PLAYER_CAMERA");
            cameraObject.transform.SetParent(transform);
            cameraObject.transform.localPosition = new Vector3(0, 0.65f, 0);
            playerCamera = cameraObject.AddComponent<Camera>();
            cameraObject.tag = "MainCamera";
        }
    }

    void Update()
    {
        if (controller == null) return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 movement =
            transform.right * horizontal +
            transform.forward * vertical;

        movement = Vector3.ClampMagnitude(movement, 1f);

        float speed = Input.GetKey(KeyCode.LeftShift)
            ? sprintSpeed : moveSpeed;

        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        if (controller.isGrounded && Input.GetButtonDown("Jump"))
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = movement * speed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            transform.Rotate(Vector3.up * mouseX);

            cameraPitch -= mouseY;
            cameraPitch = Mathf.Clamp(cameraPitch, -85f, 85f);

            if (playerCamera != null)
                playerCamera.transform.localRotation =
                    Quaternion.Euler(cameraPitch, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None;

        if (Input.GetMouseButtonDown(0))
            Cursor.lockState = CursorLockMode.Locked;
    }
}
