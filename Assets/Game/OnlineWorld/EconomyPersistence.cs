using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OnlineWorld
{
    public sealed class EconomyPersistence
    {
        private readonly Dictionary<
            string,
            Dictionary<string, long>> accounts =
            new Dictionary<
                string,
                Dictionary<string, long>>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int AccountCount =>
            accounts.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            accounts.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterAccount(
            string playerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return false;
            }

            string id =
                playerId.Trim();

            if (accounts.ContainsKey(id))
            {
                return false;
            }

            accounts.Add(
                id,
                new Dictionary<string, long>(
                    StringComparer.OrdinalIgnoreCase));

            return true;
        }

        public bool SetBalance(
            string playerId,
            string currencyId,
            long amount)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId) ||
                string.IsNullOrWhiteSpace(currencyId) ||
                amount < 0)
            {
                return false;
            }

            if (!accounts.TryGetValue(
                    playerId.Trim(),
                    out Dictionary<string, long> account))
            {
                return false;
            }

            account[currencyId.Trim()] =
                amount;

            return true;
        }

        public long GetBalance(
            string playerId,
            string currencyId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId) ||
                string.IsNullOrWhiteSpace(currencyId))
            {
                return 0L;
            }

            if (!accounts.TryGetValue(
                    playerId.Trim(),
                    out Dictionary<string, long> account))
            {
                return 0L;
            }

            account.TryGetValue(
                currencyId.Trim(),
                out long amount);

            return amount;
        }

        public bool AddBalance(
            string playerId,
            string currencyId,
            long amount)
        {
            if (amount < 0)
            {
                return false;
            }

            long current =
                GetBalance(
                    playerId,
                    currencyId);

            return SetBalance(
                playerId,
                currencyId,
                current + amount);
        }

        public bool RemoveBalance(
            string playerId,
            string currencyId,
            long amount)
        {
            if (amount < 0)
            {
                return false;
            }

            long current =
                GetBalance(
                    playerId,
                    currencyId);

            if (current < amount)
            {
                return false;
            }

            return SetBalance(
                playerId,
                currencyId,
                current - amount);
        }

        public bool RemoveAccount(
            string playerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return false;
            }

            return accounts.Remove(
                playerId.Trim());
        }

        public void Reset()
        {
            accounts.Clear();
            Initialized = false;
        }
    }
}
