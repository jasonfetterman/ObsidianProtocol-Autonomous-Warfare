using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.AI.Commanders
{
    public enum LogisticsDecisionType
    {
        None,
        MaintainSupply,
        ResupplyForwardForce,
        ProtectSupplyRoute,
        EstablishForwardLogistics,
        RecoverDamagedForce,
        RedirectLogistics,
        PrioritizeFuel,
        PrioritizeEnergy,
        EmergencyLogistics
    }

    public enum LogisticsPriority
    {
        None,
        Emergency,
        Critical,
        High,
        Normal,
        Low
    }

    public sealed class LogisticsDecision
    {
        public string DecisionId { get; }

        public string LogisticsGroupId
        {
            get;
            private set;
        }

        public LogisticsDecisionType Type
        {
            get;
            private set;
        }

        public LogisticsPriority Priority
        {
            get;
            private set;
        }

        public float Urgency
        {
            get;
            private set;
        }

        public float Allocation
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
                LogisticsGroupId) &&
            Type !=
                LogisticsDecisionType.None &&
            Priority !=
                LogisticsPriority.None &&
            !string.IsNullOrWhiteSpace(
                IntentId);

        public LogisticsDecision(
            string decisionId,
            string logisticsGroupId)
        {
            DecisionId =
                decisionId ?? string.Empty;

            LogisticsGroupId =
                logisticsGroupId ?? string.Empty;

            Type =
                LogisticsDecisionType.None;

            Priority =
                LogisticsPriority.None;

            Urgency = 0.0f;
            Allocation = 0.0f;

            IntentId = string.Empty;
            Active = false;
        }

        public void Configure(
            LogisticsDecisionType type,
            LogisticsPriority priority,
            float urgency,
            float allocation,
            string intentId)
        {
            Type = type;
            Priority = priority;

            Urgency =
                Clamp01(
                    urgency);

            Allocation =
                Clamp01(
                    allocation);

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

    public sealed class LogisticsDecisionPlan
    {
        private readonly List<
            LogisticsDecision> decisions =
            new List<
                LogisticsDecision>();

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
            LogisticsDecision decision)
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
            LogisticsDecision>
            GetDecisions()
        {
            return decisions;
        }
    }

    public sealed class LogisticsDecisionPlanner
    {
        public LogisticsDecisionPlan CreatePlan(
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

            LogisticsDecisionPlan plan =
                new LogisticsDecisionPlan();

            /*
             * Critical logistics failure takes
             * priority over normal operations.
             */
            if (battlefield.LogisticsAdvantage <
                -0.60f)
            {
                AddDecision(
                    plan,
                    "LOGISTICS_EMERGENCY",
                    "COMMAND_LOGISTICS",
                    LogisticsDecisionType.EmergencyLogistics,
                    LogisticsPriority.Emergency,
                    1.0f,
                    0.90f,
                    "INTENT_LOGISTICS_EMERGENCY");

                plan.Activate();

                return plan;
            }

            /*
             * Significant logistics disadvantage
             * requires forward resupply and route
             * protection.
             */
            if (battlefield.LogisticsAdvantage <
                -0.35f)
            {
                AddDecision(
                    plan,
                    "LOGISTICS_RESUPPLY",
                    "FORWARD_FORCE",
                    LogisticsDecisionType.ResupplyForwardForce,
                    LogisticsPriority.Critical,
                    0.90f,
                    0.70f,
                    "INTENT_LOGISTICS_RESUPPLY");

                AddDecision(
                    plan,
                    "LOGISTICS_ROUTE",
                    "SUPPLY_NETWORK",
                    LogisticsDecisionType.ProtectSupplyRoute,
                    LogisticsPriority.High,
                    0.80f,
                    0.50f,
                    "INTENT_LOGISTICS_PROTECT_ROUTE");

                plan.Activate();

                return plan;
            }

            /*
             * Offensive operations beyond the current
             * logistics position may require a forward
             * logistics structure.
             */
            if (objective.Type ==
                    AICommanderObjective.Attack &&
                battlefield.LogisticsAdvantage <
                    0.10f)
            {
                AddDecision(
                    plan,
                    "LOGISTICS_FORWARD",
                    "FORWARD_BASE",
                    LogisticsDecisionType.EstablishForwardLogistics,
                    LogisticsPriority.High,
                    0.70f,
                    0.60f,
                    "INTENT_LOGISTICS_FORWARD");

                AddDecision(
                    plan,
                    "LOGISTICS_SUPPLY",
                    "ATTACK_FORCE",
                    LogisticsDecisionType.MaintainSupply,
                    LogisticsPriority.High,
                    0.65f,
                    0.55f,
                    "INTENT_LOGISTICS_MAINTAIN_SUPPLY");

                plan.Activate();

                return plan;
            }

            /*
             * Weak force conditions combined with normal
             * logistics indicate damaged or depleted
             * forces should be recovered.
             */
            if (forces.ForceAdvantage <
                -0.10f)
            {
                AddDecision(
                    plan,
                    "LOGISTICS_RECOVERY",
                    "DAMAGED_FORCE",
                    LogisticsDecisionType.RecoverDamagedForce,
                    LogisticsPriority.Normal,
                    0.55f,
                    0.40f,
                    "INTENT_LOGISTICS_RECOVER_FORCE");

                plan.Activate();

                return plan;
            }

            /*
             * Strong operations can maintain normal
             * supply without additional restructuring.
             */
            AddDecision(
                plan,
                "LOGISTICS_MAINTAIN",
                "ACTIVE_FORCE",
                LogisticsDecisionType.MaintainSupply,
                LogisticsPriority.Normal,
                0.30f,
                0.25f,
                "INTENT_LOGISTICS_MAINTAIN");

            plan.Activate();

            return plan;
        }

        private static void AddDecision(
            LogisticsDecisionPlan plan,
            string decisionId,
            string logisticsGroupId,
            LogisticsDecisionType type,
            LogisticsPriority priority,
            float urgency,
            float allocation,
            string intentId)
        {
            LogisticsDecision decision =
                new LogisticsDecision(
                    decisionId,
                    logisticsGroupId);

            decision.Configure(
                type,
                priority,
                urgency,
                allocation,
                intentId);

            plan.AddDecision(
                decision);
        }
    }
}
