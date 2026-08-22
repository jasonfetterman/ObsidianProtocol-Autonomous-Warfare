using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Command.Patrol
{
    public sealed class PatrolOrder
    {
        public int CommandId;
        public List<Vector3> Waypoints;
        public bool Loop = true;
        public float WaitTimeAtWaypoint;

        public PatrolOrder(
            int commandId,
            List<Vector3> waypoints)
        {
            CommandId = commandId;
            Waypoints = waypoints != null
                ? new List<Vector3>(waypoints)
                : new List<Vector3>();
        }
    }

    public sealed class PatrolOrderSystem
    {
        private readonly Dictionary<int, PatrolOrder> orders =
            new Dictionary<int, PatrolOrder>();

        public void IssuePatrolOrder(
            int commandId,
            List<Vector3> waypoints)
        {
            orders[commandId] =
                new PatrolOrder(commandId, waypoints);
        }

        public bool TryGetOrder(
            int commandId,
            out PatrolOrder order)
        {
            return orders.TryGetValue(
                commandId,
                out order);
        }

        public void RemoveOrder(int commandId)
        {
            orders.Remove(commandId);
        }

        public void Clear()
        {
            orders.Clear();
        }
    }
}
