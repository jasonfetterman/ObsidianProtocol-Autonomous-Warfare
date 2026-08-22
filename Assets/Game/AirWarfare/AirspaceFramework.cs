using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.AirWarfare
{
    public enum AirspaceClassification
    {
        Open,
        Restricted,
        Controlled,
        Hostile,
        NoFly
    }

    public sealed class AirspaceZone
    {
        public string ZoneId { get; }
        public AirspaceClassification Classification { get; private set; }

        public float MinimumAltitude { get; private set; }
        public float MaximumAltitude { get; private set; }

        public bool AllowsWardenFlight { get; private set; }

        public AirspaceZone(
            string zoneId,
            AirspaceClassification classification)
        {
            ZoneId = zoneId ?? string.Empty;
            Classification = classification;

            MinimumAltitude = 0f;
            MaximumAltitude = float.MaxValue;

            AllowsWardenFlight =
                classification != AirspaceClassification.NoFly;
        }

        public void ConfigureAltitude(
            float minimumAltitude,
            float maximumAltitude)
        {
            MinimumAltitude =
                Math.Max(0f, minimumAltitude);

            MaximumAltitude =
                Math.Max(
                    MinimumAltitude,
                    maximumAltitude);
        }

        public void SetClassification(
            AirspaceClassification classification)
        {
            Classification = classification;

            AllowsWardenFlight =
                classification != AirspaceClassification.NoFly;
        }

        public void SetWardenFlightAllowed(
            bool allowed)
        {
            AllowsWardenFlight = allowed;
        }

        public bool ContainsAltitude(
            float altitude)
        {
            return altitude >= MinimumAltitude &&
                   altitude <= MaximumAltitude;
        }
    }

    public sealed class AirspaceFramework
    {
        private readonly Dictionary<string, AirspaceZone> zones =
            new Dictionary<string, AirspaceZone>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterZone(
            string zoneId,
            AirspaceClassification classification)
        {
            if (string.IsNullOrWhiteSpace(zoneId))
            {
                return;
            }

            zones[zoneId] =
                new AirspaceZone(
                    zoneId,
                    classification);
        }

        public bool TryGetZone(
            string zoneId,
            out AirspaceZone zone)
        {
            return zones.TryGetValue(
                zoneId,
                out zone);
        }

        public bool CanEnter(
            string zoneId,
            float altitude)
        {
            return zones.TryGetValue(
                       zoneId,
                       out AirspaceZone zone) &&
                   zone.AllowsWardenFlight &&
                   zone.ContainsAltitude(altitude);
        }

        public void RemoveZone(string zoneId)
        {
            zones.Remove(zoneId);
        }

        public void Clear()
        {
            zones.Clear();
        }
    }
}
