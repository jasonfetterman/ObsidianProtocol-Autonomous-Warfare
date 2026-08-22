using UnityEngine;

namespace ObsidianProtocol.Game.World.Time
{
    [CreateAssetMenu(
        fileName = "DayNightDefinition",
        menuName = "Obsidian Protocol/World/Day Night Definition")]
    public sealed class DayNightDefinition : ScriptableObject
    {
        [SerializeField] private float dayLengthSeconds = 1200f;
        [SerializeField] private float startingTimeOfDay = 12f;

        public float DayLengthSeconds =>
            Mathf.Max(1f, dayLengthSeconds);

        public float StartingTimeOfDay =>
            Mathf.Repeat(startingTimeOfDay, 24f);
    }
}
