using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuCampaign : MonoBehaviour
{
    public static UIMainMenuCampaign Instance { get; private set; }

    [SerializeField] private Button campaignButton;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (campaignButton == null)
            campaignButton = GetComponent<Button>();
    }

    public void Campaign()
    {
        Debug.Log("Campaign selected.");
    }

    public void SetInteractable(bool interactable)
    {
        if (campaignButton != null)
            campaignButton.interactable = interactable;
    }
}
