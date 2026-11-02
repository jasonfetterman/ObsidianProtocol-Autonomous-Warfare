using UnityEngine;

public class VRCommandHUDProbe : MonoBehaviour
{
    void Start()
    {
        Debug.Log("=== VR COMMAND HUD ===");

        Transform hud = GameObject.Find("[HUD] VR COMMAND HUD")?.transform;

        if (hud == null)
        {
            Debug.LogError("VR HUD NOT FOUND");
            return;
        }

        Debug.Log("HUD FOUND: " + hud.name);
        Debug.Log("CHILD COUNT: " + hud.childCount);

        foreach (Transform child in hud)
        {
            Debug.Log("HUD CHILD: " + child.name + " | ACTIVE=" + child.gameObject.activeSelf);
        }

        GameObject xr = GameObject.Find("XR Origin (VR)");
        Debug.Log("XR ORIGIN: " + (xr != null ? "FOUND ACTIVE=" + xr.activeSelf : "NOT FOUND"));
    }
}
