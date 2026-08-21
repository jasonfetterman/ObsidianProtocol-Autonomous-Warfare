using UnityEngine;

namespace Obsidian.VR
{
    /// <summary>
    /// Tracks and updates VR unit power state (battery, charge, drain).
    /// Works with any BaseUnitVRController that exposes battery level.
    /// </summary>
    public class VRUnitPowerState : MonoBehaviour
    {
        [SerializeField] private VRSessionManager _session;
        [SerializeField] private VRRuntimeAdapter _runtime;

        private BaseUnitVRController _unit;

        private float _batteryLevel;
        private float _lastUpdateTime;

        private void Awake()
        {
            if (_session == null)
                _session = Object.FindAnyObjectByType<VRSessionManager>();

            if (_runtime == null)
                _runtime = Object.FindAnyObjectByType<VRRuntimeAdapter>();
        }

        private void Start()
        {
            _unit = _session?.ActiveUnit;
            _lastUpdateTime = Time.time;
        }

        private void Update()
        {
            if (_session == null || _runtime == null)
                return;

            if (_unit == null)
                _unit = _session.ActiveUnit;

            if (_unit == null)
                return;

            float now = Time.time;
            float delta = now - _lastUpdateTime;

            _batteryLevel = _unit.GetBatteryLevel();

            // Optional drain logic:
            // _batteryLevel -= delta * 0.1f;

            _batteryLevel = Mathf.Clamp(_batteryLevel, 0f, 100f);

            _lastUpdateTime = now;
        }

        public float GetBatteryLevel()
        {
            return _batteryLevel;
        }
    }
}
