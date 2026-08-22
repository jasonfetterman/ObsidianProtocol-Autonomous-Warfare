using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.AI.Commanders
{
    public enum RetreatReason
    {
        None,
        CriticalThreat,
        ForceDefeat,
        UnsustainableLosses,
        EncirclementRisk,
        LogisticsFailure,
        ObjectiveLoss,
        StrategicWithdrawal
    }

    public enum RetreatPriority
    {
        None,
        Emergency,
        Immediate,
        High,
        Controlled,
        Planned
    }

    public enum RetreatPosture
    {
        None,
        EmergencyWithdrawal,
        FightingWithdrawal,
        Fallback,
        Regroup,
        StrategicWithdrawal
    }

    public sealed class RetreatDecision
    {
        public string DecisionId { get; }

        public string ForceGroupId
        {
            get;
            private set;
        }

        public RetreatReason Reason
        {
            get;
            private set;
        }

        public RetreatPriority Priority
        {
            get;
            private set;
        }

        public RetreatPosture Posture
        {
            get;
            private set;
        }

        public float Urgency
        {
            get;
            private set;
        }

        public float ForcePreservation
        {
            get;
            private set;
        }

        public bool PreserveObjective
        {
            get;
            private set;
        }

        public string IntentId
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
                DecisionId) &&
            !string.IsNullOrWhiteSpace(
                ForceGroupId) &&
            Reason !=
                RetreatReason.None &&
            Priority !=
                RetreatPriority.None &&
            Posture !=
                RetreatPosture.None &&
            !string.IsNullOrWhiteSpace(
                IntentId);

        public RetreatDecision(
            string decisionId,
            string forceGroupId)
        {
            DecisionId =
                decisionId ?? string.Empty;

            ForceGroupId =
                forceGroupId ?? string.Empty;

            Reason =
                RetreatReason.None;

            Priority =
                RetreatPriority.None;

            Posture =
                RetreatPosture.None;

            Urgency = 0.0f;
            ForcePreservation = 0.0f;
            PreserveObjective = false;

            IntentId = string.Empty;
            Active = false;
        }

        public void Configure(
            RetreatReason reason,
            RetreatPriority priority,
            RetreatPosture posture,
            float urgency,
            float forcePreservation,
            bool preserveObjective,
            string intentId)
        {
            Reason = reason;
            Priority = priority;
            Posture = posture;

            Urgency =
                Clamp01(
                    urgency);

            ForcePreservation =
                Clamp01(
                    forcePreservation);

            PreserveObjective =
                preserveObjective;

            IntentId =
                intentId ?? string.Empty;

            Active = Valid;
        }

        public void Cancel()
        {
            Active = false;
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

    public sealed class RetreatPlan
    {
        private readonly List<
            RetreatDecision> decisions =
            new List<
                RetreatDecision>();

        public bool Active
        {
            get;
            private set;
        }

        public bool Valid =>
            Active &&
            decisions.Count > 0;

        public int DecisionCount =>
            decisions.Count;

        public bool AddDecision(
            RetreatDecision decision)
        {
            if (decision == null ||
                !decision.Valid)
            {
                return false;
            }

            if (decisions.Count >= 16)
                return false;

            decisions.Add(
                decision);

            return true;
        }

        public void Activate()
        {
            if (decisions.Count > 0)
                Active = true;
        }

        public void Deactivate()
        {
            Active = false;
        }

        public void Clear()
        {
            decisions.Clear();
            Active = false;
        }

        public IReadOnlyCollection<
            RetreatDecision>
            GetDecisions()
        {
            return decisions;
        }
    }

    public sealed class RetreatDecisionPlanner
    {
        public RetreatPlan CreatePlan(
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

            RetreatPlan plan =
                new RetreatPlan();

            /*
             * Critical battlefield conditions override
             * normal strategic objectives.
             */
            if (battlefield.ThreatLevel ==
                BattlefieldThreatLevel.Critical)
            {
                AddDecision(
                    plan,
                    "COMMANDER_EMERGENCY",
                    RetreatReason.CriticalThreat,
                    RetreatPriority.Emergency,
                    RetreatPosture.EmergencyWithdrawal,
                    1.0f,
                    1.0f,
                    false,
                    "INTENT_RETREAT_EMERGENCY");

                plan.Activate();

                return plan;
            }

            /*
             * Severe force disadvantage makes continued
             * engagement potentially unsustainable.
             */
            if (forces.ForceAdvantage <
                -0.40f)
            {
                AddDecision(
                    plan,
                    "COMMANDER_FORCE_DEFEAT",
                    RetreatReason.ForceDefeat,
                    RetreatPriority.Immediate,
                    RetreatPosture.FightingWithdrawal,
                    0.90f,
                    0.95f,
                    false,
                    "INTENT_RETREAT_FORCE_DEFEAT");

                plan.Activate();

                return plan;
            }

            /*
             * Moderate force disadvantage results in a
             * controlled fallback rather than immediate
             * army-wide withdrawal.
             */
            if (forces.ForceAdvantage <
                -0.20f)
            {
                AddDecision(
                    plan,
                    "COMMANDER_FALLBACK",
                    RetreatReason.UnsustainableLosses,
                    RetreatPriority.High,
                    RetreatPosture.Fallback,
                    0.75f,
                    0.85f,
                    true,
                    "INTENT_RETREAT_FALLBACK");

                plan.Activate();

                return plan;
            }

            /*
             * Poor logistics can make an otherwise viable
             * position impossible to maintain.
             */
            if (battlefield.LogisticsAdvantage <
                -0.45f)
            {
                AddDecision(
                    plan,
                    "COMMANDER_LOGISTICS",
                    RetreatReason.LogisticsFailure,
                    RetreatPriority.High,
                    RetreatPosture.Regroup,
                    0.75f,
                    0.90f,
                    true,
                    "INTENT_RETREAT_LOGISTICS");

                plan.Activate();

                return plan;
            }

            /*
             * Losing territorial control can justify a
             * planned strategic withdrawal.
             */
            if (battlefield.TerritoryAdvantage <
                -0.50f &&
                objective.Type !=
                    AICommanderObjective.Attack)
            {
                AddDecision(
                    plan,
                    "COMMANDER_OBJECTIVE_LOSS",
                    RetreatReason.ObjectiveLoss,
                    RetreatPriority.Controlled,
                    RetreatPosture.StrategicWithdrawal,
                    0.60f,
                    0.80f,
                    false,
                    "INTENT_RETREAT_OBJECTIVE_LOSS");

                plan.Activate();

                return plan;
            }

            /*
             * No retreat requirement.
             */
            return plan;
        }

        private static void AddDecision(
            RetreatPlan plan,
            string forceGroupId,
            RetreatReason reason,
            RetreatPriority priority,
            RetreatPosture posture,
            float urgency,
            float forcePreservation,
            bool preserveObjective,
            string intentId)
        {
            RetreatDecision decision =
                new RetreatDecision(
                    "RETREAT_" +
                    forceGroupId,
                    forceGroupId);

            decision.Configure(
                reason,
                priority,
                posture,
                urgency,
                forcePreservation,
                preserveObjective,
                intentId);

            plan.AddDecision(
                decision);
        }
    }
}
