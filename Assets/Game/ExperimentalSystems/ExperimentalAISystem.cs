using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.ExperimentalSystems
{
    public enum ExperimentalAIState
    {
        Offline,
        Initializing,
        Learning,
        Operational,
        Unstable,
        Contained
    }

    public enum ExperimentalAIDecision
    {
        Observe,
        Analyze,
        Adapt,
        Assist,
        Retreat,
        Contain
    }

    public sealed class ExperimentalAIProfile
    {
        public string UnitId { get; }

        public ExperimentalAIState State { get; private set; }

        public ExperimentalAIDecision CurrentDecision
        {
            get;
            private set;
        }

        public float LearningRate { get; private set; }
        public float AdaptationLevel { get; private set; }
        public float Stability { get; private set; }

        public bool Autonomous { get; private set; }

        public ExperimentalAIProfile(
            string unitId)
        {
            UnitId =
                unitId ?? string.Empty;

            State =
                ExperimentalAIState.Offline;

            CurrentDecision =
                ExperimentalAIDecision.Observe;

            LearningRate = 0.1f;
            AdaptationLevel = 0f;
            Stability = 1f;
        }

        public void Configure(
            float learningRate,
            bool autonomous)
        {
            LearningRate =
                Math.Clamp(
                    learningRate,
                    0f,
                    1f);

            Autonomous =
                autonomous;
        }

        public void SetState(
            ExperimentalAIState state)
        {
            State = state;
        }

        public void SetDecision(
            ExperimentalAIDecision decision)
        {
            CurrentDecision =
                decision;
        }

        public void Learn(
            float amount)
        {
            if (State ==
                ExperimentalAIState.Contained ||
                State ==
                ExperimentalAIState.Offline)
            {
                return;
            }

            AdaptationLevel =
                Math.Clamp(
                    AdaptationLevel +
                    Math.Max(
                        0f,
                        amount) *
                    LearningRate,
                    0f,
                    1f);
        }

        public void ApplyInstability(
            float amount)
        {
            Stability =
                Math.Clamp(
                    Stability -
                    Math.Max(
                        0f,
                        amount),
                    0f,
                    1f);

            if (Stability <= 0f)
            {
                State =
                    ExperimentalAIState.Unstable;

                CurrentDecision =
                    ExperimentalAIDecision.Contain;
            }
        }

        public void Stabilize(
            float amount)
        {
            Stability =
                Math.Clamp(
                    Stability +
                    Math.Max(
                        0f,
                        amount),
                    0f,
                    1f);

            if (Stability >= 0.5f &&
                State ==
                ExperimentalAIState.Unstable)
            {
                State =
                    ExperimentalAIState.Operational;
            }
        }
    }

    public sealed class ExperimentalAISystem
    {
        private readonly Dictionary<string, ExperimentalAIProfile> profiles =
            new Dictionary<string, ExperimentalAIProfile>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterUnit(
            string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!profiles.ContainsKey(unitId))
            {
                profiles.Add(
                    unitId,
                    new ExperimentalAIProfile(unitId));
            }
        }

        public void ConfigureUnit(
            string unitId,
            float learningRate,
            bool autonomous)
        {
            RegisterUnit(unitId);

            profiles[unitId].Configure(
                learningRate,
                autonomous);
        }

        public void SetState(
            string unitId,
            ExperimentalAIState state)
        {
            RegisterUnit(unitId);

            profiles[unitId].SetState(
                state);
        }

        public void SetDecision(
            string unitId,
            ExperimentalAIDecision decision)
        {
            RegisterUnit(unitId);

            profiles[unitId].SetDecision(
                decision);
        }

        public void Learn(
            string unitId,
            float amount)
        {
            if (profiles.TryGetValue(
                    unitId,
                    out ExperimentalAIProfile profile))
            {
                profile.Learn(amount);
            }
        }

        public void ApplyInstability(
            string unitId,
            float amount)
        {
            if (profiles.TryGetValue(
                    unitId,
                    out ExperimentalAIProfile profile))
            {
                profile.ApplyInstability(
                    amount);
            }
        }

        public void Stabilize(
            string unitId,
            float amount)
        {
            if (profiles.TryGetValue(
                    unitId,
                    out ExperimentalAIProfile profile))
            {
                profile.Stabilize(
                    amount);
            }
        }

        public bool TryGetProfile(
            string unitId,
            out ExperimentalAIProfile profile)
        {
            return profiles.TryGetValue(
                unitId,
                out profile);
        }

        public IReadOnlyCollection<ExperimentalAIProfile>
            GetProfiles()
        {
            return profiles.Values;
        }

        public void RemoveUnit(
            string unitId)
        {
            profiles.Remove(unitId);
        }

        public void Clear()
        {
            profiles.Clear();
        }
    }
}
