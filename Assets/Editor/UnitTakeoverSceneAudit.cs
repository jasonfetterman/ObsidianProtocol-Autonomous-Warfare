using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using ObsidianProtocol.Game.Units;

public static class UnitTakeoverSceneAudit
{
    [MenuItem("Obsidian Protocol/Unit Takeover/Audit Open Scene")]
    private static void AuditOpenScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log("[TAKEOVER AUDIT] Scene: " + scene.name);

        foreach (SCN17FirstPersonWalkController player in
                 Resources.FindObjectsOfTypeAll<SCN17FirstPersonWalkController>())
        {
            if (player == null || player.gameObject.scene != scene)
                continue;

            UnitTakeoverController takeover =
                player.GetComponent<UnitTakeoverController>();

            Camera camera = takeover != null
                ? player.GetComponentInChildren<Camera>(true)
                : player.playerCamera;

            Debug.Log(
                "[TAKEOVER AUDIT] Player: " + GetPath(player.transform) +
                " | World position: " + player.transform.position +
                " | Takeover attached: " + (takeover != null) +
                " | Camera: " + (camera != null ? GetPath(camera.transform) : "NONE") +
                (camera != null ? " | Camera world position: " + camera.transform.position : "") +
                (takeover != null ? " | Camera offset: " + takeover.CameraOffset : ""),
                player.gameObject);
        }

        foreach (MonoBehaviour component in
                 Resources.FindObjectsOfTypeAll<MonoBehaviour>())
        {
            if (component == null || component.gameObject.scene != scene)
                continue;

            if (component is IUnitManualControl)
            {
                Debug.Log(
                    "[TAKEOVER AUDIT] Compatible unit: " +
                    GetPath(component.transform) +
                    " | Driver: " + component.GetType().Name +
                    " | World position: " + component.transform.position +
                    " | Local position: " + component.transform.localPosition,
                    component.gameObject);
            }
        }

        Debug.Log("[TAKEOVER AUDIT] Position diagnostics complete.");
    }

    private static string GetPath(Transform target)
    {
        string path = target.name;
        while (target.parent != null)
        {
            target = target.parent;
            path = target.name + "/" + path;
        }
        return path;
    }
}
