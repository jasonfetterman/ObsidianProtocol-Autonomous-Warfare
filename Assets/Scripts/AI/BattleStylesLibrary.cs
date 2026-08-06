using UnityEngine;

namespace Obsidian.VR
{
    public static class BattleStylesLibrary
    {
        public static BattleStyle MilitarySim => new BattleStyle
        {
            povSwitchBias = 0.4f,
            heroFocusBias = 0.2f,
            threatBias = 1.0f,
            cohesionBias = 1.2f,
            explosionBias = 0.6f,
            killBias = 0.5f,
            suppressionBias = 0.8f,

            minSwitchInterval = 6f,
            maxSwitchInterval = 12f
        };

        public static BattleStyle HeroicBlockbuster => new BattleStyle
        {
            povSwitchBias = 1.2f,
            heroFocusBias = 2.0f,
            threatBias = 1.0f,
            cohesionBias = 0.8f,
            explosionBias = 2.0f,
            killBias = 1.8f,
            suppressionBias = 1.2f,

            minSwitchInterval = 3f,
            maxSwitchInterval = 6f
        };

        public static BattleStyle DocumentaryRealism => new BattleStyle
        {
            povSwitchBias = 0.3f,
            heroFocusBias = 0.1f,
            threatBias = 0.8f,
            cohesionBias = 1.0f,
            explosionBias = 0.3f,
            killBias = 0.2f,
            suppressionBias = 0.4f,

            minSwitchInterval = 8f,
            maxSwitchInterval = 14f
        };

        public static BattleStyle AnimeWarOpera => new BattleStyle
        {
            povSwitchBias = 1.8f,
            heroFocusBias = 3.0f,
            threatBias = 1.5f,
            cohesionBias = 0.5f,
            explosionBias = 3.0f,
            killBias = 2.5f,
            suppressionBias = 1.8f,

            minSwitchInterval = 2f,
            maxSwitchInterval = 4f
        };
    }
}
