using UnityEngine;

namespace ObsidianProtocol.World
{
    public class WorldRuntimeSynchronizer : MonoBehaviour
    {
        [Header("Systems")]
        [SerializeField]
        private PersistentWorldManager worldManager;

        [SerializeField]
        private WorldEntityManager entityManager;

        [SerializeField]
        private WorldPersistenceController persistenceController;

        [Header("Settings")]
        [SerializeField]
        private bool synchronizeEveryFrame;

        private void Update()
        {
            if (!synchronizeEveryFrame)
                return;

            Synchronize();
        }

        public void Synchronize()
        {
            if (worldManager == null ||
                entityManager == null)
                return;

            if (worldManager.State == null)
                return;

            foreach (
                PersistentWorldUnit unit
                in worldManager.State.units)
            {
                if (unit == null ||
                    string.IsNullOrWhiteSpace(
                        unit.unitInstanceId))
                    continue;

                GameObject entity =
                    entityManager.FindEntity(
                        unit.unitInstanceId);

                if (entity == null)
                    continue;

                if (unit.destroyed)
                {
                    if (entity.activeSelf)
                        entity.SetActive(false);

                    continue;
                }

                if (!entity.activeSelf)
                    entity.SetActive(true);

                entity.transform.position =
                    unit.position;

                entity.transform.rotation =
                    Quaternion.Euler(
                        unit.rotation);
            }
        }

        public void CaptureRuntimeTransforms()
        {
            if (worldManager == null ||
                entityManager == null)
                return;

            foreach (
                PersistentWorldUnit unit
                in worldManager.State.units)
            {
                if (unit == null)
                    continue;

                GameObject entity =
                    entityManager.FindEntity(
                        unit.unitInstanceId);

                if (entity == null)
                    continue;

                unit.position =
                    entity.transform.position;

                unit.rotation =
                    entity.transform.eulerAngles;

                if (persistenceController != null)
                {
                    persistenceController
                        .UpdateUnitTransform(
                            unit.unitInstanceId,
                            entity.transform.position,
                            entity.transform.eulerAngles);
                }
            }
        }
    }
}
