using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Intelligence
{
    public sealed class VisualDetectionResult
    {
        public int ObserverId;
        public int TargetId;
        public float Distance;
        public float FieldOfView;
        public float Confidence;
        public bool LineOfSight;

        public VisualDetectionResult(
            int observerId,
            int targetId,
            float distance,
            float fieldOfView,
            float confidence,
            bool lineOfSight)
        {
            ObserverId = observerId;
            TargetId = targetId;
            Distance = distance;
            FieldOfView = fieldOfView;
            Confidence = Math.Clamp(confidence, 0f, 1f);
            LineOfSight = lineOfSight;
        }
    }

    public sealed class VisualDetectionSystem
    {
        private readonly Dictionary<int, VisualDetectionResult> detections =
            new Dictionary<int, VisualDetectionResult>();

        public void ReportVisualDetection(
            int observerId,
            int targetId,
            float distance,
            float fieldOfView,
            float confidence,
            bool lineOfSight)
        {
            if (observerId < 0 ||
                targetId < 0 ||
                !lineOfSight)
            {
                return;
            }

            detections[targetId] =
                new VisualDetectionResult(
                    observerId,
                    targetId,
                    distance,
                    fieldOfView,
                    confidence,
                    lineOfSight);
        }

        public bool IsVisible(int targetId)
        {
            return detections.TryGetValue(
                       targetId,
                       out VisualDetectionResult result) &&
                   result.LineOfSight;
        }

        public bool TryGetDetection(
            int targetId,
            out VisualDetectionResult result)
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
