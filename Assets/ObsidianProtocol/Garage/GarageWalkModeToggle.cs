using UnityEngine;
using UnityEngine.UI;

public class GarageWalkModeToggle : MonoBehaviour
{
    [Header("Garage Cameras")]
    public Camera garageCamera;
    public Camera walkCamera;
    public GameObject walkPlayer;

    [Header("HUD")]
    public Canvas garageHudCanvas;

    [Header("Controls")]
    public KeyCode toggleKey = KeyCode.F8;

    private bool walking;

    private void Start()
    {
        SetWalkMode(true);
    }

    private void Update()
    {
        // Walking mode is automatic; no F8 toggle required.
    }

    private void SetWalkMode(bool enable)
    {
        walking = enable;

        if (garageHudCanvas != null)
            garageHudCanvas.enabled = !enable;

        if (garageCamera != null)
            garageCamera.enabled = !enable;

        if (walkCamera != null)
            walkCamera.enabled = enable;

        if (walkPlayer != null)
            walkPlayer.SetActive(enable);

        if (!enable)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}

