using UnityEngine;

public class EnemyBuilder : MonoBehaviour
{
    public GameObject barracksPrefab;
    public GameObject factoryPrefab;

    public float buildSpacing = 10f;
    int barracksCount = 0;
    int factoryCount = 0;

    public void BuildTick(EnemyResourceManager rm)
    {
        if (barracksCount < 1)
        {
            TryBuildBarracks(rm);
        }
        else if (factoryCount < 1)
        {
            TryBuildFactory(rm);
        }
    }

    void TryBuildBarracks(EnemyResourceManager rm)
    {
        if (!rm.Spend(50, 20, 0)) return;

        Vector3 pos = transform.position + new Vector3(buildSpacing * barracksCount, 0, 0);
        Instantiate(barracksPrefab, pos, Quaternion.identity);
        barracksCount++;
    }

    void TryBuildFactory(EnemyResourceManager rm)
    {
        if (!rm.Spend(80, 40, 20)) return;

        Vector3 pos = transform.position + new Vector3(0, 0, buildSpacing * factoryCount);
        Instantiate(factoryPrefab, pos, Quaternion.identity);
        factoryCount++;
    }
}
