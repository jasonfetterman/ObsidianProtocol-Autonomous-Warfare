using UnityEngine;

public class BallisticDebugger : MonoBehaviour
{
    public BallisticProjectile projectile;

    void OnDrawGizmos()
    {
        if (projectile == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(projectile.targetPos, 0.5f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, projectile.targetPos);
    }
}
