using System.Collections.Generic;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    [System.Serializable]
    public class LootEntry
    {
        public Item item;
        [Min(0f)]
        public float weight = 1f;
    }

    [SerializeField] private List<LootEntry> entries = new();

    public Item GetLoot()
    {
        if (entries == null || entries.Count == 0)
            return null;

        float totalWeight = 0f;

        foreach (LootEntry entry in entries)
        {
            if (entry != null && entry.item != null && entry.weight > 0f)
                totalWeight += entry.weight;
        }

        if (totalWeight <= 0f)
            return null;

        float roll = Random.Range(0f, totalWeight);

        foreach (LootEntry entry in entries)
        {
            if (entry == null || entry.item == null || entry.weight <= 0f)
                continue;

            roll -= entry.weight;

            if (roll <= 0f)
                return entry.item;
        }

        return null;
    }
}