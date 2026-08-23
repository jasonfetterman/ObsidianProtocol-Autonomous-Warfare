using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuPlayerStatus : MonoBehaviour
{
    public static UIMainMenuPlayerStatus Instance { get; private set; }

    [SerializeField] private Text levelText;
    [SerializeField] private Text statusText;

    public int PlayerLevel { get; private set; } = 1;
    public string PlayerStatus { get; private set; } = "Operational";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Refresh();
    }

    public void SetLevel(int level)
    {
        PlayerLevel = Mathf.Max(1, level);
        Refresh();
    }

    public void SetStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return;

        PlayerStatus = status;
        Refresh();
    }

    private void Refresh()
    {
        if (levelText != null)
            levelText.text = $"LEVEL {PlayerLevel}";

        if (statusText != null)
            statusText.text = PlayerStatus;
    }
}
