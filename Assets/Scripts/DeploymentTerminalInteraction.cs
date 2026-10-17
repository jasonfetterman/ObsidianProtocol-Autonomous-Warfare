using UnityEngine;

public class DeploymentTerminalInteraction : MonoBehaviour
{
    [SerializeField] private GameObject deploymentHUD;
    [SerializeField] private float interactionDistance = 3.0f;

    private Camera playerCamera;
    private CommanderPlayerMovement commanderMovement;

    private void Start()
    {
        playerCamera = Camera.main;

        commanderMovement = FindFirstObjectByType<CommanderPlayerMovement>();

        if (deploymentHUD != null)
            deploymentHUD.SetActive(false);
    }

    private void Update()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;

            if (playerCamera == null)
                return;
        }

        if (!Input.GetKeyDown(KeyCode.E))
            return;

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (!Physics.Raycast(
            ray,
            out RaycastHit hit,
            interactionDistance
        ))
            return;

        DeploymentTerminalInteraction terminal =
            hit.collider.GetComponentInParent<DeploymentTerminalInteraction>();

        if (terminal != this)
            return;

        OpenDeploymentHUD();
    }

    private void OpenDeploymentHUD()
    {
        if (deploymentHUD == null)
        {
            Debug.LogError(
                "[DEPLOYMENT TERMINAL] Deployment HUD reference is missing."
            );
            return;
        }

        if (commanderMovement == null)
            commanderMovement = FindFirstObjectByType<CommanderPlayerMovement>();

        if (commanderMovement != null)
            commanderMovement.enabled = false;

        deploymentHUD.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log(
            "[DEPLOYMENT TERMINAL] AUTHORIZATION CONSOLE ACTIVATED."
        );

        Debug.Log(
            "[DEPLOYMENT TERMINAL] COMMANDER MOVEMENT DISABLED."
        );

        Debug.Log(
            "[DEPLOYMENT TERMINAL] DEPLOYMENT HUD OPENED."
        );
    }

    public void CloseDeploymentHUD()
    {
        if (deploymentHUD != null)
            deploymentHUD.SetActive(false);

        if (commanderMovement == null)
            commanderMovement = FindFirstObjectByType<CommanderPlayerMovement>();

        if (commanderMovement != null)
            commanderMovement.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log(
            "[DEPLOYMENT TERMINAL] DEPLOYMENT HUD CLOSED."
        );

        Debug.Log(
            "[DEPLOYMENT TERMINAL] COMMANDER MOVEMENT ENABLED."
        );
    }
}
