using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.AI.Commanders
{
    public enum ArmyOperationalState
    {
        None,
        Organizing,
        Reconnaissance,
        Advancing,
        Attacking,
        Defending,
        Reinforcing,
        Regrouping,
        Retreating,
        Sieging,
        Consolidating
    }

    public enum ArmyReadiness
    {
        None,
        Critical,
        Low,
        Moderate,
        High,
        Full
    }

    public sealed class ArmyManagementState
    {
        public string ArmyId { get; }

        public ArmyOperationalState OperationalState
        {
            get;
            private set;
        }

        public ArmyReadiness Readiness
        {
            get;
            private set;
        }

        public float ForceStrength
        {
            get;
            private set;
        }

        public float LogisticsStrength
        {
            get;
            private set;
        }

        public float ReconnaissanceStrength
        {
            get;
            private set;
        }

        public float BattlefieldControl
        {
            get;
            private set;
        }

        public float ThreatLevel
        {
            get;
            private set;
        }

        public string PrimaryIntentId
        {
            get;
            private set;
        }

        public bool Active
        {
            get;
            private set;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(
                ArmyId) &&
            OperationalState !=
                ArmyOperationalState.None &&
            Readiness !=
                ArmyReadiness.None &&
            !string.IsNullOrWhiteSpace(
                PrimaryIntentId);

        public ArmyManagementState(
            string armyId)
        {
            ArmyId =
                armyId ?? string.Empty;

            OperationalState =
                ArmyOperationalState.None;

            Readiness =
                ArmyReadiness.None;

            ForceStrength = 0.0f;
            LogisticsStrength = 0.0f;
            ReconnaissanceStrength = 0.0f;
            BattlefieldControl = 0.0f;
            ThreatLevel = 0.0f;

            PrimaryIntentId = string.Empty;
            Active = false;
        }

        public void UpdateAssessment(
            float forceStrength,
            float logisticsStrength,
            float reconnaissanceStrength,
            float battlefieldControl,
            float threatLevel)
        {
            ForceStrength =
                Clamp01(
                    forceStrength);

            LogisticsStrength =
                Clamp01(
                    logisticsStrength);

            ReconnaissanceStrength =
                Clamp01(
                    reconnaissanceStrength);

            BattlefieldControl =
                Clamp01(
                    battlefieldControl);

            ThreatLevel =
                Clamp01(
                    threatLevel);

            Readiness =
                DetermineReadiness();

            Active = true;
        }

        public void SetOperationalState(
            ArmyOperationalState state,
            string intentId)
        {
            OperationalState = state;

            PrimaryIntentId =
                intentId ?? string.Empty;
        }

        private ArmyReadiness DetermineReadiness()
        {
            float average =
                (ForceStrength +
                 LogisticsStrength +
                 ReconnaissanceStrength) /
                3.0f;

            if (ThreatLevel >= 0.85f)
                return ArmyReadiness.Critical;

            if (average < 0.25f)
                return ArmyReadiness.Low;

            if (average < 0.50f)
                return ArmyReadiness.Moderate;

            if (average < 0.80f)
                return ArmyReadiness.High;

            return ArmyReadiness.Full;
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

    public sealed class ArmyManagementPlan
    {
        private readonly List<
            ArmyManagementState> armies =
            new List<
                ArmyManagementState>();

        public bool Active
        {
            get;
            private set;
        }

        public bool Valid =>
            Active &&
            armies.Count > 0;

        public int ArmyCount =>
            armies.Count;

        public bool AddArmy(
            ArmyManagementState army)
        {
            if (army == null ||
                !army.Valid)
            {
                return false;
            }

            if (armies.Count >= 32)
                return false;

            armies.Add(army);

            return true;
        }

        public void Activate()
        {
            if (armies.Count > 0)
                Active = true;
        }

        public void Deactivate()
        {
            Active = false;
        }

        public void Clear()
        {
            armies.Clear();
            Active = false;
        }

        public IReadOnlyCollection<
            ArmyManagementState>
            GetArmies()
        {
            return armies;
        }
    }

    public sealed class AutonomousArmyManager
    {
        public ArmyManagementPlan EvaluateArmy(
            BattlefieldEvaluation battlefield,
            ForceEvaluation forces,
            StrategicObjective objective)
        {
            if (battlefield == null ||
                !battlefield.Valid ||
                forces == null ||
                !forces.Valid ||
                objective == null ||
                !objective.Valid)
            {
                return null;
            }

            ArmyManagementPlan plan =
                new ArmyManagementPlan();

            ArmyManagementState army =
                new ArmyManagementState(
                    "PRIMARY_ARMY");

            float forceStrength =
                Clamp01(
                    0.50f +
                    forces.ForceAdvantage);

            float logisticsStrength =
                Clamp01(
                    0.50f +
                    battlefield.LogisticsAdvantage);

            float reconnaissanceStrength =
                Clamp01(
                    0.50f +
                    battlefield.ReconAdvantage);

            float battlefieldControl =
                Clamp01(
                    0.50f +
                    battlefield.TerritoryAdvantage);

            float threatLevel =
                GetThreatValue(
                    battlefield.ThreatLevel);

            army.UpdateAssessment(
                forceStrength,
                logisticsStrength,
                reconnaissanceStrength,
                battlefieldControl,
                threatLevel);

            ArmyOperationalState state;
            string intentId;

            /*
             * Emergency conditions always take priority.
             */
            if (threatLevel >= 0.85f)
            {
                state =
                    ArmyOperationalState.Retreating;

                intentId =
                    "INTENT_ARMY_EMERGENCY_RETREAT";
            }
            else if (forces.ForceAdvantage <
                     -0.30f)
            {
                state =
                    ArmyOperationalState.Regrouping;

                intentId =
                    "INTENT_ARMY_REGROUP";
            }
            else if (logisticsStrength <
                     0.30f)
            {
                state =
                    ArmyOperationalState.Reinforcing;

                intentId =
                    "INTENT_ARMY_REINFORCE";
            }
            else if (reconnaissanceStrength <
                     0.30f)
            {
                state =
                    ArmyOperationalState.Reconnaissance;

                intentId =
                    "INTENT_ARMY_RECON";
            }
            else if (objective.Type ==
                     AICommanderObjective.Attack)
            {
                /*
                 * Siege classification is intentionally
                 * based only on systems already present.
                 *
                 * A high defensive/territorial resistance
                 * combined with a viable attacking force
                 * enters siege posture.
                 */
                if (battlefield.TerritoryAdvantage <
                        -0.25f &&
                    forces.ForceAdvantage >=
                        0.20f)
                {
                    state =
                        ArmyOperationalState.Sieging;

                    intentId =
                        "INTENT_ARMY_SIEGE";
                }
                else
                {
                    state =
                        ArmyOperationalState.Attacking;

                    intentId =
                        "INTENT_ARMY_ATTACK";
                }
            }
            else if (objective.Type ==
                     AICommanderObjective.Defend ||
                     objective.Type ==
                     AICommanderObjective.Hold)
            {
                state =
                    ArmyOperationalState.Defending;

                intentId =
                    "INTENT_ARMY_DEFEND";
            }
            else
            {
                state =
                    ArmyOperationalState.Organizing;

                intentId =
                    "INTENT_ARMY_ORGANIZE";
            }

            army.SetOperationalState(
                state,
                intentId);

            plan.AddArmy(
                army);

            plan.Activate();

            return plan;
        }

        public ArmyOperationalState
            DetermineOperationalState(
                ArmyManagementState army)
        {
            if (army == null ||
                !army.Valid)
            {
                return ArmyOperationalState.None;
            }

            return army.OperationalState;
        }

        private static float GetThreatValue(
            BattlefieldThreatLevel level)
        {
            /*
             * Only use threat levels that are already
             * established by the existing project.
             * Do not introduce a new enum value here.
             */
            switch (level)
            {
                case BattlefieldThreatLevel.Critical:
                    return 1.0f;

                case BattlefieldThreatLevel.High:
                    return 0.80f;

                case BattlefieldThreatLevel.Low:
                    return 0.30f;

                default:
                    return 0.0f;
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
