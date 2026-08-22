using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.CommandUnits
{
    public enum WardenCommandUnitRole
    {
        Archive,
        Worldmap,
        CommandCore,
        Fusion,
        Nexus,
        Insight,
        Pulse,
        VectorCore
    }

    public enum WardenCommandDomain
    {
        Strategic,
        Intelligence,
        Network,
        Fleet,
        Data,
        Analytics,
        MultiDomain
    }

    public sealed class WardenCommandUnitProfile
    {
        public string UnitId { get; }
        public string UnitName { get; }

        public WardenCommandUnitRole Role { get; }
        public WardenCommandDomain Domain { get; }

        public float CommandRange { get; }
        public float ProcessingCapacity { get; }
        public float NetworkCapacity { get; }

        public bool Autonomous { get; }

        public WardenCommandUnitProfile(
            string unitId,
            string unitName,
            WardenCommandUnitRole role,
            WardenCommandDomain domain,
            float commandRange,
            float processingCapacity,
            float networkCapacity,
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

            CommandRange =
                Math.Max(
                    0f,
                    commandRange);

            ProcessingCapacity =
                Math.Max(
                    0f,
                    processingCapacity);

            NetworkCapacity =
                Math.Max(
                    0f,
                    networkCapacity);

            Autonomous =
                autonomous;
        }
    }

    public sealed class WardenCommandRosterIntegration
    {
        private readonly Dictionary<string, WardenCommandUnitProfile> roster =
            new Dictionary<string, WardenCommandUnitProfile>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterUnit(
            string unitId,
            string unitName,
            WardenCommandUnitRole role,
            WardenCommandDomain domain,
            float commandRange,
            float processingCapacity,
            float networkCapacity,
            bool autonomous)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            roster[unitId] =
                new WardenCommandUnitProfile(
                    unitId,
                    unitName,
                    role,
                    domain,
                    commandRange,
                    processingCapacity,
                    networkCapacity,
                    autonomous);
        }

        public bool IsRegistered(
            string unitId)
        {
            return roster.ContainsKey(unitId);
        }

        public bool TryGetUnit(
            string unitId,
            out WardenCommandUnitProfile profile)
        {
            return roster.TryGetValue(
                unitId,
                out profile);
        }

        public IReadOnlyCollection<WardenCommandUnitProfile> GetRoster()
        {
            return roster.Values;
        }

        public int Count()
        {
            return roster.Count;
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
