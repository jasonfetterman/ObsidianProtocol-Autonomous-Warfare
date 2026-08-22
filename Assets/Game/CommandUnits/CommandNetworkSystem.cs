using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.CommandUnits
{
    public enum CommandNetworkState
    {
        Offline,
        Initializing,
        Operational,
        Degraded,
        Compromised,
        Destroyed
    }

    public enum CommandNetworkPriority
    {
        Low,
        Normal,
        High,
        Critical
    }

    public sealed class CommandNetworkNode
    {
        public string NodeId { get; }
        public string CommandUnitId { get; }

        public CommandNetworkState State { get; private set; }

        public float Reliability { get; private set; }

        public CommandNetworkNode(
            string nodeId,
            string commandUnitId)
        {
            NodeId =
                nodeId ?? string.Empty;

            CommandUnitId =
                commandUnitId ?? string.Empty;

            State =
                CommandNetworkState.Offline;
        }

        public void SetState(
            CommandNetworkState state)
        {
            State = state;
        }

        public void SetReliability(
            float reliability)
        {
            Reliability =
                Math.Clamp(
                    reliability,
                    0f,
                    1f);
        }
    }

    public sealed class CommandNetworkMessage
    {
        public string MessageId { get; }
        public string SourceNodeId { get; }
        public string TargetNodeId { get; }

        public CommandNetworkPriority Priority { get; }

        public string Payload { get; }

        public CommandNetworkMessage(
            string messageId,
            string sourceNodeId,
            string targetNodeId,
            CommandNetworkPriority priority,
            string payload)
        {
            MessageId =
                messageId ?? string.Empty;

            SourceNodeId =
                sourceNodeId ?? string.Empty;

            TargetNodeId =
                targetNodeId ?? string.Empty;

            Priority =
                priority;

            Payload =
                payload ?? string.Empty;
        }
    }

    public sealed class CommandNetworkSystem
    {
        private readonly Dictionary<string, CommandNetworkNode> nodes =
            new Dictionary<string, CommandNetworkNode>(
                StringComparer.OrdinalIgnoreCase);

        private readonly Queue<CommandNetworkMessage> messages =
            new Queue<CommandNetworkMessage>();

        public void RegisterNode(
            string nodeId,
            string commandUnitId)
        {
            if (string.IsNullOrWhiteSpace(nodeId))
            {
                return;
            }

            nodes[nodeId] =
                new CommandNetworkNode(
                    nodeId,
                    commandUnitId);
        }

        public void SetNodeState(
            string nodeId,
            CommandNetworkState state)
        {
            if (nodes.TryGetValue(
                    nodeId,
                    out CommandNetworkNode node))
            {
                node.SetState(state);
            }
        }

        public void SetNodeReliability(
            string nodeId,
            float reliability)
        {
            if (nodes.TryGetValue(
                    nodeId,
                    out CommandNetworkNode node))
            {
                node.SetReliability(
                    reliability);
            }
        }

        public bool CanTransmit(
            string nodeId)
        {
            return nodes.TryGetValue(
                       nodeId,
                       out CommandNetworkNode node) &&
                   node.State ==
                   CommandNetworkState.Operational &&
                   node.Reliability > 0f;
        }

        public bool Send(
            string messageId,
            string sourceNodeId,
            string targetNodeId,
            CommandNetworkPriority priority,
            string payload)
        {
            if (!CanTransmit(sourceNodeId) ||
                !CanTransmit(targetNodeId))
            {
                return false;
            }

            messages.Enqueue(
                new CommandNetworkMessage(
                    messageId,
                    sourceNodeId,
                    targetNodeId,
                    priority,
                    payload));

            return true;
        }

        public bool TryReceive(
            out CommandNetworkMessage message)
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
            out CommandNetworkNode node)
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
