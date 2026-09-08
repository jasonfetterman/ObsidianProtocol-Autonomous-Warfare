using System.Collections.Generic;
using UnityEngine;
public class Inventory_Manager : MonoBehaviour
{
    public List<Item> items;
    [SerializeField] public GameObject Inventory_Panel;
    private void Start()
    {
        Inventory_Panel = Camera.main.GetComponent<Select>().Inventory_Canvas;
    }
    public void DisplayInventory()
    {
        //Debug.Log("Displaying "+this.name+" inventory");
        Canvas_Controller.AddImages(items);
        //this.DisplayItemNames(items);
    }
    private void DisplayItemNames(List<Item> list)
    {
        if (list.Count <= 0) { return; }

        for(int i=0;i<list.Count;i++)
        {
            Debug.Log(list[i].GetComponent<Item>().item_name);
        }
    }
}
