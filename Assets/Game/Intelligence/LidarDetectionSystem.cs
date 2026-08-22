using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Intelligence
{
    public sealed class LidarDetectionResult
    {
        public int SensorUnitId;
        public int TargetId;
        public float Distance;
        public float PointDensity;
        public float Confidence;
        public bool Detected;

        public LidarDetectionResult(
            int sensorUnitId,
            int targetId,
            float distance,
            float pointDensity,
            float confidence)
        {
            SensorUnitId = sensorUnitId;
            TargetId = targetId;
            Distance = Math.Max(0f, distance);
            PointDensity =
                Math.Clamp(pointDensity, 0f, 1f);
            Confidence =
                Math.Clamp(confidence, 0f, 1f);
            Detected =
                PointDensity > 0f &&
                Confidence > 0f;
        }
    }

    public sealed class LidarDetectionSystem
    {
        private readonly Dictionary<int, LidarDetectionResult> detections =
            new Dictionary<int, LidarDetectionResult>();

        public void ReportLidarDetection(
            int sensorUnitId,
            int targetId,
            float distance,
            float pointDensity,
            float confidence)
        {
            if (sensorUnitId < 0 ||
                targetId < 0 ||
                pointDensity <= 0f ||
                confidence <= 0f)
            {
                return;
            }

            detections[targetId] =
                new LidarDetectionResult(
                    sensorUnitId,
                    targetId,
                    distance,
                    pointDensity,
                    confidence);
        }

        public bool IsDetected(int targetId)
        {
            return detections.TryGetValue(
                       targetId,
                       out LidarDetectionResult result) &&
                   result.Detected;
        }

        public bool TryGetDetection(
            int targetId,
            out LidarDetectionResult result)
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
