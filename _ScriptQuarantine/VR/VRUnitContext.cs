using UnityEngine;

namespace Obsidian.VR
{
    public class VRUnitContext : MonoBehaviour
    {
        [SerializeField] private VRSessionManager _session;
        [SerializeField] private VRRuntimeAdapter _runtime;

        private BaseUnitVRController _unit;

        // ⭐ REQUIRED — fixes your error
        public BaseUnitVRController Unit => _unit;

        public bool Valid => _session != null && _runtime != null && _unit != null;

        public VRSessionManager Session => _session;
        public VRRuntimeAdapter Runtime => _runtime;

        // Context flags
        public bool IsAlive { get; set; }
        public bool IsOperatorLinked { get; set; }
        public bool IsInCombat { get; set; }
        public bool IsMoving { get; set; }

        // Cached values
        public float Health { get; set; }
        public float Battery { get; set; }
        public float Speed { get; set; }
        public int Ammo { get; set; }

        private void Awake()
        {
            if (_session == null)
                _session = FindAnyObjectByType<VRSessionManager>();

            if (_runtime == null)
                _runtime = FindAnyObjectByType<VRRuntimeAdapter>();
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

            UpdateContext();
        }

        private void BindToActiveUnit()
        {
            _unit = _session?.ActiveUnit;

            if (_unit == null)
            {
                IsAlive = false;
                IsOperatorLinked = false;
                return;
            }

            IsOperatorLinked = true;
        }

        private void UpdateContext()
        {
            IsAlive = _unit.IsAlive();
            Health = _unit.GetHealth();
            Battery = _unit.GetBatteryLevel();

            Speed = _unit.GetCurrentSpeed();
            IsMoving = Speed > 0.1f;

            IsInCombat = _unit.IsInCombat();
            Ammo = _unit.GetAmmoCount();
        }
    }
}
