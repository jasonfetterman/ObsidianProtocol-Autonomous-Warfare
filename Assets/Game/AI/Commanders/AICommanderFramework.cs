using System;

namespace ObsidianProtocol.Game.AI.Commanders
{
    public enum AICommanderState
    {
        Inactive,
        Initializing,
        Evaluating,
        Planning,
        Executing,
        Reassessing,
        Disabled
    }

    public enum AICommanderPriority
    {
        Survival,
        StrategicObjective,
        ForcePreservation,
        TacticalAdvantage,
        Logistics,
        Reconnaissance
    }

    public enum AICommanderObjective
    {
        None,
        Attack,
        Defend,
        Advance,
        Hold,
        Capture,
        Reinforce,
        Recon,
        Withdraw,
        Regroup,
        Siege
    }

    public sealed class AICommanderDecision
    {
        public AICommanderObjective Objective
        {
            get;
            private set;
        }

        public AICommanderPriority Priority
        {
            get;
            private set;
        }

        public string IntentId
        {
            get;
            private set;
        }

        public bool Valid =>
            Objective !=
                AICommanderObjective.None &&
            !string.IsNullOrWhiteSpace(
                IntentId);

        public AICommanderDecision()
        {
            Objective =
                AICommanderObjective.None;

            Priority =
                AICommanderPriority.StrategicObjective;

            IntentId =
                string.Empty;
        }

        public void Set(
            AICommanderObjective objective,
            AICommanderPriority priority,
            string intentId)
        {
            Objective = objective;
            Priority = priority;
            IntentId =
                intentId ?? string.Empty;
        }

        public void Clear()
        {
            Objective =
                AICommanderObjective.None;

            Priority =
                AICommanderPriority.StrategicObjective;

            IntentId =
                string.Empty;
        }
    }

    public sealed class AICommander
    {
        public string CommanderId
        {
            get;
        }

        public string FactionId
        {
            get;
        }

        public AICommanderState State
        {
            get;
            private set;
        }

        public AICommanderObjective CurrentObjective
        {
            get;
            private set;
        }

        public AICommanderPriority CurrentPriority
        {
            get;
            private set;
        }

        public float EvaluationInterval
        {
            get;
            private set;
        }

        public float TimeSinceEvaluation
        {
            get;
            private set;
        }

        public AICommanderDecision CurrentDecision
        {
            get;
        }

        public bool Active
        {
            get;
            private set;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(
                CommanderId) &&
            !string.IsNullOrWhiteSpace(
                FactionId);

        public AICommander(
            string commanderId,
            string factionId,
            float evaluationInterval = 2.0f)
        {
            CommanderId =
                commanderId ?? string.Empty;

            FactionId =
                factionId ?? string.Empty;

            State =
                AICommanderState.Inactive;

            CurrentObjective =
                AICommanderObjective.None;

            CurrentPriority =
                AICommanderPriority.StrategicObjective;

            EvaluationInterval =
                Math.Max(
                    0.1f,
                    evaluationInterval);

            TimeSinceEvaluation = 0.0f;

            CurrentDecision =
                new AICommanderDecision();

            Active = false;
        }

        public bool Activate()
        {
            if (!Valid ||
                State ==
                    AICommanderState.Disabled)
            {
                return false;
            }

            Active = true;

            State =
                AICommanderState.Initializing;

            TimeSinceEvaluation = 0.0f;

            return true;
        }

        public void Deactivate()
        {
            Active = false;

            State =
                AICommanderState.Inactive;
        }

        public void Disable()
        {
            Active = false;

            State =
                AICommanderState.Disabled;

            CurrentDecision.Clear();
        }

        public void Update(
            float deltaTime)
        {
            if (!Active ||
                State ==
                    AICommanderState.Disabled)
            {
                return;
            }

            if (deltaTime < 0.0f)
                deltaTime = 0.0f;

            TimeSinceEvaluation +=
                deltaTime;

            if (State ==
                AICommanderState.Initializing)
            {
                State =
                    AICommanderState.Evaluating;
            }

            if (TimeSinceEvaluation >=
                EvaluationInterval)
            {
                TimeSinceEvaluation = 0.0f;

                State =
                    AICommanderState.Evaluating;
            }
        }

        public bool BeginPlanning()
        {
            if (!Active ||
                State !=
                    AICommanderState.Evaluating)
            {
                return false;
            }

            State =
                AICommanderState.Planning;

            return true;
        }

        public bool SetDecision(
            AICommanderObjective objective,
            AICommanderPriority priority,
            string intentId)
        {
            if (!Active ||
                State !=
                    AICommanderState.Planning)
            {
                return false;
            }

            CurrentObjective =
                objective;

            CurrentPriority =
                priority;

            CurrentDecision.Set(
                objective,
                priority,
                intentId);

            State =
                AICommanderState.Executing;

            return CurrentDecision.Valid;
        }

        public bool CompleteDecision()
        {
            if (!Active ||
                State !=
                    AICommanderState.Executing)
            {
                return false;
            }

            State =
                AICommanderState.Reassessing;

            return true;
        }

        public void BeginReevaluation()
        {
            if (!Active)
                return;

            State =
                AICommanderState.Evaluating;
        }

        public void SetEvaluationInterval(
            float seconds)
        {
            EvaluationInterval =
                Math.Max(
                    0.1f,
                    seconds);
        }

        public void ClearDecision()
        {
            CurrentDecision.Clear();

            CurrentObjective =
                AICommanderObjective.None;
        }
    }

    public sealed class AICommanderRegistry
    {
        private readonly
            System.Collections.Generic.Dictionary<
                string,
                AICommander> commanders =
            new System.Collections.Generic.Dictionary<
                string,
                AICommander>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(
            AICommander commander)
        {
            if (commander == null ||
                !commander.Valid ||
                commanders.ContainsKey(
                    commander.CommanderId))
            {
                return false;
            }

            commanders.Add(
                commander.CommanderId,
                commander);

            return true;
        }

        public bool Remove(
            string commanderId)
        {
            if (string.IsNullOrWhiteSpace(
                    commanderId))
            {
                return false;
            }

            return commanders.Remove(
                commanderId);
        }

        public bool TryGet(
            string commanderId,
            out AICommander commander)
        {
            return commanders.TryGetValue(
                commanderId,
                out commander);
        }

        public void UpdateAll(
            float deltaTime)
        {
            foreach (AICommander commander
                in commanders.Values)
            {
                commander.Update(
                    deltaTime);
            }
        }

        public System.Collections.Generic.IReadOnlyCollection<
            AICommander>
            GetCommanders()
        {
            return commanders.Values;
        }

        public void Clear()
        {
            commanders.Clear();
        }
    }
}
