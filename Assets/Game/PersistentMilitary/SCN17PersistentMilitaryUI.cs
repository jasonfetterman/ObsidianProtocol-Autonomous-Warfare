using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace ObsidianProtocol.Game.PersistentMilitary
{
    public sealed class SCN17PersistentMilitaryUI : MonoBehaviour
    {
        private readonly Dictionary<string, Button> buttons =
            new Dictionary<string, Button>(
                StringComparer.OrdinalIgnoreCase);

        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Install()
        {
            Scene scene = SceneManager.GetActiveScene();

            if (!string.Equals(
                    scene.name,
                    "Persistent_Military",
                    StringComparison.OrdinalIgnoreCase))
                return;

            GameObject root =
                GameObject.Find("PERSISTENT MILITARY HUD");

            if (root == null)
                root =
                    GameObject.Find("16. PERSISTENT MILITARY");

            if (root == null)
                return;

            if (root.GetComponent<SCN17PersistentMilitaryUI>() == null)
                root.AddComponent<SCN17PersistentMilitaryUI>();
        }

        private void Awake()
        {
            PersistentMilitaryRuntime.Initialize();

            WireButtons();
            RefreshAll();
        }

        private void OnEnable()
        {
            RefreshAll();
        }

        private void WireButtons()
        {
            Button[] found =
                FindObjectsByType<Button>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None);

            foreach (Button button in found)
            {
                if (button == null)
                    continue;

                string name =
                    button.gameObject.name;

                if (!name.StartsWith("BUTTON - ",
                        StringComparison.OrdinalIgnoreCase))
                    continue;

                buttons[name] = button;

                button.onClick.RemoveListener(
                    OnButtonClicked);

                button.onClick.AddListener(
                    OnButtonClicked);
            }
        }

        private void OnButtonClicked()
        {
            GameObject selected =
                UnityEngine.EventSystems.EventSystem.current?
                    .currentSelectedGameObject;

            if (selected == null)
                return;

            HandleButton(selected.name);
        }

        private void HandleButton(string buttonName)
        {
            switch (buttonName)
            {
                case "BUTTON - UNIT HISTORY":
                    ShowUnitHistory();
                    break;

                case "BUTTON - AFTER ACTION":
                    ShowAfterAction();
                    break;

                case "BUTTON - OPERATIONS":
                    ShowOperations();
                    break;

                case "BUTTON - CLOSE":
                    CloseDetail();
                    break;

                case "BUTTON - MILITARY DEVELOPMENT":
                    PersistentMilitaryRuntime
                        .DevelopMilitary(100);
                    RefreshAll();
                    break;

                case "BUTTON - GARAGE":
                    Debug.Log(
                        "[SCN-17] GARAGE navigation requested.");
                    break;

                case "BUTTON - LOSSES":
                    ShowLosses();
                    break;

                case "BUTTON - VETERAN UNITS":
                    ShowVeterans();
                    break;

                case "BUTTON - OPERATIONAL HISTORY":
                    ShowOperationalHistory();
                    break;

                case "BUTTON - LOGISTICS":
                    Debug.Log(
                        "[SCN-17] LOGISTICS navigation requested.");
                    break;

                case "BUTTON - VIEW FULL RECORD":
                    ShowFullRecord();
                    break;

                case "BUTTON - RESEARCH":
                    Debug.Log(
                        "[SCN-17] RESEARCH navigation requested.");
                    break;

                case "BUTTON - EQUIPMENT HISTORY":
                    ShowEquipmentHistory();
                    break;

                case "BUTTON - REPAIRS":
                    RepairAll();
                    RefreshAll();
                    break;

                case "BUTTON - COMMAND CENTER":
                    Debug.Log(
                        "[SCN-17] COMMAND CENTER navigation requested.");
                    break;

                case "BUTTON - DAMAGE HISTORY":
                    ShowDamageHistory();
                    break;
            }
        }

        private void RefreshAll()
        {
            PersistentMilitarySaveData data =
                PersistentMilitaryRuntime.Data;

            if (data == null)
                return;

            SetScreen(
                "UNIT HISTORY TERMINAL A SCREEN",
                "UNIT HISTORY\n\n" +
                "REGISTERED UNITS     " +
                data.Units.Count +
                "\nOPERATIONS           " +
                data.TotalOperations +
                "\nVICTORIES            " +
                data.TotalVictories +
                "\nLOSSES               " +
                data.TotalLosses);

            SetScreen(
                "VETERAN TERMINAL A SCREEN",
                BuildVeteranText(data));

            SetScreen(
                "EQUIPMENT HISTORY",
                BuildEquipmentText(data));

            SetScreen(
                "OPERATIONAL HISTORY",
                BuildOperationalText(data));

            SetScreen(
                "DAMAGE HISTORY SCREEN",
                BuildDamageText(data));

            SetScreen(
                "MISSION HISTORY SCREEN",
                BuildMissionText(data));

            SetScreen(
                "COMMANDER DEVELOPMENT SCREEN",
                "COMMANDER DEVELOPMENT\n\n" +
                "LEVEL        " +
                data.CommanderLevel +
                "\nEXPERIENCE   " +
                data.CommanderExperience);

            SetScreen(
                "MILITARY DEVELOPMENT HUD",
                "MILITARY DEVELOPMENT\n\n" +
                "LEVEL        " +
                data.MilitaryDevelopmentLevel +
                "\nTECHNOLOGY   " +
                data.TechnologyLevel +
                "\nDOCTRINE     " +
                data.DoctrineLevel);

            SetScreen(
                "SYSTEM STATUS SCREEN",
                "SYSTEM STATUS\n\n" +
                "DATABASE     ONLINE\n" +
                "UNIT RECORDS " +
                data.Units.Count +
                "\nHISTORY      " +
                data.History.Count);

            SetScreen(
                "RECOVERY STATUS SCREEN",
                BuildRecoveryText(data));

            SetScreen(
                "VETERAN UNIT ARCHIVE BODY",
                BuildVeteranText(data));

            SetScreen(
                "EQUIPMENT HISTORY BODY",
                BuildEquipmentText(data));

            SetScreen(
                "OPERATIONAL HISTORY BODY",
                BuildOperationalText(data));

            SetScreen(
                "DAMAGE / REPAIR HISTORY BODY",
                BuildDamageText(data));

            SetScreen(
                "MILITARY LOSSES BODY",
                BuildLossText(data));

            SetScreen(
                "UNIT HISTORY COMMAND WALL BODY",
                BuildUnitWall(data));

            SetScreen(
                "SERVICE HISTORY DISPLAY",
                BuildServiceHistory(data));

            SetScreen(
                "COMBAT HISTORY",
                BuildCombatHistory(data));
        }

        private void ShowUnitHistory()
        {
            SetScreen(
                "UNIT HISTORY TERMINAL B SCREEN",
                BuildUnitWall(
                    PersistentMilitaryRuntime.Data));

            RefreshAll();
        }

        private void ShowAfterAction()
        {
            SetScreen(
                "COMBAT HISTORY SCREEN",
                BuildHistory(
                    PersistentMilitaryRuntime.Data,
                    "VICTORY"));

            RefreshAll();
        }

        private void ShowOperations()
        {
            SetScreen(
                "MISSION HISTORY SCREEN",
                BuildMissionText(
                    PersistentMilitaryRuntime.Data));

            RefreshAll();
        }

        private void ShowLosses()
        {
            SetScreen(
                "MILITARY LOSSES BODY",
                BuildLossText(
                    PersistentMilitaryRuntime.Data));

            RefreshAll();
        }

        private void ShowVeterans()
        {
            SetScreen(
                "VETERAN UNIT ARCHIVE BODY",
                BuildVeteranText(
                    PersistentMilitaryRuntime.Data));

            RefreshAll();
        }

        private void ShowOperationalHistory()
        {
            SetScreen(
                "OPERATIONAL HISTORY BODY",
                BuildOperationalText(
                    PersistentMilitaryRuntime.Data));

            RefreshAll();
        }

        private void ShowFullRecord()
        {
            SetScreen(
                "UNIT HISTORY COMMAND WALL BODY",
                BuildUnitWall(
                    PersistentMilitaryRuntime.Data));

            RefreshAll();
        }

        private void ShowEquipmentHistory()
        {
            SetScreen(
                "EQUIPMENT HISTORY BODY",
                BuildEquipmentText(
                    PersistentMilitaryRuntime.Data));

            RefreshAll();
        }

        private void ShowDamageHistory()
        {
            SetScreen(
                "DAMAGE / REPAIR HISTORY BODY",
                BuildDamageText(
                    PersistentMilitaryRuntime.Data));

            RefreshAll();
        }

        private void RepairAll()
        {
            PersistentMilitarySaveData data =
                PersistentMilitaryRuntime.Data;

            if (data == null)
                return;

            foreach (MilitaryUnitRecord unit in data.Units)
            {
                if (unit.Damage > 0)
                {
                    PersistentMilitaryRuntime.RepairUnit(
                        unit.UnitId,
                        unit.Damage);
                }
            }
        }

        private void CloseDetail()
        {
            GameObject detail =
                GameObject.Find(
                    "RECORD DETAIL WINDOW");

            if (detail != null)
                detail.SetActive(false);
        }

        private void SetScreen(
            string objectName,
            string text)
        {
            GameObject target =
                GameObject.Find(objectName);

            if (target == null)
                return;

            TMP_Text[] texts =
                target.GetComponentsInChildren<TMP_Text>(
                    true);

            if (texts.Length == 0)
                return;

            texts[0].text = text;
        }

        private string BuildVeteranText(
            PersistentMilitarySaveData data)
        {
            int veterans = 0;

            foreach (MilitaryUnitRecord unit in data.Units)
            {
                if (unit.VeteranLevel > 0)
                    veterans++;
            }

            return
                "VETERAN UNIT ARCHIVE\n\n" +
                "VETERAN UNITS     " + veterans +
                "\nREGISTERED UNITS  " + data.Units.Count;
        }

        private string BuildEquipmentText(
            PersistentMilitarySaveData data)
        {
            return
                "EQUIPMENT HISTORY\n\n" +
                "TOTAL REPAIRS     " + data.TotalRepairs +
                "\n\nUNIT DAMAGE RECORDS";

        }

        private string BuildOperationalText(
            PersistentMilitarySaveData data)
        {
            return
                "OPERATIONAL HISTORY\n\n" +
                "TOTAL OPERATIONS  " + data.TotalOperations +
                "\nVICTORIES         " + data.TotalVictories +
                "\nLOSSES            " + data.TotalLosses;
        }

        private string BuildDamageText(
            PersistentMilitarySaveData data)
        {
            int damaged = 0;
            int disabled = 0;

            foreach (MilitaryUnitRecord unit in data.Units)
            {
                if (unit.Damage > 0)
                    damaged++;

                if (string.Equals(
                        unit.Status,
                        "DISABLED",
                        StringComparison.OrdinalIgnoreCase))
                    disabled++;
            }

            return
                "DAMAGE / REPAIR HISTORY\n\n" +
                "DAMAGED UNITS     " + damaged +
                "\nDISABLED UNITS    " + disabled +
                "\nTOTAL REPAIRS     " + data.TotalRepairs;
        }

        private string BuildLossText(
            PersistentMilitarySaveData data)
        {
            return
                "LOSSES AND MEMORIAL\n\n" +
                "TOTAL UNITS LOST  " +
                data.TotalLosses;
        }

        private string BuildMissionText(
            PersistentMilitarySaveData data)
        {
            return
                "MISSION HISTORY\n\n" +
                "MISSIONS          " +
                data.TotalOperations +
                "\nVICTORIES         " +
                data.TotalVictories;
        }

        private string BuildRecoveryText(
            PersistentMilitarySaveData data)
        {
            int damaged = 0;

            foreach (MilitaryUnitRecord unit in data.Units)
            {
                if (unit.Damage > 0)
                    damaged++;
            }

            return
                "RECOVERY STATUS\n\n" +
                "DAMAGED UNITS     " + damaged +
                "\nREPAIRS COMPLETED " +
                data.TotalRepairs;
        }

        private string BuildUnitWall(
            PersistentMilitarySaveData data)
        {
            string result =
                "UNIT HISTORY ARCHIVE\n\n";

            foreach (MilitaryUnitRecord unit in data.Units)
            {
                result +=
                    unit.UnitId +
                    "  " +
                    unit.UnitType +
                    "  " +
                    unit.Status +
                    "\n";
            }

            return result;
        }

        private string BuildServiceHistory(
            PersistentMilitarySaveData data)
        {
            int totalExperience = 0;

            foreach (MilitaryUnitRecord unit in data.Units)
                totalExperience += unit.Experience;

            return
                "SERVICE HISTORY\n\n" +
                "UNITS             " + data.Units.Count +
                "\nTOTAL EXPERIENCE  " + totalExperience +
                "\nOPERATIONS        " + data.TotalOperations;
        }

        private string BuildCombatHistory(
            PersistentMilitarySaveData data)
        {
            return BuildHistory(
                data,
                "VICTORY");
        }

        private string BuildHistory(
            PersistentMilitarySaveData data,
            string type)
        {
            int count = 0;

            foreach (MilitaryEventRecord record in data.History)
            {
                if (string.Equals(
                        record.EventType,
                        type,
                        StringComparison.OrdinalIgnoreCase))
                {
                    count++;
                }
            }

            return
                "COMBAT HISTORY\n\n" +
                type +
                " RECORDS     " +
                count;
        }
    }
}
