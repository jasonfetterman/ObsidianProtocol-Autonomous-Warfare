using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Intelligence
{
    public sealed class SensorObservation
    {
        public int SensorUnitId;
        public SensorType SensorType;
        public int TargetId;
        public float Confidence;
        public float Distance;
        public DateTime Timestamp;

        public SensorObservation(
            int sensorUnitId,
            SensorType sensorType,
            int targetId,
            float confidence,
            float distance)
        {
            SensorUnitId = sensorUnitId;
            SensorType = sensorType;
            TargetId = targetId;
            Confidence =
                Math.Clamp(confidence, 0f, 1f);
            Distance =
                Math.Max(0f, distance);
            Timestamp = DateTime.UtcNow;
        }
    }

    public sealed class FusedSensorContact
    {
        public int TargetId;
        public float Confidence;
        public float EstimatedDistance;
        public int ObservationCount;

        public FusedSensorContact(int targetId)
        {
            TargetId = targetId;
            Confidence = 0f;
            EstimatedDistance = 0f;
            ObservationCount = 0;
        }
    }

    public sealed class SensorFusionSystem
    {
        private readonly Dictionary<int, List<SensorObservation>> observations =
            new Dictionary<int, List<SensorObservation>>();

        private readonly Dictionary<int, FusedSensorContact> contacts =
            new Dictionary<int, FusedSensorContact>();

        public void AddObservation(
            int sensorUnitId,
            SensorType sensorType,
            int targetId,
            float confidence,
            float distance)
        {
            if (sensorUnitId < 0 ||
                targetId < 0)
            {
                return;
            }

            if (!observations.TryGetValue(
                    targetId,
                    out List<SensorObservation> targetObservations))
            {
                targetObservations =
                    new List<SensorObservation>();

                observations.Add(
                    targetId,
                    targetObservations);
            }

            targetObservations.Add(
                new SensorObservation(
                    sensorUnitId,
                    sensorType,
                    targetId,
                    confidence,
                    distance));

            RecalculateContact(targetId);
        }

        private void RecalculateContact(int targetId)
        {
            if (!observations.TryGetValue(
                    targetId,
                    out List<SensorObservation> targetObservations) ||
                targetObservations.Count == 0)
            {
                return;
            }

            float confidenceTotal = 0f;
            float distanceTotal = 0f;

            foreach (SensorObservation observation in targetObservations)
            {
                confidenceTotal += observation.Confidence;
                distanceTotal += observation.Distance;
            }

            float averageConfidence =
                confidenceTotal / targetObservations.Count;

            float averageDistance =
                distanceTotal / targetObservations.Count;

            if (!contacts.TryGetValue(
                    targetId,
                    out FusedSensorContact contact))
            {
                contact =
                    new FusedSensorContact(targetId);

                contacts.Add(
                    targetId,
                    contact);
            }

            contact.Confidence =
                Math.Clamp(
                    averageConfidence,
                    0f,
                    1f);

            contact.EstimatedDistance =
                averageDistance;

            contact.ObservationCount =
                targetObservations.Count;
        }

        public bool TryGetContact(
            int targetId,
            out FusedSensorContact contact)
        {
            return contacts.TryGetValue(
                targetId,
                out contact);
        }

        public void ClearTarget(int targetId)
        {
            observations.Remove(targetId);
            contacts.Remove(targetId);
        }

        public void Clear()
        {
            observations.Clear();
            contacts.Clear();
        }
    }
}
