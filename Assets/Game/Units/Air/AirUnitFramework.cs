using System;

namespace ObsidianProtocol.Game.Units.Air
{
    public enum AirUnitRole
    {
        Combat,
        Reconnaissance,
        Surveillance,
        Support,
        Logistics,
        Relay,
        SearchAndRescue,
        Command
    }

    public sealed class AirUnitFramework
    {
        public string UnitId { get; }
        public string DisplayName { get; }

        public AirUnitRole Role { get; private set; }

        public float MaxSpeed { get; private set; }
        public float MaxAltitude { get; private set; }
        public float SensorRange { get; private set; }

        public bool CanFly { get; private set; }
        public bool Operational { get; private set; }

        public AirUnitFramework(
            string unitId,
            string displayName)
        {
            UnitId = unitId ?? string.Empty;
            DisplayName = displayName ?? string.Empty;

            Role = AirUnitRole.Combat;

            MaxSpeed = 0f;
            MaxAltitude = 0f;
            SensorRange = 0f;

            CanFly = true;
            Operational = false;
        }

        public void Configure(
            AirUnitRole role,
            float maxSpeed,
            float maxAltitude,
            float sensorRange)
        {
            Role = role;
            MaxSpeed = Math.Max(0f, maxSpeed);
            MaxAltitude = Math.Max(0f, maxAltitude);
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
