using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Resources
{
    public enum ResourceNodeState
    {
        Inactive,
        Active,
        Depleted
    }

    public sealed class ResourceNode
    {
        public string NodeId { get; }

        public string ResourceId { get; }

        public int MaximumAmount { get; }

        public int RemainingAmount { get; private set; }

        public ResourceNodeState State { get; private set; }

        public bool Available =>
            State == ResourceNodeState.Active &&
            RemainingAmount > 0;

        public ResourceNode(
            string nodeId,
            string resourceId,
            int maximumAmount)
        {
            NodeId =
                nodeId ?? string.Empty;

            ResourceId =
                resourceId ?? string.Empty;

            MaximumAmount =
                Math.Max(
                    0,
                    maximumAmount);

            RemainingAmount =
                MaximumAmount;

            State =
                MaximumAmount > 0
                    ? ResourceNodeState.Active
                    : ResourceNodeState.Depleted;
        }

        public void SetActive(
            bool active)
        {
            if (RemainingAmount <= 0)
            {
                State =
                    ResourceNodeState.Depleted;

                return;
            }

            State =
                active
                    ? ResourceNodeState.Active
                    : ResourceNodeState.Inactive;
        }

        public int GetRemainingAmount()
        {
            return RemainingAmount;
        }
        public bool TryExtract(
            int amount)
        {
            if (!Available ||
                amount <= 0 ||
                amount > RemainingAmount)
            {
                return false;
            }

            RemainingAmount -= amount;

            if (RemainingAmount <= 0)
            {
                RemainingAmount = 0;
                State = ResourceNodeState.Depleted;
            }

            return true;
        }


        public float GetRemainingPercent()
        {
            if (MaximumAmount <= 0)
            {
                return 0f;
            }

            return (float)RemainingAmount /
                   MaximumAmount;
        }

        public bool Restore(
            int amount)
        {
            if (amount <= 0 ||
                RemainingAmount >= MaximumAmount)
            {
                return false;
            }

            RemainingAmount =
                Math.Min(
                    MaximumAmount,
                    RemainingAmount + amount);

            if (RemainingAmount > 0 &&
                State == ResourceNodeState.Depleted)
            {
                State =
                    ResourceNodeState.Active;
            }

            return true;
        }

        public void MarkDepleted()
        {
            RemainingAmount = 0;

            State =
                ResourceNodeState.Depleted;
        }
    }

    public sealed class ResourceNodeSystem
    {
        private readonly Dictionary<string, ResourceNode> nodes =
            new Dictionary<string, ResourceNode>(
                StringComparer.OrdinalIgnoreCase);

        public bool RegisterNode(
            ResourceNode node)
        {
            if (node == null ||
                string.IsNullOrWhiteSpace(node.NodeId) ||
                string.IsNullOrWhiteSpace(node.ResourceId) ||
                nodes.ContainsKey(node.NodeId))
            {
                return false;
            }

            nodes.Add(
                node.NodeId,
                node);

            return true;
        }

        public bool RemoveNode(
            string nodeId)
        {
            if (string.IsNullOrWhiteSpace(nodeId))
            {
                return false;
            }

            return nodes.Remove(nodeId);
        }

        public bool TryGetNode(
            string nodeId,
            out ResourceNode node)
        {
            return nodes.TryGetValue(
                nodeId,
                out node);
        }

        public IReadOnlyCollection<ResourceNode>
            GetNodes()
        {
            return nodes.Values;
        }

        public IReadOnlyCollection<ResourceNode>
            GetNodesForResource(
                string resourceId)
        {
            List<ResourceNode> matches =
                new List<ResourceNode>();

            if (string.IsNullOrWhiteSpace(resourceId))
            {
                return matches;
            }

            foreach (
                ResourceNode node
                in nodes.Values)
            {
                if (string.Equals(
                        node.ResourceId,
                        resourceId,
                        StringComparison.OrdinalIgnoreCase))
                {
                    matches.Add(node);
                }
            }

            return matches;
        }

        public int GetTotalRemaining(
            string resourceId)
        {
            int total = 0;

            foreach (
                ResourceNode node
                in nodes.Values)
            {
                if (string.Equals(
                        node.ResourceId,
                        resourceId,
                        StringComparison.OrdinalIgnoreCase))
                {
                    total += node.RemainingAmount;
                }
            }

            return total;
        }
    }
}



