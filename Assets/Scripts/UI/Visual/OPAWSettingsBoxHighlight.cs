using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public sealed class OPAWSettingsBoxHighlight : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IPointerUpHandler,
    ISelectHandler, IDeselectHandler
{
    public OPAWSettingsBoxFrameGraphic frame;
    public Image background;

    private Color normalFrame;
    private Color hoverFrame;
    private Color pressedFrame;
    private bool pointerInside;
    private bool selected;

    public void Bind(OPAWSettingsBoxFrameGraphic boxFrame, Image boxBackground)
    {
        frame = boxFrame;
        background = boxBackground;

        normalFrame = new Color(0.20f, 0.70f, 0.85f, 0.72f);
        hoverFrame = new Color(0.38f, 0.88f, 1f, 1f);
        pressedFrame = new Color(0.68f, 0.95f, 1f, 1f);

        ApplyState();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerInside = true;
        ApplyState();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerInside = false;
        ApplyState();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (frame != null)
            frame.color = pressedFrame;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ApplyState();
    }

    public void OnSelect(BaseEventData eventData)
    {
        selected = true;
        ApplyState();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selected = false;
        ApplyState();
    }

    private void OnDisable()
    {
        pointerInside = false;
        selected = false;

        if (frame != null)
            frame.color = normalFrame;
    }

    private void ApplyState()
    {
        if (frame == null)
            return;

        frame.color = pointerInside || selected
            ? hoverFrame
            : normalFrame;
    }
}
