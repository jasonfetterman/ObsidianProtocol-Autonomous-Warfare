using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.VR
{
    public enum InteriorInteractionType
    {
        None,
        Door,
        Hatch,
        Seat,
        Console,
        Storage,
        Equipment,
        Maintenance,
        EmergencySystem
    }

    public sealed class InteriorInteractionPoint
    {
        public string InteractionId { get; }

        public InteriorInteractionType Type { get; }

        public bool Available { get; private set; }

        public bool Active { get; private set; }

        public InteriorInteractionPoint(
            string interactionId,
            InteriorInteractionType type)
        {
            InteractionId =
                interactionId ?? string.Empty;

            Type = type;

            Available = true;
            Active = false;
        }

        public bool SetAvailable(
            bool available)
        {
            if (!available)
            {
                Active = false;
            }

            Available = available;

            return true;
        }

        public bool Activate()
        {
            if (!Available ||
                Active)
            {
                return false;
            }

            Active = true;

            return true;
        }

        public bool Deactivate()
        {
            if (!Active)
            {
                return false;
            }

            Active = false;

            return true;
        }
    }

    public sealed class UnitInteriorInteraction
    {
        private readonly Dictionary<
            string,
            InteriorInteractionPoint> interactions =
            new Dictionary<
                string,
                InteriorInteractionPoint>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public string UnitId { get; private set; }

        public int InteractionCount =>
            interactions.Count;

        public bool Initialize(
            string unitId)
        {
            if (Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            UnitId =
                unitId.Trim();

            interactions.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterInteraction(
            string interactionId,
            InteriorInteractionType type)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(interactionId) ||
                type == InteriorInteractionType.None)
            {
                return false;
            }

            string id =
                interactionId.Trim();

            if (interactions.ContainsKey(id))
            {
                return false;
            }

            interactions.Add(
                id,
                new InteriorInteractionPoint(
                    id,
                    type));

            return true;
        }

        public bool SetAvailable(
            string interactionId,
            bool available)
        {
            InteriorInteractionPoint interaction =
                GetInteraction(interactionId);

            return interaction != null &&
                   interaction.SetAvailable(available);
        }

        public bool Activate(
            string interactionId)
        {
            InteriorInteractionPoint interaction =
                GetInteraction(interactionId);

            return interaction != null &&
                   interaction.Activate();
        }

        public bool Deactivate(
            string interactionId)
        {
            InteriorInteractionPoint interaction =
                GetInteraction(interactionId);

            return interaction != null &&
                   interaction.Deactivate();
        }

        public InteriorInteractionPoint GetInteraction(
            string interactionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(interactionId))
            {
                return null;
            }

            interactions.TryGetValue(
                interactionId.Trim(),
                out InteriorInteractionPoint interaction);

            return interaction;
        }

        public IReadOnlyCollection<
            InteriorInteractionPoint>
            GetInteractions()
        {
            return interactions.Values;
        }

        public void Reset()
        {
            interactions.Clear();

            Initialized = false;

            UnitId =
                string.Empty;
        }
    }
}
