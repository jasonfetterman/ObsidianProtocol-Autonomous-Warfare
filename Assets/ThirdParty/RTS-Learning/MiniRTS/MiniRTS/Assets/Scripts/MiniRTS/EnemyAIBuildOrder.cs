using System;

namespace MiniRTS
{
    public enum EnemyAIBuildAction
    {
        None,
        TrainWorker,
        BuildSupplyDepot,
        BuildBarracks,
        TrainMarine
    }

    /// <summary>
    /// Scene-independent facts consumed by the deterministic enemy build order.
    /// Queued units are included separately because their supply is already reserved.
    /// </summary>
    public readonly struct EnemyAIEconomySnapshot
    {
        public int Minerals { get; }
        public int Gas { get; }
        public int SupplyUsed { get; }
        public int SupplyCap { get; }
        public int WorkerCount { get; }
        public int QueuedWorkerCount { get; }
        public bool HeadquartersCanQueue { get; }
        public bool HasSupplyDepotUnderConstruction { get; }
        public int ConstructedBarracksCount { get; }
        public bool HasBarracksUnderConstruction { get; }
        public bool BarracksCanQueue { get; }

        public EnemyAIEconomySnapshot(
            int minerals,
            int gas,
            int supplyUsed,
            int supplyCap,
            int workerCount,
            int queuedWorkerCount,
            bool headquartersCanQueue,
            bool hasSupplyDepotUnderConstruction,
            int constructedBarracksCount,
            bool hasBarracksUnderConstruction,
            bool barracksCanQueue)
        {
            Minerals = minerals;
            Gas = gas;
            SupplyUsed = supplyUsed;
            SupplyCap = supplyCap;
            WorkerCount = workerCount;
            QueuedWorkerCount = queuedWorkerCount;
            HeadquartersCanQueue = headquartersCanQueue;
            HasSupplyDepotUnderConstruction =
                hasSupplyDepotUnderConstruction;
            ConstructedBarracksCount = constructedBarracksCount;
            HasBarracksUnderConstruction = hasBarracksUnderConstruction;
            BarracksCanQueue = barracksCanQueue;
        }
    }

    /// <summary>
    /// Pure, deterministic worker/depot/barracks/marine build-order policy.
    /// </summary>
    public static class EnemyAIBuildOrder
    {
        public static EnemyAIBuildAction Decide(
            EnemyAIEconomySnapshot snapshot)
        {
            int availableSupply = Math.Max(
                0,
                snapshot.SupplyCap - snapshot.SupplyUsed);
            bool nearSupplyCap =
                snapshot.SupplyCap < BalanceConfig.MaximumSupply &&
                availableSupply <= BalanceConfig.EnemyAISupplyBuffer;
            if (nearSupplyCap &&
                !snapshot.HasSupplyDepotUnderConstruction &&
                CanAfford(
                    snapshot,
                    BalanceConfig.SupplyDepotMineralCost,
                    BalanceConfig.SupplyDepotGasCost))
            {
                return EnemyAIBuildAction.BuildSupplyDepot;
            }

            int expectedWorkers =
                snapshot.WorkerCount + snapshot.QueuedWorkerCount;
            if (expectedWorkers < BalanceConfig.EnemyAITargetWorkerCount &&
                snapshot.HeadquartersCanQueue &&
                availableSupply >= BalanceConfig.WorkerSupplyCost &&
                CanAfford(
                    snapshot,
                    BalanceConfig.WorkerMineralCost,
                    BalanceConfig.WorkerGasCost))
            {
                return EnemyAIBuildAction.TrainWorker;
            }

            bool hasAnyBarracks =
                snapshot.ConstructedBarracksCount > 0 ||
                snapshot.HasBarracksUnderConstruction;
            if (!hasAnyBarracks &&
                CanAfford(
                    snapshot,
                    BalanceConfig.BarracksMineralCost,
                    BalanceConfig.BarracksGasCost))
            {
                return EnemyAIBuildAction.BuildBarracks;
            }

            if (snapshot.ConstructedBarracksCount > 0 &&
                snapshot.BarracksCanQueue &&
                availableSupply >= BalanceConfig.MarineSupplyCost &&
                CanAfford(
                    snapshot,
                    BalanceConfig.MarineMineralCost,
                    BalanceConfig.MarineGasCost))
            {
                return EnemyAIBuildAction.TrainMarine;
            }

            return EnemyAIBuildAction.None;
        }

        private static bool CanAfford(
            EnemyAIEconomySnapshot snapshot,
            int mineralCost,
            int gasCost)
        {
            return snapshot.Minerals >= mineralCost &&
                   snapshot.Gas >= gasCost;
        }
    }
}
