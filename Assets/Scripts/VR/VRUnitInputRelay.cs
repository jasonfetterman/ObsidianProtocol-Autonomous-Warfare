using UnityEngine;

namespace Obsidian.VR
{
    /// <summary>
    /// Relays VR controller movement input to the active unit.
    /// Handles throttle, steering, and directional movement.
    /// </summary>
    public class VRUnitInputRelay : MonoBehaviour
    {
        [SerializeField] private VRSessionManager _session;
        [SerializeField] private VRRuntimeAdapter _runtime;

        private BaseUnitVRController _unit;
        private UnitMover _mover;

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

            if (_unit == null || _mover == null)
                return;

            HandleMovementInput();
        }

        private void BindToActiveUnit()
        {
            _unit = _session?.ActiveUnit;

            if (_unit == null)
            {
                _mover = null;
                return;
            }

            _mover = _unit.GetComponent<UnitMover>();
        }

        private void HandleMovementInput()
        {
            Vector2 move = _runtime.GetMoveVector();   // VR joystick / thumbstick
            float throttle = _runtime.GetThrottle();   // Trigger or grip input

            Vector3 forward = _unit.transform.forward * throttle;
            Vector3 strafe = _unit.transform.right * move.x;
            Vector3 motion = forward + strafe;

            _mover?.SetMoveInput(motion);
        }
    }
}
