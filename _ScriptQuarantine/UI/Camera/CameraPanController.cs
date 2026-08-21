using UnityEngine;

public class CameraPanController : MonoBehaviour
{
    [Header("Pan")]
    public float panSpeed = 40f;
    public float panCollisionRadius = 3f;

    [Header("Zoom")]
    public float zoomSpeed = 80f;
    public float minHeight = 20f;
    public float maxHeight = 150f;
    public float groundClearance = 2f;

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * vertical + right * horizontal;

        if (move.sqrMagnitude > 1f)
            move.Normalize();

        // WASD movement with collision protection.
        if (move.sqrMagnitude > 0f)
        {
            float moveDistance = panSpeed * Time.deltaTime;
            Vector3 direction = move.normalized;

            if (!Physics.SphereCast(
                transform.position,
                panCollisionRadius,
                direction,
                out RaycastHit hit,
                moveDistance))
            {
                transform.position += move * moveDistance;
            }
        }

        // Mouse Wheel Zoom
        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");

        if (!Mathf.Approximately(scroll, 0f))
        {
            Vector3 position = transform.position;

            float targetHeight = position.y - (scroll * zoomSpeed);
            targetHeight = Mathf.Clamp(targetHeight, minHeight, maxHeight);

            // Find the terrain directly below the camera.
            Ray ray = new Ray(
                new Vector3(position.x, 10000f, position.z),
                Vector3.down
            );

            if (Physics.Raycast(ray, out RaycastHit hit, 20000f))
            {
                float minimumHeight = hit.point.y + groundClearance;
                targetHeight = Mathf.Max(targetHeight, minimumHeight);
            }

            position.y = targetHeight;
            transform.position = position;
        }
    }
}