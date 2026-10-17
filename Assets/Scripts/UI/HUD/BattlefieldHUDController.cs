using UnityEngine;
using UnityEngine.UI;

public class BattlefieldHUDController : MonoBehaviour
{
    [Header("Command Intent")]
    [SerializeField] private Button commandButton;
    [SerializeField] private GameObject commandIntentWindow;

    private void Awake()
    {
        if (commandButton == null)
        {
            Debug.LogError("[BATTLEFIELD HUD] COMMAND BUTTON REFERENCE MISSING.");
            return;
        }

        if (commandIntentWindow == null)
        {
            Debug.LogError("[BATTLEFIELD HUD] COMMAND INTENT WINDOW REFERENCE MISSING.");
            return;
        }

        commandButton.onClick.RemoveListener(OpenCommandIntent);
        commandButton.onClick.AddListener(OpenCommandIntent);

        commandIntentWindow.SetActive(false);

        Debug.Log("[BATTLEFIELD HUD] COMMAND ? COMMAND INTENT WIRED.");
    }

    public void OpenCommandIntent()
    {
        if (commandIntentWindow == null)
            return;

        commandIntentWindow.SetActive(true);

        Debug.Log("[BATTLEFIELD HUD] COMMAND INTENT WINDOW OPENED.");
    }

    public void CloseCommandIntent()
    {
        if (commandIntentWindow == null)
            return;

        commandIntentWindow.SetActive(false);

        Debug.Log("[BATTLEFIELD HUD] COMMAND INTENT WINDOW CLOSED.");
    }
}
