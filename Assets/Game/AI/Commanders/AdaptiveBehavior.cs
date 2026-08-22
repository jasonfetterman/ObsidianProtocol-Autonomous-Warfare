using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.AI.Commanders
{
    public enum AdaptiveResponse
    {
        None,
        Maintain,
        Escalate,
        Deescalate,
        Reinforce,
        Regroup,
        Retreat,
        IncreaseReconnaissance,
        ChangeObjectivePriority,
        ChangeOperationalPosture
    }

    public enum AdaptiveTrigger
    {
        None,
        ThreatIncrease,
        ThreatDecrease,
        ForceLoss,
        ForceRecovery,
        LogisticsDecline,
        LogisticsRecovery,
        IntelligenceGap,
        TerritoryLoss,
        TerritoryGain,
        ObjectiveChange
    }

    public sealed class AdaptiveBehaviorDecision
    {
        public string DecisionId { get; }

        public AdaptiveTrigger Trigger
        {
            get;
            private set;
        }

        public AdaptiveResponse Response
        {
            get;
            private set;
        }

        public float Magnitude
        {
            get;
            private set;
        }

        public float Confidence
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
            Trigger !=
                AdaptiveTrigger.None &&
            Response !=
                AdaptiveResponse.None &&
            !string.IsNullOrWhiteSpace(
                IntentId);

        public AdaptiveBehaviorDecision(
            string decisionId)
        {
            DecisionId =
                decisionId ?? string.Empty;

            Trigger =
                AdaptiveTrigger.None;

            Response =
                AdaptiveResponse.None;

            Magnitude = 0.0f;
            Confidence = 0.0f;

            IntentId = string.Empty;
            Active = false;
        }

        public void Configure(
            AdaptiveTrigger trigger,
            AdaptiveResponse response,
            float magnitude,
            float confidence,
            string intentId)
        {
            Trigger = trigger;
            Response = response;

            Magnitude =
                Clamp01(
                    magnitude);

            Confidence =
                Clamp01(
                    confidence);

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

    public sealed class AdaptiveBehaviorPlan
    {
        private readonly List<
            AdaptiveBehaviorDecision> decisions =
            new List<
                AdaptiveBehaviorDecision>();

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
            AdaptiveBehaviorDecision decision)
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
            AdaptiveBehaviorDecision>
            GetDecisions()
        {
            return decisions;
        }
    }

    public sealed class AdaptiveBehaviorPlanner
    {
        public AdaptiveBehaviorPlan EvaluateChange(
            BattlefieldEvaluation previousBattlefield,
            BattlefieldEvaluation currentBattlefield,
            ForceEvaluation previousForces,
            ForceEvaluation currentForces,
            StrategicObjective objective)
        {
            if (previousBattlefield == null ||
                !previousBattlefield.Valid ||
                currentBattlefield == null ||
                !currentBattlefield.Valid ||
                previousForces == null ||
                !previousForces.Valid ||
                currentForces == null ||
                !currentForces.Valid ||
                objective == null ||
                !objective.Valid)
            {
                return null;
            }

            AdaptiveBehaviorPlan plan =
                new AdaptiveBehaviorPlan();

            /*
             * Threat increase.
             */
            if (ThreatIncreased(
                    previousBattlefield,
                    currentBattlefield))
            {
                AddDecision(
                    plan,
                    "ADAPT_THREAT_INCREASE",
                    AdaptiveTrigger.ThreatIncrease,
                    AdaptiveResponse.ChangeOperationalPosture,
                    0.80f,
                    0.90f,
                    "INTENT_ADAPT_THREAT_INCREASE");
            }

            /*
             * Threat reduction.
             */
            if (ThreatDecreased(
                    previousBattlefield,
                    currentBattlefield))
            {
                AddDecision(
                    plan,
                    "ADAPT_THREAT_DECREASE",
                    AdaptiveTrigger.ThreatDecrease,
                    AdaptiveResponse.Escalate,
                    0.45f,
                    0.70f,
                    "INTENT_ADAPT_THREAT_DECREASE");
            }

            /*
             * Significant force loss.
             */
            if (currentForces.ForceAdvantage <
                previousForces.ForceAdvantage -
                0.20f)
            {
                AddDecision(
                    plan,
                    "ADAPT_FORCE_LOSS",
                    AdaptiveTrigger.ForceLoss,
                    AdaptiveResponse.Regroup,
                    0.75f,
                    0.90f,
                    "INTENT_ADAPT_FORCE_LOSS");
            }

            /*
             * Force recovery allows the commander
             * to become more aggressive again.
             */
            if (currentForces.ForceAdvantage >
                previousForces.ForceAdvantage +
                0.20f)
            {
                AddDecision(
                    plan,
                    "ADAPT_FORCE_RECOVERY",
                    AdaptiveTrigger.ForceRecovery,
                    AdaptiveResponse.Escalate,
                    0.60f,
                    0.75f,
                    "INTENT_ADAPT_FORCE_RECOVERY");
            }

            /*
             * Logistics decline.
             */
            if (currentBattlefield.LogisticsAdvantage <
                previousBattlefield.LogisticsAdvantage -
                0.20f)
            {
                AddDecision(
                    plan,
                    "ADAPT_LOGISTICS_DECLINE",
                    AdaptiveTrigger.LogisticsDecline,
                    AdaptiveResponse.Reinforce,
                    0.70f,
                    0.85f,
                    "INTENT_ADAPT_LOGISTICS_DECLINE");
            }

            /*
             * Logistics recovery.
             */
            if (currentBattlefield.LogisticsAdvantage >
                previousBattlefield.LogisticsAdvantage +
                0.20f)
            {
                AddDecision(
                    plan,
                    "ADAPT_LOGISTICS_RECOVERY",
                    AdaptiveTrigger.LogisticsRecovery,
                    AdaptiveResponse.Maintain,
                    0.40f,
                    0.70f,
                    "INTENT_ADAPT_LOGISTICS_RECOVERY");
            }

            /*
             * Intelligence gap.
             */
            if (currentBattlefield.ReconAdvantage <
                previousBattlefield.ReconAdvantage -
                0.20f)
            {
                AddDecision(
                    plan,
                    "ADAPT_INTELLIGENCE_GAP",
                    AdaptiveTrigger.IntelligenceGap,
                    AdaptiveResponse.IncreaseReconnaissance,
                    0.75f,
                    0.85f,
                    "INTENT_ADAPT_RECON");
            }

            /*
             * Territory loss.
             */
            if (currentBattlefield.TerritoryAdvantage <
                previousBattlefield.TerritoryAdvantage -
                0.20f)
            {
                AddDecision(
                    plan,
                    "ADAPT_TERRITORY_LOSS",
                    AdaptiveTrigger.TerritoryLoss,
                    AdaptiveResponse.ChangeOperationalPosture,
                    0.70f,
                    0.85f,
                    "INTENT_ADAPT_TERRITORY_LOSS");
            }

            /*
             * Territory gain.
             */
            if (currentBattlefield.TerritoryAdvantage >
                previousBattlefield.TerritoryAdvantage +
                0.20f)
            {
                AddDecision(
                    plan,
                    "ADAPT_TERRITORY_GAIN",
                    AdaptiveTrigger.TerritoryGain,
                    AdaptiveResponse.Escalate,
                    0.50f,
                    0.75f,
                    "INTENT_ADAPT_TERRITORY_GAIN");
            }

            /*
             * If nothing meaningful changed, maintain
             * the current operational behavior.
             */
            if (plan.DecisionCount == 0)
            {
                AddDecision(
                    plan,
                    "ADAPT_MAINTAIN",
                    AdaptiveTrigger.None,
                    AdaptiveResponse.Maintain,
                    0.20f,
                    0.60f,
                    "INTENT_ADAPT_MAINTAIN");
            }

            plan.Activate();

            return plan;
        }

        private static bool ThreatIncreased(
            BattlefieldEvaluation previous,
            BattlefieldEvaluation current)
        {
            return ThreatValue(
                       current.ThreatLevel) >
                   ThreatValue(
                       previous.ThreatLevel) +
                   0.20f;
        }

        private static bool ThreatDecreased(
            BattlefieldEvaluation previous,
            BattlefieldEvaluation current)
        {
            return ThreatValue(
                       current.ThreatLevel) <
                   ThreatValue(
                       previous.ThreatLevel) -
                   0.20f;
        }

        private static float ThreatValue(
            BattlefieldThreatLevel level)
        {
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

        private static void AddDecision(
            AdaptiveBehaviorPlan plan,
            string decisionId,
            AdaptiveTrigger trigger,
            AdaptiveResponse response,
            float magnitude,
            float confidence,
            string intentId)
        {
            /*
             * Maintain is the only response intentionally
             * allowed when there is no trigger.
             */
            if (trigger ==
                    AdaptiveTrigger.None &&
                response !=
                    AdaptiveResponse.Maintain)
            {
                return;
            }

            AdaptiveBehaviorDecision decision =
                new AdaptiveBehaviorDecision(
                    decisionId);

            decision.Configure(
                trigger,
                response,
                magnitude,
                confidence,
                intentId);

            plan.AddDecision(
                decision);
        }
    }
}
