using UnityEngine;

namespace ObsidianProtocol.Game.Core
{
    public sealed class CoreInstaller : MonoBehaviour
    {
        [SerializeField] private CoreRoot coreRoot;

        private void Awake()
        {
            if (coreRoot == null)
            {
                coreRoot = FindAnyObjectByType<CoreRoot>();
            }
        }
    }
}
