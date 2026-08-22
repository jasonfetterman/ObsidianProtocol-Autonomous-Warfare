using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Logistics
{
    public enum ConvoyState
    {
        Planned,
        Loading,
        Departing,
        InTransit,
        Arrived,
        Cancelled,
        Lost
    }

    public sealed class SupplyConvoy
    {
        private readonly List<string> shipmentIds =
            new List<string>();

        public string ConvoyId { get; }

        public string OriginId { get; }

        public string DestinationId { get; }

        public ConvoyState State { get; private set; }

        public IReadOnlyCollection<string> ShipmentIds =>
            shipmentIds.AsReadOnly();

        public SupplyConvoy(
            string convoyId,
            string originId,
            string destinationId)
        {
            ConvoyId =
                convoyId ?? string.Empty;

            OriginId =
                originId ?? string.Empty;

            DestinationId =
                destinationId ?? string.Empty;

            State =
                ConvoyState.Planned;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(ConvoyId) &&
            !string.IsNullOrWhiteSpace(OriginId) &&
            !string.IsNullOrWhiteSpace(DestinationId) &&
            !string.Equals(
                OriginId,
                DestinationId,
                StringComparison.OrdinalIgnoreCase);

        public bool AddShipment(
            string shipmentId)
        {
            if (!Valid ||
                string.IsNullOrWhiteSpace(shipmentId) ||
                State != ConvoyState.Planned &&
                State != ConvoyState.Loading ||
                shipmentIds.Contains(shipmentId))
            {
                return false;
            }

            shipmentIds.Add(
                shipmentId);

            State =
                ConvoyState.Loading;

            return true;
        }

        public void Depart()
        {
            if (State == ConvoyState.Planned ||
                State == ConvoyState.Loading)
            {
                State =
                    ConvoyState.Departing;
            }
        }

        public void BeginTransit()
        {
            if (State == ConvoyState.Departing)
            {
                State =
                    ConvoyState.InTransit;
            }
        }

        public void Arrive()
        {
            if (State == ConvoyState.InTransit)
            {
                State =
                    ConvoyState.Arrived;
            }
        }

        public void Cancel()
        {
            if (State != ConvoyState.Arrived &&
                State != ConvoyState.Lost)
            {
                State =
                    ConvoyState.Cancelled;
            }
        }

        public void MarkLost()
        {
            if (State == ConvoyState.InTransit ||
                State == ConvoyState.Departing)
            {
                State =
                    ConvoyState.Lost;
            }
        }
    }

    public sealed class ConvoySystem
    {
        private readonly Dictionary<string, SupplyConvoy> convoys =
            new Dictionary<string, SupplyConvoy>(
                StringComparer.OrdinalIgnoreCase);

        public bool RegisterConvoy(
            SupplyConvoy convoy)
        {
            if (convoy == null ||
                !convoy.Valid ||
                convoys.ContainsKey(convoy.ConvoyId))
            {
                return false;
            }

            convoys.Add(
                convoy.ConvoyId,
                convoy);

            return true;
        }

        public bool RemoveConvoy(
            string convoyId)
        {
            if (string.IsNullOrWhiteSpace(convoyId))
            {
                return false;
            }

            return convoys.Remove(
                convoyId);
        }

        public bool TryGetConvoy(
            string convoyId,
            out SupplyConvoy convoy)
        {
            return convoys.TryGetValue(
                convoyId,
                out convoy);
        }

        public bool AddShipmentToConvoy(
            string convoyId,
            string shipmentId)
        {
            if (!convoys.TryGetValue(
                    convoyId,
                    out SupplyConvoy convoy))
            {
                return false;
            }

            return convoy.AddShipment(
                shipmentId);
        }

        public bool DepartConvoy(
            string convoyId)
        {
            if (!convoys.TryGetValue(
                    convoyId,
                    out SupplyConvoy convoy))
            {
                return false;
            }

            convoy.Depart();

            return true;
        }

        public bool BeginConvoyTransit(
            string convoyId)
        {
            if (!convoys.TryGetValue(
                    convoyId,
                    out SupplyConvoy convoy))
            {
                return false;
            }

            convoy.BeginTransit();

            return true;
        }

        public bool ArriveConvoy(
            string convoyId)
        {
            if (!convoys.TryGetValue(
                    convoyId,
                    out SupplyConvoy convoy))
            {
                return false;
            }

            convoy.Arrive();

            return true;
        }

        public bool CancelConvoy(
            string convoyId)
        {
            if (!convoys.TryGetValue(
                    convoyId,
                    out SupplyConvoy convoy))
            {
                return false;
            }

            convoy.Cancel();

            return true;
        }

        public bool MarkConvoyLost(
            string convoyId)
        {
            if (!convoys.TryGetValue(
                    convoyId,
                    out SupplyConvoy convoy))
            {
                return false;
            }

            convoy.MarkLost();

            return true;
        }

        public IReadOnlyCollection<SupplyConvoy>
            GetConvoys()
        {
            return convoys.Values;
        }

        public IReadOnlyCollection<SupplyConvoy>
            GetActiveConvoys()
        {
            List<SupplyConvoy> active =
                new List<SupplyConvoy>();

            foreach (
                SupplyConvoy convoy
                in convoys.Values)
            {
                if (convoy.State ==
                        ConvoyState.Departing ||
                    convoy.State ==
                        ConvoyState.InTransit)
                {
                    active.Add(
                        convoy);
                }
            }

            return active;
        }

        public void Clear()
        {
            convoys.Clear();
        }
    }
}
