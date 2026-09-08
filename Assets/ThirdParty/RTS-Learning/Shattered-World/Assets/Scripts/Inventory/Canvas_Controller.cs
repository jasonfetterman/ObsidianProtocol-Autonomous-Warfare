using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_Controller : MonoBehaviour
{
    [SerializeField]private static GameObject Inventory_Panel;
    private static float next_object_length = 0f;
    private static readonly Vector2 half = new(0.5f, 0.5f);

    private void Awake()
    {
        Inventory_Panel = transform.GetChild(0).gameObject;
    }
    public static void AddImages(List<Item> list)
    {
        for (int i=0;i<list.Count;i++)
        {
            GameObject imgObject = new GameObject(""+i+"_icon");
            RectTransform trans = imgObject.AddComponent<RectTransform>();
            Image image = imgObject.AddComponent<Image>();
            imgObject.transform.SetParent(Inventory_Panel.transform);

            image.sprite = list[i].icon;
            trans.localScale = Vector3.one;
            trans.anchorMax = half;
            trans.anchorMin = half;
            trans.pivot = Vector2.zero;
            trans.position = new Vector3(next_object_length, 0f, 0f);
            next_object_length += trans.sizeDelta.x;
        }
    }
    [System.Obsolete]
    public static void DeleteAllChildren()
    {
        for(int i=0;i<Inventory_Panel.transform.GetChildCount(); i++)
        {
            Destroy(Inventory_Panel.transform.GetChild(i).gameObject);
        }
        next_object_length = 0f; 
    }
}
