using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Performance
{
    public sealed class ProcessingBatch<T>
    {
        private readonly List<T> items =
            new List<T>();

        public string BatchId { get; }

        public int Capacity { get; private set; }

        public int Count =>
            items.Count;

        public bool IsFull =>
            Count >= Capacity;

        public ProcessingBatch(
            string batchId,
            int capacity)
        {
            BatchId =
                batchId ?? string.Empty;

            Capacity =
                Math.Max(1, capacity);
        }

        public bool Add(
            T item)
        {
            if (IsFull)
            {
                return false;
            }

            items.Add(item);

            return true;
        }

        public bool Remove(
            T item)
        {
            return items.Remove(item);
        }

        public void Clear()
        {
            items.Clear();
        }

        public IReadOnlyList<T>
            GetItems()
        {
            return items;
        }
    }

    public sealed class BatchedProcessing
    {
        private readonly Dictionary<
            string,
            int> batchCapacities =
            new Dictionary<
                string,
                int>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int BatchTypeCount =>
            batchCapacities.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            batchCapacities.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterBatchType(
            string batchId,
            int capacity)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(batchId) ||
                capacity <= 0)
            {
                return false;
            }

            string id =
                batchId.Trim();

            if (batchCapacities.ContainsKey(id))
            {
                return false;
            }

            batchCapacities.Add(
                id,
                capacity);

            return true;
        }

        public bool SetBatchCapacity(
            string batchId,
            int capacity)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(batchId) ||
                capacity <= 0)
            {
                return false;
            }

            string id =
                batchId.Trim();

            if (!batchCapacities.ContainsKey(id))
            {
                return false;
            }

            batchCapacities[id] =
                capacity;

            return true;
        }

        public ProcessingBatch<T>
            CreateBatch<T>(
                string batchId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(batchId))
            {
                return null;
            }

            string id =
                batchId.Trim();

            if (!batchCapacities.TryGetValue(
                    id,
                    out int capacity))
            {
                return null;
            }

            return new ProcessingBatch<T>(
                id,
                capacity);
        }

        public int GetBatchCapacity(
            string batchId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(batchId))
            {
                return 0;
            }

            batchCapacities.TryGetValue(
                batchId.Trim(),
                out int capacity);

            return capacity;
        }

        public IReadOnlyDictionary<
            string,
            int>
            GetBatchTypes()
        {
            return batchCapacities;
        }

        public void Reset()
        {
            batchCapacities.Clear();

            Initialized = false;
        }
    }
}
