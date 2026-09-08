using System.Collections.Generic;
using NUnit.Framework;

namespace MiniRTS.Tests.EditMode
{
    public sealed class ProductionQueueTests
    {
        [Test]
        public void Enqueue_ChargesResourcesAndReservesSupply()
        {
            PlayerEconomy economy = new PlayerEconomy(0, 500, 100, 10);
            ProductionQueueState queue = new ProductionQueueState(economy);

            Assert.That(queue.TryEnqueue(UnitType.Marine), Is.True);
            Assert.That(queue.TryEnqueue(UnitType.Tank), Is.True);

            Assert.That(queue.Count, Is.EqualTo(2));
            Assert.That(economy.Minerals, Is.EqualTo(
                500 -
                BalanceConfig.MarineMineralCost -
                BalanceConfig.TankMineralCost));
            Assert.That(economy.Gas, Is.EqualTo(
                100 - BalanceConfig.TankGasCost));
            Assert.That(economy.SupplyUsed, Is.EqualTo(
                BalanceConfig.MarineSupplyCost +
                BalanceConfig.TankSupplyCost));
        }

        [Test]
        public void Enqueue_EnforcesGasSupplyAndCapacityWithoutPartialCharges()
        {
            PlayerEconomy noGas = new PlayerEconomy(0, 500, 0, 10);
            ProductionQueueState gasQueue = new ProductionQueueState(noGas);
            Assert.That(gasQueue.TryEnqueue(UnitType.Tank), Is.False);
            Assert.That(noGas.Minerals, Is.EqualTo(500));
            Assert.That(noGas.SupplyUsed, Is.Zero);

            PlayerEconomy noSupply = new PlayerEconomy(0, 500, 100, 1);
            ProductionQueueState supplyQueue = new ProductionQueueState(noSupply);
            Assert.That(supplyQueue.TryEnqueue(UnitType.Tank), Is.False);
            Assert.That(noSupply.Minerals, Is.EqualTo(500));
            Assert.That(noSupply.Gas, Is.EqualTo(100));

            PlayerEconomy capacityEconomy =
                new PlayerEconomy(0, 1000, 0, 10);
            ProductionQueueState capacityQueue =
                new ProductionQueueState(capacityEconomy, 2);
            Assert.That(capacityQueue.TryEnqueue(UnitType.Marine), Is.True);
            Assert.That(capacityQueue.TryEnqueue(UnitType.Marine), Is.True);
            int mineralsAfterTwo = capacityEconomy.Minerals;
            Assert.That(capacityQueue.TryEnqueue(UnitType.Marine), Is.False);
            Assert.That(capacityEconomy.Minerals, Is.EqualTo(mineralsAfterTwo));
        }

        [Test]
        public void Advance_ReportsProgressAndCarriesOverflowToNextOrder()
        {
            PlayerEconomy economy = new PlayerEconomy(0, 500, 0, 10);
            ProductionQueueState queue = new ProductionQueueState(economy);
            List<UnitType> completed = new List<UnitType>();
            queue.TryEnqueue(UnitType.Marine);
            queue.TryEnqueue(UnitType.Worker);

            queue.Advance(BalanceConfig.MarineTrainingSeconds * 0.25f, completed.Add);
            Assert.That(queue.CurrentProgress, Is.EqualTo(0.25f).Within(0.0001f));
            Assert.That(completed, Is.Empty);

            queue.Advance(
                BalanceConfig.MarineTrainingSeconds * 0.75f +
                BalanceConfig.WorkerTrainingSeconds * 0.5f,
                completed.Add);
            Assert.That(completed, Is.EqualTo(new[] { UnitType.Marine }));
            Assert.That(queue.Count, Is.EqualTo(1));
            Assert.That(queue.GetEntry(0), Is.EqualTo(UnitType.Worker));
            Assert.That(queue.CurrentProgress, Is.EqualTo(0.5f).Within(0.0001f));
        }

        [Test]
        public void RefundAll_ReturnsQueuedCostsAndSupply()
        {
            PlayerEconomy economy = new PlayerEconomy(0, 500, 100, 10);
            ProductionQueueState queue = new ProductionQueueState(economy);
            queue.TryEnqueue(UnitType.Marine);
            queue.TryEnqueue(UnitType.Tank);

            queue.RefundAll();

            Assert.That(queue.Count, Is.Zero);
            Assert.That(queue.CurrentProgress, Is.Zero);
            Assert.That(economy.Minerals, Is.EqualTo(500));
            Assert.That(economy.Gas, Is.EqualTo(100));
            Assert.That(economy.SupplyUsed, Is.Zero);
        }
    }
}
