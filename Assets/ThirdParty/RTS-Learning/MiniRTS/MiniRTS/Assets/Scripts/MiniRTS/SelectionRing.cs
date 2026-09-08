using UnityEngine;
using UnityEngine.Rendering;

namespace MiniRTS
{
    /// <summary>
    /// Shared, collider-free ring geometry for world-space selection feedback.
    /// </summary>
    public static class SelectionRing
    {
        private const int SegmentCount = 48;
        private static Mesh unitMesh;
        private static Mesh buildingMesh;
        private static Material ringMaterial;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetForPlaySession()
        {
            unitMesh = null;
            buildingMesh = null;
            ringMaterial = null;
        }

        public static GameObject Create(
            Transform parent,
            Vector3 localPosition,
            bool buildingSized)
        {
            GameObject ring = new GameObject(
                "SelectionRing",
                typeof(MeshFilter),
                typeof(MeshRenderer));
            ring.transform.SetParent(parent, false);
            ring.transform.localPosition = localPosition;
            ring.GetComponent<MeshFilter>().sharedMesh =
                GetMesh(buildingSized);

            MeshRenderer renderer = ring.GetComponent<MeshRenderer>();
            renderer.sharedMaterial = GetMaterial();
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            ring.SetActive(false);
            return ring;
        }

        private static Mesh GetMesh(bool buildingSized)
        {
            Mesh mesh = buildingSized ? buildingMesh : unitMesh;
            if (mesh != null)
            {
                return mesh;
            }

            float outerRadius = buildingSized ? 0.56f : 0.72f;
            float innerRadius = buildingSized ? 0.515f : 0.59f;
            mesh = CreateMesh(
                buildingSized ? "BuildingSelectionRingMesh" : "UnitSelectionRingMesh",
                outerRadius,
                innerRadius);
            if (buildingSized)
            {
                buildingMesh = mesh;
            }
            else
            {
                unitMesh = mesh;
            }

            return mesh;
        }

        private static Mesh CreateMesh(
            string meshName,
            float outerRadius,
            float innerRadius)
        {
            Vector3[] vertices = new Vector3[SegmentCount * 2];
            Vector2[] uvs = new Vector2[vertices.Length];
            int[] triangles = new int[SegmentCount * 6];

            for (int i = 0; i < SegmentCount; i++)
            {
                float angle = i * Mathf.PI * 2f / SegmentCount;
                Vector3 direction = new Vector3(
                    Mathf.Cos(angle),
                    0f,
                    Mathf.Sin(angle));
                int vertex = i * 2;
                vertices[vertex] = direction * innerRadius;
                vertices[vertex + 1] = direction * outerRadius;
                uvs[vertex] = new Vector2(0f, i / (float)SegmentCount);
                uvs[vertex + 1] = new Vector2(1f, i / (float)SegmentCount);

                int nextVertex = ((i + 1) % SegmentCount) * 2;
                int triangle = i * 6;
                triangles[triangle] = vertex;
                triangles[triangle + 1] = nextVertex + 1;
                triangles[triangle + 2] = vertex + 1;
                triangles[triangle + 3] = vertex;
                triangles[triangle + 4] = nextVertex;
                triangles[triangle + 5] = nextVertex + 1;
            }

            Mesh mesh = new Mesh
            {
                name = meshName,
                hideFlags = HideFlags.DontSave,
                vertices = vertices,
                uv = uvs,
                triangles = triangles
            };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Material GetMaterial()
        {
            if (ringMaterial != null)
            {
                return ringMaterial;
            }

            Shader shader = Shader.Find("Universal Render Pipeline/Unlit");
            if (shader == null)
            {
                shader = Shader.Find("Unlit/Color");
            }

            if (shader == null)
            {
                shader = Shader.Find("Standard");
            }

            ringMaterial = new Material(shader)
            {
                name = "SelectionRingMaterial",
                hideFlags = HideFlags.DontSave,
                color = BalanceConfig.SelectionColor
            };
            if (ringMaterial.HasProperty("_BaseColor"))
            {
                ringMaterial.SetColor(
                    "_BaseColor",
                    BalanceConfig.SelectionColor);
            }

            return ringMaterial;
        }
    }
}
