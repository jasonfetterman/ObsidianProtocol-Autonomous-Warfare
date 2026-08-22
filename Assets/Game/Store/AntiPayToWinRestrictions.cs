using System;

namespace ObsidianProtocol.Game.Store
{
    public enum CompetitivePurchaseRule
    {
        Allowed,
        RequiresProgression,
        Restricted,
        Prohibited
    }

    public sealed class AntiPayToWinRestrictions
    {
        public CompetitivePurchaseRule Evaluate(
            StoreItemCategory category)
        {
            switch (category)
            {
                case StoreItemCategory.Cosmetic:
                    return CompetitivePurchaseRule.Allowed;

                case StoreItemCategory.GarageSlot:
                    return CompetitivePurchaseRule.Allowed;

                case StoreItemCategory.Convenience:
                    return CompetitivePurchaseRule.Allowed;

                case StoreItemCategory.Repair:
                    return CompetitivePurchaseRule.Allowed;

                case StoreItemCategory.FabricationMaterial:
                    return CompetitivePurchaseRule.Allowed;

                case StoreItemCategory.CampaignResource:
                    return CompetitivePurchaseRule.Allowed;

                case StoreItemCategory.Equipment:
                    return CompetitivePurchaseRule.RequiresProgression;

                case StoreItemCategory.Module:
                    return CompetitivePurchaseRule.RequiresProgression;

                case StoreItemCategory.Unit:
                    return CompetitivePurchaseRule.RequiresProgression;

                default:
                    return CompetitivePurchaseRule.Restricted;
            }
        }

        public bool IsAllowed(
            StoreItemCategory category,
            bool progressionUnlocked,
            bool competitiveMatch)
        {
            CompetitivePurchaseRule rule =
                Evaluate(category);

            switch (rule)
            {
                case CompetitivePurchaseRule.Allowed:
                    return true;

                case CompetitivePurchaseRule.RequiresProgression:
                    return progressionUnlocked &&
                           !competitiveMatch;

                case CompetitivePurchaseRule.Restricted:
                    return !competitiveMatch;

                case CompetitivePurchaseRule.Prohibited:
                    return false;

                default:
                    return false;
            }
        }

        public bool CanPurchaseForCompetitiveMatch(
            StoreItemCategory category)
        {
            switch (category)
            {
                case StoreItemCategory.Cosmetic:
                case StoreItemCategory.GarageSlot:
                case StoreItemCategory.Convenience:
                    return true;

                default:
                    return false;
            }
        }

        public bool GrantsCombatPower(
            StoreItemCategory category)
        {
            switch (category)
            {
                case StoreItemCategory.Cosmetic:
                case StoreItemCategory.GarageSlot:
                case StoreItemCategory.Convenience:
                case StoreItemCategory.CampaignResource:
                    return false;

                default:
                    return true;
            }
        }

        public bool CanBypassDeploymentBudget(
            StoreItemCategory category)
        {
            return false;
        }

        public bool CanIncreaseBattleBudget(
            StoreItemCategory category)
        {
            return false;
        }
    }
}
