using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.RoutePlanning
{
    public sealed class RoutePlanningService : MonoBehaviour
    {
        public List<Vector3> BuildRoute(
            Vector3 start,
            IReadOnlyList<Vector3> waypoints,
            Vector3 destination)
        {
            var route = new List<Vector3>();

            route.Add(start);

            if (waypoints != null)
            {
                for (int i = 0; i < waypoints.Count; i++)
                {
                    route.Add(waypoints[i]);
                }
            }

            if (route.Count == 0 ||
                Vector3.Distance(
                    route[route.Count - 1],
                    destination) > 0.01f)
            {
                route.Add(destination);
            }

            return route;
        }

        public Vector3 GetNextWaypoint(
            IReadOnlyList<Vector3> route,
            int currentIndex)
        {
            if (route == null ||
                route.Count == 0 ||
                currentIndex < 0 ||
                currentIndex >= route.Count)
            {
                return Vector3.zero;
            }

            return route[currentIndex];
        }

        public bool HasReachedWaypoint(
            Vector3 position,
            Vector3 waypoint,
            float tolerance = 1f)
        {
            tolerance = Mathf.Max(0.01f, tolerance);

            return Vector3.Distance(
                position,
                waypoint) <= tolerance;
        }
    }
}
