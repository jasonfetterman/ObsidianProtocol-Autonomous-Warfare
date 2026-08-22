using UnityEngine;

namespace ObsidianProtocol.Game.Combat.MobilityDamage
{
    public sealed class MobilityDamageSystem
    {
        public float MaxMobility { get; private set; }
        public float CurrentMobility { get; private set; }

        public float MobilityPercent =>
            MaxMobility <= 0f
                ? 0f
                : CurrentMobility / MaxMobility;

        public bool IsImmobilized =>
            CurrentMobility <= 0f;

        public MobilityDamageSystem(float maxMobility)
        {
            MaxMobility = Mathf.Max(0f, maxMobility);
            CurrentMobility = MaxMobility;
        }

        public void ApplyDamage(float amount)
        {
            CurrentMobility =
                Mathf.Clamp(
                    CurrentMobility - Mathf.Max(0f, amount),
                    0f,
                    MaxMobility);
        }

        public void Repair(float amount)
        {
            CurrentMobility =
                Mathf.Clamp(
                    CurrentMobility + Mathf.Max(0f, amount),
                    0f,
                    MaxMobility);
        }

        public float GetMovementMultiplier()
        {
            return Mathf.Clamp01(MobilityPercent);
        }

        public void Reset()
        {
            CurrentMobility = MaxMobility;
        }
    }
}
