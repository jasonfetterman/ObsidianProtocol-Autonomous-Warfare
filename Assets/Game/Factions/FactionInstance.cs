using UnityEngine;

namespace ObsidianProtocol.Game.Factions
{
    public sealed class FactionInstance : MonoBehaviour
    {
        [SerializeField] private FactionDefinition definition;

        public FactionDefinition Definition => definition;
        public string FactionId => definition != null ? definition.FactionId : string.Empty;
    }
}
