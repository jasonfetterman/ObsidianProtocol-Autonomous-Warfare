using UnityEngine;

public class TerrainMetadata : MonoBehaviour
{
    public LayerMask coverZones;
    public LayerMask dangerZones;
    public LayerMask slowZones;

    public float slowMultiplier = 0.5f;

    public bool IsCover(Vector3 pos)
    {
        return Physics.CheckSphere(pos, 1f, coverZones);
    }

    public bool IsDanger(Vector3 pos)
    {
        return Physics.CheckSphere(pos, 1f, dangerZones);
    }

    public bool IsSlow(Vector3 pos)
    {
        return Physics.CheckSphere(pos, 1f, slowZones);
    }
}
