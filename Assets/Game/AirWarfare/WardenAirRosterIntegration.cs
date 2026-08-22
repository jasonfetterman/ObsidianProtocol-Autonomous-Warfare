using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.AirWarfare
{
    public enum WardenAirUnitRole
    {
        Combat,
        Reconnaissance,
        Surveillance,
        Relay,
        Support,
        Transport,
        SearchAndRescue,
        Command
    }

    public sealed class WardenAirUnitProfile
    {
        public string UnitId { get; }
        public string UnitName { get; }
        public WardenAirUnitRole Role { get; }

        public float MaximumSpeed { get; }
        public float MinimumAltitude { get; }
        public float MaximumAltitude { get; }

        public bool Autonomous { get; }

        public WardenAirUnitProfile(
            string unitId,
            string unitName,
            WardenAirUnitRole role,
            float maximumSpeed,
            float minimumAltitude,
            float maximumAltitude,
            bool autonomous)
        {
            UnitId = unitId ?? string.Empty;
            UnitName = unitName ?? string.Empty;
            Role = role;

            MaximumSpeed =
                Math.Max(0f, maximumSpeed);

            MinimumAltitude =
                Math.Max(0f, minimumAltitude);

            MaximumAltitude =
                Math.Max(
                    MinimumAltitude,
                    maximumAltitude);

            Autonomous = autonomous;
        }
    }

    public sealed class WardenAirRosterIntegration
    {
        private readonly Dictionary<string, WardenAirUnitProfile> roster =
            new Dictionary<string, WardenAirUnitProfile>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterUnit(
            string unitId,
            string unitName,
            WardenAirUnitRole role,
            float maximumSpeed,
            float minimumAltitude,
            float maximumAltitude,
            bool autonomous)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            roster[unitId] =
                new WardenAirUnitProfile(
                    unitId,
                    unitName,
                    role,
                    maximumSpeed,
                    minimumAltitude,
                    maximumAltitude,
                    autonomous);
        }

        public bool IsRegistered(
            string unitId)
        {
            return roster.ContainsKey(unitId);
        }

        public bool TryGetUnit(
            string unitId,
            out WardenAirUnitProfile profile)
        {
            return roster.TryGetValue(
                unitId,
                out profile);
        }

        public IReadOnlyCollection<WardenAirUnitProfile> GetRoster()
        {
            return roster.Values;
        }

        public void RemoveUnit(
            string unitId)
        {
            roster.Remove(unitId);
        }

        public void Clear()
        {
            roster.Clear();
        }
    }
}
