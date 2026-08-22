using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Construction
{
    public enum BaseStructureState
    {
        Planned,
        Operational,
        Damaged,
        Disabled,
        Destroyed
    }

    public sealed class BaseStructure
    {
        public string StructureId { get; }

        public string OwnerId { get; }

        public string StructureType { get; }

        public float Health { get; private set; }

        public float MaxHealth { get; }

        public BaseStructureState State { get; private set; }

        public BaseStructure(
            string structureId,
            string ownerId,
            string structureType,
            float maxHealth)
        {
            StructureId =
                structureId ?? string.Empty;

            OwnerId =
                ownerId ?? string.Empty;

            StructureType =
                structureType ?? string.Empty;

            MaxHealth =
                Math.Max(
                    1f,
                    maxHealth);

            Health =
                MaxHealth;

            State =
                BaseStructureState.Planned;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(
                StructureId) &&
            !string.IsNullOrWhiteSpace(
                OwnerId) &&
            !string.IsNullOrWhiteSpace(
                StructureType) &&
            MaxHealth > 0f;

        public bool Operational =>
            State ==
            BaseStructureState.Operational;

        public bool Damaged =>
            State ==
                BaseStructureState.Damaged ||
            State ==
                BaseStructureState.Disabled;

        public void Activate()
        {
            if (State ==
                BaseStructureState.Planned)
            {
                State =
                    BaseStructureState.Operational;
            }
        }

        public void ApplyDamage(
            float amount)
        {
            if (State ==
                BaseStructureState.Destroyed)
            {
                return;
            }

            float damage =
                Math.Max(
                    0f,
                    amount);

            Health =
                Math.Max(
                    0f,
                    Health - damage);

            if (Health <= 0f)
            {
                State =
                    BaseStructureState.Destroyed;

                return;
            }

            if (Health < MaxHealth)
            {
                State =
                    BaseStructureState.Damaged;
            }
        }

        public void Repair(
            float amount)
        {
            if (State ==
                BaseStructureState.Destroyed)
            {
                return;
            }

            float repair =
                Math.Max(
                    0f,
                    amount);

            Health =
                Math.Min(
                    MaxHealth,
                    Health + repair);

            if (Health >= MaxHealth)
            {
                State =
                    BaseStructureState.Operational;
            }
            else if (Health > 0f)
            {
                State =
                    BaseStructureState.Damaged;
            }
        }

        public void Disable()
        {
            if (State !=
                BaseStructureState.Destroyed)
            {
                State =
                    BaseStructureState.Disabled;
            }
        }

        public void Restore()
        {
            if (State ==
                    BaseStructureState.Disabled ||
                State ==
                    BaseStructureState.Damaged)
            {
                if (Health > 0f)
                {
                    State =
                        BaseStructureState.Operational;
                }
            }
        }

        public void Destroy()
        {
            Health =
                0f;

            State =
                BaseStructureState.Destroyed;
        }
    }

    public sealed class BaseStructureSystem
    {
        private readonly Dictionary<string, BaseStructure>
            structures =
                new Dictionary<string, BaseStructure>(
                    StringComparer.OrdinalIgnoreCase);

        public bool RegisterStructure(
            BaseStructure structure)
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
            out BaseStructure structure)
        {
            return structures.TryGetValue(
                structureId,
                out structure);
        }

        public bool ActivateStructure(
            string structureId)
        {
            if (!structures.TryGetValue(
                    structureId,
                    out BaseStructure structure))
            {
                return false;
            }

            structure.Activate();

            return true;
        }

        public bool DamageStructure(
            string structureId,
            float amount)
        {
            if (!structures.TryGetValue(
                    structureId,
                    out BaseStructure structure))
            {
                return false;
            }

            structure.ApplyDamage(
                amount);

            return true;
        }

        public bool RepairStructure(
            string structureId,
            float amount)
        {
            if (!structures.TryGetValue(
                    structureId,
                    out BaseStructure structure))
            {
                return false;
            }

            structure.Repair(
                amount);

            return true;
        }

        public bool DisableStructure(
            string structureId)
        {
            if (!structures.TryGetValue(
                    structureId,
                    out BaseStructure structure))
            {
                return false;
            }

            structure.Disable();

            return true;
        }

        public bool RestoreStructure(
            string structureId)
        {
            if (!structures.TryGetValue(
                    structureId,
                    out BaseStructure structure))
            {
                return false;
            }

            structure.Restore();

            return true;
        }

        public bool DestroyStructure(
            string structureId)
        {
            if (!structures.TryGetValue(
                    structureId,
                    out BaseStructure structure))
            {
                return false;
            }

            structure.Destroy();

            return true;
        }

        public IReadOnlyCollection<BaseStructure>
            GetStructures()
        {
            return structures.Values;
        }

        public IReadOnlyCollection<BaseStructure>
            GetOperationalStructures()
        {
            List<BaseStructure> operational =
                new List<BaseStructure>();

            foreach (
                BaseStructure structure
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

        public void Clear()
        {
            structures.Clear();
        }
    }
}
