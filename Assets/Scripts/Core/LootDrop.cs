using UnityEngine;

public class LootDrop : MonoBehaviour
{
    public GameObject lootPrefab;

    public void Drop(Item item)
    {
        if (item == null || lootPrefab == null)
            return;

        GameObject obj = Instantiate(lootPrefab, transform.position, Quaternion.identity);
        LootPickup pickup = obj.GetComponent<LootPickup>();
        pickup.item = item;
    }
}
