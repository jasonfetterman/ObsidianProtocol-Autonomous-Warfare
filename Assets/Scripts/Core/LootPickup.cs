using UnityEngine;

public class LootPickup : MonoBehaviour
{
    public Item item;

    void OnTriggerEnter(Collider other)
    {
        Inventory inv = other.GetComponent<Inventory>();
        if (inv != null)
        {
            inv.AddItem(item);
            Destroy(gameObject);
        }
    }
}
