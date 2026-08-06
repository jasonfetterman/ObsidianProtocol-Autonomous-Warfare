using UnityEngine;
using Assets.Scripts.AI;   // CombatAI

namespace Assets.Scripts.AI
{
    public class CombatDriver : MonoBehaviour
    {
        private CombatAI combat;

        void Awake()
        {
            combat = ServiceLocator.Get<CombatAI>();
        }

        void Update()
        {
            combat?.Tick();
        }
    }
}
