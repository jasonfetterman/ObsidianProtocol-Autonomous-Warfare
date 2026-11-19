using UnityEngine;

public class UnitSizeReporter : MonoBehaviour
{
    [SerializeField] private Vector3 measuredDimensions;

    public Vector3 MeasuredDimensions => measuredDimensions;

    [ContextMenu("Measure Unit Size")]
    public void MeasureUnitSize()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>(true);

        if (renderers.Length == 0)
        {
            Debug.LogWarning("[UNIT SIZE] No renderers found on " + name, this);
            return;
        }

        Vector3 min = Vector3.positiveInfinity;
        Vector3 max = Vector3.negativeInfinity;
        int measuredCount = 0;

        foreach (Renderer item in renderers)
        {
            Bounds local = item.localBounds;

            if (local.size == Vector3.zero)
                continue;

            Vector3 center = local.center;
            Vector3 extents = local.extents;

            for (int x = -1; x <= 1; x += 2)
            for (int y = -1; y <= 1; y += 2)
            for (int z = -1; z <= 1; z += 2)
            {
                Vector3 corner = center + Vector3.Scale(
                    extents, new Vector3(x, y, z));

                Vector3 worldCorner = item.transform.TransformPoint(corner);
                min = Vector3.Min(min, worldCorner);
                max = Vector3.Max(max, worldCorner);
            }

            measuredCount++;
        }

        if (measuredCount == 0)
        {
            Debug.LogWarning(
                "[UNIT SIZE] Renderers were found, but none had usable local bounds on " + name,
                this);
            return;
        }

        measuredDimensions = max - min;

        Debug.Log(
            "[UNIT SIZE] " + name +
            "\nWidth: " + measuredDimensions.x.ToString("F2") + " m" +
            "\nHeight: " + measuredDimensions.y.ToString("F2") + " m" +
            "\nDepth: " + measuredDimensions.z.ToString("F2") + " m" +
            "\nRenderers measured: " + measuredCount,
            this);
    }
}
