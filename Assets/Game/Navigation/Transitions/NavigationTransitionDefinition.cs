using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Transitions
{
    [CreateAssetMenu(
        fileName = "NavigationTransitionDefinition",
        menuName = "Obsidian Protocol/Navigation/Navigation Transition Definition")]
    public sealed class NavigationTransitionDefinition : ScriptableObject
    {
        [SerializeField] private float transitionDuration = 1f;

        public float TransitionDuration =>
            Mathf.Max(0f, transitionDuration);
    }
}
