using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.ExperimentalSystems
{
    public enum CovertNetworkState
    {
        Offline,
        Establishing,
        Active,
        Compromised,
        Lost
    }

    public sealed class CovertNetworkNode
    {
        public string NodeId { get; }
        public string NetworkId { get; }

        public CovertNetworkState State { get; private set; }

        public float Concealment { get; private set; }
        public float Reliability { get; private set; }

        public CovertNetworkNode(
            string nodeId,
            string networkId)
        {
            NodeId =
                nodeId ?? string.Empty;

            NetworkId =
                networkId ?? string.Empty;

            State =
                CovertNetworkState.Offline;
        }

        public void SetState(
            CovertNetworkState state)
        {
            State = state;
        }

        public void SetPerformance(
            float concealment,
            float reliability)
        {
            Concealment =
                Math.Clamp(
                    concealment,
                    0f,
                    1f);

            Reliability =
                Math.Clamp(
                    reliability,
                    0f,
                    1f);
        }

        public bool CanTransmit()
        {
            return State ==
                       CovertNetworkState.Active &&
                   Reliability > 0f &&
                   Concealment > 0f;
        }
    }

    public sealed class CovertNetworkMessage
    {
        public string MessageId { get; }
        public string SourceNodeId { get; }
        public string TargetNodeId { get; }

        public string Payload { get; }

        public CovertNetworkMessage(
            string messageId,
            string sourceNodeId,
            string targetNodeId,
            string payload)
        {
            MessageId =
                messageId ?? string.Empty;

            SourceNodeId =
                sourceNodeId ?? string.Empty;

            TargetNodeId =
                targetNodeId ?? string.Empty;

            Payload =
                payload ?? string.Empty;
        }
    }

    public sealed class CovertNetworkingSystem
    {
        private readonly Dictionary<string, CovertNetworkNode> nodes =
            new Dictionary<string, CovertNetworkNode>(
                StringComparer.OrdinalIgnoreCase);

        private readonly Queue<CovertNetworkMessage> messages =
            new Queue<CovertNetworkMessage>();

        public void RegisterNode(
            string nodeId,
            string networkId)
        {
            if (string.IsNullOrWhiteSpace(nodeId))
            {
                return;
            }

            nodes[nodeId] =
                new CovertNetworkNode(
                    nodeId,
                    networkId);
        }

        public void SetState(
            string nodeId,
            CovertNetworkState state)
        {
            if (nodes.TryGetValue(
                    nodeId,
                    out CovertNetworkNode node))
            {
                node.SetState(state);
            }
        }

        public void SetPerformance(
            string nodeId,
            float concealment,
            float reliability)
        {
            if (nodes.TryGetValue(
                    nodeId,
                    out CovertNetworkNode node))
            {
                node.SetPerformance(
                    concealment,
                    reliability);
            }
        }

        public bool Send(
            string messageId,
            string sourceNodeId,
            string targetNodeId,
            string payload)
        {
            if (!CanTransmit(sourceNodeId) ||
                !CanTransmit(targetNodeId))
            {
                return false;
            }

            if (!nodes.TryGetValue(
                    sourceNodeId,
                    out CovertNetworkNode source) ||
                !nodes.TryGetValue(
                    targetNodeId,
                    out CovertNetworkNode target))
            {
                return false;
            }

            if (!string.Equals(
                    source.NetworkId,
                    target.NetworkId,
                    StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            messages.Enqueue(
                new CovertNetworkMessage(
                    messageId,
                    sourceNodeId,
                    targetNodeId,
                    payload));

            return true;
        }

        public bool CanTransmit(
            string nodeId)
        {
            return nodes.TryGetValue(
                       nodeId,
                       out CovertNetworkNode node) &&
                   node.CanTransmit();
        }

        public bool TryReceive(
            out CovertNetworkMessage message)
        {
            if (messages.Count == 0)
            {
                message = null;
                return false;
            }

            message =
                messages.Dequeue();

            return true;
        }

        public bool TryGetNode(
            string nodeId,
            out CovertNetworkNode node)
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
            messages.Clear();
        }
    }
}
