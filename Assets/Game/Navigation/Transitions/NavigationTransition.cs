using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Transitions
{
    public sealed class NavigationTransition : MonoBehaviour
    {
        [SerializeField] private NavigationTransitionDefinition definition;

        private float transitionStartTime;
        private bool isTransitioning;

        public NavigationTransitionDefinition Definition => definition;
        public bool IsTransitioning => isTransitioning;

        public void Begin()
        {
            transitionStartTime = UnityEngine.Time.time;
            isTransitioning = true;
        }

        private void Update()
        {
            if (!isTransitioning)
            {
                return;
            }

            float duration =
                definition != null
                    ? definition.TransitionDuration
                    : 1f;

            if (UnityEngine.Time.time >= transitionStartTime + duration)
            {
                isTransitioning = false;
            }
        }

        public void Cancel()
        {
            isTransitioning = false;
        }
    }
}
