using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Multiplayer.Online
{
    public enum OnlineValidationResult
    {
        Valid,
        InvalidPlayer,
        InvalidCommand,
        InvalidAuthority,
        InvalidValue,
        RateLimited
    }

    public sealed class OnlineValidationRecord
    {
        public string PlayerId { get; }

        public int InvalidAttempts { get; private set; }

        public long LastValidationTick { get; private set; }

        public OnlineValidationRecord(
            string playerId)
        {
            PlayerId =
                playerId ?? string.Empty;
        }

        public void RecordInvalidAttempt(
            long tick)
        {
            InvalidAttempts++;
            LastValidationTick = tick;
        }

        public void RecordValidAttempt(
            long tick)
        {
            LastValidationTick = tick;
        }
    }

    public sealed class OnlineAntiCheatFoundation
    {
        private readonly Dictionary<
            string,
            OnlineValidationRecord> players =
            new Dictionary<
                string,
                OnlineValidationRecord>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int PlayerCount =>
            players.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            players.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterPlayer(
            string playerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return false;
            }

            string id =
                playerId.Trim();

            if (players.ContainsKey(id))
            {
                return false;
            }

            players.Add(
                id,
                new OnlineValidationRecord(id));

            return true;
        }

        public OnlineValidationResult
            ValidatePlayer(
                string playerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return OnlineValidationResult.InvalidPlayer;
            }

            return players.ContainsKey(
                       playerId.Trim())
                ? OnlineValidationResult.Valid
                : OnlineValidationResult.InvalidPlayer;
        }

        public OnlineValidationResult
            ValidateCommand(
                string playerId,
                bool commandValid,
                bool hasAuthority,
                long tick)
        {
            OnlineValidationResult result =
                ValidatePlayer(playerId);

            if (result != OnlineValidationResult.Valid)
            {
                return result;
            }

            if (!commandValid)
            {
                RecordInvalid(
                    playerId,
                    tick);

                return OnlineValidationResult.InvalidCommand;
            }

            if (!hasAuthority)
            {
                RecordInvalid(
                    playerId,
                    tick);

                return OnlineValidationResult.InvalidAuthority;
            }

            RecordValid(
                playerId,
                tick);

            return OnlineValidationResult.Valid;
        }

        public OnlineValidationResult
            ValidateValue(
                string playerId,
                bool valueValid,
                long tick)
        {
            OnlineValidationResult result =
                ValidatePlayer(playerId);

            if (result != OnlineValidationResult.Valid)
            {
                return result;
            }

            if (!valueValid)
            {
                RecordInvalid(
                    playerId,
                    tick);

                return OnlineValidationResult.InvalidValue;
            }

            RecordValid(
                playerId,
                tick);

            return OnlineValidationResult.Valid;
        }

        public bool IsRateLimited(
            string playerId,
            int maximumInvalidAttempts)
        {
            if (maximumInvalidAttempts <= 0 ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return true;
            }

            if (!players.TryGetValue(
                    playerId.Trim(),
                    out OnlineValidationRecord record))
            {
                return true;
            }

            return record.InvalidAttempts >=
                   maximumInvalidAttempts;
        }

        public OnlineValidationRecord
            GetRecord(
                string playerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return null;
            }

            players.TryGetValue(
                playerId.Trim(),
                out OnlineValidationRecord record);

            return record;
        }

        private void RecordInvalid(
            string playerId,
            long tick)
        {
            if (players.TryGetValue(
                    playerId.Trim(),
                    out OnlineValidationRecord record))
            {
                record.RecordInvalidAttempt(tick);
            }
        }

        private void RecordValid(
            string playerId,
            long tick)
        {
            if (players.TryGetValue(
                    playerId.Trim(),
                    out OnlineValidationRecord record))
            {
                record.RecordValidAttempt(tick);
            }
        }

        public void Reset()
        {
            players.Clear();
            Initialized = false;
        }
    }
}
