using UnityEngine;

namespace Obsidian.VR
{
    [System.Serializable]
    public class BattleStyle
    {
        public float povSwitchBias = 1f;          // How often POV switches
        public float heroFocusBias = 1f;          // How often hero units get POV
        public float threatBias = 1f;             // Weight of enemy proximity
        public float cohesionBias = 1f;           // Weight of squad cohesion
        public float explosionBias = 1f;          // Explosion cut-in intensity
        public float killBias = 1f;               // Kill cut-in intensity
        public float suppressionBias = 1f;        // Suppression cut-in intensity

        public float minSwitchInterval = 3f;
        public float maxSwitchInterval = 7f;

        public static BattleStyle Lerp(BattleStyle a, BattleStyle b, float t)
        {
            return new BattleStyle
            {
                povSwitchBias = Mathf.Lerp(a.povSwitchBias, b.povSwitchBias, t),
                heroFocusBias = Mathf.Lerp(a.heroFocusBias, b.heroFocusBias, t),
                threatBias = Mathf.Lerp(a.threatBias, b.threatBias, t),
                cohesionBias = Mathf.Lerp(a.cohesionBias, b.cohesionBias, t),
                explosionBias = Mathf.Lerp(a.explosionBias, b.explosionBias, t),
                killBias = Mathf.Lerp(a.killBias, b.killBias, t),
                suppressionBias = Mathf.Lerp(a.suppressionBias, b.suppressionBias, t),

                minSwitchInterval = Mathf.Lerp(a.minSwitchInterval, b.minSwitchInterval, t),
                maxSwitchInterval = Mathf.Lerp(a.maxSwitchInterval, b.maxSwitchInterval, t)
            };
        }
    }
}
