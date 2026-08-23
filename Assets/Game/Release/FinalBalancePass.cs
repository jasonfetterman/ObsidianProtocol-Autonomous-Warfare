using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Release
{
    public sealed class BalanceMetric
    {
        public string MetricId { get; }

        public float TargetValue { get; private set; }

        public float ActualValue { get; private set; }

        public float Tolerance { get; private set; }

        public bool Passed { get; private set; }

        public BalanceMetric(
            string metricId,
            float targetValue,
            float tolerance)
        {
            MetricId =
                metricId ?? string.Empty;

            TargetValue = targetValue;
            ActualValue = 0f;
            Tolerance = Math.Max(0f, tolerance);
            Passed = false;
        }

        public void Evaluate(
            float actualValue)
        {
            ActualValue = actualValue;

            Passed =
                Math.Abs(
                    ActualValue - TargetValue)
                <= Tolerance;
        }
    }

    public sealed class FinalBalancePass
    {
        private readonly Dictionary<
            string,
            BalanceMetric> metrics =
            new Dictionary<
                string,
                BalanceMetric>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int MetricCount =>
            metrics.Count;

        public int PassedCount
        {
            get
            {
                int count = 0;

                foreach (BalanceMetric metric
                         in metrics.Values)
                {
                    if (metric.Passed)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public int FailedCount =>
            MetricCount - PassedCount;

        public bool Balanced =>
            Initialized &&
            MetricCount > 0 &&
            FailedCount == 0;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            metrics.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterMetric(
            string metricId,
            float targetValue,
            float tolerance)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(metricId) ||
                tolerance < 0f)
            {
                return false;
            }

            string id =
                metricId.Trim();

            if (metrics.ContainsKey(id))
            {
                return false;
            }

            metrics.Add(
                id,
                new BalanceMetric(
                    id,
                    targetValue,
                    tolerance));

            return true;
        }

        public bool EvaluateMetric(
            string metricId,
            float actualValue)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(metricId))
            {
                return false;
            }

            if (!metrics.TryGetValue(
                    metricId.Trim(),
                    out BalanceMetric metric))
            {
                return false;
            }

            metric.Evaluate(actualValue);

            return true;
        }

        public BalanceMetric GetMetric(
            string metricId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(metricId))
            {
                return null;
            }

            metrics.TryGetValue(
                metricId.Trim(),
                out BalanceMetric metric);

            return metric;
        }

        public IReadOnlyCollection<
            BalanceMetric>
            GetMetrics()
        {
            return metrics.Values;
        }

        public void Reset()
        {
            metrics.Clear();
            Initialized = false;
        }
    }
}
