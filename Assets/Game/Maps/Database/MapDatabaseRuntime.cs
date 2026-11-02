using UnityEngine;

namespace ObsidianProtocol.Game.Maps
{
    public static class MapDatabaseRuntime
    {
        private static MapDatabase database;

        public static MapDatabase Database
        {
            get
            {
                if (database == null)
                    database = global::UnityEngine.Resources.Load<MapDatabase>("MapDatabase");

                return database;
            }
        }

        public static MapDefinition GetById(string mapId)
        {
            return Database == null ? null : Database.GetById(mapId);
        }
    }
}
