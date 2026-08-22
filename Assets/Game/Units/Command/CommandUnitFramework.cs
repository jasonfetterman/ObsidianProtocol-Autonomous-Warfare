using System;

namespace ObsidianProtocol.Game.Units.Command
{
    public enum CommandUnitRole
    {
        Command,
        Coordination,
        Intelligence,
        Logistics,
        Communication,
        StrategicSupport
    }

    public sealed class CommandUnitFramework
    {
        public string UnitId { get; }
        public string DisplayName { get; }

        public CommandUnitRole Role { get; private set; }

        public float CommandRange { get; private set; }
        public float IntelligenceRange { get; private set; }
        public float NetworkRange { get; private set; }

        public int CommandCapacity { get; private set; }

        public bool Operational { get; private set; }

        public CommandUnitFramework(
            string unitId,
            string displayName)
        {
            UnitId = unitId ?? string.Empty;
            DisplayName = displayName ?? string.Empty;

            Role = CommandUnitRole.Command;

            CommandRange = 0f;
            IntelligenceRange = 0f;
            NetworkRange = 0f;
            CommandCapacity = 0;

            Operational = false;
        }

        public void Configure(
            CommandUnitRole role,
            float commandRange,
            float intelligenceRange,
            float networkRange,
            int commandCapacity)
        {
            Role = role;

            CommandRange =
                Math.Max(0f, commandRange);

            IntelligenceRange =
                Math.Max(0f, intelligenceRange);

            NetworkRange =
                Math.Max(0f, networkRange);

            CommandCapacity =
                Math.Max(0, commandCapacity);
        }

        public void Activate()
        {
            Operational = true;
        }

        public void Deactivate()
        {
            Operational = false;
        }
    }
}
