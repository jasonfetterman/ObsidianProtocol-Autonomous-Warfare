using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Command
{
    public enum CommandOrderType
    {
        Move,
        Attack,
        Defend,
        Hold,
        Patrol,
        Follow,
        Retreat,
        Pursue,
        Reinforce,
        Repair,
        Scout,
        Suppress,
        Breach
    }

    public sealed class CommandOrder
    {
        public string OrderId { get; }
        public CommandOrderType Type { get; }
        public string TargetId { get; }
        public float PositionX { get; }
        public float PositionY { get; }
        public float PositionZ { get; }

        public CommandOrder(
            string orderId,
            CommandOrderType type,
            string targetId,
            float positionX,
            float positionY,
            float positionZ)
        {
            OrderId = orderId ?? string.Empty;
            Type = type;
            TargetId = targetId ?? string.Empty;

            PositionX = positionX;
            PositionY = positionY;
            PositionZ = positionZ;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(OrderId);
    }

    public sealed class CommandOrderSystem
    {
        private readonly Dictionary<string, CommandOrder> orders =
            new Dictionary<string, CommandOrder>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(CommandOrder order)
        {
            if (order == null ||
                !order.Valid ||
                orders.ContainsKey(order.OrderId))
            {
                return false;
            }

            orders.Add(order.OrderId, order);
            return true;
        }

        public bool Remove(string orderId)
        {
            if (string.IsNullOrWhiteSpace(orderId))
                return false;

            return orders.Remove(orderId);
        }

        public bool TryGet(
            string orderId,
            out CommandOrder order)
        {
            return orders.TryGetValue(
                orderId,
                out order);
        }

        public IReadOnlyCollection<CommandOrder> GetOrders()
        {
            return orders.Values;
        }

        public void Clear()
        {
            orders.Clear();
        }
    }
}
