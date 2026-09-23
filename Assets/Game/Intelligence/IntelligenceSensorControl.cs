using UnityEngine;

namespace ObsidianProtocol.Game.Intelligence
{
    public sealed class IntelligenceSensorControl : MonoBehaviour
    {
        [SerializeField]
        private GameObject coverageDisplay;

        private void Awake()
        {
            if (coverageDisplay == null)
                coverageDisplay = GameObject.Find("SENSOR COVERAGE DISPLAY");
        }

        public void ToggleSensors()
        {
            if (IntelligenceRuntime.Instance == null ||
                IntelligenceRuntime.Instance.Sensors == null)
            {
                Debug.LogWarning("[INTELLIGENCE] Sensor control unavailable.");
                return;
            }

            SensorFramework sensors =
                IntelligenceRuntime.Instance.Sensors;

            bool anyActive = false;

            foreach (SensorDefinition sensor in sensors.GetSensors())
            {
                if (sensor.Status == SensorStatus.Active)
                {
                    anyActive = true;
                    break;
                }
            }

            foreach (SensorDefinition sensor in sensors.GetSensors())
            {
                if (sensor.Status == SensorStatus.Damaged)
                    continue;

                if (anyActive)
                    sensors.DeactivateSensor(sensor.SensorId);
                else
                    sensors.ActivateSensor(sensor.SensorId);
            }

            if (coverageDisplay != null)
                coverageDisplay.SetActive(!anyActive);

            Debug.Log(
                anyActive
                    ? "[INTELLIGENCE] SENSOR CONTROL: All sensors set to STANDBY. Coverage display CLOSED."
                    : "[INTELLIGENCE] SENSOR CONTROL: All sensors set to ACTIVE. Coverage display OPEN.");
        }
    }
}