using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
        public int MilitaryDevelopmentExperience;

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
                Save();
            }

            if (data.Units == null)
                data.Units = new List<MilitaryUnitRecord>();

            if (data.History == null)
                data.History = new List<MilitaryEventRecord>();

            Validate();
        }

        public static MilitaryUnitRecord AddUnit(
            string unitId,
            string unitType)
        {
            InitializeIfNeeded();

            MilitaryUnitRecord existing = GetUnit(unitId);

            if (existing != null)
                return existing;

            MilitaryUnitRecord unit =
                new MilitaryUnitRecord
                {
                    UnitId = unitId ?? "",
                    UnitType = unitType ?? "",
                    Status = "ACTIVE",
                    LastUpdate = DateTime.UtcNow.ToString("o")
                };

            data.Units.Add(unit);

            Save();

            return unit;
        }

        public static MilitaryUnitRecord GetUnit(string unitId)
        {
            InitializeIfNeeded();

            if (string.IsNullOrEmpty(unitId))
                return null;

            for (int i = 0; i < data.Units.Count; i++)
            {
                MilitaryUnitRecord unit = data.Units[i];

                if (unit != null &&
                    string.Equals(
                        unit.UnitId,
                        unitId,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return unit;
                }
            }

            return null;
        }

        public static void RecordOperation(
            string unitId,
            string operation,
            bool victory)
        {
            InitializeIfNeeded();

            MilitaryUnitRecord unit = GetUnit(unitId);

            if (unit == null)
                return;

            unit.Missions++;

            if (victory)
            {
                unit.Victories++;
                data.TotalVictories++;
            }

            data.TotalOperations++;

            unit.Experience += victory ? 100 : 50;
            unit.LastOperation = operation ?? "";
            unit.LastUpdate = DateTime.UtcNow.ToString("o");

            UpdateVeteranLevel(unit);

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
            string details = "")
        {
            InitializeIfNeeded();

            MilitaryUnitRecord unit = GetUnit(unitId);

            if (unit == null)
                return;

            unit.Losses++;
            unit.Status = "LOST";

            data.TotalLosses++;

            unit.LastOperation = operation ?? "";
            unit.LastUpdate = DateTime.UtcNow.ToString("o");

            AddHistory(
                unitId,
                "LOSS",
                operation,
                string.IsNullOrEmpty(details)
                    ? "Unit lost."
                    : details);

            Save();
        }

        public static void RecordDamage(
            string unitId,
            int damage,
            string operation = "")
        {
            InitializeIfNeeded();

            MilitaryUnitRecord unit = GetUnit(unitId);

            if (unit == null)
                return;

            unit.Damage =
                Mathf.Clamp(
                    unit.Damage + Mathf.Max(0, damage),
                    0,
                    100);

            unit.Status =
                unit.Damage >= 100
                    ? "DISABLED"
                    : "DAMAGED";

            unit.LastOperation = operation ?? "";
            unit.LastUpdate = DateTime.UtcNow.ToString("o");

            AddHistory(
                unitId,
                "DAMAGE",
                operation,
                "Damage recorded: " + damage + "%.");

            Save();
        }

        public static void RepairUnit(
            string unitId,
            int amount)
        {
            InitializeIfNeeded();

            MilitaryUnitRecord unit = GetUnit(unitId);

            if (unit == null)
                return;

            int repair =
                Mathf.Clamp(
                    amount,
                    0,
                    unit.Damage);

            if (repair <= 0)
                return;

            unit.Damage -= repair;
            unit.Repairs++;

            data.TotalRepairs++;

            unit.Status =
                unit.Damage <= 0
                    ? "ACTIVE"
                    : "DAMAGED";

            unit.LastUpdate = DateTime.UtcNow.ToString("o");

            AddHistory(
                unitId,
                "REPAIR",
                "",
                "Repair completed: " + repair + "%.");

            Save();
        }

        public static void AddHistory(
            string unitId,
            string eventType,
            string operation,
            string details)
        {
            InitializeIfNeeded();

            MilitaryEventRecord record =
                new MilitaryEventRecord
                {
                    EventId =
                        Guid.NewGuid().ToString("N"),

                    UnitId = unitId ?? "",
                    EventType = eventType ?? "",
                    Operation = operation ?? "",
                    Details = details ?? "",
                    Timestamp =
                        DateTime.UtcNow.ToString("o")
                };

            data.History.Add(record);

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
            InitializeIfNeeded();

            int gained =
                Mathf.Max(0, experience);

            data.CommanderExperience += gained;

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
            InitializeIfNeeded();

            int gainedExperience =
                Mathf.Max(0, experience);

            data.MilitaryDevelopmentExperience +=
                gainedExperience;

            data.MilitaryDevelopmentLevel =
                1 +
                data.MilitaryDevelopmentExperience / 1000;

            AddHistory(
                "",
                "MILITARY_DEVELOPMENT",
                "",
                "Military development updated.");

            Save();
        }

        public static void DevelopDoctrine()
        {
            InitializeIfNeeded();

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
            InitializeIfNeeded();

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

            if (string.IsNullOrEmpty(json))
            {
                data = null;
                return;
            }

            try
            {
                data =
                    JsonUtility.FromJson<PersistentMilitarySaveData>(
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
            InitializeIfNeeded();

            if (data.Units == null)
                data.Units =
                    new List<MilitaryUnitRecord>();

            if (data.History == null)
                data.History =
                    new List<MilitaryEventRecord>();

            if (data.MilitaryDevelopmentLevel < 1)
                data.MilitaryDevelopmentLevel = 1;

            if (data.CommanderLevel < 1)
                data.CommanderLevel = 1;

            if (data.TechnologyLevel < 1)
                data.TechnologyLevel = 1;

            if (data.DoctrineLevel < 1)
                data.DoctrineLevel = 1;
        }

        private static void InitializeIfNeeded()
        {
            if (data == null)
                Initialize();
        }
    }

    public sealed class PersistentMilitaryController :
        MonoBehaviour
    {
        private static readonly string[] ButtonNames =
        {
            "BUTTON - VIEW FULL RECORD",
            "BUTTON - CLOSE",
            "BUTTON - DAMAGE HISTORY",
            "BUTTON - GARAGE",
            "BUTTON - RESEARCH",
            "BUTTON - AFTER ACTION",
            "BUTTON - VETERAN UNITS",
            "BUTTON - UNIT HISTORY",
            "BUTTON - MILITARY DEVELOPMENT",
            "BUTTON - REPAIRS",
            "BUTTON - EQUIPMENT HISTORY",
            "BUTTON - LOSSES",
            "BUTTON - OPERATIONS",
            "BUTTON - LOGISTICS",
            "BUTTON - OPERATIONAL HISTORY",
            "BUTTON - COMMAND CENTER"
        };

        private void Awake()
        {
            PersistentMilitaryRuntime.Initialize();
            WireButtons();

            Debug.Log(
                "[SCN17] Persistent Military controller initialized.");
        }

        private void WireButtons()
        {
            int wired = 0;

            foreach (string buttonName in ButtonNames)
            {
                GameObject objectFound =
                    GameObject.Find(buttonName);

                if (objectFound == null)
                    continue;

                Button button =
                    objectFound.GetComponent<Button>();

                if (button == null)
                    continue;

                button.onClick.RemoveListener(
                    HandleButton);

                button.onClick.AddListener(
                    HandleButton);

                wired++;
            }

            Debug.Log(
                "[SCN17] Controller buttons wired: " +
                wired);
        }

        private void HandleButton()
        {
            GameObject selectedObject =
                EventSystem.current != null
                    ? EventSystem.current.currentSelectedGameObject
                    : null;

            Button source =
                selectedObject != null
                    ? selectedObject.GetComponent<Button>()
                    : null;

            if (source == null)
                return;

            string buttonName =
                source.gameObject.name;

            Debug.Log(
                "[SCN17 ACTION] " +
                buttonName +
                " onClick invoked.");

            switch (buttonName)
            {
                case "BUTTON - MILITARY DEVELOPMENT":
                    PersistentMilitaryRuntime
                        .DevelopMilitary(100);
                    break;

                case "BUTTON - REPAIRS":
                    RepairAll();
                    break;

                default:
                    Debug.Log(
                        "[SCN17] Action: " +
                        buttonName);
                    break;
            }

            PersistentMilitaryRuntime.Save();
        }

        private void RepairAll()
        {
            PersistentMilitarySaveData save =
                PersistentMilitaryRuntime.Data;

            if (save == null ||
                save.Units == null)
                return;

            for (int i = 0;
                 i < save.Units.Count;
                 i++)
            {
                MilitaryUnitRecord unit =
                    save.Units[i];

                if (unit == null)
                    continue;

                if (unit.Damage > 0)
                {
                    PersistentMilitaryRuntime
                        .RepairUnit(
                            unit.UnitId,
                            unit.Damage);
                }
            }
        }
    }
}











