using UnityEngine;

namespace Obsidian.VR
{
    /// <summary>
    /// Manages lifecycle events for VR-controlled units:
    /// spawn, death, revive, operator link, and state propagation.
    /// </summary>
    public class VRUnitLifeCycle : MonoBehaviour
    {
        [SerializeField] private VRSessionManager _session;
        [SerializeField] private VRRuntimeAdapter _runtime;

        private BaseUnitVRController _unit;
        private VRUnitContext _context;

        public System.Action OnUnitSpawned;
        public System.Action OnUnitDied;
        public System.Action OnUnitRevived;

        private bool _wasAlive;

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

            MonitorLifeCycle();
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

            _wasAlive = _unit.IsAlive();
            OnUnitSpawned?.Invoke();
        }

        private void MonitorLifeCycle()
        {
            bool alive = _unit.IsAlive();

            if (alive && !_wasAlive)
            {
                OnUnitRevived?.Invoke();
            }
            else if (!alive && _wasAlive)
            {
                OnUnitDied?.Invoke();
            }

            _wasAlive = alive;
        }
    }
}
