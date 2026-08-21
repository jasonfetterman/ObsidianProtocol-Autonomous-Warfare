using UnityEngine;

public class CameraEdgeScroll : MonoBehaviour
{
    [Header("Edge Scroll")]
    [SerializeField] private float scrollSpeed = 40f;
    [SerializeField] private float edgeSize = 20f;

    [Header("Settings")]
    [SerializeField] private bool useEdgeScroll = true;

    private void Update()
    {
        if (!useEdgeScroll)
            return;

        Vector3 mousePosition = Input.mousePosition;

        if (mousePosition.x < 0f ||
            mousePosition.x > Screen.width ||
            mousePosition.y < 0f ||
            mousePosition.y > Screen.height)
        {
            return;
        }

        Vector3 move = Vector3.zero;

        if (mousePosition.x <= edgeSize)
            move.x -= 1f;
        else if (mousePosition.x >= Screen.width - edgeSize)
            move.x += 1f;

        if (mousePosition.y <= edgeSize)
            move.z -= 1f;
        else if (mousePosition.y >= Screen.height - edgeSize)
            move.z += 1f;

        if (move.sqrMagnitude > 0f)
        {
            move.Normalize();
            transform.position += move * scrollSpeed * Time.deltaTime;
        }
    }
}