using UnityEngine;

namespace ObsidianProtocol.Game.Core
{
    public sealed class CoreLifetime : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
