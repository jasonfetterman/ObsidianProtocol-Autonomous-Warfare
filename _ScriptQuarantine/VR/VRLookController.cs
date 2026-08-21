using UnityEngine;
using UnityEngine.InputSystem;

namespace Obsidian.VR
{
    public class VRLookController : MonoBehaviour
    {
        [SerializeField] private Transform head;
        [SerializeField] private InputActionAsset inputActions;
        [SerializeField] private float lookSpeed = 90f;
        [SerializeField] private float minPitch = -80f;
        [SerializeField] private float maxPitch = 80f;

        private InputAction lookAction;
        private float pitch;

        private void Awake()
        {
            if (inputActions == null)
            {
                Debug.LogError("VRLookController: Input Actions asset is not assigned.");
                return;
            }

            lookAction = inputActions.FindAction("Look");

            if (lookAction == null)
            {
                Debug.LogError("VRLookController: Look action was not found.");
                return;
            }

            lookAction.Enable();

            if (head != null)
            {
                pitch = NormalizeAngle(head.localEulerAngles.x);
                pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
            }
        }

        private void Update()
        {
            if (head == null || lookAction == null)
                return;

            Vector2 input = lookAction.ReadValue<Vector2>();

            // RIGHT STICK LEFT / RIGHT
            transform.Rotate(
                Vector3.up,
                input.x * lookSpeed * Time.deltaTime,
                Space.World
            );

            // RIGHT STICK UP / DOWN
            pitch -= input.y * lookSpeed * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            Vector3 angles = head.localEulerAngles;

            head.localRotation = Quaternion.Euler(
                pitch,
                angles.y,
                angles.z
            );
        }

        private static float NormalizeAngle(float angle)
        {
            if (angle > 180f)
                angle -= 360f;

            return angle;
        }

        private void OnDestroy()
        {
            if (lookAction != null)
                lookAction.Disable();
        }
    }
}