using UnityEngine;

[RequireComponent(typeof(BuildingCost))]
public class BuildingSpawner : MonoBehaviour
{
    public GameObject unitPrefab;

    private ResourceManager rm;
    private BuildingCost cost;
    private SupplyManager sm;

    private void Awake()
    {
        rm = ServiceLocator.Get<ResourceManager>();
        sm = ServiceLocator.Get<SupplyManager>();

        cost = GetComponent<BuildingCost>();
    }

    public void SpawnUnit()
    {
        UnitSupply us = unitPrefab.GetComponent<UnitSupply>();

        // Supply check
        if (us != null && !sm.CanAdd(us.supplyCost))
            return;

        // Resource check
        if (cost != null && !cost.CanAfford(rm))
            return;

        // Pay cost
        if (cost != null)
            cost.Pay(rm);

        // Spawn unit
        Instantiate(
            unitPrefab,
            transform.position + Vector3.forward * 2f,
            Quaternion.identity
        );
    }
}
