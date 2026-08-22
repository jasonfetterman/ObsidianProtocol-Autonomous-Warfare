using System;

namespace ObsidianProtocol.Game.Units.Experimental
{
    public enum ExperimentalUnitRole
    {
        Prototype,
        AdvancedCombat,
        AdvancedReconnaissance,
        ElectronicWarfare,
        ExperimentalSupport,
        StrategicPrototype
    }

    public sealed class ExperimentalUnitFramework
    {
        public string UnitId { get; }
        public string DisplayName { get; }

        public ExperimentalUnitRole Role { get; private set; }

        public float Stability { get; private set; }
        public float PowerOutput { get; private set; }
        public float SensorRange { get; private set; }

        public bool RequiresTesting { get; private set; }
        public bool Operational { get; private set; }

        public ExperimentalUnitFramework(
            string unitId,
            string displayName)
        {
            UnitId = unitId ?? string.Empty;
            DisplayName = displayName ?? string.Empty;

            Role = ExperimentalUnitRole.Prototype;

            Stability = 0f;
            PowerOutput = 0f;
            SensorRange = 0f;

            RequiresTesting = true;
            Operational = false;
        }

        public void Configure(
            ExperimentalUnitRole role,
            float stability,
            float powerOutput,
            float sensorRange)
        {
            Role = role;

            Stability =
                Math.Clamp(stability, 0f, 1f);

            PowerOutput =
                Math.Max(0f, powerOutput);

            SensorRange =
                Math.Max(0f, sensorRange);
        }

        public void CompleteTesting()
        {
            RequiresTesting = false;
        }

        public void Activate()
        {
            if (RequiresTesting)
            {
                return;
            }

            Operational = true;
        }

        public void Deactivate()
        {
            Operational = false;
        }
    }
}
