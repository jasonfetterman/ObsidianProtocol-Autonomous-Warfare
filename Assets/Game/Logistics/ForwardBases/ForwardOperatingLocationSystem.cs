using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Logistics
{
    public enum ForwardOperatingLocationState
    {
        Inactive,
        Active,
        Compromised,
        Destroyed
    }

    public sealed class ForwardOperatingLocation
    {
        public string LocationId { get; }

        public string Name { get; }

        public float FuelCapacity { get; }

        public float AmmunitionCapacity { get; }

        public float SparePartsCapacity { get; }

        public float ResourceCapacity { get; }

        public ForwardOperatingLocationState State { get; private set; }

        public ForwardOperatingLocation(
            string locationId,
            string name,
            float fuelCapacity,
            float ammunitionCapacity,
            float sparePartsCapacity,
            float resourceCapacity)
        {
            LocationId =
                locationId ?? string.Empty;

            Name =
                name ?? string.Empty;

            FuelCapacity =
                Math.Max(
                    0f,
                    fuelCapacity);

            AmmunitionCapacity =
                Math.Max(
                    0f,
                    ammunitionCapacity);

            SparePartsCapacity =
                Math.Max(
                    0f,
                    sparePartsCapacity);

            ResourceCapacity =
                Math.Max(
                    0f,
                    resourceCapacity);

            State =
                ForwardOperatingLocationState.Inactive;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(
                LocationId) &&
            !string.IsNullOrWhiteSpace(
                Name);

        public bool Operational =>
            State ==
                ForwardOperatingLocationState.Active ||
            State ==
                ForwardOperatingLocationState.Compromised;

        public void Activate()
        {
            if (State ==
                ForwardOperatingLocationState.Inactive)
            {
                State =
                    ForwardOperatingLocationState.Active;
            }
        }

        public void Compromise()
        {
            if (State ==
                    ForwardOperatingLocationState.Active ||
                State ==
                    ForwardOperatingLocationState.Inactive)
            {
                State =
                    ForwardOperatingLocationState.Compromised;
            }
        }

        public void Destroy()
        {
            State =
                ForwardOperatingLocationState.Destroyed;
        }

        public void Restore()
        {
            if (State ==
                ForwardOperatingLocationState.Compromised)
            {
                State =
                    ForwardOperatingLocationState.Active;
            }
        }
    }

    public sealed class ForwardOperatingLocationSystem
    {
        private readonly Dictionary<string, ForwardOperatingLocation>
            locations =
                new Dictionary<string, ForwardOperatingLocation>(
                    StringComparer.OrdinalIgnoreCase);

        public bool RegisterLocation(
            ForwardOperatingLocation location)
        {
            if (location == null ||
                !location.Valid ||
                locations.ContainsKey(
                    location.LocationId))
            {
                return false;
            }

            locations.Add(
                location.LocationId,
                location);

            return true;
        }

        public bool RemoveLocation(
            string locationId)
        {
            if (string.IsNullOrWhiteSpace(
                    locationId))
            {
                return false;
            }

            return locations.Remove(
                locationId);
        }

        public bool TryGetLocation(
            string locationId,
            out ForwardOperatingLocation location)
        {
            return locations.TryGetValue(
                locationId,
                out location);
        }

        public bool ActivateLocation(
            string locationId)
        {
            if (!locations.TryGetValue(
                    locationId,
                    out ForwardOperatingLocation location))
            {
                return false;
            }

            location.Activate();

            return true;
        }

        public bool CompromiseLocation(
            string locationId)
        {
            if (!locations.TryGetValue(
                    locationId,
                    out ForwardOperatingLocation location))
            {
                return false;
            }

            location.Compromise();

            return true;
        }

        public bool DestroyLocation(
            string locationId)
        {
            if (!locations.TryGetValue(
                    locationId,
                    out ForwardOperatingLocation location))
            {
                return false;
            }

            location.Destroy();

            return true;
        }

        public bool RestoreLocation(
            string locationId)
        {
            if (!locations.TryGetValue(
                    locationId,
                    out ForwardOperatingLocation location))
            {
                return false;
            }

            location.Restore();

            return true;
        }

        public IReadOnlyCollection<ForwardOperatingLocation>
            GetLocations()
        {
            return locations.Values;
        }

        public IReadOnlyCollection<ForwardOperatingLocation>
            GetOperationalLocations()
        {
            List<ForwardOperatingLocation> operational =
                new List<ForwardOperatingLocation>();

            foreach (
                ForwardOperatingLocation location
                in locations.Values)
            {
                if (location.Operational)
                {
                    operational.Add(
                        location);
                }
            }

            return operational;
        }

        public void Clear()
        {
            locations.Clear();
        }
    }
}
