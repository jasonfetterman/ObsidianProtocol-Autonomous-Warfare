using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Logistics
{
    public enum LogisticsDisruptionType
    {
        Blocked,
        Damaged,
        Destroyed,
        Interdicted,
        Delayed
    }

    public enum LogisticsDisruptionState
    {
        Active,
        Resolved
    }

    public sealed class LogisticsDisruption
    {
        public string DisruptionId { get; }

        public string TargetId { get; }

        public LogisticsDisruptionType Type { get; }

        public float Severity { get; }

        public float DelayMultiplier { get; }

        public LogisticsDisruptionState State { get; private set; }

        public LogisticsDisruption(
            string disruptionId,
            string targetId,
            LogisticsDisruptionType type,
            float severity,
            float delayMultiplier)
        {
            DisruptionId =
                disruptionId ?? string.Empty;

            TargetId =
                targetId ?? string.Empty;

            Type =
                type;

            Severity =
                Math.Max(
                    0f,
                    Math.Min(
                        1f,
                        severity));

            DelayMultiplier =
                Math.Max(
                    1f,
                    delayMultiplier);

            State =
                LogisticsDisruptionState.Active;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(
                DisruptionId) &&
            !string.IsNullOrWhiteSpace(
                TargetId);

        public bool Active =>
            State ==
            LogisticsDisruptionState.Active;

        public float ApplyDelay(
            float baseTravelTime)
        {
            if (!Active)
            {
                return Math.Max(
                    0f,
                    baseTravelTime);
            }

            return Math.Max(
                0f,
                baseTravelTime * DelayMultiplier);
        }

        public void Resolve()
        {
            if (State ==
                LogisticsDisruptionState.Active)
            {
                State =
                    LogisticsDisruptionState.Resolved;
            }
        }
    }

    public sealed class LogisticsDisruptionSystem
    {
        private readonly Dictionary<string, LogisticsDisruption>
            disruptions =
                new Dictionary<string, LogisticsDisruption>(
                    StringComparer.OrdinalIgnoreCase);

        public bool RegisterDisruption(
            LogisticsDisruption disruption)
        {
            if (disruption == null ||
                !disruption.Valid ||
                disruptions.ContainsKey(
                    disruption.DisruptionId))
            {
                return false;
            }

            disruptions.Add(
                disruption.DisruptionId,
                disruption);

            return true;
        }

        public bool RemoveDisruption(
            string disruptionId)
        {
            if (string.IsNullOrWhiteSpace(
                    disruptionId))
            {
                return false;
            }

            return disruptions.Remove(
                disruptionId);
        }

        public bool TryGetDisruption(
            string disruptionId,
            out LogisticsDisruption disruption)
        {
            return disruptions.TryGetValue(
                disruptionId,
                out disruption);
        }

        public bool ResolveDisruption(
            string disruptionId)
        {
            if (!disruptions.TryGetValue(
                    disruptionId,
                    out LogisticsDisruption disruption))
            {
                return false;
            }

            disruption.Resolve();

            return true;
        }

        public bool IsDisrupted(
            string targetId)
        {
            if (string.IsNullOrWhiteSpace(
                    targetId))
            {
                return false;
            }

            foreach (
                LogisticsDisruption disruption
                in disruptions.Values)
            {
                if (disruption.Active &&
                    string.Equals(
                        disruption.TargetId,
                        targetId,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public float GetTravelTime(
            string targetId,
            float baseTravelTime)
        {
            float travelTime =
                Math.Max(
                    0f,
                    baseTravelTime);

            if (string.IsNullOrWhiteSpace(
                    targetId))
            {
                return travelTime;
            }

            foreach (
                LogisticsDisruption disruption
                in disruptions.Values)
            {
                if (disruption.Active &&
                    string.Equals(
                        disruption.TargetId,
                        targetId,
                        StringComparison.OrdinalIgnoreCase))
                {
                    travelTime =
                        disruption.ApplyDelay(
                            travelTime);
                }
            }

            return travelTime;
        }

        public IReadOnlyCollection<LogisticsDisruption>
            GetDisruptions()
        {
            return disruptions.Values;
        }

        public IReadOnlyCollection<LogisticsDisruption>
            GetActiveDisruptions()
        {
            List<LogisticsDisruption> active =
                new List<LogisticsDisruption>();

            foreach (
                LogisticsDisruption disruption
                in disruptions.Values)
            {
                if (disruption.Active)
                {
                    active.Add(
                        disruption);
                }
            }

            return active;
        }

        public void Clear()
        {
            disruptions.Clear();
        }
    }
}
