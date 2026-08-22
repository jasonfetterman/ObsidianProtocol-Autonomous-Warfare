using UnityEngine;
using System.Collections.Generic;

public class RoutePlanningSystem
{
    public List<Vector3> GenerateRoute(Vector3 start, Vector3 end)
    {
        List<Vector3> route = new List<Vector3>();

        // Basic placeholder route logic
        route.Add(start);
        route.Add((start + end) * 0.5f); // midpoint
        route.Add(end);

        return route;
    }
}
