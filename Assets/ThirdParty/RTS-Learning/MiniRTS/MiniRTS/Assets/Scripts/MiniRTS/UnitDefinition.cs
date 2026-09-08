using System;

namespace MiniRTS
{
    /// <summary>
    /// Immutable unit production and movement values sourced from BalanceConfig.
    /// </summary>
    public readonly struct UnitDefinition
    {
        public UnitType Type { get; }
        public string DisplayName { get; }
        public int MineralCost { get; }
        public int GasCost { get; }
        public int SupplyCost { get; }
        public int HitPoints { get; }
        public float TrainingSeconds { get; }
        public float MoveSpeed { get; }
        public float GroundHeight { get; }
        public float TurnSpeed { get; }
        public CombatStats Combat { get; }

        public UnitDefinition(
            UnitType type,
            string displayName,
            int mineralCost,
            int gasCost,
            int supplyCost,
            int hitPoints,
            float trainingSeconds,
            float moveSpeed,
            float groundHeight,
            float turnSpeed,
            CombatStats combat)
        {
            Type = type;
            DisplayName = displayName;
            MineralCost = mineralCost;
            GasCost = gasCost;
            SupplyCost = supplyCost;
            HitPoints = hitPoints;
            TrainingSeconds = trainingSeconds;
            MoveSpeed = moveSpeed;
            GroundHeight = groundHeight;
            TurnSpeed = turnSpeed;
            Combat = combat;
        }

        public static UnitDefinition Get(UnitType type)
        {
            switch (type)
            {
                case UnitType.Worker:
                    return new UnitDefinition(
                        type,
                        "Worker",
                        BalanceConfig.WorkerMineralCost,
                        BalanceConfig.WorkerGasCost,
                        BalanceConfig.WorkerSupplyCost,
                        BalanceConfig.WorkerHitPoints,
                        BalanceConfig.WorkerTrainingSeconds,
                        BalanceConfig.WorkerMoveSpeed,
                        BalanceConfig.WorkerGroundHeight,
                        BalanceConfig.WorkerTurnSpeed,
                        new CombatStats(
                            BalanceConfig.WorkerAttackDamage,
                            BalanceConfig.WorkerAttackRange,
                            BalanceConfig.WorkerAttackCooldown,
                            BalanceConfig.WorkerAggroRadius));
                case UnitType.Marine:
                    return new UnitDefinition(
                        type,
                        "Marine",
                        BalanceConfig.MarineMineralCost,
                        BalanceConfig.MarineGasCost,
                        BalanceConfig.MarineSupplyCost,
                        BalanceConfig.MarineHitPoints,
                        BalanceConfig.MarineTrainingSeconds,
                        BalanceConfig.MarineMoveSpeed,
                        BalanceConfig.MarineGroundHeight,
                        BalanceConfig.MarineTurnSpeed,
                        new CombatStats(
                            BalanceConfig.MarineAttackDamage,
                            BalanceConfig.MarineAttackRange,
                            BalanceConfig.MarineAttackCooldown,
                            BalanceConfig.MarineAggroRadius));
                case UnitType.Tank:
                    return new UnitDefinition(
                        type,
                        "Tank",
                        BalanceConfig.TankMineralCost,
                        BalanceConfig.TankGasCost,
                        BalanceConfig.TankSupplyCost,
                        BalanceConfig.TankHitPoints,
                        BalanceConfig.TankTrainingSeconds,
                        BalanceConfig.TankMoveSpeed,
                        BalanceConfig.TankGroundHeight,
                        BalanceConfig.TankTurnSpeed,
                        new CombatStats(
                            BalanceConfig.TankAttackDamage,
                            BalanceConfig.TankAttackRange,
                            BalanceConfig.TankAttackCooldown,
                            BalanceConfig.TankAggroRadius));
                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            }
        }
    }

    /// <summary>
    /// Immutable offensive values sourced from BalanceConfig.
    /// </summary>
    public readonly struct CombatStats
    {
        public int Damage { get; }
        public float Range { get; }
        public float CooldownSeconds { get; }
        public float AggroRadius { get; }

        public CombatStats(
            int damage,
            float range,
            float cooldownSeconds,
            float aggroRadius)
        {
            if (damage <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(damage));
            }

            if (range <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(range));
            }

            if (cooldownSeconds <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(cooldownSeconds));
            }

            if (aggroRadius < range)
            {
                throw new ArgumentOutOfRangeException(nameof(aggroRadius));
            }

            Damage = damage;
            Range = range;
            CooldownSeconds = cooldownSeconds;
            AggroRadius = aggroRadius;
        }
    }
}
