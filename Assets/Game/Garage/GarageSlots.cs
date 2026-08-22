using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Garage
{
    public sealed class GarageSlot
    {
        public string SlotId { get; }

        public string OwnershipId { get; private set; }

        public bool Occupied =>
            !string.IsNullOrWhiteSpace(OwnershipId);

        public GarageSlot(
            string slotId)
        {
            SlotId =
                slotId ?? string.Empty;

            OwnershipId = string.Empty;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(SlotId);

        public bool Assign(
            string ownershipId)
        {
            if (!Valid ||
                Occupied ||
                string.IsNullOrWhiteSpace(
                    ownershipId))
            {
                return false;
            }

            OwnershipId = ownershipId;
            return true;
        }

        public bool Release()
        {
            if (!Occupied)
                return false;

            OwnershipId = string.Empty;
            return true;
        }
    }

    public sealed class GarageSlots
    {
        private readonly Dictionary<
            string,
            GarageSlot> slots =
            new Dictionary<
                string,
                GarageSlot>(
                StringComparer.OrdinalIgnoreCase);

        public int Capacity =>
            slots.Count;

        public int OccupiedCount
        {
            get
            {
                int count = 0;

                foreach (GarageSlot slot in slots.Values)
                {
                    if (slot.Occupied)
                        count++;
                }

                return count;
            }
        }

        public int AvailableCount =>
            Math.Max(
                0,
                Capacity - OccupiedCount);

        public bool RegisterSlot(
            GarageSlot slot)
        {
            if (slot == null ||
                !slot.Valid ||
                slots.ContainsKey(slot.SlotId))
            {
                return false;
            }

            slots.Add(
                slot.SlotId,
                slot);

            return true;
        }

        public bool RemoveSlot(
            string slotId)
        {
            if (string.IsNullOrWhiteSpace(slotId))
                return false;

            if (!slots.TryGetValue(
                    slotId,
                    out GarageSlot slot))
            {
                return false;
            }

            if (slot.Occupied)
                return false;

            return slots.Remove(slotId);
        }

        public bool TryGetSlot(
            string slotId,
            out GarageSlot slot)
        {
            return slots.TryGetValue(
                slotId,
                out slot);
        }

        public bool AssignUnit(
            string slotId,
            string ownershipId)
        {
            if (!slots.TryGetValue(
                    slotId,
                    out GarageSlot slot))
            {
                return false;
            }

            return slot.Assign(
                ownershipId);
        }

        public bool ReleaseUnit(
            string slotId)
        {
            if (!slots.TryGetValue(
                    slotId,
                    out GarageSlot slot))
            {
                return false;
            }

            return slot.Release();
        }

        public bool ContainsUnit(
            string ownershipId)
        {
            if (string.IsNullOrWhiteSpace(
                    ownershipId))
            {
                return false;
            }

            foreach (GarageSlot slot in slots.Values)
            {
                if (string.Equals(
                        slot.OwnershipId,
                        ownershipId,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public IReadOnlyCollection<
            GarageSlot>
            GetSlots()
        {
            return slots.Values;
        }

        public void Clear()
        {
            slots.Clear();
        }
    }
}
