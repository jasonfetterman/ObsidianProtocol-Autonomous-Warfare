using UnityEngine;

namespace Obsidian.VR
{
    public class VRUnitSelectionManager : MonoBehaviour
    {
        [SerializeField] private VRSessionManager _session;
        [SerializeField] private VRRuntimeAdapter _runtime;

        private BaseUnitVRController _unit;
        private VRUnitContext _context;

        [Header("Raycast Settings")]
        public float rayDistance = 10f;
        public LayerMask interactMask;

        private Transform _cameraTransform;

        private void Awake()
        {
            if (_session == null)
                _session = Object.FindAnyObjectByType<VRSessionManager>();

            if (_runtime == null)
                _runtime = Object.FindAnyObjectByType<VRRuntimeAdapter>();

            _cameraTransform = Camera.main?.transform;
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

            if (_unit == null || _context == null || _cameraTransform == null)
                return;

            HandleSelection();
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

        private void HandleSelection()
        {
            Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayDistance, interactMask))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null && _runtime.IsInteractPressed())
                {
                    interactable.Interact(_unit);
                }
            }
        }
    }
}
