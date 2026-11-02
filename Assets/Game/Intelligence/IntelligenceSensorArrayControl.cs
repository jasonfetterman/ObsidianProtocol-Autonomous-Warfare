using UnityEngine;

namespace ObsidianProtocol.Game.Intelligence
{
    public sealed class IntelligenceSensorArrayControl : MonoBehaviour
    {
        public void SelectArray(string sensorId)
        {
            IntelligenceRuntime runtime = IntelligenceRuntime.Instance;

            if (runtime == null || runtime.Sensors == null)
            {
                Debug.LogWarning("[INTELLIGENCE] SENSOR ARRAY: Sensor system unavailable.");
                return;
            }

            if (!runtime.Sensors.TryGetSensor(sensorId, out SensorDefinition sensor))
            {
                Debug.LogWarning(
                    "[INTELLIGENCE] SENSOR ARRAY: " + sensorId + " not found.");
                return;
            }

            Debug.Log(
                "[INTELLIGENCE] SENSOR ARRAY SELECTED: " +
                sensor.SensorId +
                " | TYPE=" + sensor.Type +
                " | RANGE=" + sensor.Range +
                " KM" +
                " | ACCURACY=" + (sensor.Accuracy * 100f).ToString("0") + "%" +
                " | STATUS=" + sensor.Status);
        }
    }
}