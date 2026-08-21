using UnityEngine;

namespace ObsidianProtocol.World
{
    public class PersistentWorldManager : MonoBehaviour
    {
        [Header("World")]
        [SerializeField]
        private string worldId = "WORLD_01";

        [SerializeField]
        private string worldName = "Obsidian World";

        [Header("Runtime State")]
        [SerializeField]
        private PersistentWorldState worldState;

        public PersistentWorldState State =>
            worldState;

        public string WorldId =>
            worldId;

        public bool IsInitialized =>
            worldState != null &&
            worldState.initialized;

        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (worldState == null)
            {
                worldState =
                    new PersistentWorldState();
            }

            worldState.Initialize(
                worldId,
                worldName);

            Debug.Log(
                $"WORLD INITIALIZED: {worldId}");
        }

        public void SetOnline()
        {
            EnsureInitialized();

            worldState.SetOnline(true);

            Debug.Log(
                $"WORLD MODE: ONLINE — {worldId}");
        }

        public void SetOffline()
        {
            EnsureInitialized();

            worldState.SetOffline(true);

            Debug.Log(
                $"WORLD MODE: OFFLINE — {worldId}");
        }

        public PersistentWorldUnit GetUnit(
            string unitInstanceId)
        {
            EnsureInitialized();

            return worldState.FindUnit(
                unitInstanceId);
        }

        public void RegisterUnit(
            string unitInstanceId,
            string unitDefinitionId,
            Vector3 position,
            Vector3 rotation)
        {
            EnsureInitialized();

            if (string.IsNullOrWhiteSpace(
                    unitInstanceId))
                return;

            if (worldState.FindUnit(
                    unitInstanceId) != null)
                return;

            PersistentWorldUnit unit =
                new PersistentWorldUnit
                {
                    unitInstanceId =
                        unitInstanceId,

                    unitDefinitionId =
                        unitDefinitionId,

                    position =
                        position,

                    rotation =
                        rotation,

                    active = true,
                    destroyed = false
                };

            worldState.AddUnit(unit);

            Debug.Log(
                $"WORLD UNIT REGISTERED: {unitInstanceId}");
        }

        public void RemoveUnit(
            string unitInstanceId)
        {
            EnsureInitialized();

            worldState.RemoveUnit(
                unitInstanceId);
        }

        public void ClearWorldUnits()
        {
            EnsureInitialized();

            worldState.ClearUnits();
        }

        private void EnsureInitialized()
        {
            if (!IsInitialized)
                Initialize();
        }
    }
}
