using UnityEngine;
using System.Collections.Generic;

public class EnemyUnitManager : MonoBehaviour
{
    public List<GameObject> barracks = new();
    public List<GameObject> factories = new();

    public GameObject infantryPrefab;
    public GameObject tankPrefab;

    public float productionInterval = 5f;
    float nextProductionTime;

    public List<GameObject> units = new();

    void Update()
    {
        CleanupDestroyed();
    }

    void CleanupDestroyed()
    {
        barracks.RemoveAll(b => b == null);
        factories.RemoveAll(f => f == null);
        units.RemoveAll(u => u == null);
    }

    public void ProductionTick(EnemyResourceManager rm)
    {
        if (Time.time < nextProductionTime) return;
        nextProductionTime = Time.time + productionInterval;

        if (barracks.Count > 0)
            TryProduceInfantry(rm);

        if (factories.Count > 0)
            TryProduceTank(rm);
    }

    void TryProduceInfantry(EnemyResourceManager rm)
    {
        if (!rm.Spend(20, 0, 0)) return;

        GameObject b = barracks[Random.Range(0, barracks.Count)];
        Vector3 pos = b.transform.position + Vector3.forward * 2f;

        GameObject unit = Instantiate(infantryPrefab, pos, Quaternion.identity);
        units.Add(unit);
    }

    void TryProduceTank(EnemyResourceManager rm)
    {
        if (!rm.Spend(40, 20, 10)) return;

        GameObject f = factories[Random.Range(0, factories.Count)];
        Vector3 pos = f.transform.position + Vector3.forward * 3f;

        GameObject unit = Instantiate(tankPrefab, pos, Quaternion.identity);
        units.Add(unit);
    }
}
