using UnityEngine;

public class CoverResolver : MonoBehaviour
{
    public float coverCheckRadius = 1.5f;
    public LayerMask coverMask;

    public float GetCoverMultiplier(Vector3 attackerPos)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, coverCheckRadius, coverMask);

        foreach (var h in hits)
        {
            CoverObject cover = h.GetComponent<CoverObject>();
            if (cover == null) continue;

            if (cover.ProvidesCover(attackerPos, transform.position))
                return cover.damageReduction;
        }

        return 1f; // no cover
    }
}
