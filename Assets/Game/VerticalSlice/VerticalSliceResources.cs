using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.VerticalSlice
{
    public enum VerticalSliceResourceType
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

    public sealed class VerticalSliceResourcePool
    {
        private readonly Dictionary<
            VerticalSliceResourceType,
            int> resources =
            new Dictionary<
                VerticalSliceResourceType,
                int>();

        public bool Initialized { get; private set; }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            resources.Clear();

            foreach (VerticalSliceResourceType type
                     in Enum.GetValues(
                         typeof(
                             VerticalSliceResourceType)))
            {
                resources[type] = 0;
            }

            Initialized = true;

            return true;
        }

        public bool SetAmount(
            VerticalSliceResourceType type,
            int amount)
        {
            if (!Initialized ||
                amount < 0)
            {
                return false;
            }

            resources[type] =
                amount;

            return true;
        }

        public bool Add(
            VerticalSliceResourceType type,
            int amount)
        {
            if (!Initialized ||
                amount < 0)
            {
                return false;
            }

            resources[type] +=
                amount;

            return true;
        }

        public bool Spend(
            VerticalSliceResourceType type,
            int amount)
        {
            if (!Initialized ||
                amount < 0 ||
                !resources.ContainsKey(type) ||
                resources[type] < amount)
            {
                return false;
            }

            resources[type] -=
                amount;

            return true;
        }

        public int GetAmount(
            VerticalSliceResourceType type)
        {
            if (!Initialized ||
                !resources.ContainsKey(type))
            {
                return 0;
            }

            return resources[type];
        }

        public IReadOnlyDictionary<
            VerticalSliceResourceType,
            int>
            GetResources()
        {
            return resources;
        }

        public void Reset()
        {
            resources.Clear();

            Initialized = false;
        }
    }
}
