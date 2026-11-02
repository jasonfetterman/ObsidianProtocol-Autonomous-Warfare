using UnityEngine;

namespace ObsidianProtocol.Game.Intelligence
{
    public sealed class IntelligenceSensorArrayTarget : MonoBehaviour
    {
        [SerializeField] private string sensorId;

        public void Select()
        {
            IntelligenceRuntime.Instance?.GetType();
            var control = FindFirstObjectByType<IntelligenceSensorArrayControl>();
            if (control != null)
                control.SelectArray(sensorId);
        }
    }
}
