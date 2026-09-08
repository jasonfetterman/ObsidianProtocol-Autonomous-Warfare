using NUnit.Framework;

namespace MiniRTS.Tests.EditMode
{
    public sealed class PlayerEconomyTests
    {
        [Test]
        public void SpendRefundAndPurchase_UpdateResourcesAtomically()
        {
            PlayerEconomy economy = new PlayerEconomy(0, 500, 100, 10);

            Assert.That(economy.TrySpend(75, 25), Is.True);
            Assert.That(economy.Minerals, Is.EqualTo(425));
            Assert.That(economy.Gas, Is.EqualTo(75));

            economy.Refund(25, 5);
            Assert.That(economy.Minerals, Is.EqualTo(450));
            Assert.That(economy.Gas, Is.EqualTo(80));

            Assert.That(economy.TryPurchase(50, 0, 1), Is.True);
            Assert.That(economy.Minerals, Is.EqualTo(400));
            Assert.That(economy.SupplyUsed, Is.EqualTo(1));

            Assert.That(economy.TryPurchase(1000, 0, 1), Is.False);
            Assert.That(economy.Minerals, Is.EqualTo(400));
            Assert.That(economy.SupplyUsed, Is.EqualTo(1));

            economy.RefundPurchase(50, 0, 1);
            Assert.That(economy.Minerals, Is.EqualTo(450));
            Assert.That(economy.SupplyUsed, Is.Zero);
        }

        [Test]
        public void Supply_IsClampedAndCannotBeOversubscribed()
        {
            PlayerEconomy economy = new PlayerEconomy(0, 0, 0);

            economy.AddSupplyCap(100);
            Assert.That(economy.SupplyCap, Is.EqualTo(BalanceConfig.MaximumSupply));
            Assert.That(economy.TryUseSupply(BalanceConfig.MaximumSupply + 1), Is.False);
            Assert.That(economy.TryUseSupply(BalanceConfig.MaximumSupply), Is.True);

            economy.ReleaseSupply(1000);
            Assert.That(economy.SupplyUsed, Is.Zero);
            economy.RemoveSupplyCap(1000);
            Assert.That(economy.SupplyCap, Is.Zero);
        }

        [Test]
        public void SupplyProviders_KeepOverflowCapacityBehindTheMaximum()
        {
            PlayerEconomy economy = new PlayerEconomy(0, 0, 0, 56);

            economy.AddSupplyCap(8);
            economy.AddSupplyCap(8);
            Assert.That(economy.SupplyCap, Is.EqualTo(BalanceConfig.MaximumSupply));

            economy.RemoveSupplyCap(8);
            Assert.That(economy.SupplyCap, Is.EqualTo(BalanceConfig.MaximumSupply));
            economy.RemoveSupplyCap(8);
            Assert.That(economy.SupplyCap, Is.EqualTo(56));
        }
    }
}
