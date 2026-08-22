using UnityEngine;

namespace ObsidianProtocol.Game.World
{
    public sealed class WorldController : MonoBehaviour
    {
        [SerializeField] private WorldDefinition definition;

        public WorldDefinition Definition => definition;
        public bool IsInitialized { get; private set; }

        private void Awake()
        {
            IsInitialized = definition != null;
        }
    }
}
