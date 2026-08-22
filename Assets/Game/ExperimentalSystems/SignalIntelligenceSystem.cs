using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.ExperimentalSystems
{
    public enum SignalType
    {
        Communication,
        Radar,
        Navigation,
        Telemetry,
        Command,
        Unknown
    }

    public enum SignalClassification
    {
        Unknown,
        Friendly,
        Neutral,
        Hostile,
        Suspicious
    }

    public sealed class DetectedSignal
    {
        public string SignalId { get; }
        public string SourceId { get; }

        public SignalType Type { get; }

        public SignalClassification Classification
        {
            get;
            private set;
        }

        public float Strength { get; private set; }
        public float Confidence { get; private set; }

        public bool Active { get; private set; }

        public DateTime LastDetected { get; private set; }

        public DetectedSignal(
            string signalId,
            string sourceId,
            SignalType type)
        {
            SignalId =
                signalId ?? string.Empty;

            SourceId =
                sourceId ?? string.Empty;

            Type =
                type;

            Classification =
                SignalClassification.Unknown;

            Active = true;

            LastDetected =
                DateTime.UtcNow;
        }

        public void Update(
            float strength,
            float confidence)
        {
            Strength =
                Math.Clamp(
                    strength,
                    0f,
                    1f);

            Confidence =
                Math.Clamp(
                    confidence,
                    0f,
                    1f);

            Active = true;

            LastDetected =
                DateTime.UtcNow;
        }

        public void Classify(
            SignalClassification classification)
        {
            Classification =
                classification;
        }

        public void MarkLost()
        {
            Active = false;
        }
    }

    public sealed class SignalIntelligenceSystem
    {
        private readonly Dictionary<string, DetectedSignal> signals =
            new Dictionary<string, DetectedSignal>(
                StringComparer.OrdinalIgnoreCase);

        public void DetectSignal(
            string signalId,
            string sourceId,
            SignalType type,
            float strength,
            float confidence)
        {
            if (string.IsNullOrWhiteSpace(signalId))
            {
                return;
            }

            if (!signals.TryGetValue(
                    signalId,
                    out DetectedSignal signal))
            {
                signal =
                    new DetectedSignal(
                        signalId,
                        sourceId,
                        type);

                signals.Add(
                    signalId,
                    signal);
            }

            signal.Update(
                strength,
                confidence);
        }

        public void ClassifySignal(
            string signalId,
            SignalClassification classification)
        {
            if (signals.TryGetValue(
                    signalId,
                    out DetectedSignal signal))
            {
                signal.Classify(
                    classification);
            }
        }

        public void MarkSignalLost(
            string signalId)
        {
            if (signals.TryGetValue(
                    signalId,
                    out DetectedSignal signal))
            {
                signal.MarkLost();
            }
        }

        public bool TryGetSignal(
            string signalId,
            out DetectedSignal signal)
        {
            return signals.TryGetValue(
                signalId,
                out signal);
        }

        public IReadOnlyCollection<DetectedSignal>
            GetSignals()
        {
            return signals.Values;
        }

        public int GetActiveSignalCount()
        {
            int count = 0;

            foreach (DetectedSignal signal in signals.Values)
            {
                if (signal.Active)
                {
                    count++;
                }
            }

            return count;
        }

        public void RemoveSignal(
            string signalId)
        {
            signals.Remove(signalId);
        }

        public void Clear()
        {
            signals.Clear();
        }
    }
}
