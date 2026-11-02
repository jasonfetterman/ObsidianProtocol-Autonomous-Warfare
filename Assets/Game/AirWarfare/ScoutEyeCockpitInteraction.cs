using UnityEngine;
using ObsidianProtocol.Game.AirWarfare;

public sealed class ScoutEyeCockpitInteraction : MonoBehaviour
{
    [SerializeField] private Transform drone;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private Vector3 cockpitOffset = new Vector3(0f, 0.2f, 0.35f);
    [SerializeField] private float mouseSensitivity = 2f;

    private SCN17FirstPersonWalkController walking;
    private CharacterController character;
    private ScoutEyeFlightBehaviour flight;
    private Camera playerCamera;
    private Transform originalCameraParent;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    private float pitch;
    private bool piloting;

    private void Awake()
    {
        walking = GetComponent<SCN17FirstPersonWalkController>();
        character = GetComponent<CharacterController>();

        if (drone == null)
        {
            GameObject found = GameObject.Find(
                "Meshy_AI_Emberhawk_Drone_1015183626_texture");

            if (found != null)
                drone = found.transform;
        }

        if (drone != null)
            flight = drone.GetComponent<ScoutEyeFlightBehaviour>();

        if (walking != null)
            playerCamera = walking.playerCamera;

        if (playerCamera != null)
        {
            originalCameraParent = playerCamera.transform.parent;
            originalCameraPosition = playerCamera.transform.localPosition;
            originalCameraRotation = playerCamera.transform.localRotation;
        }

        if (flight != null)
            flight.enabled = false;
    }

    private void Update()
    {
        // Input is handled by UnitTakeoverController.
    }
    private bool CanEnter()
    {
        return drone != null &&
               flight != null &&
               playerCamera != null &&
               Vector3.Distance(transform.position, drone.position)
                   <= interactionDistance;
    }

    private void EnterCockpit()
    {
        piloting = true;

        if (character != null)
            character.enabled = false;

        if (walking != null)
            walking.enabled = false;

        playerCamera.transform.SetParent(drone, false);
        playerCamera.transform.localPosition = cockpitOffset;
        playerCamera.transform.localRotation = Quaternion.identity;
        pitch = 0f;

        flight.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ExitCockpit()
    {
        piloting = false;

        if (flight != null)
        {
            flight.enabled = false;
        }

        if (playerCamera != null)
        {
            playerCamera.transform.SetParent(originalCameraParent, false);
            playerCamera.transform.localPosition = originalCameraPosition;
            playerCamera.transform.localRotation = originalCameraRotation;
        }

        if (character != null)
            character.enabled = true;

        if (walking != null)
            walking.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        if (piloting)
            ExitCockpit();
    }
}




