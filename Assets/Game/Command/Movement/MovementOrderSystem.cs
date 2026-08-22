using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Command.Movement
{
    [Serializable]
    public sealed class MovementOrder
    {
        public int CommandId;
        public Vector3 Destination;
        public float FormationSpacing = 5f;
        public bool UseFormation = true;

        public MovementOrder(
            int commandId,
            Vector3 destination)
        {
            CommandId = commandId;
            Destination = destination;
        }
    }

    public sealed class MovementOrderSystem
    {
        private readonly Dictionary<int, MovementOrder> orders =
            new Dictionary<int, MovementOrder>();

        public void IssueMovementOrder(
            int commandId,
            Vector3 destination)
        {
            orders[commandId] =
                new MovementOrder(commandId, destination);
        }

        public bool TryGetOrder(
            int commandId,
            out MovementOrder order)
        {
            return orders.TryGetValue(
                commandId,
                out order);
        }

        public void UpdateDestination(
            int commandId,
            Vector3 destination)
        {
            if (orders.TryGetValue(commandId, out MovementOrder order))
            {
                order.Destination = destination;
            }
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
