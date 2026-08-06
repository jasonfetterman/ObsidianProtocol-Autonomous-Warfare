using Assets.Scripts.Squad;   // SquadAI, SquadMember
using Assets.Scripts.UI;      // SelectionBox
using Obsidian.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class SelectionManager : MonoBehaviour
    {
        public SelectionBox selectionBox;
        public LayerMask unitMask;

        private Camera cam;
        private List<SquadMember> selected = new();

        void Awake()
        {
            cam = Camera.main;
        }

        void Update()
        {
            HandleClickSelect();
            HandleDragSelect();
        }

        private void HandleClickSelect()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 999f, unitMask))
                {
                    ClearSelection();
                    TrySelect(hit.collider.GetComponent<SquadMember>());
                }
            }
        }

        private void HandleDragSelect()
        {
            if (Input.GetMouseButtonDown(0))
            {
                selectionBox.Begin(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                selectionBox.UpdateDrag(Input.mousePosition);
            }

            if (Input.GetMouseButtonUp(0))
            {
                Rect rect = selectionBox.GetSelectionRect();
                selectionBox.End();

                ClearSelection();
                SelectUnitsInRect(rect);
            }
        }

        private void SelectUnitsInRect(Rect rect)
        {
            SquadAI squad = ServiceLocator.Get<SquadAI>();
            if (squad == null)
                return;

            foreach (var member in squad.members)
            {
                Vector3 screenPos = cam.WorldToScreenPoint(member.transform.position);

                if (rect.Contains(screenPos))
                    selected.Add(member);
            }
        }

        private void TrySelect(SquadMember member)
        {
            if (member != null)
                selected.Add(member);
        }

        private void ClearSelection()
        {
            selected.Clear();
        }

        public IReadOnlyList<SquadMember> GetSelected()
        {
            return selected;
        }
    }
}
