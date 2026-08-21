using UnityEngine;

namespace Obsidian.VR
{
    public class VRUnitStatusRelay : MonoBehaviour
    {
        [SerializeField] private VRSessionManager _session;
        [SerializeField] private VRRuntimeAdapter _runtime;

        private BaseUnitVRController _unit;
        private VRUnitHUDRelay _hud;

        private float _health;
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

            UpdateStatus();
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

        private void UpdateStatus()
        {
            _health = _unit.GetHealth();
            _battery = _unit.GetBatteryLevel();
        }

        private void RelayToHUD()
        {
            if (_hud == null)
                return;

            _hud.SetHealth(_health);
            _hud.SetBattery(_battery);
        }
    }
}
