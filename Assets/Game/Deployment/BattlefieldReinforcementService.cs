using System;
using ObsidianProtocol.Game.Runtime;
using ObsidianProtocol.Game.Units;
using ObsidianProtocol.Game.PersistentMilitary;
using UnityEngine;

namespace ObsidianProtocol.Game.Deployment
{
    public sealed class BattlefieldReinforcementService : MonoBehaviour
    {
        public bool RequestReinforcement(string unitId, string serviceRecordId = null)
        {
            BattlefieldDeploymentContext context = BattlefieldDeploymentContext.Instance;

            if (context == null || !context.ReinforcementsAllowed)
                return Reject("REINFORCEMENTS NOT AVAILABLE");

            BattlefieldDeploymentMaterializer materializer =
                GetComponent<BattlefieldDeploymentMaterializer>();

            if (materializer == null)
                return Reject("BATTLEFIELD MATERIALIZER NOT FOUND");

            if (string.IsNullOrWhiteSpace(unitId))
                return Reject("UNIT ID IS REQUIRED");

            UnitDefinition definition = UnitDatabaseRuntime.Get(unitId);

            if (definition == null)
                return Reject("UNIT NOT FOUND IN DATABASE // " + unitId);

            if (definition.ModelPrefab == null)
                return Reject("UNIT MODEL PREFAB NOT ASSIGNED // " + unitId);

            if (!definition.Economy.deploymentPointsDefined ||
                definition.Economy.deploymentPoints <= 0)
                return Reject("UNIT DEPLOYMENT POINT COST NOT CONFIGURED // " + unitId);

            int cost = definition.Economy.deploymentPoints;

            if (!context.CanSpendReinforcementPoints(cost))
                return Reject("INSUFFICIENT REINFORCEMENT POINTS // " + unitId);

            string deploymentId = "deployment-" + Guid.NewGuid().ToString("N");
            string resolvedServiceRecordId = "unit-" + Guid.NewGuid().ToString("N");

            DeploymentEntry entry = new DeploymentEntry(
                deploymentId,
                unitId,
                cost,
                resolvedServiceRecordId);

            if (!entry.Valid)
                return Reject("REINFORCEMENT ENTRY INVALID");

            entry.SetAvailable();
            entry.Stage();

            bool persistentRecordCreated = false;
            bool pointsSpent = false;
            bool contextEntryAdded = false;

            try
            {
                MilitaryUnitRecord record =
                    PersistentMilitaryRuntime.AddUnit(
                        resolvedServiceRecordId,
                        definition.DisplayName,
                        "PLAYER");

                if (record == null)
                    return Reject("PERSISTENT UNIT RECORD CREATION FAILED");

                persistentRecordCreated = true;

                if (!context.TrySpendReinforcementPoints(cost))
                    throw new InvalidOperationException("REINFORCEMENT BUDGET SPEND FAILED");

                pointsSpent = true;

                if (!context.AddReinforcements(new DeploymentEntry[] { entry }))
                    throw new InvalidOperationException("REINFORCEMENT CONTEXT REGISTRATION FAILED");

                contextEntryAdded = true;

                if (!materializer.MaterializeEntries(new DeploymentEntry[] { entry }))
                    throw new InvalidOperationException("REINFORCEMENT MATERIALIZATION FAILED");

                entry.Deploy();

                Debug.Log(
                    "[BATTLEFIELD REINFORCEMENT] DEPLOYED // " +
                    definition.DisplayName +
                    " // UNIT=" + unitId +
                    " // SERVICE RECORD=" + resolvedServiceRecordId +
                    " // DP=" + cost +
                    " // REMAINING=" + context.RemainingReinforcementPoints);

                return true;
            }
            catch (Exception exception)
            {
                if (contextEntryAdded)
                    context.RemoveReinforcements(new string[] { deploymentId });

                if (pointsSpent)
                    context.RefundReinforcementPoints(cost);

                if (persistentRecordCreated)
                    PersistentMilitaryRuntime.RemoveUnit(resolvedServiceRecordId);

                Debug.LogException(exception);
                return Reject("REINFORCEMENT ROLLED BACK // " + exception.Message);
            }
        }

        private static bool Reject(string reason)
        {
            Debug.LogWarning("[BATTLEFIELD REINFORCEMENT] REQUEST REJECTED // " + reason);
            return false;
        }
    }
}
