using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.VR
{
    public enum VehicleInteractionType
    {
        None,
        Hatch,
        Door,
        Seat,
        Console,
        ControlPanel,
        Maintenance,
        Storage,
        WeaponStation
    }

    public sealed class VehicleInteractionPoint
    {
        public string InteractionId { get; }

        public VehicleInteractionType Type { get; }

        public bool Available { get; private set; }

        public bool Occupied { get; private set; }

        public VehicleInteractionPoint(
            string interactionId,
            VehicleInteractionType type)
        {
            InteractionId =
                interactionId ?? string.Empty;

            Type = type;

            Available = true;
            Occupied = false;
        }

        public bool SetAvailable(
            bool available)
        {
            if (!available &&
                Occupied)
            {
                return false;
            }

            Available = available;

            return true;
        }

        public bool Occupy()
        {
            if (!Available ||
                Occupied)
            {
                return false;
            }

            Occupied = true;

            return true;
        }

        public bool Release()
        {
            if (!Occupied)
            {
                return false;
            }

            Occupied = false;

            return true;
        }
    }

    public sealed class VehicleInteraction
    {
        private readonly Dictionary<
            string,
            VehicleInteractionPoint> interactionPoints =
            new Dictionary<
                string,
                VehicleInteractionPoint>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public string UnitId { get; private set; }

        public int InteractionPointCount =>
            interactionPoints.Count;

        public bool Initialize(
            string unitId)
        {
            if (Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            UnitId =
                unitId.Trim();

            interactionPoints.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterInteractionPoint(
            string interactionId,
            VehicleInteractionType type)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(interactionId) ||
                type == VehicleInteractionType.None)
            {
                return false;
            }

            string id =
                interactionId.Trim();

            if (interactionPoints.ContainsKey(id))
            {
                return false;
            }

            interactionPoints.Add(
                id,
                new VehicleInteractionPoint(
                    id,
                    type));

            return true;
        }

        public bool SetAvailable(
            string interactionId,
            bool available)
        {
            VehicleInteractionPoint point =
                GetInteractionPoint(interactionId);

            return point != null &&
                   point.SetAvailable(available);
        }

        public bool Occupy(
            string interactionId)
        {
            VehicleInteractionPoint point =
                GetInteractionPoint(interactionId);

            return point != null &&
                   point.Occupy();
        }

        public bool Release(
            string interactionId)
        {
            VehicleInteractionPoint point =
                GetInteractionPoint(interactionId);

            return point != null &&
                   point.Release();
        }

        public VehicleInteractionPoint
            GetInteractionPoint(
                string interactionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(interactionId))
            {
                return null;
            }

            interactionPoints.TryGetValue(
                interactionId.Trim(),
                out VehicleInteractionPoint point);

            return point;
        }

        public IReadOnlyCollection<
            VehicleInteractionPoint>
            GetInteractionPoints()
        {
            return interactionPoints.Values;
        }

        public void Reset()
        {
            interactionPoints.Clear();

            Initialized = false;

            UnitId =
                string.Empty;
        }
    }
}
