using UnityEngine;

public class MinimapCameraFollow : MonoBehaviour
{
    [Header("Camera To Follow")]
    [SerializeField] private Transform targetCamera;

    [Header("Height")]
    [SerializeField] private float height = 500f;

    [Header("Follow")]
    [SerializeField] private bool followX = true;
    [SerializeField] private bool followZ = true;

    private void LateUpdate()
    {
        if (targetCamera == null)
            return;

        Vector3 newPosition = transform.position;

        if (followX)
            newPosition.x = targetCamera.position.x;

        if (followZ)
            newPosition.z = targetCamera.position.z;

        newPosition.y = height;

        transform.position = newPosition;
    }
}