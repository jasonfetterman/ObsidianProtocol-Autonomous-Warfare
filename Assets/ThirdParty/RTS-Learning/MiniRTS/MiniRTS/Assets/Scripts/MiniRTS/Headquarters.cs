using System;
using UnityEngine;

namespace MiniRTS
{
    /// <summary>
    /// Worker drop-off and worker production building.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class Headquarters : Building
    {
        private ProductionQueue productionQueue;
        private bool suppliedEconomy;

        public override bool IsResourceDropoff => true;
        public ProductionQueue ProductionQueue => productionQueue;
        public int QueuedWorkers => productionQueue != null ? productionQueue.Count : 0;
        public float TrainingProgress =>
            productionQueue != null ? productionQueue.Progress : 0f;

        public void InitializeHeadquarters(
            WalkGrid walkGrid,
            PlayerEconomy factionEconomy,
            int ownerId,
            Color factionColor,
            Func<Vector3, Unit> spawnWorker)
        {
            if (spawnWorker == null)
            {
                throw new ArgumentNullException(nameof(spawnWorker));
            }

            Initialize(
                walkGrid,
                factionEconomy,
                ownerId,
                factionColor,
                BuildingType.Headquarters,
                new Vector2Int(
                    BalanceConfig.HeadquartersFootprintWidth,
                    BalanceConfig.HeadquartersFootprintDepth),
                BalanceConfig.HeadquartersHitPoints);

            productionQueue = gameObject.AddComponent<ProductionQueue>();
            productionQueue.Initialize(
                this,
                factionEconomy,
                new[] { UnitType.Worker },
                (type, position) => spawnWorker(position));
        }

        public bool TryQueueWorker()
        {
            return productionQueue != null &&
                   productionQueue.TryQueue(UnitType.Worker);
        }

        protected override void OnConstructionCompleted()
        {
            if (!suppliedEconomy && Economy != null)
            {
                Economy.AddSupplyCap(BalanceConfig.HeadquartersSupply);
                suppliedEconomy = true;
            }
        }

        protected override void OnDestroy()
        {
            if (suppliedEconomy && Economy != null)
            {
                Economy.RemoveSupplyCap(BalanceConfig.HeadquartersSupply);
                suppliedEconomy = false;
            }

            base.OnDestroy();
        }
    }
}
