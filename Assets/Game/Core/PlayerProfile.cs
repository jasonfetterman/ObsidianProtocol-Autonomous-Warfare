using System;

namespace ObsidianProtocol.Game.Core
{
    [Serializable]
    public sealed class PlayerProfile
    {
        public string PlayerId;
        public string CommanderName;
        public string FactionId;

        public bool IsValid =>
            !string.IsNullOrWhiteSpace(PlayerId) &&
            !string.IsNullOrWhiteSpace(CommanderName) &&
            !string.IsNullOrWhiteSpace(FactionId);

        public void Initialize(
            string playerId,
            string commanderName,
            string factionId)
        {
            PlayerId =
                playerId ?? string.Empty;

            CommanderName =
                string.IsNullOrWhiteSpace(commanderName)
                    ? "COMMANDER"
                    : commanderName.Trim();

            FactionId =
                factionId ?? string.Empty;
        }

        public void Clear()
        {
            PlayerId = string.Empty;
            CommanderName = string.Empty;
            FactionId = string.Empty;
        }
    }
}
