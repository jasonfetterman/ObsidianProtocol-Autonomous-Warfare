using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.ExperimentalSystems
{
    public enum ExperimentalProgressionState
    {
        Locked,
        Researching,
        Unlocked,
        FieldTested,
        Mastered
    }

    public enum ExperimentalProgressionRequirement
    {
        Research,
        FieldTesting,
        Stability,
        MissionCompletion,
        ResourceInvestment
    }

    public sealed class ExperimentalProgressionProfile
    {
        private readonly HashSet<ExperimentalProgressionRequirement>
            requirements =
                new HashSet<ExperimentalProgressionRequirement>();

        public string UnitId { get; }

        public ExperimentalProgressionState State
        {
            get;
            private set;
        }

        public float ResearchProgress
        {
            get;
            private set;
        }

        public float FieldTestProgress
        {
            get;
            private set;
        }

        public float StabilityRequirement
        {
            get;
            private set;
        }

        public ExperimentalProgressionProfile(
            string unitId)
        {
            UnitId =
                unitId ?? string.Empty;

            State =
                ExperimentalProgressionState.Locked;

            StabilityRequirement =
                0.5f;
        }

        public void AddRequirement(
            ExperimentalProgressionRequirement requirement)
        {
            requirements.Add(
                requirement);
        }

        public bool HasRequirement(
            ExperimentalProgressionRequirement requirement)
        {
            return requirements.Contains(
                requirement);
        }

        public void SetStabilityRequirement(
            float value)
        {
            StabilityRequirement =
                Math.Clamp(
                    value,
                    0f,
                    1f);
        }

        public void AddResearch(
            float amount)
        {
            if (State ==
                ExperimentalProgressionState.Mastered)
            {
                return;
            }

            ResearchProgress =
                Math.Clamp(
                    ResearchProgress +
                    Math.Max(
                        0f,
                        amount),
                    0f,
                    1f);

            if (ResearchProgress >= 1f &&
                State ==
                ExperimentalProgressionState.Locked)
            {
                State =
                    ExperimentalProgressionState.Researching;
            }
        }

        public void AddFieldTesting(
            float amount)
        {
            if (State ==
                ExperimentalProgressionState.Mastered)
            {
                return;
            }

            FieldTestProgress =
                Math.Clamp(
                    FieldTestProgress +
                    Math.Max(
                        0f,
                        amount),
                    0f,
                    1f);
        }

        public bool TryUnlock(
            bool researchComplete,
            bool fieldTestingComplete,
            bool stabilityRequirementMet,
            bool missionRequirementMet,
            bool resourceRequirementMet)
        {
            if (HasRequirement(
                    ExperimentalProgressionRequirement.Research) &&
                !researchComplete)
            {
                return false;
            }

            if (HasRequirement(
                    ExperimentalProgressionRequirement.FieldTesting) &&
                !fieldTestingComplete)
            {
                return false;
            }

            if (HasRequirement(
                    ExperimentalProgressionRequirement.Stability) &&
                !stabilityRequirementMet)
            {
                return false;
            }

            if (HasRequirement(
                    ExperimentalProgressionRequirement.MissionCompletion) &&
                !missionRequirementMet)
            {
                return false;
            }

            if (HasRequirement(
                    ExperimentalProgressionRequirement.ResourceInvestment) &&
                !resourceRequirementMet)
            {
                return false;
            }

            State =
                ExperimentalProgressionState.Unlocked;

            return true;
        }

        public void MarkFieldTested()
        {
            if (State ==
                ExperimentalProgressionState.Unlocked)
            {
                State =
                    ExperimentalProgressionState.FieldTested;
            }
        }

        public void MarkMastered()
        {
            if (State ==
                ExperimentalProgressionState.FieldTested)
            {
                State =
                    ExperimentalProgressionState.Mastered;
            }
        }
    }

    public sealed class ExperimentalProgressionSystem
    {
        private readonly Dictionary<string, ExperimentalProgressionProfile>
            profiles =
                new Dictionary<string, ExperimentalProgressionProfile>(
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
                    new ExperimentalProgressionProfile(
                        unitId));
            }
        }

        public void AddRequirement(
            string unitId,
            ExperimentalProgressionRequirement requirement)
        {
            RegisterUnit(unitId);

            profiles[unitId].AddRequirement(
                requirement);
        }

        public void SetStabilityRequirement(
            string unitId,
            float value)
        {
            RegisterUnit(unitId);

            profiles[unitId].SetStabilityRequirement(
                value);
        }

        public void AddResearch(
            string unitId,
            float amount)
        {
            RegisterUnit(unitId);

            profiles[unitId].AddResearch(
                amount);
        }

        public void AddFieldTesting(
            string unitId,
            float amount)
        {
            RegisterUnit(unitId);

            profiles[unitId].AddFieldTesting(
                amount);
        }

        public bool TryUnlock(
            string unitId,
            bool researchComplete,
            bool fieldTestingComplete,
            bool stabilityRequirementMet,
            bool missionRequirementMet,
            bool resourceRequirementMet)
        {
            return profiles.TryGetValue(
                       unitId,
                       out ExperimentalProgressionProfile profile) &&
                   profile.TryUnlock(
                       researchComplete,
                       fieldTestingComplete,
                       stabilityRequirementMet,
                       missionRequirementMet,
                       resourceRequirementMet);
        }

        public void MarkFieldTested(
            string unitId)
        {
            if (profiles.TryGetValue(
                    unitId,
                    out ExperimentalProgressionProfile profile))
            {
                profile.MarkFieldTested();
            }
        }

        public void MarkMastered(
            string unitId)
        {
            if (profiles.TryGetValue(
                    unitId,
                    out ExperimentalProgressionProfile profile))
            {
                profile.MarkMastered();
            }
        }

        public bool TryGetProfile(
            string unitId,
            out ExperimentalProgressionProfile profile)
        {
            return profiles.TryGetValue(
                unitId,
                out profile);
        }

        public IReadOnlyCollection<ExperimentalProgressionProfile>
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
