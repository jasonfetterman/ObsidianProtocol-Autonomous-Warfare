using UnityEngine;
using System.Collections.Generic;

public class ReinforcementRoutingSystem
{
    // Generates a route for reinforcements to reach a friendly unit or position
    public List<Vector3> GenerateReinforcementRoute(Vector3 reinforcementOrigin, Vector3 friendlyPosition)
    {
        List<Vector3> route = new List<Vector3>();

        Debug.Log($\"Reinforcement routing: From {reinforcementOrigin} to support {friendlyPosition}\");

        // Basic placeholder reinforcement logic
        Vector3 midpoint = (reinforcementOrigin + friendlyPosition) * 0.5f;

        route.Add(reinforcementOrigin);
        route.Add(midpoint);
        route.Add(friendlyPosition);

        return route;
    }
}
