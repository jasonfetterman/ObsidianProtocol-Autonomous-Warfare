using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ObsidianProtocol.Game.PersistentMilitary
{
    public sealed class SCN17PersistentMilitaryUI : MonoBehaviour
    {
        private readonly Dictionary<string, Button> buttons =
            new Dictionary<string, Button>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, GameObject> objects =
            new Dictionary<string, GameObject>(StringComparer.OrdinalIgnoreCase);

        private static readonly string[] MainPanels =
        {
            "SYSTEM STATUS SCREEN",
            "RECOVERY STATUS SCREEN",
            "EQUIPMENT TIMELINE SCREEN",
            "UNIT HISTORY",
            "OPERATIONAL HISTORY",
            "COMMANDER DEVELOPMENT",
            "MILITARY DEVELOPMENT"
        };

        private static readonly string[] SecondaryDisplays =
        {
            "UNIT IDENTITY",
            "UNIT HISTORY TERMINAL A SCREEN",
            "UNIT HISTORY TERMINAL B SCREEN",
            "UNIT HISTORY TERMINAL C SCREEN",
            "SERVICE HISTORY DISPLAY",
            "COMBAT HISTORY",
            "DAMAGE / REPAIR",
            "DAMAGE HISTORY SCREEN",
            "DAMAGE / REPAIR HISTORY BODY",
            "EQUIPMENT HISTORY BODY",
            "EQUIPMENT HISTORY",
            "MILITARY LOSSES BODY",
            "VETERAN TERMINAL A SCREEN",
            "VETERAN UNIT ARCHIVE BODY",
            "OPERATIONAL HISTORY BODY",
            "OPERATIONAL EXPERIENCE SCREEN",
            "MISSION HISTORY SCREEN",
            "REPAIR HISTORY SCREEN",
            "UNIT HISTORY COMMAND WALL BODY"
        };

        private bool initialized;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Install()
        {
            string sceneName = SceneManager.GetActiveScene().name;

            if (sceneName != "Persistent_Military" &&
                sceneName != "SCN17_PERSISTENT_MILITARY")
                return;

            GameObject root =
                GameObject.Find("PERSISTENT MILITARY HUD") ??
                GameObject.Find("16. PERSISTENT MILITARY");

            if (root == null)
            {
                Debug.LogWarning("[SCN17] Persistent Military HUD root not found.");
                return;
            }

            if (root.GetComponent<SCN17PersistentMilitaryUI>() == null)
                root.AddComponent<SCN17PersistentMilitaryUI>();
        }

        private void Awake()
        {
            CancelInvoke(nameof(RetryInitialize));
            Invoke(nameof(RetryInitialize), 0.1f);
        }

        private void OnEnable()
        {
            if (initialized)
                RefreshAll();
        }

        private void RetryInitialize()
        {
            if (!initialized)
                Initialize();
        }
        private void Initialize()
        {
            if (initialized)
                return;

            CacheObjects();

            if (objects.Count == 0)
            {
                Debug.LogWarning("[SCN17] Scene objects not ready; retrying initialization.");
                CancelInvoke(nameof(RetryInitialize));
                Invoke(nameof(RetryInitialize), 0.25f);
                return;
            }

            initialized = true;

            PersistentMilitaryRuntime.Initialize();
            WireButtons();

            SCN17RuntimeUnitSelector unitSelector =
                GetComponent<SCN17RuntimeUnitSelector>();

            if (unitSelector == null)
                unitSelector = gameObject.AddComponent<SCN17RuntimeUnitSelector>();

            unitSelector.Build();

            CloseAllSecondaryDisplays();
            CloseAllMainPanels();

            SetPanelActive("SYSTEM STATUS SCREEN", true);

            Debug.Log("[SCN17] Persistent Military UI initialized.");
        }

        private void CacheObjects()
{
    objects.Clear();
    buttons.Clear();

    Scene activeScene = SceneManager.GetActiveScene();

    // Cache active-scene objects first so duplicate names prefer this scene.
    for (int pass = 0; pass < SceneManager.sceneCount; pass++)
    {
        Scene scene = SceneManager.GetSceneAt(pass);
        if (!scene.IsValid() || !scene.isLoaded)
            continue;

        if (pass == 0 && scene != activeScene)
            continue;

        if (scene != activeScene)
            continue;

        CacheSceneRoots(scene);
    }

    // Include other loaded scenes only for names not already found.
    for (int i = 0; i < SceneManager.sceneCount; i++)
    {
        Scene scene = SceneManager.GetSceneAt(i);
        if (!scene.IsValid() || !scene.isLoaded || scene == activeScene)
            continue;

        CacheSceneRoots(scene);
    }

    Debug.Log("[SCN17] Cached " + objects.Count +
        " scene objects; " + buttons.Count + " buttons. Active scene: " +
        activeScene.name);
}

private void CacheSceneRoots(Scene scene)
{
    GameObject[] roots = scene.GetRootGameObjects();

    foreach (GameObject root in roots)
    {
        Transform[] transforms = root.GetComponentsInChildren<Transform>(true);

        foreach (Transform item in transforms)
        {
            if (item == null)
                continue;

            if (!objects.ContainsKey(item.name))
                objects.Add(item.name, item.gameObject);

            Button button = item.GetComponent<Button>();
            if (button != null && !buttons.ContainsKey(item.name))
                buttons.Add(item.name, button);
        }
    }
}

        private GameObject GetObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName))
                return null;

            GameObject target;

            if (objects.TryGetValue(objectName, out target))
                return target;

            if (string.Equals(objectName, "SYSTEM STATUS SCREEN", StringComparison.OrdinalIgnoreCase) &&
                objects.TryGetValue("STATUS", out target))
                return target;
            // The main-panel name differs from the existing scene object name.
            if (string.Equals(objectName, "UNIT HISTORY", StringComparison.OrdinalIgnoreCase) &&
                objects.TryGetValue("UNIT RECORD PANEL - UNIT HISTORY", out target))
                return target;

            return null;
        }

        private void WireButtons()
{
    buttons.Clear();

    Button[] allButtons =
        GetComponentsInChildren<Button>(true);

    foreach (Button button in allButtons)
    {
        if (button == null)
            continue;

        if (!button.gameObject.name.StartsWith(
                "BUTTON - ",
                StringComparison.OrdinalIgnoreCase))
            continue;

        string key =
            button.gameObject.name.Substring(9).Trim();

        if (!buttons.ContainsKey(key))
            buttons.Add(key, button);

        /*
         * SCN17InputBootstrap owns mouse dispatch.
         *
         * Do NOT add Button.onClick listeners here.
         * The direct mouse bridge detects the actual Button
         * and calls HandleExternalButton() exactly once.
         */
        button.onClick.RemoveAllListeners();
    }

    Debug.Log(
        "[SCN17] Buttons registered: " +
        buttons.Count);
}
public void HandleButton()
        {
            GameObject selected =
                EventSystem.current != null
                    ? EventSystem.current.currentSelectedGameObject
                    : null;

            if (selected == null)
            {
                Debug.LogWarning(
                    "[SCN17] HandleButton: no selected button.");
                return;
            }

            HandleButton(selected.GetComponent<Button>());
        }

        public void HandleButton(Button button)
        {
            if (button == null)
            {
                Debug.LogWarning(
                    "[SCN17] HandleButton: null button.");
                return;
            }

            GameObject selected = button.gameObject;

            string key =
                selected.name.Substring(
                    Math.Min(9, selected.name.Length)).Trim();

            Debug.Log("[SCN17] Handling: " + key);

            if (selected.name.StartsWith(
                    "BUTTON - SELECT UNIT - ",
                    StringComparison.OrdinalIgnoreCase))
            {
                SCN17RuntimeUnitSelector selector =
                    GetComponent<SCN17RuntimeUnitSelector>();

                if (selector != null)
                    selector.SelectUnitFromButton(selected.name);
                else
                    Debug.LogWarning("[SCN17] Runtime unit selector is missing.");

                return;
            }

            SCN17PersistentUnitRecordPanels recordPanels =
                FindFirstObjectByType<SCN17PersistentUnitRecordPanels>();

            if (recordPanels != null &&
                recordPanels.HandleButton(selected.name))
            {
                PersistentMilitaryRuntime.Save();
                RefreshAll();
                return;
            }

            switch (key.ToUpperInvariant())
            {
                case "UNIT HISTORY":
                    ShowUnitHistory();
                    break;

                case "AFTER ACTION":
            UnityEngine.SceneManagement.SceneManager.LoadScene("After_Action_HUD");
            break;

                case "OPERATIONS":
            UnityEngine.SceneManagement.SceneManager.LoadScene("Operations");
            break;

                case "CLOSE":
                    CloseRecord();
                    break;

                case "MILITARY DEVELOPMENT":
                    ShowMilitaryDevelopment();
                    break;

                case "GARAGE":
            UnityEngine.SceneManagement.SceneManager.LoadScene("SCN04_GARAGE");
            break;

                case "LOSSES":
                    ShowLosses();
                    break;

                case "VETERAN UNITS":
                    ShowVeterans();
                    break;

                case "OPERATIONAL HISTORY":
                    ShowOperationalHistory();
                    break;

                case "LOGISTICS":
            UnityEngine.SceneManagement.SceneManager.LoadScene("Logistics_Bay");
            break;

                case "VIEW FULL RECORD":
                    OpenRecordDetailWindow();
                    break;

                case "RESEARCH":
            UnityEngine.SceneManagement.SceneManager.LoadScene("Research_Lab");
            break;

                case "EQUIPMENT HISTORY":
                    ShowEquipmentHistory();
                    break;

                case "REPAIRS":
                    ShowRepairHistory();
                    break;

                case "COMMAND CENTER":
            UnityEngine.SceneManagement.SceneManager.LoadScene("Command_Center");
            break;

                case "DAMAGE HISTORY":
                    ShowDamageHistory();
                    break;
            }

            PersistentMilitaryRuntime.Save();
            RefreshAll();
        }

        private void ShowMilitaryDevelopment()
        {
            ShowMainPanel("MILITARY DEVELOPMENT");

            PersistentMilitarySaveData before =
                PersistentMilitaryRuntime.Data;

            int oldLevel =
                before != null ? before.MilitaryDevelopmentLevel : 0;

            int oldExperience =
                before != null ? before.MilitaryDevelopmentExperience : 0;

            Debug.Log(
                "[SCN17] Military Development BEFORE: " +
                "LEVEL=" + oldLevel +
                " XP=" + oldExperience);


        }

        private void ShowUnitHistory()
        {
            ShowMainPanel("UNIT HISTORY");

            SetDisplay(
                "UNIT HISTORY TERMINAL A SCREEN",
                BuildUnitHistory());

            SetDisplay(
                "SERVICE HISTORY DISPLAY",
                BuildServiceHistory());
        }

        private void ShowAfterAction()
        {
            CloseAllSecondaryDisplays();

            SetDisplay(
                "COMBAT HISTORY",
                BuildCombatHistory());
        }

        private void ShowOperations()
        {
            ShowMainPanel("OPERATIONAL HISTORY");

            SetDisplay(
                "MISSION HISTORY SCREEN",
                BuildMissionHistory());

            SetDisplay(
                "OPERATIONAL HISTORY",
                BuildOperationalHistory());
        }

        private void CloseRecord()
        {
            SetObjectActive("RECORD DETAIL WINDOW", false);
            SetObjectActive("UNIT HISTORY COMMAND WALL BODY", false);

            Debug.Log("[SCN17] Record detail closed.");
        }

        private void ShowLosses()
        {
            CloseAllSecondaryDisplays();

            SetDisplay(
                "MILITARY LOSSES BODY",
                BuildLosses());
        }

        private void ShowVeterans()
        {
            CloseAllSecondaryDisplays();

            SetDisplay(
                "VETERAN TERMINAL A SCREEN",
                BuildVeterans());

            SetDisplay(
                "VETERAN UNIT ARCHIVE BODY",
                BuildVeterans());
        }

        private void ShowOperationalHistory()
        {
            ShowMainPanel("OPERATIONAL HISTORY");

            SetDisplay(
                "OPERATIONAL HISTORY BODY",
                BuildOperationalHistory());

            SetDisplay(
                "OPERATIONAL EXPERIENCE SCREEN",
                BuildExperience());
        }

        public void OpenRecordDetailWindow()
        {
            SetObjectActive("RECORD DETAIL WINDOW", true);
            ShowFullRecord();
            Debug.Log("[SCN17] Record Detail Window opened.");
        }
        private void ShowFullRecord()
        {
            GameObject window = GetObject("RECORD DETAIL WINDOW");
            if (window == null)
            {
                Debug.LogWarning("[SCN17] RECORD DETAIL WINDOW not found.");
                return;
            }

            SetObjectActive("RECORD DETAIL WINDOW", true);

            Transform existing = window.transform.Find("RUNTIME FULL RECORD TEXT");
            TextMeshProUGUI display = existing != null
                ? existing.GetComponent<TextMeshProUGUI>()
                : null;

            if (display == null)
            {
                GameObject textObject = new GameObject(
                    "RUNTIME FULL RECORD TEXT",
                    typeof(RectTransform),
                    typeof(CanvasRenderer),
                    typeof(TextMeshProUGUI));

                textObject.transform.SetParent(window.transform, false);
                RectTransform rect = textObject.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(0.06f, 0.08f);
                rect.anchorMax = new Vector2(0.94f, 0.66f);
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;

                display = textObject.GetComponent<TextMeshProUGUI>();
                display.fontSize = 14;
                display.color = Color.white;
                display.textWrappingMode = TextWrappingModes.Normal;
                display.alignment = TextAlignmentOptions.TopLeft;
                display.overflowMode = TextOverflowModes.Overflow;
                display.raycastTarget = false;
            }

            display.gameObject.SetActive(true);
            display.text = BuildFullRecord();
            Debug.Log("[SCN17] Full unit records displayed.");
        }

        private void ShowEquipmentHistory()
        {
            ShowMainPanel("EQUIPMENT TIMELINE SCREEN");

            SetDisplay(
                "EQUIPMENT HISTORY BODY",
                BuildEquipmentHistory());

            SetDisplay(
                "EQUIPMENT HISTORY",
                BuildEquipmentHistory());
        }

        private void ShowRepairHistory()
        {
            ShowMainPanel("RECOVERY STATUS SCREEN");

            SetDisplay(
                "REPAIR HISTORY SCREEN",
                BuildRepairHistory());

            SetDisplay(
                "RECOVERY STATUS SCREEN",
                BuildRecoveryStatus());
        }
        private void RepairAll()
        {
            PersistentMilitarySaveData data =
                PersistentMilitaryRuntime.Data;

            if (data == null || data.Units == null)
                return;

            foreach (MilitaryUnitRecord unit in data.Units)
            {
                if (unit == null)
                    continue;

                if (unit.Damage > 0)
                    PersistentMilitaryRuntime.RepairUnit(
                        unit.UnitId,
                        100);
            }

            ShowMainPanel("RECOVERY STATUS SCREEN");

            SetDisplay(
                "REPAIR HISTORY SCREEN",
                BuildRepairHistory());

            SetDisplay(
                "RECOVERY STATUS SCREEN",
                BuildRecoveryStatus());
        }

        private void ShowDamageHistory()
        {
            CloseAllSecondaryDisplays();

            SetDisplay(
                "DAMAGE HISTORY SCREEN",
                BuildDamageHistory());

            SetDisplay(
                "DAMAGE / REPAIR HISTORY BODY",
                BuildDamageHistory());
        }

        private void LoadSceneExact(string scenePath)
    {
        if (string.IsNullOrWhiteSpace(scenePath))
        {
            Debug.LogWarning("[SCN17] Scene path is empty.");
            return;
        }

        if (!System.IO.File.Exists(scenePath))
        {
            Debug.LogError(
                "[SCN17] Scene file not found: " +
                scenePath);
            return;
        }

        string sceneName =
            System.IO.Path.GetFileNameWithoutExtension(scenePath);

        Debug.Log(
            "[SCN17] Loading scene: " +
            sceneName +
            " | " +
            scenePath);

        SceneManager.LoadScene(sceneName);
    }
    public void HandleExternalButton(string buttonName)
{
    if (string.IsNullOrWhiteSpace(buttonName))
    {
        Debug.LogWarning(
            "[SCN17] External button name is empty.");
        return;
    }

    /*
     * Find the actual Button in the HUD hierarchy.
     * Do not depend on the object-name cache because SCN17
     * contains duplicate display/object names.
     */
    Button target = null;

    Button[] allButtons =
        GetComponentsInChildren<Button>(true);

    foreach (Button button in allButtons)
    {
        if (button == null)
            continue;

        if (string.Equals(
                button.gameObject.name,
                buttonName,
                StringComparison.Ordinal))
        {
            target = button;
            break;
        }
    }

    if (target == null)
    {
        Debug.LogWarning(
            "[SCN17] Button not found in HUD: " +
            buttonName);
        return;
    }

    if (EventSystem.current != null)
    {
        EventSystem.current.SetSelectedGameObject(
            target.gameObject);
    }

    Debug.Log(
        "[SCN17] Dispatching: " +
        buttonName);

    HandleButton(target);
}
private void RefreshAll()
        {
            PersistentMilitarySaveData data =
                PersistentMilitaryRuntime.Data;

            if (data == null)
                return;

            UpdateDisplay(
                "SYSTEM STATUS SCREEN",
                "DATABASE ONLINE\n\n" +
                "UNITS       " + data.Units.Count + "\n" +
                "OPERATIONS  " + data.TotalOperations + "\n" +
                "VICTORIES   " + data.TotalVictories + "\n" +
                "LOSSES      " + data.TotalLosses + "\n" +
                "REPAIRS     " + data.TotalRepairs);

            UpdateDisplay(
                "COMMANDER DEVELOPMENT SCREEN",
                "COMMANDER DEVELOPMENT\n\n" +
                "LEVEL       " + data.CommanderLevel + "\n" +
                "EXPERIENCE  " + data.CommanderExperience + "\n" +
                "OPERATIONS  " + data.TotalOperations + "\n" +
                "VICTORIES   " + data.TotalVictories);

            UpdateDisplay(
                "MILITARY DEVELOPMENT HUD",
                "MILITARY DEVELOPMENT\n\n" +
                "LEVEL       " + data.MilitaryDevelopmentLevel + "\n" +
                "EXPERIENCE  " + data.MilitaryDevelopmentExperience + "\n" +
                "TECHNOLOGY  " + data.TechnologyLevel + "\n" +
                "DOCTRINE    " + data.DoctrineLevel);

            UpdateDisplay("RECOVERY STATUS SCREEN", BuildRecoveryStatus());
            UpdateDisplay("UNIT HISTORY TERMINAL A SCREEN", BuildUnitHistory());
            UpdateDisplay("VETERAN TERMINAL A SCREEN", BuildVeterans());
            UpdateDisplay("EQUIPMENT HISTORY BODY", BuildEquipmentHistory());
            UpdateDisplay("OPERATIONAL HISTORY BODY", BuildOperationalHistory());
            UpdateDisplay("DAMAGE / REPAIR HISTORY BODY", BuildDamageHistory());
            UpdateDisplay("MILITARY LOSSES BODY", BuildLosses());
            UpdateDisplay("UNIT HISTORY COMMAND WALL BODY", BuildFullRecord());
            UpdateDisplay("SERVICE HISTORY DISPLAY", BuildServiceHistory());
            UpdateDisplay("COMBAT HISTORY", BuildCombatHistory());
        }

        private void ShowMainPanel(string panelName)
        {
            CloseAllSecondaryDisplays();
            CloseAllMainPanels();

            SetPanelActive(panelName, true);

            Debug.Log("[SCN17] Main panel opened: " + panelName);
        }

        private void CloseAllMainPanels()
        {
            foreach (string panelName in MainPanels)
                SetPanelActive(panelName, false);
        }

        private void CloseAllSecondaryDisplays()
        {
            foreach (string displayName in SecondaryDisplays)
                SetObjectActive(displayName, false);
        }

        private void SetPanelActive(string objectName, bool active)
        {
            GameObject target = GetObject(objectName);

            if (target == null)
            {
                Debug.LogError("[SCN17] PANEL NOT FOUND in controller cache: " + objectName);
                return;
            }

            target.SetActive(active);

            Debug.Log(
                "[SCN17] PANEL ACTIVATION: " + objectName +
                " | selected=" + target.name +
                " | activeSelf=" + target.activeSelf +
                " | activeInHierarchy=" + target.activeInHierarchy +
                " | path=" + GetHierarchyPath(target.transform));
        }

        private string GetHierarchyPath(Transform target)
        {
            if (target == null)
                return "(null)";

            string path = target.name;
            Transform parent = target.parent;

            while (parent != null)
            {
                path = parent.name + "/" + path;
                parent = parent.parent;
            }

            return path;
        }

        private void SetObjectActive(string objectName, bool active)
        {
            GameObject target = GetObject(objectName);

            if (target != null)
                target.SetActive(active);
        }

        private void UpdateDisplay(string objectName, string text)
        {
            GameObject target = GetObject(objectName);

            if (target == null)
                return;

            TMP_Text[] texts =
                target.GetComponentsInChildren<TMP_Text>(true);

            foreach (TMP_Text tmp in texts)
            {
                if (tmp == null)
                    continue;

                tmp.text = text;
            }
        }

        private void SetDisplay(string objectName, string text)
        {
            GameObject target = GetObject(objectName);

            if (target == null)
                return;

            ActivateDisplayHierarchy(target);

            TMP_Text[] texts =
                target.GetComponentsInChildren<TMP_Text>(true);

            if (texts.Length == 0)
                return;

            foreach (TMP_Text tmp in texts)
            {
                if (tmp == null)
                    continue;

                tmp.gameObject.SetActive(true);
                tmp.enabled = true;
                tmp.text = text;
            }
        }

        private void ActivateDisplayHierarchy(GameObject target)
        {
            if (target == null)
                return;

            Transform current = target.transform;

            while (current != null)
            {
                current.gameObject.SetActive(true);
                current = current.parent;
            }
        }

        private string BuildUnitHistory()
        {
            if (PersistentMilitaryRuntime.Data == null ||
                PersistentMilitaryRuntime.Data.Units == null)
            {
                Debug.LogWarning("[SCN17] Unit history data is not initialized.");
                return "UNIT HISTORY DATA NOT INITIALIZED";
            }

            List<MilitaryUnitRecord> units =
                PersistentMilitaryRuntime.Data.Units;

            if (units.Count == 0)
                return "NO UNIT RECORDS";

            System.Text.StringBuilder sb =
                new System.Text.StringBuilder();

            sb.AppendLine("PERSISTENT UNIT HISTORY");
            sb.AppendLine();

            foreach (MilitaryUnitRecord unit in units)
            {
                if (unit == null)
                    continue;

                sb.AppendLine(unit.UnitId);
                sb.AppendLine("TYPE       " + unit.UnitType);
                sb.AppendLine("STATUS     " + unit.Status);
                sb.AppendLine("MISSIONS   " + unit.Missions);
                sb.AppendLine("VICTORIES  " + unit.Victories);
                sb.AppendLine("LOSSES     " + unit.Losses);
                sb.AppendLine("DAMAGE     " + unit.Damage.ToString("0.0") + "%");
                sb.AppendLine("REPAIRS    " + unit.Repairs);
                sb.AppendLine("VETERAN    " + unit.VeteranLevel);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private string BuildServiceHistory()
        {
            if (PersistentMilitaryRuntime.Data == null)
                return "SERVICE HISTORY DATA NOT INITIALIZED";

            PersistentMilitarySaveData data =
                PersistentMilitaryRuntime.Data;

            return "SERVICE HISTORY\n\n" +
                   "PERSISTENT RECORDS\n" +
                   "ALL UNIT SERVICE IS RETAINED\n\n" +
                   "OPERATIONS  " + data.TotalOperations + "\n" +
                   "LOSSES      " + data.TotalLosses + "\n" +
                   "REPAIRS     " + data.TotalRepairs;
        }

        private string BuildVeterans()
        {
            if (PersistentMilitaryRuntime.Data == null ||
                PersistentMilitaryRuntime.Data.Units == null)
            {
                Debug.LogWarning("[SCN17] Unit history data is not initialized.");
                return "UNIT HISTORY DATA NOT INITIALIZED";
            }

            List<MilitaryUnitRecord> units =
                PersistentMilitaryRuntime.Data.Units;

            System.Text.StringBuilder sb =
                new System.Text.StringBuilder();

            sb.AppendLine("VETERAN UNIT ARCHIVE");
            sb.AppendLine();

            bool found = false;

            foreach (MilitaryUnitRecord unit in units)
            {
                if (unit == null || unit.VeteranLevel <= 0)
                    continue;

                found = true;

                sb.AppendLine(unit.UnitId);
                sb.AppendLine("VETERAN LEVEL " + unit.VeteranLevel);
                sb.AppendLine("MISSIONS " + unit.Missions);
                sb.AppendLine("VICTORIES " + unit.Victories);
                sb.AppendLine();
            }

            if (!found)
                sb.AppendLine("NO VETERAN UNITS");

            return sb.ToString();
        }

        private string BuildEquipmentHistory()
        {
            return "EQUIPMENT HISTORY\n\n" +
                   "REPAIRS COMPLETED  " +
                   PersistentMilitaryRuntime.Data.TotalRepairs +
                   "\n\nUNIT EQUIPMENT RECORDS\n" +
                   "PERSISTENT SERVICE DATA ONLINE";
        }

        private string BuildOperationalHistory()
        {
            PersistentMilitarySaveData data =
                PersistentMilitaryRuntime.Data;

            return "OPERATIONAL HISTORY\n\n" +
                   "TOTAL OPERATIONS  " + data.TotalOperations + "\n" +
                   "VICTORIES         " + data.TotalVictories + "\n" +
                   "LOSSES            " + data.TotalLosses +
                   "\n\nPERSISTENT OPERATIONAL RECORD ONLINE";
        }

        private string BuildExperience()
        {
            PersistentMilitarySaveData data =
                PersistentMilitaryRuntime.Data;

            return "OPERATIONAL EXPERIENCE\n\n" +
                   "COMMANDER XP  " + data.CommanderExperience + "\n" +
                   "OPERATIONS    " + data.TotalOperations + "\n" +
                   "VICTORIES     " + data.TotalVictories;
        }

        private string BuildDamageHistory()
        {
            System.Text.StringBuilder sb =
                new System.Text.StringBuilder();

            sb.AppendLine("DAMAGE / REPAIR HISTORY");
            sb.AppendLine();

            foreach (MilitaryUnitRecord unit
                in PersistentMilitaryRuntime.Data.Units)
            {
                if (unit == null)
                    continue;

                if (unit.Damage <= 0 && unit.Repairs <= 0)
                    continue;

                sb.AppendLine(unit.UnitId);
                sb.AppendLine("DAMAGE  " + unit.Damage.ToString("0.0") + "%");
                sb.AppendLine("REPAIRS " + unit.Repairs);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private string BuildRepairHistory()
        {
            return "REPAIR HISTORY\n\n" +
                   "TOTAL REPAIRS  " +
                   PersistentMilitaryRuntime.Data.TotalRepairs +
                   "\n\nRECOVERY SYSTEM ONLINE";
        }

        private string BuildRecoveryStatus()
        {
            int damaged = 0;

            foreach (MilitaryUnitRecord unit
                in PersistentMilitaryRuntime.Data.Units)
            {
                if (unit == null)
                    continue;

                if (unit.Damage > 0)
                    damaged++;
            }

            return "RECOVERY STATUS\n\n" +
                   "DAMAGED UNITS   " + damaged + "\n" +
                   "TOTAL REPAIRS   " +
                   PersistentMilitaryRuntime.Data.TotalRepairs +
                   "\n\n" +
                   (damaged == 0
                       ? "ALL UNITS OPERATIONAL"
                       : "RECOVERY REQUIRED");
        }

        private string BuildMissionHistory()
        {
            PersistentMilitarySaveData data =
                PersistentMilitaryRuntime.Data;

            return "MISSION HISTORY\n\n" +
                   "OPERATIONS  " + data.TotalOperations + "\n" +
                   "VICTORIES   " + data.TotalVictories + "\n" +
                   "LOSSES      " + data.TotalLosses;
        }

        private string BuildCombatHistory()
        {
            PersistentMilitarySaveData data =
                PersistentMilitaryRuntime.Data;

            return "COMBAT HISTORY\n\n" +
                   "VICTORIES  " + data.TotalVictories + "\n" +
                   "LOSSES     " + data.TotalLosses + "\n" +
                   "OPERATIONS " + data.TotalOperations;
        }

        private string BuildLosses()
        {
            PersistentMilitarySaveData data =
                PersistentMilitaryRuntime.Data;

            return "LOSSES AND MEMORIAL\n\n" +
                   "TOTAL UNITS LOST\n\n" +
                   data.TotalLosses +
                   "\n\nALL LOSS RECORDS ARE PERSISTENT";
        }

        private string BuildFullRecord()
        {
            ObsidianProtocol.Game.Deployment.BattlefieldDeploymentContext context =
                ObsidianProtocol.Game.Deployment.BattlefieldDeploymentContext.Instance;

            string serviceRecordId = context != null
                ? context.SelectedServiceRecordId
                : string.Empty;

            if (string.IsNullOrWhiteSpace(serviceRecordId))
                return "UNIT SERVICE RECORD\n\nNO UNIT SELECTED\n\nSelect a unit first, then open its full record.";

            MilitaryUnitRecord unit =
                PersistentMilitaryRuntime.GetUnit(serviceRecordId);

            if (unit == null)
                return "UNIT SERVICE RECORD\n\nRECORD NOT FOUND\n\nSERVICE ID: " + serviceRecordId;

            return "FULL UNIT SERVICE RECORD\n\n" +
                   "UNIT ID         " + unit.UnitId + "\n" +
                   "UNIT TYPE       " + unit.UnitType + "\n" +
                   "STATUS          " + unit.Status + "\n" +
                   "MISSIONS        " + unit.Missions + "\n" +
                   "VICTORIES       " + unit.Victories + "\n" +
                   "LOSSES          " + unit.Losses + "\n" +
                   "DAMAGE          " + unit.Damage + "%\n" +
                   "REPAIRS         " + unit.Repairs + "\n" +
                   "EXPERIENCE      " + unit.Experience + "\n" +
                   "VETERAN LEVEL   " + unit.VeteranLevel + "\n" +
                   "LAST OPERATION  " + (string.IsNullOrEmpty(unit.LastOperation) ? "NONE" : unit.LastOperation) + "\n" +
                   "LAST UPDATE     " + (string.IsNullOrEmpty(unit.LastUpdate) ? "N/A" : unit.LastUpdate);
        }
    }
}

































