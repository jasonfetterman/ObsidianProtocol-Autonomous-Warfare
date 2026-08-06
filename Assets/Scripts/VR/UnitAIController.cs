using UnityEngine;

namespace Obsidian.VR
{
    public class UnitAIController : MonoBehaviour
    {
        public bool IsAIEnabled { get; private set; } = true;

        public void EnableAI()
        {
            IsAIEnabled = true;
        }

        public void DisableAI()
        {
            IsAIEnabled = false;
        }

        public void OverrideMovement(Vector3 direction)
        {
            // AI movement override stub
        }

        public void OverrideLook(Vector3 target)
        {
            // AI look override stub
        }

        public void OverrideAction(string action)
        {
            // AI action override stub
        }
    }
}
