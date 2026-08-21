using UnityEngine;

namespace Obsidian.VR
{
    [System.Serializable]
    public class CombatStyle
    {
        public float lowHealthForce = 0.5f;
        public float sprintForce = 0.25f;
        public float stressForce = 0.35f;

        public static CombatStyle Lerp(CombatStyle a, CombatStyle b, float t)
        {
            if (a == null && b == null)
                return new CombatStyle();

            if (a == null)
                return b;

            if (b == null)
                return a;

            t = Mathf.Clamp01(t);

            return new CombatStyle
            {
                lowHealthForce = Mathf.Lerp(a.lowHealthForce, b.lowHealthForce, t),
                sprintForce = Mathf.Lerp(a.sprintForce, b.sprintForce, t),
                stressForce = Mathf.Lerp(a.stressForce, b.stressForce, t)
            };
        }
    }
}