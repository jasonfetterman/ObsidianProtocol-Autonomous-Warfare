using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Command.Defend
{
    public sealed class DefendOrder
    {
        public int CommandId;
        public Vector3 Position;
        public float Radius = 25f;
        public bool HoldPosition = true;

        public DefendOrder(
            int commandId,
            Vector3 position)
        {
            CommandId = commandId;
            Position = position;
        }
    }

    public sealed class DefendOrderSystem
    {
        private readonly Dictionary<int, DefendOrder> orders =
            new Dictionary<int, DefendOrder>();

        public void IssueDefendOrder(
            int commandId,
            Vector3 position)
        {
            orders[commandId] =
                new DefendOrder(commandId, position);
        }

        public bool TryGetOrder(
            int commandId,
            out DefendOrder order)
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
