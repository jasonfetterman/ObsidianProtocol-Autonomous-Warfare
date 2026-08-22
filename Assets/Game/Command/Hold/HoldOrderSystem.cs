using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Command.Hold
{
    public sealed class HoldOrder
    {
        public int CommandId;
        public Vector3 Position;
        public bool HoldPosition = true;
        public bool AllowDefensiveFire = true;

        public HoldOrder(
            int commandId,
            Vector3 position)
        {
            CommandId = commandId;
            Position = position;
        }
    }

    public sealed class HoldOrderSystem
    {
        private readonly Dictionary<int, HoldOrder> orders =
            new Dictionary<int, HoldOrder>();

        public void IssueHoldOrder(
            int commandId,
            Vector3 position)
        {
            orders[commandId] =
                new HoldOrder(commandId, position);
        }

        public bool TryGetOrder(
            int commandId,
            out HoldOrder order)
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
