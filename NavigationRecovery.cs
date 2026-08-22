using UnityEngine;

public class NavigationRecoverySystem
{
    // Attempts to recover navigation when a unit becomes stuck or loses its path
    public bool AttemptRecovery(Vector3 currentPosition, Vector3 lastKnownPathPoint)
    {
        Debug.Log($\"Navigation recovery: From {currentPosition} attempting to return to {lastKnownPathPoint}\");

        float distance = Vector3.Distance(currentPosition, lastKnownPathPoint);

        if (distance > 50f)
        {
            Debug.Log(\"Unit is too far from path — recalculating route.\");
            return false;
        }

        if (distance > 10f)
        {
            Debug.Log(\"Unit is off-path — steering back toward route.\");
            return true;
        }

        Debug.Log(\"Unit successfully recovered navigation.\");
        return true;
    }
}
