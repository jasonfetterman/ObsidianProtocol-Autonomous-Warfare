using UnityEngine;

namespace ObsidianProtocol.Game.Combat.SensorDamage
{
    public sealed class SensorDamageSystem
    {
        public float MaxSensorIntegrity { get; private set; }
        public float CurrentSensorIntegrity { get; private set; }

        public float IntegrityPercent =>
            MaxSensorIntegrity <= 0f
                ? 0f
                : CurrentSensorIntegrity / MaxSensorIntegrity;

        public bool IsDisabled =>
            CurrentSensorIntegrity <= 0f;

        public SensorDamageSystem(float maxIntegrity)
        {
            MaxSensorIntegrity =
                Mathf.Max(0f, maxIntegrity);

            CurrentSensorIntegrity =
                MaxSensorIntegrity;
        }

        public void ApplyDamage(float amount)
        {
            CurrentSensorIntegrity =
                Mathf.Clamp(
                    CurrentSensorIntegrity -
                    Mathf.Max(0f, amount),
                    0f,
                    MaxSensorIntegrity);
        }

        public void Repair(float amount)
        {
            CurrentSensorIntegrity =
                Mathf.Clamp(
                    CurrentSensorIntegrity +
                    Mathf.Max(0f, amount),
                    0f,
                    MaxSensorIntegrity);
        }

        public float GetSensorEffectiveness()
        {
            return Mathf.Clamp01(IntegrityPercent);
        }

        public void Reset()
        {
            CurrentSensorIntegrity =
                MaxSensorIntegrity;
        }
    }
}
