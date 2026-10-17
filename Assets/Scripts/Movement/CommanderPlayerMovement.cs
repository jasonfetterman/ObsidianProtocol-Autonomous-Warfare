using UnityEngine;

public class CommanderPlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float sprintSpeed = 7f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -20f;

    [Header("Look")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float maxLookAngle = 85f;

    private CharacterController characterController;
    private float verticalVelocity;
    private float cameraPitch;
    private float debugTimer;
    private bool firstFrameLogged;

    private void Awake()
    {
        characterController =
            GetComponent<CharacterController>();

        if (characterController == null)
            characterController =
                gameObject.AddComponent<CharacterController>();

        if (cameraTransform == null)
        {
            Camera sceneCamera =
                GetComponentInChildren<Camera>();

            if (sceneCamera != null)
                cameraTransform =
                    sceneCamera.transform;
        }

        if (cameraTransform != null)
        {
            cameraTransform.localPosition =
                new Vector3(0f, 5.0f, 0f);

            cameraPitch = 16.35f;

            cameraTransform.localRotation =
                Quaternion.Euler(
                    cameraPitch,
                    0f,
                    0f);
        }

        verticalVelocity = 0f;

        Debug.Log(
            "[COMMANDER DEBUG] AWAKE PLAYER POSITION = " +
            transform.position);

        Debug.Log(
            "[COMMANDER DEBUG] AWAKE PLAYER SCALE = " +
            transform.lossyScale);

        Debug.Log(
            "[COMMANDER DEBUG] CC HEIGHT = " +
            characterController.height);

        Debug.Log(
            "[COMMANDER DEBUG] CC CENTER = " +
            characterController.center);

        Debug.Log(
            "[COMMANDER DEBUG] CC RADIUS = " +
            characterController.radius);

        if (cameraTransform != null)
        {
            Debug.Log(
                "[COMMANDER DEBUG] AWAKE CAMERA WORLD = " +
                cameraTransform.position);

            Debug.Log(
                "[COMMANDER DEBUG] AWAKE CAMERA LOCAL = " +
                cameraTransform.localPosition);
        }

        Cursor.lockState =
            CursorLockMode.Locked;

        Cursor.visible = false;
    }

    private void Update()
    {
        HandleLook();
        HandleMovement();

        if (!firstFrameLogged)
        {
            firstFrameLogged = true;

            Debug.Log(
                "[COMMANDER DEBUG] FIRST UPDATE PLAYER = " +
                transform.position);

            Debug.Log(
                "[COMMANDER DEBUG] FIRST UPDATE GROUNDED = " +
                characterController.isGrounded);

            if (cameraTransform != null)
            {
                Debug.Log(
                    "[COMMANDER DEBUG] FIRST UPDATE CAMERA = " +
                    cameraTransform.position);
            }
        }

        debugTimer += Time.deltaTime;

        if (debugTimer >= 1f)
        {
            debugTimer = 0f;

            Debug.Log(
                "[COMMANDER DEBUG] 1 SECOND PLAYER = " +
                transform.position);

            Debug.Log(
                "[COMMANDER DEBUG] 1 SECOND GROUNDED = " +
                characterController.isGrounded);

            Debug.Log(
                "[COMMANDER DEBUG] VERTICAL VELOCITY = " +
                verticalVelocity);

            if (cameraTransform != null)
            {
                Debug.Log(
                    "[COMMANDER DEBUG] 1 SECOND CAMERA = " +
                    cameraTransform.position);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState =
                Cursor.lockState == CursorLockMode.Locked
                    ? CursorLockMode.None
                    : CursorLockMode.Locked;

            Cursor.visible =
                Cursor.lockState != CursorLockMode.Locked;
        }
    }

    private void HandleLook()
    {
        if (cameraTransform == null)
            return;

        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        float mouseX =
            Input.GetAxis("Mouse X") *
            mouseSensitivity;

        float mouseY =
            Input.GetAxis("Mouse Y") *
            mouseSensitivity;

        transform.Rotate(
            Vector3.up,
            mouseX);

        cameraPitch -= mouseY;

        cameraPitch =
            Mathf.Clamp(
                cameraPitch,
                -maxLookAngle,
                maxLookAngle);

        cameraTransform.localRotation =
            Quaternion.Euler(
                cameraPitch,
                0f,
                0f);
    }

    private void HandleMovement()
    {
        if (characterController == null)
            return;

        float horizontal =
            Input.GetAxisRaw("Horizontal");

        float vertical =
            Input.GetAxisRaw("Vertical");

        Vector3 direction =
            transform.right * horizontal +
            transform.forward * vertical;

        if (direction.sqrMagnitude > 1f)
            direction.Normalize();

        float speed =
            Input.GetKey(KeyCode.LeftShift)
                ? sprintSpeed
                : moveSpeed;

        Vector3 movement =
            direction * speed;

        if (characterController.isGrounded &&
            verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        if (characterController.isGrounded &&
            Input.GetKeyDown(KeyCode.Space))
        {
            verticalVelocity =
                Mathf.Sqrt(
                    jumpHeight *
                    -2f *
                    gravity);
        }

        verticalVelocity +=
            gravity *
            Time.deltaTime;

        movement.y =
            verticalVelocity;

        characterController.Move(
            movement *
            Time.deltaTime);
    }
}
