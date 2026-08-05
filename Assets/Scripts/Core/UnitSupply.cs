using UnityEngine;

public class UnitSupply : MonoBehaviour
{
    public int supplyCost = 1;

    SupplyManager sm;

    void Awake()
    {
        sm = FindAnyObjectByType<SupplyManager>();

        if (sm != null)
            sm.Add(supplyCost);
    }

    void OnDestroy()
    {
        if (sm != null)
            sm.Remove(supplyCost);
    }
}
