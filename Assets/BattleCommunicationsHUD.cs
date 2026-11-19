using TMPro;
using UnityEngine;

public class BattleCommunicationsHUD : MonoBehaviour
{
    [SerializeField] private string currentMessage = "COMMUNICATIONS ONLINE";

    private TMP_Text communicationsText;

    private void Awake()
    {
        communicationsText = GetComponent<TMP_Text>();
        UpdateMessage();
    }

    public void SetMessage(string message)
    {
        currentMessage = string.IsNullOrWhiteSpace(message)
            ? "COMMUNICATIONS ONLINE"
            : message;

        UpdateMessage();
    }

    private void UpdateMessage()
    {
        if (communicationsText != null)
            communicationsText.text = currentMessage;
    }
}