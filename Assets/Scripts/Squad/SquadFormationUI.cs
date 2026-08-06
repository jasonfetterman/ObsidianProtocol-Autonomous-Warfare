using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.AI;      // SquadFormationController
using Assets.Scripts.Squad;   // SquadAI

namespace Assets.Scripts.Squad
{
    public class SquadFormationUI : MonoBehaviour
    {
        public Button lineButton;
        public Button wedgeButton;
        public Button circleButton;
        public Button clearButton;

        private SquadFormationController formationController;
        private SquadAI squadAI;

        void Awake()
        {
            formationController = ServiceLocator.Get<SquadFormationController>();
            squadAI = ServiceLocator.Get<SquadAI>();

            if (lineButton != null)
                lineButton.onClick.AddListener(() => SetFormation(SquadAI.FormationType.Line));

            if (wedgeButton != null)
                wedgeButton.onClick.AddListener(() => SetFormation(SquadAI.FormationType.Wedge));

            if (circleButton != null)
                circleButton.onClick.AddListener(() => SetFormation(SquadAI.FormationType.Circle));

            if (clearButton != null)
                clearButton.onClick.AddListener(ClearFormation);
        }

        private void SetFormation(SquadAI.FormationType type)
        {
            if (squadAI != null)
                squadAI.SetFormation(type);
        }

        private void ClearFormation()
        {
            if (squadAI != null)
                squadAI.ClearFormation();
        }
    }
}
