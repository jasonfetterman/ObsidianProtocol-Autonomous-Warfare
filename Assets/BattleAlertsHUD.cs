using TMPro;
using UnityEngine;

public class BattleAlertsHUD : MonoBehaviour
{
    [SerializeField] private string currentAlert = "NO ACTIVE ALERTS";

    private TMP_Text alertText;

    private void Awake()
    {
        alertText = GetComponent<TMP_Text>();
        UpdateAlert();
    }

    public void SetAlert(string message)
    {
        currentAlert = string.IsNullOrWhiteSpace(message)
            ? "NO ACTIVE ALERTS"
            : message;

        UpdateAlert();
    }

    private void UpdateAlert()
    {
        if (alertText != null)
            alertText.text = currentAlert;
    }
}