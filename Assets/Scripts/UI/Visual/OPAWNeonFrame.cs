using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasRenderer))]
public class OPAWNeonFrame : MaskableGraphic,
    IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IPointerUpHandler
{
    public float thickness = 1.5f;
    public float cornerCut = 12f;
    public Color normalColor = new Color(.25f,.80f,.93f,.78f);
    public Color hoverColor = new Color(.42f,.91f,1f,1f);
    public Color pressedColor = new Color(.72f,.96f,1f,1f);

    bool hovered;
    bool pressed;

    protected override void Awake()
    {
        base.Awake();
        raycastTarget = false;
        UpdateColor();
    }

    public void OnPointerEnter(PointerEventData e)
    {
        hovered = true;
        UpdateColor();
    }

    public void OnPointerExit(PointerEventData e)
    {
        hovered = false;
        pressed = false;
        UpdateColor();
    }

    public void OnPointerDown(PointerEventData e)
    {
        pressed = true;
        UpdateColor();
    }

    public void OnPointerUp(PointerEventData e)
    {
        pressed = false;
        UpdateColor();
    }

    void UpdateColor()
    {
        color = pressed ? pressedColor :
                hovered ? hoverColor : normalColor;
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        Rect r = GetPixelAdjustedRect();
        float w = Mathf.Max(1f, thickness);
        float cut = Mathf.Clamp(cornerCut, 2f,
            Mathf.Min(r.width, r.height) * .20f);

        Vector2[] outer = {
            new Vector2(r.xMin + cut, r.yMin),
            new Vector2(r.xMax - cut, r.yMin),
            new Vector2(r.xMax, r.yMin + cut),
            new Vector2(r.xMax, r.yMax - cut),
            new Vector2(r.xMax - cut, r.yMax),
            new Vector2(r.xMin + cut, r.yMax),
            new Vector2(r.xMin, r.yMax - cut),
            new Vector2(r.xMin, r.yMin + cut)
        };

        float innerCut = Mathf.Max(1f, cut - w);
        Vector2[] inner = {
            new Vector2(r.xMin + w + innerCut, r.yMin + w),
            new Vector2(r.xMax - w - innerCut, r.yMin + w),
            new Vector2(r.xMax - w, r.yMin + w + innerCut),
            new Vector2(r.xMax - w, r.yMax - w - innerCut),
            new Vector2(r.xMax - w - innerCut, r.yMax - w),
            new Vector2(r.xMin + w + innerCut, r.yMax - w),
            new Vector2(r.xMin + w, r.yMax - w - innerCut),
            new Vector2(r.xMin + w, r.yMin + w + innerCut)
        };

        for (int i = 0; i < 8; i++)
        {
            int next = (i + 1) % 8;
            int start = vh.currentVertCount;

            UIVertex v = UIVertex.simpleVert;
            v.color = color;

            v.position = outer[i]; vh.AddVert(v);
            v.position = outer[next]; vh.AddVert(v);
            v.position = inner[next]; vh.AddVert(v);
            v.position = inner[i]; vh.AddVert(v);

            vh.AddTriangle(start, start + 1, start + 2);
            vh.AddTriangle(start, start + 2, start + 3);
        }
    }
}
