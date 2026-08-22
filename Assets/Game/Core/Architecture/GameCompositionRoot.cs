using UnityEngine;

namespace ObsidianProtocol.Game.Core
{
    public sealed class GameCompositionRoot : MonoBehaviour
    {
        [SerializeField] private CoreRoot coreRoot;
        [SerializeField] private SystemRegistry systemRegistry;

        private void Awake()
        {
            if (coreRoot == null)
            {
                coreRoot = FindAnyObjectByType<CoreRoot>();
            }

            if (systemRegistry == null)
            {
                systemRegistry = FindAnyObjectByType<SystemRegistry>();
            }
        }
    }
}
