using UnityEngine;

public class AreaOfEffectSystem
{
    public float Radius = 12f;
    public float BaseDamage = 30f;

    // Applies AoE damage to all objects within radius
    public void ApplyAoE(Vector3 center)
    {
        Debug.Log($\"AoE triggered at {center} with radius {Radius}.\");
        
        Collider[] hits = Physics.OverlapSphere(center, Radius);

        foreach (var hit in hits)
        {
            float distance = Vector3.Distance(center, hit.transform.position);
            float falloff = Mathf.Clamp01(1f - (distance / Radius));
            float finalDamage = BaseDamage * falloff;

            Debug.Log($\"Object {hit.name} takes {finalDamage} AoE damage (distance: {distance}).\");

            // Placeholder: real system will apply damage to hit object
        }
    }

    // Persistent AoE field (e.g., fire, radiation, poison gas)
    public void ApplyPersistentAoE(Vector3 center, float dps)
    {
        Collider[] hits = Physics.OverlapSphere(center, Radius);

        foreach (var hit in hits)
        {
            Debug.Log($\"Object {hit.name} takes {dps} DPS from persistent AoE.\");
        }
    }
}
