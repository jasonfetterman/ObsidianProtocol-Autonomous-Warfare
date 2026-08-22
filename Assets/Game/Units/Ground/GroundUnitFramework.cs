using System;

namespace ObsidianProtocol.Game.Units.Ground
{
    public enum GroundUnitRole
    {
        Combat,
        Reconnaissance,
        Support,
        Logistics,
        Engineering,
        Transport,
        Command
    }

    public sealed class GroundUnitFramework
    {
        public string UnitId { get; }
        public string DisplayName { get; }

        public GroundUnitRole Role { get; private set; }

        public float MaxSpeed { get; private set; }
        public float ArmorRating { get; private set; }
        public float SensorRange { get; private set; }

        public bool CanTraverseTerrain { get; private set; }
        public bool Operational { get; private set; }

        public GroundUnitFramework(
            string unitId,
            string displayName)
        {
            UnitId = unitId ?? string.Empty;
            DisplayName = displayName ?? string.Empty;

            Role = GroundUnitRole.Combat;

            MaxSpeed = 0f;
            ArmorRating = 0f;
            SensorRange = 0f;

            CanTraverseTerrain = true;
            Operational = false;
        }

        public void Configure(
            GroundUnitRole role,
            float maxSpeed,
            float armorRating,
            float sensorRange)
        {
            Role = role;
            MaxSpeed = Math.Max(0f, maxSpeed);
            ArmorRating = Math.Max(0f, armorRating);
            SensorRange = Math.Max(0f, sensorRange);
        }

        public void Activate()
        {
            Operational = true;
        }

        public void Deactivate()
        {
            Operational = false;
        }
    }
}
