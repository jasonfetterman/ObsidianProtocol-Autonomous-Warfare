using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.AI.Commanders
{
    public enum ReinforcementReason
    {
        None,
        CombatLosses,
        LowReadiness,
        StrategicImportance,
        DefensiveEmergency,
        OffensiveOpportunity,
        LogisticsRecovery
    }

    public enum ReinforcementPriority
    {
        None,
        Critical,
        High,
        Normal,
        Low
    }

    public sealed class ReinforcementRequest
    {
        public string RequestId { get; }

        public string ForceGroupId
        {
            get;
            private set;
        }

        public ReinforcementReason Reason
        {
            get;
            private set;
        }

        public ReinforcementPriority Priority
        {
            get;
            private set;
        }

        public float RequestedStrength
        {
            get;
            private set;
        }

        public float Urgency
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
                RequestId) &&
            !string.IsNullOrWhiteSpace(
                ForceGroupId) &&
            Reason !=
                ReinforcementReason.None &&
            Priority !=
                ReinforcementPriority.None &&
            RequestedStrength > 0.0f &&
            !string.IsNullOrWhiteSpace(
                IntentId);

        public ReinforcementRequest(
            string requestId,
            string forceGroupId)
        {
            RequestId =
                requestId ?? string.Empty;

            ForceGroupId =
                forceGroupId ?? string.Empty;

            Reason =
                ReinforcementReason.None;

            Priority =
                ReinforcementPriority.None;

            RequestedStrength = 0.0f;
            Urgency = 0.0f;

            IntentId = string.Empty;
            Active = false;
        }

        public void Configure(
            ReinforcementReason reason,
            ReinforcementPriority priority,
            float requestedStrength,
            float urgency,
            string intentId)
        {
            Reason = reason;
            Priority = priority;

            RequestedStrength =
                Clamp01(
                    requestedStrength);

            Urgency =
                Clamp01(
                    urgency);

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

    public sealed class ReinforcementPlan
    {
        private readonly List<
            ReinforcementRequest> requests =
            new List<
                ReinforcementRequest>();

        public bool Active
        {
            get;
            private set;
        }

        public bool Valid =>
            Active &&
            requests.Count > 0;

        public int RequestCount =>
            requests.Count;

        public bool AddRequest(
            ReinforcementRequest request)
        {
            if (request == null ||
                !request.Valid)
            {
                return false;
            }

            if (requests.Count >= 16)
                return false;

            requests.Add(request);

            return true;
        }

        public void Activate()
        {
            if (requests.Count > 0)
                Active = true;
        }

        public void Deactivate()
        {
            Active = false;
        }

        public void Clear()
        {
            requests.Clear();
            Active = false;
        }

        public IReadOnlyCollection<
            ReinforcementRequest>
            GetRequests()
        {
            return requests;
        }
    }

    public sealed class ReinforcementDecisionPlanner
    {
        public ReinforcementPlan CreatePlan(
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

            ReinforcementPlan plan =
                new ReinforcementPlan();

            /*
             * Critical battlefield pressure takes
             * immediate priority.
             */
            if (battlefield.ThreatLevel ==
                BattlefieldThreatLevel.Critical)
            {
                AddRequest(
                    plan,
                    "COMMANDER_CRITICAL",
                    ReinforcementReason.DefensiveEmergency,
                    ReinforcementPriority.Critical,
                    0.75f,
                    1.0f,
                    "INTENT_REINFORCE_CRITICAL");

                plan.Activate();

                return plan;
            }

            /*
             * Major force disadvantage requires
             * force preservation before committing
             * additional offensive actions.
             */
            if (forces.ForceAdvantage <
                -0.25f)
            {
                AddRequest(
                    plan,
                    "COMMANDER_FORCE_DEFICIT",
                    ReinforcementReason.CombatLosses,
                    ReinforcementPriority.High,
                    0.65f,
                    0.90f,
                    "INTENT_REINFORCE_FORCE_DEFICIT");

                plan.Activate();

                return plan;
            }

            /*
             * Low logistics health creates a
             * recovery reinforcement requirement.
             */
            if (battlefield.LogisticsAdvantage <
                -0.35f)
            {
                AddRequest(
                    plan,
                    "COMMANDER_LOGISTICS",
                    ReinforcementReason.LogisticsRecovery,
                    ReinforcementPriority.High,
                    0.50f,
                    0.80f,
                    "INTENT_REINFORCE_LOGISTICS");

                plan.Activate();

                return plan;
            }

            /*
             * A strong offensive position can justify
             * reinforcing the attacking force before
             * expanding the operation.
             */
            if (objective.Type ==
                    AICommanderObjective.Attack &&
                forces.ForceAdvantage >=
                    0.10f)
            {
                AddRequest(
                    plan,
                    "COMMANDER_OFFENSIVE",
                    ReinforcementReason.OffensiveOpportunity,
                    ReinforcementPriority.Normal,
                    0.30f,
                    0.55f,
                    "INTENT_REINFORCE_OFFENSIVE");

                plan.Activate();

                return plan;
            }

            /*
             * Defending an important position can
             * justify a smaller reserve request.
             */
            if (objective.Type ==
                    AICommanderObjective.Defend ||
                objective.Type ==
                    AICommanderObjective.Hold)
            {
                AddRequest(
                    plan,
                    "COMMANDER_DEFENSE",
                    ReinforcementReason.StrategicImportance,
                    ReinforcementPriority.Normal,
                    0.25f,
                    0.50f,
                    "INTENT_REINFORCE_DEFENSE");

                plan.Activate();

                return plan;
            }

            /*
             * No meaningful reinforcement requirement.
             */
            return plan;
        }

        private static void AddRequest(
            ReinforcementPlan plan,
            string forceGroupId,
            ReinforcementReason reason,
            ReinforcementPriority priority,
            float requestedStrength,
            float urgency,
            string intentId)
        {
            ReinforcementRequest request =
                new ReinforcementRequest(
                    "REINFORCEMENT_" +
                    forceGroupId,
                    forceGroupId);

            request.Configure(
                reason,
                priority,
                requestedStrength,
                urgency,
                intentId);

            plan.AddRequest(request);
        }
    }
}
