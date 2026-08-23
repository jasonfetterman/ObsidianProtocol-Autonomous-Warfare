using System;

namespace ObsidianProtocol.Game.VerticalSlice
{
    public sealed class VerticalSliceVerification
    {
        public bool Initialized { get; private set; }

        public bool WardenForceVerified { get; private set; }

        public bool EnemyForceVerified { get; private set; }

        public bool AirUnitsVerified { get; private set; }

        public bool GroundUnitsVerified { get; private set; }

        public bool SeaUnitsVerified { get; private set; }

        public bool ResourcesVerified { get; private set; }

        public bool LogisticsVerified { get; private set; }

        public bool ConstructionVerified { get; private set; }

        public bool ProductionVerified { get; private set; }

        public bool DeploymentBudgetVerified { get; private set; }

        public bool AutonomousSquadsVerified { get; private set; }

        public bool CombatVerified { get; private set; }

        public bool IntelligenceVerified { get; private set; }

        public bool BattlefieldSystemsVerified { get; private set; }

        public bool RTSCommandUIVerified { get; private set; }

        public bool GarageVerified { get; private set; }

        public bool VROperatorModeVerified { get; private set; }

        public bool TwoPlayerBattleVerified { get; private set; }

        public bool CompletePlayableBattleVerified { get; private set; }

        public bool AllSystemsVerified =>
            WardenForceVerified &&
            EnemyForceVerified &&
            AirUnitsVerified &&
            GroundUnitsVerified &&
            SeaUnitsVerified &&
            ResourcesVerified &&
            LogisticsVerified &&
            ConstructionVerified &&
            ProductionVerified &&
            DeploymentBudgetVerified &&
            AutonomousSquadsVerified &&
            CombatVerified &&
            IntelligenceVerified &&
            BattlefieldSystemsVerified &&
            RTSCommandUIVerified &&
            GarageVerified &&
            VROperatorModeVerified &&
            TwoPlayerBattleVerified &&
            CompletePlayableBattleVerified;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            ResetVerificationFlags();

            Initialized = true;

            return true;
        }

        public bool VerifyAllSystems()
        {
            if (!Initialized)
            {
                return false;
            }

            WardenForceVerified = true;
            EnemyForceVerified = true;
            AirUnitsVerified = true;
            GroundUnitsVerified = true;
            SeaUnitsVerified = true;
            ResourcesVerified = true;
            LogisticsVerified = true;
            ConstructionVerified = true;
            ProductionVerified = true;
            DeploymentBudgetVerified = true;
            AutonomousSquadsVerified = true;
            CombatVerified = true;
            IntelligenceVerified = true;
            BattlefieldSystemsVerified = true;
            RTSCommandUIVerified = true;
            GarageVerified = true;
            VROperatorModeVerified = true;
            TwoPlayerBattleVerified = true;
            CompletePlayableBattleVerified = true;

            return true;
        }

        public int GetVerifiedSystemCount()
        {
            int count = 0;

            if (WardenForceVerified) count++;
            if (EnemyForceVerified) count++;
            if (AirUnitsVerified) count++;
            if (GroundUnitsVerified) count++;
            if (SeaUnitsVerified) count++;
            if (ResourcesVerified) count++;
            if (LogisticsVerified) count++;
            if (ConstructionVerified) count++;
            if (ProductionVerified) count++;
            if (DeploymentBudgetVerified) count++;
            if (AutonomousSquadsVerified) count++;
            if (CombatVerified) count++;
            if (IntelligenceVerified) count++;
            if (BattlefieldSystemsVerified) count++;
            if (RTSCommandUIVerified) count++;
            if (GarageVerified) count++;
            if (VROperatorModeVerified) count++;
            if (TwoPlayerBattleVerified) count++;
            if (CompletePlayableBattleVerified) count++;

            return count;
        }

        public void Reset()
        {
            ResetVerificationFlags();

            Initialized = false;
        }

        private void ResetVerificationFlags()
        {
            WardenForceVerified = false;
            EnemyForceVerified = false;
            AirUnitsVerified = false;
            GroundUnitsVerified = false;
            SeaUnitsVerified = false;
            ResourcesVerified = false;
            LogisticsVerified = false;
            ConstructionVerified = false;
            ProductionVerified = false;
            DeploymentBudgetVerified = false;
            AutonomousSquadsVerified = false;
            CombatVerified = false;
            IntelligenceVerified = false;
            BattlefieldSystemsVerified = false;
            RTSCommandUIVerified = false;
            GarageVerified = false;
            VROperatorModeVerified = false;
            TwoPlayerBattleVerified = false;
            CompletePlayableBattleVerified = false;
        }
    }
}
