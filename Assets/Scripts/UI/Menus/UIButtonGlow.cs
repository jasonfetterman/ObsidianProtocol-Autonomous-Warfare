using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonGlow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Glow Image")]
    public Image glow;

    [Header("Glow Alphas")]
    [Range(0f, 1f)] public float hoverAlpha = 0.18f;   // Hover glow
    [Range(0f, 1f)] public float selectedAlpha = 0.18f; // Selected glow
    private float defaultAlpha = 0f;                    // Normal / Disabled / Pressed

    [Header("State Flags")]
    public bool isSelected = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected)
            SetGlowAlpha(hoverAlpha);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected)
            SetGlowAlpha(defaultAlpha);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetGlowAlpha(defaultAlpha); // Pressed = no glow
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isSelected)
            SetGlowAlpha(selectedAlpha);
        else
            SetGlowAlpha(hoverAlpha);
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;

        if (selected)
            SetGlowAlpha(selectedAlpha);
        else
            SetGlowAlpha(defaultAlpha);
    }

    private void SetGlowAlpha(float a)
    {
        if (glow == null) return;

        Color c = glow.color;
        c.a = a;
        glow.color = c;
    }
}
