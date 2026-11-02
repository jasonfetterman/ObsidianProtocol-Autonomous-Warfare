using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public sealed class LogisticsFirstPersonController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintSpeed = 9f;
    public float mouseSensitivity = 0.12f;
    public float gravity = -20f;
    public float jumpSpeed = 4.5f;

    private CharacterController controller;
    private Transform playerCamera;
    private float pitch;
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        controller.height = 1.8f;
        controller.radius = 0.35f;
        controller.center = new Vector3(0f, 0.9f, 0f);
        controller.minMoveDistance = 0f;

        Camera cam = GetComponentInChildren<Camera>();
        if (cam != null)
            playerCamera = cam.transform;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Keyboard.current == null || Mouse.current == null || playerCamera == null)
            return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Vector2 look = Mouse.current.delta.ReadValue() * mouseSensitivity;
            pitch = Mathf.Clamp(pitch - look.y, -85f, 85f);
            transform.Rotate(0f, look.x, 0f);
            playerCamera.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }

        float x = (Keyboard.current.dKey.isPressed ? 1f : 0f)
                - (Keyboard.current.aKey.isPressed ? 1f : 0f);
        float z = (Keyboard.current.wKey.isPressed ? 1f : 0f)
                - (Keyboard.current.sKey.isPressed ? 1f : 0f);

        Vector3 horizontal = (transform.right * x + transform.forward * z).normalized;
        float speed = Keyboard.current.leftShiftKey.isPressed ? sprintSpeed : moveSpeed;

        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        if (controller.isGrounded && Keyboard.current.spaceKey.wasPressedThisFrame)
            verticalVelocity = jumpSpeed;

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 finalMove = horizontal * speed;
        finalMove.y = verticalVelocity;
        controller.Move(finalMove * Time.deltaTime);
    }
}
