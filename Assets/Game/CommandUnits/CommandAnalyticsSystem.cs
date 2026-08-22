using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.CommandUnits
{
    public enum AnalyticsMetricType
    {
        FleetStrength,
        UnitCount,
        CombatReadiness,
        ResourceLevel,
        NetworkHealth,
        IntelligenceConfidence,
        MissionProgress,
        BattlefieldControl
    }

    public sealed class AnalyticsMetric
    {
        public string MetricId { get; }
        public AnalyticsMetricType Type { get; }

        public float Value { get; private set; }
        public float PreviousValue { get; private set; }

        public DateTime LastUpdated { get; private set; }

        public AnalyticsMetric(
            string metricId,
            AnalyticsMetricType type)
        {
            MetricId =
                metricId ?? string.Empty;

            Type =
                type;

            LastUpdated =
                DateTime.UtcNow;
        }

        public void Update(
            float value)
        {
            PreviousValue =
                Value;

            Value =
                value;

            LastUpdated =
                DateTime.UtcNow;
        }

        public float Change()
        {
            return Value - PreviousValue;
        }
    }

    public sealed class CommandAnalyticsSystem
    {
        private readonly Dictionary<string, AnalyticsMetric> metrics =
            new Dictionary<string, AnalyticsMetric>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterMetric(
            string metricId,
            AnalyticsMetricType type)
        {
            if (string.IsNullOrWhiteSpace(metricId))
            {
                return;
            }

            if (!metrics.ContainsKey(metricId))
            {
                metrics.Add(
                    metricId,
                    new AnalyticsMetric(
                        metricId,
                        type));
            }
        }

        public void UpdateMetric(
            string metricId,
            float value)
        {
            if (metrics.TryGetValue(
                    metricId,
                    out AnalyticsMetric metric))
            {
                metric.Update(value);
            }
        }

        public bool TryGetMetric(
            string metricId,
            out AnalyticsMetric metric)
        {
            return metrics.TryGetValue(
                metricId,
                out metric);
        }

        public IReadOnlyCollection<AnalyticsMetric> GetMetrics()
        {
            return metrics.Values;
        }

        public void RemoveMetric(
            string metricId)
        {
            metrics.Remove(metricId);
        }

        public void Clear()
        {
            metrics.Clear();
        }
    }
}
