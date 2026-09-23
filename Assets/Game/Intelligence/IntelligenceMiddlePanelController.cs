using UnityEngine;

namespace ObsidianProtocol.Game.Intelligence
{
    public sealed class IntelligenceMiddlePanelController : MonoBehaviour
    {
        private void Open(string panelName)
        {
            Transform panel = transform.Find(panelName);

            if (panel != null)
                panel.gameObject.SetActive(true);
            else
                Debug.LogWarning("[INTELLIGENCE] Panel not found: " + panelName);
        }

        public void OpenSensors() => Open("SENSORS");
        public void OpenThreatAnalysis() => Open("THREAT ANALYSIS");
        public void OpenNetwork() => Open("NETWORK");
        public void OpenIntelligenceReports() => Open("INTELLIGENCE REPORTS");
        public void OpenContacts() => Open("CONTACTS");
        public void OpenSurveillance() => Open("SURVEILLANCE");
        public void OpenAnalysis() => Open("ANALYSIS");

        public void CloseThisPanel()
        {
            gameObject.SetActive(false);
        }
    }
}
