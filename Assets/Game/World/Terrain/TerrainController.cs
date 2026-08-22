using UnityEngine;

namespace ObsidianProtocol.Game.World.Terrain
{
    public sealed class TerrainController : MonoBehaviour
    {
        [SerializeField] private TerrainDefinition definition;
        [SerializeField] private UnityEngine.Terrain terrain;

        public TerrainDefinition Definition => definition;
        public UnityEngine.Terrain Terrain => terrain;
        public bool IsReady => definition != null && terrain != null;

        private void Awake()
        {
            if (terrain == null)
            {
                terrain = GetComponent<UnityEngine.Terrain>();
            }
        }
    }
}
