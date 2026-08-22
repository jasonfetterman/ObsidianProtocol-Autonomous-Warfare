using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.AI.Commanders
{
    public enum DefensivePosture
    {
        None,
        HoldLine,
        ElasticDefense,
        Fallback,
        Concentrate,
        ProtectLogistics,
        ProtectObjective,
        Screen,
        CounterAttack
    }

    public enum DefensivePriority
    {
        Survival,
        Objective,
        Logistics,
        ForcePreservation,
        CounterAttack
    }

    public sealed class DefensivePosition
    {
        public string PositionId { get; }

        public DefensivePosture Posture
        {
            get;
            private set;
        }

        public DefensivePriority Priority
        {
            get;
            private set;
        }

        public float StrengthAllocation
        {
            get;
            private set;
        }

        public float ThreatPressure
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
            !string.IsNullOrWhiteSpace(PositionId) &&
            Posture != DefensivePosture.None &&
            !string.IsNullOrWhiteSpace(IntentId);

        public DefensivePosition(
            string positionId)
        {
            PositionId =
                positionId ?? string.Empty;

            Posture =
                DefensivePosture.None;

            Priority =
                DefensivePriority.Objective;

            StrengthAllocation = 0.0f;
            ThreatPressure = 0.0f;
            IntentId = string.Empty;
            Active = false;
        }

        public void Configure(
            DefensivePosture posture,
            DefensivePriority priority,
            float strengthAllocation,
            float threatPressure,
            string intentId)
        {
            Posture = posture;
            Priority = priority;

            StrengthAllocation =
                Clamp01(
                    strengthAllocation);

            ThreatPressure =
                Clamp01(
                    threatPressure);

            IntentId =
                intentId ?? string.Empty;

            Active = Valid;
        }

        public void Deactivate()
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

    public sealed class DefensiveRepositionPlan
    {
        private readonly List<
            DefensivePosition> positions =
            new List<
                DefensivePosition>();

        public bool Active
        {
            get;
            private set;
        }

        public bool Valid =>
            Active &&
            positions.Count > 0;

        public int PositionCount =>
            positions.Count;

        public bool AddPosition(
            DefensivePosition position)
        {
            if (position == null ||
                !position.Valid)
            {
                return false;
            }

            if (positions.Count >= 6)
                return false;

            positions.Add(position);

            return true;
        }

        public void Activate()
        {
            if (positions.Count > 0)
                Active = true;
        }

        public void Deactivate()
        {
            Active = false;
        }

        public void Clear()
        {
            positions.Clear();
            Active = false;
        }

        public IReadOnlyCollection<
            DefensivePosition>
            GetPositions()
        {
            return positions;
        }
    }

    public sealed class DefensiveRepositionPlanner
    {
        public DefensiveRepositionPlan CreatePlan(
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

            DefensiveRepositionPlan plan =
                new DefensiveRepositionPlan();

            /*
             * Critical threats require immediate
             * defensive restructuring.
             */
            if (battlefield.ThreatLevel ==
                BattlefieldThreatLevel.Critical)
            {
                AddFallback(
                    plan,
                    0.60f,
                    1.0f);

                AddLogisticsProtection(
                    plan,
                    0.20f,
                    0.90f);

                AddObjectiveProtection(
                    plan,
                    0.20f,
                    0.85f);

                plan.Activate();

                return plan;
            }

            /*
             * Major force disadvantage favors an
             * elastic defense rather than a static line.
             */
            if (forces.ForceAdvantage <
                -0.25f)
            {
                AddElasticDefense(
                    plan,
                    0.55f,
                    0.85f);

                AddScreen(
                    plan,
                    0.20f,
                    0.75f);

                AddLogisticsProtection(
                    plan,
                    0.15f,
                    0.80f);

                AddObjectiveProtection(
                    plan,
                    0.10f,
                    0.70f);

                plan.Activate();

                return plan;
            }

            /*
             * Weak territorial control with reasonable
             * force strength favors concentration.
             */
            if (battlefield.TerritoryAdvantage <
                    -0.25f &&
                forces.ForceAdvantage >=
                    0.0f)
            {
                AddConcentration(
                    plan,
                    0.55f,
                    0.80f);

                AddHoldLine(
                    plan,
                    0.25f,
                    0.70f);

                AddCounterAttackReserve(
                    plan,
                    0.20f,
                    0.60f);

                plan.Activate();

                return plan;
            }

            /*
             * Normal defensive posture.
             */
            AddHoldLine(
                plan,
                0.65f,
                0.60f);

            AddObjectiveProtection(
                plan,
                0.20f,
                0.65f);

            AddCounterAttackReserve(
                plan,
                0.15f,
                0.50f);

            plan.Activate();

            return plan;
        }

        private static void AddHoldLine(
            DefensiveRepositionPlan plan,
            float allocation,
            float pressure)
        {
            DefensivePosition position =
                new DefensivePosition(
                    "DEFENSE_HOLD_LINE");

            position.Configure(
                DefensivePosture.HoldLine,
                DefensivePriority.Objective,
                allocation,
                pressure,
                "INTENT_DEFEND_HOLD_LINE");

            plan.AddPosition(position);
        }

        private static void AddElasticDefense(
            DefensiveRepositionPlan plan,
            float allocation,
            float pressure)
        {
            DefensivePosition position =
                new DefensivePosition(
                    "DEFENSE_ELASTIC");

            position.Configure(
                DefensivePosture.ElasticDefense,
                DefensivePriority.ForcePreservation,
                allocation,
                pressure,
                "INTENT_DEFEND_ELASTIC");

            plan.AddPosition(position);
        }

        private static void AddFallback(
            DefensiveRepositionPlan plan,
            float allocation,
            float pressure)
        {
            DefensivePosition position =
                new DefensivePosition(
                    "DEFENSE_FALLBACK");

            position.Configure(
                DefensivePosture.Fallback,
                DefensivePriority.Survival,
                allocation,
                pressure,
                "INTENT_DEFEND_FALLBACK");

            plan.AddPosition(position);
        }

        private static void AddConcentration(
            DefensiveRepositionPlan plan,
            float allocation,
            float pressure)
        {
            DefensivePosition position =
                new DefensivePosition(
                    "DEFENSE_CONCENTRATE");

            position.Configure(
                DefensivePosture.Concentrate,
                DefensivePriority.ForcePreservation,
                allocation,
                pressure,
                "INTENT_DEFEND_CONCENTRATE");

            plan.AddPosition(position);
        }

        private static void AddLogisticsProtection(
            DefensiveRepositionPlan plan,
            float allocation,
            float pressure)
        {
            DefensivePosition position =
                new DefensivePosition(
                    "DEFENSE_LOGISTICS");

            position.Configure(
                DefensivePosture.ProtectLogistics,
                DefensivePriority.Logistics,
                allocation,
                pressure,
                "INTENT_DEFEND_LOGISTICS");

            plan.AddPosition(position);
        }

        private static void AddObjectiveProtection(
            DefensiveRepositionPlan plan,
            float allocation,
            float pressure)
        {
            DefensivePosition position =
                new DefensivePosition(
                    "DEFENSE_OBJECTIVE");

            position.Configure(
                DefensivePosture.ProtectObjective,
                DefensivePriority.Objective,
                allocation,
                pressure,
                "INTENT_DEFEND_OBJECTIVE");

            plan.AddPosition(position);
        }

        private static void AddScreen(
            DefensiveRepositionPlan plan,
            float allocation,
            float pressure)
        {
            DefensivePosition position =
                new DefensivePosition(
                    "DEFENSE_SCREEN");

            position.Configure(
                DefensivePosture.Screen,
                DefensivePriority.Survival,
                allocation,
                pressure,
                "INTENT_DEFEND_SCREEN");

            plan.AddPosition(position);
        }

        private static void AddCounterAttackReserve(
            DefensiveRepositionPlan plan,
            float allocation,
            float pressure)
        {
            DefensivePosition position =
                new DefensivePosition(
                    "DEFENSE_COUNTER_RESERVE");

            position.Configure(
                DefensivePosture.CounterAttack,
                DefensivePriority.CounterAttack,
                allocation,
                pressure,
                "INTENT_DEFEND_COUNTER_RESERVE");

            plan.AddPosition(position);
        }
    }
}
