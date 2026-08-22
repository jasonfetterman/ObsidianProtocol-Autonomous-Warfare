using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Garage
{
    public enum AIDiagnosticStatus
    {
        Unknown,
        Healthy,
        Warning,
        Fault,
        Offline
    }

    public sealed class AIDiagnosticResult
    {
        public string DiagnosticId { get; }
        public string OwnershipId { get; }
        public string Category { get; }
        public string Message { get; }

        public AIDiagnosticStatus Status { get; }

        public AIDiagnosticResult(
            string diagnosticId,
            string ownershipId,
            string category,
            string message,
            AIDiagnosticStatus status)
        {
            DiagnosticId =
                diagnosticId ?? string.Empty;

            OwnershipId =
                ownershipId ?? string.Empty;

            Category =
                category ?? string.Empty;

            Message =
                message ?? string.Empty;

            Status = status;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(
                DiagnosticId);
    }

    public sealed class AIDiagnostics
    {
        private readonly Dictionary<
            string,
            AIDiagnosticResult> results =
            new Dictionary<
                string,
                AIDiagnosticResult>(
                StringComparer.OrdinalIgnoreCase);

        public bool Running { get; private set; }

        public void Begin()
        {
            Running = true;
        }

        public void End()
        {
            Running = false;
        }

        public bool Register(
            AIDiagnosticResult result)
        {
            if (result == null ||
                !result.Valid ||
                results.ContainsKey(
                    result.DiagnosticId))
            {
                return false;
            }

            results.Add(
                result.DiagnosticId,
                result);

            return true;
        }

        public bool Remove(
            string diagnosticId)
        {
            if (string.IsNullOrWhiteSpace(
                    diagnosticId))
            {
                return false;
            }

            return results.Remove(
                diagnosticId);
        }

        public bool TryGet(
            string diagnosticId,
            out AIDiagnosticResult result)
        {
            return results.TryGetValue(
                diagnosticId,
                out result);
        }

        public bool HasFaults()
        {
            foreach (AIDiagnosticResult result in results.Values)
            {
                if (result.Status ==
                    AIDiagnosticStatus.Fault)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasWarnings()
        {
            foreach (AIDiagnosticResult result in results.Values)
            {
                if (result.Status ==
                    AIDiagnosticStatus.Warning)
                {
                    return true;
                }
            }

            return false;
        }

        public IReadOnlyCollection<
            AIDiagnosticResult>
            GetResults()
        {
            return results.Values;
        }

        public void Clear()
        {
            results.Clear();
        }

        public void Reset()
        {
            Running = false;
            results.Clear();
        }
    }
}
