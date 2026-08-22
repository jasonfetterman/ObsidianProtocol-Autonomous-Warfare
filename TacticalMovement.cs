using UnityEngine;

public class TacticalMovementSystem
{
    // Handles short-range tactical movement decisions
    public void ExecuteTacticalMovement(Vector3 currentPosition, Vector3 targetPosition)
    {
        Debug.Log($\"Tactical movement: From {currentPosition} toward {targetPosition}\");

        float distance = Vector3.Distance(currentPosition, targetPosition);

        if (distance > 25f)
        {
            Debug.Log(\"Advancing toward target...\");
        }
        else if (distance > 5f)
        {
            Debug.Log(\"Preparing close-range maneuver...\");
        }
        else
        {
            Debug.Log(\"Engaging immediate tactical action.\");
        }
    }
}
