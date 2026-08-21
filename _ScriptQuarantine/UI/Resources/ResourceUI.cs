using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour
{
    private ResourceManager resourceManager;
    private SupplyManager supplyManager;

    [SerializeField] private Text woodText;
    [SerializeField] private Text stoneText;
    [SerializeField] private Text goldText;
    [SerializeField] private Text supplyText;

    private void Awake()
    {
        resourceManager = ServiceLocator.Get<ResourceManager>();
        supplyManager = ServiceLocator.Get<SupplyManager>();
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
