using UnityEngine;

public class CoverObject : MonoBehaviour
{
    public float damageReduction = 0.5f; // 50% reduction
    public bool directional = true;

    public bool ProvidesCover(Vector3 attackerPos, Vector3 targetPos)
    {
        if (!directional)
            return true;

        Vector3 dirToAttacker = (attackerPos - targetPos).normalized;
        Vector3 forward = transform.forward;

        float dot = Vector3.Dot(forward, dirToAttacker);

        return dot > 0.3f; // attacker must be in front arc
    }
}
