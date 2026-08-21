using UnityEngine;

public class LootDrop : MonoBehaviour
{
    [SerializeField] private Transform dropPoint;

    public void Drop(Item item)
    {
        if (item == null)
            return;

        if (item.worldPrefab == null)
            return;

        Transform spawnPoint = dropPoint != null ? dropPoint : transform;

        Instantiate(
            item.worldPrefab,
            spawnPoint.position,
            Quaternion.identity
        );
    }
}