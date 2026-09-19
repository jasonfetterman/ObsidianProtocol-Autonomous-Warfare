using UnityEngine;
using UnityEngine.InputSystem;

public class RTSCameraController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 20f;

    [Header("Edge Scrolling")]
    [SerializeField] private float edgeScrollSpeed = 20f;
    [SerializeField] private float edgeSize = 10f;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float minHeight = 8f;
    [SerializeField] private float maxHeight = 40f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 90f;

    [Header("Bounds")]
    [SerializeField] private float minX = -250f;
    [SerializeField] private float maxX = 250f;
    [SerializeField] private float minZ = -250f;
    [SerializeField] private float maxZ = 250f;

    private void Update()
    {
        HandleMovement();
        HandleEdgeScrolling();
        HandleZoom();
        HandleRotation();
        ClampPosition();
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Keyboard.current.wKey.isPressed)
            moveDirection += transform.forward;

        if (Keyboard.current.sKey.isPressed)
            moveDirection -= transform.forward;

        if (Keyboard.current.aKey.isPressed)
            moveDirection -= transform.right;

        if (Keyboard.current.dKey.isPressed)
            moveDirection += transform.right;

        moveDirection.y = 0f;

        transform.position +=
            moveDirection.normalized *
            moveSpeed *
            Time.deltaTime;
    }

    private void HandleEdgeScrolling()
    {
        if (Mouse.current == null)
            return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Vector3 moveDirection = Vector3.zero;

        if (mousePosition.y >= Screen.height - edgeSize)
            moveDirection += transform.forward;

        if (mousePosition.y <= edgeSize)
            moveDirection -= transform.forward;

        if (mousePosition.x <= edgeSize)
            moveDirection -= transform.right;

        if (mousePosition.x >= Screen.width - edgeSize)
            moveDirection += transform.right;

        moveDirection.y = 0f;

        transform.position +=
            moveDirection.normalized *
            edgeScrollSpeed *
            Time.deltaTime;
    }

    private void HandleZoom()
    {
        float scroll = Mouse.current.scroll.ReadValue().y;

        if (Mathf.Approximately(scroll, 0f))
            return;

        Vector3 position = transform.position;

        position.y -= scroll * zoomSpeed * Time.deltaTime;
        position.y = Mathf.Clamp(position.y, minHeight, maxHeight);

        transform.position = position;
    }

    private void HandleRotation()
    {
        float rotation = 0f;

        if (Keyboard.current.qKey.isPressed)
            rotation = -1f;

        if (Keyboard.current.eKey.isPressed)
            rotation = 1f;

        transform.Rotate(
            Vector3.up,
            rotation * rotationSpeed * Time.deltaTime,
            Space.World);
    }

    private void ClampPosition()
    {
        Vector3 position = transform.position;

        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.z = Mathf.Clamp(position.z, minZ, maxZ);

        transform.position = position;
    }
}