using UnityEngine;
using UnityEngine.InputSystem;

public class GarageOfficeActivation : MonoBehaviour
{
    [SerializeField] private string panelName;
    [SerializeField] private float interactionDistance = 5f;

    private FirstPersonController player;
    private GaragePanelManager panelManager;

    private void FindReferences()
    {
        if (player == null)
        {
            var players = FindObjectsByType<FirstPersonController>(
                FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var candidate in players)
            {
                if (candidate.gameObject.activeInHierarchy)
                {
                    player = candidate;
                    Debug.Log("[OFFICE] PLAYER FOUND: " + player.name);
                    break;
                }
            }
        }

        if (panelManager == null)
            panelManager = FindFirstObjectByType<GaragePanelManager>();
    }

    private void Update()
    {
        FindReferences();

        if (Keyboard.current == null || !Keyboard.current.eKey.wasPressedThisFrame)
            return;

        if (player == null || panelManager == null)
        {
            Debug.LogError("[OFFICE] PLAYER MISSING: " + (player == null) + " | MANAGER MISSING: " + (panelManager == null));
            return;
        }

        float distance = Vector3.Distance(player.transform.position, transform.position);
        Debug.Log("[OFFICE] E PRESSED | " + panelName + " | DISTANCE: " + distance);

        if (distance > interactionDistance)
            return;

        panelManager.ShowPanel(panelName);
        player.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}

