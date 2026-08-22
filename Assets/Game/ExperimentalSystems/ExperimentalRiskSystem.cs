using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.ExperimentalSystems
{
    public enum ExperimentalRiskType
    {
        Instability,
        Overload,
        SignalExposure,
        NetworkCompromise,
        AIDeviation,
        SystemFailure
    }

    public enum ExperimentalRiskLevel
    {
        None,
        Low,
        Moderate,
        High,
        Critical
    }

    public sealed class ExperimentalRiskProfile
    {
        private readonly Dictionary<ExperimentalRiskType, float> riskValues =
            new Dictionary<ExperimentalRiskType, float>();

        public string UnitId { get; }

        public float OverallRisk { get; private set; }

        public ExperimentalRiskLevel RiskLevel { get; private set; }

        public bool Critical =>
            RiskLevel ==
            ExperimentalRiskLevel.Critical;

        public ExperimentalRiskProfile(
            string unitId)
        {
            UnitId =
                unitId ?? string.Empty;

            foreach (ExperimentalRiskType type
                in Enum.GetValues(
                    typeof(ExperimentalRiskType)))
            {
                riskValues[type] = 0f;
            }

            Recalculate();
        }

        public void SetRisk(
            ExperimentalRiskType type,
            float value)
        {
            riskValues[type] =
                Math.Clamp(
                    value,
                    0f,
                    1f);

            Recalculate();
        }

        public void AddRisk(
            ExperimentalRiskType type,
            float amount)
        {
            float current =
                GetRisk(type);

            SetRisk(
                type,
                current +
                Math.Max(
                    0f,
                    amount));
        }

        public void ReduceRisk(
            ExperimentalRiskType type,
            float amount)
        {
            float current =
                GetRisk(type);

            SetRisk(
                type,
                current -
                Math.Max(
                    0f,
                    amount));
        }

        public float GetRisk(
            ExperimentalRiskType type)
        {
            return riskValues.TryGetValue(
                type,
                out float value)
                ? value
                : 0f;
        }

        private void Recalculate()
        {
            float total = 0f;

            foreach (float value
                in riskValues.Values)
            {
                total += value;
            }

            OverallRisk =
                riskValues.Count == 0
                    ? 0f
                    : total / riskValues.Count;

            if (OverallRisk >= 0.85f)
            {
                RiskLevel =
                    ExperimentalRiskLevel.Critical;
            }
            else if (OverallRisk >= 0.65f)
            {
                RiskLevel =
                    ExperimentalRiskLevel.High;
            }
            else if (OverallRisk >= 0.4f)
            {
                RiskLevel =
                    ExperimentalRiskLevel.Moderate;
            }
            else if (OverallRisk > 0f)
            {
                RiskLevel =
                    ExperimentalRiskLevel.Low;
            }
            else
            {
                RiskLevel =
                    ExperimentalRiskLevel.None;
            }
        }
    }

    public sealed class ExperimentalRiskSystem
    {
        private readonly Dictionary<string, ExperimentalRiskProfile> profiles =
            new Dictionary<string, ExperimentalRiskProfile>(
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
                    new ExperimentalRiskProfile(unitId));
            }
        }

        public void SetRisk(
            string unitId,
            ExperimentalRiskType type,
            float value)
        {
            RegisterUnit(unitId);

            profiles[unitId].SetRisk(
                type,
                value);
        }

        public void AddRisk(
            string unitId,
            ExperimentalRiskType type,
            float amount)
        {
            RegisterUnit(unitId);

            profiles[unitId].AddRisk(
                type,
                amount);
        }

        public void ReduceRisk(
            string unitId,
            ExperimentalRiskType type,
            float amount)
        {
            RegisterUnit(unitId);

            profiles[unitId].ReduceRisk(
                type,
                amount);
        }

        public float GetRisk(
            string unitId,
            ExperimentalRiskType type)
        {
            return profiles.TryGetValue(
                       unitId,
                       out ExperimentalRiskProfile profile)
                ? profile.GetRisk(type)
                : 0f;
        }

        public float GetOverallRisk(
            string unitId)
        {
            return profiles.TryGetValue(
                       unitId,
                       out ExperimentalRiskProfile profile)
                ? profile.OverallRisk
                : 0f;
        }

        public ExperimentalRiskLevel GetRiskLevel(
            string unitId)
        {
            return profiles.TryGetValue(
                       unitId,
                       out ExperimentalRiskProfile profile)
                ? profile.RiskLevel
                : ExperimentalRiskLevel.None;
        }

        public bool IsCritical(
            string unitId)
        {
            return profiles.TryGetValue(
                       unitId,
                       out ExperimentalRiskProfile profile) &&
                   profile.Critical;
        }

        public bool TryGetProfile(
            string unitId,
            out ExperimentalRiskProfile profile)
        {
            return profiles.TryGetValue(
                unitId,
                out profile);
        }

        public IReadOnlyCollection<ExperimentalRiskProfile>
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
