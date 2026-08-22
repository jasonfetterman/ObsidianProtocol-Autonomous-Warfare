using System;

namespace ObsidianProtocol.Game.Multiplayer.Online
{
    public sealed class OnlinePlayerIdentity
    {
        public string PlayerId { get; private set; }

        public string DisplayName { get; private set; }

        public bool Authenticated { get; private set; }

        public bool Connected { get; private set; }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(PlayerId) &&
            !string.IsNullOrWhiteSpace(DisplayName);

        public bool Assign(
            string playerId,
            string displayName)
        {
            if (string.IsNullOrWhiteSpace(playerId) ||
                string.IsNullOrWhiteSpace(displayName))
            {
                return false;
            }

            PlayerId =
                playerId.Trim();

            DisplayName =
                displayName.Trim();

            Authenticated = false;
            Connected = false;

            return true;
        }

        public bool SetAuthenticated(
            bool authenticated)
        {
            if (!Valid)
            {
                return false;
            }

            Authenticated =
                authenticated;

            if (!authenticated)
            {
                Connected = false;
            }

            return true;
        }

        public bool SetConnected(
            bool connected)
        {
            if (!Valid ||
                !Authenticated)
            {
                return false;
            }

            Connected =
                connected;

            return true;
        }

        public bool CanPlay()
        {
            return Valid &&
                   Authenticated &&
                   Connected;
        }

        public void Clear()
        {
            PlayerId =
                string.Empty;

            DisplayName =
                string.Empty;

            Authenticated = false;
            Connected = false;
        }
    }
}
