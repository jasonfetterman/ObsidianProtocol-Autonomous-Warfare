using UnityEngine;

namespace ObsidianProtocol.Game.AI.Commanders
{
    public sealed class BattlefieldEvaluationRuntime : MonoBehaviour
    {
        public static BattlefieldEvaluationRuntime Instance { get; private set; }

        public BattlefieldEvaluation Evaluation { get; private set; }

        public BattlefieldThreatLevel ThreatLevel =>
            Evaluation == null
                ? BattlefieldThreatLevel.None
                : Evaluation.ThreatLevel;

        public int FriendlyUnitCount { get; private set; }
        public int EnemyUnitCount { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            Evaluation = new BattlefieldEvaluation();

            Debug.Log("[BATTLEFIELD EVALUATION] Runtime initialized.");
        }

        private void Update()
        {
            EvaluateBattlefield();
        }

        public void EvaluateBattlefield()
        {
            if (Evaluation == null)
                Evaluation = new BattlefieldEvaluation();

            UnitAutonomy[] units =
                FindObjectsByType<UnitAutonomy>(
                    FindObjectsInactive.Exclude,
                    FindObjectsSortMode.None);

            FriendlyUnitCount = 0;
            EnemyUnitCount = 0;

            foreach (UnitAutonomy unit in units)
            {
                if (unit == null)
                    continue;

                if (unit.gameObject.CompareTag("Enemy"))
                    EnemyUnitCount++;
                else
                    FriendlyUnitCount++;
            }

            float total =
                FriendlyUnitCount + EnemyUnitCount;

            float friendlyStrength =
                total > 0f
                    ? FriendlyUnitCount / total
                    : 0f;

            float enemyStrength =
                total > 0f
                    ? EnemyUnitCount / total
                    : 0f;

            Evaluation.Reset();

            Evaluation.SetForceStrength(
                friendlyStrength,
                enemyStrength);

            Evaluation.SetTerritoryControl(
                friendlyStrength,
                enemyStrength);

            Evaluation.SetResourceSecurity(
                friendlyStrength,
                enemyStrength);

            Evaluation.SetLogisticsHealth(
                friendlyStrength,
                enemyStrength);

            Evaluation.SetReconCoverage(
                friendlyStrength,
                enemyStrength);

            Evaluation.SetMorale(
                friendlyStrength,
                enemyStrength);

            Evaluation.FinalizeEvaluation();
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}
