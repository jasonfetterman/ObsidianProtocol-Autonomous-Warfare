using UnityEngine;

namespace ObsidianProtocol.Game.World.Time
{
    public sealed class DayNightSystem : MonoBehaviour
    {
        [SerializeField] private DayNightDefinition definition;

        private float timeOfDay;

        public DayNightDefinition Definition => definition;
        public float TimeOfDay => timeOfDay;

        public bool IsDaytime => timeOfDay >= 6f && timeOfDay < 18f;

        private void Awake()
        {
            if (definition != null)
            {
                timeOfDay = definition.StartingTimeOfDay;
            }
        }

        private void Update()
        {
            if (definition == null)
            {
                return;
            }

            timeOfDay +=
                (24f / definition.DayLengthSeconds) * UnityEngine.Time.deltaTime;

            timeOfDay = Mathf.Repeat(timeOfDay, 24f);
        }
    }
}
