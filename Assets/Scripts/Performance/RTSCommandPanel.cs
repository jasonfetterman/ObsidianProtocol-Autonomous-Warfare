using Assets.Scripts.AI;     // SquadController
using Assets.Scripts.UI;     // SelectionManager
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Performance
{
    public class RTSCommandPanel : MonoBehaviour
    {
        public Button moveButton;
        public Button attackButton;
        public Button stopButton;

        private SelectionManager selectionManager;
        private SquadController squadController;
        private Camera cam;

        void Awake()
        {
            selectionManager = Object.FindAnyObjectByType<SelectionManager>();
            squadController = ServiceLocator.Get<SquadController>();
            cam = Camera.main;

            moveButton.onClick.AddListener(OnMove);
            attackButton.onClick.AddListener(OnAttack);
            stopButton.onClick.AddListener(OnStop);
        }

        private void OnMove()
        {
            var selected = selectionManager.GetSelected();
            if (selected.Count == 0)
                return;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                squadController.IssueMoveCommand(selected, hit.point);
            }
        }

        private void OnAttack()
        {
            var selected = selectionManager.GetSelected();
            if (selected.Count == 0)
                return;

            squadController.IssueAttackCommand(selected);
        }

        private void OnStop()
        {
            var selected = selectionManager.GetSelected();
            if (selected.Count == 0)
                return;

            squadController.IssueStopCommand(selected);
        }
    }
}
