using UnityEngine;

public class CraftingStation : MonoBehaviour
{
    public bool CanCraft(Inventory inv, CraftingRecipe recipe)
    {
        foreach (var ing in recipe.ingredients)
        {
            int count = 0;

            foreach (var i in inv.items)
                if (i.itemName == ing.item.itemName)
                    count++;

            if (count < ing.amount)
                return false;
        }

        return true;
    }

    public void Craft(Inventory inv, Equipment equip, CraftingRecipe recipe)
    {
        if (!CanCraft(inv, recipe))
            return;

        // remove ingredients
        foreach (var ing in recipe.ingredients)
        {
            int removed = 0;

            for (int i = inv.items.Count - 1; i >= 0; i--)
            {
                if (inv.items[i].itemName == ing.item.itemName)
                {
                    inv.items.RemoveAt(i);
                    removed++;
                    if (removed >= ing.amount)
                        break;
                }
            }
        }

        // equip crafted item
        if (equip != null && recipe.result != null)
        {
            equip.Equip(recipe.result, EquipSlot.Weapon);
        }
    }
}
