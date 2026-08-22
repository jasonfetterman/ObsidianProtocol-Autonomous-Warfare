using UnityEngine;

public class ExplosiveDamage
{
    public float BaseDamage = 50f;
    public float BlastRadius = 10f;

    // Applies explosive damage to all objects within radius
    public void ApplyExplosion(Vector3 explosionCenter)
    {
        Debug.Log($\"Explosion triggered at {explosionCenter} with radius {BlastRadius}.\");
        
        Collider[] hits = Physics.OverlapSphere(explosionCenter, BlastRadius);

        foreach (var hit in hits)
        {
            float distance = Vector3.Distance(explosionCenter, hit.transform.position);
            float falloff = Mathf.Clamp01(1f - (distance / BlastRadius));
            float finalDamage = BaseDamage * falloff;

            Debug.Log($\"Object {hit.name} takes {finalDamage} explosive damage (distance: {distance}).\");

            // Placeholder: real system will apply damage to hit object
        }
    }
}
