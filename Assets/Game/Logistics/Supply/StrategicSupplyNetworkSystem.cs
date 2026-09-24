using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Logistics
{
    public enum StrategicNodeType
    {
        Headquarters,
        SupplyDepot,
        ForwardOperatingLocation,
        FabricationFacility,
        CommandCenter,
        ResourceSite
    }

    public enum StrategicNodeState
    {
        Offline,
        Operational,
        Compromised,
        Destroyed
    }

    public sealed class StrategicSupplyNode
    {
        public string NodeId { get; }

        public string Name { get; }

        public StrategicNodeType Type { get; }

        public StrategicNodeState State { get; private set; }

        public StrategicSupplyNode(
            string nodeId,
            string name,
            StrategicNodeType type)
        {
            NodeId =
                nodeId ?? string.Empty;

            Name =
                name ?? string.Empty;

            Type =
                type;

            State =
                StrategicNodeState.Offline;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(NodeId) &&
            !string.IsNullOrWhiteSpace(Name);

        public bool Operational =>
            State ==
                StrategicNodeState.Operational ||
            State ==
                StrategicNodeState.Compromised;

        public void Activate()
        {
            if (State ==
                StrategicNodeState.Offline)
            {
                State =
                    StrategicNodeState.Operational;
            }
        }

        public void Compromise()
        {
            if (State ==
                    StrategicNodeState.Operational ||
                State ==
                    StrategicNodeState.Offline)
            {
                State =
                    StrategicNodeState.Compromised;
            }
        }

        public void Restore()
        {
            if (State ==
                StrategicNodeState.Compromised)
            {
                State =
                    StrategicNodeState.Operational;
            }
        }

        public void Destroy()
        {
            State =
                StrategicNodeState.Destroyed;
        }
    }

    public sealed class StrategicSupplyLink
    {
        public string LinkId { get; }

        public string OriginNodeId { get; }

        public string DestinationNodeId { get; }

        public float Capacity { get; }

        public bool Active { get; private set; }

        public StrategicSupplyLink(
            string linkId,
            string originNodeId,
            string destinationNodeId,
            float capacity)
        {
            LinkId =
                linkId ?? string.Empty;

            OriginNodeId =
                originNodeId ?? string.Empty;

            DestinationNodeId =
                destinationNodeId ?? string.Empty;

            Capacity =
                Math.Max(
                    0f,
                    capacity);

            Active =
                false;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(LinkId) &&
            !string.IsNullOrWhiteSpace(OriginNodeId) &&
            !string.IsNullOrWhiteSpace(DestinationNodeId) &&
            !string.Equals(
                OriginNodeId,
                DestinationNodeId,
                StringComparison.OrdinalIgnoreCase) &&
            Capacity > 0f;

        public void SetActive(
            bool active)
        {
            Active =
                active;
        }

        public bool Connects(
            string originNodeId,
            string destinationNodeId)
        {
            return
                string.Equals(
                    OriginNodeId,
                    originNodeId,
                    StringComparison.OrdinalIgnoreCase) &&
                string.Equals(
                    DestinationNodeId,
                    destinationNodeId,
                    StringComparison.OrdinalIgnoreCase);
        }
    }

    public sealed class StrategicSupplyNetworkSystem
    {
        private readonly Dictionary<string, StrategicSupplyNode>
            nodes =
                new Dictionary<string, StrategicSupplyNode>(
                    StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, StrategicSupplyLink>
            links =
                new Dictionary<string, StrategicSupplyLink>(
                    StringComparer.OrdinalIgnoreCase);

        public bool RegisterNode(
            StrategicSupplyNode node)
        {
            if (node == null ||
                !node.Valid ||
                nodes.ContainsKey(
                    node.NodeId))
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

            foreach (
                StrategicSupplyLink link
                in links.Values)
            {
                if (string.Equals(
                        link.OriginNodeId,
                        nodeId,
                        StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(
                        link.DestinationNodeId,
                        nodeId,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return nodes.Remove(
                nodeId);
        }

        public bool TryGetNode(
            string nodeId,
            out StrategicSupplyNode node)
        {
            return nodes.TryGetValue(
                nodeId,
                out node);
        }

        public bool RegisterLink(
            StrategicSupplyLink link)
        {
            if (link == null ||
                !link.Valid ||
                links.ContainsKey(
                    link.LinkId))
            {
                return false;
            }

            if (!nodes.ContainsKey(link.OriginNodeId) ||
                !nodes.ContainsKey(link.DestinationNodeId))
            {
                return false;
            }

            links.Add(
                link.LinkId,
                link);

            return true;
        }

        public bool RemoveLink(
            string linkId)
        {
            if (string.IsNullOrWhiteSpace(linkId))
            {
                return false;
            }

            return links.Remove(
                linkId);
        }

        public bool TryGetLink(
            string linkId,
            out StrategicSupplyLink link)
        {
            return links.TryGetValue(
                linkId,
                out link);
        }

        public bool SetLinkActive(
            string linkId,
            bool active)
        {
            if (!links.TryGetValue(
                    linkId,
                    out StrategicSupplyLink link))
            {
                return false;
            }

            link.SetActive(
                active);

            return true;
        }

        public bool HasOperationalConnection(
            string originNodeId,
            string destinationNodeId)
        {
            if (!nodes.TryGetValue(
                    originNodeId,
                    out StrategicSupplyNode origin) ||
                !nodes.TryGetValue(
                    destinationNodeId,
                    out StrategicSupplyNode destination))
            {
                return false;
            }

            if (!origin.Operational ||
                !destination.Operational)
            {
                return false;
            }

            foreach (
                StrategicSupplyLink link
                in links.Values)
            {
                if (!link.Active)
                {
                    continue;
                }

                if (!link.Connects(
                        originNodeId,
                        destinationNodeId))
                {
                    continue;
                }

                return true;
            }

            return false;
        }

        public IReadOnlyCollection<StrategicSupplyNode>
            GetNodes()
        {
            return nodes.Values;
        }

        public IReadOnlyCollection<StrategicSupplyNode>
            GetOperationalNodes()
        {
            List<StrategicSupplyNode> operational =
                new List<StrategicSupplyNode>();

            foreach (
                StrategicSupplyNode node
                in nodes.Values)
            {
                if (node.Operational)
                {
                    operational.Add(
                        node);
                }
            }

            return operational;
        }

        public IReadOnlyCollection<StrategicSupplyLink>
            GetLinks()
        {
            return links.Values;
        }

        public IReadOnlyCollection<StrategicSupplyLink>
            GetActiveLinks()
        {
            List<StrategicSupplyLink> active =
                new List<StrategicSupplyLink>();

            foreach (
                StrategicSupplyLink link
                in links.Values)
            {
                if (link.Active)
                {
                    active.Add(
                        link);
                }
            }

            return active;
        }

        public void Clear()
        {
            nodes.Clear();
            links.Clear();
        }
    }
}
