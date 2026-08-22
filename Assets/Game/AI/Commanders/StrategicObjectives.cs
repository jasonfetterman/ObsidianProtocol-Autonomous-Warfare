using System;

namespace ObsidianProtocol.Game.AI.Commanders
{
    public sealed class StrategicObjective
    {
        public string ObjectiveId { get; }

        public AICommanderObjective Type
        {
            get;
            private set;
        }

        public AICommanderPriority Priority
        {
            get;
            private set;
        }

        public float PriorityScore
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
                ObjectiveId) &&
            Type != AICommanderObjective.None &&
            PriorityScore >= 0.0f;

        public StrategicObjective(
            string objectiveId)
        {
            ObjectiveId =
                objectiveId ?? string.Empty;

            Type =
                AICommanderObjective.None;

            Priority =
                AICommanderPriority.StrategicObjective;

            PriorityScore = 0.0f;
            Active = false;
        }

        public void Set(
            AICommanderObjective type,
            AICommanderPriority priority,
            float score)
        {
            Type = type;
            Priority = priority;
            PriorityScore =
                Math.Max(
                    0.0f,
                    score);

            Active =
                Type !=
                AICommanderObjective.None;
        }

        public void Clear()
        {
            Type =
                AICommanderObjective.None;

            Priority =
                AICommanderPriority.StrategicObjective;

            PriorityScore = 0.0f;
            Active = false;
        }
    }

    public sealed class StrategicObjectiveSelector
    {
        public StrategicObjective Select(
            BattlefieldEvaluation evaluation)
        {
            if (evaluation == null ||
                !evaluation.Valid)
            {
                return null;
            }

            StrategicObjective objective =
                new StrategicObjective(
                    "AI_STRATEGIC_OBJECTIVE");

            /*
             * Survival takes precedence when the
             * battlefield becomes critical.
             */
            if (evaluation.ThreatLevel ==
                BattlefieldThreatLevel.Critical)
            {
                objective.Set(
                    AICommanderObjective.Withdraw,
                    AICommanderPriority.Survival,
                    1.0f);

                return objective;
            }

            /*
             * Severe force disadvantage favors
             * regrouping instead of blind attack.
             */
            if (evaluation.ForceAdvantage <
                -0.35f)
            {
                objective.Set(
                    AICommanderObjective.Regroup,
                    AICommanderPriority.ForcePreservation,
                    0.90f);

                return objective;
            }

            /*
             * Poor logistics can force the commander
             * to stabilize the army before advancing.
             */
            if (evaluation.LogisticsAdvantage <
                -0.40f)
            {
                objective.Set(
                    AICommanderObjective.Reinforce,
                    AICommanderPriority.Logistics,
                    0.85f);

                return objective;
            }

            /*
             * Poor reconnaissance can create a
             * reconnaissance objective before committing
             * the main force.
             */
            if (evaluation.ReconAdvantage <
                -0.40f)
            {
                objective.Set(
                    AICommanderObjective.Recon,
                    AICommanderPriority.Reconnaissance,
                    0.80f);

                return objective;
            }

            /*
             * Strong force advantage combined with
             * battlefield control creates an attack
             * opportunity.
             */
            if (evaluation.ForceAdvantage >
                    0.25f &&
                evaluation.TerritoryAdvantage >=
                    0.0f)
            {
                objective.Set(
                    AICommanderObjective.Attack,
                    AICommanderPriority.StrategicObjective,
                    0.75f);

                return objective;
            }

            /*
             * Weak territorial position with sufficient
             * force encourages advancement.
             */
            if (evaluation.TerritoryAdvantage <
                    -0.20f &&
                evaluation.ForceAdvantage >
                    0.10f)
            {
                objective.Set(
                    AICommanderObjective.Advance,
                    AICommanderPriority.StrategicObjective,
                    0.70f);

                return objective;
            }

            /*
             * Otherwise hold and continue evaluating.
             */
            objective.Set(
                AICommanderObjective.Hold,
                AICommanderPriority.StrategicObjective,
                0.50f);

            return objective;
        }
    }
}
