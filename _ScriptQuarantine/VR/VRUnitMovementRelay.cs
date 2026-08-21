using UnityEngine;

namespace Obsidian.VR
{
    /// <summary>
    /// Relays VR movement input to the unit's locomotion system.
    /// Ensures VR operator movement does not conflict with AI navigation.
    /// </summary>
    public class VRUnitMovementRelay : MonoBehaviour
    {
        [SerializeField] private VRSessionManager _session;
        [SerializeField] private VRRuntimeAdapter _runtime;

        private BaseUnitVRController _unit;
        private VRUnitContext _context;

        private CharacterController _controller;

        [Header("Movement Settings")]
        public float moveSpeed = 4f;
        public float acceleration = 10f;

        private Vector3 _currentVelocity;

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

            if (_unit == null || _context == null || _controller == null)
                return;

            HandleMovement();
        }

        private void BindToActiveUnit()
        {
            _unit = _session?.ActiveUnit;

            if (_unit == null)
            {
                _context = null;
                _controller = null;
                return;
            }

            _context = _unit.GetComponent<VRUnitContext>();
            if (_context == null)
                _context = _unit.gameObject.AddComponent<VRUnitContext>();

            _controller = _unit.GetComponent<CharacterController>();
            if (_controller == null)
                _controller = _unit.gameObject.AddComponent<CharacterController>();
        }

        private void HandleMovement()
        {
            if (!_context.IsOperatorLinked)
                return;

            Vector2 input = _runtime.GetMoveVector();
            if (input.sqrMagnitude < 0.01f)
            {
                _currentVelocity = Vector3.Lerp(_currentVelocity, Vector3.zero, Time.deltaTime * acceleration);
                _controller.Move(_currentVelocity * Time.deltaTime);
                return;
            }

            Transform head = Camera.main.transform;

            Vector3 forward = head.forward;
            forward.y = 0f;
            forward.Normalize();

            Vector3 right = head.right;
            right.y = 0f;
            right.Normalize();

            Vector3 targetVelocity = (forward * input.y + right * input.x) * moveSpeed;

            _currentVelocity = Vector3.Lerp(_currentVelocity, targetVelocity, Time.deltaTime * acceleration);

            _controller.Move(_currentVelocity * Time.deltaTime);

            _context.IsMoving = true;
            _context.Speed = _currentVelocity.magnitude;
        }
    }
}
