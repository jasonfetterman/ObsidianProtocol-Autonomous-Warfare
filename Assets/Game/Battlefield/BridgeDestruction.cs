using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Battlefield
{
    public enum BridgeState
    {
        Operational,
        Damaged,
        Critical,
        Destroyed
    }

    public sealed class BridgeStructure
    {
        public string BridgeId { get; }

        public float MaximumIntegrity { get; }

        public float Integrity { get; private set; }

        public BridgeState State { get; private set; }

        public bool Traversable =>
            State == BridgeState.Operational ||
            State == BridgeState.Damaged;

        public BridgeStructure(
            string bridgeId,
            float maximumIntegrity)
        {
            BridgeId =
                bridgeId ?? string.Empty;

            MaximumIntegrity =
                Math.Max(0f, maximumIntegrity);

            Integrity =
                MaximumIntegrity;

            State =
                MaximumIntegrity > 0f
                    ? BridgeState.Operational
                    : BridgeState.Destroyed;
        }

        public bool ApplyDamage(
            float damage)
        {
            if (damage < 0f ||
                State == BridgeState.Destroyed)
            {
                return false;
            }

            Integrity =
                Math.Max(
                    0f,
                    Integrity - damage);

            UpdateState();

            return true;
        }

        public bool Repair(
            float amount)
        {
            if (amount < 0f ||
                MaximumIntegrity <= 0f ||
                State == BridgeState.Destroyed)
            {
                return false;
            }

            Integrity =
                Math.Min(
                    MaximumIntegrity,
                    Integrity + amount);

            UpdateState();

            return true;
        }

        private void UpdateState()
        {
            if (Integrity <= 0f)
            {
                State =
                    BridgeState.Destroyed;
                return;
            }

            float ratio =
                Integrity / MaximumIntegrity;

            if (ratio <= 0.25f)
            {
                State =
                    BridgeState.Critical;
            }
            else if (ratio < 1f)
            {
                State =
                    BridgeState.Damaged;
            }
            else
            {
                State =
                    BridgeState.Operational;
            }
        }
    }

    public sealed class BridgeDestruction
    {
        private readonly Dictionary<
            string,
            BridgeStructure> bridges =
            new Dictionary<
                string,
                BridgeStructure>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int BridgeCount =>
            bridges.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            bridges.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterBridge(
            string bridgeId,
            float maximumIntegrity)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(bridgeId) ||
                maximumIntegrity <= 0f)
            {
                return false;
            }

            string id =
                bridgeId.Trim();

            if (bridges.ContainsKey(id))
            {
                return false;
            }

            bridges.Add(
                id,
                new BridgeStructure(
                    id,
                    maximumIntegrity));

            return true;
        }

        public bool ApplyDamage(
            string bridgeId,
            float damage)
        {
            BridgeStructure bridge =
                GetBridge(bridgeId);

            return bridge != null &&
                   bridge.ApplyDamage(damage);
        }

        public bool RepairBridge(
            string bridgeId,
            float amount)
        {
            BridgeStructure bridge =
                GetBridge(bridgeId);

            return bridge != null &&
                   bridge.Repair(amount);
        }

        public bool IsTraversable(
            string bridgeId)
        {
            BridgeStructure bridge =
                GetBridge(bridgeId);

            return bridge != null &&
                   bridge.Traversable;
        }

        public BridgeStructure GetBridge(
            string bridgeId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(bridgeId))
            {
                return null;
            }

            bridges.TryGetValue(
                bridgeId.Trim(),
                out BridgeStructure bridge);

            return bridge;
        }

        public IReadOnlyCollection<BridgeStructure>
            GetBridges()
        {
            return bridges.Values;
        }

        public void Reset()
        {
            bridges.Clear();

            Initialized = false;
        }
    }
}
