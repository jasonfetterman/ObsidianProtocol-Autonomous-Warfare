using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Multiplayer.Online
{
    public enum AuthenticationState
    {
        Unauthenticated,
        Authenticating,
        Authenticated,
        Failed,
        SignedOut
    }

    public sealed class OnlineAuthentication
    {
        private readonly HashSet<string> authenticatedPlayers =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        public AuthenticationState State { get; private set; }

        public bool Initialized { get; private set; }

        public string AuthenticatedPlayerId { get; private set; }

        public bool IsAuthenticated =>
            State == AuthenticationState.Authenticated;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            State =
                AuthenticationState.Unauthenticated;

            AuthenticatedPlayerId =
                string.Empty;

            authenticatedPlayers.Clear();

            Initialized = true;

            return true;
        }

        public bool BeginAuthentication(
            string playerId)
        {
            if (!Initialized ||
                State != AuthenticationState.Unauthenticated ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return false;
            }

            AuthenticatedPlayerId =
                playerId.Trim();

            State =
                AuthenticationState.Authenticating;

            return true;
        }

        public bool CompleteAuthentication(
            bool successful)
        {
            if (!Initialized ||
                State != AuthenticationState.Authenticating)
            {
                return false;
            }

            if (!successful)
            {
                State =
                    AuthenticationState.Failed;

                AuthenticatedPlayerId =
                    string.Empty;

                return false;
            }

            authenticatedPlayers.Add(
                AuthenticatedPlayerId);

            State =
                AuthenticationState.Authenticated;

            return true;
        }

        public bool IsPlayerAuthenticated(
            string playerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return false;
            }

            return authenticatedPlayers.Contains(
                playerId.Trim());
        }

        public bool SignOut()
        {
            if (!Initialized ||
                State != AuthenticationState.Authenticated)
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(
                    AuthenticatedPlayerId))
            {
                authenticatedPlayers.Remove(
                    AuthenticatedPlayerId);
            }

            AuthenticatedPlayerId =
                string.Empty;

            State =
                AuthenticationState.SignedOut;

            return true;
        }

        public void Reset()
        {
            authenticatedPlayers.Clear();

            AuthenticatedPlayerId =
                string.Empty;

            State =
                AuthenticationState.Unauthenticated;

            Initialized = false;
        }
    }
}
