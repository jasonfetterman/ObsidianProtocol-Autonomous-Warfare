using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.AirWarfare
{
    public sealed class AerialRelayNode
    {
        public string NodeId { get; }

        public float Range { get; private set; }
        public float Reliability { get; private set; }

        public bool Active { get; private set; }

        private readonly HashSet<string> connectedNodes =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        public AerialRelayNode(string nodeId)
        {
            NodeId = nodeId ?? string.Empty;

            Range = 0f;
            Reliability = 1f;
            Active = true;
        }

        public void Configure(
            float range,
            float reliability)
        {
            Range =
                Math.Max(0f, range);

            Reliability =
                Math.Clamp(
                    reliability,
                    0f,
                    1f);
        }

        public void SetActive(bool active)
        {
            Active = active;
        }

        public void Connect(string nodeId)
        {
            if (string.IsNullOrWhiteSpace(nodeId) ||
                nodeId.Equals(
                    NodeId,
                    StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            connectedNodes.Add(nodeId);
        }

        public void Disconnect(string nodeId)
        {
            connectedNodes.Remove(nodeId);
        }

        public bool IsConnected(string nodeId)
        {
            return connectedNodes.Contains(nodeId);
        }

        public IReadOnlyCollection<string> GetConnections()
        {
            return connectedNodes;
        }
    }

    public sealed class AerialRelaySystem
    {
        private readonly Dictionary<string, AerialRelayNode> nodes =
            new Dictionary<string, AerialRelayNode>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterNode(string nodeId)
        {
            if (string.IsNullOrWhiteSpace(nodeId))
            {
                return;
            }

            if (!nodes.ContainsKey(nodeId))
            {
                nodes.Add(
                    nodeId,
                    new AerialRelayNode(nodeId));
            }
        }

        public void ConfigureNode(
            string nodeId,
            float range,
            float reliability)
        {
            RegisterNode(nodeId);

            nodes[nodeId].Configure(
                range,
                reliability);
        }

        public void ConnectNodes(
            string firstNodeId,
            string secondNodeId)
        {
            RegisterNode(firstNodeId);
            RegisterNode(secondNodeId);

            nodes[firstNodeId].Connect(secondNodeId);
            nodes[secondNodeId].Connect(firstNodeId);
        }

        public void DisconnectNodes(
            string firstNodeId,
            string secondNodeId)
        {
            if (nodes.TryGetValue(
                    firstNodeId,
                    out AerialRelayNode first))
            {
                first.Disconnect(secondNodeId);
            }

            if (nodes.TryGetValue(
                    secondNodeId,
                    out AerialRelayNode second))
            {
                second.Disconnect(firstNodeId);
            }
        }

        public bool TryGetNode(
            string nodeId,
            out AerialRelayNode node)
        {
            return nodes.TryGetValue(
                nodeId,
                out node);
        }

        public void RemoveNode(string nodeId)
        {
            if (!nodes.TryGetValue(
                    nodeId,
                    out AerialRelayNode node))
            {
                return;
            }

            foreach (string connectedId in node.GetConnections())
            {
                if (nodes.TryGetValue(
                        connectedId,
                        out AerialRelayNode connected))
                {
                    connected.Disconnect(nodeId);
                }
            }

            nodes.Remove(nodeId);
        }

        public void Clear()
        {
            nodes.Clear();
        }
    }
}
