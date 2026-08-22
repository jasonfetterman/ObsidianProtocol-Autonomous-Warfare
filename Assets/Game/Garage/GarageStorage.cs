using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Garage
{
    public sealed class StorageResource
    {
        public string ResourceId { get; }

        public int Capacity { get; private set; }
        public int Amount { get; private set; }

        public int AvailableSpace =>
            Math.Max(
                0,
                Capacity - Amount);

        public StorageResource(
            string resourceId,
            int capacity)
        {
            ResourceId =
                resourceId ?? string.Empty;

            Capacity =
                Math.Max(0, capacity);

            Amount = 0;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(
                ResourceId);

        public void SetCapacity(
            int capacity)
        {
            Capacity =
                Math.Max(0, capacity);

            Amount =
                Math.Min(
                    Amount,
                    Capacity);
        }

        public bool Add(
            int amount)
        {
            if (amount <= 0 ||
                amount > AvailableSpace)
            {
                return false;
            }

            Amount += amount;
            return true;
        }

        public bool Remove(
            int amount)
        {
            if (amount <= 0 ||
                amount > Amount)
            {
                return false;
            }

            Amount -= amount;
            return true;
        }

        public void Clear()
        {
            Amount = 0;
        }
    }

    public sealed class GarageStorage
    {
        private readonly Dictionary<
            string,
            StorageResource> resources =
            new Dictionary<
                string,
                StorageResource>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(
            StorageResource resource)
        {
            if (resource == null ||
                !resource.Valid ||
                resources.ContainsKey(
                    resource.ResourceId))
            {
                return false;
            }

            resources.Add(
                resource.ResourceId,
                resource);

            return true;
        }

        public bool Remove(
            string resourceId)
        {
            if (string.IsNullOrWhiteSpace(
                    resourceId))
            {
                return false;
            }

            return resources.Remove(
                resourceId);
        }

        public bool TryGet(
            string resourceId,
            out StorageResource resource)
        {
            return resources.TryGetValue(
                resourceId,
                out resource);
        }

        public bool Add(
            string resourceId,
            int amount)
        {
            if (!resources.TryGetValue(
                    resourceId,
                    out StorageResource resource))
            {
                return false;
            }

            return resource.Add(amount);
        }

        public bool Remove(
            string resourceId,
            int amount)
        {
            if (!resources.TryGetValue(
                    resourceId,
                    out StorageResource resource))
            {
                return false;
            }

            return resource.Remove(amount);
        }

        public IReadOnlyCollection<
            StorageResource>
            GetResources()
        {
            return resources.Values;
        }

        public void Clear()
        {
            resources.Clear();
        }
    }
}
