using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.AI.Commanders
{
    public enum ResourceCondition
    {
        Unknown,
        Abundant,
        Stable,
        Strained,
        Critical,
        Depleted
    }

    public sealed class ResourceStatus
    {
        public string ResourceId { get; }

        public float Available
        {
            get;
            private set;
        }

        public float Capacity
        {
            get;
            private set;
        }

        public float ConsumptionRate
        {
            get;
            private set;
        }

        public float ProductionRate
        {
            get;
            private set;
        }

        public float SupplySecurity
        {
            get;
            private set;
        }

        public ResourceCondition Condition
        {
            get;
            private set;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(ResourceId) &&
            Available >= 0.0f &&
            Capacity > 0.0f;

        public ResourceStatus(
            string resourceId)
        {
            ResourceId =
                resourceId ?? string.Empty;

            Available = 0.0f;
            Capacity = 1.0f;

            ConsumptionRate = 0.0f;
            ProductionRate = 0.0f;
            SupplySecurity = 0.0f;

            Condition =
                ResourceCondition.Unknown;
        }

        public void SetStock(
            float available,
            float capacity)
        {
            Available =
                Math.Max(
                    0.0f,
                    available);

            Capacity =
                Math.Max(
                    0.001f,
                    capacity);

            Recalculate();
        }

        public void SetFlow(
            float productionRate,
            float consumptionRate)
        {
            ProductionRate =
                Math.Max(
                    0.0f,
                    productionRate);

            ConsumptionRate =
                Math.Max(
                    0.0f,
                    consumptionRate);

            Recalculate();
        }

        public void SetSupplySecurity(
            float security)
        {
            SupplySecurity =
                Clamp01(security);

            Recalculate();
        }

        public float StockRatio =>
            Clamp01(
                Available /
                Capacity);

        public float NetFlow =>
            ProductionRate -
            ConsumptionRate;

        private void Recalculate()
        {
            float stockRatio =
                StockRatio;

            if (Available <= 0.0f)
            {
                Condition =
                    ResourceCondition.Depleted;

                return;
            }

            if (stockRatio <= 0.15f ||
                SupplySecurity <= 0.15f)
            {
                Condition =
                    ResourceCondition.Critical;

                return;
            }

            if (NetFlow < 0.0f &&
                stockRatio <= 0.35f)
            {
                Condition =
                    ResourceCondition.Strained;

                return;
            }

            if (stockRatio >= 0.75f &&
                NetFlow >= 0.0f &&
                SupplySecurity >= 0.70f)
            {
                Condition =
                    ResourceCondition.Abundant;

                return;
            }

            Condition =
                ResourceCondition.Stable;
        }

        private static float Clamp01(
            float value)
        {
            return Math.Max(
                0.0f,
                Math.Min(
                    1.0f,
                    value));
        }
    }

    public sealed class ResourceEvaluation
    {
        private readonly Dictionary<
            string,
            ResourceStatus> resources =
            new Dictionary<
                string,
                ResourceStatus>(
                StringComparer.OrdinalIgnoreCase);

        public bool Valid =>
            resources.Count > 0;

        public bool Register(
            ResourceStatus resource)
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
            out ResourceStatus resource)
        {
            return resources.TryGetValue(
                resourceId,
                out resource);
        }

        public bool HasCriticalResource()
        {
            foreach (ResourceStatus resource
                in resources.Values)
            {
                if (resource.Condition ==
                    ResourceCondition.Critical ||
                    resource.Condition ==
                    ResourceCondition.Depleted)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasStrainedResource()
        {
            foreach (ResourceStatus resource
                in resources.Values)
            {
                if (resource.Condition ==
                    ResourceCondition.Strained)
                {
                    return true;
                }
            }

            return false;
        }

        public float AverageSupplySecurity
        {
            get
            {
                if (resources.Count == 0)
                    return 0.0f;

                float total = 0.0f;

                foreach (ResourceStatus resource
                    in resources.Values)
                {
                    total +=
                        resource.SupplySecurity;
                }

                return total /
                       resources.Count;
            }
        }

        public float AverageStockRatio
        {
            get
            {
                if (resources.Count == 0)
                    return 0.0f;

                float total = 0.0f;

                foreach (ResourceStatus resource
                    in resources.Values)
                {
                    total +=
                        resource.StockRatio;
                }

                return total /
                       resources.Count;
            }
        }

        public IReadOnlyCollection<
            ResourceStatus>
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
