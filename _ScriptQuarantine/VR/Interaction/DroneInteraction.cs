using UnityEngine;
using UnityEngine.InputSystem;

public class DroneInteraction : MonoBehaviour
{
    [SerializeField] private Transform cockpitPoint;
    [SerializeField] private Transform xrOrigin;

    private Vector3 savedPosition;
    private Quaternion savedRotation;
    private bool inCockpit;

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (inCockpit)
                ExitCockpit();
            else
                EnterCockpit();
        }
    }

    public void EnterCockpit()
    {
        if (cockpitPoint == null || xrOrigin == null)
        {
            Debug.LogWarning("DroneInteraction: CockpitPoint or XR Origin is not assigned.");
            return;
        }

        savedPosition = xrOrigin.position;
        savedRotation = xrOrigin.rotation;

        xrOrigin.position = cockpitPoint.position;
        xrOrigin.rotation = cockpitPoint.rotation;

        inCockpit = true;

        Debug.Log("Entered drone cockpit.");
    }

    public void ExitCockpit()
    {
        if (!inCockpit || xrOrigin == null)
            return;

        xrOrigin.position = savedPosition;
        xrOrigin.rotation = savedRotation;

        inCockpit = false;

        Debug.Log("Exited drone cockpit.");
    }
}