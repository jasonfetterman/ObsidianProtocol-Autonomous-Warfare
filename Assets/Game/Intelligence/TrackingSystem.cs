using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Intelligence
{
    public sealed class TrackedContact
    {
        public int TargetId;
        public int LastSensorUnitId;
        public float Confidence;
        public float Distance;
        public float Bearing;
        public DateTime LastUpdated;
        public bool Active;

        public TrackedContact(int targetId)
        {
            TargetId = targetId;
            LastSensorUnitId = -1;
            Confidence = 0f;
            Distance = 0f;
            Bearing = 0f;
            LastUpdated = DateTime.UtcNow;
            Active = false;
        }
    }

    public sealed class TrackingSystem
    {
        private readonly Dictionary<int, TrackedContact> contacts =
            new Dictionary<int, TrackedContact>();

        public void UpdateContact(
            int targetId,
            int sensorUnitId,
            float confidence,
            float distance,
            float bearing)
        {
            if (targetId < 0 ||
                sensorUnitId < 0)
            {
                return;
            }

            if (!contacts.TryGetValue(
                    targetId,
                    out TrackedContact contact))
            {
                contact =
                    new TrackedContact(targetId);

                contacts.Add(
                    targetId,
                    contact);
            }

            contact.LastSensorUnitId = sensorUnitId;
            contact.Confidence =
                Math.Clamp(confidence, 0f, 1f);
            contact.Distance =
                Math.Max(0f, distance);
            contact.Bearing = bearing;
            contact.LastUpdated = DateTime.UtcNow;
            contact.Active = true;
        }

        public bool IsTracked(int targetId)
        {
            return contacts.TryGetValue(
                       targetId,
                       out TrackedContact contact) &&
                   contact.Active;
        }

        public bool TryGetContact(
            int targetId,
            out TrackedContact contact)
        {
            return contacts.TryGetValue(
                targetId,
                out contact);
        }

        public void MarkLost(int targetId)
        {
            if (contacts.TryGetValue(
                    targetId,
                    out TrackedContact contact))
            {
                contact.Active = false;
            }
        }

        public void RemoveContact(int targetId)
        {
            contacts.Remove(targetId);
        }

        public void Clear()
        {
            contacts.Clear();
        }
    }
}
