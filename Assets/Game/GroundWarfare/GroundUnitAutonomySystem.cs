using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.GroundWarfare
{
    public enum GroundAutonomyMode
    {
        Manual,
        Assisted,
        Autonomous,
        Defensive,
        Emergency
    }

    public enum GroundAutonomyBehavior
    {
        Idle,
        Navigate,
        Recon,
        Secure,
        Support,
        Engage,
        Pursue,
        Reinforce,
        Retreat,
        Recover
    }

    public sealed class GroundUnitAutonomyState
    {
        public string UnitId { get; }

        public GroundAutonomyMode Mode { get; private set; }
        public GroundAutonomyBehavior Behavior { get; private set; }

        public string AssignedObjective { get; private set; }
        public string CurrentTarget { get; private set; }

        public float DecisionInterval { get; private set; }
        public float DecisionTimer { get; private set; }

        public bool Enabled { get; private set; }

        public GroundUnitAutonomyState(
            string unitId)
        {
            UnitId =
                unitId ?? string.Empty;

            Mode =
                GroundAutonomyMode.Autonomous;

            Behavior =
                GroundAutonomyBehavior.Idle;

            AssignedObjective =
                string.Empty;

            CurrentTarget =
                string.Empty;

            DecisionInterval =
                0.25f;

            DecisionTimer =
                0f;

            Enabled = true;
        }

        public void Configure(
            float decisionInterval)
        {
            DecisionInterval =
                Math.Max(
                    0.01f,
                    decisionInterval);
        }

        public void SetMode(
            GroundAutonomyMode mode)
        {
            Mode = mode;
        }

        public void SetBehavior(
            GroundAutonomyBehavior behavior)
        {
            Behavior = behavior;
        }

        public void SetObjective(
            string objectiveId)
        {
            AssignedObjective =
                objectiveId ?? string.Empty;
        }

        public void SetTarget(
            string targetId)
        {
            CurrentTarget =
                targetId ?? string.Empty;
        }

        public void ClearTarget()
        {
            CurrentTarget =
                string.Empty;
        }

        public void Enable()
        {
            Enabled = true;
        }

        public void Disable()
        {
            Enabled = false;

            Behavior =
                GroundAutonomyBehavior.Idle;
        }

        public bool UpdateDecisionTimer(
            float deltaTime)
        {
            if (!Enabled)
            {
                return false;
            }

            DecisionTimer +=
                Math.Max(
                    0f,
                    deltaTime);

            if (DecisionTimer <
                DecisionInterval)
            {
                return false;
            }

            DecisionTimer = 0f;

            return true;
        }
    }

    public sealed class GroundUnitAutonomySystem
    {
        private readonly Dictionary<string, GroundUnitAutonomyState> states =
            new Dictionary<string, GroundUnitAutonomyState>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterUnit(
            string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!states.ContainsKey(unitId))
            {
                states.Add(
                    unitId,
                    new GroundUnitAutonomyState(unitId));
            }
        }

        public void ConfigureUnit(
            string unitId,
            float decisionInterval)
        {
            RegisterUnit(unitId);

            states[unitId].Configure(
                decisionInterval);
        }

        public void SetMode(
            string unitId,
            GroundAutonomyMode mode)
        {
            RegisterUnit(unitId);

            states[unitId].SetMode(mode);
        }

        public void SetBehavior(
            string unitId,
            GroundAutonomyBehavior behavior)
        {
            RegisterUnit(unitId);

            states[unitId].SetBehavior(behavior);
        }

        public void SetObjective(
            string unitId,
            string objectiveId)
        {
            RegisterUnit(unitId);

            states[unitId].SetObjective(
                objectiveId);
        }

        public void SetTarget(
            string unitId,
            string targetId)
        {
            RegisterUnit(unitId);

            states[unitId].SetTarget(
                targetId);
        }

        public bool ShouldEvaluate(
            string unitId,
            float deltaTime)
        {
            return states.TryGetValue(
                       unitId,
                       out GroundUnitAutonomyState state) &&
                   state.UpdateDecisionTimer(
                       deltaTime);
        }

        public void EnableUnit(
            string unitId)
        {
            RegisterUnit(unitId);

            states[unitId].Enable();
        }

        public void DisableUnit(
            string unitId)
        {
            if (states.TryGetValue(
                    unitId,
                    out GroundUnitAutonomyState state))
            {
                state.Disable();
            }
        }

        public bool TryGetState(
            string unitId,
            out GroundUnitAutonomyState state)
        {
            return states.TryGetValue(
                unitId,
                out state);
        }

        public void RemoveUnit(
            string unitId)
        {
            states.Remove(unitId);
        }

        public void Clear()
        {
            states.Clear();
        }
    }
}
