using UnityEngine;

public class ThreatLevel : MonoBehaviour
{
    public float baseThreat = 1f;     // baseline threat
    public float damageThreat = 0.5f; // threat added per damage dealt
    public float healthThreat = 0.01f; // threat added per HP

    public float CalculateThreat(Health h, Shooter s)
    {
        float threat = baseThreat;

        if (h != null)
            threat += h.currentHealth * healthThreat;

        if (s != null)
            threat += s.baseDamage * damageThreat;

        return threat;
    }
}
