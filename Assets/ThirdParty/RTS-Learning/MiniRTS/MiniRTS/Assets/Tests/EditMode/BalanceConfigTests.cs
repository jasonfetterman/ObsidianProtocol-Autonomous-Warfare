using NUnit.Framework;

namespace MiniRTS.Tests.EditMode
{
    public sealed class BalanceConfigTests
    {
        [Test]
        public void MilestoneOneValues_AreSane()
        {
            Assert.That(BalanceConfig.MapSize, Is.EqualTo(96));
            Assert.That(BalanceConfig.CellSize, Is.GreaterThan(0f));
            Assert.That(BalanceConfig.StartingWorkerCount, Is.EqualTo(6));
            Assert.That(BalanceConfig.WorkerMoveSpeed, Is.GreaterThan(0f));
            Assert.That(BalanceConfig.WorkerRadius, Is.GreaterThan(0f));
            Assert.That(
                BalanceConfig.LocalAvoidanceRadius,
                Is.GreaterThan(BalanceConfig.WorkerRadius));
            Assert.That(
                BalanceConfig.CameraMinZoom,
                Is.LessThan(BalanceConfig.CameraDefaultZoom));
            Assert.That(
                BalanceConfig.CameraDefaultZoom,
                Is.LessThan(BalanceConfig.CameraMaxZoom));
            Assert.That(BalanceConfig.CameraPitch, Is.InRange(30f, 80f));
        }

        [Test]
        public void EconomyValues_MatchDesign()
        {
            Assert.That(BalanceConfig.WorkerMineralCost, Is.EqualTo(50));
            Assert.That(BalanceConfig.WorkerTrainingSeconds, Is.EqualTo(12f));
            Assert.That(BalanceConfig.HarvestSeconds, Is.EqualTo(2f));
            Assert.That(BalanceConfig.HarvestCarryCapacity, Is.EqualTo(5));
            Assert.That(BalanceConfig.SupplyDepotSupply, Is.EqualTo(8));
            Assert.That(BalanceConfig.MaximumSupply, Is.EqualTo(60));
            Assert.That(BalanceConfig.HeadquartersMineralCost, Is.EqualTo(400));
        }

        [Test]
        public void ProductionAndBuildingValues_MatchDesign()
        {
            Assert.That(BalanceConfig.ProductionQueueCapacity, Is.EqualTo(5));

            Assert.That(BalanceConfig.MarineMineralCost, Is.EqualTo(50));
            Assert.That(BalanceConfig.MarineGasCost, Is.Zero);
            Assert.That(BalanceConfig.MarineHitPoints, Is.GreaterThan(0));
            Assert.That(BalanceConfig.MarineMoveSpeed,
                Is.GreaterThan(BalanceConfig.TankMoveSpeed));

            Assert.That(BalanceConfig.TankMineralCost, Is.EqualTo(150));
            Assert.That(BalanceConfig.TankGasCost, Is.EqualTo(50));
            Assert.That(BalanceConfig.TankHitPoints, Is.GreaterThan(0));

            Assert.That(BalanceConfig.BarracksMineralCost, Is.EqualTo(150));
            Assert.That(BalanceConfig.FactoryMineralCost, Is.EqualTo(200));
            Assert.That(BalanceConfig.FactoryGasCost, Is.EqualTo(50));
            Assert.That(BalanceConfig.RefineryMineralCost, Is.EqualTo(75));

            Assert.That(BalanceConfig.BarracksHitPoints, Is.GreaterThan(0));
            Assert.That(BalanceConfig.FactoryHitPoints, Is.GreaterThan(0));
            Assert.That(BalanceConfig.RefineryHitPoints, Is.GreaterThan(0));
            Assert.That(BalanceConfig.BarracksBuildSeconds, Is.GreaterThan(0f));
            Assert.That(BalanceConfig.FactoryBuildSeconds, Is.GreaterThan(0f));
            Assert.That(BalanceConfig.RefineryBuildSeconds, Is.GreaterThan(0f));
        }

        [Test]
        public void CombatValues_MatchMilestoneFourBalance()
        {
            Assert.That(BalanceConfig.MarineAttackDamage, Is.EqualTo(6));
            Assert.That(BalanceConfig.MarineAttackRange, Is.EqualTo(5f));
            Assert.That(BalanceConfig.MarineAttackCooldown, Is.EqualTo(0.8f));

            Assert.That(BalanceConfig.TankAttackDamage, Is.EqualTo(30));
            Assert.That(BalanceConfig.TankAttackRange, Is.EqualTo(7f));
            Assert.That(BalanceConfig.TankAttackCooldown, Is.EqualTo(2.5f));
            Assert.That(
                BalanceConfig.TankTurnSpeed,
                Is.LessThan(BalanceConfig.MarineTurnSpeed));

            Assert.That(
                BalanceConfig.WorkerAttackRange,
                Is.LessThan(BalanceConfig.MarineAttackRange));
            Assert.That(
                BalanceConfig.WorkerAttackDamage,
                Is.LessThan(BalanceConfig.MarineAttackDamage));
        }

        [Test]
        public void FogAndMinimapValues_MatchMilestoneFiveDesign()
        {
            Assert.That(BalanceConfig.GetVisionRadius(UnitType.Worker), Is.EqualTo(8));
            Assert.That(BalanceConfig.GetVisionRadius(UnitType.Marine), Is.EqualTo(9));
            Assert.That(BalanceConfig.GetVisionRadius(UnitType.Tank), Is.EqualTo(9));
            Assert.That(
                BalanceConfig.GetVisionRadius(BuildingType.Headquarters),
                Is.EqualTo(10));
            Assert.That(BalanceConfig.FogUpdateInterval, Is.EqualTo(0.2f));
            Assert.That(BalanceConfig.MinimapUpdateInterval, Is.EqualTo(0.5f));
            Assert.That(BalanceConfig.MinimapSize, Is.EqualTo(200f));
        }
    }
}
