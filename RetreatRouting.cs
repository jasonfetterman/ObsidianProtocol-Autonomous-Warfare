using UnityEngine;
using System.Collections.Generic;

public class RetreatRoutingSystem
{
    // Generates a safe retreat route away from danger
    public List<Vector3> GenerateRetreatRoute(Vector3 currentPosition, Vector3 threatPosition)
    {
        List<Vector3> route = new List<Vector3>();

        Debug.Log($\"Retreat routing: From {currentPosition} away from {threatPosition}\");

        // Direction away from the threat
        Vector3 retreatDirection = (currentPosition - threatPosition).normalized;

        // Basic placeholder retreat logic
        Vector3 point1 = currentPosition + retreatDirection * 20f;
        Vector3 point2 = currentPosition + retreatDirection * 40f;
        Vector3 point3 = currentPosition + retreatDirection * 60f;

        route.Add(point1);
        route.Add(point2);
        route.Add(point3);

        return route;
    }
}
