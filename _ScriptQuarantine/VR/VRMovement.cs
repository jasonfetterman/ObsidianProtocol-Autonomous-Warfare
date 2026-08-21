using UnityEngine;
using UnityEngine.InputSystem;

public class VRMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    private CharacterController characterController;
    private Transform cameraTransform;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        if (characterController == null || cameraTransform == null)
            return;

        Vector2 input = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed)
                input.y += 1f;

            if (Keyboard.current.sKey.isPressed)
                input.y -= 1f;

            if (Keyboard.current.dKey.isPressed)
                input.x += 1f;

            if (Keyboard.current.aKey.isPressed)
                input.x -= 1f;
        }

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 movement =
            (forward * input.y + right * input.x) * moveSpeed;

        characterController.Move(movement * Time.deltaTime);
    }
}