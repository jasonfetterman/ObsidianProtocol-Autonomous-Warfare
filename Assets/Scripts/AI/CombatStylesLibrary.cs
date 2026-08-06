using UnityEngine;

namespace Obsidian.VR
{
    public static class CombatStylesLibrary
    {
        public static CombatStyle Tactical => new CombatStyle
        {
            gunfireForce = 0.25f,
            hitForce = 0.5f,
            killForce = 0.8f,
            explosionForce = 1.0f,
            suppressionForce = 0.4f,

            lowHealthForce = 0.3f,
            sprintForce = 0.2f,
            stressForce = 0.25f,

            whipPanIntensity = 20f,
            snapZoomAmount = 6f,
            shakeIntensity = 0.08f
        };

        public static CombatStyle Heroic => new CombatStyle
        {
            gunfireForce = 0.6f,
            hitForce = 1.0f,
            killForce = 1.6f,
            explosionForce = 2.0f,
            suppressionForce = 0.8f,

            lowHealthForce = 0.7f,
            sprintForce = 0.5f,
            stressForce = 0.6f,

            whipPanIntensity = 55f,
            snapZoomAmount = 18f,
            shakeIntensity = 0.22f
        };

        public static CombatStyle Documentary => new CombatStyle
        {
            gunfireForce = 0.15f,
            hitForce = 0.3f,
            killForce = 0.4f,
            explosionForce = 0.6f,
            suppressionForce = 0.2f,

            lowHealthForce = 0.1f,
            sprintForce = 0.05f,
            stressForce = 0.1f,

            whipPanIntensity = 8f,
            snapZoomAmount = 3f,
            shakeIntensity = 0.04f
        };

        public static CombatStyle Anime => new CombatStyle
        {
            gunfireForce = 0.8f,
            hitForce = 1.4f,
            killForce = 2.2f,
            explosionForce = 3.0f,
            suppressionForce = 1.0f,

            lowHealthForce = 1.2f,
            sprintForce = 0.9f,
            stressForce = 1.0f,

            whipPanIntensity = 95f,
            snapZoomAmount = 28f,
            shakeIntensity = 0.35f
        };
    }
}
