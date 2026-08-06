using UnityEngine;

namespace Obsidian.VR
{
    /// <summary>
    /// Routes VR input signals to the correct relay components.
    /// This is the central hub for movement, weapon, and interaction input.
    /// </summary>
    public class VRUnitInputRouter : MonoBehaviour
    {
        [SerializeField] private VRSessionManager _session;
        [SerializeField] private VRRuntimeAdapter _runtime;

        private BaseUnitVRController _unit;

        private VRUnitInputRelay _movementRelay;
        private VRUnitWeaponRelay _weaponRelay;
        private VRUnitSelectionManager _selectionRelay;

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

            RouteInput();
        }

        private void BindToActiveUnit()
        {
            _unit = _session?.ActiveUnit;

            if (_unit == null)
            {
                _movementRelay = null;
                _weaponRelay = null;
                _selectionRelay = null;
                return;
            }

            _movementRelay = _unit.GetComponent<VRUnitInputRelay>();
            if (_movementRelay == null)
                _movementRelay = _unit.gameObject.AddComponent<VRUnitInputRelay>();

            _weaponRelay = _unit.GetComponent<VRUnitWeaponRelay>();
            if (_weaponRelay == null)
                _weaponRelay = _unit.gameObject.AddComponent<VRUnitWeaponRelay>();

            _selectionRelay = _unit.GetComponent<VRUnitSelectionManager>();
            if (_selectionRelay == null)
                _selectionRelay = _unit.gameObject.AddComponent<VRUnitSelectionManager>();
        }

        private void RouteInput()
        {
            _movementRelay?.enabled.Equals(true);
            _weaponRelay?.enabled.Equals(true);
            _selectionRelay?.enabled.Equals(true);
        }
    }
}
