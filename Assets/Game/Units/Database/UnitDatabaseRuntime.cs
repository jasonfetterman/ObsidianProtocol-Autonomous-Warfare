using UnityEngine;
using ObsidianProtocol.Game.Units;

namespace ObsidianProtocol.Game.Runtime
{
    public static class UnitDatabaseRuntime
    {
        public static UnitDatabase Database { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeOnLoad()
        {
            EnsureInitialized();
        }

        public static bool EnsureInitialized()
        {
            if (Database != null)
                return true;

            Database = UnityEngine.Resources.Load<UnitDatabase>("UnitDatabase");

            if (Database == null)
            {
                Debug.LogError("UnitDatabase not found in Resources.");
                return false;
            }

            Database.RebuildLookup();
            return true;
        }

        public static bool Initialize(UnitDatabase database)
        {
            if (database == null)
                return false;

            Database = database;
            Database.RebuildLookup();
            return true;
        }

        public static UnitDefinition Get(string unitId)
        {
            return Database == null ? null : Database.GetById(unitId);
        }

        public static int Count =>
            Database == null ? 0 : Database.Units.Count;

        public static void Clear()
        {
            Database = null;
        }
    }
}

