using UnityEngine;
using Obsidian.VR;   // ⭐ REQUIRED — fixes your error

namespace Obsidian.UI
{
    public class SelectionController : MonoBehaviour
    {
        [SerializeField] private UnitMover _mover;

        private void Awake()
        {
            if (_mover == null)
                _mover = FindAnyObjectByType<UnitMover>();
        }

        public void SelectUnit(UnitMover mover)
        {
            _mover = mover;
        }

        public void ClearSelection()
        {
            _mover = null;
        }
    }
}
