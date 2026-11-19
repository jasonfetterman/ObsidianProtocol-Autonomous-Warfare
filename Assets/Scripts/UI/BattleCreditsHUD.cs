using TMPro;
using UnityEngine;
using ObsidianProtocol.Game.Economy;

public class BattleCreditsHUD : MonoBehaviour
{
    [SerializeField] private string displayName = "CREDITS";

    private TMP_Text creditsText;

    private void Awake()
    {
        creditsText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (creditsText == null)
            return;

        creditsText.text = $"{displayName}\n{EconomyRuntime.Credits:N0}";
    }
}