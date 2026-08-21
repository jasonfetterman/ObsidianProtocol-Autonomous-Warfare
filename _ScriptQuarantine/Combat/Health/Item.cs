using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Obsidian Protocol/Loot/Item")]
public class Item : ScriptableObject
{
    [Header("Item")]
    public string itemName;

    [TextArea]
    public string description;

    [Header("Loot")]
    public GameObject worldPrefab;
}