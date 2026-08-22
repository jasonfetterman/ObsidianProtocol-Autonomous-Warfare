using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.NavalWarfare
{
    public enum SonarMode
    {
        Passive,
        Active,
        Ping
    }

    public enum SonarContactType
    {
        Unknown,
        SurfaceVessel,
        Submersible,
        Terrain,
        Wreck,
        Structure
    }

    public sealed class SonarContact
    {
        public string ContactId { get; }
        public SonarContactType Type { get; }

        public float Distance { get; private set; }
        public float Bearing { get; private set; }
        public float Depth { get; private set; }

        public float Confidence { get; private set; }

        public bool Active { get; private set; }

        public SonarContact(
            string contactId,
            SonarContactType type)
        {
            ContactId =
                contactId ?? string.Empty;

            Type =
                type;

            Active = true;
        }

        public void Update(
            float distance,
            float bearing,
            float depth,
            float confidence)
        {
            Distance =
                Math.Max(
                    0f,
                    distance);

            Bearing =
                bearing;

            Depth =
                Math.Max(
                    0f,
                    depth);

            Confidence =
                Math.Clamp(
                    confidence,
                    0f,
                    1f);

            Active = true;
        }

        public void Clear()
        {
            Active = false;
        }
    }

    public sealed class SonarState
    {
        public string UnitId { get; }

        public SonarMode Mode { get; private set; }

        public float MaximumRange { get; private set; }
        public float PingInterval { get; private set; }
        public float PingTimer { get; private set; }

        public bool Enabled { get; private set; }

        private readonly Dictionary<string, SonarContact> contacts =
            new Dictionary<string, SonarContact>(
                StringComparer.OrdinalIgnoreCase);

        public SonarState(
            string unitId)
        {
            UnitId =
                unitId ?? string.Empty;

            Mode =
                SonarMode.Passive;

            Enabled = true;
        }

        public void Configure(
            float maximumRange,
            float pingInterval)
        {
            MaximumRange =
                Math.Max(
                    0f,
                    maximumRange);

            PingInterval =
                Math.Max(
                    0.01f,
                    pingInterval);
        }

        public void SetMode(
            SonarMode mode)
        {
            Mode = mode;
            PingTimer = 0f;
        }

        public void SetEnabled(
            bool enabled)
        {
            Enabled = enabled;

            if (!enabled)
            {
                contacts.Clear();
            }
        }

        public bool Update(
            float deltaTime)
        {
            if (!Enabled)
            {
                return false;
            }

            PingTimer +=
                Math.Max(
                    0f,
                    deltaTime);

            if (Mode != SonarMode.Ping &&
                Mode != SonarMode.Active)
            {
                return false;
            }

            if (PingTimer < PingInterval)
            {
                return false;
            }

            PingTimer = 0f;

            return true;
        }

        public void ReportContact(
            string contactId,
            SonarContactType type,
            float distance,
            float bearing,
            float depth,
            float confidence)
        {
            if (!Enabled ||
                string.IsNullOrWhiteSpace(contactId) ||
                distance > MaximumRange)
            {
                return;
            }

            if (!contacts.TryGetValue(
                    contactId,
                    out SonarContact contact))
            {
                contact =
                    new SonarContact(
                        contactId,
                        type);

                contacts.Add(
                    contactId,
                    contact);
            }

            contact.Update(
                distance,
                bearing,
                depth,
                confidence);
        }

        public bool TryGetContact(
            string contactId,
            out SonarContact contact)
        {
            return contacts.TryGetValue(
                contactId,
                out contact);
        }

        public IReadOnlyCollection<SonarContact> GetContacts()
        {
            return contacts.Values;
        }

        public void ClearContacts()
        {
            contacts.Clear();
        }
    }

    public sealed class SonarSystem
    {
        private readonly Dictionary<string, SonarState> states =
            new Dictionary<string, SonarState>(
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
                    new SonarState(unitId));
            }
        }

        public void ConfigureUnit(
            string unitId,
            float maximumRange,
            float pingInterval)
        {
            RegisterUnit(unitId);

            states[unitId].Configure(
                maximumRange,
                pingInterval);
        }

        public void SetMode(
            string unitId,
            SonarMode mode)
        {
            RegisterUnit(unitId);

            states[unitId].SetMode(mode);
        }

        public bool UpdateUnit(
            string unitId,
            float deltaTime)
        {
            return states.TryGetValue(
                       unitId,
                       out SonarState state) &&
                   state.Update(deltaTime);
        }

        public void ReportContact(
            string unitId,
            string contactId,
            SonarContactType type,
            float distance,
            float bearing,
            float depth,
            float confidence)
        {
            RegisterUnit(unitId);

            states[unitId].ReportContact(
                contactId,
                type,
                distance,
                bearing,
                depth,
                confidence);
        }

        public void SetEnabled(
            string unitId,
            bool enabled)
        {
            RegisterUnit(unitId);

            states[unitId].SetEnabled(enabled);
        }

        public bool TryGetState(
            string unitId,
            out SonarState state)
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
