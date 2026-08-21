using UnityEngine;

public class TerrainAlign : MonoBehaviour
{
    public float sampleDistance = 1.5f;
    public float rayHeight = 100f;
    public float rayDistance = 300f;

    void Start()
    {
        AlignToTerrain();
    }

    void AlignToTerrain()
    {
        Vector3 center = transform.position;

        Vector3 front = center + Vector3.forward * sampleDistance;
        Vector3 back = center - Vector3.forward * sampleDistance;
        Vector3 right = center + Vector3.right * sampleDistance;
        Vector3 left = center - Vector3.right * sampleDistance;

        Vector3 frontGround;
        Vector3 backGround;
        Vector3 rightGround;
        Vector3 leftGround;

        if (!GetGround(front, out frontGround))
            return;

        if (!GetGround(back, out backGround))
            return;

        if (!GetGround(right, out rightGround))
            return;

        if (!GetGround(left, out leftGround))
            return;

        Vector3 forwardSlope = frontGround - backGround;
        Vector3 rightSlope = rightGround - leftGround;

        Vector3 normal = Vector3.Cross(rightSlope, forwardSlope).normalized;

        Vector3 currentForward = transform.forward;
        Vector3 projectedForward =
            Vector3.ProjectOnPlane(currentForward, normal).normalized;

        if (projectedForward.sqrMagnitude > 0.001f)
        {
            transform.rotation =
                Quaternion.LookRotation(projectedForward, normal);
        }

        Ray ray = new Ray(
            transform.position + Vector3.up * rayHeight,
            Vector3.down
        );

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            transform.position = new Vector3(
                transform.position.x,
                hit.point.y,
                transform.position.z
            );
        }
    }

    bool GetGround(Vector3 position, out Vector3 groundPoint)
    {
        Ray ray = new Ray(
            position + Vector3.up * rayHeight,
            Vector3.down
        );

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            groundPoint = hit.point;
            return true;
        }

        groundPoint = Vector3.zero;
        return false;
    }
}