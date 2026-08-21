using UnityEngine;

namespace Obsidian.VR
{
    /// <summary>
    /// Relays telemetry data (speed, heading, battery, etc.) from the unit
    /// to VR HUD systems or operator interfaces.
    /// </summary>
    public class VRUnitTelemetryRelay : MonoBehaviour
    {
        [SerializeField] private VRSessionManager _session;
        [SerializeField] private VRRuntimeAdapter _runtime;

        private BaseUnitVRController _unit;
        private VRUnitHUDRelay _hud;

        private float _speed;
        private float _heading;
        private float _battery;

        private void Awake()
        {
            if (_session == null)
                _session = Object.FindAnyObjectByType<VRSessionManager>();

            if (_runtime == null)
                _runtime = Object.FindAnyObjectByType<VRRuntimeAdapter>();
        }

        private void Start()
        {
            BindToActiveUnit();
        }

        private void Update()
        {
            if (_session == null || _runtime == null)
                return;

            if (_unit == null)
                BindToActiveUnit();

            if (_unit == null)
                return;

            UpdateTelemetry();
            RelayToHUD();
        }

        private void BindToActiveUnit()
        {
            _unit = _session?.ActiveUnit;

            if (_unit == null)
            {
                _hud = null;
                return;
            }

            _hud = _unit.GetComponent<VRUnitHUDRelay>();
            if (_hud == null)
                _hud = _unit.gameObject.AddComponent<VRUnitHUDRelay>();
        }

        private void UpdateTelemetry()
        {
            _speed = _unit.GetCurrentSpeed();
            _heading = _unit.transform.eulerAngles.y;
            _battery = _unit.GetBatteryLevel();
        }

        private void RelayToHUD()
        {
            if (_hud == null)
                return;

            _hud.SetSpeed(_speed);
            _hud.SetHeading(_heading);
            _hud.SetBattery(_battery);
        }
    }
}
