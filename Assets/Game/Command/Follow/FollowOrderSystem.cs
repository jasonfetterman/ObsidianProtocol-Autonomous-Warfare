using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Command.Follow
{
    public sealed class FollowOrder
    {
        public int CommandId;
        public GameObject TargetUnit;
        public float FollowDistance = 15f;
        public float SupportRadius = 25f;
        public bool MaintainFormation = true;
        public bool EngageThreats = true;

        public FollowOrder(
            int commandId,
            GameObject targetUnit)
        {
            CommandId = commandId;
            TargetUnit = targetUnit;
        }
    }

    public sealed class FollowOrderSystem
    {
        private readonly Dictionary<int, FollowOrder> orders =
            new Dictionary<int, FollowOrder>();

        public void IssueFollowOrder(
            int commandId,
            GameObject targetUnit)
        {
            if (targetUnit == null)
            {
                return;
            }

            orders[commandId] =
                new FollowOrder(
                    commandId,
                    targetUnit);
        }

        public bool TryGetOrder(
            int commandId,
            out FollowOrder order)
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
