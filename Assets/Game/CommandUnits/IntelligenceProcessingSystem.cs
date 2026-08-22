using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.CommandUnits
{
    public enum IntelligenceType
    {
        Reconnaissance,
        Threat,
        Movement,
        Combat,
        Logistics,
        Strategic
    }

    public enum IntelligencePriority
    {
        Low,
        Normal,
        High,
        Critical
    }

    public sealed class IntelligenceReport
    {
        public string ReportId { get; }
        public IntelligenceType Type { get; }

        public IntelligencePriority Priority { get; private set; }

        public string SourceId { get; }
        public string SubjectId { get; }

        public string Data { get; private set; }

        public float Confidence { get; private set; }

        public DateTime CreatedAt { get; }

        public IntelligenceReport(
            string reportId,
            IntelligenceType type,
            IntelligencePriority priority,
            string sourceId,
            string subjectId,
            string data,
            float confidence)
        {
            ReportId =
                reportId ?? string.Empty;

            Type =
                type;

            Priority =
                priority;

            SourceId =
                sourceId ?? string.Empty;

            SubjectId =
                subjectId ?? string.Empty;

            Data =
                data ?? string.Empty;

            Confidence =
                Math.Clamp(
                    confidence,
                    0f,
                    1f);

            CreatedAt =
                DateTime.UtcNow;
        }

        public void UpdateConfidence(
            float confidence)
        {
            Confidence =
                Math.Clamp(
                    confidence,
                    0f,
                    1f);
        }

        public void SetPriority(
            IntelligencePriority priority)
        {
            Priority = priority;
        }
    }

    public sealed class IntelligenceProcessingSystem
    {
        private readonly Dictionary<string, IntelligenceReport> reports =
            new Dictionary<string, IntelligenceReport>(
                StringComparer.OrdinalIgnoreCase);

        public void SubmitReport(
            string reportId,
            IntelligenceType type,
            IntelligencePriority priority,
            string sourceId,
            string subjectId,
            string data,
            float confidence)
        {
            if (string.IsNullOrWhiteSpace(reportId))
            {
                return;
            }

            reports[reportId] =
                new IntelligenceReport(
                    reportId,
                    type,
                    priority,
                    sourceId,
                    subjectId,
                    data,
                    confidence);
        }

        public bool TryGetReport(
            string reportId,
            out IntelligenceReport report)
        {
            return reports.TryGetValue(
                reportId,
                out report);
        }

        public IReadOnlyCollection<IntelligenceReport> GetReports()
        {
            return reports.Values;
        }

        public int Count()
        {
            return reports.Count;
        }

        public void RemoveReport(
            string reportId)
        {
            reports.Remove(reportId);
        }

        public void Clear()
        {
            reports.Clear();
        }
    }
}
