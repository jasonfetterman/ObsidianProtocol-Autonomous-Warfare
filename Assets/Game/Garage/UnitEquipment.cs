using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Garage
{
    public sealed class UnitEquipment
    {
        public string EquipmentId { get; }
        public string EquipmentType { get; }

        public int Quantity { get; private set; }

        public bool Equipped { get; private set; }
        public bool Operational { get; private set; }

        public UnitEquipment(
            string equipmentId,
            string equipmentType,
            int quantity)
        {
            EquipmentId =
                equipmentId ?? string.Empty;

            EquipmentType =
                equipmentType ?? string.Empty;

            Quantity =
                Math.Max(0, quantity);

            Equipped = false;
            Operational = true;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(
                EquipmentId) &&
            Quantity > 0;

        public void Equip()
        {
            if (Quantity > 0)
                Equipped = true;
        }

        public void Unequip()
        {
            Equipped = false;
        }

        public void SetOperational(
            bool operational)
        {
            Operational =
                operational && Quantity > 0;
        }

        public void SetQuantity(
            int quantity)
        {
            Quantity =
                Math.Max(0, quantity);

            if (Quantity == 0)
            {
                Equipped = false;
                Operational = false;
            }
        }
    }

    public sealed class UnitEquipmentRegistry
    {
        private readonly Dictionary<
            string,
            List<UnitEquipment>> equipment =
            new Dictionary<
                string,
                List<UnitEquipment>>(
                StringComparer.OrdinalIgnoreCase);

        public bool RegisterUnit(
            string ownershipId)
        {
            if (string.IsNullOrWhiteSpace(
                    ownershipId) ||
                equipment.ContainsKey(ownershipId))
            {
                return false;
            }

            equipment.Add(
                ownershipId,
                new List<UnitEquipment>());

            return true;
        }

        public bool RemoveUnit(
            string ownershipId)
        {
            if (string.IsNullOrWhiteSpace(
                    ownershipId))
            {
                return false;
            }

            return equipment.Remove(ownershipId);
        }

        public bool AddEquipment(
            string ownershipId,
            UnitEquipment item)
        {
            if (string.IsNullOrWhiteSpace(
                    ownershipId) ||
                item == null ||
                !item.Valid ||
                !equipment.TryGetValue(
                    ownershipId,
                    out List<UnitEquipment> items))
            {
                return false;
            }

            foreach (UnitEquipment existing in items)
            {
                if (string.Equals(
                        existing.EquipmentId,
                        item.EquipmentId,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            items.Add(item);
            return true;
        }

        public bool RemoveEquipment(
            string ownershipId,
            string equipmentId)
        {
            if (!equipment.TryGetValue(
                    ownershipId,
                    out List<UnitEquipment> items) ||
                string.IsNullOrWhiteSpace(
                    equipmentId))
            {
                return false;
            }

            for (int i = 0; i < items.Count; i++)
            {
                if (string.Equals(
                        items[i].EquipmentId,
                        equipmentId,
                        StringComparison.OrdinalIgnoreCase))
                {
                    items.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public IReadOnlyList<UnitEquipment>
            GetEquipment(
                string ownershipId)
        {
            if (!equipment.TryGetValue(
                    ownershipId,
                    out List<UnitEquipment> items))
            {
                return Array.Empty<UnitEquipment>();
            }

            return items;
        }

        public void Clear()
        {
            equipment.Clear();
        }
    }
}
