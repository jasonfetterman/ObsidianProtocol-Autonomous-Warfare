using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniRTS
{
    /// <summary>
    /// Pure sequential queue logic. Purchases reserve both resources and supply up front.
    /// </summary>
    public sealed class ProductionQueueState
    {
        private readonly List<UnitType> entries = new List<UnitType>();
        private readonly PlayerEconomy economy;
        private readonly int capacity;
        private float elapsed;

        public int Count => entries.Count;
        public int Capacity => capacity;
        public bool IsEmpty => entries.Count == 0;
        public float CurrentProgress =>
            entries.Count == 0
                ? 0f
                : Mathf.Clamp01(
                    elapsed / UnitDefinition.Get(entries[0]).TrainingSeconds);

        public ProductionQueueState(
            PlayerEconomy factionEconomy,
            int queueCapacity = BalanceConfig.ProductionQueueCapacity)
        {
            economy = factionEconomy ??
                throw new ArgumentNullException(nameof(factionEconomy));
            if (queueCapacity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(queueCapacity));
            }

            capacity = queueCapacity;
        }

        public UnitType GetEntry(int index)
        {
            return entries[index];
        }

        public bool TryEnqueue(UnitType type)
        {
            if (entries.Count >= capacity)
            {
                return false;
            }

            UnitDefinition definition = UnitDefinition.Get(type);
            if (!economy.TryPurchase(
                    definition.MineralCost,
                    definition.GasCost,
                    definition.SupplyCost))
            {
                return false;
            }

            entries.Add(type);
            return true;
        }

        public void Advance(float deltaTime, Action<UnitType> completed)
        {
            if (deltaTime < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaTime));
            }

            elapsed += deltaTime;
            while (entries.Count > 0)
            {
                UnitType current = entries[0];
                float duration = UnitDefinition.Get(current).TrainingSeconds;
                if (elapsed < duration)
                {
                    break;
                }

                elapsed -= duration;
                entries.RemoveAt(0);
                completed?.Invoke(current);
            }

            if (entries.Count == 0)
            {
                elapsed = 0f;
            }
        }

        public void RefundAll()
        {
            for (int i = 0; i < entries.Count; i++)
            {
                UnitDefinition definition = UnitDefinition.Get(entries[i]);
                economy.RefundPurchase(
                    definition.MineralCost,
                    definition.GasCost,
                    definition.SupplyCost);
            }

            entries.Clear();
            elapsed = 0f;
        }
    }
}
