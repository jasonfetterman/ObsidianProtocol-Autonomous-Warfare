
using TMPro;
using UnityEngine;
using ObsidianProtocol.Game.Economy;

public class BattleResourceHUD : MonoBehaviour
{
    [SerializeField] private string resourceId;
    [SerializeField] private string displayName;

    private TMP_Text resourceText;

    private void Awake()
    {
        resourceText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (resourceText == null)
            return;

        int amount = EconomyRuntime.GetResource(resourceId);

        resourceText.text = $"{displayName}\n{amount:N0}";
    }
}
