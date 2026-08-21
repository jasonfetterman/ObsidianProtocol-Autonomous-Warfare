using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    public ResourceType type;
    public int amount = 100;

    public int Harvest(int rate)
    {
        int harvested = Mathf.Min(rate, amount);
        amount -= harvested;
        return harvested;
    }
}
