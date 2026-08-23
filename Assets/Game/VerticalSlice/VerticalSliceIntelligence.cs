using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.VerticalSlice
{
    public enum VerticalSliceIntelType
    {
        UnitDetected,
        EnemyDetected,
        StructureDetected,
        ResourceDetected,
        ObjectiveDetected,
        BattlefieldChange
    }

    public sealed class VerticalSliceIntelContact
    {
        public string ContactId { get; }

        public VerticalSliceIntelType Type { get; }

        public string SourceId { get; }

        public string TargetId { get; }

        public bool Confirmed { get; private set; }

        public VerticalSliceIntelContact(
            string contactId,
            VerticalSliceIntelType type,
            string sourceId,
            string targetId)
        {
            ContactId =
                contactId ?? string.Empty;

            Type =
                type;

            SourceId =
                sourceId ?? string.Empty;

            TargetId =
                targetId ?? string.Empty;

            Confirmed = false;
        }

        public bool Confirm()
        {
            if (Confirmed)
            {
                return false;
            }

            Confirmed = true;

            return true;
        }
    }

    public sealed class VerticalSliceIntelligence
    {
        private readonly Dictionary<
            string,
            VerticalSliceIntelContact> contacts =
            new Dictionary<
                string,
                VerticalSliceIntelContact>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ContactCount =>
            contacts.Count;

        public int ConfirmedContactCount
        {
            get
            {
                int count = 0;

                foreach (VerticalSliceIntelContact contact
                         in contacts.Values)
                {
                    if (contact.Confirmed)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            contacts.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterContact(
            string contactId,
            VerticalSliceIntelType type,
            string sourceId,
            string targetId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(contactId) ||
                string.IsNullOrWhiteSpace(sourceId) ||
                string.IsNullOrWhiteSpace(targetId))
            {
                return false;
            }

            string id =
                contactId.Trim();

            if (contacts.ContainsKey(id))
            {
                return false;
            }

            contacts.Add(
                id,
                new VerticalSliceIntelContact(
                    id,
                    type,
                    sourceId.Trim(),
                    targetId.Trim()));

            return true;
        }

        public bool ConfirmContact(
            string contactId)
        {
            VerticalSliceIntelContact contact =
                GetContact(contactId);

            return contact != null &&
                   contact.Confirm();
        }

        public VerticalSliceIntelContact GetContact(
            string contactId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(contactId))
            {
                return null;
            }

            contacts.TryGetValue(
                contactId.Trim(),
                out VerticalSliceIntelContact contact);

            return contact;
        }

        public IReadOnlyCollection<
            VerticalSliceIntelContact>
            GetContacts()
        {
            return contacts.Values;
        }

        public void Reset()
        {
            contacts.Clear();

            Initialized = false;
        }
    }
}
