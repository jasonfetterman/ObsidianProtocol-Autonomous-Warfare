using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class CommanderPlayerMovement : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionAsset inputActions;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -20f;

    [Header("Look")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensitivity = 3f;
    [SerializeField] private float maxLookAngle = 85f;

    private CharacterController characterController;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction sprintAction;
    private InputAction jumpAction;

    private float verticalVelocity;
    private float cameraPitch;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (inputActions == null)
        {
            Debug.LogError("Commander: Input Actions asset is missing.", this);
            enabled = false;
            return;
        }

        InputActionMap playerMap = inputActions.FindActionMap("Player", true);

        moveAction = playerMap.FindAction("Move", true);
        lookAction = playerMap.FindAction("Look", true);
        sprintAction = playerMap.FindAction("Sprint", true);
        jumpAction = playerMap.FindAction("Jump", true);

        if (cameraTransform == null)
        {
            Camera sceneCamera = GetComponentInChildren<Camera>();

            if (sceneCamera != null)
                cameraTransform = sceneCamera.transform;
        }

        if (cameraTransform == null)
        {
            Debug.LogError("Commander: Camera reference is missing.", this);
            enabled = false;
            return;
        }

        cameraPitch = cameraTransform.localEulerAngles.x;

        if (cameraPitch > 180f)
            cameraPitch -= 360f;
    }

    private void OnEnable()
    {
        if (moveAction == null)
            return;

        moveAction.Enable();
        lookAction.Enable();
        sprintAction.Enable();
        jumpAction.Enable();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        moveAction?.Disable();
        lookAction?.Disable();
        sprintAction?.Disable();
        jumpAction?.Disable();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            bool locked = Cursor.lockState == CursorLockMode.Locked;

            Cursor.lockState = locked
                ? CursorLockMode.None
                : CursorLockMode.Locked;

            Cursor.visible = locked;
        }

        HandleLook();
        HandleMovement();
    }

    private void HandleLook()
    {
        if (cameraTransform == null ||
            Cursor.lockState != CursorLockMode.Locked)
            return;

        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        transform.Rotate(
            Vector3.up,
            lookInput.x * mouseSensitivity * 0.02f
        );

        cameraPitch -= lookInput.y * mouseSensitivity * 0.02f;

        cameraPitch = Mathf.Clamp(
            cameraPitch,
            -maxLookAngle,
            maxLookAngle
        );

        cameraTransform.localRotation =
            Quaternion.Euler(cameraPitch, 0f, 0f);
    }

    private void HandleMovement()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();

        Vector3 direction =
            transform.right * input.x +
            transform.forward * input.y;

        if (direction.sqrMagnitude > 1f)
            direction.Normalize();

        float speed = sprintAction.IsPressed()
            ? sprintSpeed
            : moveSpeed;

        Vector3 movement = direction * speed;

        if (characterController.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        if (characterController.isGrounded && jumpAction.WasPressedThisFrame())
        {
            verticalVelocity = Mathf.Sqrt(
                jumpHeight * -2f * gravity
            );
        }

        verticalVelocity += gravity * Time.deltaTime;
        movement.y = verticalVelocity;

        characterController.Move(movement * Time.deltaTime);
    }
}