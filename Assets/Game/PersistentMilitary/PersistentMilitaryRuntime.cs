using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ObsidianProtocol.Game.PersistentMilitary
{
    [Serializable]
    public sealed class MilitaryUnitRecord
    {
        public string UnitId = "";
        public string UnitType = "";
        public string Status = "ACTIVE";

        public int Missions;
        public int Victories;
        public int Losses;

        public int Damage;
        public int Repairs;

        public int Experience;
        public int VeteranLevel;

        public string LastOperation = "";
        public string LastUpdate = "";
    }

    [Serializable]
    public sealed class MilitaryEventRecord
    {
        public string EventId = "";
        public string UnitId = "";
        public string EventType = "";
        public string Operation = "";
        public string Details = "";
        public string Timestamp = "";
    }

    [Serializable]
    public sealed class PersistentMilitarySaveData
    {
        public string PlayerId = "PLAYER";

        public int CommanderLevel = 1;
        public int CommanderExperience;

        public int MilitaryDevelopmentLevel = 1;
        public int TechnologyLevel = 1;
        public int DoctrineLevel = 1;

        public int TotalOperations;
        public int TotalVictories;
        public int TotalLosses;
        public int TotalRepairs;

        public List<MilitaryUnitRecord> Units =
            new List<MilitaryUnitRecord>();

        public List<MilitaryEventRecord> History =
            new List<MilitaryEventRecord>();
    }

    public static class PersistentMilitaryRuntime
    {
        private const string SaveKey =
            "obsidian_protocol_persistent_military";

        private static PersistentMilitarySaveData data;

        public static bool Initialized =>
            data != null;

        public static PersistentMilitarySaveData Data =>
            data;

        public static void Initialize()
        {
            if (data != null)
                return;

            Load();

            if (data == null)
            {
                data = new PersistentMilitarySaveData();

                AddDefaultUnits();
                Save();
            }

            Validate();
        }

        private static void AddDefaultUnits()
        {
            AddUnit("ARCHIVE", "COMMAND");
            AddUnit("BULLDOG", "GROUND");
            AddUnit("FORGE", "GROUND");
            AddUnit("WARDEN", "AIR");
            AddUnit("BEACON", "AIR");
        }

        public static MilitaryUnitRecord AddUnit(
            string unitId,
            string unitType)
        {
            if (data == null)
                Initialize();

            MilitaryUnitRecord existing =
                GetUnit(unitId);

            if (existing != null)
                return existing;

            MilitaryUnitRecord unit =
                new MilitaryUnitRecord
                {
                    UnitId = unitId ?? "",
                    UnitType = unitType ?? "",
                    Status = "ACTIVE",
                    LastUpdate = DateTime.UtcNow.ToString("O")
                };

            data.Units.Add(unit);
            Save();

            return unit;
        }

        public static MilitaryUnitRecord GetUnit(
            string unitId)
        {
            if (data == null ||
                string.IsNullOrWhiteSpace(unitId))
                return null;

            for (int i = 0; i < data.Units.Count; i++)
            {
                if (string.Equals(
                        data.Units[i].UnitId,
                        unitId,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return data.Units[i];
                }
            }

            return null;
        }

        public static void RecordOperation(
            string unitId,
            string operation,
            bool victory)
        {
            MilitaryUnitRecord unit =
                GetUnit(unitId);

            if (unit == null)
                unit = AddUnit(unitId, "UNKNOWN");

            unit.Missions++;

            if (victory)
            {
                unit.Victories++;
                data.TotalVictories++;
            }

            data.TotalOperations++;

            unit.Experience += victory ? 100 : 50;

            UpdateVeteranLevel(unit);

            unit.LastOperation =
                operation ?? "";

            unit.LastUpdate =
                DateTime.UtcNow.ToString("O");

            AddHistory(
                unitId,
                victory ? "VICTORY" : "OPERATION",
                operation,
                victory
                    ? "Operation completed successfully."
                    : "Operation completed.");

            Save();
        }

        public static void RecordLoss(
            string unitId,
            string operation,
            string details)
        {
            MilitaryUnitRecord unit =
                GetUnit(unitId);

            if (unit == null)
                unit = AddUnit(unitId, "UNKNOWN");

            unit.Losses++;
            unit.Status = "LOST";

            data.TotalLosses++;

            AddHistory(
                unitId,
                "LOSS",
                operation,
                details);

            Save();
        }

        public static void RecordDamage(
            string unitId,
            int damage,
            string details)
        {
            MilitaryUnitRecord unit =
                GetUnit(unitId);

            if (unit == null)
                unit = AddUnit(unitId, "UNKNOWN");

            unit.Damage =
                Mathf.Clamp(
                    unit.Damage + Mathf.Max(0, damage),
                    0,
                    100);

            unit.Status =
                unit.Damage >= 100
                    ? "DISABLED"
                    : "DAMAGED";

            AddHistory(
                unitId,
                "DAMAGE",
                "",
                details);

            Save();
        }

        public static void RepairUnit(
            string unitId,
            int amount)
        {
            MilitaryUnitRecord unit =
                GetUnit(unitId);

            if (unit == null)
                return;

            int repair =
                Mathf.Clamp(amount, 0, unit.Damage);

            unit.Damage -= repair;
            unit.Repairs++;

            data.TotalRepairs++;

            if (unit.Damage <= 0)
                unit.Status = "ACTIVE";
            else
                unit.Status = "DAMAGED";

            AddHistory(
                unitId,
                "REPAIR",
                "",
                "Unit repaired.");

            Save();
        }

        public static void AddHistory(
            string unitId,
            string eventType,
            string operation,
            string details)
        {
            if (data == null)
                return;

            data.History.Add(
                new MilitaryEventRecord
                {
                    EventId =
                        Guid.NewGuid().ToString("N"),

                    UnitId =
                        unitId ?? "",

                    EventType =
                        eventType ?? "",

                    Operation =
                        operation ?? "",

                    Details =
                        details ?? "",

                    Timestamp =
                        DateTime.UtcNow.ToString("O")
                });

            if (data.History.Count > 1000)
                data.History.RemoveAt(0);
        }

        private static void UpdateVeteranLevel(
            MilitaryUnitRecord unit)
        {
            if (unit == null)
                return;

            unit.VeteranLevel =
                Mathf.Clamp(
                    unit.Experience / 500,
                    0,
                    10);
        }

        public static void DevelopCommander(
            int experience)
        {
            if (data == null)
                Initialize();

            data.CommanderExperience +=
                Mathf.Max(0, experience);

            data.CommanderLevel =
                1 +
                data.CommanderExperience / 1000;

            AddHistory(
                "",
                "COMMANDER_DEVELOPMENT",
                "",
                "Commander development updated.");

            Save();
        }

        public static void DevelopMilitary(
            int experience)
        {
            if (data == null)
                Initialize();

            data.MilitaryDevelopmentLevel =
                Mathf.Max(
                    1,
                    data.MilitaryDevelopmentLevel +
                    Mathf.Max(0, experience) / 1000);

            AddHistory(
                "",
                "MILITARY_DEVELOPMENT",
                "",
                "Military development updated.");

            Save();
        }

        public static void DevelopDoctrine()
        {
            if (data == null)
                Initialize();

            data.DoctrineLevel++;

            AddHistory(
                "",
                "DOCTRINE_DEVELOPMENT",
                "",
                "Doctrine development advanced.");

            Save();
        }

        public static void DevelopTechnology()
        {
            if (data == null)
                Initialize();

            data.TechnologyLevel++;

            AddHistory(
                "",
                "TECHNOLOGY_DEVELOPMENT",
                "",
                "Technology development advanced.");

            Save();
        }

        public static void Save()
        {
            if (data == null)
                return;

            string json =
                JsonUtility.ToJson(
                    data,
                    true);

            PlayerPrefs.SetString(
                SaveKey,
                json);

            PlayerPrefs.Save();
        }

        public static void Load()
        {
            if (!PlayerPrefs.HasKey(SaveKey))
            {
                data = null;
                return;
            }

            string json =
                PlayerPrefs.GetString(
                    SaveKey,
                    "");

            if (string.IsNullOrWhiteSpace(json))
            {
                data = null;
                return;
            }

            try
            {
                data =
                    JsonUtility.FromJson
                        <PersistentMilitarySaveData>(
                            json);
            }
            catch
            {
                data = null;
            }

            if (data != null)
            {
                if (data.Units == null)
                    data.Units =
                        new List<MilitaryUnitRecord>();

                if (data.History == null)
                    data.History =
                        new List<MilitaryEventRecord>();
            }
        }

        public static void Validate()
        {
            if (data == null)
                return;

            data.CommanderLevel =
                Mathf.Max(
                    1,
                    data.CommanderLevel);

            data.MilitaryDevelopmentLevel =
                Mathf.Max(
                    1,
                    data.MilitaryDevelopmentLevel);

            data.TechnologyLevel =
                Mathf.Max(
                    1,
                    data.TechnologyLevel);

            data.DoctrineLevel =
                Mathf.Max(
                    1,
                    data.DoctrineLevel);
        }
    }

    public sealed class PersistentMilitaryController
        : MonoBehaviour
    {
        private readonly Dictionary<
            string,
            Button> buttons =
            new Dictionary<
                string,
                Button>(
                StringComparer.OrdinalIgnoreCase);

        private void Awake()
        {
            PersistentMilitaryRuntime.Initialize();
            WireButtons();
            Refresh();
        }

        private void OnApplicationPause(
            bool pause)
        {
            if (pause)
                PersistentMilitaryRuntime.Save();
        }

        private void OnApplicationQuit()
        {
            PersistentMilitaryRuntime.Save();
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

                if (string.IsNullOrWhiteSpace(name))
                    continue;

                buttons[name] = button;

                button.onClick.RemoveListener(
                    HandleButton);

                button.onClick.AddListener(
                    HandleButton);
            }
        }

        private void HandleButton()
        {
            GameObject selectedObject =
                UnityEngine.EventSystems.EventSystem.current?
                    .currentSelectedGameObject;

            Button source =
                selectedObject?
                    .GetComponent<Button>();

            if (source == null)
                return;

            HandleAction(
                source.gameObject.name);
        }

        private void HandleAction(
            string buttonName)
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
                    break;

                case "BUTTON - GARAGE":
                    OpenScene("GARAGE");
                    break;

                case "BUTTON - LOSSES":
                    ShowLosses();
                    break;

                case "BUTTON - VETERAN UNITS":
                    ShowVeteranUnits();
                    break;

                case "BUTTON - OPERATIONAL HISTORY":
                    ShowOperationalHistory();
                    break;

                case "BUTTON - LOGISTICS":
                    OpenScene("LOGISTICS");
                    break;

                case "BUTTON - VIEW FULL RECORD":
                    ShowFullRecord();
                    break;

                case "BUTTON - RESEARCH":
                    OpenScene("RESEARCH");
                    break;

                case "BUTTON - EQUIPMENT HISTORY":
                    ShowEquipmentHistory();
                    break;

                case "BUTTON - REPAIRS":
                    RepairAll();
                    break;

                case "BUTTON - COMMAND CENTER":
                    OpenScene("COMMAND");
                    break;

                case "BUTTON - DAMAGE HISTORY":
                    ShowDamageHistory();
                    break;
            }

            Refresh();
        }

        private void ShowUnitHistory()
        {
            AddStatus(
                "UNIT HISTORY",
                BuildUnitSummary());
        }

        private void ShowAfterAction()
        {
            AddStatus(
                "AFTER ACTION",
                BuildHistory("VICTORY"));
        }

        private void ShowOperations()
        {
            AddStatus(
                "OPERATIONS",
                "OPERATIONS: " +
                PersistentMilitaryRuntime.Data
                    .TotalOperations);
        }

        private void ShowLosses()
        {
            AddStatus(
                "LOSSES",
                "LOSSES: " +
                PersistentMilitaryRuntime.Data
                    .TotalLosses);
        }

        private void ShowVeteranUnits()
        {
            int count = 0;

            foreach (
                MilitaryUnitRecord unit
                in PersistentMilitaryRuntime.Data.Units)
            {
                if (unit.VeteranLevel > 0)
                    count++;
            }

            AddStatus(
                "VETERAN UNITS",
                "VETERAN UNITS: " + count);
        }

        private void ShowOperationalHistory()
        {
            AddStatus(
                "OPERATIONAL HISTORY",
                BuildHistory("OPERATION"));
        }

        private void ShowFullRecord()
        {
            AddStatus(
                "FULL RECORD",
                BuildUnitSummary());
        }

        private void ShowEquipmentHistory()
        {
            AddStatus(
                "EQUIPMENT HISTORY",
                BuildHistory("REPAIR"));
        }

        private void ShowDamageHistory()
        {
            AddStatus(
                "DAMAGE HISTORY",
                BuildHistory("DAMAGE"));
        }

        private void RepairAll()
        {
            foreach (
                MilitaryUnitRecord unit
                in PersistentMilitaryRuntime.Data.Units)
            {
                if (unit.Damage > 0)
                    PersistentMilitaryRuntime.RepairUnit(
                        unit.UnitId,
                        unit.Damage);
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

        private string BuildUnitSummary()
        {
            int count =
                PersistentMilitaryRuntime.Data.Units.Count;

            return "REGISTERED UNITS: " + count;
        }

        private string BuildHistory(
            string type)
        {
            int count = 0;

            foreach (
                MilitaryEventRecord record
                in PersistentMilitaryRuntime.Data.History)
            {
                if (string.Equals(
                        record.EventType,
                        type,
                        StringComparison.OrdinalIgnoreCase))
                {
                    count++;
                }
            }

            return type +
                   " RECORDS: " +
                   count;
        }

        private void AddStatus(
            string title,
            string value)
        {
            GameObject status =
                GameObject.Find("STATUS");

            if (status == null)
                return;

            TMPro.TMP_Text text =
                status.GetComponentInChildren<
                    TMPro.TMP_Text>(
                        true);

            if (text != null)
                text.text =
                    title + "\n" + value;
        }

        private void OpenScene(
            string sceneKey)
        {
            AddStatus(
                "NAVIGATION",
                "REQUESTED: " + sceneKey);

            Debug.Log(
                "[PERSISTENT MILITARY] Navigation requested: " +
                sceneKey);
        }

        private void Refresh()
        {
            PersistentMilitaryRuntime.Save();
            RefreshDisplay();
        }

        private void RefreshDisplay()
        {
            PersistentMilitarySaveData data =
                PersistentMilitaryRuntime.Data;

            if (data == null)
                return;

            SetText("VALUE - COMMANDER LEVEL",
                data.CommanderLevel.ToString());

            SetText("VALUE - COMMANDER EXPERIENCE",
                data.CommanderExperience.ToString());

            SetText("VALUE - MILITARY DEVELOPMENT",
                data.MilitaryDevelopmentLevel.ToString());

            SetText("VALUE - TECHNOLOGY",
                data.TechnologyLevel.ToString());

            SetText("VALUE - DOCTRINE",
                data.DoctrineLevel.ToString());

            SetText("VALUE - OPERATIONS",
                data.TotalOperations.ToString());

            SetText("VALUE - VICTORIES",
                data.TotalVictories.ToString());

            SetText("VALUE - LOSSES",
                data.TotalLosses.ToString());

            SetText("VALUE - REPAIRS",
                data.TotalRepairs.ToString());

            SetText("VALUE - UNITS",
                data.Units.Count.ToString());

            SetText("VALUE - HISTORY",
                data.History.Count.ToString());
        }

        private void SetText(
            string objectName,
            string value)
        {
            GameObject target =
                GameObject.Find(objectName);

            if (target == null)
                return;

            TMPro.TMP_Text text =
                target.GetComponentInChildren<TMPro.TMP_Text>(
                    true);

            if (text != null)
                text.text = value;
        }
    }
}


