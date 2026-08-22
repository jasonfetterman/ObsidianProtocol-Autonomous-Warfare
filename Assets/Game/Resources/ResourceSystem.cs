using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Resources
{
    public enum ResourceType
    {
        Meat,
        Wood,
        Coal,
        Iron,
        Alloy,
        Electronics,
        Fuel,
        Energy
    }

    public sealed class ResourceSystem
    {
        private readonly Dictionary<string, ResourceDefinition> definitions =
            new Dictionary<string, ResourceDefinition>(
                StringComparer.OrdinalIgnoreCase);

        private readonly ResourceInventory inventory =
            new ResourceInventory();

        public ResourceInventory Inventory =>
            inventory;

        public bool RegisterDefinition(
            ResourceDefinition definition)
        {
            if (definition == null ||
                string.IsNullOrWhiteSpace(
                    definition.ResourceId))
            {
                return false;
            }

            if (definitions.ContainsKey(
                    definition.ResourceId))
            {
                return false;
            }

            definitions.Add(
                definition.ResourceId,
                definition);

            return true;
        }

        public bool UnregisterDefinition(
            string resourceId)
        {
            if (string.IsNullOrWhiteSpace(resourceId))
            {
                return false;
            }

            return definitions.Remove(
                resourceId);
        }

        public bool HasDefinition(
            string resourceId)
        {
            return !string.IsNullOrWhiteSpace(resourceId) &&
                   definitions.ContainsKey(resourceId);
        }

        public bool TryGetDefinition(
            string resourceId,
            out ResourceDefinition definition)
        {
            return definitions.TryGetValue(
                resourceId,
                out definition);
        }

        public int GetAmount(
            string resourceId)
        {
            return inventory.GetAmount(
                resourceId);
        }

        public bool Add(
            string resourceId,
            int amount)
        {
            if (!HasDefinition(resourceId) ||
                amount <= 0)
            {
                return false;
            }

            inventory.Add(
                resourceId,
                amount);

            return true;
        }

        public bool TrySpend(
            string resourceId,
            int amount)
        {
            if (!HasDefinition(resourceId))
            {
                return false;
            }

            return inventory.TrySpend(
                resourceId,
                amount);
        }

        public bool TryTransferTo(
            ResourceSystem target,
            string resourceId,
            int amount)
        {
            if (target == null ||
                ReferenceEquals(
                    this,
                    target) ||
                amount <= 0 ||
                !HasDefinition(resourceId) ||
                !target.HasDefinition(resourceId))
            {
                return false;
            }

            if (GetAmount(resourceId) < amount)
            {
                return false;
            }

            if (!inventory.TrySpend(
                    resourceId,
                    amount))
            {
                return false;
            }

            if (!target.Add(
                    resourceId,
                    amount))
            {
                inventory.Add(
                    resourceId,
                    amount);

                return false;
            }

            return true;
        }

        public IReadOnlyCollection<ResourceDefinition>
            GetDefinitions()
        {
            return definitions.Values;
        }

        public void ClearDefinitions()
        {
            definitions.Clear();
        }
    }
}
