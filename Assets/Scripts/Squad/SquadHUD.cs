using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.Scripts.UI;     // SelectionManager
using Assets.Scripts.Squad;  // SquadAI

namespace Assets.Scripts.Squad
{
    public class SquadHUD : MonoBehaviour
    {
        public Text squadNameText;
        public Text countText;

        private SelectionManager selectionManager;
        private SquadAI squad;

        void Awake()
        {
            selectionManager = Object.FindAnyObjectByType<SelectionManager>();
            squad = ServiceLocator.Get<SquadAI>();
        }

        void Update()
        {
            var selected = selectionManager.GetSelected();

            if (selected == null || selected.Count == 0)
            {
                squadNameText.text = "";
                countText.text = "";
                return;
            }

            squadNameText.text = "Squad";
            countText.text = "Units: " + selected.Count;
        }
    }
}
