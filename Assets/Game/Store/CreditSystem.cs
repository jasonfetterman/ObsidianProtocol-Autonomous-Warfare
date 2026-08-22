using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Store
{
    public sealed class CreditWallet
    {
        public string PlayerId { get; }

        public int Balance { get; private set; }

        public CreditWallet(
            string playerId,
            int startingBalance = 0)
        {
            PlayerId =
                playerId ?? string.Empty;

            Balance =
                Math.Max(0, startingBalance);
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(PlayerId);

        public bool CanSpend(
            int amount)
        {
            return amount >= 0 &&
                   amount <= Balance;
        }

        public bool Add(
            int amount)
        {
            if (amount <= 0)
                return false;

            if (Balance > int.MaxValue - amount)
                return false;

            Balance += amount;
            return true;
        }

        public bool Spend(
            int amount)
        {
            if (!CanSpend(amount))
                return false;

            Balance -= amount;
            return true;
        }

        public void SetBalance(
            int balance)
        {
            Balance =
                Math.Max(0, balance);
        }

        public void Clear()
        {
            Balance = 0;
        }
    }

    public sealed class CreditTransaction
    {
        public string TransactionId { get; }
        public string PlayerId { get; }

        public int Amount { get; }
        public bool IsCredit { get; }

        public CreditTransaction(
            string transactionId,
            string playerId,
            int amount,
            bool isCredit)
        {
            TransactionId =
                transactionId ?? string.Empty;

            PlayerId =
                playerId ?? string.Empty;

            Amount =
                Math.Max(0, amount);

            IsCredit = isCredit;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(
                TransactionId) &&
            !string.IsNullOrWhiteSpace(
                PlayerId) &&
            Amount > 0;
    }

    public sealed class CreditSystem
    {
        private readonly Dictionary<
            string,
            CreditWallet> wallets =
            new Dictionary<
                string,
                CreditWallet>(
                StringComparer.OrdinalIgnoreCase);

        private readonly List<
            CreditTransaction> transactions =
            new List<
                CreditTransaction>();

        public bool RegisterWallet(
            CreditWallet wallet)
        {
            if (wallet == null ||
                !wallet.Valid ||
                wallets.ContainsKey(
                    wallet.PlayerId))
            {
                return false;
            }

            wallets.Add(
                wallet.PlayerId,
                wallet);

            return true;
        }

        public bool RemoveWallet(
            string playerId)
        {
            if (string.IsNullOrWhiteSpace(playerId))
                return false;

            return wallets.Remove(playerId);
        }

        public bool TryGetWallet(
            string playerId,
            out CreditWallet wallet)
        {
            return wallets.TryGetValue(
                playerId,
                out wallet);
        }

        public bool GrantCredits(
            string playerId,
            int amount,
            string transactionId)
        {
            if (!wallets.TryGetValue(
                    playerId,
                    out CreditWallet wallet))
            {
                return false;
            }

            if (amount <= 0)
                return false;

            CreditTransaction transaction =
                new CreditTransaction(
                    transactionId,
                    playerId,
                    amount,
                    true);

            if (!transaction.Valid)
                return false;

            if (!wallet.Add(amount))
                return false;

            transactions.Add(transaction);

            return true;
        }

        public bool SpendCredits(
            string playerId,
            int amount,
            string transactionId)
        {
            if (!wallets.TryGetValue(
                    playerId,
                    out CreditWallet wallet))
            {
                return false;
            }

            if (amount <= 0)
                return false;

            CreditTransaction transaction =
                new CreditTransaction(
                    transactionId,
                    playerId,
                    amount,
                    false);

            if (!transaction.Valid)
                return false;

            if (!wallet.Spend(amount))
                return false;

            transactions.Add(transaction);

            return true;
        }

        public IReadOnlyCollection<
            CreditWallet>
            GetWallets()
        {
            return wallets.Values;
        }

        public IReadOnlyList<
            CreditTransaction>
            GetTransactions()
        {
            return transactions;
        }

        public void Clear()
        {
            wallets.Clear();
            transactions.Clear();
        }
    }
}
