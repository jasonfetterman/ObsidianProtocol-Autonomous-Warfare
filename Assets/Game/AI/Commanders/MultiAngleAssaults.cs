using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.AI.Commanders
{
    public enum AssaultAxisRole
    {
        MainAttack,
        FlankingAttack,
        SupportingAttack,
        Suppression,
        Reserve,
        Breach
    }

    public sealed class AssaultAxis
    {
        public string AxisId { get; }

        public AssaultAxisRole Role
        {
            get;
            private set;
        }

        public float StrengthAllocation
        {
            get;
            private set;
        }

        public float Priority
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
            !string.IsNullOrWhiteSpace(AxisId) &&
            StrengthAllocation >= 0.0f &&
            Priority >= 0.0f &&
            !string.IsNullOrWhiteSpace(IntentId);

        public AssaultAxis(
            string axisId)
        {
            AxisId =
                axisId ?? string.Empty;

            Role =
                AssaultAxisRole.MainAttack;

            StrengthAllocation = 0.0f;
            Priority = 0.0f;
            IntentId = string.Empty;
            Active = false;
        }

        public void Configure(
            AssaultAxisRole role,
            float strengthAllocation,
            float priority,
            string intentId)
        {
            Role = role;

            StrengthAllocation =
                Clamp01(
                    strengthAllocation);

            Priority =
                Math.Max(
                    0.0f,
                    priority);

            IntentId =
                intentId ?? string.Empty;

            Active =
                Valid;
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

    public sealed class MultiAngleAssaultPlan
    {
        private readonly List<
            AssaultAxis> axes =
            new List<
                AssaultAxis>();

        public bool Active
        {
            get;
            private set;
        }

        public int AxisCount =>
            axes.Count;

        public bool Valid =>
            Active &&
            axes.Count > 0;

        public float TotalStrengthAllocation
        {
            get
            {
                float total = 0.0f;

                foreach (AssaultAxis axis
                    in axes)
                {
                    total +=
                        axis.StrengthAllocation;
                }

                return total;
            }
        }

        public void Clear()
        {
            axes.Clear();
            Active = false;
        }

        public bool AddAxis(
            AssaultAxis axis)
        {
            if (axis == null ||
                !axis.Valid)
            {
                return false;
            }

            if (axes.Count >= 5)
                return false;

            axes.Add(axis);

            return true;
        }

        public void Activate()
        {
            if (axes.Count > 0)
                Active = true;
        }

        public void Deactivate()
        {
            Active = false;
        }

        public IReadOnlyCollection<
            AssaultAxis>
            GetAxes()
        {
            return axes;
        }
    }

    public sealed class MultiAngleAssaultPlanner
    {
        public MultiAngleAssaultPlan CreatePlan(
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

            if (objective.Type !=
                AICommanderObjective.Attack)
            {
                return null;
            }

            if (forces.ForceAdvantage < 0.10f)
            {
                return null;
            }

            MultiAngleAssaultPlan plan =
                new MultiAngleAssaultPlan();

            float availableStrength =
                Clamp01(
                    0.50f +
                    forces.ForceAdvantage);

            /*
             * Strong reconnaissance allows the
             * commander to coordinate more than one
             * assault axis.
             */
            if (battlefield.ReconAdvantage >= 0.20f &&
                forces.ForceAdvantage >= 0.25f)
            {
                AddMainAxis(
                    plan,
                    0.40f);

                AddFlankingAxis(
                    plan,
                    0.25f);

                AddSupportingAxis(
                    plan,
                    0.15f);

                AddReserveAxis(
                    plan,
                    0.10f);

                AddSuppressionAxis(
                    plan,
                    0.10f);
            }
            else if (forces.ForceAdvantage >= 0.15f)
            {
                AddMainAxis(
                    plan,
                    0.55f);

                AddFlankingAxis(
                    plan,
                    0.25f);

                AddReserveAxis(
                    plan,
                    0.20f);
            }
            else
            {
                AddMainAxis(
                    plan,
                    0.70f);

                AddSupportingAxis(
                    plan,
                    0.20f);

                AddReserveAxis(
                    plan,
                    0.10f);
            }

            /*
             * availableStrength represents the amount
             * of force the commander is willing to
             * commit. It prevents the plan from assuming
             * the entire army must attack.
             */
            foreach (AssaultAxis axis
                in plan.GetAxes())
            {
                axis.Configure(
                    GetRole(axis),
                    axis.StrengthAllocation *
                    availableStrength,
                    axis.Priority,
                    axis.IntentId);
            }

            plan.Activate();

            return plan;
        }

        private static AssaultAxisRole GetRole(
            AssaultAxis axis)
        {
            return axis.Role;
        }

        private static void AddMainAxis(
            MultiAngleAssaultPlan plan,
            float allocation)
        {
            AssaultAxis axis =
                new AssaultAxis(
                    "ASSAULT_MAIN");

            axis.Configure(
                AssaultAxisRole.MainAttack,
                allocation,
                1.0f,
                "INTENT_ASSAULT_MAIN");

            plan.AddAxis(axis);
        }

        private static void AddFlankingAxis(
            MultiAngleAssaultPlan plan,
            float allocation)
        {
            AssaultAxis axis =
                new AssaultAxis(
                    "ASSAULT_FLANK");

            axis.Configure(
                AssaultAxisRole.FlankingAttack,
                allocation,
                0.90f,
                "INTENT_ASSAULT_FLANK");

            plan.AddAxis(axis);
        }

        private static void AddSupportingAxis(
            MultiAngleAssaultPlan plan,
            float allocation)
        {
            AssaultAxis axis =
                new AssaultAxis(
                    "ASSAULT_SUPPORT");

            axis.Configure(
                AssaultAxisRole.SupportingAttack,
                allocation,
                0.75f,
                "INTENT_ASSAULT_SUPPORT");

            plan.AddAxis(axis);
        }

        private static void AddSuppressionAxis(
            MultiAngleAssaultPlan plan,
            float allocation)
        {
            AssaultAxis axis =
                new AssaultAxis(
                    "ASSAULT_SUPPRESSION");

            axis.Configure(
                AssaultAxisRole.Suppression,
                allocation,
                0.80f,
                "INTENT_ASSAULT_SUPPRESSION");

            plan.AddAxis(axis);
        }

        private static void AddReserveAxis(
            MultiAngleAssaultPlan plan,
            float allocation)
        {
            AssaultAxis axis =
                new AssaultAxis(
                    "ASSAULT_RESERVE");

            axis.Configure(
                AssaultAxisRole.Reserve,
                allocation,
                0.65f,
                "INTENT_ASSAULT_RESERVE");

            plan.AddAxis(axis);
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
