using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Construction
{
    public enum ResourceStructureType
    {
        ResourceExtractor,
        Mine,
        LumberFacility,
        CoalFacility,
        IronFacility,
        AlloyFacility,
        ElectronicsFacility,
        FuelFacility,
        EnergyFacility,
        ResourceStorage
    }

    public enum ResourceStructureState
    {
        Planned,
        UnderConstruction,
        Operational,
        Damaged,
        Destroyed
    }

    public sealed class ResourceStructure
    {
        public string StructureId { get; }
        public string OwnerId { get; }
        public ResourceStructureType StructureType { get; }

        public float BuildCost { get; }
        public float BuildTime { get; }
        public float MaxHealth { get; }
        public float Health { get; private set; }

        public float ProductionRate { get; }

        public ResourceStructureState State { get; private set; }

        public bool Operational =>
            State == ResourceStructureState.Operational &&
            Health > 0f;

        public bool Valid =>
            !string.IsNullOrWhiteSpace(StructureId) &&
            !string.IsNullOrWhiteSpace(OwnerId) &&
            BuildCost > 0f &&
            BuildTime >= 0f &&
            MaxHealth > 0f &&
            ProductionRate >= 0f;

        public ResourceStructure(
            string structureId,
            string ownerId,
            ResourceStructureType structureType,
            float buildCost,
            float buildTime,
            float maxHealth,
            float productionRate)
        {
            StructureId = structureId ?? string.Empty;
            OwnerId = ownerId ?? string.Empty;
            StructureType = structureType;

            BuildCost = Mathf.Max(0f, buildCost);
            BuildTime = Mathf.Max(0f, buildTime);
            MaxHealth = Mathf.Max(1f, maxHealth);
            Health = MaxHealth;
            ProductionRate = Mathf.Max(0f, productionRate);

            State = ResourceStructureState.Planned;
        }

        public void BeginConstruction()
        {
            if (State == ResourceStructureState.Planned)
            {
                State = ResourceStructureState.UnderConstruction;
            }
        }

        public void CompleteConstruction()
        {
            if (State == ResourceStructureState.UnderConstruction)
            {
                State = ResourceStructureState.Operational;
                Health = MaxHealth;
            }
        }

        public void ApplyDamage(float amount)
        {
            if (State == ResourceStructureState.Destroyed ||
                amount <= 0f)
            {
                return;
            }

            Health = Mathf.Max(
                0f,
                Health - amount);

            if (Health <= 0f)
            {
                State = ResourceStructureState.Destroyed;
            }
            else if (State == ResourceStructureState.Operational)
            {
                State = ResourceStructureState.Damaged;
            }
        }

        public void Repair(float amount)
        {
            if (State == ResourceStructureState.Destroyed ||
                amount <= 0f)
            {
                return;
            }

            Health = Mathf.Min(
                MaxHealth,
                Health + amount);

            if (Health >= MaxHealth &&
                State == ResourceStructureState.Damaged)
            {
                State = ResourceStructureState.Operational;
            }
        }

        public float GetProduction(float deltaTime)
        {
            if (!Operational ||
                deltaTime <= 0f)
            {
                return 0f;
            }

            return ProductionRate * deltaTime;
        }
    }

    public sealed class ResourceStructureSystem
    {
        private readonly Dictionary<string, ResourceStructure> structures =
            new Dictionary<string, ResourceStructure>(
                StringComparer.OrdinalIgnoreCase);

        public bool RegisterStructure(
            ResourceStructure structure)
        {
            if (structure == null ||
                !structure.Valid ||
                structures.ContainsKey(
                    structure.StructureId))
            {
                return false;
            }

            structures.Add(
                structure.StructureId,
                structure);

            return true;
        }

        public bool RemoveStructure(
            string structureId)
        {
            if (string.IsNullOrWhiteSpace(
                    structureId))
            {
                return false;
            }

            return structures.Remove(
                structureId);
        }

        public bool TryGetStructure(
            string structureId,
            out ResourceStructure structure)
        {
            return structures.TryGetValue(
                structureId,
                out structure);
        }

        public bool BeginConstruction(
            string structureId)
        {
            if (!structures.TryGetValue(
                    structureId,
                    out ResourceStructure structure))
            {
                return false;
            }

            structure.BeginConstruction();

            return true;
        }

        public bool CompleteConstruction(
            string structureId)
        {
            if (!structures.TryGetValue(
                    structureId,
                    out ResourceStructure structure))
            {
                return false;
            }

            structure.CompleteConstruction();

            return true;
        }

        public IReadOnlyCollection<ResourceStructure>
            GetStructures()
        {
            return structures.Values;
        }

        public IReadOnlyCollection<ResourceStructure>
            GetOperationalStructures()
        {
            List<ResourceStructure> operational =
                new List<ResourceStructure>();

            foreach (
                ResourceStructure structure
                in structures.Values)
            {
                if (structure.Operational)
                {
                    operational.Add(
                        structure);
                }
            }

            return operational;
        }

        public float GetTotalProduction(
            float deltaTime)
        {
            if (deltaTime <= 0f)
            {
                return 0f;
            }

            float total =
                0f;

            foreach (
                ResourceStructure structure
                in structures.Values)
            {
                total +=
                    structure.GetProduction(
                        deltaTime);
            }

            return total;
        }

        public void Clear()
        {
            structures.Clear();
        }
    }
}
