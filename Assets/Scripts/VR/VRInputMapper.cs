using UnityEngine;
using UnityEngine.XR;

namespace Obsidian.VR
{
    /// <summary>
    /// Maps VR controller input into movement, firing, and targeting commands.
    /// Works with any BaseUnitVRController (UGV/UAV/USV/UUV/etc).
    /// </summary>
    public class VRInputMapper : MonoBehaviour
    {
        [Header("Runtime")]
        [SerializeField] private VRSessionManager _session;
        private BaseUnitVRController _unit;

        private void Awake()
        {
            if (_session == null)
                _session = Object.FindAnyObjectByType<VRSessionManager>();
        }

        private void Update()
        {
            if (_session == null)
                return;

            if (_session.Info.Mode != VRMode.Operator)
                return;

            int id = _session.Info.ActiveUnitId;
            if (id < 0)
                return;

            if (_unit == null || _unit.UnitId != id)
                _unit = FindUnit(id);

            if (_unit == null)
                return;

            HandleMovement();
            HandleWeapons();
            HandleTargeting();
        }

        private BaseUnitVRController FindUnit(int id)
        {
            var units = Object.FindObjectsByType<BaseUnitVRController>();
            foreach (var u in units)
            {
                if (u.UnitId == id)
                    return u;
            }
            return null;
        }

        private void HandleMovement()
        {
            Vector2 leftStick = GetAxis(XRNode.LeftHand);
            Vector2 rightStick = GetAxis(XRNode.RightHand);

            float throttle = leftStick.y;
            float strafe = leftStick.x;
            float yaw = rightStick.x;
            float pitch = -rightStick.y;

            _unit.ApplyMovement(throttle, strafe, yaw, pitch);
        }

        private void HandleWeapons()
        {
            bool trigger = GetTrigger(XRNode.RightHand);
            if (trigger)
                _unit.FirePrimary();
        }

        private void HandleTargeting()
        {
            bool grip = GetGrip(XRNode.LeftHand);
            if (grip)
                _unit.CycleTarget();
        }

        private Vector2 GetAxis(XRNode node)
        {
            Vector2 axis = Vector2.zero;
            InputDevice device = InputDevices.GetDeviceAtXRNode(node);

            if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 value))
                axis = value;

            return axis;
        }

        private bool GetTrigger(XRNode node)
        {
            InputDevice device = InputDevices.GetDeviceAtXRNode(node);
            if (device.TryGetFeatureValue(CommonUsages.triggerButton, out bool pressed))
                return pressed;

            return false;
        }

        private bool GetGrip(XRNode node)
        {
            InputDevice device = InputDevices.GetDeviceAtXRNode(node);
            if (device.TryGetFeatureValue(CommonUsages.gripButton, out bool pressed))
                return pressed;

            return false;
        }
    }
}
