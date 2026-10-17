using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace ObsidianProtocol.Game.Deployment
{
    public class DeploymentRuntimeController : MonoBehaviour
    {
        private const int MAX_DP = 10000;

        private readonly Dictionary<string, int> unitCosts =
            new Dictionary<string, int>
            {
                { "WARDEN", 1200 },
                { "BEACON", 850 },
                { "BULLDOG", 1400 },
                { "FORGE", 1700 },
                { "SENTINEL", 1100 }
            };

        private readonly Dictionary<string, string> unitIds =
            new Dictionary<string, string>
            {
                { "WARDEN", "obs-air-warden" },
                { "BEACON", "obs-air-beacon" },
                { "BULLDOG", "obs-ground-bulldog" },
                { "FORGE", "obs-ground-forge" },
                { "SENTINEL", "obs-air-sentinel" }
            };

        private DeploymentBudget deploymentBudget;
        private BattleBudget battleBudget;
        private DeploymentPointPool deploymentPoints;
        private DeploymentPointLedger pointLedger;
        private DeploymentFramework deploymentFramework;
        private DeploymentRoster deploymentRoster;
        private DeploymentStagingSystem stagingSystem;
        private UnitDeploymentCostRegistry costRegistry;
        private DeploymentZoneSystem zoneSystem;
        private DeploymentRestrictionRegistry restrictionRegistry;
        private DeploymentValidator validator;
        private ArmyComposition armyComposition;

        private DeploymentOperationSelectionController
            operationSelectionController;

        private string selectedOperation =
            "BLACK HORIZON";

        private readonly List<string> selectedUnits =
            new List<string>();

        private readonly List<string> selectedDeploymentIds =
            new List<string>();

        private readonly string[] windows =
        {
            "[WINDOW] SELECT OPERATION",
            "[WINDOW] SELECT FORCE",
            "[WINDOW] FLEET SELECTION",
            "[WINDOW] FORMATION",
            "[WINDOW] COMMAND STRUCTURE",
            "[WINDOW] COMMAND INTENT",
            "[WINDOW] RULES OF ENGAGEMENT",
            "[WINDOW] DEPLOYMENT BUDGET",
            "[WINDOW] FINAL REVIEW",
            "[HUD] UNIT INSPECTION"
        };

        private GameObject currentWindow;

        public string SelectedOperation
        {
            get
            {
                return selectedOperation;
            }
        }

        private void Awake()
        {
            operationSelectionController =
                FindFirstObjectByType<
                    DeploymentOperationSelectionController>();

            if (operationSelectionController != null)
            {
                selectedOperation =
                    operationSelectionController.GetSelectedOperation();

                Debug.Log(
                    "[DEPLOYMENT] OPERATION INITIALIZED // " +
                    selectedOperation);
            }
            else
            {
                Debug.LogWarning(
                    "[DEPLOYMENT] OPERATION SELECTION CONTROLLER NOT FOUND // " +
                    "DEFAULTING TO " +
                    selectedOperation);
            }

            InitializeDeploymentSystems();
            HideAllWindows();
            BindButtons();
            UpdateDisplays();

            Debug.Log(
                "[DEPLOYMENT] REAL DEPLOYMENT RUNTIME INITIALIZED // 10,000 DP");
        }

        private void InitializeDeploymentSystems()
        {
            deploymentBudget =
                new DeploymentBudget(MAX_DP);

            battleBudget =
                new BattleBudget(MAX_DP);

            deploymentPoints =
                new DeploymentPointPool(MAX_DP);

            pointLedger =
                new DeploymentPointLedger();

            deploymentFramework =
                new DeploymentFramework();

            deploymentRoster =
                new DeploymentRoster();

            stagingSystem =
                new DeploymentStagingSystem();

            costRegistry =
                new UnitDeploymentCostRegistry();

            zoneSystem =
                new DeploymentZoneSystem();

            zoneSystem.RegisterZone(
                new DeploymentZone(
                    "PLAYER_DEPLOYMENT",
                    DeploymentZoneType.Player));

            restrictionRegistry =
                new DeploymentRestrictionRegistry();

            armyComposition =
                new ArmyComposition();

            RegisterUnitCosts();

            validator =
                new DeploymentValidator(
                    deploymentFramework,
                    zoneSystem,
                    costRegistry);
        }

        private void RegisterUnitCosts()
        {
            foreach (KeyValuePair<string, int> pair in unitCosts)
            {
                if (!unitIds.ContainsKey(pair.Key))
                    continue;

                costRegistry.Register(
                    new UnitDeploymentCost(
                        unitIds[pair.Key],
                        pair.Value));
            }
        }

        private void BindButtons()
        {
            BindButton(
                "[BUTTON] SELECT OPERATION",
                () => OpenWindow("[WINDOW] SELECT OPERATION"));

            BindButton(
                "[BUTTON] OPEN FLEET",
                () => OpenWindow("[WINDOW] FLEET SELECTION"));

            BindButton(
                "[BUTTON] CONFIGURE FORMATION",
                () => OpenWindow("[WINDOW] FORMATION"));

            BindButton(
                "[BUTTON] CONFIGURE COMMAND",
                () => OpenWindow("[WINDOW] COMMAND STRUCTURE"));

            BindButton(
                "[BUTTON] CONFIGURE INTENT",
                () => OpenWindow("[WINDOW] COMMAND INTENT"));

            BindButton(
                "[BUTTON] CONFIGURE ROE",
                () => OpenWindow("[WINDOW] RULES OF ENGAGEMENT"));

            BindButton(
                "[BUTTON] FINAL REVIEW",
                () => OpenWindow("[WINDOW] FINAL REVIEW"));

            BindButton(
                "[BUTTON] WARDEN",
                () => AddUnit("WARDEN"));

            BindButton(
                "[BUTTON] BEACON",
                () => AddUnit("BEACON"));

            BindButton(
                "[BUTTON] BULLDOG",
                () => AddUnit("BULLDOG"));

            BindButton(
                "[BUTTON] FORGE",
                () => AddUnit("FORGE"));

            BindButton(
                "[BUTTON] SENTINEL",
                () => AddUnit("SENTINEL"));

            BindButton(
                "[BUTTON] ADD UNIT",
                AddFirstAffordableUnit);

            BindButton(
                "[BUTTON] REMOVE UNIT",
                RemoveLastUnit);

            BindButton(
                "[BUTTON] CLEAR FORCE",
                ClearForce);

            BindButton(
                "[BUTTON] FINAL REVIEW",
                () => OpenWindow("[WINDOW] FINAL REVIEW"));

            BindButton(
                "CONFIRM DEPLOYMENT",
                Deploy);

            BindButton(
                "[BUTTON] DEPLOY",
                Deploy);

            BindButton(
                "CANCEL DEPLOYMENT",
                CancelDeployment);

            BindButton(
                "[BUTTON] BACK",
                Back);

            BindButton(
                "[BUTTON] CLOSE MAP",
                CloseCurrentWindow);

            BindButton(
                "CLOSE INSPECTION",
                CloseCurrentWindow);

            BindButton(
                "CLOSE FLEET",
                CloseCurrentWindow);

            BindButton(
                "CLOSE REVIEW",
                CloseCurrentWindow);

            BindButton(
                "[BUTTON] SAVE LOADOUT",
                SaveLoadout);

            BindButton(
                "[BUTTON] UNIT DETAILS",
                OpenUnitInspection);
        }

        private void AddUnit(string displayName)
        {
            if (!unitCosts.ContainsKey(displayName))
                return;

            int cost = unitCosts[displayName];

            if (!deploymentBudget.CanSpend(cost))
            {
                Debug.LogWarning(
                    "[DEPLOYMENT] INSUFFICIENT DP // " +
                    displayName);
                return;
            }

            string unitId = unitIds[displayName];

            string deploymentId =
                "deployment-" +
                Guid.NewGuid().ToString("N");

            string serviceRecordId =
                "unit-" +
                Guid.NewGuid().ToString("N");

            DeploymentEntry entry =
                new DeploymentEntry(
                    deploymentId,
                    unitId,
                    cost,
                    serviceRecordId);

            if (!deploymentRoster.TryAdd(
                deploymentBudget,
                entry))
            {
                Debug.LogWarning(
                    "[DEPLOYMENT] ROSTER REJECTED // " +
                    displayName);
                return;
            }

            ObsidianProtocol.Game.PersistentMilitary.PersistentMilitaryRuntime.AddUnit(
                serviceRecordId,
                displayName);

            deploymentFramework.Register(entry);
            deploymentFramework.SetAvailable(deploymentId);

            deploymentPoints.TrySpend(cost);

            pointLedger.Allocate(
                deploymentId,
                cost,
                deploymentPoints);

            stagingSystem.Register(
                new DeploymentStagingEntry(
                    deploymentId,
                    "PLAYER_DEPLOYMENT"));

            selectedUnits.Add(displayName);
            selectedDeploymentIds.Add(deploymentId);

            armyComposition.AddUnit(unitId);

            UpdateDisplays();

            Debug.Log(
                "[DEPLOYMENT] ADDED // " +
                displayName +
                " // " +
                cost +
                " DP");
        }

        private void AddFirstAffordableUnit()
        {
            foreach (KeyValuePair<string, int> pair in unitCosts)
            {
                if (deploymentBudget.CanSpend(pair.Value))
                {
                    AddUnit(pair.Key);
                    return;
                }
            }
        }

        private void RemoveLastUnit()
        {
            if (selectedDeploymentIds.Count == 0)
                return;

            int index =
                selectedDeploymentIds.Count - 1;

            string deploymentId =
                selectedDeploymentIds[index];

            DeploymentEntry entry;

            if (deploymentFramework.TryGet(
                deploymentId,
                out entry))
            {
                deploymentRoster.Remove(
                    deploymentBudget,
                    entry);

                deploymentFramework.Remove(
                    deploymentId);

                deploymentPoints.Refund(
                    entry.DeploymentPoints);

                pointLedger.Release(
                    deploymentId,
                    deploymentPoints);

                stagingSystem.Remove(
                    deploymentId);

                armyComposition.RemoveUnit(
                    entry.UnitId);
            }

            selectedDeploymentIds.RemoveAt(index);
            selectedUnits.RemoveAt(index);

            UpdateDisplays();

            Debug.Log(
                "[DEPLOYMENT] REMOVED LAST UNIT");
        }

        private void ClearForce()
        {
            for (int i = selectedDeploymentIds.Count - 1;
                 i >= 0;
                 i--)
            {
                string deploymentId =
                    selectedDeploymentIds[i];

                DeploymentEntry entry;

                if (deploymentFramework.TryGet(
                    deploymentId,
                    out entry))
                {
                    deploymentFramework.Remove(
                        deploymentId);

                    stagingSystem.Remove(
                        deploymentId);
                }
            }

            deploymentRoster.Clear(
                deploymentBudget);

            pointLedger.Clear(
                deploymentPoints);

            deploymentPoints.Reset();

            deploymentFramework.Clear();
            stagingSystem.Clear();
            armyComposition.Clear();

            selectedUnits.Clear();
            selectedDeploymentIds.Clear();

            UpdateDisplays();

            Debug.Log(
                "[DEPLOYMENT] FORCE CLEARED // 10,000 DP");
        }

        private void Deploy()
        {
            if (operationSelectionController != null)
            {
                selectedOperation =
                    operationSelectionController.GetSelectedOperation();
            }

            Debug.Log(
                "[DEPLOYMENT] DEPLOYMENT OPERATION // " +
                selectedOperation);

            if (selectedDeploymentIds.Count == 0)
            {
                Debug.LogWarning(
                    "[DEPLOYMENT] CANNOT DEPLOY // FORCE EMPTY");
                return;
            }

            foreach (string deploymentId
                     in selectedDeploymentIds)
            {
                DeploymentEntry entry;

                if (!deploymentFramework.TryGet(
                    deploymentId,
                    out entry))
                {
                    Debug.LogError(
                        "[DEPLOYMENT] VALIDATION FAILED // ENTRY MISSING");
                    return;
                }

                DeploymentValidationResult result =
                    validator.Validate(
                        deploymentId,
                        "PLAYER_DEPLOYMENT",
                        entry.UnitId,
                        battleBudget,
                        deploymentPoints);

                if (!result.Valid)
                {
                    Debug.LogWarning(
                        "[DEPLOYMENT] VALIDATION FAILED // " +
                        result.Failure);
                    return;
                }

                deploymentFramework.Stage(
                    deploymentId);

                stagingSystem.Stage(
                    deploymentId);

                stagingSystem.MarkReady(
                    deploymentId);
            }

            foreach (string deploymentId
                     in selectedDeploymentIds)
            {
                deploymentFramework.Deploy(
                    deploymentId);

                stagingSystem.Deploy(
                    deploymentId);
            }

            Debug.Log(
                "[DEPLOYMENT] DEPLOYMENT COMPLETE // " +
                selectedUnits.Count +
                " UNITS // OPERATION=" +
                selectedOperation);

        

            ObsidianProtocol.Game.Deployment.BattlefieldDeploymentContext context =
                ObsidianProtocol.Game.Deployment.BattlefieldDeploymentContext.Instance;

            if (context == null)
            {
                GameObject contextObject =
                    new GameObject("[SYSTEM] BATTLEFIELD DEPLOYMENT CONTEXT");

                context =
                    contextObject.AddComponent<
                        ObsidianProtocol.Game.Deployment.BattlefieldDeploymentContext>();
            }

            List<DeploymentEntry> battlefieldEntries =
                new List<DeploymentEntry>();

            foreach (string deploymentId in selectedDeploymentIds)
            {
                DeploymentEntry battlefieldEntry;

                if (deploymentFramework.TryGet(
                    deploymentId,
                    out battlefieldEntry))
                {
                    battlefieldEntries.Add(battlefieldEntry);
                }
            }

            context.SetDeployment(
                selectedOperation,
                battlefieldEntries);

            Debug.Log(
                "[DEPLOYMENT] BATTLEFIELD CONTEXT HANDOFF COMPLETE // " +
                battlefieldEntries.Count +
                " UNITS");
    TryOpenBattlefield();
        }

        public string GetSelectedOperation()
        {
            return selectedOperation;
        }

        private void CancelDeployment()
        {
            ClearForce();
            CloseCurrentWindow();

            Debug.Log(
                "[DEPLOYMENT] DEPLOYMENT CANCELLED");
        }

        private void SaveLoadout()
        {
            Debug.Log(
                "[DEPLOYMENT] LOADOUT SAVE REQUESTED // " +
                selectedUnits.Count +
                " UNITS");
        }

        private void OpenUnitInspection()
        {
            Debug.Log(
                "[DEPLOYMENT] UNIT INSPECTION REQUESTED");

            OpenWindow("[HUD] UNIT INSPECTION");
        }

        private void Back()
        {
            if (currentWindow != null)
            {
                CloseCurrentWindow();

                Debug.Log(
                    "[DEPLOYMENT] BACK // CURRENT WINDOW CLOSED");

                return;
            }

            GameObject hud =
                FindSceneObject("[HUD] DEPLOYMENT HUD");

            if (hud != null)
            {
                hud.SetActive(false);

                DeploymentTerminalInteraction terminal =
                    FindFirstObjectByType<DeploymentTerminalInteraction>();

                if (terminal != null)
                    terminal.CloseDeploymentHUD();

                Debug.Log(
                    "[DEPLOYMENT] BACK // DEPLOYMENT HUD CLOSED");
            }
        }

        private void TryOpenBattlefield()
        {
            string[] scenePaths =
            {
                "Assets/Scenes/SCN03 _BATTLEFIELD/[HUD] BATTLEFIELD HUD/Battlefield_HUD.unity",
                "Battlefield_HUD"
            };

            foreach (string scenePath in scenePaths)
            {
                if (Application.CanStreamedLevelBeLoaded(scenePath))
                {
                    SceneManager.LoadScene(scenePath);
                    return;
                }

                if (Application.CanStreamedLevelBeLoaded(
                    System.IO.Path.GetFileNameWithoutExtension(
                        scenePath)))
                {
                    SceneManager.LoadScene(
                        System.IO.Path.GetFileNameWithoutExtension(
                            scenePath));
                    return;
                }
            }

            Debug.LogWarning(
                "[DEPLOYMENT] BATTLEFIELD SCENE NOT FOUND IN BUILD SETTINGS");
        }

        private void HideAllWindows()
        {
            foreach (string windowName in windows)
            {
                GameObject window =
                    FindSceneObject(windowName);

                if (window != null)
                    window.SetActive(false);
            }

            currentWindow = null;
        }

        private void OpenWindow(string windowName)
        {
            HideAllWindows();

            GameObject window =
                FindSceneObject(windowName);

            if (window == null)
            {
                Debug.LogWarning(
                    "[DEPLOYMENT] WINDOW NOT FOUND // " +
                    windowName);
                return;
            }

            window.SetActive(true);
            currentWindow = window;

            Debug.Log(
                "[DEPLOYMENT] OPENED // " +
                windowName);
        }

        private void CloseCurrentWindow()
        {
            if (currentWindow != null)
                currentWindow.SetActive(false);

            currentWindow = null;
        }

        private void BindButton(
            string objectName,
            UnityEngine.Events.UnityAction action)
        {
            GameObject obj =
                FindSceneObject(objectName);

            if (obj == null)
            {
                Debug.LogWarning(
                    "[DEPLOYMENT] BUTTON NOT FOUND // " +
                    objectName);
                return;
            }

            Button button =
                obj.GetComponent<Button>();

            if (button == null)
            {
                Debug.LogWarning(
                    "[DEPLOYMENT] NO BUTTON COMPONENT // " +
                    objectName);
                return;
            }

            button.onClick.AddListener(action);
        }

        private GameObject FindSceneObject(
            string objectName)
        {
            GameObject[] objects =
                UnityEngine.Resources
                    .FindObjectsOfTypeAll<GameObject>();

            GameObject best = null;
            int bestScore = int.MinValue;

            foreach (GameObject obj in objects)
            {
                if (obj.name != objectName)
                    continue;

                if (obj.hideFlags != HideFlags.None)
                    continue;

                int score = 0;

                RectTransform rect =
                    obj.GetComponent<RectTransform>();

                if (rect != null)
                {
                    if (rect.parent != null)
                        score += 100;

                    if (rect.childCount > 0)
                        score += 100;

                    if (rect.anchorMin !=
                        new Vector2(0.5f, 0.5f))
                        score += 50;

                    if (rect.anchorMax !=
                        new Vector2(0.5f, 0.5f))
                        score += 50;
                }

                if (score > bestScore)
                {
                    bestScore = score;
                    best = obj;
                }
            }

            return best;
        }

        private void UpdateDisplays()
        {
            int remaining =
                deploymentBudget != null
                    ? deploymentBudget.RemainingPoints
                    : MAX_DP;

            SetText(
                "[STAT] 10,000 / 10,000",
                remaining.ToString("N0") +
                " / " +
                MAX_DP.ToString("N0"));

            SetText(
                "REMAINING BUDGET",
                remaining.ToString("N0") +
                " DP");

            SetText(
                "BUDGET SUMMARY",
                remaining.ToString("N0") +
                " DP REMAINING");

            SetText(
                "UNIT COUNT // 0",
                "UNIT COUNT // " +
                selectedUnits.Count);

            SetText(
                "UNIT COST VALUE",
                selectedUnits.Count > 0
                    ? unitCosts[selectedUnits[
                        selectedUnits.Count - 1]]
                        .ToString("N0") + " DP"
                    : "0 DP");
        }

        private void SetText(
            string objectName,
            string value)
        {
            GameObject obj =
                FindSceneObject(objectName);

            if (obj == null)
                return;

            TMP_Text tmp =
                obj.GetComponent<TMP_Text>();

            if (tmp != null)
            {
                tmp.text = value;
                return;
            }

            Text legacy =
                obj.GetComponent<Text>();

            if (legacy != null)
                legacy.text = value;
        }
    }
}










