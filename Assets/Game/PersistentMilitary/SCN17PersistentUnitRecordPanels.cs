using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace ObsidianProtocol.Game.PersistentMilitary
{
    public class SCN17PersistentUnitRecordPanels : MonoBehaviour
    {
        private static readonly string[] PanelNames =
        {
            "UNIT HISTORY",
            "VETERAN UNITS",
            "DAMAGE HISTORY",
            "REPAIRS",
            "LOSSES",
            "EQUIPMENT HISTORY",
            "OPERATIONAL HISTORY",
            "MILITARY DEVELOPMENT"
        };

        private readonly Dictionary<string, GameObject> panels =
            new Dictionary<string, GameObject>();

        private GameObject recordStack;

        private void Awake()
        {
            CachePanels();
            WireAllCloseButtons();
            HideAll();

            Debug.Log("[SCN17 RECORD] Record-panel controller initialized.");
        }

        // SCN17_DIRECT_CLOSE_HIT_TEST
        // Bypasses Button.onClick and UI event dispatch for Close buttons.
        private void Update()
        {
            if (Mouse.current == null ||
                !Mouse.current.leftButton.wasPressedThisFrame)
                return;

            Vector2 pointer = Mouse.current.position.ReadValue();

            Button[] closeButtons = FindObjectsByType<Button>(
                FindObjectsInactive.Exclude,
                FindObjectsSortMode.None);

            foreach (Button button in closeButtons)
            {
                if (button == null ||
                    button.gameObject.name != "BUTTON - CLOSE" ||
                    !button.isActiveAndEnabled ||
                    !button.interactable)
                    continue;

                RectTransform rect =
                    button.transform as RectTransform;

                if (rect == null)
                    continue;

                Canvas canvas = button.GetComponentInParent<Canvas>();

                Camera uiCamera = canvas != null &&
                    canvas.renderMode != RenderMode.ScreenSpaceOverlay
                    ? canvas.worldCamera
                    : null;

                if (!RectTransformUtility.RectangleContainsScreenPoint(
                    rect, pointer, uiCamera))
                    continue;

                Debug.Log(
                    "[SCN17 RECORD] DIRECT CLOSE HIT: " +
                    button.transform.parent.name);

                HideAll();
                return;
            }
        }
        private void CachePanels()
        {
            panels.Clear();

            Transform record = FindDeepChild(
                transform, "PERSISTENT UNIT RECORD");

            Transform stack = record != null
                ? FindDeepChild(record, "UNIT RECORD 8 PANEL STACK")
                : FindDeepChild(transform, "UNIT RECORD 8 PANEL STACK");

            recordStack = stack != null ? stack.gameObject : null;

            foreach (string panelName in PanelNames)
            {
                Transform child = stack != null
                    ? stack.Find("UNIT RECORD PANEL - " + panelName)
                    : FindDeepChild(
                        transform, "UNIT RECORD PANEL - " + panelName);

                if (child == null)
                {
                    Debug.LogWarning(
                        "[SCN17 RECORD] Missing panel: " + panelName);
                    continue;
                }

                panels[panelName] = child.gameObject;
                CreateCloseButton(child.gameObject);
            }

            if (recordStack == null)
            {
                Debug.LogError(
                    "[SCN17 RECORD] UNIT RECORD 8 PANEL STACK was not found.");
            }
        }

        private void WireAllCloseButtons()
        {
            Button[] buttons = FindObjectsByType<Button>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);

            int wired = 0;

            foreach (Button button in buttons)
            {
                if (button == null ||
                    button.gameObject.name != "BUTTON - CLOSE")
                    continue;

                button.onClick.AddListener(HideAll);

            SCN17CloseButtonPointerHandler pointerHandler =
                button.GetComponent<SCN17CloseButtonPointerHandler>();

            if (pointerHandler == null)
                pointerHandler =
                    button.gameObject.AddComponent<SCN17CloseButtonPointerHandler>();

            pointerHandler.Owner = this;
                wired++;
            }

            Debug.Log(
                "[SCN17 RECORD] Close-button listeners attached: " + wired);
        }

        private void CreateCloseButton(GameObject panel)
        {
            Transform existing = panel.transform.Find("BUTTON - CLOSE");

            if (existing != null)
            {
                Button existingButton = existing.GetComponent<Button>();

                if (existingButton == null)
                    existingButton = existing.gameObject.AddComponent<Button>();

                existingButton.interactable = true;
                existing.SetAsLastSibling();
                return;
            }

            GameObject buttonObject = new GameObject(
                "BUTTON - CLOSE",
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Image),
                typeof(Button));

            buttonObject.transform.SetParent(panel.transform, false);
            buttonObject.transform.SetAsLastSibling();

            RectTransform rect =
                buttonObject.GetComponent<RectTransform>();

            rect.anchorMin = new Vector2(1f, 1f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.pivot = new Vector2(1f, 1f);
            rect.anchoredPosition = new Vector2(-12f, -12f);
            rect.sizeDelta = new Vector2(110f, 40f);

            Image image = buttonObject.GetComponent<Image>();
            image.color = new Color(0.35f, 0.08f, 0.08f, 1f);
            image.raycastTarget = true;

            Button button = buttonObject.GetComponent<Button>();
            button.targetGraphic = image;
            button.interactable = true;

            GameObject labelObject = new GameObject(
                "LABEL",
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(TextMeshProUGUI));

            labelObject.transform.SetParent(buttonObject.transform, false);

            RectTransform labelRect =
                labelObject.GetComponent<RectTransform>();

            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;

            TextMeshProUGUI label =
                labelObject.GetComponent<TextMeshProUGUI>();

            label.text = "CLOSE";
            label.fontSize = 18f;
            label.alignment = TextAlignmentOptions.Center;
            label.color = Color.white;
            label.raycastTarget = false;
        }

        public bool HandleButton(string buttonName)
        {
            if (string.IsNullOrWhiteSpace(buttonName))
                return false;

            const string prefix = "BUTTON - ";

            if (!buttonName.StartsWith(prefix))
                return false;

            string logicalName =
                buttonName.Substring(prefix.Length).Trim();

            if (logicalName == "CLOSE")
            {
                HideAll();
                return true;
            }

            foreach (string panelName in PanelNames)
            {
                if (logicalName == panelName)
                {
                    ShowPanel(panelName);
                    return true;
                }
            }

            return false;
        }

        public void ShowPanel(string panelName)
        {
            if (string.IsNullOrWhiteSpace(panelName))
                return;

            if (!panels.ContainsKey(panelName))
                CachePanels();

            if (!panels.TryGetValue(panelName, out GameObject panel) ||
                panel == null)
            {
                Debug.LogWarning(
                    "[SCN17 RECORD] Panel not found: " + panelName);
                return;
            }

            HideAll();

            if (recordStack != null)
                recordStack.SetActive(true);

            panel.SetActive(true);
            PopulatePanel(panelName, panel);

            Debug.Log("[SCN17 RECORD] OPEN: " + panelName);
        }

        public void CloseAllPanels()
        {
            HideAll();
        }

        private void HideAll()
        {
            foreach (GameObject panel in panels.Values)
            {
                if (panel != null)
                    panel.SetActive(false);
            }

            if (recordStack != null)
                recordStack.SetActive(false);

            Debug.Log("[SCN17 RECORD] CLOSE requested; panel stack hidden.");
        }

        private void PopulatePanel(string panelName, GameObject panel)
        {
            TMP_Text title =
                panel.transform.Find("TITLE")?.GetComponent<TMP_Text>();

            TMP_Text body =
                panel.transform.Find("BODY")?.GetComponent<TMP_Text>();

            if (title != null)
                title.text = panelName;

            if (body == null)
                return;

            PersistentMilitarySaveData data =
                PersistentMilitaryRuntime.Data;

            if (data == null)
            {
                body.text =
                    "PERSISTENT MILITARY DATABASE\n\nDATABASE OFFLINE";
                return;
            }

            switch (panelName)
            {
                case "UNIT HISTORY":
                    string serviceRecordId = string.Empty;
                    ObsidianProtocol.Game.Deployment.BattlefieldDeploymentContext context =
                        ObsidianProtocol.Game.Deployment.BattlefieldDeploymentContext.Instance;
                    if (context != null)
                        serviceRecordId = context.SelectedServiceRecordId;

                    ObsidianProtocol.Game.PersistentMilitary.MilitaryUnitRecord record =
                        ObsidianProtocol.Game.PersistentMilitary.PersistentMilitaryRuntime.GetUnit(serviceRecordId);

                    string unitDetails = record != null
                        ? "\n\nSELECTED UNIT SERVICE RECORD\n" +
                          "UNIT TYPE       " + record.UnitType + "\n" +
                          "SERVICE ID      " + record.UnitId + "\n" +
                          "STATUS          " + record.Status + "\n" +
                          "MISSIONS        " + record.Missions + "\n" +
                          "VICTORIES       " + record.Victories + "\n" +
                          "LOSSES          " + record.Losses + "\n" +
                          "DAMAGE          " + record.Damage + "\n" +
                          "REPAIRS         " + record.Repairs + "\n" +
                          "EXPERIENCE      " + record.Experience + "\n" +
                          "VETERAN LEVEL   " + record.VeteranLevel + "\n" +
                          "LAST OPERATION  " + (string.IsNullOrEmpty(record.LastOperation) ? "NONE" : record.LastOperation) + "\n" +
                          "LAST UPDATE     " + (string.IsNullOrEmpty(record.LastUpdate) ? "N/A" : record.LastUpdate)
                        : "\n\nSELECTED UNIT SERVICE RECORD\nNO DEPLOYED UNIT SELECTED";

                    body.text =
                        "UNIT HISTORY\n\n" +
                        "DATABASE STATUS     ONLINE\n" +
                        "UNITS               " + data.Units.Count + "\n" +
                        "OPERATIONS          " + data.TotalOperations + "\n" +
                        "VICTORIES           " + data.TotalVictories + "\n" +
                        "LOSSES              " + data.TotalLosses + "\n" +
                        "REPAIRS             " + data.TotalRepairs +
                        unitDetails;
                    break;

                case "VETERAN UNITS":
                    body.text =
                        "VETERAN UNITS\n\nPERSISTENT UNIT ARCHIVE\n\n" +
                        "REGISTERED UNITS    " + data.Units.Count + "\n" +
                        "OPERATIONS          " + data.TotalOperations + "\n" +
                        "VICTORIES           " + data.TotalVictories + "\n\n" +
                        "VETERAN RECORD SYSTEM ONLINE";
                    break;

                case "DAMAGE HISTORY":
                    body.text =
                        "DAMAGE HISTORY\n\nPERSISTENT DAMAGE RECORDS\n\n" +
                        "REGISTERED UNITS    " + data.Units.Count + "\n" +
                        "TOTAL LOSSES        " + data.TotalLosses + "\n" +
                        "TOTAL REPAIRS       " + data.TotalRepairs;
                    break;

                case "REPAIRS":
                    body.text =
                        "REPAIRS\n\nRECOVERY / REPAIR RECORDS\n\n" +
                        "TOTAL REPAIRS       " + data.TotalRepairs + "\n" +
                        "REGISTERED UNITS    " + data.Units.Count + "\n\n" +
                        "REPAIR HISTORY ONLINE";
                    break;

                case "LOSSES":
                    body.text =
                        "LOSSES\n\nMILITARY LOSS RECORD\n\n" +
                        "TOTAL LOSSES        " + data.TotalLosses + "\n" +
                        "TOTAL OPERATIONS    " + data.TotalOperations + "\n" +
                        "REGISTERED UNITS    " + data.Units.Count;
                    break;

                case "EQUIPMENT HISTORY":
                    body.text =
                        "EQUIPMENT HISTORY\n\nPERSISTENT EQUIPMENT RECORD\n\n" +
                        "REGISTERED UNITS    " + data.Units.Count + "\n" +
                        "TOTAL REPAIRS       " + data.TotalRepairs + "\n" +
                        "DATABASE STATUS     ONLINE";
                    break;

                case "OPERATIONAL HISTORY":
                    body.text =
                        "OPERATIONAL HISTORY\n\n" +
                        "OPERATIONS          " + data.TotalOperations + "\n" +
                        "VICTORIES           " + data.TotalVictories + "\n" +
                        "LOSSES              " + data.TotalLosses + "\n" +
                        "REGISTERED UNITS    " + data.Units.Count;
                    break;

                case "MILITARY DEVELOPMENT":
                    body.text =
                        "MILITARY DEVELOPMENT\n\n" +
                        "MILITARY LEVEL      " + data.MilitaryDevelopmentLevel + "\n" +
                        "MILITARY XP         " + data.MilitaryDevelopmentExperience + "\n" +
                        "TECHNOLOGY LEVEL    " + data.TechnologyLevel + "\n" +
                        "DOCTRINE LEVEL      " + data.DoctrineLevel + "\n" +
                        "COMMANDER LEVEL     " + data.CommanderLevel + "\n" +
                        "COMMANDER XP        " + data.CommanderExperience;
                    break;
            }
        }

        private static Transform FindDeepChild(
            Transform root, string targetName)
        {
            if (root == null)
                return null;

            if (root.name == targetName)
                return root;

            for (int i = 0; i < root.childCount; i++)
            {
                Transform result = FindDeepChild(
                    root.GetChild(i), targetName);

                if (result != null)
                    return result;
            }

            return null;
        }
    }

    public class SCN17CloseButtonPointerHandler : MonoBehaviour, IPointerClickHandler
    {
        public SCN17PersistentUnitRecordPanels Owner;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Owner == null)
                Owner = FindFirstObjectByType<SCN17PersistentUnitRecordPanels>();

            if (Owner != null)
            {
                Debug.Log("[SCN17 RECORD] POINTER CLOSE RECEIVED");
                Owner.CloseAllPanels();
            }
            else
            {
                Debug.LogError("[SCN17 RECORD] Pointer handler could not find its controller.");
            }
        }
    }
}



