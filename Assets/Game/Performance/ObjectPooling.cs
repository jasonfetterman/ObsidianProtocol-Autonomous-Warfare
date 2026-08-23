using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Performance
{
    public sealed class PooledObject<T>
    {
        public T Value { get; }

        public bool InUse { get; private set; }

        public PooledObject(T value)
        {
            Value = value;
            InUse = false;
        }

        public bool Acquire()
        {
            if (InUse)
            {
                return false;
            }

            InUse = true;

            return true;
        }

        public bool Release()
        {
            if (!InUse)
            {
                return false;
            }

            InUse = false;

            return true;
        }
    }

    public sealed class ObjectPool<T>
    {
        private readonly List<PooledObject<T>> objects =
            new List<PooledObject<T>>();

        private readonly Func<T> factory;

        public int Capacity =>
            objects.Count;

        public int ActiveCount
        {
            get
            {
                int count = 0;

                foreach (PooledObject<T> item
                         in objects)
                {
                    if (item.InUse)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public int AvailableCount =>
            Capacity - ActiveCount;

        public ObjectPool(
            Func<T> factory)
        {
            this.factory =
                factory ??
                throw new ArgumentNullException(
                    nameof(factory));
        }

        public bool Prewarm(
            int count)
        {
            if (count < 0)
            {
                return false;
            }

            for (int i = 0; i < count; i++)
            {
                objects.Add(
                    new PooledObject<T>(
                        factory()));
            }

            return true;
        }

        public T Acquire()
        {
            foreach (PooledObject<T> item
                     in objects)
            {
                if (item.Acquire())
                {
                    return item.Value;
                }
            }

            PooledObject<T> created =
                new PooledObject<T>(
                    factory());

            created.Acquire();

            objects.Add(created);

            return created.Value;
        }

        public bool Release(
            T value)
        {
            foreach (PooledObject<T> item
                     in objects)
            {
                if (EqualityComparer<T>.Default.Equals(
                        item.Value,
                        value))
                {
                    return item.Release();
                }
            }

            return false;
        }

        public void Clear()
        {
            objects.Clear();
        }
    }

    public sealed class ObjectPooling
    {
        private readonly Dictionary<
            string,
            int> poolCapacities =
            new Dictionary<
                string,
                int>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int PoolCount =>
            poolCapacities.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            poolCapacities.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterPool(
            string poolId,
            int initialCapacity)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(poolId) ||
                initialCapacity < 0)
            {
                return false;
            }

            string id =
                poolId.Trim();

            if (poolCapacities.ContainsKey(id))
            {
                return false;
            }

            poolCapacities.Add(
                id,
                initialCapacity);

            return true;
        }

        public bool SetCapacity(
            string poolId,
            int capacity)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(poolId) ||
                capacity < 0)
            {
                return false;
            }

            string id =
                poolId.Trim();

            if (!poolCapacities.ContainsKey(id))
            {
                return false;
            }

            poolCapacities[id] =
                capacity;

            return true;
        }

        public int GetCapacity(
            string poolId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(poolId))
            {
                return 0;
            }

            poolCapacities.TryGetValue(
                poolId.Trim(),
                out int capacity);

            return capacity;
        }

        public IReadOnlyDictionary<
            string,
            int>
            GetPools()
        {
            return poolCapacities;
        }

        public void Reset()
        {
            poolCapacities.Clear();

            Initialized = false;
        }
    }
}
