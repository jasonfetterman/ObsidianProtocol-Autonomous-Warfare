using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Intelligence
{
    public sealed class ThermalDetectionResult
    {
        public int SensorUnitId;
        public int TargetId;
        public float Distance;
        public float HeatSignature;
        public float Confidence;
        public bool Detected;

        public ThermalDetectionResult(
            int sensorUnitId,
            int targetId,
            float distance,
            float heatSignature,
            float confidence)
        {
            SensorUnitId = sensorUnitId;
            TargetId = targetId;
            Distance = distance;
            HeatSignature =
                Math.Clamp(heatSignature, 0f, 1f);
            Confidence =
                Math.Clamp(confidence, 0f, 1f);
            Detected =
                HeatSignature > 0f &&
                Confidence > 0f;
        }
    }

    public sealed class ThermalDetectionSystem
    {
        private readonly Dictionary<int, ThermalDetectionResult> detections =
            new Dictionary<int, ThermalDetectionResult>();

        public void ReportThermalDetection(
            int sensorUnitId,
            int targetId,
            float distance,
            float heatSignature,
            float confidence)
        {
            if (sensorUnitId < 0 ||
                targetId < 0 ||
                heatSignature <= 0f ||
                confidence <= 0f)
            {
                return;
            }

            detections[targetId] =
                new ThermalDetectionResult(
                    sensorUnitId,
                    targetId,
                    distance,
                    heatSignature,
                    confidence);
        }

        public bool IsDetected(int targetId)
        {
            return detections.TryGetValue(
                       targetId,
                       out ThermalDetectionResult result) &&
                   result.Detected;
        }

        public bool TryGetDetection(
            int targetId,
            out ThermalDetectionResult result)
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
