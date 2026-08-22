using System;

namespace ObsidianProtocol.Game.Store
{
    public enum PurchaseValidationResult
    {
        Valid,
        InvalidPlayer,
        InvalidItem,
        ItemUnavailable,
        InsufficientCredits,
        InvalidCost,
        AlreadyOwned,
        ProgressionLocked,
        CompetitiveRestriction
    }

    public sealed class PurchaseValidation
    {
        public PurchaseValidationResult Result
        {
            get;
            private set;
        }

        public bool Approved =>
            Result ==
            PurchaseValidationResult.Valid;

        public string Message
        {
            get;
            private set;
        }

        private PurchaseValidation(
            PurchaseValidationResult result,
            string message)
        {
            Result = result;
            Message = message ?? string.Empty;
        }

        public static PurchaseValidation Valid()
        {
            return new PurchaseValidation(
                PurchaseValidationResult.Valid,
                "Purchase approved.");
        }

        public static PurchaseValidation Reject(
            PurchaseValidationResult result,
            string message)
        {
            return new PurchaseValidation(
                result,
                message);
        }
    }

    public sealed class PurchaseValidator
    {
        public PurchaseValidation Validate(
            string playerId,
            StoreItem item,
            CreditWallet wallet)
        {
            if (string.IsNullOrWhiteSpace(playerId))
            {
                return PurchaseValidation.Reject(
                    PurchaseValidationResult.InvalidPlayer,
                    "Player ID is invalid.");
            }

            if (item == null ||
                !item.Valid)
            {
                return PurchaseValidation.Reject(
                    PurchaseValidationResult.InvalidItem,
                    "Store item is invalid.");
            }

            if (!item.Purchasable)
            {
                return PurchaseValidation.Reject(
                    PurchaseValidationResult.ItemUnavailable,
                    "Store item is unavailable.");
            }

            if (item.CreditCost < 0)
            {
                return PurchaseValidation.Reject(
                    PurchaseValidationResult.InvalidCost,
                    "Store item has an invalid cost.");
            }

            if (wallet == null ||
                !wallet.Valid)
            {
                return PurchaseValidation.Reject(
                    PurchaseValidationResult.InvalidPlayer,
                    "Credit wallet is invalid.");
            }

            if (!string.Equals(
                    playerId,
                    wallet.PlayerId,
                    StringComparison.OrdinalIgnoreCase))
            {
                return PurchaseValidation.Reject(
                    PurchaseValidationResult.InvalidPlayer,
                    "Credit wallet does not belong to player.");
            }

            if (!wallet.CanSpend(
                    item.CreditCost))
            {
                return PurchaseValidation.Reject(
                    PurchaseValidationResult.InsufficientCredits,
                    "Insufficient credits.");
            }

            return PurchaseValidation.Valid();
        }

        public PurchaseValidation ValidateCredits(
            string playerId,
            int creditCost,
            CreditWallet wallet)
        {
            if (string.IsNullOrWhiteSpace(playerId))
            {
                return PurchaseValidation.Reject(
                    PurchaseValidationResult.InvalidPlayer,
                    "Player ID is invalid.");
            }

            if (creditCost < 0)
            {
                return PurchaseValidation.Reject(
                    PurchaseValidationResult.InvalidCost,
                    "Credit cost is invalid.");
            }

            if (wallet == null ||
                !wallet.Valid)
            {
                return PurchaseValidation.Reject(
                    PurchaseValidationResult.InvalidPlayer,
                    "Credit wallet is invalid.");
            }

            if (!string.Equals(
                    playerId,
                    wallet.PlayerId,
                    StringComparison.OrdinalIgnoreCase))
            {
                return PurchaseValidation.Reject(
                    PurchaseValidationResult.InvalidPlayer,
                    "Credit wallet does not belong to player.");
            }

            if (!wallet.CanSpend(creditCost))
            {
                return PurchaseValidation.Reject(
                    PurchaseValidationResult.InsufficientCredits,
                    "Insufficient credits.");
            }

            return PurchaseValidation.Valid();
        }

        public PurchaseValidation ValidateProgression(
            bool unlocked)
        {
            if (!unlocked)
            {
                return PurchaseValidation.Reject(
                    PurchaseValidationResult.ProgressionLocked,
                    "Required progression has not been completed.");
            }

            return PurchaseValidation.Valid();
        }

        public PurchaseValidation ValidateCompetitiveRestriction(
            bool allowed)
        {
            if (!allowed)
            {
                return PurchaseValidation.Reject(
                    PurchaseValidationResult.CompetitiveRestriction,
                    "This purchase cannot be used to bypass competitive restrictions.");
            }

            return PurchaseValidation.Valid();
        }
    }
}
