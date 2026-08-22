using System.Collections.Generic;
using UnityEngine;

public sealed class LargeScaleNavigationOptimization
{
    public void OptimizePaths(
        List<Vector3> unitPositions,
        Vector3 objectivePosition)
    {
        if (unitPositions == null ||
            unitPositions.Count == 0)
        {
            return;
        }

        for (int i = 0; i < unitPositions.Count; i++)
        {
            Vector3 position = unitPositions[i];

            float distance =
                Vector3.Distance(
                    position,
                    objectivePosition);

            if (distance > 200f)
            {
                Debug.Log(
                    "Large-scale navigation: long-range unit.");
            }
            else if (distance > 50f)
            {
                Debug.Log(
                    "Large-scale navigation: mid-range unit.");
            }
        }
    }

    public Vector3 GetSharedMovementDirection(
        Vector3 averagePosition,
        Vector3 objectivePosition)
    {
        Vector3 direction =
            objectivePosition -
            averagePosition;

        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.0001f)
        {
            return Vector3.zero;
        }

        return direction.normalized;
    }

    public Vector3 GetFormationCenter(
        List<Vector3> unitPositions)
    {
        if (unitPositions == null ||
            unitPositions.Count == 0)
        {
            return Vector3.zero;
        }

        Vector3 total = Vector3.zero;

        for (int i = 0; i < unitPositions.Count; i++)
        {
            total += unitPositions[i];
        }

        return total / unitPositions.Count;
    }
}
