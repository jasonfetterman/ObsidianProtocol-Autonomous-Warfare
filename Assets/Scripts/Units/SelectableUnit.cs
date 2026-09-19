using UnityEngine;

public class SelectableUnit : MonoBehaviour
{
    [SerializeField]
    private bool isSelected;

    [SerializeField]
    private Color normalColor = Color.white;

    [SerializeField]
    private Color hoverColor = Color.yellow;

    [SerializeField]
    private Color selectedColor = Color.green;

    private Renderer unitRenderer;
    private bool isHovered;

    public bool IsSelected => isSelected;

    private void Awake()
    {
        unitRenderer = GetComponent<Renderer>();
        UpdateColor();
    }

    public void Select()
    {
        isSelected = true;
        UpdateColor();
    }

    public void Deselect()
    {
        isSelected = false;
        UpdateColor();
    }

    public void SetHovered(bool hovered)
    {
        isHovered = hovered;
        UpdateColor();
    }

    private void UpdateColor()
    {
        if (unitRenderer == null)
        {
            return;
        }

        if (isSelected)
        {
            unitRenderer.material.color = selectedColor;
        }
        else if (isHovered)
        {
            unitRenderer.material.color = hoverColor;
        }
        else
        {
            unitRenderer.material.color = normalColor;
        }
    }
}