using UnityEngine;
using UnityEngine.XR;

namespace Obsidian.VR
{
    public class VRControllerSetup : MonoBehaviour
    {
        [Header("Controller Objects")]
        public Transform leftController;
        public Transform rightController;

        private InputDevice leftDevice;
        private InputDevice rightDevice;

        private void Start()
        {
            FindControllers();
        }

        private void Update()
        {
            if (!leftDevice.isValid || !rightDevice.isValid)
                FindControllers();

            UpdateController(leftDevice, leftController);
            UpdateController(rightDevice, rightController);
        }

        private void FindControllers()
        {
            leftDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            rightDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        }

        private void UpdateController(InputDevice device, Transform controller)
        {
            if (!device.isValid || controller == null)
                return;

            if (device.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position))
                controller.localPosition = position;

            if (device.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation))
                controller.localRotation = rotation;
        }
    }
}