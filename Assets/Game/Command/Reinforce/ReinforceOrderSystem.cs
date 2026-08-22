using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Command.Reinforce
{
    public sealed class ReinforceOrder
    {
        public int CommandId;
        public Vector3 Destination;
        public GameObject SupportedUnit;
        public float ArrivalRadius = 10f;
        public bool JoinExistingForce = true;

        public ReinforceOrder(
            int commandId,
            Vector3 destination)
        {
            CommandId = commandId;
            Destination = destination;
        }

        public ReinforceOrder(
            int commandId,
            Vector3 destination,
            GameObject supportedUnit)
        {
            CommandId = commandId;
            Destination = destination;
            SupportedUnit = supportedUnit;
        }
    }

    public sealed class ReinforceOrderSystem
    {
        private readonly Dictionary<int, ReinforceOrder> orders =
            new Dictionary<int, ReinforceOrder>();

        public void IssueReinforceOrder(
            int commandId,
            Vector3 destination)
        {
            orders[commandId] =
                new ReinforceOrder(
                    commandId,
                    destination);
        }

        public void IssueReinforceOrder(
            int commandId,
            Vector3 destination,
            GameObject supportedUnit)
        {
            orders[commandId] =
                new ReinforceOrder(
                    commandId,
                    destination,
                    supportedUnit);
        }

        public bool TryGetOrder(
            int commandId,
            out ReinforceOrder order)
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
