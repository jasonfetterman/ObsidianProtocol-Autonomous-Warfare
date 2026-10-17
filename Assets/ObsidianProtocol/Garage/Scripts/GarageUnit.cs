using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public enum GarageUnitCategory
    {
        Air,
        Ground,
        Naval,
        Command,
        Experimental
    }

    [CreateAssetMenu(
        fileName = "GarageUnit",
        menuName = "Obsidian Protocol/Garage/Unit")]
    public class GarageUnit : ScriptableObject
    {
        public string unitID;
        public string displayName;

        [TextArea]
        public string description;

        public GarageUnitCategory category;

        public Vector3 dimensions;
        public float mass;
        public float speed;
        public float maxAngle;

        public int deploymentCost;

        public bool unlocked;
        public bool owned;

        public GameObject unitPrefab;
    }
}
