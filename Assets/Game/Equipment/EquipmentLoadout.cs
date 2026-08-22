using System.Collections.Generic;

namespace ObsidianProtocol.Game.Equipment
{
    public sealed class EquipmentLoadout
    {
        private readonly List<EquipmentDefinition> equipped = new();

        public IReadOnlyList<EquipmentDefinition> Equipped => equipped;

        public bool Equip(EquipmentDefinition equipment)
        {
            if (equipment == null || equipped.Contains(equipment))
            {
                return false;
            }

            equipped.Add(equipment);
            return true;
        }

        public bool Unequip(EquipmentDefinition equipment)
        {
            if (equipment == null)
            {
                return false;
            }

            return equipped.Remove(equipment);
        }

        public void Clear()
        {
            equipped.Clear();
        }
    }
}
