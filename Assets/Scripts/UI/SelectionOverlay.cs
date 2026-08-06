using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.UI;     // SelectionManager
using Assets.Scripts.Squad;  // SquadMember

namespace Assets.Scripts.UI
{
    public class SelectionOverlay : MonoBehaviour
    {
        public Color outlineColor = Color.cyan;
        public float outlineWidth = 0.03f;

        private Camera cam;
        private SelectionManager selectionManager;

        void Awake()
        {
            cam = Camera.main;
            selectionManager = Object.FindAnyObjectByType<SelectionManager>();
        }

        void OnRenderObject()
        {
            if (selectionManager == null)
                return;

            var selected = selectionManager.GetSelected();
            if (selected == null || selected.Count == 0)
                return;

            Material mat = GetLineMaterial();
            mat.SetPass(0);

            GL.PushMatrix();
            GL.MultMatrix(Matrix4x4.identity);
            GL.Begin(GL.LINES);
            GL.Color(outlineColor);

            foreach (var member in selected)
            {
                DrawOutline(member.transform);
            }

            GL.End();
            GL.PopMatrix();
        }

        private void DrawOutline(Transform t)
        {
            Vector3 p = t.position;
            float s = outlineWidth;

            GL.Vertex3(p.x - s, p.y, p.z - s);
            GL.Vertex3(p.x + s, p.y, p.z - s);

            GL.Vertex3(p.x + s, p.y, p.z - s);
            GL.Vertex3(p.x + s, p.y, p.z + s);

            GL.Vertex3(p.x + s, p.y, p.z + s);
            GL.Vertex3(p.x - s, p.y, p.z + s);

            GL.Vertex3(p.x - s, p.y, p.z + s);
            GL.Vertex3(p.x - s, p.y, p.z - s);
        }

        private static Material lineMat;
        private Material GetLineMaterial()
        {
            if (lineMat != null)
                return lineMat;

            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMat = new Material(shader)
            {
                hideFlags = HideFlags.HideAndDontSave
            };
            lineMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            lineMat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            lineMat.SetInt("_ZWrite", 0);

            return lineMat;
        }
    }
}
