using UnityEngine;

namespace MiniRTS
{
    /// <summary>
    /// Central, asset-free balance and presentation values for MiniRTS.
    /// Later milestones can extend this table without introducing ScriptableObjects.
    /// </summary>
    public static class BalanceConfig
    {
        public const int MapSize = 96;
        public const float CellSize = 1f;
        public const int RandomSeed = 1337;

        public const int PlayerOwnerId = 0;
        public const int EnemyOwnerId = 1;
        public const int StartingWorkerCount = 6;
        public const int StartingMinerals = 500;
        public const int StartingGas = 0;
        public const int MaximumSupply = 60;
        public const int HeadquartersSupply = 10;
        public const int SupplyDepotSupply = 8;

        public const float WorkerMoveSpeed = 5.25f;
        public const float WorkerTurnSpeed = 720f;
        public const float WorkerRadius = 0.42f;
        public const float WorkerGroundHeight = 0.8f;
        public const int WorkerMineralCost = 50;
        public const int WorkerGasCost = 0;
        public const int WorkerSupplyCost = 1;
        public const int WorkerHitPoints = 40;
        public const float WorkerTrainingSeconds = 12f;
        public const int WorkerAttackDamage = 4;
        public const float WorkerAttackRange = 1.15f;
        public const float WorkerAttackCooldown = 1.25f;
        public const float WorkerAggroRadius = 5f;
        public const float MarineMoveSpeed = 6f;
        public const float MarineGroundHeight = 0.95f;
        public const float MarineTurnSpeed = 720f;
        public const int MarineMineralCost = 50;
        public const int MarineGasCost = 0;
        public const int MarineSupplyCost = 1;
        public const int MarineHitPoints = 45;
        public const float MarineTrainingSeconds = 18f;
        public const int MarineAttackDamage = 6;
        public const float MarineAttackRange = 5f;
        public const float MarineAttackCooldown = 0.8f;
        public const float MarineAggroRadius = 8f;
        public const float TankMoveSpeed = 3.35f;
        public const float TankGroundHeight = 0.55f;
        public const float TankTurnSpeed = 120f;
        public const int TankMineralCost = 150;
        public const int TankGasCost = 50;
        public const int TankSupplyCost = 2;
        public const int TankHitPoints = 160;
        public const float TankTrainingSeconds = 30f;
        public const int TankAttackDamage = 30;
        public const float TankAttackRange = 7f;
        public const float TankAttackCooldown = 2.5f;
        public const float TankAggroRadius = 10f;
        public const int ProductionQueueCapacity = 5;
        public const float WaypointTolerance = 0.12f;
        public const float LocalAvoidanceRadius = 1.35f;
        public const float LocalAvoidanceStrength = 1.6f;
        public const float FormationSpacing = 1.35f;
        public const float CombatAcquireInterval = 0.2f;
        public const float CombatChaseRepathInterval = 0.35f;
        public const float AttackFacingTolerance = 8f;
        public const float TracerDuration = 0.09f;
        public const float MuzzleFlashDuration = 0.075f;
        public const float UnitDeathEffectSeconds = 0.4f;
        public const float BuildingRubbleSeconds = 1.25f;
        public const float HealthBarWidth = 1.25f;
        public const float HealthBarHeight = 0.12f;
        public const int ControlGroupCount = 5;
        public const int WorkerVisionRadius = 8;
        public const int MarineVisionRadius = 9;
        public const int TankVisionRadius = 9;
        public const int BuildingVisionRadius = 10;
        public const float FogUpdateInterval = 0.2f;
        public const float FogOverlayHeight = 0.06f;
        public const float MinimapUpdateInterval = 0.5f;
        public const float MinimapSize = 200f;
        public const int EnemyAITargetWorkerCount = 10;
        public const int EnemyAISupplyBuffer = 2;
        public const float EnemyAIThinkInterval = 0.75f;
        public const float EnemyAIFirstWaveSeconds = 90f;
        public const float EnemyAIWaveInterval = 90f;
        public const int EnemyAIInitialWaveSize = 4;
        public const int EnemyAIWaveSizeIncrease = 2;
        public const float EnemyAIBaseDefenseRadius = 16f;
        public const float EnemyAIDefenderRallyDistance = 8f;
        public const float EnemyAIDefenderLeashRadius = 5f;
        public const int EnemyAIBuildSearchMinimumRadius = 7;
        public const int EnemyAIBuildSearchMaximumRadius = 22;
        public const int EnemyAIBuildSearchSamplesPerRing = 16;

        public const float HarvestSeconds = 2f;
        public const int HarvestCarryCapacity = 5;
        public const int MineralNodeAmount = 1500;
        public const int GasGeyserAmount = 5000;
        public const float MineralInteractionRadius = 1.85f;
        public const float GeyserInteractionRadius = 2.8f;
        public const float GeyserBlockingRadius = 2f;
        public const float RefinerySnapRadius = 2.75f;
        public const float DropoffInteractionRange = 0.85f;
        public const float BuildInteractionRange = 0.75f;
        public const float BuildMoveRetrySeconds = 0.5f;

        public const int HeadquartersMineralCost = 400;
        public const int HeadquartersGasCost = 0;
        public const int HeadquartersHitPoints = 1500;
        public const int HeadquartersFootprintWidth = 6;
        public const int HeadquartersFootprintDepth = 6;
        public const float HeadquartersHeight = 3.2f;
        public const int SupplyDepotMineralCost = 100;
        public const int SupplyDepotGasCost = 0;
        public const int SupplyDepotHitPoints = 500;
        public const int SupplyDepotFootprintWidth = 3;
        public const int SupplyDepotFootprintDepth = 3;
        public const float SupplyDepotHeight = 2.2f;
        public const float SupplyDepotBuildSeconds = 20f;
        public const int BarracksMineralCost = 150;
        public const int BarracksGasCost = 0;
        public const int BarracksHitPoints = 1000;
        public const int BarracksFootprintWidth = 5;
        public const int BarracksFootprintDepth = 4;
        public const float BarracksHeight = 3f;
        public const float BarracksBuildSeconds = 35f;
        public const int FactoryMineralCost = 200;
        public const int FactoryGasCost = 50;
        public const int FactoryHitPoints = 1250;
        public const int FactoryFootprintWidth = 5;
        public const int FactoryFootprintDepth = 5;
        public const float FactoryHeight = 3.4f;
        public const float FactoryBuildSeconds = 50f;
        public const int RefineryMineralCost = 75;
        public const int RefineryGasCost = 0;
        public const int RefineryHitPoints = 750;
        public const int RefineryFootprintWidth = 4;
        public const int RefineryFootprintDepth = 4;
        public const float RefineryHeight = 2.5f;
        public const float RefineryBuildSeconds = 25f;

        public const float CameraPitch = 60f;
        public const float CameraDefaultZoom = 31f;
        public const float CameraMinZoom = 15f;
        public const float CameraMaxZoom = 48f;
        public const float CameraPanSpeed = 22f;
        public const float CameraZoomSpeed = 0.018f;
        public const float CameraZoomSharpness = 12f;
        public const float CameraEdgeSize = 14f;
        public const float CameraFieldOfView = 45f;
        public const float CameraBoundsPadding = 4f;

        public const float SelectionDragThreshold = 7f;

        public static readonly Color PlayerColor = new Color(0.12f, 0.42f, 0.95f);
        public static readonly Color EnemyColor = new Color(0.88f, 0.16f, 0.12f);
        public static readonly Color SelectionColor = new Color(0.2f, 1f, 0.28f);
        public static readonly Color MineralColor = new Color(0.1f, 0.7f, 1f);
        public static readonly Color GasColor = new Color(0.2f, 0.95f, 0.42f);

        public static int GetVisionRadius(UnitType type)
        {
            switch (type)
            {
                case UnitType.Worker:
                    return WorkerVisionRadius;
                case UnitType.Marine:
                    return MarineVisionRadius;
                case UnitType.Tank:
                    return TankVisionRadius;
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(type));
            }
        }

        public static int GetVisionRadius(BuildingType type)
        {
            switch (type)
            {
                case BuildingType.Headquarters:
                case BuildingType.SupplyDepot:
                case BuildingType.Barracks:
                case BuildingType.Factory:
                case BuildingType.Refinery:
                    return BuildingVisionRadius;
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(type));
            }
        }
    }
}
