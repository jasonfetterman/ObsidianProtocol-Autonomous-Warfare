using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Intelligence
{
    public sealed class IntelligenceReport
    {
        public int SourceUnitId;
        public int TargetId;
        public int AreaId;
        public string InformationType;
        public float Confidence;
        public DateTime Timestamp;

        public IntelligenceReport(
            int sourceUnitId,
            int targetId,
            int areaId,
            string informationType,
            float confidence)
        {
            SourceUnitId = sourceUnitId;
            TargetId = targetId;
            AreaId = areaId;
            InformationType =
                informationType ?? string.Empty;
            Confidence =
                Math.Clamp(confidence, 0f, 1f);
            Timestamp = DateTime.UtcNow;
        }
    }

    public sealed class IntelligenceSharingSystem
    {
        private readonly Dictionary<int, List<IntelligenceReport>> reports =
            new Dictionary<int, List<IntelligenceReport>>();

        public void ShareReport(
            int sourceUnitId,
            int targetId,
            int areaId,
            string informationType,
            float confidence)
        {
            if (sourceUnitId < 0)
            {
                return;
            }

            if (!reports.TryGetValue(
                    sourceUnitId,
                    out List<IntelligenceReport> unitReports))
            {
                unitReports =
                    new List<IntelligenceReport>();

                reports.Add(
                    sourceUnitId,
                    unitReports);
            }

            unitReports.Add(
                new IntelligenceReport(
                    sourceUnitId,
                    targetId,
                    areaId,
                    informationType,
                    confidence));
        }

        public bool TryGetReports(
            int sourceUnitId,
            out IReadOnlyList<IntelligenceReport> unitReports)
        {
            if (reports.TryGetValue(
                    sourceUnitId,
                    out List<IntelligenceReport> storedReports))
            {
                unitReports = storedReports;
                return true;
            }

            unitReports = null;
            return false;
        }

        public void ClearUnit(int sourceUnitId)
        {
            reports.Remove(sourceUnitId);
        }

        public void Clear()
        {
            reports.Clear();
        }
    }
}
