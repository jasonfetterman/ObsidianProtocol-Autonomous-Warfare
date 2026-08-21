using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.AI;   // SquadFormationController
using Assets.Scripts.UI;   // SelectionManager

namespace Assets.Scripts.Core
{
    public class FormationGhostPreview : MonoBehaviour
    {
        public Color color = new Color(0f, 0.6f, 1f, 0.35f);
        public float size = 0.35f;

        private Camera cam;
        private SquadFormationController formationController;
        private SelectionManager selectionManager;

        private bool active;
        private Vector3 targetPos;

        void Awake()
        {
            cam = Camera.main;
            formationController = ServiceLocator.Get<SquadFormationController>();
            selectionManager = Object.FindAnyObjectByType<SelectionManager>();
        }

        void Update()
        {
            if (!active)
                return;

            if (Input.GetMouseButtonUp(1))
                active = false;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
                targetPos = hit.point;
        }

        void OnRenderObject()
        {
            if (!active)
                return;

            var selected = selectionManager.GetSelected();
            if (selected == null || selected.Count == 0)
                return;

            Material mat = GetLineMaterial();
            mat.SetPass(0);

            GL.PushMatrix();
            GL.Begin(GL.LINES);
            GL.Color(color);

            List<Vector3> positions = new List<Vector3>(
                formationController.GetFormationPositions(targetPos, selected.Count)
            );

            foreach (var pos in positions)
                DrawGhost(pos);

            GL.End();
            GL.PopMatrix();
        }

        public void Show()
        {
            active = true;
        }

        private void DrawGhost(Vector3 worldPos)
        {
            float s = size;

            GL.Vertex(worldPos + new Vector3(-s, 0, -s));
            GL.Vertex(worldPos + new Vector3(s, 0, -s));

            GL.Vertex(worldPos + new Vector3(s, 0, -s));
            GL.Vertex(worldPos + new Vector3(s, 0, s));

            GL.Vertex(worldPos + new Vector3(s, 0, s));
            GL.Vertex(worldPos + new Vector3(-s, 0, s));

            GL.Vertex(worldPos + new Vector3(-s, 0, s));
            GL.Vertex(worldPos + new Vector3(-s, 0, -s));
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
