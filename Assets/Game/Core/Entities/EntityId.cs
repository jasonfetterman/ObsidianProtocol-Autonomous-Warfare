using System;
using UnityEngine;

namespace ObsidianProtocol.Game.Core
{
    public sealed class EntityId : MonoBehaviour
    {
        [SerializeField] private string id;

        public string Id => id;

        private void Awake()
        {
            if (string.IsNullOrEmpty(id))
            {
                id = Guid.NewGuid().ToString("N");
            }

            EntityRegistry.Register(this);
        }

        private void OnDestroy()
        {
            EntityRegistry.Unregister(this);
        }
    }
}
