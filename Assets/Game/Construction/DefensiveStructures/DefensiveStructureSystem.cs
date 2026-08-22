using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Construction
{
    public enum DefensiveStructureType
    {
        Wall,
        Barrier,
        Bunker,
        Turret,
        AntiAir,
        ShieldGenerator,
        SensorTower
    }

    public enum DefensiveStructureState
    {
        Planned,
        Constructing,
        Operational,
        Damaged,
        Destroyed
    }

    public sealed class DefensiveStructure
    {
        public string StructureId { get; }
        public DefensiveStructureType Type { get; }
        public Vector3 Position { get; }
        public float MaxHealth { get; }
        public float Health { get; private set; }
        public DefensiveStructureState State { get; private set; }

        public bool Operational =>
            State == DefensiveStructureState.Operational &&
            Health > 0f;

        public DefensiveStructure(
            string structureId,
            DefensiveStructureType type,
            Vector3 position,
            float maxHealth)
        {
            StructureId = structureId ?? string.Empty;
            Type = type;
            Position = position;
            MaxHealth = Mathf.Max(1f, maxHealth);
            Health = MaxHealth;
            State = DefensiveStructureState.Planned;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(StructureId) &&
            MaxHealth > 0f;

        public void BeginConstruction()
        {
            if (State == DefensiveStructureState.Planned)
            {
                State = DefensiveStructureState.Constructing;
            }
        }

        public void CompleteConstruction()
        {
            if (State == DefensiveStructureState.Constructing)
            {
                State = DefensiveStructureState.Operational;
            }
        }

        public void ApplyDamage(float amount)
        {
            if (State == DefensiveStructureState.Destroyed)
            {
                return;
            }

            Health = Mathf.Max(0f, Health - Mathf.Max(0f, amount));

            if (Health <= 0f)
            {
                State = DefensiveStructureState.Destroyed;
            }
            else if (Health < MaxHealth)
            {
                State = DefensiveStructureState.Damaged;
            }
        }

        public void Repair(float amount)
        {
            if (State == DefensiveStructureState.Destroyed)
            {
                return;
            }

            Health = Mathf.Min(
                MaxHealth,
                Health + Mathf.Max(0f, amount));

            if (Health >= MaxHealth &&
                State == DefensiveStructureState.Damaged)
            {
                State = DefensiveStructureState.Operational;
            }
        }
    }

    public sealed class DefensiveStructureSystem
    {
        private readonly Dictionary<string, DefensiveStructure> structures =
            new Dictionary<string, DefensiveStructure>(
                StringComparer.OrdinalIgnoreCase);

        public bool RegisterStructure(
            DefensiveStructure structure)
        {
            if (structure == null ||
                !structure.Valid ||
                structures.ContainsKey(structure.StructureId))
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
            out DefensiveStructure structure)
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
                    out DefensiveStructure structure))
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
                    out DefensiveStructure structure))
            {
                return false;
            }

            structure.CompleteConstruction();
            return true;
        }

        public bool ApplyDamage(
            string structureId,
            float amount)
        {
            if (!structures.TryGetValue(
                    structureId,
                    out DefensiveStructure structure))
            {
                return false;
            }

            structure.ApplyDamage(amount);
            return true;
        }

        public bool Repair(
            string structureId,
            float amount)
        {
            if (!structures.TryGetValue(
                    structureId,
                    out DefensiveStructure structure))
            {
                return false;
            }

            structure.Repair(amount);
            return true;
        }

        public IReadOnlyCollection<DefensiveStructure>
            GetStructures()
        {
            return structures.Values;
        }

        public IReadOnlyCollection<DefensiveStructure>
            GetOperationalStructures()
        {
            List<DefensiveStructure> operational =
                new List<DefensiveStructure>();

            foreach (
                DefensiveStructure structure
                in structures.Values)
            {
                if (structure.Operational)
                {
                    operational.Add(structure);
                }
            }

            return operational;
        }

        public void Clear()
        {
            structures.Clear();
        }
    }
}
