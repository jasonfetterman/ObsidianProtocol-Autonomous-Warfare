using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Resources
{
    public sealed class EnergyInfrastructureNode
    {
        public string NodeId { get; }
        public float Generation { get; private set; }
        public float Consumption { get; private set; }
        public float Capacity { get; private set; }
        public float StoredEnergy { get; private set; }
        public bool Online { get; private set; }

        public float AvailableEnergy =>
            Math.Max(0f, Generation - Consumption);

        public EnergyInfrastructureNode(
            string nodeId)
        {
            NodeId =
                nodeId ?? string.Empty;

            Online = false;
        }

        public void Configure(
            float generation,
            float consumption,
            float capacity)
        {
            Generation =
                Math.Max(0f, generation);

            Consumption =
                Math.Max(0f, consumption);

            Capacity =
                Math.Max(0f, capacity);

            StoredEnergy =
                Math.Min(
                    StoredEnergy,
                    Capacity);
        }

        public void SetOnline(
            bool online)
        {
            Online = online;
        }

        public void AddStoredEnergy(
            float amount)
        {
            if (!Online || amount <= 0f)
            {
                return;
            }

            StoredEnergy =
                Math.Min(
                    Capacity,
                    StoredEnergy + amount);
        }

        public bool TryConsumeStoredEnergy(
            float amount)
        {
            if (!Online || amount <= 0f)
            {
                return false;
            }

            if (StoredEnergy < amount)
            {
                return false;
            }

            StoredEnergy -= amount;

            return true;
        }

        public void Update(
            float deltaTime)
        {
            if (!Online ||
                deltaTime <= 0f)
            {
                return;
            }

            float netEnergy =
                (Generation - Consumption) *
                deltaTime;

            StoredEnergy =
                Math.Clamp(
                    StoredEnergy + netEnergy,
                    0f,
                    Capacity);
        }
    }

    public sealed class EnergyInfrastructureNetwork
    {
        private readonly Dictionary<string, EnergyInfrastructureNode> nodes =
            new Dictionary<string, EnergyInfrastructureNode>(
                StringComparer.OrdinalIgnoreCase);

        public string NetworkId { get; }

        public bool Online { get; private set; }

        public EnergyInfrastructureNetwork(
            string networkId)
        {
            NetworkId =
                networkId ?? string.Empty;

            Online = false;
        }

        public bool AddNode(
            EnergyInfrastructureNode node)
        {
            if (node == null ||
                string.IsNullOrWhiteSpace(node.NodeId) ||
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

        public void SetOnline(
            bool online)
        {
            Online = online;

            foreach (
                EnergyInfrastructureNode node
                in nodes.Values)
            {
                node.SetOnline(online);
            }
        }

        public float TotalGeneration
        {
            get
            {
                float total = 0f;

                foreach (
                    EnergyInfrastructureNode node
                    in nodes.Values)
                {
                    if (node.Online)
                    {
                        total += node.Generation;
                    }
                }

                return total;
            }
        }

        public float TotalConsumption
        {
            get
            {
                float total = 0f;

                foreach (
                    EnergyInfrastructureNode node
                    in nodes.Values)
                {
                    if (node.Online)
                    {
                        total += node.Consumption;
                    }
                }

                return total;
            }
        }

        public float TotalCapacity
        {
            get
            {
                float total = 0f;

                foreach (
                    EnergyInfrastructureNode node
                    in nodes.Values)
                {
                    total += node.Capacity;
                }

                return total;
            }
        }

        public float StoredEnergy
        {
            get
            {
                float total = 0f;

                foreach (
                    EnergyInfrastructureNode node
                    in nodes.Values)
                {
                    total += node.StoredEnergy;
                }

                return total;
            }
        }

        public void Update(
            float deltaTime)
        {
            if (!Online)
            {
                return;
            }

            foreach (
                EnergyInfrastructureNode node
                in nodes.Values)
            {
                node.Update(deltaTime);
            }
        }

        public IReadOnlyCollection<EnergyInfrastructureNode>
            GetNodes()
        {
            return nodes.Values;
        }
    }

    public sealed class EnergyInfrastructureSystem
    {
        private readonly Dictionary<string, EnergyInfrastructureNetwork> networks =
            new Dictionary<string, EnergyInfrastructureNetwork>(
                StringComparer.OrdinalIgnoreCase);

        public bool RegisterNetwork(
            string networkId)
        {
            if (string.IsNullOrWhiteSpace(networkId) ||
                networks.ContainsKey(networkId))
            {
                return false;
            }

            networks.Add(
                networkId,
                new EnergyInfrastructureNetwork(
                    networkId));

            return true;
        }

        public bool RemoveNetwork(
            string networkId)
        {
            if (string.IsNullOrWhiteSpace(networkId))
            {
                return false;
            }

            return networks.Remove(networkId);
        }

        public bool TryGetNetwork(
            string networkId,
            out EnergyInfrastructureNetwork network)
        {
            return networks.TryGetValue(
                networkId,
                out network);
        }

        public bool AddNode(
            string networkId,
            EnergyInfrastructureNode node)
        {
            if (!networks.TryGetValue(
                    networkId,
                    out EnergyInfrastructureNetwork network))
            {
                return false;
            }

            return network.AddNode(node);
        }

        public void Update(
            float deltaTime)
        {
            foreach (
                EnergyInfrastructureNetwork network
                in networks.Values)
            {
                network.Update(deltaTime);
            }
        }

        public IReadOnlyCollection<EnergyInfrastructureNetwork>
            GetNetworks()
        {
            return networks.Values;
        }
    }
}
