using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.GroundWarfare
{
    public enum GroundLogisticsResource
    {
        Fuel,
        Ammunition,
        Energy,
        SpareParts,
        FabricationMaterials
    }

    public sealed class GroundLogisticsInventory
    {
        private readonly Dictionary<GroundLogisticsResource, float> resources =
            new Dictionary<GroundLogisticsResource, float>();

        public GroundLogisticsInventory()
        {
            foreach (GroundLogisticsResource resource in
                     Enum.GetValues(typeof(GroundLogisticsResource)))
            {
                resources[resource] = 0f;
            }
        }

        public void Add(
            GroundLogisticsResource resource,
            float amount)
        {
            if (amount <= 0f)
            {
                return;
            }

            resources[resource] += amount;
        }

        public bool TryConsume(
            GroundLogisticsResource resource,
            float amount)
        {
            if (amount <= 0f)
            {
                return true;
            }

            if (Get(resource) < amount)
            {
                return false;
            }

            resources[resource] -= amount;
            return true;
        }

        public float Get(
            GroundLogisticsResource resource)
        {
            return resources.TryGetValue(
                       resource,
                       out float amount)
                ? amount
                : 0f;
        }

        public void Clear()
        {
            resources.Clear();

            foreach (GroundLogisticsResource resource in
                     Enum.GetValues(typeof(GroundLogisticsResource)))
            {
                resources[resource] = 0f;
            }
        }
    }

    public sealed class GroundLogisticsSystem
    {
        private readonly Dictionary<string, GroundLogisticsInventory> inventories =
            new Dictionary<string, GroundLogisticsInventory>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterVehicle(
            string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!inventories.ContainsKey(unitId))
            {
                inventories.Add(
                    unitId,
                    new GroundLogisticsInventory());
            }
        }

        public void AddResource(
            string unitId,
            GroundLogisticsResource resource,
            float amount)
        {
            RegisterVehicle(unitId);

            inventories[unitId].Add(
                resource,
                amount);
        }

        public bool TryConsumeResource(
            string unitId,
            GroundLogisticsResource resource,
            float amount)
        {
            return inventories.TryGetValue(
                       unitId,
                       out GroundLogisticsInventory inventory) &&
                   inventory.TryConsume(
                       resource,
                       amount);
        }

        public float GetResource(
            string unitId,
            GroundLogisticsResource resource)
        {
            return inventories.TryGetValue(
                       unitId,
                       out GroundLogisticsInventory inventory)
                ? inventory.Get(resource)
                : 0f;
        }

        public bool TryGetInventory(
            string unitId,
            out GroundLogisticsInventory inventory)
        {
            return inventories.TryGetValue(
                unitId,
                out inventory);
        }

        public void RemoveVehicle(
            string unitId)
        {
            inventories.Remove(unitId);
        }

        public void Clear()
        {
            inventories.Clear();
        }
    }
}
