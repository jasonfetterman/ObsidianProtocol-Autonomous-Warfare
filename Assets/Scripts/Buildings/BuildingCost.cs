using UnityEngine;

[System.Serializable]
public class ResourceCost
{
    public ResourceType type;
    public int amount;
}

public class BuildingCost : MonoBehaviour
{
    public ResourceCost[] costs;

    public bool CanAfford(ResourceManager rm)
    {
        foreach (var c in costs)
            if (!rm.HasEnough(c.type, c.amount))
                return false;

        return true;
    }

    public void Pay(ResourceManager rm)
    {
        foreach (var c in costs)
            rm.Spend(c.type, c.amount);
    }
}

