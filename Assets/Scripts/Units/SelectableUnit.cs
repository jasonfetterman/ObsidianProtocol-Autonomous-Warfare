using UnityEngine;

public class SelectableUnit : MonoBehaviour
{
    [SerializeField]
    private bool isSelected;

    public bool IsSelected => isSelected;

    public void Select()
    {
        isSelected = true;

        Debug.Log($"{gameObject.name} Selected");
    }

    public void Deselect()
    {
        isSelected = false;

        Debug.Log($"{gameObject.name} Deselected");
    }
}