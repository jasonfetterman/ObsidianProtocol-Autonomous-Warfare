using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.AI.Commanders
{
    public enum SiegePhase
    {
        None,
        Reconnaissance,
        Isolation,
        Encirclement,
        Suppression,
        Breach,
        Assault,
        Consolidation,
        Aborted
    }

    public enum SiegePriority
    {
        Objective,
        ForcePreservation,
        Logistics,
        Reconnaissance,
        Assault
    }

    public sealed class SiegeObjective
    {
        public string ObjectiveId { get; }

        public SiegePhase Phase
        {
            get;
            private set;
        }

        public SiegePriority Priority
        {
            get;
            private set;
        }

        public float FortificationStrength
        {
            get;
            private set;
        }

        public float EnemyStrength
        {
            get;
            private set;
        }

        public float FriendlyStrength
        {
            get;
            private set;
        }

        public float IsolationLevel
        {
            get;
            private set;
        }

        public float ReconnaissanceLevel
        {
            get;
            private set;
        }

        public float BreachReadiness
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
                ObjectiveId) &&
            Phase != SiegePhase.None &&
            !string.IsNullOrWhiteSpace(
                IntentId);

        public SiegeObjective(
            string objectiveId)
        {
            ObjectiveId =
                objectiveId ?? string.Empty;

            Phase =
                SiegePhase.None;

            Priority =
                SiegePriority.Objective;

            FortificationStrength = 0.0f;
            EnemyStrength = 0.0f;
            FriendlyStrength = 0.0f;
            IsolationLevel = 0.0f;
            ReconnaissanceLevel = 0.0f;
            BreachReadiness = 0.0f;

            IntentId = string.Empty;
            Active = false;
        }

        public void SetBattlefieldState(
            float fortificationStrength,
            float enemyStrength,
            float friendlyStrength,
            float isolationLevel,
            float reconnaissanceLevel,
            float breachReadiness)
        {
            FortificationStrength =
                Clamp01(fortificationStrength);

            EnemyStrength =
                Clamp01(enemyStrength);

            FriendlyStrength =
                Clamp01(friendlyStrength);

            IsolationLevel =
                Clamp01(isolationLevel);

            ReconnaissanceLevel =
                Clamp01(reconnaissanceLevel);

            BreachReadiness =
                Clamp01(breachReadiness);
        }

        public void SetPhase(
            SiegePhase phase,
            SiegePriority priority,
            string intentId)
        {
            Phase = phase;
            Priority = priority;

            IntentId =
                intentId ?? string.Empty;

            Active =
                Phase != SiegePhase.None;
        }

        public void Abort()
        {
            Phase =
                SiegePhase.Aborted;

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

    public sealed class SiegePlan
    {
        private readonly List<
            SiegeObjective> objectives =
            new List<
                SiegeObjective>();

        public bool Active
        {
            get;
            private set;
        }

        public bool Valid =>
            Active &&
            objectives.Count > 0;

        public bool AddObjective(
            SiegeObjective objective)
        {
            if (objective == null ||
                !objective.Valid)
            {
                return false;
            }

            if (objectives.Count >= 8)
                return false;

            objectives.Add(
                objective);

            return true;
        }

        public void Activate()
        {
            if (objectives.Count > 0)
                Active = true;
        }

        public void Deactivate()
        {
            Active = false;
        }

        public void Clear()
        {
            objectives.Clear();
            Active = false;
        }

        public IReadOnlyCollection<
            SiegeObjective>
            GetObjectives()
        {
            return objectives;
        }
    }

    public sealed class SiegePlanner
    {
        public SiegePlan CreatePlan(
            BattlefieldEvaluation battlefield,
            ForceEvaluation forces,
            StrategicObjective strategicObjective)
        {
            if (battlefield == null ||
                !battlefield.Valid ||
                forces == null ||
                !forces.Valid ||
                strategicObjective == null ||
                !strategicObjective.Valid)
            {
                return null;
            }

            /*
             * Siege operations require a strategic attack
             * objective. The planner does not activate siege
             * behavior for ordinary movement or defense.
             */
            if (strategicObjective.Type !=
                AICommanderObjective.Attack)
            {
                return null;
            }

            /*
             * A siege requires enough force to sustain
             * prolonged operations. A commander should not
             * automatically commit a weak army to a siege.
             */
            if (forces.ForceAdvantage < 0.10f)
            {
                return null;
            }

            SiegePlan plan =
                new SiegePlan();

            /*
             * Reconnaissance is the first phase.
             */
            AddPhase(
                plan,
                "SIEGE_RECON",
                SiegePhase.Reconnaissance,
                SiegePriority.Reconnaissance,
                "INTENT_SIEGE_RECON");

            /*
             * Once sufficient intelligence exists,
             * the commander seeks to isolate the objective.
             */
            AddPhase(
                plan,
                "SIEGE_ISOLATE",
                SiegePhase.Isolation,
                SiegePriority.Logistics,
                "INTENT_SIEGE_ISOLATE");

            /*
             * Stronger forces can establish a full
             * encirclement intent.
             */
            if (forces.ForceAdvantage >= 0.25f)
            {
                AddPhase(
                    plan,
                    "SIEGE_ENCIRCLE",
                    SiegePhase.Encirclement,
                    SiegePriority.Objective,
                    "INTENT_SIEGE_ENCIRCLE");
            }

            /*
             * Suppression prepares the objective for
             * breach operations.
             */
            AddPhase(
                plan,
                "SIEGE_SUPPRESS",
                SiegePhase.Suppression,
                SiegePriority.Assault,
                "INTENT_SIEGE_SUPPRESS");

            /*
             * Breach is only planned when the force
             * advantage is sufficient.
             */
            if (forces.ForceAdvantage >= 0.20f)
            {
                AddPhase(
                    plan,
                    "SIEGE_BREACH",
                    SiegePhase.Breach,
                    SiegePriority.Assault,
                    "INTENT_SIEGE_BREACH");
            }

            /*
             * Final assault follows successful preparation.
             */
            if (forces.ForceAdvantage >= 0.30f)
            {
                AddPhase(
                    plan,
                    "SIEGE_ASSAULT",
                    SiegePhase.Assault,
                    SiegePriority.Assault,
                    "INTENT_SIEGE_ASSAULT");

                AddPhase(
                    plan,
                    "SIEGE_CONSOLIDATE",
                    SiegePhase.Consolidation,
                    SiegePriority.Objective,
                    "INTENT_SIEGE_CONSOLIDATE");
            }

            plan.Activate();

            return plan;
        }

        private static void AddPhase(
            SiegePlan plan,
            string objectiveId,
            SiegePhase phase,
            SiegePriority priority,
            string intentId)
        {
            SiegeObjective objective =
                new SiegeObjective(
                    objectiveId);

            objective.SetBattlefieldState(
                0.0f,
                0.0f,
                0.0f,
                0.0f,
                0.0f,
                0.0f);

            objective.SetPhase(
                phase,
                priority,
                intentId);

            plan.AddObjective(
                objective);
        }
    }
}
