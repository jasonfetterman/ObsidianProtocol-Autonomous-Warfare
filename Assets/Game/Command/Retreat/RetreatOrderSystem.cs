using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Command.Retreat
{
    public sealed class RetreatOrder
    {
        public int CommandId;
        public Vector3 Destination;
        public float SafeRadius = 25f;
        public bool RegroupAfterArrival = true;

        public RetreatOrder(
            int commandId,
            Vector3 destination)
        {
            CommandId = commandId;
            Destination = destination;
        }
    }

    public sealed class RetreatOrderSystem
    {
        private readonly Dictionary<int, RetreatOrder> orders =
            new Dictionary<int, RetreatOrder>();

        public void IssueRetreatOrder(
            int commandId,
            Vector3 destination)
        {
            orders[commandId] =
                new RetreatOrder(
                    commandId,
                    destination);
        }

        public bool TryGetOrder(
            int commandId,
            out RetreatOrder order)
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
