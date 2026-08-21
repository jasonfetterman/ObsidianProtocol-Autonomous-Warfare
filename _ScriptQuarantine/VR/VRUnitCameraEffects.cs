using UnityEngine;

namespace Obsidian.VR
{
    public class VRUnitCameraEffects : MonoBehaviour
    {
        private Camera _cam;

        private void Awake()
        {
            _cam = GetComponentInChildren<Camera>();
        }

        // ⭐ REQUIRED — fixes your error
        public void AddShake(float intensity, float duration)
        {
            // Placeholder shake logic
            // You can replace this with your real camera shake system
        }

        public void AddDamageFlash(float intensity)
        {
            // Placeholder damage flash logic
        }

        public void AddImpactPulse(float strength)
        {
            // Placeholder impact pulse logic
        }
    }
}
