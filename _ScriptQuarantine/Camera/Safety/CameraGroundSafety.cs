using UnityEngine;

public class CameraGroundSafety : MonoBehaviour
{
    [SerializeField] private float groundClearance = 2f;
    [SerializeField] private float raycastDistance = 10000f;
    [SerializeField] private LayerMask groundLayer = ~0;

    private void LateUpdate()
    {
        Vector3 origin = transform.position + Vector3.up * 10f;

        if (Physics.Raycast(
            origin,
            Vector3.down,
            out RaycastHit hit,
            raycastDistance,
            groundLayer,
            QueryTriggerInteraction.Ignore))
        {
            float minimumHeight = hit.point.y + groundClearance;

            if (transform.position.y < minimumHeight)
            {
                Vector3 position = transform.position;
                position.y = minimumHeight;
                transform.position = position;
            }
        }
    }
}