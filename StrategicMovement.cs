using UnityEngine;
using System.Collections.Generic;

public class StrategicMovementSystem
{
    // Evaluates long-range movement decisions
    public void EvaluateStrategicMovement(Vector3 currentPosition, Vector3 objectivePosition)
    {
        Debug.Log($\"Strategic movement evaluation: From {currentPosition} to {objectivePosition}\");

        // Placeholder logic — replace with real strategic evaluation later
        float distance = Vector3.Distance(currentPosition, objectivePosition);

        if (distance > 100f)
        {
            Debug.Log(\"Unit requires long-range movement planning.\");
        }
        else
        {
            Debug.Log(\"Unit is within tactical movement range.\");
        }
    }
}
