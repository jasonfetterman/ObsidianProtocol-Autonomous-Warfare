using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Multiplayer.Online
{
    public enum OnlineProductionState
    {
        Queued,
        Producing,
        Completed,
        Cancelled
    }

    public sealed class OnlineProductionItem
    {
        public string ProductionId { get; }

        public string OwnerPlayerId { get; private set; }

        public string UnitId { get; private set; }

        public int Quantity { get; private set; }

        public float Progress { get; private set; }

        public OnlineProductionState State { get; private set; }

        public long LastUpdateTick { get; private set; }

        public OnlineProductionItem(
            string productionId,
            string ownerPlayerId,
            string unitId,
            int quantity)
        {
            ProductionId =
                productionId ?? string.Empty;

            OwnerPlayerId =
                ownerPlayerId ?? string.Empty;

            UnitId =
                unitId ?? string.Empty;

            Quantity =
                Math.Max(0, quantity);

            State =
                OnlineProductionState.Queued;
        }

        public bool Update(
            string ownerPlayerId,
            string unitId,
            int quantity,
            float progress,
            OnlineProductionState state,
            long tick)
        {
            if (string.IsNullOrWhiteSpace(ProductionId) ||
                string.IsNullOrWhiteSpace(UnitId) ||
                quantity < 0)
            {
                return false;
            }

            OwnerPlayerId =
                ownerPlayerId ?? string.Empty;

            UnitId =
                unitId ?? string.Empty;

            Quantity = quantity;

            Progress =
                Math.Max(
                    0f,
                    Math.Min(1f, progress));

            State = state;
            LastUpdateTick = tick;

            return true;
        }
    }

    public sealed class OnlineProductionSynchronization
    {
        private readonly Dictionary<
            string,
            OnlineProductionItem> productionItems =
            new Dictionary<
                string,
                OnlineProductionItem>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ProductionCount =>
            productionItems.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            productionItems.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterProduction(
            string productionId,
            string ownerPlayerId,
            string unitId,
            int quantity)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(productionId) ||
                string.IsNullOrWhiteSpace(unitId) ||
                quantity <= 0)
            {
                return false;
            }

            string id =
                productionId.Trim();

            if (productionItems.ContainsKey(id))
            {
                return false;
            }

            productionItems.Add(
                id,
                new OnlineProductionItem(
                    id,
                    ownerPlayerId,
                    unitId,
                    quantity));

            return true;
        }

        public bool SynchronizeProduction(
            string productionId,
            string ownerPlayerId,
            string unitId,
            int quantity,
            float progress,
            OnlineProductionState state,
            long tick)
        {
            OnlineProductionItem item =
                GetProduction(productionId);

            return item != null &&
                   item.Update(
                       ownerPlayerId,
                       unitId,
                       quantity,
                       progress,
                       state,
                       tick);
        }

        public OnlineProductionItem GetProduction(
            string productionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(productionId))
            {
                return null;
            }

            productionItems.TryGetValue(
                productionId.Trim(),
                out OnlineProductionItem item);

            return item;
        }

        public bool RemoveProduction(
            string productionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(productionId))
            {
                return false;
            }

            return productionItems.Remove(
                productionId.Trim());
        }

        public IReadOnlyCollection<
            OnlineProductionItem>
            GetProductions()
        {
            return productionItems.Values;
        }

        public void Reset()
        {
            productionItems.Clear();
            Initialized = false;
        }
    }
}
