using UnityEngine;
using UnityEngine.InputSystem;

public class RTSCameraController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 35f;

    [Header("Edge Scrolling")]
    [SerializeField] private float edgeScrollSpeed = 35f;
    [SerializeField] private float edgeSize = 12f;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 20f;
    [SerializeField] private float minHeight = 30f;
    [SerializeField] private float maxHeight = 200f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 90f;

    [Header("Bounds - MAP 001 CARRIER GROUP")]
    [SerializeField] private float minX = 1000f;
    [SerializeField] private float maxX = 2024f;
    [SerializeField] private float minZ = 0f;
    [SerializeField] private float maxZ = 1024f;

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
        if (Keyboard.current == null)
            return;

        Vector3 direction = Vector3.zero;

        if (Keyboard.current.wKey.isPressed) direction += transform.forward;
        if (Keyboard.current.sKey.isPressed) direction -= transform.forward;
        if (Keyboard.current.aKey.isPressed) direction -= transform.right;
        if (Keyboard.current.dKey.isPressed) direction += transform.right;

        direction.y = 0f;

        transform.position += direction.normalized * moveSpeed * Time.deltaTime;
    }

    private void HandleEdgeScrolling()
    {
        if (Mouse.current == null)
            return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 direction = Vector3.zero;

        if (mousePosition.y >= Screen.height - edgeSize) direction += transform.forward;
        if (mousePosition.y <= edgeSize) direction -= transform.forward;
        if (mousePosition.x <= edgeSize) direction -= transform.right;
        if (mousePosition.x >= Screen.width - edgeSize) direction += transform.right;

        direction.y = 0f;

        transform.position += direction.normalized * edgeScrollSpeed * Time.deltaTime;
    }

    private void HandleZoom()
    {
        if (Mouse.current == null)
            return;

        float scroll = Mouse.current.scroll.ReadValue().y;

        if (Mathf.Approximately(scroll, 0f))
            return;

        Vector3 position = transform.position;
        position.y = Mathf.Clamp(position.y - scroll * zoomSpeed * Time.deltaTime, minHeight, maxHeight);
        transform.position = position;
    }

    private void HandleRotation()
    {
        if (Keyboard.current == null)
            return;

        float rotation = 0f;

        if (Keyboard.current.qKey.isPressed) rotation -= 1f;
        if (Keyboard.current.eKey.isPressed) rotation += 1f;

        transform.Rotate(Vector3.up, rotation * rotationSpeed * Time.deltaTime, Space.World);
    }

    private void ClampPosition()
    {
        Vector3 position = transform.position;

        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.z = Mathf.Clamp(position.z, minZ, maxZ);

        transform.position = position;
    }
}
