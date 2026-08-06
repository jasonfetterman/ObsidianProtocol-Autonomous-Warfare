using UnityEngine;

public class UnitSupply : MonoBehaviour
{
    public int supplyCost = 1;

    private SupplyManager sm;

    void Awake()
    {
        sm = ServiceLocator.Get<SupplyManager>();
        sm?.Add(supplyCost);
    }

    void OnDestroy()
    {
        sm?.Remove(supplyCost);
    }
}
