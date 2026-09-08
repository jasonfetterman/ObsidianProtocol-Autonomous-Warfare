using System;

namespace MiniRTS
{
    /// <summary>
    /// Pure per-faction resource and supply ledger.
    /// </summary>
    public sealed class PlayerEconomy
    {
        private int supplyCapacity;

        public event Action Changed;

        public int OwnerId { get; }
        public int Minerals { get; private set; }
        public int Gas { get; private set; }
        public int SupplyUsed { get; private set; }
        public int SupplyCap => ClampSupply(supplyCapacity);

        public PlayerEconomy(int ownerId, int minerals, int gas, int supplyCap = 0)
        {
            if (minerals < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minerals));
            }

            if (gas < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(gas));
            }

            OwnerId = ownerId;
            Minerals = minerals;
            Gas = gas;
            supplyCapacity = ClampSupply(supplyCap);
        }

        public bool CanAfford(int mineralCost, int gasCost, int supplyCost = 0)
        {
            ValidateNonNegative(mineralCost, nameof(mineralCost));
            ValidateNonNegative(gasCost, nameof(gasCost));
            ValidateNonNegative(supplyCost, nameof(supplyCost));
            return Minerals >= mineralCost &&
                   Gas >= gasCost &&
                   supplyCost <= SupplyCap - SupplyUsed;
        }

        public bool TrySpend(int mineralCost, int gasCost)
        {
            ValidateNonNegative(mineralCost, nameof(mineralCost));
            ValidateNonNegative(gasCost, nameof(gasCost));
            if (Minerals < mineralCost || Gas < gasCost)
            {
                return false;
            }

            Minerals -= mineralCost;
            Gas -= gasCost;
            NotifyChanged();
            return true;
        }

        public bool TryPurchase(int mineralCost, int gasCost, int supplyCost)
        {
            if (!CanAfford(mineralCost, gasCost, supplyCost))
            {
                return false;
            }

            Minerals -= mineralCost;
            Gas -= gasCost;
            SupplyUsed += supplyCost;
            NotifyChanged();
            return true;
        }

        public void Refund(int minerals, int gas)
        {
            ValidateNonNegative(minerals, nameof(minerals));
            ValidateNonNegative(gas, nameof(gas));
            Minerals = SaturatingAdd(Minerals, minerals);
            Gas = SaturatingAdd(Gas, gas);
            NotifyChanged();
        }

        public void RefundPurchase(int minerals, int gas, int supply)
        {
            ValidateNonNegative(supply, nameof(supply));
            Refund(minerals, gas);
            ReleaseSupply(supply);
        }

        public void AddResources(ResourceType type, int amount)
        {
            ValidateNonNegative(amount, nameof(amount));
            if (amount == 0)
            {
                return;
            }

            if (type == ResourceType.Minerals)
            {
                Minerals = SaturatingAdd(Minerals, amount);
            }
            else
            {
                Gas = SaturatingAdd(Gas, amount);
            }

            NotifyChanged();
        }

        public bool TryUseSupply(int amount)
        {
            ValidateNonNegative(amount, nameof(amount));
            if (amount > SupplyCap - SupplyUsed)
            {
                return false;
            }

            SupplyUsed += amount;
            NotifyChanged();
            return true;
        }

        public void ReleaseSupply(int amount)
        {
            ValidateNonNegative(amount, nameof(amount));
            SupplyUsed = Math.Max(0, SupplyUsed - amount);
            NotifyChanged();
        }

        public void AddSupplyCap(int amount)
        {
            ValidateNonNegative(amount, nameof(amount));
            supplyCapacity = SaturatingAdd(supplyCapacity, amount);
            NotifyChanged();
        }

        public void RemoveSupplyCap(int amount)
        {
            ValidateNonNegative(amount, nameof(amount));
            supplyCapacity = Math.Max(0, supplyCapacity - amount);
            NotifyChanged();
        }

        private static int ClampSupply(int value)
        {
            return Math.Max(0, Math.Min(BalanceConfig.MaximumSupply, value));
        }

        private static int SaturatingAdd(int value, int amount)
        {
            return amount > int.MaxValue - value ? int.MaxValue : value + amount;
        }

        private static void ValidateNonNegative(int value, string parameterName)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }

        private void NotifyChanged()
        {
            Changed?.Invoke();
        }
    }
}
