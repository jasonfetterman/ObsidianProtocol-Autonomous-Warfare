using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.AirWarfare
{
    public enum AerialPayloadType
    {
        Sensor,
        Camera,
        Thermal,
        Lidar,
        Radar,
        Relay,
        Reconnaissance,
        Support,
        Utility,
        Weapon
    }

    public sealed class AerialPayload
    {
        public string PayloadId { get; }
        public AerialPayloadType Type { get; }

        public float Capacity { get; private set; }
        public float RemainingCapacity { get; private set; }

        public bool Active { get; private set; }

        public AerialPayload(
            string payloadId,
            AerialPayloadType type,
            float capacity)
        {
            PayloadId = payloadId ?? string.Empty;
            Type = type;

            Capacity =
                Math.Max(0f, capacity);

            RemainingCapacity =
                Capacity;

            Active = true;
        }

        public bool Consume(float amount)
        {
            if (!Active ||
                amount < 0f ||
                amount > RemainingCapacity)
            {
                return false;
            }

            RemainingCapacity -= amount;

            if (RemainingCapacity <= 0f)
            {
                RemainingCapacity = 0f;
                Active = false;
            }

            return true;
        }

        public void Replenish()
        {
            RemainingCapacity = Capacity;
            Active = true;
        }

        public void SetActive(bool active)
        {
            Active = active;
        }
    }

    public sealed class AerialPayloadSystem
    {
        private readonly Dictionary<string, AerialPayload> payloads =
            new Dictionary<string, AerialPayload>(
                StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, List<string>> unitPayloads =
            new Dictionary<string, List<string>>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterPayload(
            string payloadId,
            AerialPayloadType type,
            float capacity)
        {
            if (string.IsNullOrWhiteSpace(payloadId))
            {
                return;
            }

            payloads[payloadId] =
                new AerialPayload(
                    payloadId,
                    type,
                    capacity);
        }

        public void AttachPayload(
            string unitId,
            string payloadId)
        {
            if (string.IsNullOrWhiteSpace(unitId) ||
                !payloads.ContainsKey(payloadId))
            {
                return;
            }

            if (!unitPayloads.TryGetValue(
                    unitId,
                    out List<string> attached))
            {
                attached = new List<string>();
                unitPayloads.Add(unitId, attached);
            }

            if (!attached.Contains(payloadId))
            {
                attached.Add(payloadId);
            }
        }

        public bool ConsumePayload(
            string payloadId,
            float amount)
        {
            return payloads.TryGetValue(
                       payloadId,
                       out AerialPayload payload) &&
                   payload.Consume(amount);
        }

        public void ReplenishPayload(
            string payloadId)
        {
            if (payloads.TryGetValue(
                    payloadId,
                    out AerialPayload payload))
            {
                payload.Replenish();
            }
        }

        public bool TryGetPayload(
            string payloadId,
            out AerialPayload payload)
        {
            return payloads.TryGetValue(
                payloadId,
                out payload);
        }

        public void DetachPayload(
            string unitId,
            string payloadId)
        {
            if (unitPayloads.TryGetValue(
                    unitId,
                    out List<string> attached))
            {
                attached.Remove(payloadId);

                if (attached.Count == 0)
                {
                    unitPayloads.Remove(unitId);
                }
            }
        }

        public void RemovePayload(
            string payloadId)
        {
            payloads.Remove(payloadId);

            foreach (List<string> attached in unitPayloads.Values)
            {
                attached.Remove(payloadId);
            }
        }

        public void Clear()
        {
            payloads.Clear();
            unitPayloads.Clear();
        }
    }
}
