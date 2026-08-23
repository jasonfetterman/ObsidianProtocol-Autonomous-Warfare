using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.ContentCompletion
{
    public sealed class ContentCompletionEquipment
    {
        private readonly HashSet<string> equipment =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int EquipmentCount =>
            equipment.Count;

        public bool Complete =>
            EquipmentCount > 0;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            equipment.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterEquipment(
            string equipmentId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(equipmentId))
            {
                return false;
            }

            return equipment.Add(
                equipmentId.Trim());
        }

        public bool ContainsEquipment(
            string equipmentId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(equipmentId))
            {
                return false;
            }

            return equipment.Contains(
                equipmentId.Trim());
        }

        public IReadOnlyCollection<string>
            GetEquipment()
        {
            return equipment;
        }

        public void Reset()
        {
            equipment.Clear();
            Initialized = false;
        }
    }
}
