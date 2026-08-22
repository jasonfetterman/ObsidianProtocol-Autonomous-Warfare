using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Intelligence
{
    public sealed class RadarDetectionResult
    {
        public int SensorUnitId;
        public int TargetId;
        public float Distance;
        public float Bearing;
        public float SignalStrength;
        public float Confidence;
        public bool Detected;

        public RadarDetectionResult(
            int sensorUnitId,
            int targetId,
            float distance,
            float bearing,
            float signalStrength,
            float confidence)
        {
            SensorUnitId = sensorUnitId;
            TargetId = targetId;
            Distance = Math.Max(0f, distance);
            Bearing = bearing;
            SignalStrength =
                Math.Clamp(signalStrength, 0f, 1f);
            Confidence =
                Math.Clamp(confidence, 0f, 1f);
            Detected =
                SignalStrength > 0f &&
                Confidence > 0f;
        }
    }

    public sealed class RadarDetectionSystem
    {
        private readonly Dictionary<int, RadarDetectionResult> detections =
            new Dictionary<int, RadarDetectionResult>();

        public void ReportRadarDetection(
            int sensorUnitId,
            int targetId,
            float distance,
            float bearing,
            float signalStrength,
            float confidence)
        {
            if (sensorUnitId < 0 ||
                targetId < 0 ||
                signalStrength <= 0f ||
                confidence <= 0f)
            {
                return;
            }

            detections[targetId] =
                new RadarDetectionResult(
                    sensorUnitId,
                    targetId,
                    distance,
                    bearing,
                    signalStrength,
                    confidence);
        }

        public bool IsDetected(int targetId)
        {
            return detections.TryGetValue(
                       targetId,
                       out RadarDetectionResult result) &&
                   result.Detected;
        }

        public bool TryGetDetection(
            int targetId,
            out RadarDetectionResult result)
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
