using UnityEngine;
using Assets.Scripts.Squad;   // REQUIRED — SquadTactics lives here

namespace Obsidian.VR
{
    public class SquadTacticsDriver : MonoBehaviour
    {
        private SquadTactics tactics;

        void Awake()
        {
            tactics = ServiceLocator.Get<SquadTactics>();
        }

        void Update()
        {
            tactics?.Tick();
        }
    }
}
