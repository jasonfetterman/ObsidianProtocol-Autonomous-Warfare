using UnityEngine;

public class HitscanSystem
{
    public float Damage = 15f;
    public float Range = 200f;

    // Performs an instant hit from origin toward target direction
    public void FireHitscan(Vector3 origin, Vector3 direction)
    {
        Debug.Log($\"Hitscan fired from {origin} in direction {direction}.\");
        
        RaycastHit hit;

        if (Physics.Raycast(origin, direction, out hit, Range))
        {
            Debug.Log($\"Hitscan impact at {hit.point}, dealing {Damage} damage.\");

            // Placeholder: real system will apply damage to hit object
        }
        else
        {
            Debug.Log(\"Hitscan missed — no impact detected.\");
        }
    }
}
