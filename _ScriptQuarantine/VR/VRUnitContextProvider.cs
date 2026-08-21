using UnityEngine;

namespace Obsidian.VR
{
    /// <summary>
    /// Provides global access to the VRUnitContext for any VR subsystem.
    /// Ensures all systems read from the same context instance.
    /// </summary>
    public class VRUnitContextProvider : MonoBehaviour
    {
        [SerializeField] private VRSessionManager _session;
        [SerializeField] private VRRuntimeAdapter _runtime;

        private BaseUnitVRController _unit;
        private VRUnitContext _context;

        public VRUnitContext Context => _context;

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
        }

        private void BindToActiveUnit()
        {
            _unit = _session?.ActiveUnit;

            if (_unit == null)
            {
                _context = null;
                return;
            }

            _context = _unit.GetComponent<VRUnitContext>();
            if (_context == null)
                _context = _unit.gameObject.AddComponent<VRUnitContext>();
        }
    }
}
