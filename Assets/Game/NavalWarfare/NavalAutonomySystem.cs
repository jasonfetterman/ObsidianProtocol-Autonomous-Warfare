using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.NavalWarfare
{
    public enum NavalAutonomyMode
    {
        Manual,
        Assisted,
        Autonomous,
        Defensive,
        Emergency
    }

    public enum NavalAutonomyBehavior
    {
        Idle,
        Navigate,
        Patrol,
        Recon,
        Escort,
        Support,
        Engage,
        Pursue,
        Retreat,
        Recover
    }

    public sealed class NavalAutonomyState
    {
        public string UnitId { get; }

        public NavalAutonomyMode Mode { get; private set; }
        public NavalAutonomyBehavior Behavior { get; private set; }

        public string AssignedObjective { get; private set; }
        public string CurrentTarget { get; private set; }

        public float DecisionInterval { get; private set; }
        public float DecisionTimer { get; private set; }

        public bool Enabled { get; private set; }

        public NavalAutonomyState(
            string unitId)
        {
            UnitId =
                unitId ?? string.Empty;

            Mode =
                NavalAutonomyMode.Autonomous;

            Behavior =
                NavalAutonomyBehavior.Idle;

            AssignedObjective =
                string.Empty;

            CurrentTarget =
                string.Empty;

            DecisionInterval =
                0.25f;

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
            NavalAutonomyMode mode)
        {
            Mode = mode;
        }

        public void SetBehavior(
            NavalAutonomyBehavior behavior)
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

        public void Enable()
        {
            Enabled = true;
        }

        public void Disable()
        {
            Enabled = false;

            Behavior =
                NavalAutonomyBehavior.Idle;
        }
    }

    public sealed class NavalAutonomySystem
    {
        private readonly Dictionary<string, NavalAutonomyState> states =
            new Dictionary<string, NavalAutonomyState>(
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
                    new NavalAutonomyState(unitId));
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
            NavalAutonomyMode mode)
        {
            RegisterUnit(unitId);

            states[unitId].SetMode(mode);
        }

        public void SetBehavior(
            string unitId,
            NavalAutonomyBehavior behavior)
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
                       out NavalAutonomyState state) &&
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
                    out NavalAutonomyState state))
            {
                state.Disable();
            }
        }

        public bool TryGetState(
            string unitId,
            out NavalAutonomyState state)
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
