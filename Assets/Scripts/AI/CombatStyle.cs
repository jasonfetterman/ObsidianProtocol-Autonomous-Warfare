using UnityEngine;

namespace Obsidian.VR
{
    [System.Serializable]
    public class CombatStyle
    {
        public float gunfireForce = 0.4f;
        public float hitForce = 0.8f;
        public float killForce = 1.2f;
        public float explosionForce = 1.5f;
        public float suppressionForce = 0.6f;

        public float lowHealthForce = 0.5f;
        public float sprintForce = 0.3f;
        public float stressForce = 0.4f;

        public float whipPanIntensity = 45f;
        public float snapZoomAmount = 12f;
        public float shakeIntensity = 0.15f;

        public static CombatStyle Lerp(CombatStyle a, CombatStyle b, float t)
        {
            return new CombatStyle
            {
                gunfireForce = Mathf.Lerp(a.gunfireForce, b.gunfireForce, t),
                hitForce = Mathf.Lerp(a.hitForce, b.hitForce, t),
                killForce = Mathf.Lerp(a.killForce, b.killForce, t),
                explosionForce = Mathf.Lerp(a.explosionForce, b.explosionForce, t),
                suppressionForce = Mathf.Lerp(a.suppressionForce, b.suppressionForce, t),

                lowHealthForce = Mathf.Lerp(a.lowHealthForce, b.lowHealthForce, t),
                sprintForce = Mathf.Lerp(a.sprintForce, b.sprintForce, t),
                stressForce = Mathf.Lerp(a.stressForce, b.stressForce, t),

                whipPanIntensity = Mathf.Lerp(a.whipPanIntensity, b.whipPanIntensity, t),
                snapZoomAmount = Mathf.Lerp(a.snapZoomAmount, b.snapZoomAmount, t),
                shakeIntensity = Mathf.Lerp(a.shakeIntensity, b.shakeIntensity, t)
            };
        }
    }
}
