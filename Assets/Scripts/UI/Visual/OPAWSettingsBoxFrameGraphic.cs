using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public sealed class OPAWSettingsBoxFrameGraphic : MaskableGraphic
{
    [Min(0.5f)] public float thickness = 1.35f;
    [Min(2f)] public float cornerCut = 12f;

    protected override void Awake()
    {
        base.Awake();
        raycastTarget = false;
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        Rect r = GetPixelAdjustedRect();
        float t = Mathf.Clamp(thickness, 0.5f, Mathf.Min(r.width, r.height) * 0.08f);
        float c = Mathf.Clamp(cornerCut, 2f, Mathf.Min(r.width, r.height) * 0.2f);

        Rect inner = Rect.MinMaxRect(
            r.xMin + t, r.yMin + t,
            r.xMax - t, r.yMax - t
        );

        if (inner.width <= 0 || inner.height <= 0)
            return;

        Vector2[] outerPoints = Octagon(r, c);
        Vector2[] innerPoints = Octagon(inner, Mathf.Max(1f, c - t));

        for (int i = 0; i < 8; i++)
        {
            AddVertex(vh, outerPoints[i]);
            AddVertex(vh, innerPoints[i]);
        }

        for (int i = 0; i < 8; i++)
        {
            int next = (i + 1) % 8;
            int outerA = i * 2;
            int innerA = outerA + 1;
            int outerB = next * 2;
            int innerB = outerB + 1;

            vh.AddTriangle(outerA, outerB, innerB);
            vh.AddTriangle(outerA, innerB, innerA);
        }
    }

    private void AddVertex(VertexHelper vh, Vector2 position)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.position = position;
        vertex.color = color;
        vh.AddVert(vertex);
    }

    private static Vector2[] Octagon(Rect r, float c)
    {
        return new[]
        {
            new Vector2(r.xMin + c, r.yMin),
            new Vector2(r.xMax - c, r.yMin),
            new Vector2(r.xMax, r.yMin + c),
            new Vector2(r.xMax, r.yMax - c),
            new Vector2(r.xMax - c, r.yMax),
            new Vector2(r.xMin + c, r.yMax),
            new Vector2(r.xMin, r.yMax - c),
            new Vector2(r.xMin, r.yMin + c)
        };
    }
}
