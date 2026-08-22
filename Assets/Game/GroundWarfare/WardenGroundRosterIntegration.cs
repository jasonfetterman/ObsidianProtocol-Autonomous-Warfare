using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.GroundWarfare
{
    public enum WardenGroundUnitRole
    {
        Recon,
        Security,
        Support,
        Transport,
        Construction,
        Rescue,
        Combat
    }

    public sealed class WardenGroundUnitProfile
    {
        public string UnitId { get; }
        public string UnitName { get; }
        public WardenGroundUnitRole Role { get; }

        public float MaximumSpeed { get; }
        public float ArmorRating { get; }
        public float CargoCapacity { get; }

        public bool Autonomous { get; }

        public WardenGroundUnitProfile(
            string unitId,
            string unitName,
            WardenGroundUnitRole role,
            float maximumSpeed,
            float armorRating,
            float cargoCapacity,
            bool autonomous)
        {
            UnitId =
                unitId ?? string.Empty;

            UnitName =
                unitName ?? string.Empty;

            Role =
                role;

            MaximumSpeed =
                Math.Max(
                    0f,
                    maximumSpeed);

            ArmorRating =
                Math.Max(
                    0f,
                    armorRating);

            CargoCapacity =
                Math.Max(
                    0f,
                    cargoCapacity);

            Autonomous =
                autonomous;
        }
    }

    public sealed class WardenGroundRosterIntegration
    {
        private readonly Dictionary<string, WardenGroundUnitProfile> roster =
            new Dictionary<string, WardenGroundUnitProfile>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterUnit(
            string unitId,
            string unitName,
            WardenGroundUnitRole role,
            float maximumSpeed,
            float armorRating,
            float cargoCapacity,
            bool autonomous)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            roster[unitId] =
                new WardenGroundUnitProfile(
                    unitId,
                    unitName,
                    role,
                    maximumSpeed,
                    armorRating,
                    cargoCapacity,
                    autonomous);
        }

        public bool IsRegistered(
            string unitId)
        {
            return roster.ContainsKey(unitId);
        }

        public bool TryGetUnit(
            string unitId,
            out WardenGroundUnitProfile profile)
        {
            return roster.TryGetValue(
                unitId,
                out profile);
        }

        public IReadOnlyCollection<WardenGroundUnitProfile> GetRoster()
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
