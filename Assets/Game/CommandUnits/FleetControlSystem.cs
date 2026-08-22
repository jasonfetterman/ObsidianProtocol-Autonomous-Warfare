using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.CommandUnits
{
    public enum FleetOrderType
    {
        Hold,
        Move,
        Patrol,
        Recon,
        Escort,
        Support,
        Attack,
        Defend,
        Retreat,
        Recover
    }

    public enum FleetOrderState
    {
        Queued,
        Active,
        Complete,
        Cancelled
    }

    public sealed class FleetOrder
    {
        public string OrderId { get; }
        public FleetOrderType Type { get; }

        public string FleetId { get; }
        public string ObjectiveId { get; }

        public FleetOrderState State { get; private set; }

        public FleetOrder(
            string orderId,
            string fleetId,
            string objectiveId,
            FleetOrderType type)
        {
            OrderId =
                orderId ?? string.Empty;

            FleetId =
                fleetId ?? string.Empty;

            ObjectiveId =
                objectiveId ?? string.Empty;

            Type =
                type;

            State =
                FleetOrderState.Queued;
        }

        public void Activate()
        {
            if (State ==
                FleetOrderState.Queued)
            {
                State =
                    FleetOrderState.Active;
            }
        }

        public void Complete()
        {
            if (State ==
                FleetOrderState.Active)
            {
                State =
                    FleetOrderState.Complete;
            }
        }

        public void Cancel()
        {
            State =
                FleetOrderState.Cancelled;
        }
    }

    public sealed class Fleet
    {
        public string FleetId { get; }
        public string FleetName { get; }

        private readonly HashSet<string> units =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, FleetOrder> orders =
            new Dictionary<string, FleetOrder>(
                StringComparer.OrdinalIgnoreCase);

        public Fleet(
            string fleetId,
            string fleetName)
        {
            FleetId =
                fleetId ?? string.Empty;

            FleetName =
                fleetName ?? string.Empty;
        }

        public void AddUnit(
            string unitId)
        {
            if (!string.IsNullOrWhiteSpace(unitId))
            {
                units.Add(unitId);
            }
        }

        public void RemoveUnit(
            string unitId)
        {
            units.Remove(unitId);
        }

        public bool ContainsUnit(
            string unitId)
        {
            return units.Contains(unitId);
        }

        public IReadOnlyCollection<string> GetUnits()
        {
            return units;
        }

        public void AddOrder(
            FleetOrder order)
        {
            if (order == null ||
                string.IsNullOrWhiteSpace(order.OrderId))
            {
                return;
            }

            orders[order.OrderId] = order;
        }

        public bool TryGetOrder(
            string orderId,
            out FleetOrder order)
        {
            return orders.TryGetValue(
                orderId,
                out order);
        }

        public IReadOnlyCollection<FleetOrder> GetOrders()
        {
            return orders.Values;
        }

        public void RemoveOrder(
            string orderId)
        {
            orders.Remove(orderId);
        }
    }

    public sealed class FleetControlSystem
    {
        private readonly Dictionary<string, Fleet> fleets =
            new Dictionary<string, Fleet>(
                StringComparer.OrdinalIgnoreCase);

        public void CreateFleet(
            string fleetId,
            string fleetName)
        {
            if (string.IsNullOrWhiteSpace(fleetId))
            {
                return;
            }

            fleets[fleetId] =
                new Fleet(
                    fleetId,
                    fleetName);
        }

        public void AddUnitToFleet(
            string fleetId,
            string unitId)
        {
            if (fleets.TryGetValue(
                    fleetId,
                    out Fleet fleet))
            {
                fleet.AddUnit(unitId);
            }
        }

        public void RemoveUnitFromFleet(
            string fleetId,
            string unitId)
        {
            if (fleets.TryGetValue(
                    fleetId,
                    out Fleet fleet))
            {
                fleet.RemoveUnit(unitId);
            }
        }

        public void IssueOrder(
            string fleetId,
            string orderId,
            FleetOrderType type,
            string objectiveId)
        {
            if (!fleets.TryGetValue(
                    fleetId,
                    out Fleet fleet))
            {
                return;
            }

            fleet.AddOrder(
                new FleetOrder(
                    orderId,
                    fleetId,
                    objectiveId,
                    type));
        }

        public void ActivateOrder(
            string fleetId,
            string orderId)
        {
            if (fleets.TryGetValue(
                    fleetId,
                    out Fleet fleet) &&
                fleet.TryGetOrder(
                    orderId,
                    out FleetOrder order))
            {
                order.Activate();
            }
        }

        public void CompleteOrder(
            string fleetId,
            string orderId)
        {
            if (fleets.TryGetValue(
                    fleetId,
                    out Fleet fleet) &&
                fleet.TryGetOrder(
                    orderId,
                    out FleetOrder order))
            {
                order.Complete();
            }
        }

        public void CancelOrder(
            string fleetId,
            string orderId)
        {
            if (fleets.TryGetValue(
                    fleetId,
                    out Fleet fleet) &&
                fleet.TryGetOrder(
                    orderId,
                    out FleetOrder order))
            {
                order.Cancel();
            }
        }

        public bool TryGetFleet(
            string fleetId,
            out Fleet fleet)
        {
            return fleets.TryGetValue(
                fleetId,
                out fleet);
        }

        public void RemoveFleet(
            string fleetId)
        {
            fleets.Remove(fleetId);
        }

        public void Clear()
        {
            fleets.Clear();
        }
    }
}
