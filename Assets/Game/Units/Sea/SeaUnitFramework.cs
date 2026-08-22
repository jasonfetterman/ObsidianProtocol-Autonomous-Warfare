using System;

namespace ObsidianProtocol.Game.Units.Sea
{
    public enum SeaUnitRole
    {
        Combat,
        Reconnaissance,
        Surveillance,
        Support,
        Logistics,
        SearchAndRescue,
        Command
    }

    public sealed class SeaUnitFramework
    {
        public string UnitId { get; }
        public string DisplayName { get; }

        public SeaUnitRole Role { get; private set; }

        public float MaxSpeed { get; private set; }
        public float SensorRange { get; private set; }
        public float OperationalDepth { get; private set; }

        public bool CanOperateAtSea { get; private set; }
        public bool Operational { get; private set; }

        public SeaUnitFramework(
            string unitId,
            string displayName)
        {
            UnitId = unitId ?? string.Empty;
            DisplayName = displayName ?? string.Empty;

            Role = SeaUnitRole.Combat;

            MaxSpeed = 0f;
            SensorRange = 0f;
            OperationalDepth = 0f;

            CanOperateAtSea = true;
            Operational = false;
        }

        public void Configure(
            SeaUnitRole role,
            float maxSpeed,
            float sensorRange,
            float operationalDepth)
        {
            Role = role;
            MaxSpeed = Math.Max(0f, maxSpeed);
            SensorRange = Math.Max(0f, sensorRange);
            OperationalDepth = Math.Max(0f, operationalDepth);
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
