using UnityEngine;

public class GarageRuntimeAudit : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("[AUDIT] SCENE: " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().path);

        foreach (var p in FindObjectsByType<CommanderPlayerMovement>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            Debug.Log("[AUDIT] PLAYER: " + p.name + " | ACTIVE: " + p.gameObject.activeInHierarchy + " | ENABLED: " + p.enabled);

        foreach (var m in FindObjectsByType<GaragePanelManager>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            Debug.Log("[AUDIT] MANAGER: " + m.name + " | ACTIVE: " + m.gameObject.activeInHierarchy + " | ENABLED: " + m.enabled);
    }
}
