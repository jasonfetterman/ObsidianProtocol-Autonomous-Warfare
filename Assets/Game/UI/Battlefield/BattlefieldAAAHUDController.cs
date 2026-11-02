using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ObsidianProtocol.Game.UI.Battlefield
{
    [DisallowMultipleComponent]
    public sealed class BattlefieldAAAHUDController : MonoBehaviour
    {
        private readonly Dictionary<string, GameObject> panels =
            new Dictionary<string, GameObject>(StringComparer.OrdinalIgnoreCase);

        public string CurrentAction { get; private set; } = "SELECT";

        private static readonly string[] PanelKeywords =
        {
            "DOCTRINE", "OPERATIONAL ORDERS", "ALERT DETAILS",
            "FORMATION MANAGEMENT", "SENSOR COVERAGE", "OBJECTIVES",
            "BATTLEFIELD LOGISTICS", "FORCE OVERVIEW", "CONTACT DETAILS",
            "COMMAND INTENT", "NETWORK STATUS"
        };

        private void Awake()
        {
            CachePanels();
            WireButtons();
            Debug.Log("[BATTLEFIELD AAA HUD] Controller initialized.");
        }

        private void CachePanels()
        {
            panels.Clear();

            foreach (string keyword in PanelKeywords)
            {
                string expectedName = "[WINDOW] " + keyword;

                foreach (Transform t in UnityEngine.Resources.FindObjectsOfTypeAll<Transform>())
                {
                    if (!t.gameObject.scene.IsValid() ||
                        t.gameObject.scene != gameObject.scene)
                        continue;

                    if (string.Equals(t.gameObject.name, expectedName,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        panels[keyword] = t.gameObject;
                        Debug.Log("[BATTLEFIELD AAA HUD] Panel found: " + keyword +
                                  " -> " + t.gameObject.name);
                        break;
                    }
                }

                if (!panels.ContainsKey(keyword))
                    Debug.LogWarning("[BATTLEFIELD AAA HUD] Panel not found: " + keyword);
            }
        }
        private void WireButtons()
        {
            foreach (Button b in UnityEngine.Resources.FindObjectsOfTypeAll<Button>())
            {
                if (!b.gameObject.scene.IsValid() ||
                    b.gameObject.scene != gameObject.scene)
                    continue;

                string n = Normalize(b.gameObject.name);

                if (n == "CLOSE DOCTRINE") Bind(b, () => ClosePanel("DOCTRINE"));
                else if (n == "CLOSE OPERATIONAL ORDERS") Bind(b, () => ClosePanel("OPERATIONAL ORDERS"));
                else if (n == "CLOSE ALERT DETAILS") Bind(b, () => ClosePanel("ALERT DETAILS"));
                else if (n == "CLOSE FORMATION MANAGEMENT") Bind(b, () => ClosePanel("FORMATION MANAGEMENT"));
                else if (n == "CLOSE SENSOR COVERAGE") Bind(b, () => ClosePanel("SENSOR COVERAGE"));
                else if (n == "CLOSE OBJECTIVES") Bind(b, () => ClosePanel("OBJECTIVES"));
                else if (n == "CLOSE BATTLEFIELD LOGISTICS") Bind(b, () => ClosePanel("BATTLEFIELD LOGISTICS"));
                else if (n == "CLOSE FORCE OVERVIEW") Bind(b, () => ClosePanel("FORCE OVERVIEW"));
                else if (n == "CLOSE CONTACT DETAILS") Bind(b, () => ClosePanel("CONTACT DETAILS"));
                else if (n == "CLOSE COMMAND INTENT") Bind(b, () => ClosePanel("COMMAND INTENT"));
                else if (n == "CLOSE NETWORK STATUS") Bind(b, () => ClosePanel("NETWORK STATUS"));
                else if (n == "DOCTRINE" || n == "OPEN DOCTRINE") Bind(b, () => OnDoctrineButtonClicked());
                else if (n == "OPERATIONAL ORDERS" || n == "OPEN OPERATIONAL ORDERS") Bind(b, () => OnOperationalOrdersButtonClicked());
                else if (n == "ALERT DETAILS" || n == "OPEN ALERT DETAILS") Bind(b, () => OnAlertDetailsButtonClicked());
                else if (n == "FORMATIONS" || n == "FORMATION MANAGEMENT" || n == "OPEN FORMATIONS") Bind(b, () => OnFormationManagementButtonClicked());
                else if (n == "SENSOR COVERAGE" || n == "OPEN SENSOR COVERAGE") Bind(b, () => OnSensorCoverageButtonClicked());
                else if (n == "OBJECTIVES" || n == "OPEN OBJECTIVES") Bind(b, () => OnObjectivesButtonClicked());
                else if (n == "BATTLEFIELD LOGISTICS" || n == "OPEN BATTLEFIELD LOGISTICS") Bind(b, () => OnBattlefieldLogisticsButtonClicked());
                else if (n == "FORCE OVERVIEW" || n == "OPEN FORCE OVERVIEW") Bind(b, () => OnForceOverviewButtonClicked());
                else if (n == "CONTACT DETAILS" || n == "OPEN CONTACT DETAILS") Bind(b, () => OnContactDetailsButtonClicked());
                else if (n == "COMMAND INTENT" || n == "OPEN COMMAND INTENT") Bind(b, () => OnCommandIntentButtonClicked());
                else if (n == "NETWORK STATUS" || n == "OPEN NETWORK STATUS") Bind(b, () => OnNetworkStatusButtonClicked());
                else if (n == "ACTION SELECT") Bind(b, () => OnSelectButtonClicked());
                else if (n == "ACTION MOVE") Bind(b, () => OnMoveButtonClicked());
                else if (n == "ACTION ORDERS") Bind(b, () => OnOrdersButtonClicked());
                else if (n == "ACTION ENGAGE") Bind(b, () => OnEngageButtonClicked());
                else if (n == "ACTION RECON") Bind(b, () => OnReconButtonClicked());
            }
        }

        private void Bind(Button button, UnityEngine.Events.UnityAction action)
        {
            button.onClick.AddListener(action);
            Debug.Log("[BATTLEFIELD AAA HUD] Runtime click handler attached: " +
                      button.gameObject.name);
        }

        public void OpenPanel(string key)
        {
            if (panels.TryGetValue(key, out GameObject panel) && panel != null)
                panel.SetActive(true);
            else
                Debug.LogWarning("[BATTLEFIELD AAA HUD] Cannot open missing panel: " + key);
        }

        public void ClosePanel(string key)
        {
            if (panels.TryGetValue(key, out GameObject panel) && panel != null)
                panel.SetActive(false);
            else
                Debug.LogWarning("[BATTLEFIELD AAA HUD] Cannot close missing panel: " + key);
        }

        public void SetAction(string action)
        {
            CurrentAction = action;
            Debug.Log("[BATTLEFIELD AAA HUD] Action selected: " + action);
        }

        // ACTION BUTTONS — assign these from each Button's On Click().

        public void OnSelectButtonClicked()
        {
            SetAction("SELECT");
        }

        public void OnMoveButtonClicked()
        {
            SetAction("MOVE");
        }

        public void OnOrdersButtonClicked()
        {
            SetAction("ORDERS");
        }

        public void OnEngageButtonClicked()
        {
            SetAction("ENGAGE");
        }

        public void OnReconButtonClicked()
        {
            SetAction("RECON");
        }

        // PANEL OPEN BUTTONS

        public void OnDoctrineButtonClicked()
        {
            OpenPanel("DOCTRINE");
        }

        public void OnOperationalOrdersButtonClicked()
        {
            OpenPanel("OPERATIONAL ORDERS");
        }

        public void OnAlertDetailsButtonClicked()
        {
            OpenPanel("ALERT DETAILS");
        }

        public void OnFormationManagementButtonClicked()
        {
            OpenPanel("FORMATION MANAGEMENT");
        }

        public void OnSensorCoverageButtonClicked()
        {
            OpenPanel("SENSOR COVERAGE");
        }

        public void OnObjectivesButtonClicked()
        {
            OpenPanel("OBJECTIVES");
        }

        public void OnBattlefieldLogisticsButtonClicked()
        {
            OpenPanel("BATTLEFIELD LOGISTICS");
        }

        public void OnForceOverviewButtonClicked()
        {
            OpenPanel("FORCE OVERVIEW");
        }

        public void OnContactDetailsButtonClicked()
        {
            OpenPanel("CONTACT DETAILS");
        }

        public void OnCommandIntentButtonClicked()
        {
            OpenPanel("COMMAND INTENT");
        }

        public void OnNetworkStatusButtonClicked()
        {
            OpenPanel("NETWORK STATUS");
        }

        // PANEL CLOSE BUTTONS

        public void OnCloseDoctrineButtonClicked() { ClosePanel("DOCTRINE"); }
        public void OnCloseOperationalOrdersButtonClicked() { ClosePanel("OPERATIONAL ORDERS"); }
        public void OnCloseAlertDetailsButtonClicked() { ClosePanel("ALERT DETAILS"); }
        public void OnCloseFormationManagementButtonClicked() { ClosePanel("FORMATION MANAGEMENT"); }
        public void OnCloseSensorCoverageButtonClicked() { ClosePanel("SENSOR COVERAGE"); }
        public void OnCloseObjectivesButtonClicked() { ClosePanel("OBJECTIVES"); }
        public void OnCloseBattlefieldLogisticsButtonClicked() { ClosePanel("BATTLEFIELD LOGISTICS"); }
        public void OnCloseForceOverviewButtonClicked() { ClosePanel("FORCE OVERVIEW"); }
        public void OnCloseContactDetailsButtonClicked() { ClosePanel("CONTACT DETAILS"); }
        public void OnCloseCommandIntentButtonClicked() { ClosePanel("COMMAND INTENT"); }
        public void OnCloseNetworkStatusButtonClicked() { ClosePanel("NETWORK STATUS"); }
        private static string Normalize(string value)
        {
            return (value ?? string.Empty)
                .Replace("[BUTTON]", "")
                .Replace("[PANEL]", "")
                .Replace("[WINDOW]", "")
                .Trim()
                .ToUpperInvariant();
        }
    }
}




