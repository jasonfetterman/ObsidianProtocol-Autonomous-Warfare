using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public enum GarageStoreItemType
    {
        Unit,
        Equipment,
        Cosmetic,
        Upgrade,
        Slot,
        Convenience,
        Material
    }

    [CreateAssetMenu(
        fileName = "GarageStoreItem",
        menuName = "Obsidian Protocol/Garage/Store Item")]
    public class GarageStoreItem : ScriptableObject
    {
        public string itemID;
        public string displayName;

        [TextArea]
        public string description;

        public GarageStoreItemType purchaseType;

        public int creditCost;

        public bool available = true;
        public bool requiresUnlock;

        public bool subjectToDeploymentBudget = true;
    }
}
