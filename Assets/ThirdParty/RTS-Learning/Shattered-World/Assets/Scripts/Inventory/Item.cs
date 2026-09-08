using UnityEngine;
public class Item : MonoBehaviour
{
    public Sprite icon = null;
    public string item_name = null;
    private void Start()
    {
        item_name = this.name;
    }
}
