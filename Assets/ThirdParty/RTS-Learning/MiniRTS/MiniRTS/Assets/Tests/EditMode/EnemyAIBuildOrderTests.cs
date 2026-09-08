using NUnit.Framework;

namespace MiniRTS.Tests.EditMode
{
    public sealed class EnemyAIBuildOrderTests
    {
        [Test]
        public void Decide_NearSupplyCapBuildsDepotBeforeMoreUnits()
        {
            EnemyAIEconomySnapshot snapshot = CreateSnapshot(
                minerals: 500,
                supplyUsed: 8,
                supplyCap: 10,
                workerCount: 6,
                queuedWorkerCount: 2);

            Assert.That(
                EnemyAIBuildOrder.Decide(snapshot),
                Is.EqualTo(EnemyAIBuildAction.BuildSupplyDepot));
        }

        [Test]
        public void Decide_QueuesWorkersUntilExpectedCountReachesTarget()
        {
            EnemyAIEconomySnapshot belowTarget = CreateSnapshot(
                minerals: 500,
                supplyUsed: 7,
                supplyCap: 18,
                workerCount: 7,
                queuedWorkerCount: 2);
            EnemyAIEconomySnapshot atTarget = CreateSnapshot(
                minerals: BalanceConfig.BarracksMineralCost,
                supplyUsed: 10,
                supplyCap: 18,
                workerCount: 8,
                queuedWorkerCount: 2);

            Assert.That(
                EnemyAIBuildOrder.Decide(belowTarget),
                Is.EqualTo(EnemyAIBuildAction.TrainWorker));
            Assert.That(
                EnemyAIBuildOrder.Decide(atTarget),
                Is.EqualTo(EnemyAIBuildAction.BuildBarracks));
        }

        [Test]
        public void Decide_BarracksConstructionPreventsDuplicateSite()
        {
            EnemyAIEconomySnapshot snapshot = CreateSnapshot(
                minerals: 500,
                supplyUsed: 10,
                supplyCap: 18,
                workerCount: 10,
                hasBarracksUnderConstruction: true);

            Assert.That(
                EnemyAIBuildOrder.Decide(snapshot),
                Is.EqualTo(EnemyAIBuildAction.None));
        }

        [Test]
        public void Decide_QueuesMarineFromCompletedBarracks()
        {
            EnemyAIEconomySnapshot snapshot = CreateSnapshot(
                minerals: BalanceConfig.MarineMineralCost,
                supplyUsed: 10,
                supplyCap: 18,
                workerCount: 10,
                constructedBarracksCount: 1,
                barracksCanQueue: true);

            Assert.That(
                EnemyAIBuildOrder.Decide(snapshot),
                Is.EqualTo(EnemyAIBuildAction.TrainMarine));
        }

        private static EnemyAIEconomySnapshot CreateSnapshot(
            int minerals,
            int supplyUsed,
            int supplyCap,
            int workerCount,
            int queuedWorkerCount = 0,
            bool hasBarracksUnderConstruction = false,
            int constructedBarracksCount = 0,
            bool barracksCanQueue = false)
        {
            return new EnemyAIEconomySnapshot(
                minerals,
                0,
                supplyUsed,
                supplyCap,
                workerCount,
                queuedWorkerCount,
                true,
                false,
                constructedBarracksCount,
                hasBarracksUnderConstruction,
                barracksCanQueue);
        }
    }
}
