using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private SupplyManager supplyManager;

    [SerializeField] private Text woodText;
    [SerializeField] private Text stoneText;
    [SerializeField] private Text goldText;
    [SerializeField] private Text supplyText;

    private void Awake()
    {
        if (resourceManager == null)
            resourceManager = Object.FindAnyObjectByType<ResourceManager>();

        if (supplyManager == null)
            supplyManager = Object.FindAnyObjectByType<SupplyManager>();
    }

    private void Update()
    {
        if (resourceManager == null || supplyManager == null)
            return;

        woodText.text = resourceManager.Get(ResourceType.Wood).ToString();
        stoneText.text = resourceManager.Get(ResourceType.Stone).ToString();
        goldText.text = resourceManager.Get(ResourceType.Gold).ToString();

        supplyText.text = $"{supplyManager.supplyUsed}/{supplyManager.supplyMax}";
    }
}
