using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerLobbyPlayerController : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] public float lookSpeed = 2f;

    private CharacterController controller;
    private Camera playerCamera;
    private float verticalVelocity;
    private float pitch;
    private float startupMoveTimer;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (controller == null)
            controller = gameObject.AddComponent<CharacterController>();

        controller.height = 1.8f;
        controller.radius = 0.35f;
        controller.center = new Vector3(0f, 0.9f, 0f);
        controller.stepOffset = 0.3f;
        controller.slopeLimit = 50f;
        controller.skinWidth = 0.08f;

        playerCamera = GetComponentInChildren<Camera>(true);

        if (playerCamera == null)
        {
            GameObject cameraObject = new GameObject("CommanderCamera");
            cameraObject.transform.SetParent(transform, false);
            cameraObject.transform.localPosition = new Vector3(0f, 1.6f, 0f);
            cameraObject.transform.localRotation = Quaternion.identity;

            playerCamera = cameraObject.AddComponent<Camera>();
            cameraObject.AddComponent<AudioListener>();
        }

        playerCamera.gameObject.SetActive(true);
        playerCamera.tag = "MainCamera";

        startupMoveTimer = 2f;

        Debug.Log("[MULTIPLAYER MOVEMENT] PLAYER CONTROLLER ACTIVE");
    }

    private void Update()
    {
        if (startupMoveTimer > 0f)
        {
            startupMoveTimer -= Time.deltaTime;

            controller.Move(transform.forward * 2f * Time.deltaTime);

            return;
        }

        HandleLook();
        HandleMovement();
    }

    private void HandleLook()
    {
        if (Mouse.current == null)
            return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float yaw = mouseDelta.x * lookSpeed * 0.02f;
        float lookY = mouseDelta.y * lookSpeed * 0.02f;

        transform.Rotate(Vector3.up * yaw);

        pitch -= lookY;
        pitch = Mathf.Clamp(pitch, -85f, 85f);

        if (playerCamera != null)
            playerCamera.transform.localRotation =
                Quaternion.Euler(pitch, 0f, 0f);
    }
    private void HandleMovement()
    {
        if (Keyboard.current == null)
        {
            Debug.LogWarning("[MULTIPLAYER MOVEMENT] Keyboard.current IS NULL");
            return;
        }

        Vector2 input = Vector2.zero;

        if (Keyboard.current.wKey.isPressed)
            input.y += 1f;

        if (Keyboard.current.sKey.isPressed)
            input.y -= 1f;

        if (Keyboard.current.dKey.isPressed)
            input.x += 1f;

        if (Keyboard.current.aKey.isPressed)
            input.x -= 1f;

        input = Vector2.ClampMagnitude(input, 1f);

        bool sprint =
            Keyboard.current.leftShiftKey.isPressed ||
            Keyboard.current.rightShiftKey.isPressed;

        float speed = sprint ? moveSpeed * 2f : moveSpeed;

        Vector3 movement =
            transform.forward * input.y +
            transform.right * input.x;

        if (controller.isGrounded)
        {
            verticalVelocity = -2f;

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                verticalVelocity = 5f;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        movement *= speed;
        movement.y = verticalVelocity;

        controller.Move(movement * Time.deltaTime);
    }
}


