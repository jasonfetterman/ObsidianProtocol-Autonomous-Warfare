using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Construction
{
    public enum CommandStructureType
    {
        CommandPost,
        CommandCenter,
        OperationsCenter,
        CommunicationsHub,
        IntelligenceCenter,
        TacticalCommandNode,
        Headquarters
    }

    public enum CommandStructureState
    {
        Planned,
        UnderConstruction,
        Operational,
        Damaged,
        Destroyed
    }

    public sealed class CommandStructure
    {
        public string StructureId { get; }
        public string OwnerId { get; }
        public CommandStructureType StructureType { get; }

        public float BuildCost { get; }
        public float BuildTime { get; }
        public float MaxHealth { get; }
        public float Health { get; private set; }

        public float CommandRange { get; }
        public int CommandCapacity { get; }

        public CommandStructureState State { get; private set; }

        public bool Operational =>
            State == CommandStructureState.Operational &&
            Health > 0f;

        public bool Valid =>
            !string.IsNullOrWhiteSpace(StructureId) &&
            !string.IsNullOrWhiteSpace(OwnerId) &&
            BuildCost > 0f &&
            BuildTime >= 0f &&
            MaxHealth > 0f &&
            CommandRange >= 0f &&
            CommandCapacity > 0;

        public CommandStructure(
            string structureId,
            string ownerId,
            CommandStructureType structureType,
            float buildCost,
            float buildTime,
            float maxHealth,
            float commandRange,
            int commandCapacity)
        {
            StructureId = structureId ?? string.Empty;
            OwnerId = ownerId ?? string.Empty;
            StructureType = structureType;

            BuildCost = Mathf.Max(0f, buildCost);
            BuildTime = Mathf.Max(0f, buildTime);
            MaxHealth = Mathf.Max(1f, maxHealth);
            Health = MaxHealth;

            CommandRange = Mathf.Max(0f, commandRange);
            CommandCapacity = Mathf.Max(1, commandCapacity);

            State = CommandStructureState.Planned;
        }

        public void BeginConstruction()
        {
            if (State == CommandStructureState.Planned)
            {
                State = CommandStructureState.UnderConstruction;
            }
        }

        public void CompleteConstruction()
        {
            if (State == CommandStructureState.UnderConstruction)
            {
                State = CommandStructureState.Operational;
                Health = MaxHealth;
            }
        }

        public void ApplyDamage(float amount)
        {
            if (State == CommandStructureState.Destroyed ||
                amount <= 0f)
            {
                return;
            }

            Health = Mathf.Max(
                0f,
                Health - amount);

            if (Health <= 0f)
            {
                State = CommandStructureState.Destroyed;
            }
            else if (State == CommandStructureState.Operational)
            {
                State = CommandStructureState.Damaged;
            }
        }

        public void Repair(float amount)
        {
            if (State == CommandStructureState.Destroyed ||
                amount <= 0f)
            {
                return;
            }

            Health = Mathf.Min(
                MaxHealth,
                Health + amount);

            if (Health >= MaxHealth &&
                State == CommandStructureState.Damaged)
            {
                State = CommandStructureState.Operational;
            }
        }
    }

    public sealed class CommandStructureSystem
    {
        private readonly Dictionary<string, CommandStructure> structures =
            new Dictionary<string, CommandStructure>(
                StringComparer.OrdinalIgnoreCase);

        public bool RegisterStructure(
            CommandStructure structure)
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
            out CommandStructure structure)
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
                    out CommandStructure structure))
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
                    out CommandStructure structure))
            {
                return false;
            }

            structure.CompleteConstruction();
            return true;
        }

        public IReadOnlyCollection<CommandStructure>
            GetStructures()
        {
            return structures.Values;
        }

        public IReadOnlyCollection<CommandStructure>
            GetOperationalStructures()
        {
            List<CommandStructure> operational =
                new List<CommandStructure>();

            foreach (
                CommandStructure structure
                in structures.Values)
            {
                if (structure.Operational)
                {
                    operational.Add(structure);
                }
            }

            return operational;
        }

        public int GetTotalCommandCapacity()
        {
            int total = 0;

            foreach (
                CommandStructure structure
                in structures.Values)
            {
                if (structure.Operational)
                {
                    total += structure.CommandCapacity;
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
