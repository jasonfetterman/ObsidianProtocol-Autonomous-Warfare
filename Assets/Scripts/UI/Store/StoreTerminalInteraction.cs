using UnityEngine;
using ObsidianProtocol.UI.Store;

[DisallowMultipleComponent]
public sealed class StoreTerminalInteraction : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 3.0f;
    private Camera playerCamera;
    private StoreMasterController storeController;
    private Collider terminalCollider;

    private void Start()
    {
        playerCamera = Camera.main;
        storeController = FindFirstObjectByType<StoreMasterController>();
        terminalCollider = GetComponent<Collider>();

        foreach (Transform item in FindObjectsByType<Transform>(
            FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (item.name != "Procurement terminal glass")
                continue;

            BoxCollider glassCollider = item.GetComponent<BoxCollider>();
            if (glassCollider == null)
                glassCollider = item.gameObject.AddComponent<BoxCollider>();

            glassCollider.center = Vector3.zero;
            glassCollider.size = Vector3.one;
            glassCollider.isTrigger = false;
            glassCollider.enabled = true;
        }

        Debug.Log("[STORE] Three procurement glass panels initialized.");
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.E)) return;
        if (playerCamera == null) playerCamera = Camera.main;
        if (playerCamera == null) return;

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward);

        if (!Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
            return;

        bool isTerminal = terminalCollider != null &&
                          hit.collider == terminalCollider;
        bool isProcurementGlass =
            hit.collider.gameObject.name == "Procurement terminal glass";

        if (!isTerminal && !isProcurementGlass)
            return;

        if (storeController == null)
            storeController = FindFirstObjectByType<StoreMasterController>();

        if (storeController == null)
        {
            Debug.LogError("[STORE] StoreMasterController not found.");
            return;
        }

        storeController.OpenStore();
    }
}
