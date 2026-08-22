using System;

namespace ObsidianProtocol.Game.AI.Commanders
{
    public enum BattlefieldThreatLevel
    {
        None,
        Low,
        Moderate,
        High,
        Critical
    }

    public sealed class BattlefieldEvaluation
    {
        public float BattlefieldTime { get; private set; }

        public float FriendlyForceStrength { get; private set; }
        public float EnemyForceStrength { get; private set; }

        public float FriendlyTerritoryControl { get; private set; }
        public float EnemyTerritoryControl { get; private set; }

        public float FriendlyResourceSecurity { get; private set; }
        public float EnemyResourceSecurity { get; private set; }

        public float FriendlyLogisticsHealth { get; private set; }
        public float EnemyLogisticsHealth { get; private set; }

        public float FriendlyReconCoverage { get; private set; }
        public float EnemyReconCoverage { get; private set; }

        public float FriendlyMorale { get; private set; }
        public float EnemyMorale { get; private set; }

        public BattlefieldThreatLevel ThreatLevel
        {
            get;
            private set;
        }

        public bool Valid { get; private set; }

        public BattlefieldEvaluation()
        {
            Reset();
        }

        public void Reset()
        {
            BattlefieldTime = 0.0f;

            FriendlyForceStrength = 0.0f;
            EnemyForceStrength = 0.0f;

            FriendlyTerritoryControl = 0.0f;
            EnemyTerritoryControl = 0.0f;

            FriendlyResourceSecurity = 0.0f;
            EnemyResourceSecurity = 0.0f;

            FriendlyLogisticsHealth = 0.0f;
            EnemyLogisticsHealth = 0.0f;

            FriendlyReconCoverage = 0.0f;
            EnemyReconCoverage = 0.0f;

            FriendlyMorale = 0.0f;
            EnemyMorale = 0.0f;

            ThreatLevel =
                BattlefieldThreatLevel.None;

            Valid = false;
        }

        public void UpdateTime(
            float battlefieldTime)
        {
            BattlefieldTime =
                Math.Max(
                    0.0f,
                    battlefieldTime);
        }

        public void SetForceStrength(
            float friendly,
            float enemy)
        {
            FriendlyForceStrength =
                Clamp01(friendly);

            EnemyForceStrength =
                Clamp01(enemy);

            RecalculateThreat();
        }

        public void SetTerritoryControl(
            float friendly,
            float enemy)
        {
            FriendlyTerritoryControl =
                Clamp01(friendly);

            EnemyTerritoryControl =
                Clamp01(enemy);
        }

        public void SetResourceSecurity(
            float friendly,
            float enemy)
        {
            FriendlyResourceSecurity =
                Clamp01(friendly);

            EnemyResourceSecurity =
                Clamp01(enemy);
        }

        public void SetLogisticsHealth(
            float friendly,
            float enemy)
        {
            FriendlyLogisticsHealth =
                Clamp01(friendly);

            EnemyLogisticsHealth =
                Clamp01(enemy);
        }

        public void SetReconCoverage(
            float friendly,
            float enemy)
        {
            FriendlyReconCoverage =
                Clamp01(friendly);

            EnemyReconCoverage =
                Clamp01(enemy);
        }

        public void SetMorale(
            float friendly,
            float enemy)
        {
            FriendlyMorale =
                Clamp01(friendly);

            EnemyMorale =
                Clamp01(enemy);
        }

        public void FinalizeEvaluation()
        {
            RecalculateThreat();
            Valid = true;
        }

        public float ForceAdvantage
        {
            get
            {
                return FriendlyForceStrength -
                       EnemyForceStrength;
            }
        }

        public float TerritoryAdvantage
        {
            get
            {
                return FriendlyTerritoryControl -
                       EnemyTerritoryControl;
            }
        }

        public float LogisticsAdvantage
        {
            get
            {
                return FriendlyLogisticsHealth -
                       EnemyLogisticsHealth;
            }
        }

        public float ReconAdvantage
        {
            get
            {
                return FriendlyReconCoverage -
                       EnemyReconCoverage;
            }
        }

        public float MoraleAdvantage
        {
            get
            {
                return FriendlyMorale -
                       EnemyMorale;
            }
        }

        private void RecalculateThreat()
        {
            float enemyPressure =
                EnemyForceStrength -
                FriendlyForceStrength;

            float territoryPressure =
                EnemyTerritoryControl -
                FriendlyTerritoryControl;

            float logisticsPressure =
                EnemyLogisticsHealth -
                FriendlyLogisticsHealth;

            float pressure =
                Math.Max(
                    0.0f,
                    (enemyPressure +
                     territoryPressure +
                     logisticsPressure) /
                    3.0f);

            if (pressure >= 0.75f)
            {
                ThreatLevel =
                    BattlefieldThreatLevel.Critical;
            }
            else if (pressure >= 0.50f)
            {
                ThreatLevel =
                    BattlefieldThreatLevel.High;
            }
            else if (pressure >= 0.25f)
            {
                ThreatLevel =
                    BattlefieldThreatLevel.Moderate;
            }
            else if (pressure > 0.0f)
            {
                ThreatLevel =
                    BattlefieldThreatLevel.Low;
            }
            else
            {
                ThreatLevel =
                    BattlefieldThreatLevel.None;
            }
        }

        private static float Clamp01(
            float value)
        {
            return Math.Max(
                0.0f,
                Math.Min(
                    1.0f,
                    value));
        }
    }
}
