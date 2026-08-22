using UnityEngine;

namespace ObsidianProtocol.Game.Combat.Wreckage
{
    public sealed class WreckageSystem : MonoBehaviour
    {
        [SerializeField] private GameObject wreckagePrefab;
        [SerializeField] private float lifetime = 300f;

        private bool created;

        public bool HasCreatedWreckage => created;

        public GameObject CreateWreckage()
        {
            if (created || wreckagePrefab == null)
            {
                return null;
            }

            GameObject wreckage =
                Instantiate(
                    wreckagePrefab,
                    transform.position,
                    transform.rotation);

            created = true;

            if (lifetime > 0f)
            {
                Destroy(wreckage, lifetime);
            }

            return wreckage;
        }

        public void ResetWreckageState()
        {
            created = false;
        }
    }
}
