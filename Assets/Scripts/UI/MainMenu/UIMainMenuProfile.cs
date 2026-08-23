using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuProfile : MonoBehaviour
{
    public static UIMainMenuProfile Instance { get; private set; }

    [SerializeField] private Text profileText;

    public string PlayerName { get; private set; } = "Operator";

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

    public void SetPlayerName(string playerName)
    {
        if (string.IsNullOrWhiteSpace(playerName))
            return;

        PlayerName = playerName;
        Refresh();
    }

    private void Refresh()
    {
        if (profileText != null)
            profileText.text = PlayerName;
    }
}
