using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Command.Escort
{
    public sealed class EscortOrder
    {
        public int CommandId;
        public GameObject ProtectedUnit;
        public float EscortRadius = 25f;
        public bool MaintainFormation = true;

        public EscortOrder(
            int commandId,
            GameObject protectedUnit)
        {
            CommandId = commandId;
            ProtectedUnit = protectedUnit;
        }
    }

    public sealed class EscortOrderSystem
    {
        private readonly Dictionary<int, EscortOrder> orders =
            new Dictionary<int, EscortOrder>();

        public void IssueEscortOrder(
            int commandId,
            GameObject protectedUnit)
        {
            if (protectedUnit == null)
            {
                return;
            }

            orders[commandId] =
                new EscortOrder(commandId, protectedUnit);
        }

        public bool TryGetOrder(
            int commandId,
            out EscortOrder order)
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
