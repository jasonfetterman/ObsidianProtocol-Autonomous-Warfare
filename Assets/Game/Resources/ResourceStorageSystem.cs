using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Resources
{
    public sealed class ResourceStorage
    {
        private readonly Dictionary<string, int> resourceCapacities =
            new Dictionary<string, int>(
                StringComparer.OrdinalIgnoreCase);

        private readonly ResourceInventory inventory =
            new ResourceInventory();

        public string StorageId { get; }

        public int TotalCapacity { get; private set; }

        public ResourceStorage(
            string storageId,
            int totalCapacity)
        {
            StorageId =
                storageId ?? string.Empty;

            TotalCapacity =
                Math.Max(
                    0,
                    totalCapacity);
        }

        public ResourceInventory Inventory =>
            inventory;

        public int TotalStored
        {
            get
            {
                int total = 0;

                foreach (
                    KeyValuePair<string, int> capacity
                    in resourceCapacities)
                {
                    total +=
                        inventory.GetAmount(
                            capacity.Key);
                }

                return total;
            }
        }

        public int AvailableCapacity =>
            Math.Max(
                0,
                TotalCapacity - TotalStored);

        public void SetTotalCapacity(
            int capacity)
        {
            TotalCapacity =
                Math.Max(
                    0,
                    capacity);
        }

        public bool SetResourceCapacity(
            string resourceId,
            int capacity)
        {
            if (string.IsNullOrWhiteSpace(resourceId) ||
                capacity < 0)
            {
                return false;
            }

            if (inventory.GetAmount(resourceId) > capacity)
            {
                return false;
            }

            resourceCapacities[resourceId] =
                capacity;

            return true;
        }

        public int GetResourceCapacity(
            string resourceId)
        {
            if (string.IsNullOrWhiteSpace(resourceId))
            {
                return 0;
            }

            return resourceCapacities.TryGetValue(
                resourceId,
                out int capacity)
                ? capacity
                : TotalCapacity;
        }

        public int GetAmount(
            string resourceId)
        {
            return inventory.GetAmount(
                resourceId);
        }

        public int GetAvailableResourceCapacity(
            string resourceId)
        {
            return Math.Max(
                0,
                GetResourceCapacity(resourceId) -
                GetAmount(resourceId));
        }

        public bool TryStore(
            string resourceId,
            int amount)
        {
            if (string.IsNullOrWhiteSpace(resourceId) ||
                amount <= 0)
            {
                return false;
            }

            if (amount > AvailableCapacity ||
                amount > GetAvailableResourceCapacity(
                    resourceId))
            {
                return false;
            }

            inventory.Add(
                resourceId,
                amount);

            return true;
        }

        public bool TryWithdraw(
            string resourceId,
            int amount)
        {
            return inventory.TrySpend(
                resourceId,
                amount);
        }

        public bool HasCapacityFor(
            string resourceId,
            int amount)
        {
            if (string.IsNullOrWhiteSpace(resourceId) ||
                amount <= 0)
            {
                return false;
            }

            return amount <= AvailableCapacity &&
                   amount <=
                   GetAvailableResourceCapacity(
                       resourceId);
        }
    }

    public sealed class ResourceStorageSystem
    {
        private readonly Dictionary<string, ResourceStorage> storages =
            new Dictionary<string, ResourceStorage>(
                StringComparer.OrdinalIgnoreCase);

        public bool RegisterStorage(
            ResourceStorage storage)
        {
            if (storage == null ||
                string.IsNullOrWhiteSpace(storage.StorageId) ||
                storages.ContainsKey(storage.StorageId))
            {
                return false;
            }

            storages.Add(
                storage.StorageId,
                storage);

            return true;
        }

        public bool RemoveStorage(
            string storageId)
        {
            if (string.IsNullOrWhiteSpace(storageId))
            {
                return false;
            }

            return storages.Remove(
                storageId);
        }

        public bool TryGetStorage(
            string storageId,
            out ResourceStorage storage)
        {
            return storages.TryGetValue(
                storageId,
                out storage);
        }

        public bool TryStore(
            string storageId,
            string resourceId,
            int amount)
        {
            if (!storages.TryGetValue(
                    storageId,
                    out ResourceStorage storage))
            {
                return false;
            }

            return storage.TryStore(
                resourceId,
                amount);
        }

        public bool TryWithdraw(
            string storageId,
            string resourceId,
            int amount)
        {
            if (!storages.TryGetValue(
                    storageId,
                    out ResourceStorage storage))
            {
                return false;
            }

            return storage.TryWithdraw(
                resourceId,
                amount);
        }

        public IReadOnlyCollection<ResourceStorage>
            GetStorages()
        {
            return storages.Values;
        }
    }
}
