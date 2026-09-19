using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public enum GarageResourceType
    {
        Meat,
        Wood,
        Coal,
        Iron,
        Alloy,
        Electronics,
        Fuel,
        Energy
    }

    [CreateAssetMenu(
        fileName = "GarageResource",
        menuName = "Obsidian Protocol/Garage/Resource")]
    public class GarageResource : ScriptableObject
    {
        public GarageResourceType resourceType;
        public string displayName;
        [TextArea]
        public string description;

        public int currentAmount;
        public int storageCapacity = 1000;
    }
}
