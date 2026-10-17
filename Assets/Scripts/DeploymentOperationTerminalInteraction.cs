using UnityEngine;

public class DeploymentOperationTerminalInteraction : MonoBehaviour
{
    [SerializeField] private GameObject deploymentHUD;
    [SerializeField] private GameObject selectOperationWindow;
    [SerializeField] private float interactionDistance = 3.0f;

    private Camera playerCamera;
    private CommanderPlayerMovement commanderMovement;

    private void Start()
    {
        playerCamera = Camera.main;
        commanderMovement =
            FindFirstObjectByType<CommanderPlayerMovement>();
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

        DeploymentOperationTerminalInteraction terminal =
            hit.collider.GetComponentInParent<
                DeploymentOperationTerminalInteraction>();

        if (terminal != this)
            return;

        OpenOperationSelection();
    }

    private void OpenOperationSelection()
    {
        if (deploymentHUD == null)
        {
            Debug.LogError(
                "[OPERATION TERMINAL] Deployment HUD reference missing."
            );
            return;
        }

        if (selectOperationWindow == null)
        {
            Debug.LogError(
                "[OPERATION TERMINAL] Select Operation window reference missing."
            );
            return;
        }

        if (commanderMovement == null)
        {
            commanderMovement =
                FindFirstObjectByType<CommanderPlayerMovement>();
        }

        if (commanderMovement != null)
            commanderMovement.enabled = false;

        deploymentHUD.SetActive(true);
        selectOperationWindow.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log(
            "[OPERATION TERMINAL] OPERATION SELECTION ACTIVATED."
        );

        Debug.Log(
            "[OPERATION TERMINAL] COMMANDER MOVEMENT DISABLED."
        );

        Debug.Log(
            "[OPERATION TERMINAL] SELECT OPERATION WINDOW OPENED."
        );
    }
}
