using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Pathfinding
{
    public sealed class PathfindingService : MonoBehaviour
    {
        [SerializeField] private PathfindingDefinition definition;

        public PathfindingDefinition Definition => definition;

        public bool TryBuildPath(
            Vector3 start,
            Vector3 destination,
            List<Vector3> path)
        {
            if (path == null)
            {
                return false;
            }

            path.Clear();

            path.Add(start);

            if (Vector3.Distance(start, destination) > 0.01f)
            {
                path.Add(destination);
            }

            return true;
        }
    }
}
