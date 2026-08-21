using UnityEngine;
using Obsidian.VR;   // ⭐ REQUIRED — fixes your error

namespace Obsidian.Cover
{
    public class UnitTerrainEffects : MonoBehaviour
    {
        [SerializeField] private UnitMover _mover;

        private void Awake()
        {
            if (_mover == null)
                _mover = GetComponent<UnitMover>();
        }

        private void Update()
        {
            if (_mover == null)
                return;

            ApplyTerrainEffects();
        }

        private void ApplyTerrainEffects()
        {
            // Placeholder terrain logic
            // Example: slow movement in mud, speed boost on road, etc.
        }
    }
}
