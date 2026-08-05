using UnityEngine;

public class Worker : MonoBehaviour
{
    public int harvestRate = 5;
    public float harvestInterval = 1f;

    float nextHarvestTime;

    ResourceNode currentNode;
    ResourceManager playerResources;

    void Awake()
    {
        playerResources = FindAnyObjectByType<ResourceManager>();
    }

    void Update()
    {
        if (currentNode == null) return;

        if (Time.time >= nextHarvestTime)
        {
            nextHarvestTime = Time.time + harvestInterval;

            int harvested = currentNode.Harvest(harvestRate);
            playerResources.Add(currentNode.type, harvested);
        }
    }

    public void SetTarget(ResourceNode node)
    {
        currentNode = node;
    }
}
