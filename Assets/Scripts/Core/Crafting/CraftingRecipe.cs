using UnityEngine;

[System.Serializable]
public class CraftingIngredient
{
    public Item item;
    public int amount;
}

[CreateAssetMenu(fileName = "NewRecipe", menuName = "RTS/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public string recipeName;
    public CraftingIngredient[] ingredients;
    public EquipmentItem result;
}
