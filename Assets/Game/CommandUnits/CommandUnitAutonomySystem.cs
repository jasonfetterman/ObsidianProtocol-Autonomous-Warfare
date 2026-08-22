using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.CommandUnits
{
    public enum CommandAutonomyMode
    {
        Manual,
        Assisted,
        Autonomous,
        Emergency
    }

    public enum CommandAutonomyBehavior
    {
        Idle,
        Monitor,
        Coordinate,
        Analyze,
        Redirect,
        Defend,
        Recover
    }

    public sealed class CommandAutonomyState
    {
        public string UnitId { get; }

        public CommandAutonomyMode Mode { get; private set; }
        public CommandAutonomyBehavior Behavior { get; private set; }

        public string CurrentObjective { get; private set; }

        public float DecisionInterval { get; private set; }
        public float DecisionTimer { get; private set; }

        public bool Enabled { get; private set; }

        public CommandAutonomyState(
            string unitId)
        {
            UnitId =
                unitId ?? string.Empty;

            Mode =
                CommandAutonomyMode.Autonomous;

            Behavior =
                CommandAutonomyBehavior.Idle;

            CurrentObjective =
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
            CommandAutonomyMode mode)
        {
            Mode = mode;
        }

        public void SetBehavior(
            CommandAutonomyBehavior behavior)
        {
            Behavior = behavior;
        }

        public void SetObjective(
            string objectiveId)
        {
            CurrentObjective =
                objectiveId ?? string.Empty;
        }

        public bool ShouldEvaluate(
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
                CommandAutonomyBehavior.Idle;
        }
    }

    public sealed class CommandUnitAutonomySystem
    {
        private readonly Dictionary<string, CommandAutonomyState> states =
            new Dictionary<string, CommandAutonomyState>(
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
                    new CommandAutonomyState(unitId));
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
            CommandAutonomyMode mode)
        {
            RegisterUnit(unitId);

            states[unitId].SetMode(mode);
        }

        public void SetBehavior(
            string unitId,
            CommandAutonomyBehavior behavior)
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

        public bool ShouldEvaluate(
            string unitId,
            float deltaTime)
        {
            return states.TryGetValue(
                       unitId,
                       out CommandAutonomyState state) &&
                   state.ShouldEvaluate(
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
                    out CommandAutonomyState state))
            {
                state.Disable();
            }
        }

        public bool TryGetState(
            string unitId,
            out CommandAutonomyState state)
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
