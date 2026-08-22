using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Construction
{
    public enum ConstructionVehicleState
    {
        Idle,
        Moving,
        Building,
        Repairing,
        Disabled
    }

    public sealed class ConstructionVehicle
    {
        public string VehicleId { get; }

        public string UnitId { get; }

        public float ConstructionRate { get; }

        public ConstructionVehicleState State { get; private set; }

        public string AssignedSiteId { get; private set; }

        public ConstructionVehicle(
            string vehicleId,
            string unitId,
            float constructionRate)
        {
            VehicleId =
                vehicleId ?? string.Empty;

            UnitId =
                unitId ?? string.Empty;

            ConstructionRate =
                Math.Max(
                    0f,
                    constructionRate);

            State =
                ConstructionVehicleState.Idle;

            AssignedSiteId =
                string.Empty;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(VehicleId) &&
            !string.IsNullOrWhiteSpace(UnitId) &&
            ConstructionRate > 0f;

        public bool Available =>
            State ==
            ConstructionVehicleState.Idle;

        public bool Assign(
            string siteId)
        {
            if (!Available ||
                string.IsNullOrWhiteSpace(siteId))
            {
                return false;
            }

            AssignedSiteId =
                siteId;

            State =
                ConstructionVehicleState.Moving;

            return true;
        }

        public void BeginConstruction()
        {
            if (State ==
                    ConstructionVehicleState.Moving &&
                !string.IsNullOrWhiteSpace(
                    AssignedSiteId))
            {
                State =
                    ConstructionVehicleState.Building;
            }
        }

        public void BeginRepair()
        {
            if (State ==
                    ConstructionVehicleState.Idle ||
                State ==
                    ConstructionVehicleState.Building)
            {
                State =
                    ConstructionVehicleState.Repairing;
            }
        }

        public void Release()
        {
            AssignedSiteId =
                string.Empty;

            State =
                ConstructionVehicleState.Idle;
        }

        public void Disable()
        {
            State =
                ConstructionVehicleState.Disabled;
        }

        public void Restore()
        {
            if (State ==
                ConstructionVehicleState.Disabled)
            {
                State =
                    ConstructionVehicleState.Idle;
            }
        }
    }

    public sealed class ConstructionVehicleSystem
    {
        private readonly Dictionary<string, ConstructionVehicle>
            vehicles =
                new Dictionary<string, ConstructionVehicle>(
                    StringComparer.OrdinalIgnoreCase);

        public bool RegisterVehicle(
            ConstructionVehicle vehicle)
        {
            if (vehicle == null ||
                !vehicle.Valid ||
                vehicles.ContainsKey(
                    vehicle.VehicleId))
            {
                return false;
            }

            vehicles.Add(
                vehicle.VehicleId,
                vehicle);

            return true;
        }

        public bool RemoveVehicle(
            string vehicleId)
        {
            if (string.IsNullOrWhiteSpace(
                    vehicleId))
            {
                return false;
            }

            return vehicles.Remove(
                vehicleId);
        }

        public bool TryGetVehicle(
            string vehicleId,
            out ConstructionVehicle vehicle)
        {
            return vehicles.TryGetValue(
                vehicleId,
                out vehicle);
        }

        public bool AssignVehicle(
            string vehicleId,
            string siteId)
        {
            if (!vehicles.TryGetValue(
                    vehicleId,
                    out ConstructionVehicle vehicle))
            {
                return false;
            }

            return vehicle.Assign(
                siteId);
        }

        public bool BeginConstruction(
            string vehicleId)
        {
            if (!vehicles.TryGetValue(
                    vehicleId,
                    out ConstructionVehicle vehicle))
            {
                return false;
            }

            vehicle.BeginConstruction();

            return true;
        }

        public bool BeginRepair(
            string vehicleId)
        {
            if (!vehicles.TryGetValue(
                    vehicleId,
                    out ConstructionVehicle vehicle))
            {
                return false;
            }

            vehicle.BeginRepair();

            return true;
        }

        public bool ReleaseVehicle(
            string vehicleId)
        {
            if (!vehicles.TryGetValue(
                    vehicleId,
                    out ConstructionVehicle vehicle))
            {
                return false;
            }

            vehicle.Release();

            return true;
        }

        public bool DisableVehicle(
            string vehicleId)
        {
            if (!vehicles.TryGetValue(
                    vehicleId,
                    out ConstructionVehicle vehicle))
            {
                return false;
            }

            vehicle.Disable();

            return true;
        }

        public bool RestoreVehicle(
            string vehicleId)
        {
            if (!vehicles.TryGetValue(
                    vehicleId,
                    out ConstructionVehicle vehicle))
            {
                return false;
            }

            vehicle.Restore();

            return true;
        }

        public IReadOnlyCollection<ConstructionVehicle>
            GetVehicles()
        {
            return vehicles.Values;
        }

        public IReadOnlyCollection<ConstructionVehicle>
            GetAvailableVehicles()
        {
            List<ConstructionVehicle> available =
                new List<ConstructionVehicle>();

            foreach (
                ConstructionVehicle vehicle
                in vehicles.Values)
            {
                if (vehicle.Available)
                {
                    available.Add(
                        vehicle);
                }
            }

            return available;
        }

        public void Clear()
        {
            vehicles.Clear();
        }
    }
}
