using System;

namespace ObsidianProtocol.Game.VR
{
    public enum OperatorDamageType
    {
        None,
        Impact,
        Ballistic,
        Explosive,
        Thermal,
        Electrical,
        Structural
    }

    public sealed class OperatorDamageFeedback
    {
        public bool Initialized { get; private set; }

        public bool Active { get; private set; }

        public string UnitId { get; private set; }

        public OperatorDamageType LastDamageType { get; private set; }

        public float LastDamageAmount { get; private set; }

        public float CurrentIntegrity { get; private set; }

        public bool CriticalDamage =>
            CurrentIntegrity <= 25f;

        public bool Initialize(
            string unitId,
            float startingIntegrity)
        {
            if (Initialized ||
                string.IsNullOrWhiteSpace(unitId) ||
                startingIntegrity < 0f ||
                startingIntegrity > 100f)
            {
                return false;
            }

            UnitId =
                unitId.Trim();

            CurrentIntegrity =
                startingIntegrity;

            LastDamageType =
                OperatorDamageType.None;

            LastDamageAmount = 0f;

            Active = true;
            Initialized = true;

            return true;
        }

        public bool ApplyDamage(
            OperatorDamageType damageType,
            float damageAmount)
        {
            if (!Initialized ||
                !Active ||
                damageType == OperatorDamageType.None ||
                damageAmount < 0f)
            {
                return false;
            }

            LastDamageType =
                damageType;

            LastDamageAmount =
                damageAmount;

            CurrentIntegrity =
                Math.Max(
                    0f,
                    CurrentIntegrity - damageAmount);

            return true;
        }

        public bool Repair(
            float repairAmount)
        {
            if (!Initialized ||
                repairAmount < 0f)
            {
                return false;
            }

            CurrentIntegrity =
                Math.Min(
                    100f,
                    CurrentIntegrity + repairAmount);

            return true;
        }

        public void ClearFeedback()
        {
            LastDamageType =
                OperatorDamageType.None;

            LastDamageAmount = 0f;
        }

        public void Reset()
        {
            Initialized = false;
            Active = false;

            UnitId =
                string.Empty;

            LastDamageType =
                OperatorDamageType.None;

            LastDamageAmount = 0f;
            CurrentIntegrity = 0f;
        }
    }
}
