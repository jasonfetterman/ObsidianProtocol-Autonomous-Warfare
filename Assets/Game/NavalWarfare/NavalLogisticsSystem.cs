using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.NavalWarfare
{
    public enum NavalLogisticsResource
    {
        Fuel,
        Ammunition,
        Energy,
        SpareParts,
        FabricationMaterials
    }

    public sealed class NavalLogisticsInventory
    {
        private readonly Dictionary<NavalLogisticsResource, float> resources =
            new Dictionary<NavalLogisticsResource, float>();

        public NavalLogisticsInventory()
        {
            foreach (NavalLogisticsResource resource in
                     Enum.GetValues(typeof(NavalLogisticsResource)))
            {
                resources[resource] = 0f;
            }
        }

        public void Add(
            NavalLogisticsResource resource,
            float amount)
        {
            if (amount <= 0f)
            {
                return;
            }

            resources[resource] += amount;
        }

        public bool TryConsume(
            NavalLogisticsResource resource,
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
            NavalLogisticsResource resource)
        {
            return resources.TryGetValue(
                       resource,
                       out float amount)
                ? amount
                : 0f;
        }
    }

    public sealed class NavalLogisticsSystem
    {
        private readonly Dictionary<string, NavalLogisticsInventory> inventories =
            new Dictionary<string, NavalLogisticsInventory>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterUnit(
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
                    new NavalLogisticsInventory());
            }
        }

        public void AddResource(
            string unitId,
            NavalLogisticsResource resource,
            float amount)
        {
            RegisterUnit(unitId);

            inventories[unitId].Add(
                resource,
                amount);
        }

        public bool TryConsumeResource(
            string unitId,
            NavalLogisticsResource resource,
            float amount)
        {
            return inventories.TryGetValue(
                       unitId,
                       out NavalLogisticsInventory inventory) &&
                   inventory.TryConsume(
                       resource,
                       amount);
        }

        public float GetResource(
            string unitId,
            NavalLogisticsResource resource)
        {
            return inventories.TryGetValue(
                       unitId,
                       out NavalLogisticsInventory inventory)
                ? inventory.Get(resource)
                : 0f;
        }

        public bool TryGetInventory(
            string unitId,
            out NavalLogisticsInventory inventory)
        {
            return inventories.TryGetValue(
                unitId,
                out inventory);
        }

        public void RemoveUnit(
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
