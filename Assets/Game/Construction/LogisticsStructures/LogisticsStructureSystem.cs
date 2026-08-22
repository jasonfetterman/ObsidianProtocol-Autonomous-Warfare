using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Construction
{
    public enum LogisticsStructureType
    {
        SupplyDepot,
        FuelDepot,
        AmmunitionDepot,
        RepairDepot,
        ResourceDepot,
        ForwardSupplyPoint,
        LogisticsHub,
        TransportTerminal
    }

    public enum LogisticsStructureState
    {
        Planned,
        UnderConstruction,
        Operational,
        Damaged,
        Destroyed
    }

    public sealed class LogisticsStructure
    {
        public string StructureId { get; }
        public string OwnerId { get; }
        public LogisticsStructureType StructureType { get; }

        public float BuildCost { get; }
        public float BuildTime { get; }
        public float MaxHealth { get; }
        public float Health { get; private set; }

        public float StorageCapacity { get; }
        public float Throughput { get; }

        public LogisticsStructureState State { get; private set; }

        public bool Operational =>
            State == LogisticsStructureState.Operational &&
            Health > 0f;

        public bool Valid =>
            !string.IsNullOrWhiteSpace(StructureId) &&
            !string.IsNullOrWhiteSpace(OwnerId) &&
            BuildCost > 0f &&
            BuildTime >= 0f &&
            MaxHealth > 0f &&
            StorageCapacity >= 0f &&
            Throughput >= 0f;

        public LogisticsStructure(
            string structureId,
            string ownerId,
            LogisticsStructureType structureType,
            float buildCost,
            float buildTime,
            float maxHealth,
            float storageCapacity,
            float throughput)
        {
            StructureId = structureId ?? string.Empty;
            OwnerId = ownerId ?? string.Empty;
            StructureType = structureType;

            BuildCost = Mathf.Max(0f, buildCost);
            BuildTime = Mathf.Max(0f, buildTime);
            MaxHealth = Mathf.Max(1f, maxHealth);
            Health = MaxHealth;

            StorageCapacity =
                Mathf.Max(0f, storageCapacity);

            Throughput =
                Mathf.Max(0f, throughput);

            State =
                LogisticsStructureState.Planned;
        }

        public void BeginConstruction()
        {
            if (State ==
                LogisticsStructureState.Planned)
            {
                State =
                    LogisticsStructureState.UnderConstruction;
            }
        }

        public void CompleteConstruction()
        {
            if (State ==
                LogisticsStructureState.UnderConstruction)
            {
                State =
                    LogisticsStructureState.Operational;

                Health =
                    MaxHealth;
            }
        }

        public void ApplyDamage(
            float amount)
        {
            if (State ==
                    LogisticsStructureState.Destroyed ||
                amount <= 0f)
            {
                return;
            }

            Health =
                Mathf.Max(
                    0f,
                    Health - amount);

            if (Health <= 0f)
            {
                State =
                    LogisticsStructureState.Destroyed;
            }
            else if (
                State ==
                LogisticsStructureState.Operational)
            {
                State =
                    LogisticsStructureState.Damaged;
            }
        }

        public void Repair(
            float amount)
        {
            if (State ==
                    LogisticsStructureState.Destroyed ||
                amount <= 0f)
            {
                return;
            }

            Health =
                Mathf.Min(
                    MaxHealth,
                    Health + amount);

            if (Health >= MaxHealth &&
                State ==
                LogisticsStructureState.Damaged)
            {
                State =
                    LogisticsStructureState.Operational;
            }
        }

        public float GetThroughput(
            float deltaTime)
        {
            if (!Operational ||
                deltaTime <= 0f)
            {
                return 0f;
            }

            return Throughput *
                   deltaTime;
        }
    }

    public sealed class LogisticsStructureSystem
    {
        private readonly Dictionary<
            string,
            LogisticsStructure> structures =
            new Dictionary<
                string,
                LogisticsStructure>(
                StringComparer.OrdinalIgnoreCase);

        public bool RegisterStructure(
            LogisticsStructure structure)
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
            out LogisticsStructure structure)
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
                    out LogisticsStructure structure))
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
                    out LogisticsStructure structure))
            {
                return false;
            }

            structure.CompleteConstruction();

            return true;
        }

        public IReadOnlyCollection<
            LogisticsStructure>
            GetStructures()
        {
            return structures.Values;
        }

        public IReadOnlyCollection<
            LogisticsStructure>
            GetOperationalStructures()
        {
            List<LogisticsStructure> operational =
                new List<LogisticsStructure>();

            foreach (
                LogisticsStructure structure
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

        public float GetTotalThroughput(
            float deltaTime)
        {
            if (deltaTime <= 0f)
            {
                return 0f;
            }

            float total =
                0f;

            foreach (
                LogisticsStructure structure
                in structures.Values)
            {
                total +=
                    structure.GetThroughput(
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
