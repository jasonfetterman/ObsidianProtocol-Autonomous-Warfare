using UnityEngine;

namespace ObsidianProtocol.World
{
    public class WorldPersistenceController : MonoBehaviour
    {
        [Header("World Systems")]
        [SerializeField]
        private PersistentWorldManager worldManager;

        [SerializeField]
        private WorldUnitRegistry unitRegistry;

        [Header("Persistence")]
        [SerializeField]
        private bool persistAcrossScenes = true;

        [SerializeField]
        private bool saveOfflineState = true;

        [SerializeField]
        private bool saveOnlineState = true;

        private void Awake()
        {
            if (persistAcrossScenes)
                DontDestroyOnLoad(gameObject);
        }

        public PersistentWorldState CaptureState()
        {
            if (worldManager == null ||
                worldManager.State == null)
            {
                Debug.LogWarning(
                    "WorldPersistenceController: World state unavailable.");

                return null;
            }

            return worldManager.State;
        }

        public bool CanSave()
        {
            if (worldManager == null ||
                worldManager.State == null)
                return false;

            if (worldManager.State.offline)
                return saveOfflineState;

            if (worldManager.State.online)
                return saveOnlineState;

            return true;
        }

        public void SynchronizeRegistry()
        {
            if (worldManager == null ||
                worldManager.State == null ||
                unitRegistry == null)
                return;

            unitRegistry.Clear();

            foreach (
                PersistentWorldUnit unit
                in worldManager.State.units)
            {
                if (unit == null)
                    continue;

                unitRegistry.Register(unit);
            }
        }

        public void UpdateUnitTransform(
            string unitInstanceId,
            Vector3 position,
            Vector3 rotation)
        {
            if (worldManager == null)
                return;

            PersistentWorldUnit unit =
                worldManager.GetUnit(
                    unitInstanceId);

            if (unit == null)
                return;

            unit.position = position;
            unit.rotation = rotation;

            if (unitRegistry != null)
            {
                unitRegistry.UpdateTransform(
                    unitInstanceId,
                    position,
                    rotation);
            }
        }

        public void MarkUnitDestroyed(
            string unitInstanceId)
        {
            if (worldManager == null)
                return;

            PersistentWorldUnit unit =
                worldManager.GetUnit(
                    unitInstanceId);

            if (unit == null)
                return;

            unit.destroyed = true;
            unit.active = false;

            if (unitRegistry != null)
            {
                unitRegistry.SetDestroyed(
                    unitInstanceId,
                    true);
            }
        }

        public void MarkUnitActive(
            string unitInstanceId)
        {
            if (worldManager == null)
                return;

            PersistentWorldUnit unit =
                worldManager.GetUnit(
                    unitInstanceId);

            if (unit == null)
                return;

            unit.destroyed = false;
            unit.active = true;

            if (unitRegistry != null)
            {
                unitRegistry.SetActive(
                    unitInstanceId,
                    true);
            }
        }
    }
}
