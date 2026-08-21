using UnityEngine;

namespace Obsidian.VR
{
    /// <summary>
    /// Handles linking and unlinking the VR operator to a unit.
    /// Ensures AI yields control cleanly and VR systems activate.
    /// </summary>
    public class VROperatorLink : MonoBehaviour
    {
        [SerializeField] private VRSessionManager _session;
        [SerializeField] private VRRuntimeAdapter _runtime;

        private BaseUnitVRController _unit;
        private VRUnitContext _context;

        private bool _isLinked;

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

            HandleLinkState();
        }

        private void BindToActiveUnit()
        {
            _unit = _session?.ActiveUnit;

            if (_unit == null)
            {
                _context = null;
                _isLinked = false;
                return;
            }

            _context = _unit.GetComponent<VRUnitContext>();
            if (_context == null)
                _context = _unit.gameObject.AddComponent<VRUnitContext>();
        }

        private void HandleLinkState()
        {
            // Operator presses "link" button
            if (_runtime.IsLinkPressed() && !_isLinked)
            {
                LinkOperator();
            }

            // Operator presses "unlink" button
            if (_runtime.IsUnlinkPressed() && _isLinked)
            {
                UnlinkOperator();
            }
        }

        private void LinkOperator()
        {
            _isLinked = true;
            _context.IsOperatorLinked = true;

            // Disable AI immediately
            if (_unit.TryGetComponent(out UnitAIController ai))
                ai.enabled = false;

            Debug.Log("[VR] Operator linked to unit.");
        }

        private void UnlinkOperator()
        {
            _isLinked = false;
            _context.IsOperatorLinked = false;

            // Restore AI if it was previously enabled
            if (_unit.TryGetComponent(out UnitAIController ai))
                ai.enabled = true;

            Debug.Log("[VR] Operator unlinked from unit.");
        }
    }
}
