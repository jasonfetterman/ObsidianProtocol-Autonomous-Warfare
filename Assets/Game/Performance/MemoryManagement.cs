using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Performance
{
    public sealed class MemoryAllocation
    {
        public string AllocationId { get; }

        public long SizeBytes { get; private set; }

        public bool Active { get; private set; }

        public MemoryAllocation(
            string allocationId,
            long sizeBytes)
        {
            AllocationId =
                allocationId ?? string.Empty;

            SizeBytes =
                Math.Max(
                    0L,
                    sizeBytes);

            Active = true;
        }

        public bool Resize(
            long sizeBytes)
        {
            if (sizeBytes < 0L)
            {
                return false;
            }

            SizeBytes =
                sizeBytes;

            return true;
        }

        public bool Release()
        {
            if (!Active)
            {
                return false;
            }

            Active = false;

            SizeBytes = 0L;

            return true;
        }
    }

    public sealed class MemoryManagement
    {
        private readonly Dictionary<
            string,
            MemoryAllocation> allocations =
            new Dictionary<
                string,
                MemoryAllocation>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int AllocationCount =>
            allocations.Count;

        public long ActiveMemoryBytes
        {
            get
            {
                long total = 0L;

                foreach (MemoryAllocation allocation
                         in allocations.Values)
                {
                    if (allocation.Active)
                    {
                        total +=
                            allocation.SizeBytes;
                    }
                }

                return total;
            }
        }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            allocations.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterAllocation(
            string allocationId,
            long sizeBytes)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(allocationId) ||
                sizeBytes < 0L)
            {
                return false;
            }

            string id =
                allocationId.Trim();

            if (allocations.ContainsKey(id))
            {
                return false;
            }

            allocations.Add(
                id,
                new MemoryAllocation(
                    id,
                    sizeBytes));

            return true;
        }

        public bool ResizeAllocation(
            string allocationId,
            long sizeBytes)
        {
            MemoryAllocation allocation =
                GetAllocation(allocationId);

            return allocation != null &&
                   allocation.Resize(sizeBytes);
        }

        public bool ReleaseAllocation(
            string allocationId)
        {
            MemoryAllocation allocation =
                GetAllocation(allocationId);

            return allocation != null &&
                   allocation.Release();
        }

        public bool RemoveAllocation(
            string allocationId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(allocationId))
            {
                return false;
            }

            return allocations.Remove(
                allocationId.Trim());
        }

        public MemoryAllocation GetAllocation(
            string allocationId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(allocationId))
            {
                return null;
            }

            allocations.TryGetValue(
                allocationId.Trim(),
                out MemoryAllocation allocation);

            return allocation;
        }

        public IReadOnlyCollection<
            MemoryAllocation>
            GetAllocations()
        {
            return allocations.Values;
        }

        public void Reset()
        {
            allocations.Clear();

            Initialized = false;
        }
    }
}
