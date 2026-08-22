using System.Collections.Generic;

namespace ObsidianProtocol.Game.Intelligence
{
    public enum DetectionConfidence
    {
        None,
        Low,
        Medium,
        High,
        Confirmed
    }

    public sealed class DetectionResult
    {
        public string SensorId;
        public int TargetId;
        public float Distance;
        public DetectionConfidence Confidence;
        public bool IsDetected;

        public DetectionResult(
            string sensorId,
            int targetId,
            float distance,
            DetectionConfidence confidence)
        {
            SensorId = sensorId;
            TargetId = targetId;
            Distance = distance;
            Confidence = confidence;
            IsDetected = confidence != DetectionConfidence.None;
        }
    }

    public sealed class DetectionSystem
    {
        private readonly Dictionary<int, DetectionResult> detections =
            new Dictionary<int, DetectionResult>();

        public void ReportDetection(
            string sensorId,
            int targetId,
            float distance,
            DetectionConfidence confidence)
        {
            if (targetId < 0 ||
                confidence == DetectionConfidence.None)
            {
                return;
            }

            detections[targetId] =
                new DetectionResult(
                    sensorId,
                    targetId,
                    distance,
                    confidence);
        }

        public bool IsDetected(int targetId)
        {
            return detections.TryGetValue(
                targetId,
                out DetectionResult result) &&
                result.IsDetected;
        }

        public bool TryGetDetection(
            int targetId,
            out DetectionResult result)
        {
            return detections.TryGetValue(
                targetId,
                out result);
        }

        public void ClearDetection(int targetId)
        {
            detections.Remove(targetId);
        }

        public void Clear()
        {
            detections.Clear();
        }
    }
}
