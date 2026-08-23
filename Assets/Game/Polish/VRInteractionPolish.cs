using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Polish
{
    public sealed class VRInteractionPolish
    {
        private readonly Dictionary<
            string,
            bool> interactionStates =
            new Dictionary<
                string,
                bool>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int InteractionCount =>
            interactionStates.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            interactionStates.Clear();

            SetDefault("Grab", true);
            SetDefault("Release", true);
            SetDefault("ButtonPress", true);
            SetDefault("LeverPull", true);
            SetDefault("DoorInteraction", true);
            SetDefault("HatchInteraction", true);
            SetDefault("TerminalInteraction", true);
            SetDefault("OperatorEntry", true);

            Initialized = true;

            return true;
        }

        public bool SetInteraction(
            string interactionId,
            bool enabled)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(interactionId))
            {
                return false;
            }

            interactionStates[
                interactionId.Trim()] =
                enabled;

            return true;
        }

        public bool IsEnabled(
            string interactionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(interactionId))
            {
                return false;
            }

            return interactionStates.TryGetValue(
                interactionId.Trim(),
                out bool enabled) &&
                   enabled;
        }

        private void SetDefault(
            string key,
            bool enabled)
        {
            interactionStates[key] = enabled;
        }

        public void Reset()
        {
            interactionStates.Clear();
            Initialized = false;
        }
    }
}
