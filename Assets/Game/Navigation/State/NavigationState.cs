using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.State
{
    public sealed class NavigationState : MonoBehaviour
    {
        [SerializeField] private NavigationStateDefinition definition;

        public NavigationStateDefinition Definition => definition;

        public bool IsActive { get; private set; }

        private void Awake()
        {
            IsActive = definition != null;
        }

        public void SetActive(bool active)
        {
            IsActive = active;
        }
    }
}
