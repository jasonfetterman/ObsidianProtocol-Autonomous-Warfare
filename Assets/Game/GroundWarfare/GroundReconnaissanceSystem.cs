using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.GroundWarfare
{
    public enum GroundReconType
    {
        Visual,
        Thermal,
        Radar,
        Acoustic,
        Movement,
        Structure
    }

    public sealed class GroundReconContact
    {
        public string ContactId { get; }
        public GroundReconType DetectionType { get; }

        public float Confidence { get; private set; }
        public float Distance { get; private set; }

        public bool Active { get; private set; }

        public GroundReconContact(
            string contactId,
            GroundReconType detectionType)
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

    public sealed class GroundReconnaissanceState
    {
        public string UnitId { get; }

        private readonly Dictionary<string, GroundReconContact> contacts =
            new Dictionary<string, GroundReconContact>(
                StringComparer.OrdinalIgnoreCase);

        public GroundReconnaissanceState(
            string unitId)
        {
            UnitId =
                unitId ?? string.Empty;
        }

        public void ReportContact(
            string contactId,
            GroundReconType detectionType,
            float confidence,
            float distance)
        {
            if (string.IsNullOrWhiteSpace(contactId))
            {
                return;
            }

            if (!contacts.TryGetValue(
                    contactId,
                    out GroundReconContact contact))
            {
                contact =
                    new GroundReconContact(
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
            out GroundReconContact contact)
        {
            return contacts.TryGetValue(
                contactId,
                out contact);
        }

        public void ClearContact(
            string contactId)
        {
            if (contacts.TryGetValue(
                    contactId,
                    out GroundReconContact contact))
            {
                contact.Clear();
            }
        }

        public IReadOnlyCollection<GroundReconContact> GetContacts()
        {
            return contacts.Values;
        }

        public void Clear()
        {
            contacts.Clear();
        }
    }

    public sealed class GroundReconnaissanceSystem
    {
        private readonly Dictionary<string, GroundReconnaissanceState> states =
            new Dictionary<string, GroundReconnaissanceState>(
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
                    new GroundReconnaissanceState(unitId));
            }
        }

        public void ReportContact(
            string unitId,
            string contactId,
            GroundReconType detectionType,
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
            out GroundReconnaissanceState state)
        {
            return states.TryGetValue(
                unitId,
                out state);
        }

        public void ClearUnit(
            string unitId)
        {
            if (states.TryGetValue(
                    unitId,
                    out GroundReconnaissanceState state))
            {
                state.Clear();
            }
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
