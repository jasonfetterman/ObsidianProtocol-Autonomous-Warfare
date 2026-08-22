using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.NavalWarfare
{
    public enum WardenSeaUnitRole
    {
        Reconnaissance,
        Patrol,
        Survey,
        Sonar,
        Rescue,
        Logistics,
        Harbor,
        Combat
    }

    public enum WardenSeaDomain
    {
        Surface,
        Underwater,
        Coastal,
        MultiDomain
    }

    public sealed class WardenSeaUnitProfile
    {
        public string UnitId { get; }
        public string UnitName { get; }

        public WardenSeaUnitRole Role { get; }
        public WardenSeaDomain Domain { get; }

        public float MaximumSpeed { get; }
        public float MaximumDepth { get; }
        public float SensorRange { get; }

        public bool Autonomous { get; }

        public WardenSeaUnitProfile(
            string unitId,
            string unitName,
            WardenSeaUnitRole role,
            WardenSeaDomain domain,
            float maximumSpeed,
            float maximumDepth,
            float sensorRange,
            bool autonomous)
        {
            UnitId =
                unitId ?? string.Empty;

            UnitName =
                unitName ?? string.Empty;

            Role =
                role;

            Domain =
                domain;

            MaximumSpeed =
                Math.Max(
                    0f,
                    maximumSpeed);

            MaximumDepth =
                Math.Max(
                    0f,
                    maximumDepth);

            SensorRange =
                Math.Max(
                    0f,
                    sensorRange);

            Autonomous =
                autonomous;
        }
    }

    public sealed class WardenSeaRosterIntegration
    {
        private readonly Dictionary<string, WardenSeaUnitProfile> roster =
            new Dictionary<string, WardenSeaUnitProfile>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterUnit(
            string unitId,
            string unitName,
            WardenSeaUnitRole role,
            WardenSeaDomain domain,
            float maximumSpeed,
            float maximumDepth,
            float sensorRange,
            bool autonomous)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            roster[unitId] =
                new WardenSeaUnitProfile(
                    unitId,
                    unitName,
                    role,
                    domain,
                    maximumSpeed,
                    maximumDepth,
                    sensorRange,
                    autonomous);
        }

        public bool IsRegistered(
            string unitId)
        {
            return roster.ContainsKey(unitId);
        }

        public bool TryGetUnit(
            string unitId,
            out WardenSeaUnitProfile profile)
        {
            return roster.TryGetValue(
                unitId,
                out profile);
        }

        public IReadOnlyCollection<WardenSeaUnitProfile> GetRoster()
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
