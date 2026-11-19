using TMPro;
using UnityEngine;

public class CommandStatusHUD : MonoBehaviour
{
    [SerializeField] private string status = "COMMAND ONLINE";

    private TMP_Text statusText;

    private void Awake()
    {
        statusText = GetComponent<TMP_Text>();
        UpdateStatus();
    }

    public void SetStatus(string newStatus)
    {
        status = newStatus;
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        if (statusText != null)
            statusText.text = status;
    }
}