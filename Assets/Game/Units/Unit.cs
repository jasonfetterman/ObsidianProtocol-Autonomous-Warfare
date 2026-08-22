using UnityEngine;

namespace ObsidianProtocol.Game.Units
{
    public abstract class Unit : MonoBehaviour
    {
        [SerializeField] private ObsidianProtocol.Game.Core.EntityId entityId;

        public ObsidianProtocol.Game.Core.EntityId EntityId => entityId;

        protected virtual void Awake()
        {
            if (entityId == null)
            {
                entityId = GetComponent<ObsidianProtocol.Game.Core.EntityId>();
            }
        }
    }
}
