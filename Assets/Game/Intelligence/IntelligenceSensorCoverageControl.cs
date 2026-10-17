using UnityEngine;

namespace ObsidianProtocol.Game.Intelligence
{
    public sealed class IntelligenceSensorCoverageControl : MonoBehaviour
    {
        [SerializeField]
        private GameObject coverageDisplay;

        private void Awake()
        {
            if (coverageDisplay == null)
                coverageDisplay = FindCoverageDisplay();
        }

        public void ToggleCoverage()
        {
            if (coverageDisplay == null)
            {
                Debug.LogWarning("[INTELLIGENCE] SENSOR COVERAGE: Display not found.");
                return;
            }

            bool newState = !coverageDisplay.activeSelf;
            coverageDisplay.SetActive(newState);

            Debug.Log(
                newState
                    ? "[INTELLIGENCE] SENSOR COVERAGE: Coverage display OPEN."
                    : "[INTELLIGENCE] SENSOR COVERAGE: Coverage display CLOSED.");
        }

        private GameObject FindCoverageDisplay()
        {
            GameObject display = GameObject.Find("SENSOR COVERAGE DISPLAY");

            if (display != null)
                return display;

            display = GameObject.Find("SENSOR COVERAGE FRAME");

            if (display != null)
                return display;

            return GameObject.Find("SENSOR NETWORK");
        }
    }
}