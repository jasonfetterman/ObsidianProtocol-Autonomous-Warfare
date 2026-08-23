#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace ObsidianProtocol.Editor
{
    public static class ObsidianWindows1819Builder
    {
        [MenuItem("Obsidian Protocol/UI/Build PHASE 18-19")]
        public static void Build()
        {
            GameObject hud = GameObject.Find("HUD");

            if (hud == null)
            {
                Debug.LogError("[UI] HUD GameObject not found in the open scene.");
                return;
            }

            Transform windows = GetOrCreate("Windows", hud.transform);
            Transform popups = GetOrCreate("Popups", hud.transform);

            // PHASE 18 — DEPLOYMENT
            Transform deployment = GetOrCreate("Deployment", windows);

            Transform core = GetOrCreate("Core", deployment);
            Transform budget = GetOrCreate("Budget", deployment);
            Transform selection = GetOrCreate("Selection", deployment);
            Transform composition = GetOrCreate("Composition", deployment);
            Transform placement = GetOrCreate("Placement", deployment);
            Transform warnings = GetOrCreate("Warnings", deployment);
            Transform ready = GetOrCreate("Ready", deployment);

            CreateNode("DEP-001 DeploymentScreen", core);

            CreateNode("DEP-002 DeploymentBudgetDisplay", budget);
            CreateNode("DEP-003 AvailableDeployment", budget);
            CreateNode("DEP-004 MaximumDeploymentPoints", budget);

            CreateNode("DEP-005 UnitSelection", selection);
            CreateNode("DEP-006 UnitDeploymentPointCost", selection);

            CreateNode("DEP-007 ArmyComposition", composition);
            CreateNode("DEP-008 SquadComposition", composition);

            CreateNode("DEP-009 DeploymentZones", placement);
            CreateNode("DEP-010 UnitPlacement", placement);

            CreateNode("DEP-011 InvalidDeploymentWarning", warnings);
            CreateNode("DEP-012 BudgetExceededWarning", warnings);

            CreateNode("DEP-013 DeploymentConfirmation", ready);
            CreateNode("DEP-014 ReadyButton", ready);
            CreateNode("DEP-015 OpponentReadyStatus", ready);
            CreateNode("DEP-016 DeploymentCountdown", ready);

            // PHASE 19 — COMBAT POPUPS
            Transform combat = GetOrCreate("Combat", popups);

            Transform detection = GetOrCreate("Detection", combat);
            Transform unit = GetOrCreate("Unit", combat);
            Transform squad = GetOrCreate("Squad", combat);
            Transform objectives = GetOrCreate("Objectives", combat);
            Transform baseFolder = GetOrCreate("Base", combat);
            Transform threat = GetOrCreate("Threat", combat);

            CreateNode("POP-001 EnemyDetected", detection);
            CreateNode("POP-002 EnemyLost", detection);
            CreateNode("POP-003 TargetAcquired", detection);

            CreateNode("POP-004 UnderAttack", unit);
            CreateNode("POP-005 UnitDamaged", unit);
            CreateNode("POP-006 UnitDestroyed", unit);

            CreateNode("POP-007 SquadDamaged", squad);
            CreateNode("POP-008 SquadDestroyed", squad);

            CreateNode("POP-009 ObjectiveAttacked", objectives);
            CreateNode("POP-010 ObjectiveCaptured", objectives);
            CreateNode("POP-011 ObjectiveLost", objectives);

            CreateNode("POP-012 BaseAttacked", baseFolder);

            CreateNode("POP-013 CriticalThreat", threat);
            CreateNode("POP-014 RetreatRecommended", threat);
            CreateNode("POP-015 ReinforcementAvailable", threat);

            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                hud.scene
            );

            Selection.activeGameObject = windows.gameObject;

            Debug.Log(
                "[UI] PHASE 18-19 hierarchy built successfully."
            );
        }

        private static Transform GetOrCreate(
            string objectName,
            Transform parent
        )
        {
            Transform existing = parent.Find(objectName);

            if (existing != null)
            {
                return existing;
            }

            GameObject obj = new GameObject(objectName);
            obj.transform.SetParent(parent, false);

            return obj.transform;
        }

        private static void CreateNode(
            string objectName,
            Transform parent
        )
        {
            if (parent.Find(objectName) != null)
            {
                return;
            }

            GameObject obj = new GameObject(objectName);
            obj.transform.SetParent(parent, false);
        }
    }
}
#endif
