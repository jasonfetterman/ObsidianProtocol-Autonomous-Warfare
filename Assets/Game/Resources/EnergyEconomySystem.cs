using System;
using System.Collections.Generic;
using ObsidianProtocol.Game.Construction;

namespace ObsidianProtocol.Game.Resources
{
    public sealed class EnergyEconomySystem
    {
        private sealed class PowerSource
        {
            public float Rate;
            public bool Online;
        }

        private readonly Dictionary<string, PowerSource> generators =
            new Dictionary<string, PowerSource>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, PowerSource> consumers =
            new Dictionary<string, PowerSource>(StringComparer.OrdinalIgnoreCase);

        private float generationRemainder;
        private float consumptionRemainder;

        public float GenerationPerSecond { get; private set; }
        public float ConsumptionPerSecond { get; private set; }

        public float NetPowerPerSecond =>
            GenerationPerSecond - ConsumptionPerSecond;

        public bool RegisterGenerator(string id, float ratePerSecond)
        {
            return RegisterSource(generators, id, ratePerSecond);
        }

        public bool RegisterConsumer(string id, float ratePerSecond)
        {
            return RegisterSource(consumers, id, ratePerSecond);
        }

        private static bool RegisterSource(
            Dictionary<string, PowerSource> sources,
            string id,
            float ratePerSecond)
        {
            if (string.IsNullOrWhiteSpace(id) ||
                float.IsNaN(ratePerSecond) ||
                float.IsInfinity(ratePerSecond) ||
                ratePerSecond < 0f ||
                sources.ContainsKey(id))
            {
                return false;
            }

            sources.Add(id, new PowerSource
            {
                Rate = ratePerSecond,
                Online = true
            });

            return true;
        }

        public bool SetGeneratorOnline(string id, bool online)
        {
            return SetOnline(generators, id, online);
        }

        public bool SetConsumerOnline(string id, bool online)
        {
            return SetOnline(consumers, id, online);
        }

        private static bool SetOnline(
            Dictionary<string, PowerSource> sources,
            string id,
            bool online)
        {
            if (string.IsNullOrWhiteSpace(id) ||
                !sources.TryGetValue(id, out PowerSource source))
            {
                return false;
            }

            source.Online = online;
            return true;
        }

        public bool RemoveGenerator(string id) =>
            !string.IsNullOrWhiteSpace(id) && generators.Remove(id);

        public bool RemoveConsumer(string id) =>
            !string.IsNullOrWhiteSpace(id) && consumers.Remove(id);

        public void Update(float deltaTime, ResourceSystem resources, ResourceStructureSystem structures)
        {
            if (deltaTime <= 0f || resources == null)
                return;

            SyncEnergyFacilities(structures);
            GenerationPerSecond = GetTotalRate(generators);
            ConsumptionPerSecond = GetTotalRate(consumers);

            generationRemainder += GenerationPerSecond * deltaTime;
            int generated = (int)generationRemainder;

            if (generated > 0)
            {
                resources.Add("energy", generated);
                generationRemainder -= generated;
            }

            consumptionRemainder += ConsumptionPerSecond * deltaTime;
            int requested = (int)consumptionRemainder;

            if (requested > 0)
            {
                int available = resources.GetAmount("energy");
                int spent = Math.Min(requested, available);

                if (spent > 0 && resources.TrySpend("energy", spent))
                    consumptionRemainder -= spent;
                else
                    consumptionRemainder = Math.Min(consumptionRemainder, 1f);
            }
        }

        private void SyncEnergyFacilities(ResourceStructureSystem structures)
        {
            var structureIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (structures != null)
            {
                foreach (ResourceStructure structure in structures.GetStructures())
                {
                    if (structure == null || structure.StructureType != ResourceStructureType.EnergyFacility)
                        continue;

                    string id = "STRUCTURE:" + structure.StructureId;
                    structureIds.Add(id);

                    if (!generators.ContainsKey(id))
                        RegisterGenerator(id, structure.ProductionRate);

                    SetGeneratorOnline(id, structure.Operational);
                }
            }

            var staleIds = new List<string>();
            foreach (string id in generators.Keys)
            {
                if (id.StartsWith("STRUCTURE:", StringComparison.OrdinalIgnoreCase) && !structureIds.Contains(id))
                    staleIds.Add(id);
            }

            foreach (string id in staleIds)
                RemoveGenerator(id);
        }
        private static float GetTotalRate(
            Dictionary<string, PowerSource> sources)
        {
            float total = 0f;

            foreach (PowerSource source in sources.Values)
            {
                if (source.Online)
                    total += source.Rate;
            }

            return total;
        }
    }
}
