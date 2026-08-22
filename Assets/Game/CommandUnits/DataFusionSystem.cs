using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.CommandUnits
{
    public enum FusionConfidence
    {
        Unknown,
        Low,
        Medium,
        High,
        Confirmed
    }

    public sealed class FusedData
    {
        public string SubjectId { get; }

        public string Data { get; private set; }

        public float Confidence { get; private set; }

        public int SourceCount { get; private set; }

        public FusionConfidence ConfidenceLevel
        {
            get
            {
                if (Confidence >= 0.95f)
                    return FusionConfidence.Confirmed;

                if (Confidence >= 0.75f)
                    return FusionConfidence.High;

                if (Confidence >= 0.5f)
                    return FusionConfidence.Medium;

                if (Confidence > 0f)
                    return FusionConfidence.Low;

                return FusionConfidence.Unknown;
            }
        }

        public FusedData(
            string subjectId)
        {
            SubjectId =
                subjectId ?? string.Empty;

            Data =
                string.Empty;
        }

        public void Update(
            string data,
            float confidence,
            int sourceCount)
        {
            Data =
                data ?? string.Empty;

            Confidence =
                Math.Clamp(
                    confidence,
                    0f,
                    1f);

            SourceCount =
                Math.Max(
                    0,
                    sourceCount);
        }
    }

    public sealed class DataFusionSystem
    {
        private readonly Dictionary<string, FusedData> fusedData =
            new Dictionary<string, FusedData>(
                StringComparer.OrdinalIgnoreCase);

        public void Fuse(
            string subjectId,
            string data,
            float confidence,
            int sourceCount)
        {
            if (string.IsNullOrWhiteSpace(subjectId))
            {
                return;
            }

            if (!fusedData.TryGetValue(
                    subjectId,
                    out FusedData result))
            {
                result =
                    new FusedData(subjectId);

                fusedData.Add(
                    subjectId,
                    result);
            }

            result.Update(
                data,
                confidence,
                sourceCount);
        }

        public bool TryGetFusedData(
            string subjectId,
            out FusedData result)
        {
            return fusedData.TryGetValue(
                subjectId,
                out result);
        }

        public IReadOnlyCollection<FusedData> GetAll()
        {
            return fusedData.Values;
        }

        public int Count()
        {
            return fusedData.Count;
        }

        public void Remove(
            string subjectId)
        {
            fusedData.Remove(subjectId);
        }

        public void Clear()
        {
            fusedData.Clear();
        }
    }
}
