using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.CommandUnits
{
    public enum NetworkNodeState
    {
        Offline,
        Connecting,
        Online,
        Degraded,
        Lost
    }

    public enum NetworkMessageType
    {
        Command,
        Status,
        Intelligence,
        Target,
        Navigation,
        Logistics,
        Emergency
    }

    public sealed class NetworkNode
    {
        public string NodeId { get; }
        public string OwnerId { get; }

        public NetworkNodeState State { get; private set; }

        public float SignalStrength { get; private set; }
        public float Latency { get; private set; }

        public NetworkNode(
            string nodeId,
            string ownerId)
        {
            NodeId =
                nodeId ?? string.Empty;

            OwnerId =
                ownerId ?? string.Empty;

            State =
                NetworkNodeState.Offline;
        }

        public void UpdateConnection(
            float signalStrength,
            float latency)
        {
            SignalStrength =
                Math.Clamp(
                    signalStrength,
                    0f,
                    1f);

            Latency =
                Math.Max(
                    0f,
                    latency);

            if (SignalStrength <= 0f)
            {
                State =
                    NetworkNodeState.Lost;
            }
            else if (SignalStrength < 0.35f)
            {
                State =
                    NetworkNodeState.Degraded;
            }
            else
            {
                State =
                    NetworkNodeState.Online;
            }
        }

        public void SetState(
            NetworkNodeState state)
        {
            State = state;
        }
    }

    public sealed class NetworkMessage
    {
        public string MessageId { get; }
        public NetworkMessageType Type { get; }

        public string SourceNodeId { get; }
        public string TargetNodeId { get; }

        public string Payload { get; }

        public DateTime CreatedAt { get; }

        public NetworkMessage(
            string messageId,
            NetworkMessageType type,
            string sourceNodeId,
            string targetNodeId,
            string payload)
        {
            MessageId =
                messageId ?? string.Empty;

            Type =
                type;

            SourceNodeId =
                sourceNodeId ?? string.Empty;

            TargetNodeId =
                targetNodeId ?? string.Empty;

            Payload =
                payload ?? string.Empty;

            CreatedAt =
                DateTime.UtcNow;
        }
    }

    public sealed class NetworkCoordinationSystem
    {
        private readonly Dictionary<string, NetworkNode> nodes =
            new Dictionary<string, NetworkNode>(
                StringComparer.OrdinalIgnoreCase);

        private readonly Queue<NetworkMessage> messageQueue =
            new Queue<NetworkMessage>();

        public void RegisterNode(
            string nodeId,
            string ownerId)
        {
            if (string.IsNullOrWhiteSpace(nodeId))
            {
                return;
            }

            nodes[nodeId] =
                new NetworkNode(
                    nodeId,
                    ownerId);
        }

        public void UpdateNodeConnection(
            string nodeId,
            float signalStrength,
            float latency)
        {
            if (nodes.TryGetValue(
                    nodeId,
                    out NetworkNode node))
            {
                node.UpdateConnection(
                    signalStrength,
                    latency);
            }
        }

        public void QueueMessage(
            string messageId,
            NetworkMessageType type,
            string sourceNodeId,
            string targetNodeId,
            string payload)
        {
            if (!nodes.ContainsKey(sourceNodeId) ||
                !nodes.ContainsKey(targetNodeId))
            {
                return;
            }

            messageQueue.Enqueue(
                new NetworkMessage(
                    messageId,
                    type,
                    sourceNodeId,
                    targetNodeId,
                    payload));
        }

        public bool TryDequeueMessage(
            out NetworkMessage message)
        {
            if (messageQueue.Count == 0)
            {
                message = null;
                return false;
            }

            message =
                messageQueue.Dequeue();

            return true;
        }

        public bool TryGetNode(
            string nodeId,
            out NetworkNode node)
        {
            return nodes.TryGetValue(
                nodeId,
                out node);
        }

        public void RemoveNode(
            string nodeId)
        {
            nodes.Remove(nodeId);
        }

        public void Clear()
        {
            nodes.Clear();
            messageQueue.Clear();
        }
    }
}
