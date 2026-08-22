using System.Collections.Generic;

namespace ObsidianProtocol.Game.Core
{
    public static class EntityRegistry
    {
        private static readonly Dictionary<string, EntityId> Entities = new();

        public static void Register(EntityId entity)
        {
            if (entity == null || string.IsNullOrEmpty(entity.Id))
            {
                return;
            }

            Entities[entity.Id] = entity;
        }

        public static void Unregister(EntityId entity)
        {
            if (entity == null || string.IsNullOrEmpty(entity.Id))
            {
                return;
            }

            Entities.Remove(entity.Id);
        }

        public static bool TryGet(string id, out EntityId entity)
        {
            return Entities.TryGetValue(id, out entity);
        }

        public static void Clear()
        {
            Entities.Clear();
        }
    }
}
