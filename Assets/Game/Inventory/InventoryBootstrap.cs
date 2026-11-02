using UnityEngine;

namespace ObsidianProtocol.Game.Inventory
{
    public sealed class InventoryBootstrap : MonoBehaviour
    {
        // Inventory initialization is campaign-driven.
        // CampaignState.CreateNewCampaign() calls:
        // InventoryRuntime.StartNewCampaign(CampaignId)
    }
}
