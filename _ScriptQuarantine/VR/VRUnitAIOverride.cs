using UnityEngine;

namespace Obsidian.VR
{
    /// <summary>
    /// Overrides AI behavior when a VR operator takes control.
    /// Ensures AI is disabled during VR control and restored afterward.
    /// </summary>
    public class VRUnitAIOverride : MonoBehaviour
    {
        [SerializeField] private VRSessionManager _session;
        [SerializeField] private VRRuntimeAdapter _runtime;

        private BaseUnitVRController _unit;
        private VRUnitContext _context;
        private UnitAIController _ai;

        private bool _aiWasEnabled;

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

            ApplyOverrideLogic();
        }

        private void BindToActiveUnit()
        {
            _unit = _session?.ActiveUnit;

            if (_unit == null)
            {
                _context = null;
                _ai = null;
                return;
            }

            _context = _unit.GetComponent<VRUnitContext>();
            if (_context == null)
                _context = _unit.gameObject.AddComponent<VRUnitContext>();

            _ai = _unit.GetComponent<UnitAIController>();
            if (_ai != null)
                _aiWasEnabled = _ai.enabled;
        }

        private void ApplyOverrideLogic()
        {
            if (_context.IsOperatorLinked)
            {
                // Disable AI while VR operator is in control
                if (_ai != null && _ai.enabled)
                    _ai.enabled = false;
            }
            else
            {
                // Restore AI when operator disconnects
                if (_ai != null)
                    _ai.enabled = _aiWasEnabled;
            }
        }
    }
}
