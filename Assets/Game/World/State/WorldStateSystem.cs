using UnityEngine;

namespace ObsidianProtocol.Game.World.State
{
    public sealed class WorldStateSystem : MonoBehaviour
    {
        [SerializeField] private WorldStateDefinition definition;

        public WorldStateDefinition Definition => definition;
        public bool IsInitialized { get; private set; }

        private void Awake()
        {
            IsInitialized = definition != null;
        }
    }
}
