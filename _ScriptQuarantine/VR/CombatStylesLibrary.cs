using UnityEngine;

namespace Obsidian.VR
{
    public static class CombatStylesLibrary
    {
        public static readonly CombatStyle Documentary = new CombatStyle
        {
            lowHealthForce = 0.25f,
            sprintForce = 0.10f,
            stressForce = 0.20f
        };

        public static readonly CombatStyle Heroic = new CombatStyle
        {
            lowHealthForce = 0.75f,
            sprintForce = 0.40f,
            stressForce = 0.55f
        };
    }
}