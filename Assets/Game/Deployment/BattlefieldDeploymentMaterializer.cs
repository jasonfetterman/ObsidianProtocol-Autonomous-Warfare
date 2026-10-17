using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ObsidianProtocol.Game.Runtime;
using ObsidianProtocol.Game.Units;

namespace ObsidianProtocol.Game.Deployment
{
    public sealed class BattlefieldDeploymentMaterializer : MonoBehaviour
    {
        private const string AreaName =
            "[SYSTEM] PLAYER DEPLOYMENT AREA";

        private const string RuntimeRootName =
            "[RUNTIME] DEPLOYED UNITS";

        private void Start()
        {
            Materialize();
        }

        public void Materialize()
        {
            BattlefieldDeploymentContext context =
                BattlefieldDeploymentContext.Instance;

            if (context == null)
            {
                Debug.LogWarning(
                    "[BATTLEFIELD DEPLOYMENT] NO DEPLOYMENT CONTEXT.");
                return;
            }

            GameObject area = GameObject.Find(AreaName);

            if (area == null)
            {
                Debug.LogError(
                    "[BATTLEFIELD DEPLOYMENT] DEPLOYMENT AREA NOT FOUND.");
                return;
            }

            GameObject root = GameObject.Find(RuntimeRootName);

            if (root == null)
                root = new GameObject(RuntimeRootName);

            ClearRuntimeUnits(root);

            IReadOnlyList<DeploymentEntry> entries =
                context.DeployedEntries;

            if (entries == null || entries.Count == 0)
            {
                Debug.LogWarning(
                    "[BATTLEFIELD DEPLOYMENT] NO DEPLOYED UNITS.");
                return;
            }

            int created = 0;

            for (int i = 0; i < entries.Count; i++)
            {
                DeploymentEntry entry = entries[i];

                if (entry == null)
                    continue;

                UnitDefinition definition =
                    UnitDatabaseRuntime.Get(entry.UnitId);

                if (definition == null)
                {
                    Debug.LogError(
                        "[BATTLEFIELD DEPLOYMENT] UNIT DEFINITION NOT FOUND: " +
                        entry.UnitId);
                    continue;
                }

                Vector3 spawnPosition =
                    area.transform.position +
                    new Vector3(i * 3f, 0f, 0f);

                if (NavMesh.SamplePosition(
                    spawnPosition,
                    out NavMeshHit hit,
                    20f,
                    NavMesh.AllAreas))
                {
                    spawnPosition =
                        hit.position +
                        new Vector3(0f, 1.2f, 0f);
                }

                GameObject unitObject =
                    GameObject.CreatePrimitive(
                        PrimitiveType.Capsule);

                unitObject.name =
                    "[UNIT] " +
                    definition.DisplayName +
                    " // " +
                    entry.DeploymentId;

                unitObject.transform.SetParent(root.transform);
                unitObject.transform.position = spawnPosition;
                unitObject.transform.localScale =
                    new Vector3(1.2f, 1.2f, 1.2f);

                UnitInstance instance =
                    unitObject.AddComponent<UnitInstance>();
                    instance.SetDefinition(definition);

            ObsidianProtocol.Game.PersistentMilitary.MilitaryUnitIdentity identity =
                unitObject.AddComponent<ObsidianProtocol.Game.PersistentMilitary.MilitaryUnitIdentity>();
            identity.Initialize(entry.ServiceRecordId, entry.UnitId);

                ObsidianProtocol.Game.Core.EntityId entityId =
                    unitObject.AddComponent<
                        ObsidianProtocol.Game.Core.EntityId>();

                SelectableUnit selectable =
                    unitObject.AddComponent<SelectableUnit>();

                NavMeshAgent agent =
                    unitObject.AddComponent<NavMeshAgent>();

                UnitAutonomy autonomy =
                    unitObject.AddComponent<UnitAutonomy>();

                WardenCommandReceiver commandReceiver =
                    unitObject.AddComponent<WardenCommandReceiver>();

                agent.speed =
                    Mathf.Max(
                        1f,
                        definition.Physical.topSpeedMetersPerSecond);

                Debug.Log(
                    "[BATTLEFIELD DEPLOYMENT] MATERIALIZED // " +
                    definition.DisplayName +
                    " // UNIT=" +
                    entry.UnitId +
                    " // DEPLOYMENT=" +
                    entry.DeploymentId);

                Debug.Log(
                    "[BATTLEFIELD DEPLOYMENT] COMMAND RECEIVER ATTACHED // " +
                    unitObject.name);

                created++;
            }

            Debug.Log(
                "[BATTLEFIELD DEPLOYMENT] MATERIALIZATION COMPLETE // " +
                created +
                " UNITS // OPERATION=" +
                context.OperationId);
        }

        private static void ClearRuntimeUnits(GameObject root)
        {
            List<GameObject> children =
                new List<GameObject>();

            for (int i = 0; i < root.transform.childCount; i++)
                children.Add(root.transform.GetChild(i).gameObject);

            foreach (GameObject child in children)
                Destroy(child);
        }
    }
}

