using UnityEngine;
using ObsidianProtocol.Game.AirWarfare;

namespace ObsidianProtocol.Game.Units
{
    public interface IUnitManualControl
    {
        void EnterManualControl(Transform controller);
        void ExitManualControl();
    }

    public sealed class UnitTakeoverController : MonoBehaviour
    {
        [Header("Takeover")]
        [SerializeField] private float interactionDistance = 3f;
        [SerializeField] private KeyCode takeoverKey = KeyCode.F;
        [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 4f, -7f);
        [SerializeField] private float mouseSensitivity = 2f;
        [SerializeField] private float maxLookAngle = 90f;

        [Header("Player")]
        [SerializeField] private Camera playerCamera;

        private SCN17FirstPersonWalkController walking;
        private CharacterController character;
        private Transform originalCameraParent;
        private Vector3 originalCameraPosition;
        private Quaternion originalCameraRotation;
        private Transform controlledUnit;
        private UnitAutonomy controlledAutonomy;
        private IUnitManualControl activeDriver;
        private bool takingOver;
        private float cameraPitch;
        private Camera unitCamera;
        private bool originalPlayerCameraEnabled = true;
        private bool originalUnitCameraEnabled;
        private Quaternion unitCameraBaseLocalRotation;

        public bool IsTakingOver => takingOver;
        public Transform ControlledUnit => controlledUnit;
        public Vector3 CameraOffset
        {
            get => cameraOffset;
            set => cameraOffset = value;
        }
        public Quaternion CameraRotationOffset
        {
            get => cameraRotationOffset;
            set => cameraRotationOffset = value;
        }

        [SerializeField] private Quaternion cameraRotationOffset = Quaternion.identity;

        private void Awake()
        {
            walking = GetComponent<SCN17FirstPersonWalkController>();
            character = GetComponent<CharacterController>();

            if (playerCamera == null && walking != null)
                playerCamera = walking.playerCamera;

            if (playerCamera != null)
            {
                originalCameraParent = playerCamera.transform.parent;
                originalCameraPosition = playerCamera.transform.localPosition;
                originalCameraRotation = playerCamera.transform.localRotation;
                originalPlayerCameraEnabled = playerCamera.enabled;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(takeoverKey))
            {
                if (takingOver)
                    ExitTakeover();
                else
                    TryEnterTakeover();
            }


        }

        private void LateUpdate()
        {
            if (!takingOver || controlledUnit == null || unitCamera == null)
                return;

            HandleTakeoverLook();
        }

        private void HandleTakeoverLook()
        {
            if (controlledUnit == null || unitCamera == null)
                return;

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            if (activeDriver is ScoutEyeFlightBehaviour flightDriver)
                flightDriver.QueueManualYaw(mouseX);
            else
                controlledUnit.Rotate(Vector3.up, mouseX, Space.World);

            cameraPitch = Mathf.Clamp(
                cameraPitch - mouseY,
                -maxLookAngle,
                maxLookAngle);
            unitCamera.transform.localRotation =
                Quaternion.Euler(cameraPitch, 0f, 0f) *
                unitCameraBaseLocalRotation;


        }

        private Camera FindUnitCamera(Transform unit)
        {
            if (unit == null)
                return null;

            Camera[] cameras = unit.GetComponentsInChildren<Camera>(true);
            Camera fallback = null;

            foreach (Camera candidate in cameras)
            {
                if (candidate == null || candidate == playerCamera)
                    continue;

                if (candidate.gameObject.name ==
                    "UNIT CAMERA - ADJUST IN SCENE VIEW")
                    return candidate;

                if (fallback == null)
                    fallback = candidate;
            }

            return fallback;
        }

        private void TryEnterTakeover()
        {
            if (playerCamera == null)
                return;

            Collider[] nearby = Physics.OverlapSphere(
                transform.position,
                interactionDistance);

            float closestDistance = float.MaxValue;
            Transform bestUnit = null;
            UnitAutonomy bestAutonomy = null;
            IUnitManualControl bestDriver = null;

            foreach (Collider hit in nearby)
            {
                if (hit.transform.root == transform.root)
                    continue;

                MonoBehaviour[] behaviours =
                    hit.GetComponentsInParent<MonoBehaviour>();

                foreach (MonoBehaviour behaviour in behaviours)
                {
                    if (!(behaviour is IUnitManualControl driver))
                        continue;

                    Transform candidate = behaviour.transform;

                    if (candidate == transform ||
                        candidate.IsChildOf(transform))
                        continue;

                    float distance = Vector3.Distance(
                        transform.position,
                        candidate.position);

                    if (distance >= closestDistance)
                        continue;

                    closestDistance = distance;
                    bestUnit = candidate;
                    bestDriver = driver;
                    bestAutonomy = candidate.GetComponent<UnitAutonomy>();

                    break;
                }
            }

            if (bestUnit == null || bestDriver == null)
            {
                Debug.Log("[UNIT TAKEOVER] No compatible unit in range.");
                return;
            }

            unitCamera = FindUnitCamera(bestUnit);

            if (unitCamera == null)
            {
                Debug.LogError(
                    "[UNIT TAKEOVER] No Camera found under " + bestUnit.name +
                    ". Add a Camera child to this unit in the Hierarchy.");
                return;
            }

            originalUnitCameraEnabled = unitCamera.enabled;
            unitCameraBaseLocalRotation = unitCamera.transform.localRotation;

            controlledUnit = bestUnit;
            controlledAutonomy = bestAutonomy;
            activeDriver = bestDriver;

            // Force a predictable external chase-camera position.
            cameraOffset = new Vector3(0f, 4f, -7f);
            cameraRotationOffset = Quaternion.identity;

            if (controlledAutonomy != null)
                controlledAutonomy.SetManualControlActive(true);

            activeDriver.EnterManualControl(transform);
            takingOver = true;

            if (playerCamera != null)
                playerCamera.enabled = false;

            unitCamera.gameObject.SetActive(true);
            unitCamera.enabled = true;
            cameraPitch = 0f;

            if (character != null)

            if (character != null)
                character.enabled = false;

            if (walking != null)
                walking.enabled = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Debug.Log("[UNIT TAKEOVER] Entered: " + controlledUnit.name);
        }
        public void ExitTakeover()
        {
            if (!takingOver)
                return;

            if (activeDriver != null)
                activeDriver.ExitManualControl();

            if (controlledAutonomy != null)
                controlledAutonomy.SetManualControlActive(false);

            if (unitCamera != null)
                unitCamera.enabled = originalUnitCameraEnabled;

            if (playerCamera != null)
                playerCamera.enabled = originalPlayerCameraEnabled;

            if (character != null)
                character.enabled = true;

            if (walking != null)
                walking.enabled = true;

            controlledUnit = null;
            controlledAutonomy = null;
            activeDriver = null;
            takingOver = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Debug.Log("[UNIT TAKEOVER] Returned to player.");
        }

        private void OnDisable()
        {
            if (takingOver)
                ExitTakeover();
        }
    }
}












