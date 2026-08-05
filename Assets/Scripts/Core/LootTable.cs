using UnityEngine;

public class LootTable : MonoBehaviour
{
    public Item[] possibleLoot;
    public float dropChance = 0.5f; // 50%

    public Item GetLoot()
    {
        if (possibleLoot.Length == 0)
            return null;

        if (Random.value > dropChance)
            return null;

        int index = Random.Range(0, possibleLoot.Length);
        return possibleLoot[index];
    }
}
