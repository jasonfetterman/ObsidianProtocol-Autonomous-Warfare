using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemTest", menuName = "Shattered-World/ItemTest", order = 0)]
public class ItemTest : ScriptableObject
{
    [Header("Item's Descriptions")]
    public string itemName;
    public string itemDescription;
    public string itemType;
    [Header("Item's Checkers")]
    public bool itemIsEquippable;
    public bool itemIsConsumable;
    public bool itemIsStackable;
    public bool itemIsUnique;
    public bool itemIsQuestItem;
    public bool itemIsKeyItem;
    public bool itemIsUsable;
    public bool itemIsSellable;
    public bool itemIsBuyable;
    public bool itemIsTradeable;
    public bool itemIsDroppable;
    public bool itemIsDestroyable;
    public bool itemIsCraftable;
    public bool itemIsBreakable;
    public bool itemIsRepairable;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
