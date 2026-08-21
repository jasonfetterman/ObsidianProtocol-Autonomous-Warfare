using UnityEngine;

namespace Obsidian.VR
{
    /// <summary>
    /// Relays VR trigger input to the unit's weapon systems.
    /// Ensures VR operator firing does not conflict with AI firing.
    /// </summary>
    public class VRUnitWeaponRelay : MonoBehaviour
    {
        [SerializeField] private VRSessionManager _session;
        [SerializeField] private VRRuntimeAdapter _runtime;

        private BaseUnitVRController _unit;
        private VRUnitContext _context;

        private IWeaponModule[] _weapons;

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

            if (_unit == null || _context == null)
                return;

            HandleWeaponFire();
        }

        private void BindToActiveUnit()
        {
            _unit = _session?.ActiveUnit;

            if (_unit == null)
            {
                _context = null;
                _weapons = null;
                return;
            }

            _context = _unit.GetComponent<VRUnitContext>();
            if (_context == null)
                _context = _unit.gameObject.AddComponent<VRUnitContext>();

            _weapons = _unit.GetComponentsInChildren<IWeaponModule>();
        }

        private void HandleWeaponFire()
        {
            if (!_context.IsOperatorLinked)
                return;

            if (!_context.IsAlive)
                return;

            if (!_runtime.IsTriggerPressed())
                return;

            if (_weapons == null)
                return;

            foreach (var weapon in _weapons)
            {
                if (weapon == null)
                    continue;

                if (weapon.CanFire())
                    weapon.Fire();
            }
        }
    }
}
