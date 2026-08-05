using UnityEngine;

public class SupplyManager : MonoBehaviour
{
    public int supplyUsed = 0;
    public int supplyMax = 50;

    public bool CanAdd(int amount)
    {
        return supplyUsed + amount <= supplyMax;
    }

    public void Add(int amount)
    {
        supplyUsed += amount;
    }

    public void Remove(int amount)
    {
        supplyUsed -= amount;
        if (supplyUsed < 0) supplyUsed = 0;
    }
}
