using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(CharacterController))]
public class CommandCenterWalkability : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpHeight = 1.2f;
    public float gravity = -20f;

    [Header("Mouse Look")]
    public Transform viewCamera;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 85f;

    private CharacterController characterController;
    private float verticalVelocity;
    private float cameraPitch;
    private bool cursorLocked;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (viewCamera == null && Camera.main != null)
            viewCamera = Camera.main.transform;

        if (viewCamera != null)
        {
            cameraPitch = viewCamera.localEulerAngles.x;
            if (cameraPitch > 180f)
                cameraPitch -= 360f;
        }

        if (!XRSettings.isDeviceActive)
            SetCursorLock(true);
    }

    private void Update()
    {
        if (XRSettings.isDeviceActive)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
            SetCursorLock(!cursorLocked);

        if (cursorLocked)
            HandleMouseLook();

        HandleMovement();
    }

    private void HandleMouseLook()
    {
        if (viewCamera == null)
            return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(0f, mouseX, 0f, Space.Self);

        cameraPitch = Mathf.Clamp(
            cameraPitch - mouseY,
            -maxLookAngle,
            maxLookAngle
        );

        viewCamera.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }

    private void HandleMovement()
    {
        if (characterController == null || !characterController.enabled)
            return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction =
            (transform.right * horizontal + transform.forward * vertical).normalized;

        Vector3 motion = direction * moveSpeed;

        if (characterController.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        if (characterController.isGrounded &&
            Input.GetKeyDown(KeyCode.Space))
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

        verticalVelocity += gravity * Time.deltaTime;
        motion.y = verticalVelocity;

        characterController.Move(motion * Time.deltaTime);
    }

    private void SetCursorLock(bool locked)
    {
        cursorLocked = locked;
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
