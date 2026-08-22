using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Construction
{
    public enum ProductionStructureType
    {
        ProductionPlant,
        FabricationPlant,
        VehicleFactory,
        DroneFactory,
        NavalFactory,
        EquipmentFactory,
        WeaponsFactory,
        AssemblyPlant
    }

    public enum ProductionStructureState
    {
        Planned,
        UnderConstruction,
        Operational,
        Damaged,
        Destroyed
    }

    public sealed class ProductionStructure
    {
        public string StructureId { get; }
        public string OwnerId { get; }
        public ProductionStructureType StructureType { get; }

        public float BuildCost { get; }
        public float BuildTime { get; }
        public float MaxHealth { get; }
        public float Health { get; private set; }

        public float ProductionRate { get; }
        public int ProductionCapacity { get; }

        public ProductionStructureState State { get; private set; }

        public bool Operational =>
            State == ProductionStructureState.Operational &&
            Health > 0f;

        public bool Valid =>
            !string.IsNullOrWhiteSpace(StructureId) &&
            !string.IsNullOrWhiteSpace(OwnerId) &&
            BuildCost > 0f &&
            BuildTime >= 0f &&
            MaxHealth > 0f &&
            ProductionRate > 0f &&
            ProductionCapacity > 0;

        public ProductionStructure(
            string structureId,
            string ownerId,
            ProductionStructureType structureType,
            float buildCost,
            float buildTime,
            float maxHealth,
            float productionRate,
            int productionCapacity)
        {
            StructureId = structureId ?? string.Empty;
            OwnerId = ownerId ?? string.Empty;
            StructureType = structureType;

            BuildCost = Mathf.Max(0f, buildCost);
            BuildTime = Mathf.Max(0f, buildTime);
            MaxHealth = Mathf.Max(1f, maxHealth);
            Health = MaxHealth;

            ProductionRate = Mathf.Max(0f, productionRate);
            ProductionCapacity = Mathf.Max(1, productionCapacity);

            State = ProductionStructureState.Planned;
        }

        public void BeginConstruction()
        {
            if (State == ProductionStructureState.Planned)
            {
                State = ProductionStructureState.UnderConstruction;
            }
        }

        public void CompleteConstruction()
        {
            if (State == ProductionStructureState.UnderConstruction)
            {
                State = ProductionStructureState.Operational;
                Health = MaxHealth;
            }
        }

        public void ApplyDamage(float amount)
        {
            if (State == ProductionStructureState.Destroyed ||
                amount <= 0f)
            {
                return;
            }

            Health = Mathf.Max(
                0f,
                Health - amount);

            if (Health <= 0f)
            {
                State = ProductionStructureState.Destroyed;
            }
            else if (State == ProductionStructureState.Operational)
            {
                State = ProductionStructureState.Damaged;
            }
        }

        public void Repair(float amount)
        {
            if (State == ProductionStructureState.Destroyed ||
                amount <= 0f)
            {
                return;
            }

            Health = Mathf.Min(
                MaxHealth,
                Health + amount);

            if (Health >= MaxHealth &&
                State == ProductionStructureState.Damaged)
            {
                State = ProductionStructureState.Operational;
            }
        }

        public float GetProductionOutput(float deltaTime)
        {
            if (!Operational ||
                deltaTime <= 0f)
            {
                return 0f;
            }

            return ProductionRate * deltaTime;
        }
    }

    public sealed class ProductionStructureSystem
    {
        private readonly Dictionary<string, ProductionStructure> structures =
            new Dictionary<string, ProductionStructure>(
                StringComparer.OrdinalIgnoreCase);

        public bool RegisterStructure(
            ProductionStructure structure)
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
            if (string.IsNullOrWhiteSpace(structureId))
            {
                return false;
            }

            return structures.Remove(structureId);
        }

        public bool TryGetStructure(
            string structureId,
            out ProductionStructure structure)
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
                    out ProductionStructure structure))
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
                    out ProductionStructure structure))
            {
                return false;
            }

            structure.CompleteConstruction();
            return true;
        }

        public IReadOnlyCollection<ProductionStructure>
            GetStructures()
        {
            return structures.Values;
        }

        public IReadOnlyCollection<ProductionStructure>
            GetOperationalStructures()
        {
            List<ProductionStructure> operational =
                new List<ProductionStructure>();

            foreach (
                ProductionStructure structure
                in structures.Values)
            {
                if (structure.Operational)
                {
                    operational.Add(structure);
                }
            }

            return operational;
        }

        public float GetTotalProductionOutput(
            float deltaTime)
        {
            if (deltaTime <= 0f)
            {
                return 0f;
            }

            float total = 0f;

            foreach (
                ProductionStructure structure
                in structures.Values)
            {
                total += structure.GetProductionOutput(
                    deltaTime);
            }

            return total;
        }

        public int GetTotalProductionCapacity()
        {
            int total = 0;

            foreach (
                ProductionStructure structure
                in structures.Values)
            {
                if (structure.Operational)
                {
                    total += structure.ProductionCapacity;
                }
            }

            return total;
        }

        public void Clear()
        {
            structures.Clear();
        }
    }
}
