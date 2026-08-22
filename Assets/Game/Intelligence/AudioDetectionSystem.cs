using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Intelligence
{
    public sealed class AudioDetectionResult
    {
        public int ListenerId;
        public int SourceId;
        public float Distance;
        public float SignalStrength;
        public DetectionConfidence Confidence;

        public AudioDetectionResult(
            int listenerId,
            int sourceId,
            float distance,
            float signalStrength,
            DetectionConfidence confidence)
        {
            ListenerId = listenerId;
            SourceId = sourceId;
            Distance = distance;
            SignalStrength =
                Math.Clamp(signalStrength, 0f, 1f);
            Confidence = confidence;
        }
    }

    public sealed class AudioDetectionSystem
    {
        private readonly Dictionary<int, AudioDetectionResult> detections =
            new Dictionary<int, AudioDetectionResult>();

        public void ReportAudioDetection(
            int listenerId,
            int sourceId,
            float distance,
            float signalStrength,
            DetectionConfidence confidence)
        {
            if (listenerId < 0 ||
                sourceId < 0 ||
                confidence == DetectionConfidence.None)
            {
                return;
            }

            detections[sourceId] =
                new AudioDetectionResult(
                    listenerId,
                    sourceId,
                    distance,
                    signalStrength,
                    confidence);
        }

        public bool IsDetected(int sourceId)
        {
            return detections.ContainsKey(sourceId);
        }

        public bool TryGetDetection(
            int sourceId,
            out AudioDetectionResult result)
        {
            return detections.TryGetValue(
                sourceId,
                out result);
        }

        public void ClearDetection(int sourceId)
        {
            detections.Remove(sourceId);
        }

        public void Clear()
        {
            detections.Clear();
        }
    }
}
