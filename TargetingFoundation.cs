using UnityEngine;
using System.Collections.Generic;

public class TargetingFoundation
{
    // Finds the closest target from a list of enemy positions
    public Vector3? AcquireTarget(Vector3 unitPosition, List<Vector3> enemyPositions)
    {
        if (enemyPositions == null || enemyPositions.Count == 0)
        {
            Debug.Log(\"No enemies available for targeting.\");
            return null;
        }

        float closestDistance = float.MaxValue;
        Vector3 closestEnemy = Vector3.zero;

        foreach (var enemy in enemyPositions)
        {
            float distance = Vector3.Distance(unitPosition, enemy);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        Debug.Log($\"Target acquired at {closestEnemy} (distance: {closestDistance})\");
        return closestEnemy;
    }

    // Checks line-of-sight (placeholder)
    public bool HasLineOfSight(Vector3 unitPosition, Vector3 targetPosition)
    {
        Debug.Log($\"Checking line-of-sight from {unitPosition} to {targetPosition}\");

        // Placeholder logic — real system will use raycasts
        return true;
    }
}
