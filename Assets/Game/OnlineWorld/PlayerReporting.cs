using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OnlineWorld
{
    public sealed class PlayerReport
    {
        public string ReportId { get; }

        public string ReporterId { get; }

        public string ReportedPlayerId { get; }

        public string Reason { get; }

        public bool Resolved { get; private set; }

        public DateTime CreatedAtUtc { get; }

        public PlayerReport(
            string reportId,
            string reporterId,
            string reportedPlayerId,
            string reason)
        {
            ReportId =
                reportId ?? string.Empty;

            ReporterId =
                reporterId ?? string.Empty;

            ReportedPlayerId =
                reportedPlayerId ?? string.Empty;

            Reason =
                reason ?? string.Empty;

            Resolved = false;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public bool Resolve()
        {
            if (Resolved)
            {
                return false;
            }

            Resolved = true;

            return true;
        }
    }

    public sealed class PlayerReporting
    {
        private readonly Dictionary<
            string,
            PlayerReport> reports =
            new Dictionary<
                string,
                PlayerReport>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ReportCount =>
            reports.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            reports.Clear();
            Initialized = true;

            return true;
        }

        public bool SubmitReport(
            string reportId,
            string reporterId,
            string reportedPlayerId,
            string reason)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(reportId) ||
                string.IsNullOrWhiteSpace(reporterId) ||
                string.IsNullOrWhiteSpace(reportedPlayerId) ||
                string.IsNullOrWhiteSpace(reason))
            {
                return false;
            }

            string id =
                reportId.Trim();

            if (reports.ContainsKey(id))
            {
                return false;
            }

            reports.Add(
                id,
                new PlayerReport(
                    id,
                    reporterId.Trim(),
                    reportedPlayerId.Trim(),
                    reason.Trim()));

            return true;
        }

        public bool ResolveReport(
            string reportId)
        {
            PlayerReport report =
                GetReport(reportId);

            return report != null &&
                   report.Resolve();
        }

        public PlayerReport GetReport(
            string reportId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(reportId))
            {
                return null;
            }

            reports.TryGetValue(
                reportId.Trim(),
                out PlayerReport report);

            return report;
        }

        public IReadOnlyCollection<
            PlayerReport>
            GetReports()
        {
            return reports.Values;
        }

        public void Reset()
        {
            reports.Clear();
            Initialized = false;
        }
    }
}
