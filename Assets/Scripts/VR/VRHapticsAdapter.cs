using UnityEngine;

namespace Obsidian.VR
{
    public class VRHapticsAdapter : MonoBehaviour
    {
        public void Pulse(float strength)
        {
            // Placeholder haptics logic
        }

        public void Rumble(float intensity, float duration)
        {
            // Placeholder rumble logic
        }

        // ⭐ REQUIRED BY VRUnitHaptics
        public void Send(VRControllerHand hand, float intensity, float duration)
        {
            // Placeholder send logic
        }
    }
}
