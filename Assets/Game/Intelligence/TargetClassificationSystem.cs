using System.Collections.Generic;

namespace ObsidianProtocol.Game.Intelligence
{
    public enum TargetClassification
    {
        Unknown,
        Infantry,
        GroundVehicle,
        AirVehicle,
        NavalVehicle,
        Structure,
        CommandUnit,
        SupportUnit,
        ReconUnit,
        Experimental
    }

    public sealed class TargetClassificationRecord
    {
        public int TargetId;
        public TargetClassification Classification;
        public float Confidence;

        public TargetClassificationRecord(int targetId)
        {
            TargetId = targetId;
            Classification =
                TargetClassification.Unknown;
            Confidence = 0f;
        }
    }

    public sealed class TargetClassificationSystem
    {
        private readonly Dictionary<int, TargetClassificationRecord> targets =
            new Dictionary<int, TargetClassificationRecord>();

        public void RegisterTarget(int targetId)
        {
            if (targetId < 0)
            {
                return;
            }

            if (!targets.ContainsKey(targetId))
            {
                targets.Add(
                    targetId,
                    new TargetClassificationRecord(targetId));
            }
        }

        public void ClassifyTarget(
            int targetId,
            TargetClassification classification,
            float confidence)
        {
            RegisterTarget(targetId);

            TargetClassificationRecord record =
                targets[targetId];

            record.Classification = classification;
            record.Confidence =
                System.Math.Clamp(
                    confidence,
                    0f,
                    1f);
        }

        public bool TryGetClassification(
            int targetId,
            out TargetClassificationRecord record)
        {
            return targets.TryGetValue(
                targetId,
                out record);
        }

        public TargetClassification GetClassification(
            int targetId)
        {
            return targets.TryGetValue(
                       targetId,
                       out TargetClassificationRecord record)
                ? record.Classification
                : TargetClassification.Unknown;
        }

        public void RemoveTarget(int targetId)
        {
            targets.Remove(targetId);
        }

        public void Clear()
        {
            targets.Clear();
        }
    }
}
