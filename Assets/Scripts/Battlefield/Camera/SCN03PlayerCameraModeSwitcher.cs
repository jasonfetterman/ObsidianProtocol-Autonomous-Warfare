using UnityEngine;

public class SCN03PlayerCameraModeSwitcher : MonoBehaviour
{
    public Camera flightCamera;
    public Behaviour flightController;
    public Camera playerCamera;
    public AudioListener flightAudioListener;
    public AudioListener playerAudioListener;
    public SCN17FirstPersonWalkController playerMovement;

    private bool firstPersonActive;

    private void Start()
    {
        SetFirstPerson(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
            SetFirstPerson(!firstPersonActive);
    }

    private void SetFirstPerson(bool enabled)
    {
        firstPersonActive = enabled;

        if (flightCamera != null) flightCamera.enabled = !enabled;
        if (flightController != null) flightController.enabled = !enabled;
        if (flightAudioListener != null) flightAudioListener.enabled = !enabled;
        if (playerCamera != null) playerCamera.enabled = enabled;
        if (playerAudioListener != null) playerAudioListener.enabled = enabled;
        if (playerMovement != null) playerMovement.enabled = enabled;

        CharacterController controller = GetComponent<CharacterController>();
        if (controller != null) controller.enabled = enabled;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
