using UnityEngine;

public class VehicleSelection : MonoBehaviour
{
    private bool isSelected;

    public bool IsSelected => isSelected;

    public void Select()
    {
        isSelected = true;

        Debug.Log("Vehicle Selected: " + gameObject.name);
    }

    public void Deselect()
    {
        isSelected = false;

        Debug.Log("Vehicle Deselected: " + gameObject.name);
    }
}