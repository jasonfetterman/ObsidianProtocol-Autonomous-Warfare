using System.Collections.Generic;

namespace ObsidianProtocol.Game.Intelligence
{
    public sealed class RelayNode
    {
        public int UnitId;
        public float Range;
        public bool Active;

        public RelayNode(
            int unitId,
            float range)
        {
            UnitId = unitId;
            Range = System.Math.Max(0f, range);
            Active = false;
        }
    }

    public sealed class NetworkRelaySystem
    {
        private readonly Dictionary<int, RelayNode> relays =
            new Dictionary<int, RelayNode>();

        private readonly Dictionary<int, HashSet<int>> connections =
            new Dictionary<int, HashSet<int>>();

        public void RegisterRelay(
            int unitId,
            float range)
        {
            if (unitId < 0)
            {
                return;
            }

            relays[unitId] =
                new RelayNode(
                    unitId,
                    range);
        }

        public void ActivateRelay(int unitId)
        {
            if (relays.TryGetValue(
                    unitId,
                    out RelayNode relay))
            {
                relay.Active = true;
            }
        }

        public void DeactivateRelay(int unitId)
        {
            if (relays.TryGetValue(
                    unitId,
                    out RelayNode relay))
            {
                relay.Active = false;
            }

            connections.Remove(unitId);

            foreach (HashSet<int> linkedNodes in connections.Values)
            {
                linkedNodes.Remove(unitId);
            }
        }

        public void Connect(
            int relayA,
            int relayB)
        {
            if (!relays.ContainsKey(relayA) ||
                !relays.ContainsKey(relayB) ||
                relayA == relayB)
            {
                return;
            }

            if (!connections.TryGetValue(
                    relayA,
                    out HashSet<int> linksA))
            {
                linksA = new HashSet<int>();
                connections.Add(relayA, linksA);
            }

            if (!connections.TryGetValue(
                    relayB,
                    out HashSet<int> linksB))
            {
                linksB = new HashSet<int>();
                connections.Add(relayB, linksB);
            }

            linksA.Add(relayB);
            linksB.Add(relayA);
        }

        public bool IsConnected(
            int relayA,
            int relayB)
        {
            return connections.TryGetValue(
                       relayA,
                       out HashSet<int> links) &&
                   links.Contains(relayB);
        }

        public bool IsActive(int unitId)
        {
            return relays.TryGetValue(
                       unitId,
                       out RelayNode relay) &&
                   relay.Active;
        }

        public void RemoveRelay(int unitId)
        {
            DeactivateRelay(unitId);
            relays.Remove(unitId);
        }

        public void Clear()
        {
            relays.Clear();
            connections.Clear();
        }
    }
}
