using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.NavalWarfare
{
    public enum MarineReconType
    {
        Visual,
        Radar,
        Thermal,
        Sonar,
        Acoustic,
        Surface,
        Underwater
    }

    public sealed class MarineReconContact
    {
        public string ContactId { get; }
        public MarineReconType DetectionType { get; }

        public float Confidence { get; private set; }
        public float Distance { get; private set; }

        public bool Active { get; private set; }

        public MarineReconContact(
            string contactId,
            MarineReconType detectionType)
        {
            ContactId =
                contactId ?? string.Empty;

            DetectionType =
                detectionType;

            Active = true;
        }

        public void Update(
            float confidence,
            float distance)
        {
            Confidence =
                Math.Clamp(
                    confidence,
                    0f,
                    1f);

            Distance =
                Math.Max(
                    0f,
                    distance);

            Active = true;
        }

        public void Clear()
        {
            Active = false;
        }
    }

    public sealed class MarineReconnaissanceState
    {
        public string UnitId { get; }

        private readonly Dictionary<string, MarineReconContact> contacts =
            new Dictionary<string, MarineReconContact>(
                StringComparer.OrdinalIgnoreCase);

        public MarineReconnaissanceState(
            string unitId)
        {
            UnitId =
                unitId ?? string.Empty;
        }

        public void ReportContact(
            string contactId,
            MarineReconType detectionType,
            float confidence,
            float distance)
        {
            if (string.IsNullOrWhiteSpace(contactId))
            {
                return;
            }

            if (!contacts.TryGetValue(
                    contactId,
                    out MarineReconContact contact))
            {
                contact =
                    new MarineReconContact(
                        contactId,
                        detectionType);

                contacts.Add(
                    contactId,
                    contact);
            }

            contact.Update(
                confidence,
                distance);
        }

        public bool TryGetContact(
            string contactId,
            out MarineReconContact contact)
        {
            return contacts.TryGetValue(
                contactId,
                out contact);
        }

        public IReadOnlyCollection<MarineReconContact> GetContacts()
        {
            return contacts.Values;
        }

        public void ClearContact(
            string contactId)
        {
            contacts.Remove(contactId);
        }

        public void Clear()
        {
            contacts.Clear();
        }
    }

    public sealed class MarineReconnaissanceSystem
    {
        private readonly Dictionary<string, MarineReconnaissanceState> states =
            new Dictionary<string, MarineReconnaissanceState>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterUnit(
            string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!states.ContainsKey(unitId))
            {
                states.Add(
                    unitId,
                    new MarineReconnaissanceState(unitId));
            }
        }

        public void ReportContact(
            string unitId,
            string contactId,
            MarineReconType detectionType,
            float confidence,
            float distance)
        {
            RegisterUnit(unitId);

            states[unitId].ReportContact(
                contactId,
                detectionType,
                confidence,
                distance);
        }

        public bool TryGetState(
            string unitId,
            out MarineReconnaissanceState state)
        {
            return states.TryGetValue(
                unitId,
                out state);
        }

        public void RemoveUnit(
            string unitId)
        {
            states.Remove(unitId);
        }

        public void Clear()
        {
            states.Clear();
        }
    }
}
