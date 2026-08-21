using UnityEngine;

namespace ObsidianProtocol.World
{
    public class WorldEntityIdentity : MonoBehaviour
    {
        [Header("Persistent Identity")]
        [SerializeField]
        private string unitInstanceId;

        [SerializeField]
        private string unitDefinitionId;

        public string UnitInstanceId =>
            unitInstanceId;

        public string UnitDefinitionId =>
            unitDefinitionId;

        public void Initialize(
            string instanceId,
            string definitionId)
        {
            unitInstanceId = instanceId;
            unitDefinitionId = definitionId;

            gameObject.name =
                $"UNIT_{unitInstanceId}";
        }

        public bool Matches(
            string instanceId)
        {
            return unitInstanceId ==
                   instanceId;
        }

        public void ClearIdentity()
        {
            unitInstanceId = string.Empty;
            unitDefinitionId = string.Empty;
        }
    }
}
